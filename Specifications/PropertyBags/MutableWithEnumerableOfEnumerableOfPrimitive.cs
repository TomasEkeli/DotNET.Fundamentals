// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Dolittle.PropertyBags.Specs
{
    public class MutableWithEnumerableOfEnumerableOfPrimitive
    {
        public IEnumerable<IEnumerable<string>> Enumerable { get; set; }
    }
}