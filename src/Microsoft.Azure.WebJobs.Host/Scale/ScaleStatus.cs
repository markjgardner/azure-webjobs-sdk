// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Host.Scale
{
    /// <summary>
    /// Represents the current scale status of a trigger.
    /// </summary>
    public class ScaleStatus
    {
        /// <summary>
        /// Gets or sets the current scale decision for a trigger.
        /// </summary>
        public ScaleVote Vote { get; set; }
    }
}
