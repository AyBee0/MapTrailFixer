using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Models.Info
{
    public class DifficultyBeatmapSet
    {
        [JsonProperty("_beatmapCharacteristicName")]
        public string BeatmapCharacteristicName { get; set; }
        [JsonProperty("_difficultyBeatmaps")]
        public List<DifficultyBeatmap> DifficultyBeatmaps { get; set; }
    }
}
