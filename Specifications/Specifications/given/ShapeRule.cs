/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace doLittle.Specifications.Specs.given
{
    public class ShapeRule : Specification<ColoredShape>
    {
        readonly string _shape;

        public ShapeRule(string matchingShape)
        {
            _shape = matchingShape;
            Predicate = shape => shape.Shape == _shape;
        }
    }
}