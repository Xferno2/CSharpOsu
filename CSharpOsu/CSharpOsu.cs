using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSharpOsu
{
    public class OsuClient
    {
        public string Key { get; internal set; }


        public OsuClient(string key)
        {
            Key = key;   
        }

        string osuUrl = "https://osu.ppy.sh/";
        string osuApiUrl = "https://osu.ppy.sh/api/";
        string osuThumbnailBeatmapSet = "https://osu.ppy.sh/s/";
        string osuThumbnailBeatmap = "https://osu.ppy.sh/b/";
        string osuDowload = "https://osu.ppy.sh/d/";
        string Bloodcat = "https://bloodcat.com/osu/s/";
        string osuDirect = "osu://s/";
        string osuBeatmap = "get_beatmaps?";
        string osuScores = "get_scores?";
        string osuUser = "get_user?";
        string osuUserBest = "get_user_best?";
        string osuUserRecent = "get_user_recent?";
        string k = "k=";
        string s = "s=";
        string b = "b=";
        string u = "u=";
        string ad = "&";


        string BeatmapSet(int id) { return osuApiUrl + osuBeatmap + k + Key + ad + s + id; }
        string Beatmap(int id) { return osuApiUrl + osuBeatmap + k + Key + ad + b + id; }
        string User(string id) { return osuApiUrl + osuUser + k + Key + ad + u + id; }


        string GetUrl(string url)
        {
            var html = "";
            using (WebClient webclient = new WebClient())
            {


                using (WebClient client = new WebClient())
                {
                    html = client.DownloadString(url);
                }
            }
            return html;
        }


        public OsuBeatmap GetBeatmapSet(int id)
        {
            OsuBeatmap obj = new OsuBeatmap();
            string html = GetUrl(BeatmapSet(id));
            dynamic osudata = JsonConvert.DeserializeObject(html, typeof(object));

            obj.beatmapset_id = osudata[0].beatmapset_id;
            obj.beatmap_id = osudata[0].beatmap_id;
            obj.approved = osudata[0].approved;
            obj.total_length = osudata[0].total_length;
            obj.hit_length = osudata[0].hit_length;
            obj.version = osudata[0].version;
            obj.file_md5 = osudata[0].file_md5;
            obj.diff_size = osudata[0].diff_size;
            obj.diff_overall = osudata[0].diff_overall;
            obj.diff_approach = osudata[0].diff_approach;
            obj.diff_drain = osudata[0].diff_drain;
            obj.mode = osudata[0].mode;
            obj.approved_date = osudata[0].approved_date;
            obj.last_update = osudata[0].last_update;
            obj.artist = osudata[0].artist;
            obj.title = osudata[0].title;
            obj.creator = osudata[0].creator;
            obj.bpm = osudata[0].bpm;
            obj.source = osudata[0].source;
            obj.tags = osudata[0].tags;
            obj.genre_id = osudata[0].genre_id;
            obj.language_id = osudata[0].language_id;
            obj.favourite_count = osudata[0].favourite_count;
            obj.playcount = osudata[0].playcount;
            obj.passcount = osudata[0].passcount;
            obj.max_combo = osudata[0].max_combo;
            obj.difficultyrating = osudata[0].difficultyrating;
            obj.thumbnail = "https://b.ppy.sh/thumb/" + osudata[0].beatmapset_id + "l.jpg";
            obj.url = "https://osu.ppy.sh/s/" + obj.beatmapset_id;
            obj.download = osuDowload + obj.beatmapset_id  ;
            obj.download_no_video = osuDowload + obj.beatmapset_id + "n";
            obj.osu_direct = osuDirect + obj.beatmapset_id;
            obj.bloodcat = Bloodcat + obj.beatmapset_id;
            switch (obj.approved)
            {
                case "-2":
                    obj.approved_string = "Graveyard";
                    break;
                case "-1":
                    obj.approved_string = "Pending";
                    break;
                case "1":
                    obj.approved_string = "Ranked";
                    break;
                case "2":
                    obj.approved_string = "Approved";
                    break;
                case "3":
                    obj.approved_string = "Qualified";
                    break;
                case "4":
                    obj.approved_string = "Loved";
                    break;
                default:
                    obj.approved_string = "NULL";
                    break;
            }


            return obj;
        }

        public OsuBeatmap GetBeatmap(int id)
        {
            OsuBeatmap obj = new OsuBeatmap();
            string html = GetUrl(Beatmap(id)); ;
            dynamic osudata = JsonConvert.DeserializeObject(html, typeof(object));

            obj.beatmapset_id = osudata[0].beatmapset_id;
            obj.beatmap_id = osudata[0].beatmap_id;
            obj.approved = osudata[0].approved;
            obj.total_length = osudata[0].total_length;
            obj.hit_length = osudata[0].hit_length;
            obj.version = osudata[0].version;
            obj.file_md5 = osudata[0].file_md5;
            obj.diff_size = osudata[0].diff_size;
            obj.diff_overall = osudata[0].diff_overall;
            obj.diff_approach = osudata[0].diff_approach;
            obj.diff_drain = osudata[0].diff_drain;
            obj.mode = osudata[0].mode;
            obj.approved_date = osudata[0].approved_date;
            obj.last_update = osudata[0].last_update;
            obj.artist = osudata[0].artist;
            obj.title = osudata[0].title;
            obj.creator = osudata[0].creator;
            obj.bpm = osudata[0].bpm;
            obj.source = osudata[0].source;
            obj.tags = osudata[0].tags;
            obj.genre_id = osudata[0].genre_id;
            obj.language_id = osudata[0].language_id;
            obj.favourite_count = osudata[0].favourite_count;
            obj.playcount = osudata[0].playcount;
            obj.passcount = osudata[0].passcount;
            obj.max_combo = osudata[0].max_combo;
            obj.difficultyrating = osudata[0].difficultyrating;
            obj.thumbnail = "https://b.ppy.sh/thumb/" + osudata[0].beatmapset_id + "l.jpg";
            obj.url = "https://osu.ppy.sh/b/" + obj.beatmap_id;
            switch (obj.approved)
            {
                case "-2":
                    obj.approved_string = "Graveyard";
                    break;
                case "-1":
                    obj.approved_string = "Pending";
                    break;
                case "1":
                    obj.approved_string = "Ranked";
                    break;
                case "2":
                    obj.approved_string = "Approved";
                    break;
                case "3":
                    obj.approved_string = "Qualified";
                    break;
                case "4":
                    obj.approved_string = "Loved";
                    break;
                default:
                    obj.approved_string = "NULL";
                    break;
            }

            return obj;
        }


        public OsuUser GetUser(string id)
        {
            OsuUser obj = new OsuUser();
            string html = GetUrl(User(id));
            dynamic osudata = JsonConvert.DeserializeObject(html, typeof(object));

            obj.user_id = osudata[0].user_id;
            obj.username = osudata[0].username;
            obj.count300 = osudata[0].count300;
            obj.count100 = osudata[0].count100;
            obj.count50 = osudata[0].count50;
            obj.playcount = osudata[0].playcount;
            obj.ranked_score = osudata[0].ranked_score;
            obj.total_score = osudata[0].total_score;
            obj.pp_rank = osudata[0].pp_rank;
            obj.level = osudata[0].level;
            obj.pp_raw = osudata[0].pp_raw;
            obj.accuracy = osudata[0].accuracy;
            obj.count_rank_ss = osudata[0].count_rank_ss;
            obj.count_rank_s = osudata[0].count_rank_s;
            obj.count_rank_a = osudata[0].count_rank_a;
            obj.country = osudata[0].country;
            obj.pp_country_rank = osudata[0].pp_country_rank;
            obj.url = "https://osu.ppy.sh/u/" + obj.user_id;
            obj.image = "https://a.ppy.sh/" + obj.user_id;

            return obj;
        }
    }
}
