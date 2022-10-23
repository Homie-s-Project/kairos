using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kairos.API.Utils.OAuth2.Microsoft.Models
{
    public class Json
    {
        /// <summary>
        /// Extra data for/from the JSON serializer/deserializer to included with the object model.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> ExtraJson { get; internal set; } = new Dictionary<string, JToken>();
    }
    public class OAuthResponse : Json
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        
        [JsonProperty("expires_in")]
        protected internal int ExpiresInSeconds { get; set; }
        public TimeSpan Expires { get; private set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext ctx)
        {
            this.Expires = TimeSpan.FromSeconds(this.ExpiresInSeconds);
        }
    }
}