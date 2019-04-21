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
    public class OsuUserBest : Model
    {
        public long beatmap_id { get; set; }
        public int score { get; set; }
        public long maxcombo { get; set; }
        public long count50 { get; set; }
        public long count100 { get; set; }
        public long count300 { get; set; }
        public long countmiss { get; set; }
        public long countkatu { get; set; }
        public long countgeki { get; set; }
        [JsonConverter(typeof(BoolConvert))]
        public bool perfect { get; set; }

        [JsonConverter(typeof(ModsConvert))]
        public Mods[] enabled_mods { get; set; }

        public long user_id { get; set; }
        public DateTime date { get; set; }
        public string rank { get; set; }
        public float pp { get; set; }
        /// <summary>
        /// You will need math round to 2 decimals to get a fancy value.
        /// </summary>
        public float accuracy { get; set; }

        public string error { get; set; }
    }
}
