// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Host.Scale
{
    /// <summary>
    /// Base class for all metrics types, returned by <see cref="ITriggerScaleMonitor.GetMetricsAsync"/>.
    /// </summary>
    public class TriggerMetrics
    {
        /// <summary>
        /// Gets or sets the timestamp of when the sample was taken.
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
