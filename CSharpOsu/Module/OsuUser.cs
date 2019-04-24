using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpOsu.Module
{
    public class Event
    {
        public string display_html { get; set; }
        public long beatmap_id { get; set; }
        public long beatmapset_id { get; set; }
        public DateTime date { get; set; }
        public short epicfactor { get; set; }
    }

    public class OsuUser : Model
    {
        public long user_id { get; set; }
        public string username { get; set; }
        public DateTime join_date { get; set; }
        public long count300 { get; set; }
        public long count100 { get; set; }
        public long count50 { get; set; }
        public long playcount { get; set; }
        public long ranked_score { get; set; }
        public long total_score { get; set; }
        public int pp_rank { get; set; }
        public float level { get; set; }
        public float pp_raw { get; set; }
        public float accuracy { get; set; }
        public int count_rank_ss { get; set; }
        public int count_rank_ssh { get; set; }
        public int count_rank_s { get; set; }
        public int count_rank_sh { get; set; }
        public int count_rank_a { get; set; }
        public string country { get; set; }
        public int total_seconds_played { get; set; }
        public int pp_country_rank { get; set; }
        public List<Event> events { get; set; }
        public string url { get; set; }
        public string image { get; set; }
        public string flag { get; set; }
        public string flag_old { get; set; }
        public string spectateUser { get; set; }
        public string error { get; set; }
    }
}
