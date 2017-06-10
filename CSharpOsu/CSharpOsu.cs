using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CSharpOsu.BinaryHandler;
using CSharpOsu.Enum;
using System.IO;


[assembly: CLSCompliant(true)]
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
        string osuApiUrl = "https://osu.ppy.sh/api/";
        string osuDowload = "https://osu.ppy.sh/d/";
        string Bloodcat = "https://bloodcat.com/osu/s/";
        string osuDirect = "osu://s/";

        string osuBeatmap = "get_beatmaps?";
        string osuScores = "get_scores?";
        string osuUser = "get_user?";
        string osuUserBest = "get_user_best?";
        string osuUserRecent = "get_user_recent?";
        string osuMatch = "get_match?";
        string osuReplay = "get_replay?";

        string k = "k=";
        string s = "s=";
        string b = "b=";
        string u = "u=";
        string m = "m=";
        string mp = "mp=";
        string ad = "&";

        string Beatmap() { return osuApiUrl + osuBeatmap + k + Key; }
        string User(string id) { return osuApiUrl + osuUser + k + Key + ad + u + id; }
        string Score(int beatmap) { return osuApiUrl + osuScores + k + Key + ad + b.ToString() + beatmap; }
        string User_Best(string user) { return osuApiUrl + osuUserBest + k + Key + ad + u + user; }
        string User_Recent(string user) { return osuApiUrl + osuUserRecent + k + Key + ad + u + user; }
        string Match(int match) { return osuApiUrl + osuMatch + k + Key + ad + mp + match; }
        string Replay(mode mode, int beatmap, string user) { return osuApiUrl + osuReplay + k + Key + ad + m + mode + ad + b + beatmap + ad + u + user; }

        /// <summary>
        /// Get JSON to be pharsed.
        /// </summary>
        /// <param name="url">Url of the JSON.</param>
        /// <returns>Fetch JSON.</returns>
        string GetUrl(string url)
        {
            string html= "";
            using (WebClient client = new WebClient())
            {
                    html = client.DownloadString(url);
            }
            return html;
        }

        /// <summary>
        /// Get information about beatmaps.
        /// </summary>
        /// <param name="_id">Specify a beatmapset or beatmap id.</param>
        /// <param name="_isSet">If is beatmapset or not(default true)</param>
        /// <param name="_since">Return all beatmaps ranked since this date. Must be a MySQL date.</param>
        /// <param name="_u">Specify a user or a username to return metadata from.</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, maps of all modes are returned by default.</param>
        /// <param name="_a">Specify whether converted beatmaps are included (0 = not included, 1 = included). Only has an effect if m is chosen and not 0. Converted maps show their converted difficulty rating. Optional, default is 0.</param>
        /// <param name="_h">The beatmap hash. It can be used, for instance, if you're trying to get what beatmap has a replay played in, as .osr replays only provide beatmap hashes (example of hash: a5b99395a42bd55bc5eb1d2411cbdf8b). Optional, by default all beatmaps are returned independently from the hash.</param>
        /// <param name="_limit">Amount of results. Optional, default and maximum are 500.</param>
        /// <returns>Fetch Beatmap.</returns>
        public OsuBeatmap[] GetBeatmap(int? _id = null, bool _isSet = true, string _since = null, string _u = null, mode? _m = null, conv? _a = null, string _h = null, int? _limit = null)
        {
            OsuBeatmap[] obj;
            string beatmap = Beatmap();
            if (_since != null)
            {
                beatmap = beatmap + "&since=" + _since;
            } else if (_u != null)
            {
                beatmap = beatmap + "&u=" + _u;
            } else if (_m != null)
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
        /// Get informations about a user.
        /// </summary>
        /// <param name="id">Specify a user id or a username to return metadata from (required).</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, maps of all modes are returned by default.</param>
        /// <param name="_event_days">Max number of days between now and last event date. Range of 1-31. Optional, default value is 1.</param>
        /// <returns>Fetch User.</returns>
        public OsuUser[] GetUser(string id, mode? _m = null, int? _event_days = null)
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
                obj[i].flag = "https://new.ppy.sh/images/flags/" + obj[i].country + ".png";
                obj[i].flag_old = "https://s.ppy.sh/images/flags/" + obj[i].country.ToLower() + ".gif";
                obj[i].osutrack = "https://ameobea.me/osutrack/user/" + obj[i].username;
                obj[i].osustats = "https://osustats.ppy.sh/u/" + obj[i].username;
                obj[i].osuskills = "http://osuskills.tk/user/" + obj[i].username;
                obj[i].osuchan = "https://syrin.me/osuchan/u/" + obj[i].user_id;
                obj[i].spectateUser = "osu://spectate/" + obj[i].user_id;
            }
            return obj;
        }


        /// <summary>
        /// Get informations about scores from a beatmap.
        /// </summary>
        /// <param name="_b">Specify a beatmap_id to return score information from.</param>
        /// <param name="_u">Specify a user_id or a username to return score information for.</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, default value is 0.</param>
        /// <param name="_mods">Specify a mod or mod combination (See https://github.com/ppy/osu-api/wiki#mods )</param>
        /// <param name="_limit">Amount of results from the top (range between 1 and 100 - defaults to 50).</param>
        /// <returns>Fetch Scores.</returns>
        public OsuScore[] GetScore(int _b, string _u = null, mode? _m = null, int? _mods = null, int? _limit = null)
        {
            string score = Score(_b);
            if (_u != null)
            {
                score = score + "&u=" + _u;
            } else if (_m != null)
            {
                score = score + "&m=" + _m;
            } else if (_mods != null)
            {
                score = score + "&mods=" + _mods;
            } else if (_limit != null)
            {
                score = score + "&limit=" + _limit;
            }
            OsuScore[] obj;
            string html = GetUrl(score);
            obj = JsonConvert.DeserializeObject<OsuScore[]>(html);

            return obj;
        }

        /// <summary>
        /// Get informations about a user best scores.
        /// </summary>
        /// <param name="_u">Specify a user_id or a username to return score information for.</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, default value is 0.</param>
        /// <param name="_limit">Amount of results from the top (range between 1 and 100 - defaults to 50).</param>
        /// <returns>Fetch user best scores.</returns>
        public OsuUserBest[] GetUserBest(string _u, mode? _m = null, int? _limit = null)
        {
            string userbest = User_Best(_u);
            if (_m != null)
            {
               userbest = userbest + "&m=" + _m;
            }
            else if (_limit != null)
            {
                userbest = userbest + "&limit=" + _limit;
            }
            OsuUserBest[] obj;
            string html = GetUrl(userbest);
            obj = JsonConvert.DeserializeObject<OsuUserBest[]>(html);

            return obj;
        }

        /// <summary>
        /// Get informations about a user recent scores.
        /// </summary>
        /// <param name="_u">Specify a user_id or a username to return score information for.</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, default value is 0.</param>
        /// <param name="_limit">Amount of results from the top (range between 1 and 100 - defaults to 50).</param>
        /// <returns>Fetch user recent scores.</returns>
        public OsuUserRecent[] GetUserRecent(string _u, mode? _m = null, int? _limit = null)
        {
            string userbest = User_Recent(_u);
            if (_m != null)
            {
                userbest = userbest + "&m=" + _m;
            }
            else if (_limit != null)
            {
                userbest = userbest + "&limit=" + _limit;
            }
            OsuUserRecent[] obj;
            string html = GetUrl(userbest);
            obj = JsonConvert.DeserializeObject<OsuUserRecent[]>(html);

            return obj;
        }

        /// <summary>
        /// Get informations about a multiplayer lobby.
        /// </summary>
        /// <param name="_mp">Match id to get information from.</param>
        /// <returns>Fetch multiplayer lobby.</returns>
        public OsuMatch GetMatch(int _mp)
        {
            OsuMatch obj;
            string match = Match(_mp);
            string html = GetUrl(match);
            obj = JsonConvert.DeserializeObject<OsuMatch>(html);

            return obj;
        }

        /// <summary>
        /// Get informations about a replay.
        /// </summary>
        /// <param name="_m">The mode the score was played in.</param>
        /// <param name="_b">The beatmap ID (not beatmap set ID!) in which the replay was played.</param>
        /// <param name="_u">The user that has played the beatmap.</param>
        /// <returns>Fetch replay data.</returns>
        public OsuReplay GetReplay(mode _m, int _b, string _u)
        {
            OsuReplay obj;
            string replay = Replay(_m, _b, _u);
            string html = GetUrl(replay);
            obj = JsonConvert.DeserializeObject<OsuReplay>(html);
            if (obj.error == null)
            {
                obj.error = "none";
            }

            return obj;
            // Note that the binary data you get when you decode above base64-string, is not the contents of an.osr-file.It is the LZMA stream referred to by the osu-wiki here:
            // The remaining data contains information about mouse movement and key presses in an wikipedia:LZMA stream(https://osu.ppy.sh/wiki/Osr_(file_format)#Format)
        }

        /// <summary>
        /// Get all the bytes to create a .osr file.
        /// </summary>
        /// <param name="_m">The mode the score was played in.</param>
        /// <param name="_b">The beatmap ID (not beatmap set ID!) in which the replay was played.</param>
        /// <param name="_u">The user that has played the beatmap.</param>
        /// <param name="t">A way to show the compliler that this is a different function.</param>
        /// <param name="_mods">Specify a mod or mod combination (See https://github.com/ppy/osu-api/wiki#mods )</param>
        /// <param name="_count">There can be mroe than 1 replay that contains those arguments , an array is needed.</param>
        /// <returns>.osr file bytes.</returns>
        public byte[] GetReplay(mode _m, int _b, string _u, type t, int _mods=0, int _count = 0)
        {
            var replay = GetReplay(_m, _b, _u);

            //Throw
            if (replay.error == "Replay not available.")
            {
                throw new Exception("Replay data not available.");
            }

            var sc = GetScore(_b,_u,_m ,_mods);
            var bt = GetBeatmap(Convert.ToInt32(_b), _isSet: false);

            var score = sc[_count];
            var beatmap = bt[_count];

            var bin = new BinHandler();

            BinaryWriter binWriter = new BinaryWriter(new MemoryStream());
            BinaryReader binReader = new BinaryReader(binWriter.BaseStream);

            var replayHashData = score.maxcombo + "osu" + score.username + beatmap.file_md5 + score.score + score.rank;
            var content = Convert.FromBase64String(replay.content);
            var mode = Convert.ToInt32(_m).ToString();

            bin.writeByte(binWriter, mode);
            bin.writeInteger(binWriter, 0);
            bin.writeString(binWriter, beatmap.file_md5);
            bin.writeString(binWriter, score.username);
            bin.writeString(binWriter, bin.MD5Hash(replayHashData).ToLower());
            bin.writeShort(binWriter, Convert.ToInt16(score.count300));
            bin.writeShort(binWriter, Convert.ToInt16(score.count100));
            bin.writeShort(binWriter, Convert.ToInt16(score.count50));
            bin.writeShort(binWriter, Convert.ToInt16(score.countgeki));
            bin.writeShort(binWriter, Convert.ToInt16(score.countkatu));
            bin.writeShort(binWriter, Convert.ToInt16(score.countmiss));
            bin.writeInteger(binWriter, Convert.ToInt32(score.score));
            bin.writeShort(binWriter, Convert.ToInt16(score.maxcombo));
            bin.writeByte(binWriter, score.perfect);
            bin.writeInteger(binWriter, Convert.ToInt32(score.enabled_mods));
            bin.writeString(binWriter, "");
            bin.writeDate(binWriter, score.date);
            bin.writeInteger(binWriter, content.Length);
            binWriter.Write(content);

            binWriter.Write(Convert.ToInt64(score.score_id));
            binWriter.Write(BitConverter.GetBytes(Convert.ToUInt32(0)), 4, 0);

            binReader.BaseStream.Position = 0;
            int streamLenght = Convert.ToInt32(binReader.BaseStream.Length);

            return binReader.ReadBytes(streamLenght);

        }

    }
}
