﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 doLittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using doLittle.Artifacts;

namespace doLittle.Applications
{
    /// <summary>
    /// The exception that gets thrown when parsing an <see cref="IApplicationArtifactIdentifier"/>
    /// from a string and its not possible to find the <see cref="ApplicationArea"/> in the <see cref="string"/>
    /// </summary>
    public class MissingApplicationArea : ArgumentException
    {
        /// <summary>
        /// Initializes a new 
        /// </summary>
        public MissingApplicationArea(string identifierString)
            : base($"Missing application area in '{identifierString}'. Expected format : {ApplicationArtifactIdentifierStringConverter.ExpectedFormat}")
        { }
    }
}
