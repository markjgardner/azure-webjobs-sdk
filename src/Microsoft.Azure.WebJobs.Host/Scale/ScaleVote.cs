// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Host.Scale
{
    /// <summary>
    /// Represents a scale decision made by a <see cref="ITriggerScaleMonitor"/>.
    /// </summary>
    public enum ScaleVote
    {
        None = 0,
        ScaleOut = 1,
        ScaleIn = -1
    }
}
