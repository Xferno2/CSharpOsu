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
    public class OsuBeatmap : Model
    {
        public long beatmapset_id { get; set; }
        public long beatmap_id { get; set; }
        [JsonConverter(typeof(ApprovedConvert))]
        public ApprovedStatus approved { get; set; }
        public int total_length { get; set; }
        public int hit_length { get; set; }
        /// <summary>
        /// This is the version documented from osu!api.
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string difficulty { get; set; }
        [JsonProperty(PropertyName = "file_md5")]
        public string MD5 { get; set; }
        [JsonProperty(PropertyName = "diff_size")]
        public float CS { get; set; }
        [JsonProperty(PropertyName = "diff_overall")]
        public float OD { get; set; }
        [JsonProperty(PropertyName = "diff_approach")]
        public float AR { get; set; }
        [JsonProperty(PropertyName = "diff_drain")]
        public float HP { get; set; }
        public mode mode { get; set; }
        public DateTime? submit_date { get; set; }
        public DateTime? approved_date { get; set; }
        public DateTime last_update { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
        public string creator { get; set; }
        public string creator_id { get; set; }
        public float bpm { get; set; }
        public string source { get; set; }
        public string tags { get; set; }
        public string genre_id { get; set; }
        public string language_id { get; set; }
        public int favourite_count { get; set; }
        public float rating { get; set; }
        public int playcount { get; set; }
        public int passcount { get; set; }
        public int? max_combo { get; set; }
        [JsonProperty(PropertyName = "difficultyrating")]
        public float StarRating { get; set; }
        [JsonProperty(PropertyName = "diff_aim")]
        public float? AimRating { get; set; }
        [JsonProperty(PropertyName = "diff_speed")]
        public float? SpeedRating { get; set; }
        [JsonProperty(PropertyName = "count_normal")]
        public int CountNormal { get; set; }
        [JsonProperty(PropertyName = "count_slider")]
        public int CountSlider { get; set; }
        [JsonProperty(PropertyName = "count_spinner")]
        public int CountSpinner { get; set; }
        [JsonProperty(PropertyName = "artist_unicode")]
        public string ArtistUnicode { get; set; }
        [JsonProperty(PropertyName = "title_unicode")]
        public string TitleUnicode { get; set; }
        [JsonConverter(typeof(BoolConvert))]
        [JsonProperty(PropertyName = "storyboard")]
        public bool HasStoryboard { get; set; }
        [JsonConverter(typeof(BoolConvert))]
        [JsonProperty(PropertyName = "video")]
        public bool HasVideo { get; set; }
        [JsonConverter(typeof(BoolConvert))]
        [JsonProperty(PropertyName = "download_unavailable")]
        public bool DownloadUnavailable { get; set; }
        [JsonConverter(typeof(BoolConvert))]
        [JsonProperty(PropertyName = "audio_unavailable")]
        public bool AudioUnavailable { get; set; }
        public string Packs { get; set; }
        public string thumbnail { get; set; }
        public string beatmapset_url { get; set; }
        public string beatmap_url { get; set; }
        public string download { get; set; }
        public string download_no_video { get; set; }
        public string osu_direct { get; set; }
        public string error { get; set; }
    }
}
