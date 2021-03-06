// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Dolittle.DependencyInversion;
using Dolittle.Logging;
using Dolittle.Serialization.Json;
using Dolittle.Types;

namespace Dolittle.Configuration.Files
{
    /// <summary>
    /// Represents an implementation of <see cref="ICanParseConfigurationFile"/> for JSON.
    /// </summary>
    public class JsonConfigurationFileParser : ICanParseConfigurationFile
    {
        readonly ISerializationOptions _serializationOptions = SerializationOptions.Custom(callback:
            serializer => serializer.ContractResolver = new CamelCaseExceptDictionaryKeyResolver());

        readonly ISerializer _serializer;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationFileParser"/> class.
        /// </summary>
        /// <param name="typeFinder"><see cref="ITypeFinder"/> for finding types.</param>
        /// <param name="container"><see cerf="IContainer"/> used to get instances.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public JsonConfigurationFileParser(
            ITypeFinder typeFinder,
            IContainer container,
            ILogger logger)
        {
            var converterInstances = new InstancesOf<ICanProvideConverters>(typeFinder, container);
            _serializer = new Serializer(converterInstances);
            _logger = logger;
        }

        /// <inheritdoc/>
        public bool CanParse(Type type, string filename, string content)
        {
            if (content.StartsWith("{", StringComparison.InvariantCulture)) return true;
            return Path.GetExtension(filename).Equals(".json", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc/>
        public object Parse(Type type, string filename, string content)
        {
            _logger.Information($"Parsing '{filename}' into '{type.GetFriendlyConfigurationName()} - {type.AssemblyQualifiedName}'");
            return _serializer.FromJson(type, content, _serializationOptions);
        }
    }
}