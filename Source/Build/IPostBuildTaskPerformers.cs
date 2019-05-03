/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Dolittle.Build
{
    /// <summary>
    /// Defines the system that deals with the post build task <see cref="ICanPerformPostBuildTasks">performers</see>
    /// </summary>
    public interface IPostBuildTaskPerformers
    {
        /// <summary>
        /// Perform all tasks
        /// </summary>
        void Perform();        
    }
}