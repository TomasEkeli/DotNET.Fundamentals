﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Reflection;
using Dolittle.Concepts;
using Newtonsoft.Json;

namespace Dolittle.Concepts.Serialization.Json
{
    /// <summary>
    /// Implements a <see cref="JsonConverter"/> that deals with serializing and deserializing of <see cref="ConceptAs{T}"/>
    /// </summary>
    public class ConceptConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsConcept();
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ConceptFactory.CreateConceptInstance(objectType, reader.Value);
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object concept, JsonSerializer serializer)
        {
            var value = concept.GetType().GetTypeInfo().GetProperty("Value").GetValue(concept, null);
            writer.WriteValue(value);
        }
    }
}
