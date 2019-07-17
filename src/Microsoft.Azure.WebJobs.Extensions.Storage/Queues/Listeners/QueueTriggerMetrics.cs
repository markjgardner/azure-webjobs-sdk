// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs.Host.Scale;

namespace Microsoft.Azure.WebJobs.Host.Queues.Listeners
{
    internal class QueueTriggerMetrics : TriggerMetrics
    {
        /// <summary>
        /// The current length of the queue.
        /// </summary>
        public int QueueLength { get; set; }

        /// <summary>
        /// The lenght of time the next message in the queue has been
        /// sitting there.
        /// </summary>
        public TimeSpan QueueTime { get; set; }
    }
}
