using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ogg;
using Parser.Models.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{

    public enum Result
    {
        Success,
        DifficultyNotFound,
    }

    public class MapFixer
    {
        private readonly string folder;
        private readonly Info info;
        private readonly static Random random = new Random();
        public MapFixer(string folder)
        {
            this.folder = folder;
            this.info = JsonConvert.DeserializeObject<Info>(File.ReadAllText(Path.Combine(folder, "info.dat")));
        }

        private async Task<(Result result, int count)> Fix(string difficulty)
        {
            int count = 0;
            // Minutes
            double length = OggTools.GetOggLength(info.GetSongFile(folder));
            int maxBeatNumber = (int)Math.Round(info.BeatsPerMinute * length);

            string diffFile = info.GetDifficultyFile(folder, difficulty);
            string sDiff;
            try
            {
                sDiff = await File.ReadAllTextAsync(diffFile);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Couldn't find difficulty {difficulty}");
                return (Result.DifficultyNotFound, -1);
            }
            JObject retrieved = JObject.Parse(sDiff);

            JArray events = (JArray)retrieved["_events"];
            JArray correctEvents = Iterate(maxBeatNumber, events, "Event");
            count += events.Count - correctEvents.Count;


            JArray notes = (JArray)retrieved["_notes"];
            var correctNotes = Iterate(maxBeatNumber, notes, "Note");
            count += notes.Count - correctNotes.Count;


            JArray obstacles = (JArray)retrieved["_obstacles"];
            JArray correctObstacles = Iterate(maxBeatNumber, obstacles, "Obstacle");
            count += obstacles.Count - correctObstacles.Count;

            JArray correctBookmarks = null;

            if (retrieved.TryGetValue("_customData", out JToken cData)
                && ((JObject)cData).TryGetValue("_bookmarks", out JToken tBookmarks))
            {
                var bookmarks = (JArray)tBookmarks;
                correctBookmarks = Iterate(maxBeatNumber, bookmarks, "Bookmark");
                count += bookmarks.Count - correctBookmarks.Count;
            }

            retrieved.Remove("_notes");
            retrieved.Add("_notes", correctNotes);

            retrieved.Remove("_events");
            retrieved.Add("_events", correctEvents);

            retrieved.Remove("_obstacles");
            retrieved.Add("_obstacles", correctObstacles);

            if (correctBookmarks != null)
            {
                ((JObject)retrieved["_customData"]).Remove("_bookmarks");
                ((JObject)retrieved["_customData"]).Add("_bookmarks", correctBookmarks);
            }
            File.WriteAllText(diffFile, retrieved.ToString());
            return (Result.Success, count);
        }

        private static JArray Iterate(double maxBeatNumber, JArray obj, string type)
        {
            var correct = new JArray();
            if (random.Next(0, 8) == 0) Console.WriteLine("Cyan is a furry");
            foreach (JObject eo in obj)
            {
                if (eo.ContainsKey("_time"))
                {
                    double e = eo.Value<double>("_time");
                    if (e < maxBeatNumber)
                    {
                        correct.Add(eo);
                    }
                    else
                    {
                        Console.WriteLine($"{type} at time {e} deleted.");
                    }
                }
                else
                {
                    Console.WriteLine($"{type} doesn't have _time property. Skipping...");
                }
            }
            return correct;
        }

        public async Task<int> FixAll()
        {
            int count = 0;
            foreach (var beatmap in info.DifficultyBeatmapSets.SelectMany(x => x.DifficultyBeatmaps))
            {
                var r = await Fix(beatmap.Difficulty);
                if (r.result == Result.Success)
                {
                    count += r.count;
                }
                else
                {
                    Console.WriteLine($"Skipping difficulty {beatmap.Difficulty} due to one or more errors.");
                }
            }
            return count;
        }
    }
}
