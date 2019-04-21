using CSharpOsu.Util.Converters;
using CSharpOsu.Util.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpOsu.Module
{
    public class Match
    {
        public long match_id { get; set; }
        public string name { get; set; }
        public DateTime start_time { get; set; }
        public DateTime? end_time { get; set; }
    }

    public class Score
    {
        public short slot { get; set; }
        public short team { get; set; }
        public long user_id { get; set; }
        public int score { get; set; }
        public int maxcombo { get; set; }
        public string rank { get; set; }
        public long count50 { get; set; }
        public long count100 { get; set; }
        public long count300 { get; set; }
        public long countmiss { get; set; }
        public long countkatu { get; set; }
        public long countgeki { get; set; }
        [JsonConverter(typeof(BoolConvert))]
        public bool perfect { get; set; }
        [JsonConverter(typeof(BoolConvert))]
        public bool pass { get; set; }
    }

    public class Game
    {
        public long game_id { get; set; }
        public DateTime start_time { get; set; }
        public DateTime? end_time { get; set; }
        public long beatmap_id { get; set; }
        public mode play_mode { get; set; }
        public string match_type { get; set; }
        public string scoring_type { get; set; }
        public string team_type { get; set; }

        [JsonConverter(typeof(ModsConvert))]
        public Mods[] mods { get; set; }

        public List<Score> scores { get; set; }
    }

    public class OsuMatch : Model
    {
        public Match match { get; set; }
        public List<Game> games { get; set; }
        public string error { get; set; }
    }
}
