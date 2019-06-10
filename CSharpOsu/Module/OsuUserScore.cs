using CSharpOsu.Util.Converters;
using CSharpOsu.Util.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpOsu.Module
{
    public class OsuUserRecent : ScoreModel
    {
        public long beatmap_id { get; set; }
        public DateTime date { get; set; }
        public string rank { get; set; }
        /// <summary>
        /// You will need math round to 2 decimals to get a fancy value.
        /// </summary>
        public float accuracy { get; set; }

    }
    public class OsuUserBest : OsuUserRecent
    {
        public float pp { get; set; }
    }
}
