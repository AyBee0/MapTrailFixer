using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Parser.Models.Info
{
    public class Info
    {
        [JsonProperty("_beatsPerMinute")]
        public double BeatsPerMinute { get; set; }

        [JsonProperty("_difficultyBeatmapSets")]
        public List<DifficultyBeatmapSet> DifficultyBeatmapSets { get; set; }

        [JsonProperty("_songFilename")]
        public string SongFileName { get; set; }

        public string GetSongFile(string root)
        {
            return Path.Combine(root, SongFileName);
        }

        public string GetDifficultyFile(string root, string difficulty)
        {
            foreach (DifficultyBeatmap map in DifficultyBeatmapSets.SelectMany(x => x.DifficultyBeatmaps))
            {
                if (map.Difficulty.ToLower() == difficulty.ToLower())
                {
                    return Path.Combine(root, map.BeatmapFileName);
                }
            }
            throw new ArgumentException($"Couldn't find difficulty {difficulty}");
        }

    }
}
