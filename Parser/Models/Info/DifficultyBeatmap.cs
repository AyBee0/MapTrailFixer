using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Models.Info
{
    public class DifficultyBeatmap
    {
        [JsonProperty("_difficulty")]
        public string Difficulty { get; set; }
        [JsonProperty("_beatmapFilename")]
        public string BeatmapFileName { get; set; }
    }
}
