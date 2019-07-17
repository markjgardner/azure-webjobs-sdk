// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Microsoft.Azure.WebJobs.Host.Queues
{
    internal static class StorageQueueExtensions
    {
        public static async Task AddMessageAndCreateIfNotExistsAsync(this CloudQueue queue,
            CloudQueueMessage message, CancellationToken cancellationToken)
        {
            if (queue == null)
            {
                throw new ArgumentNullException("queue");
            }

            bool isQueueNotFoundException = false;

            try
            {
                await queue.AddMessageAsync(message, cancellationToken);
                return;
            }
            catch (StorageException exception)
            {
                if (!exception.IsNotFoundQueueNotFound())
                {
                    throw;
                }

                isQueueNotFoundException = true;
            }

            Debug.Assert(isQueueNotFoundException);
            await queue.CreateIfNotExistsAsync(cancellationToken);
            await queue.AddMessageAsync(message, cancellationToken);
        }

        public static async Task<int> TryGetLengthAsync(
            this CloudQueue queue,
            ILogger logger,
            bool ignoreQueueNotFoundError = false)
        {
            try
            {
                await queue.FetchAttributesAsync();
            }
            catch (StorageException e)
            {
                if (e.RequestInformation.HttpStatusCode == 404)
                {
                    // If this was instantiated to monitor the queue used to process blobs, ignore 404 as the queue isn't created till at least 1 blob is found
                    if (!ignoreQueueNotFoundError)
                    {
                        logger.LogInformation($"Queue '{queue.Name}' was not found.");
                    }
                }
                else
                {
                    logger.LogError($"Error occurred when checking length of queue '{queue.Name}': {e.Message}");
                }

                return -1;
            }

            int queueLength = queue.ApproximateMessageCount.GetValueOrDefault();
            return queueLength;
        }
    }
}
