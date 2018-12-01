/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.DependencyInversion;

namespace Dolittle.Booting
{
    /// <summary>
    /// Defines a builder for a <see cref="BootStage"/>
    /// </summary>
    public interface IBootStageBuilder
    {
        /// <summary>
        /// Gets the <see cref="IBindingProviderBuilder"/> for building specific 
        /// </summary>
        IBindingProviderBuilder Bindings { get; }

        /// <summary>
        /// Called to switch to a specific <see cref="IContainer"/> - any stage beyond this stage will use the <see cref="IContainer"/> specified
        /// </summary>
        void UseContainer(IContainer container);

        /// <summary>
        /// Associate a key with an object
        /// </summary>
        /// <param name="key">Key for the association</param>
        /// <param name="value">Value of the association</param>
        /// <remarks>
        /// This is used throughout the boot process for passing information along from stages
        /// </remarks>
        void Associate(string key, object value);

        /// <summary>
        /// Get association by kejy
        /// </summary>
        /// <param name="key">Key for the association</param>
        /// <returns>Instance associated</returns>
        object GetAssociation(string key);

        /// <summary>
        /// Build the <see cref="BootStage"/> and return the <see cref="BootStageResult">result</see>
        /// </summary>
        /// <returns>Resulting <see cref="BootStageResult"/></returns>
        BootStageResult Build();
    }
}