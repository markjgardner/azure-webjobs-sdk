// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Host.Scale
{
    /// <summary>
    /// Context used by <see cref="ITriggerScaleMonitor.GetScaleStatus(ScaleStatusContext)"/> to decide
    /// scale status.
    /// </summary>
    public class ScaleStatusContext
    {
        /// <summary>
        /// The current worker count for the host application.
        /// </summary>
        public int WorkerCount { get; set; }

        /// <summary>
        /// The collection of metrics samples for this trigger, used to make the decision.
        /// </summary>
        public IEnumerable<TriggerMetrics> Metrics { get; set; }
    }

    public class ScaleStatusContext<TMetrics> : ScaleStatusContext where TMetrics : TriggerMetrics
    {
        /// <summary>
        /// The collection of metrics samples for this trigger, used to make the decision.
        /// </summary>
        new public IEnumerable<TMetrics> Metrics { get; set; }
    }
}
