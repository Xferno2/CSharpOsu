using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CSharpOsu.Util.BinaryHandler;
using CSharpOsu.Util.Enums;
using CSharpOsu.Util.Strings;
using CSharpOsu.Module;
using System.IO;
using System.Net.Http;
using CSharpOsu.Util;

[assembly: CLSCompliant(true)]
namespace CSharpOsu 
{
    public class OsuClient
    {
        Strings str = new Strings();
        static HttpClient client = new HttpClient();
        Utility utl;

        /// <summary>
        /// Osu API Key
        /// </summary>
        /// <param name="key">API Key</param>
        /// <param name="_throwIfNull"> If the returned objects is null then throw error. By default is set on false.</param>
        /// <param name="httpClient">Specify a HttpClient to use. Default null, will use internal HttpClient.</param>
        public OsuClient(string key,HttpClient? httpClient= null, bool _throwIfNull= false)
        {
            str.Key = key;
            if (httpClient == null)
            {
                utl = new Utility(client, _throwIfNull);
            }
            else { utl = new Utility(httpClient, _throwIfNull); }
        }

        /// <summary>
        /// Return information about beatmaps.
        /// </summary>
        /// <param name="_id">Specify a beatmapset or beatmap id.</param>
        /// <param name="_isSet">Logical switch that determine if the request is a single beatmap or a beatmapset</param>
        /// <param name="_since">Return all beatmaps ranked since this date. Must be a MySQL date.</param>
        /// <param name="_u">Specify a user or a username to return metadata from.</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, maps of all modes are returned by default.</param>
        /// <param name="_a">Specify whether converted beatmaps are included (0 = not included, 1 = included). Only has an effect if m is chosen and not 0. Converted maps show their converted difficulty rating. Optional, default is 0.</param>
        /// <param name="_h">The beatmap hash. It can be used, for instance, if you're trying to get what beatmap has a replay played in, as .osr replays only provide beatmap hashes (example of hash: a5b99395a42bd55bc5eb1d2411cbdf8b). Optional, by default all beatmaps are returned independently from the hash.</param>
        /// <param name="_limit">Amount of results. Optional, default and maximum are 500.</param>
        /// <returns>Fetch Beatmap.</returns>
        public OsuBeatmap[] GetBeatmap(long? _id = null, bool _isSet = true, DateTime? _since = null, string _u = null, mode? _m = null, conv? _a = null, string _h = null, int? _limit = null)
        {
            OsuBeatmap[] obj;
            string beatmap = str.Beatmap();
            if (_since != null)
            {
                beatmap = beatmap + "&since=" + _since;
            }
            if (_u != null)
            {
                beatmap = beatmap + "&u=" + _u;
            }
            if (_m != null)
            {
                beatmap = beatmap + "&m=" + (int)_m;
            }
            if (_a != null)
            {
                beatmap = beatmap + "&a=" + (int)_a;
            }
            if (_h != null)
            {
                beatmap = beatmap + "&h=" + _h;
            }
            if (_limit != null)
            {
                beatmap = beatmap + "&limit=" + _limit;
            }
            if (_id != null)
            {
                beatmap = (_isSet) ? beatmap + "&s=" + _id : beatmap + "&b=" + _id;
            }

            string html = utl.GetUrl(beatmap);

            obj = JsonConvert.DeserializeObject<OsuBeatmap[]>(html);
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].thumbnail = "https://b.ppy.sh/thumb/" + obj[i].beatmapset_id + "l.jpg";
                if (_id == null)
                { obj[i].beatmapset_url = "https://osu.ppy.sh/s/" + obj[i].beatmapset_id; obj[i].beatmap_url = "https://osu.ppy.sh/b/" + obj[i].beatmap_id; }
                else
                {
                    if (_isSet)
                    { obj[i].beatmapset_url = "https://osu.ppy.sh/s/" + obj[i].beatmapset_id; obj[i].beatmap_url = null; }
                    else
                    { obj[i].beatmapset_url = "https://osu.ppy.sh/s/" + obj[i].beatmapset_id; obj[i].beatmap_url = "https://osu.ppy.sh/b/" + obj[i].beatmap_id; }
                }
                obj[i].download = str.osuDowload + obj[i].beatmapset_id;
                obj[i].download_no_video = str.osuDowload + obj[i].beatmapset_id + "n";
                obj[i].osu_direct = str.osuDirect + obj[i].beatmapset_id;
                utl.ErrorHandler(obj[i]);
            }

            return obj;
        }

        /// <summary>
        /// Return informations about a user.
        /// </summary>
        /// <param name="id">Specify a user id or a username to return metadata from (required).</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, maps of all modes are returned by default.</param>
        /// <param name="_event_days">Max number of days between now and last event date. Range of 1-31. Optional, default value is 1.</param>
        /// <returns>Fetch User.</returns>
        public OsuUser[] GetUser(string id, mode? _m = null, DateTime? _event_days = null)
        {
            string user = str.User(id);
            if (_m!= null)
            {
                user = user + "&m=" + (int)_m;
            }
            if (_event_days != null)
            {
                user = user + "&u=" + _event_days;
            }
            OsuUser[] obj;
            string html = utl.GetUrl(user);
            obj = JsonConvert.DeserializeObject<OsuUser[]>(html);
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].url = "https://osu.ppy.sh/u/" + obj[i].user_id;
                obj[i].image = "https://a.ppy.sh/" + obj[i].user_id;
                obj[i].flag = "https://new.ppy.sh/images/flags/" + obj[i].country + ".png";
                obj[i].flag_old = "https://s.ppy.sh/images/flags/" + obj[i].country.ToLower() + ".gif";
                obj[i].spectateUser = "osu://spectate/" + obj[i].user_id;
                utl.ErrorHandler(obj[i]);
            }
            return obj;
        }


        /// <summary>
        /// Return informations about scores from a beatmap.
        /// </summary>
        /// <param name="_b">Specify a beatmap_id to return score information from.</param>
        /// <param name="_u">Specify a user_id or a username to return score information for.</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, default value is 0.</param>
        /// <param name="_mods">Specify a mod or mod combination (See https://github.com/ppy/osu-api/wiki#mods )</param>
        /// <param name="_limit">Amount of results from the top (range between 1 and 100 - defaults to 50).</param>
        /// <returns>Fetch Scores.</returns>
        public OsuScore[] GetScore(long _b, string _u = null, mode? _m = null, long? _mods = null, int? _limit = null)
        {
            string score = str.Score(_b);
            if (_u != null)
            {
                score = score + "&u=" + _u;
            }
            if (_m != null)
            {
                score = score + "&m=" + (int)_m;
            }
            if (_mods != null && _mods != 0)
            {
                score = score + "&mods=" + _mods;
            }
            if (_limit != null)
            {
                score = score + "&limit=" + _limit;
            }
            OsuScore[] obj;
            string html = utl.GetUrl(score);
            obj = JsonConvert.DeserializeObject<OsuScore[]>(html);
            utl.ErrorHandler(obj);

            return obj;
        }

        /// <summary>
        /// Return informations about a user best scores.
        /// </summary>
        /// <param name="_u">Specify a user_id or a username to return score information for.</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, default value is 0.</param>
        /// <param name="_limit">Amount of results from the top (range between 1 and 100 - defaults to 50).</param>
        /// <returns>Fetch user best scores.</returns>
        public OsuUserBest[] GetUserBest(string _u, mode? _m = null, int? _limit = null)
        {
            string userbest = str.User_Best(_u);
            if (_m != null)
            {
               userbest = userbest + "&m=" + (int)_m;
            }
            if (_limit != null)
            {
                userbest = userbest + "&limit=" + _limit;
            }
            OsuUserBest[] obj;
            string html = utl.GetUrl(userbest);
            obj = JsonConvert.DeserializeObject<OsuUserBest[]>(html);

            for (int i = 0; i < obj.Length; i++)
            {
                // Get Beatmap for the mode
                var bt = GetBeatmap(Convert.ToInt32(obj[i].beatmap_id), _isSet: false);
                obj[i].accuracy = utl.accuracyCalculator(bt,
                    obj[i].count50,
                    obj[i].count100,
                    obj[i].count300,
                    obj[i].countmiss,
                    obj[i].countkatu,
                    obj[i].countgeki
                    );

                utl.ErrorHandler(obj[i]);
            }
            return obj;
        }

        /// <summary>
        /// Return informations about a user recent scores.
        /// </summary>
        /// <param name="_u">Specify a user_id or a username to return score information for.</param>
        /// <param name="_m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, default value is 0.</param>
        /// <param name="_limit">Amount of results from the top (range between 1 and 100 - defaults to 50).</param>
        /// <returns>Fetch user recent scores.</returns>
        public OsuUserRecent[] GetUserRecent(string _u, mode? _m = null, int? _limit = null)
        {
            string userbest = str.User_Recent(_u);
            if (_m != null)
            {
                userbest = userbest + "&m=" + (int)_m;
            }
            if (_limit != null)
            {
                userbest = userbest + "&limit=" + _limit;
            }
            OsuUserRecent[] obj;
            string html = utl.GetUrl(userbest);
            obj = JsonConvert.DeserializeObject<OsuUserRecent[]>(html);

            for (int i = 0; i < obj.Length; i++)
            {
                // Get Beatmap for the mode
                var bt = GetBeatmap(Convert.ToInt32(obj[i].beatmap_id), _isSet: false);
                obj[i].accuracy = utl.accuracyCalculator(bt,
                    obj[i].count50,
                    obj[i].count100,
                    obj[i].count300,
                    obj[i].countmiss,
                    obj[i].countkatu,
                    obj[i].countgeki
                    );

                utl.ErrorHandler(obj[i]);
            }
            return obj;
        }

        /// <summary>
        /// Return informations about a multiplayer lobby.
        /// </summary>
        /// <param name="_mp">Match id to get information from.</param>
        /// <returns>Fetch multiplayer lobby.</returns>
        public OsuMatch GetMatch(int _mp)
        {
            OsuMatch obj;
            string match = str.Match(_mp);
            string html = utl.GetUrl(match);
            obj = JsonConvert.DeserializeObject<OsuMatch>(html);

            utl.ErrorHandler(obj);

            return obj;
        }

        /// <summary>
        /// Return informations about a replay.
        /// </summary>
        /// <param name="_m">The mode the score was played in.</param>
        /// <param name="_b">The beatmap ID (not beatmap set ID!) in which the replay was played.</param>
        /// <param name="_u">The user that has played the beatmap.</param>
        /// <param name="_mods">Specify a mod or mod combination (.</param>
        /// <returns>Fetch replay data.</returns>
        public OsuReplay GetReplay(mode _m, long _b, string _u, long? _mods)
        {
            string replay = str.Replay(_m, _b, _u);
            if (_mods != null && _mods != 0)
            {
               replay = replay + "&mods=" + _mods;
            }
            OsuReplay obj;
            string html = utl.GetUrl(replay);
            obj = JsonConvert.DeserializeObject<OsuReplay>(html);
            utl.ErrorHandler(obj);

            return obj;
            // Note that the binary data you get when you decode above base64-string, is not the contents of an.osr-file.It is the LZMA stream referred to by the osu-wiki here:
            // The remaining data contains information about mouse movement and key presses in an wikipedia:LZMA stream(https://osu.ppy.sh/wiki/Osr_(file_format)#Format)
        }

        /// <summary>
        /// Return all the bytes needed to create a .osr file.
        /// </summary>
        /// <param name="_m">The mode the score was played in.</param>
        /// <param name="_b">The beatmap ID (not beatmap set ID!) in which the replay was played.</param>
        /// <param name="_u">The user that has played the beatmap.</param>
        /// <param name="_mods">Specify a mod or mod combination (See https://github.com/ppy/osu-api/wiki#mods )</param>
        /// <returns>.osr file bytes.</returns>
        public byte[] GetReplay(mode _m, string _u, long _b, long? _mods=null)
        {
            var replay = GetReplay(_m, _b, _u, _mods);
            var sc = GetScore(_b,_u,_m ,_mods);
            var bt = GetBeatmap(_b, _isSet: false);

            var score = sc[0];
            var beatmap = bt[0];

            var binWriter = new BinaryWriter(new MemoryStream());

            var bin = new BinHandler(binWriter);
            var binReader = new BinaryReader(binWriter.BaseStream);

            var replayHashData = bin.MD5Hash(score.maxcombo + "osu" + score.username + beatmap.MD5 + score.score + score.rank);
            var content = Convert.FromBase64String(replay.content);
            var mode = Convert.ToInt32(_m).ToString();

            // Begin
            bin.writeByte(mode);                                                // Write osu mode.
            bin.writeInteger(0);                                                // Write osu version. (Unknown)
            bin.writeString(beatmap.MD5);                                       // Write beatmap MD5.
            bin.writeString(score.username);                                    // Write username.
            bin.writeString(replayHashData);                                    // Write replay MD5.
            bin.writeShort(score.count300);                                     // Write 300s count.
            bin.writeShort(score.count100);                                     // Write 100s count.
            bin.writeShort(score.count50);                                      // Write 50s count.
            bin.writeShort(score.countgeki);                                    // Write geki count.
            bin.writeShort(score.countkatu);                                    // Write katu count.
            bin.writeShort(score.countmiss);                                    // Write miss count.
            bin.writeInteger(score.score);                                      // Write score.
            bin.writeShort(score.maxcombo);                                     // Write maxcombo.
            bin.writeByte((score.perfect ? 1:0).ToString());                    // Write if the score is perfect or not.
            if (score.enabled_mods == null) { bin.writeInteger(null); }         // Write null int if no mods were enabled.
            else{
                bin.writeInteger(Convert.ToInt32(modsCalculator(                // Convert mods to int.
                    score.enabled_mods.ToList()                                 // Cast mods array to List.
                    )));
            }                                                                   // Write enabled mods.
            bin.writeString("");                                                // Write lifebar hp. (Unknown)
            bin.writeDate(score.date);                                          // Write replay timestamp.
            bin.writeInteger(content.Length);                                   // Write replay content lenght.

            // Content
            binWriter.Write(content);                                            // Write replay content.

            // Final
            binWriter.Write(Convert.ToInt64(score.score_id));                    // Write score id.
            binWriter.Write(BitConverter.GetBytes(Convert.ToUInt32(0)), 4, 0);   // Write null byte.

            binReader.BaseStream.Position = 0;
            int streamLenght = (int)binReader.BaseStream.Length;

            // [WARNING!]
            // The Get Replay Data from osu!api dosen't have a parameter to retrieve a certain replay.
            // It is possible that the movement of the cursor to be wrong because of that.
            // There is no way to fix it until such parameter is added.
            return binReader.ReadBytes(streamLenght);
        }

        /// <summary>
        /// Return int64 for a mods combination.
        /// </summary>
        /// <param name="mods">Specify mod combination as a list.(See https://github.com/ppy/osu-api/wiki#mods )</param>
        /// <returns>.osr file bytes.</returns>
        public long modsCalculator(List<Mods> mods)
        {
            long flag = 0;
            foreach (var mod in mods)
            {
                flag += Convert.ToInt64(mod);
            }
            return flag;
        }
    }
}
