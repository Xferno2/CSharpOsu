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
    public class OsuScore : Model
    {
        public long score_id { get; set; }
        public int score { get; set; }
        public string username { get; set; }
        public long count50 { get; set; }
        public long count100 { get; set; }
        public long count300 { get; set; }
        public long countmiss { get; set; }
        public short maxcombo { get; set; }
        public long countkatu { get; set; }
        public long countgeki { get; set; }
        [JsonConverter(typeof(BoolConvert))]
        public bool perfect { get; set; }

        [JsonConverter(converterType: typeof(ModsConvert))]
        public Mods[] enabled_mods { get; set; }

        public long user_id { get; set; }
        public DateTime date { get; set; }
        public string rank { get; set; }
        public float pp { get; set; }
        public string error { get; set; }
    }
}

