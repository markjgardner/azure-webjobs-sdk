// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Host.Scale
{
    /// <summary>
    /// Interface implemented by an extension's <see cref="IListener"/> to allow the extension
    /// to participate in scale decisions.
    /// </summary>
    public interface ITriggerScaleMonitor
    {
        /// <summary>
        /// Gets the full function Id.
        /// </summary>
        string FunctionId { get; }

        /// <summary>
        /// Gets the trigger type (e.g. QueueTrigger).
        /// </summary>
        string TriggerType { get; }

        /// <summary>
        /// Gets the resource Id for the event source being monitored (e.g. the queue name).
        /// </summary>
        string ResourceId { get; }

        /// <summary>
        /// Return a current metrics sample by querying the event source.
        /// </summary>
        /// <returns></returns>
        Task<TriggerMetrics> GetMetricsAsync();

        /// <summary>
        /// Return the current scale status based on the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        ScaleStatus GetScaleStatus(ScaleStatusContext context);
    }

    public interface ITriggerScaleMonitor<TMetrics> : ITriggerScaleMonitor where TMetrics : TriggerMetrics
    {
        /// <summary>
        /// Return a current metrics sample by querying the event source.
        /// </summary>
        /// <returns></returns>
        new Task<TMetrics> GetMetricsAsync();

        /// <summary>
        /// Return the current scale status based on the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        ScaleStatus GetScaleStatus(ScaleStatusContext<TMetrics> context);
    }
}
