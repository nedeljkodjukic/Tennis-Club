﻿// Copyright (c) 2020 Allan Mobley. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Mobsites.Cosmos.Identity
{
    /// <summary>
    ///     The required Cosmos Identity implementation of an identity user login which uses a string as a primary key.
    /// </summary>
    public class IdentityUserLogin : Microsoft.AspNetCore.Identity.IdentityUserLogin<string>, ICosmosStorageType
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="IdentityUserLogin"/>.
        /// </summary>
        /// <remarks>
        ///     The Id property is initialized to form a new GUID string value.
        /// </remarks>
        public IdentityUserLogin()
        {
            Id = Guid.NewGuid().ToString();
        }


        /// <summary>
        ///     Gets the unique id associated with the item from the Azure Cosmos DB service.
        /// </summary>
        /// <remarks>
        ///     Cosmos requires a string property named "id" as a primary key. 
        ///     The base class does not provide one to override or hide.
        /// </remarks>
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public string Id { get; set; }


        /// <summary>
        ///     Gets the partition key used by the default Cosmos storage provider.
        /// </summary>
        /// <remarks>
        ///     Override this to provide a value that is different than the default.
        /// </remarks>
        public virtual string PartitionKey => nameof(IdentityUserLogin);


        /// <summary>
        ///     Gets the time to live in seconds of the item in the Azure Cosmos DB service.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, PropertyName = "ttl")]
        [System.Text.Json.Serialization.JsonPropertyName("ttl")]
        public int? TimeToLive { get; set; }


        /// <summary>
        ///     Gets the entity tag associated with the item from the Azure Cosmos DB service.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("_etag")]
        [System.Text.Json.Serialization.JsonPropertyName("_etag")]
        public string Etag { get; set; }


        /// <summary>
        ///     Gets the last modified timestamp associated with the item from the Azure Cosmos DB service.
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(UnixDateTimeConverter))]
        [Newtonsoft.Json.JsonProperty("_ts")]
        [System.Text.Json.Serialization.JsonPropertyName("_ts")]
        public DateTime Timestamp { get; set; }
    }
}
