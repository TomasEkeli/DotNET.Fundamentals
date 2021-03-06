// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Dolittle.PropertyBags.Specs.for_ObjectExtensions.for_ToPropertyBag
{
    public class DtoWithEnumerableSimple
    {
        public IEnumerable<string> StringList { get; set; } = new List<string>();
    }
}