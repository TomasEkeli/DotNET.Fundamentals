// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.Concepts;

namespace Dolittle.Serialization.Json.Specs.for_Serializer
{
    public class GuidConcept : ConceptAs<Guid>
    {
        public static implicit operator GuidConcept(Guid value)
        {
            return new GuidConcept { Value = value };
        }
    }
}