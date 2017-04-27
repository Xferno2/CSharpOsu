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

        /// <summary>
        /// Osu API Key
        /// </summary>
        /// <param name="key">API Key</param>
        public OsuClient(string key)
        {
            Key = key;   
        }

        /// <summary>
        /// A bunch of strings.
        /// </summary>
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
        string Beatmap() { return osuApiUrl + osuBeatmap + k + Key; }
        string User(string id) { return osuApiUrl + osuUser + k + Key + ad + u + id; }

        /// <summary>
        /// Fetch JSON.
        /// </summary>
        /// <param name="url">Url of the JSON.</param>
        /// <returns>Get JSON to be pharsed.</returns>
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

        /// <summary>
        /// Fetch Beatmap.
        /// </summary>
        /// <param name="_id">Specify a beatmapset or beatmap id.</param>
        /// <param name="_isSet">If is beatmapset or not(default true)</param>
        /// <param name="_since">Return all beatmaps ranked since this date. Must be a MySQL date.</param>
        /// <param name="_u">Specify a user or a username to return metadata from.</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, maps of all modes are returned by default.</param>
        /// <param name="_a">Specify whether converted beatmaps are included (0 = not included, 1 = included). Only has an effect if m is chosen and not 0. Converted maps show their converted difficulty rating. Optional, default is 0.</param>
        /// <param name="_h">The beatmap hash. It can be used, for instance, if you're trying to get what beatmap has a replay played in, as .osr replays only provide beatmap hashes (example of hash: a5b99395a42bd55bc5eb1d2411cbdf8b). Optional, by default all beatmaps are returned independently from the hash.</param>
        /// <param name="_limit">Amount of results. Optional, default and maximum are 500.</param>
        /// <returns>Get information about a beatmaps.</returns>
        public OsuBeatmap[] GetBeatmap(int? _id = null, bool _isSet = true, int? _since = null, string _u = null, int? _m = null, int? _a = null, string _h = null, int? _limit = null)
        {
            OsuBeatmap[] obj;
            string beatmap = Beatmap();
            if (_since != null)
            {
                beatmap = beatmap + "&since=" + _since;
            } else if(_u != null)
            {
                beatmap = beatmap + "&u=" + _u;
            } else if(_m != null)
            {
                beatmap = beatmap + "&m=" + _m;
            } else if (_a != null)
            {
                beatmap = beatmap + "&a=" + _a;
            } else if (_h != null)
            {
                beatmap = beatmap + "&h=" + _h;
            } else if (_limit != null)
            {
                beatmap = beatmap + "&limit=" + _limit;
            } else if (_id != null)
            {
                beatmap = (_isSet) ? beatmap + ad + s + _id : beatmap + ad + b + _id;
            }

            string html = GetUrl(beatmap);

            obj = JsonConvert.DeserializeObject<OsuBeatmap[]>(html);
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].thumbnail = "https://b.ppy.sh/thumb/" + obj[i].beatmapset_id + "l.jpg";
                obj[i].url = "https://osu.ppy.sh/s/" + obj[i].beatmapset_id;
                obj[i].download = osuDowload + obj[i].beatmapset_id;
                obj[i].download_no_video = osuDowload + obj[i].beatmapset_id + "n";
                obj[i].osu_direct = osuDirect + obj[i].beatmapset_id;
                obj[i].bloodcat = Bloodcat + obj[i].beatmapset_id;
                switch (obj[i].approved)
                {
                    case "-2":
                        obj[i].approved_string = "Graveyard";
                        break;
                    case "-1":
                        obj[i].approved_string = "Pending";
                        break;
                    case "1":
                        obj[i].approved_string = "Ranked";
                        break;
                    case "2":
                        obj[i].approved_string = "Approved";
                        break;
                    case "3":
                        obj[i].approved_string = "Qualified";
                        break;
                    case "4":
                        obj[i].approved_string = "Loved";
                        break;
                    default:
                        obj[i].approved_string = "NULL";
                        break;
                }
            }

            return obj;
        }

        /// <summary>
        /// Fetch User.
        /// </summary>
        /// <param name="id">Specify a user id or a username to return metadata from (required).</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, maps of all modes are returned by default.</param>
        /// <param name="_event_days">Max number of days between now and last event date. Range of 1-31. Optional, default value is 1.(NOT IMPLEMENTED YET!)</param>
        /// <returns>Get informations about user.</returns>
        public OsuUser[] GetUser(string id , int? _m = null, int? _event_days = null)
        {
            string user = User(id);
            if (_m!= null)
            {
                user = user + "&m=" + _m;
            }
            else if (_event_days != null)
            {
                user = user + "&u=" + _event_days;
            }
            OsuUser[] obj;
            string html = GetUrl(user);
            obj = JsonConvert.DeserializeObject<OsuUser[]>(html);
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].url = "https://osu.ppy.sh/u/" + obj[i].user_id;
                obj[i].image = "https://a.ppy.sh/" + obj[i].user_id;
            }
            return obj;
        }
    }
}
