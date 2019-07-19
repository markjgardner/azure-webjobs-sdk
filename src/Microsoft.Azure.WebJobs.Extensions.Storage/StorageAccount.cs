// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs
{

    /// <summary>
    /// Wrapper around a CloudStorageAccount for abstractions and unit testing. 
    /// This is handed out by <see cref="StorageAccountProvider"/>.
    /// CloudStorageAccount is not virtual, but all the other classes below it are. 
    /// </summary>
    public class StorageAccount
    {
        /// <summary>
        /// Get the real azure storage account. Only use this if you explicitly need to bind to the <see cref="CloudStorageAccount"/>, 
        /// else use the virtuals. 
        /// </summary>

        public CloudStorageAccount SdkObject { get; protected set; }

        public static StorageAccount NewFromConnectionString(string accountConnectionString)
        {
            var settings = ParseConnectionString(accountConnectionString);
            var account = default(CloudStorageAccount);

            if (settings.ContainsKey("AccountKey")) {
                // Traditional key based authentication/authorization
                account = CloudStorageAccount.Parse(accountConnectionString);
            }
            // If the connection string doesn't contain the AccountKey then attempt to use AAD/Oauth to acquire a token
            else {
                var token = GetStorageBearerToken().GetAwaiter().GetResult();
                account = new CloudStorageAccount(token, settings["AccountName"], settings["EndpointSuffix"], true);
            }
            return New(account);
        }

        public static StorageAccount New(CloudStorageAccount account)
        {
            return new StorageAccount { SdkObject = account };
        }

        private static IDictionary<string, string> ParseConnectionString(string accountConnectionString)
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            var tuples = accountConnectionString.Split(';');
            foreach (var t in tuples) {
                var kvp = t.Split('=');
                settings.Add(kvp[0], kvp[1]);
            }
            
            return settings;
        }

        private static async Task<NewTokenAndFrequency> RenewTokenFuncAsync(object state, CancellationToken cancellationToken)
        {
            // To request tokens for Azure Storage, specify the value https://storage.azure.com/ for the Resource ID.
            var token = await ((AzureServiceTokenProvider)state).GetAuthenticationResultAsync("https://storage.azure.com/");

            // Default expiry is 65 minutes, renew 5 minutes before expiration (or every 1 hour).
            var renewal = (token.ExpiresOn - DateTimeOffset.UtcNow) - TimeSpan.FromMinutes(5);

            // Return the new token and renewal time.
            return new NewTokenAndFrequency(token.AccessToken, renewal);
        }

        private static async Task<StorageCredentials> GetStorageBearerToken() {
            
            var tokenProvider = new AzureServiceTokenProvider();
            
            // Get the first token and its expiration
            var tokenAndFreq = await RenewTokenFuncAsync(tokenProvider, CancellationToken.None);

            // Manage the lifecycle of the token
            var tokenCredential = new TokenCredential(tokenAndFreq.Token, 
                RenewTokenFuncAsync, 
                tokenProvider, 
                tokenAndFreq.Frequency ?? TimeSpan.MinValue);

            return new StorageCredentials(tokenCredential);
        }

        public virtual bool IsDevelopmentStorageAccount()
        {
            // see the section "Addressing local storage resources" in http://msdn.microsoft.com/en-us/library/windowsazure/hh403989.aspx
            return String.Equals(
                SdkObject.BlobEndpoint.PathAndQuery.TrimStart('/'),
                SdkObject.Credentials.AccountName,
                StringComparison.OrdinalIgnoreCase);
        }

        public virtual string Name
        {
            get { return SdkObject.Credentials.AccountName; }
        }

        public virtual Uri BlobEndpoint => SdkObject.BlobEndpoint;

        public virtual CloudBlobClient CreateCloudBlobClient()
        {
            return SdkObject.CreateCloudBlobClient();
        }
        public virtual CloudQueueClient CreateCloudQueueClient()
        {
            return SdkObject.CreateCloudQueueClient();
        }

        public virtual CloudTableClient CreateCloudTableClient()
        {
            return SdkObject.CreateCloudTableClient();
        }
    }
}