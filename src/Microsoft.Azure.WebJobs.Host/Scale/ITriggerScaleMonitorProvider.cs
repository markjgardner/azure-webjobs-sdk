// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Host.Scale
{
    /// <summary>
    /// Provider for registering and returning <see cref="ITriggerScaleMonitor"/> instances for the host.
    /// </summary>
    public interface ITriggerScaleMonitorProvider
    {
        /// <summary>
        /// Register an instance.
        /// </summary>
        /// <param name="monitor">The instance to register.</param>
        void Register(ITriggerScaleMonitor monitor);

        /// <summary>
        /// Get the registered instances. Should only be called after the host has been started
        /// and all instances are registered.
        /// </summary>
        /// <returns>The monitor intances.</returns>
        IEnumerable<ITriggerScaleMonitor> GetMonitors();
    }
}
