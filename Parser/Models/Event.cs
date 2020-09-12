using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Models
{
    public class Event
    {
        [JsonProperty("_time")]
        public double Time { get; set; }
        [JsonProperty("_type")]
        public int Type { get; set; }
        [JsonProperty("_value")]
        public long Value { get; set; }

    }
}
