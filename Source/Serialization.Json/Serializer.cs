﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dolittle.Collections;
using Dolittle.DependencyInversion;
using Dolittle.Execution;
using Dolittle.Reflection;
using Dolittle.Serialization;
using Dolittle.Strings;
using Dolittle.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dolittle.Serialization.Json
{
    /// <summary>
    /// Represents a <see cref="ISerializer"/>
    /// </summary>
    [Singleton]
    public class Serializer : ISerializer
    {
        readonly IContainer _container;
        readonly ConcurrentDictionary<ISerializationOptions, JsonSerializer> _cacheAutoTypeName;
        readonly ConcurrentDictionary<ISerializationOptions, JsonSerializer> _cacheAutoTypeNameReadOnly;
        readonly ConcurrentDictionary<ISerializationOptions, JsonSerializer> _cacheNoneTypeName;
        readonly ConcurrentDictionary<ISerializationOptions, JsonSerializer> _cacheNoneTypeNameReadOnly;

        readonly IInstancesOf<ICanProvideConverters> _converterProviders;

        readonly List<JsonConverter> _converters = new List<JsonConverter>();

        /// <summary>
        /// Initializes a new instance of <see cref="Serializer"/>
        /// </summary>
        /// <param name="container">A <see cref="IContainer"/> used to create instances of types during serialization</param>
        /// <param name="converterProviders">Instances of <see cref="ICanProvideConverters"/></param>
        public Serializer(IContainer container, IInstancesOf<ICanProvideConverters> converterProviders)
        {
            _container = container;
            _converterProviders = converterProviders;
            _cacheAutoTypeName = new ConcurrentDictionary<ISerializationOptions, JsonSerializer>();
            _cacheNoneTypeName = new ConcurrentDictionary<ISerializationOptions, JsonSerializer>();
            _cacheAutoTypeNameReadOnly = new ConcurrentDictionary<ISerializationOptions, JsonSerializer>();
            _cacheNoneTypeNameReadOnly = new ConcurrentDictionary<ISerializationOptions, JsonSerializer>();

            _converters.Add(new ExceptionConverter());
            _converters.Add(new CamelCaseToPascalCaseExpandoObjectConverter());
            _converterProviders.ForEach(provider => provider.Provide().ForEach(c => 
            {
                if(c is IRequireSerializer)
                {
                    (c as IRequireSerializer).Add(this);
                }
                _converters.Add(c);
            }));
        }

        /// <inheritdoc/>
        public T FromJson<T>(string json, ISerializationOptions options = null)
        {
            return (T)FromJson(typeof(T), json, options);
        }

        /// <inheritdoc/>
        public object FromJson(Type type, string json, ISerializationOptions options = null)
        {
            var serializer = CreateSerializerForDeserialization(options);
            using (var textReader = new StringReader(json))
            {
                using (var reader = new JsonTextReader(textReader))
                {
                    object instance;

                    if (type.HasDefaultConstructor())
                    {
                        try
                        {
                            var value = serializer.Deserialize(reader, type);
                            if (value == null || value.GetType() != type)
                            {
                                var converter = serializer.Converters.SingleOrDefault(c => c.CanConvert(type) && c.CanRead);
                                if (converter != null) return converter.ReadJson(reader, type, null, serializer);
                            } else return value;
                        } catch {}

                    }

                    if (type.GetTypeInfo().IsValueType ||
                        type.HasInterface<IEnumerable>())
                        instance = serializer.Deserialize(reader, type);
                    else
                    {
                        IEnumerable<string> propertiesMatched;
                        instance = CreateInstanceOf(type, json, out propertiesMatched);

                        DeserializeRemaindingProperties(type, serializer, reader, instance, propertiesMatched);
                    }
                    return instance;
                }
            }
        }

        /// <inheritdoc/>
        public void FromJson(object instance, string json, ISerializationOptions options = null)
        {
            var serializer = CreateSerializerForDeserialization(options);
            using (var textReader = new StringReader(json))
            {
                using (var reader = new JsonTextReader(textReader))
                {
                    serializer.Populate(reader, instance);
                }
            }
        }

        /// <inheritdoc/>
        public string ToJson(object instance, ISerializationOptions options = null)
        {
            using (var stringWriter = new StringWriter())
            {
                var serializer = CreateSerializerForSerialization(options);
                serializer.Serialize(stringWriter, instance);
                var serialized = stringWriter.ToString();
                return serialized;
            }
        }

        /// <inheritdoc/>
        public Stream ToJsonStream(object instance, ISerializationOptions options = null)
        {
            var serialized = ToJson(instance, options);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(serialized);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        /// <inheritdoc/>
        public IDictionary<string, object> GetKeyValuesFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }



        object CreateInstanceOf(Type type, string json, out IEnumerable<string> propertiesMatched)
        {
            propertiesMatched = new string[0];
            if (type.HasDefaultConstructor())
                return Activator.CreateInstance(type);
            else
            {
                if (DoesPropertiesMatchConstructor(type, json))
                    return CreateInstanceByPropertiesMatchingConstructor(type, json, out propertiesMatched);
                else
                    return _container.Get(type);
            }
        }


        bool DoesPropertiesMatchConstructor(Type type, string json)
        {
            var hash = JObject.Load(new JsonTextReader(new StringReader(json)));
            var constructor = type.GetNonDefaultConstructor();
            var parameters = constructor.GetParameters();
            var properties = hash.Properties();
            var matchingParameters = parameters.Where(cp => properties.Select(p => p.Name.ToCamelCase()).Contains(cp.Name.ToCamelCase()));
            return matchingParameters.Count() == parameters.Length;
        }

        object CreateInstanceByPropertiesMatchingConstructor(Type type, string json, out IEnumerable<string> propertiesMatched)
        {
            var propertiesFound = new List<string>();
            var hash = JObject.Load(new JsonTextReader(new StringReader(json)));
            var properties = hash.Properties();

            var constructor = type.GetNonDefaultConstructor();

            var parameters = constructor.GetParameters();
            var parameterInstances = new List<object>();

            var toObjectMethod = typeof(JToken).GetTypeInfo().GetMethod("ToObject", new Type[] { typeof(JsonSerializer) });
            var serializer = CreateSerializerForDeserialization(SerializationOptions.CamelCase);

            foreach (var parameter in parameters)
            {
                var property = properties.Single(p => p.Name.ToCamelCase() == parameter.Name.ToCamelCase());
                propertiesFound.Add(property.Name);

                object parameterInstance = null;
                if (parameter.ParameterType == typeof(object))
                {
                    parameterInstance = serializer.Deserialize(new JsonTextReader(new StringReader(property.Value.ToString())), typeof(ExpandoObject));
                }
                else
                {
                    var genericToObjectMethod = toObjectMethod.MakeGenericMethod(parameter.ParameterType);
                    parameterInstance = genericToObjectMethod.Invoke(property.Value, new[] { serializer });
                }

                parameterInstances.Add(parameterInstance);
            }
            propertiesMatched = propertiesFound;
            var instance = constructor.Invoke(parameterInstances.ToArray());
            return instance;
        }


        void DeserializeRemaindingProperties(Type type, JsonSerializer serializer, JsonTextReader reader, object instance, IEnumerable<string> propertiesMatched)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = (string)reader.Value;

                    reader.Read();

                    if (!propertiesMatched.Contains(propertyName))
                    {
                        var typeInfo = type.GetTypeInfo();
                        var property = typeInfo.GetProperty(propertyName);
                        if (property == null) property = typeInfo.GetProperty(propertyName.ToPascalCase());
                        if (property != null && property.CanWrite)
                        {
                            var deserialized = serializer.Deserialize(reader, property.PropertyType);
                            property.SetValue(instance, deserialized);
                        }
                    }
                }
            }
        }


        JsonSerializer CreateSerializerForDeserialization(ISerializationOptions options = null)
        {
            return RetrieveSerializer(options ?? SerializationOptions.Default, true);
        }

        JsonSerializer CreateSerializerForSerialization(ISerializationOptions options = null)
        {
            return RetrieveSerializer(options ?? SerializationOptions.Default, false);
        }

        JsonSerializer RetrieveSerializer(ISerializationOptions options, bool ignoreReadOnlyProperties = false)
        {
            if (options.Flags.HasFlag(SerializationOptionsFlags.IncludeTypeNames))
            {
                if (ignoreReadOnlyProperties) return _cacheAutoTypeNameReadOnly.GetOrAdd(options, _ => CreateSerializer(options, TypeNameHandling.Auto, ignoreReadOnlyProperties));
                return _cacheAutoTypeName.GetOrAdd(options, _ => CreateSerializer(options, TypeNameHandling.Auto, ignoreReadOnlyProperties));
            }
            else
            {
                if (ignoreReadOnlyProperties) return _cacheNoneTypeNameReadOnly.GetOrAdd(options, _ => CreateSerializer(options, TypeNameHandling.None, ignoreReadOnlyProperties));
                return _cacheNoneTypeName.GetOrAdd(options, _ => CreateSerializer(options, TypeNameHandling.None, ignoreReadOnlyProperties));
            }
        }

        JsonSerializer CreateSerializer(ISerializationOptions options, TypeNameHandling typeNameHandling, bool ignoreReadOnlyProperties = false)
        {
            var contractResolver = new SerializerContractResolver(_container, options, ignoreReadOnlyProperties);

            var serializer = new JsonSerializer
            {
                TypeNameHandling = typeNameHandling,
                ContractResolver = contractResolver,
            };
            if( !options.IgnoreDefaultConverters ) _converters.ForEach(serializer.Converters.Add);
            options.Converters.ForEach(serializer.Converters.Add);
            options.Callback(serializer);

            return serializer;
        }
    }
}