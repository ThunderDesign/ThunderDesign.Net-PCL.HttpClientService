using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SimpleHttpClient.JSON.Converters;

namespace SimpleHttpClient.JSON.RandomName
{

    public partial class RandomName_Response
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("name")]
        public Name Name { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("first")]
        public string First { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }
    }

    public partial class RandomName_Response
    {
        public static RandomName_Response FromJson(string json) => JsonConvert.DeserializeObject<RandomName_Response>(json, DefaultConverter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this RandomName_Response self) => JsonConvert.SerializeObject(self, DefaultConverter.Settings);
    }
}
