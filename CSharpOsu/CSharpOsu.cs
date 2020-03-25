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
        Strings str;
        static HttpClient client = new HttpClient();
        Utility utl;

        public readonly string APIKey;
        public bool bypassLimit = false;
        // public short Highlimit = 1200;
        // private short burst = 200;
        public short limit = 60;
        public bool bypassReplayLimit = false;
        public short replayLimit = 10;
        private short replayReq = 0;

        DateTime oldTime = DateTime.UtcNow;
        /// <summary>
        /// Osu API Key
        /// </summary>
        /// <param name="key">API Key</param>
        /// <param name="_throwIfNull"> If the returned objects is null then throw error. By default is set on false.</param>
        /// <param name="httpClient">Specify a HttpClient to use. Default null, will use internal HttpClient.</param>
        public OsuClient(string key,HttpClient? httpClient= null, bool _throwIfNull= false)
        {
            APIKey = key;
            str = new Strings(key);
            if (httpClient == null)
            {
                utl = new Utility(client, _throwIfNull, bypassLimit, limit,bypassReplayLimit,replayLimit);
            }
            else { utl = new Utility(httpClient, _throwIfNull, bypassLimit, limit, bypassReplayLimit, replayLimit); }
        }

        /// <summary>
        /// Return information about beatmaps.
        /// </summary>
        /// <param name="id">Specify a beatmapset or beatmap id.</param>
        /// <param name="isSet">Logical switch that determine if the request is a single beatmap or a beatmapset</param>
        /// <param name="since">Return all beatmaps ranked since this date. Must be a MySQL date.</param>
        /// <param name="u">Specify a user or a username to return metadata from.</param>
        /// <param name="m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, maps of all modes are returned by default.</param>
        /// <param name="a">Specify whether converted beatmaps are included (0 = not included, 1 = included). Only has an effect if m is chosen and not 0. Converted maps show their converted difficulty rating. Optional, default is 0.</param>
        /// <param name="h">The beatmap hash. It can be used, for instance, if you're trying to get what beatmap has a replay played in, as .osr replays only provide beatmap hashes (example of hash: a5b99395a42bd55bc5eb1d2411cbdf8b). Optional, by default all beatmaps are returned independently from the hash.</param>
        /// <param name="limit">Amount of results. Optional, default and maximum are 500.</param>
        /// <param name="userType">Specify if u is a user_id or a username. Use string for usernames or id for user_ids. Optional, default behaviour is automatic recognition (may be problematic for usernames made up of digits only).</param>
        /// <param name="mods">Specify a mod or mod combination (See https://github.com/ppy/osu-api/wiki#mods )</param>
        /// <returns>Fetch Beatmap.</returns>
        public OsuBeatmap[] GetBeatmap(long? id = null, bool isSet = true, DateTime? since = null, string u = null, mode? m = null, include? a = null, string h = null, int? limit = null, typeUser ? userType = null, long ? mods = null)
        {
            OsuBeatmap[] obj;
            string html = str.CreateURL(str.Beatmap,
                "since", since,
                "u", u,
                "m", m,
                "a", a,
                "h", h,
                "limit", limit,
                "type", userType,
                "mods", mods,
                (isSet) ? "s" : "b", id);

            obj = JsonConvert.DeserializeObject<OsuBeatmap[]>(utl.GetURL(html));
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].thumbnail = "https://b.ppy.sh/thumb/" + obj[i].beatmapset_id + "l.jpg";
                if (id == null)
                { obj[i].beatmapset_url = "https://osu.ppy.sh/s/" + obj[i].beatmapset_id; obj[i].beatmap_url = "https://osu.ppy.sh/b/" + obj[i].beatmap_id; }
                else
                {
                    if (isSet)
                    { obj[i].beatmapset_url = "https://osu.ppy.sh/s/" + obj[i].beatmapset_id; obj[i].beatmap_url = null; }
                    else
                    { obj[i].beatmapset_url = "https://osu.ppy.sh/s/" + obj[i].beatmapset_id; obj[i].beatmap_url = "https://osu.ppy.sh/b/" + obj[i].beatmap_id; }
                }
                obj[i].download = str.Dowload + obj[i].beatmapset_id;
                obj[i].download_no_video = str.Dowload + obj[i].beatmapset_id + "n";
                obj[i].osu_direct = str.Direct + obj[i].beatmapset_id;
                utl.ErrorHandler(obj[i]);
            }

            return obj;
        }

        /// <summary>
        /// Return information about beatmaps.
        /// </summary>
        /// <param name="id">Specify a beatmapset or beatmap id.</param>
        /// <param name="isSet">Logical switch that determine if the request is a single beatmap or a beatmapset</param>
        /// <param name="since">Return all beatmaps ranked since this date. Must be a MySQL date.</param>
        /// <param name="u">Specify a user or a username to return metadata from.</param>
        /// <param name="m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, maps of all modes are returned by default.</param>
        /// <param name="a">Specify whether converted beatmaps are included (0 = not included, 1 = included). Only has an effect if m is chosen and not 0. Converted maps show their converted difficulty rating. Optional, default is 0.</param>
        /// <param name="h">The beatmap hash. It can be used, for instance, if you're trying to get what beatmap has a replay played in, as .osr replays only provide beatmap hashes (example of hash: a5b99395a42bd55bc5eb1d2411cbdf8b). Optional, by default all beatmaps are returned independently from the hash.</param>
        /// <param name="limit">Amount of results. Optional, default and maximum are 500.</param>
        /// <param name="userType">Specify if u is a user_id or a username. Use string for usernames or id for user_ids. Optional, default behaviour is automatic recognition (may be problematic for usernames made up of digits only).</param>
        /// <param name="mods">Specify a mod or mod combination (See https://github.com/ppy/osu-api/wiki#mods )</param>
        /// <returns>Fetch Beatmap.</returns>
        public async Task<OsuBeatmap[]> GetBeatmapAsync(long? id = null, bool isSet = true, DateTime? since = null, string u = null, mode? m = null, include? a = null, string h = null, int? limit = null, typeUser? userType = null, long? mods = null)
        {
            Task<string> CreateUrl = new Task<string>(() => str.CreateURL(str.Beatmap,
                "since", since,
                "u", u,
                "m", m,
                "a", a,
                "h", h,
                "limit", limit,
                "type", userType,
                "mods", mods,
                isSet ? "s" : "b", id));
            OsuBeatmap[] obj;
            string html = await CreateUrl;

            Task<string> getURL = new Task<string>(() => utl.GetURL(html));
            obj = JsonConvert.DeserializeObject<OsuBeatmap[]>(await getURL);
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].thumbnail = "https://b.ppy.sh/thumb/" + obj[i].beatmapset_id + "l.jpg";
                if (id == null)
                { obj[i].beatmapset_url = "https://osu.ppy.sh/s/" + obj[i].beatmapset_id; obj[i].beatmap_url = "https://osu.ppy.sh/b/" + obj[i].beatmap_id; }
                else
                {
                    if (isSet)
                    { obj[i].beatmapset_url = "https://osu.ppy.sh/s/" + obj[i].beatmapset_id; obj[i].beatmap_url = null; }
                    else
                    { obj[i].beatmapset_url = "https://osu.ppy.sh/s/" + obj[i].beatmapset_id; obj[i].beatmap_url = "https://osu.ppy.sh/b/" + obj[i].beatmap_id; }
                }
                obj[i].download = str.Dowload + obj[i].beatmapset_id;
                obj[i].download_no_video = str.Dowload + obj[i].beatmapset_id + "n";
                obj[i].osu_direct = str.Direct + obj[i].beatmapset_id;
                utl.ErrorHandler(obj[i]);
            }

            return obj;
        }

        /// <summary>
        /// Return informations about a user.
        /// </summary>
        /// <param name="u">Specify a user id or a username to return metadata from (required).</param>
        /// <param name="userType">Specify if u is a user_id or a username. Use string for usernames or id for user_ids. Optional, default behaviour is automatic recognition (may be problematic for usernames made up of digits only).</param>
        /// <param name="m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, maps of all modes are returned by default.</param>
        /// <param name="event_days">Max number of days between now and last event date. Range of 1-31. Optional, default value is 1.</param>
        /// <returns>Fetch User.</returns>
        public OsuUser[] GetUser(string u, mode? m = null, typeUser? userType = null, DateTime? event_days = null)
        {
            OsuUser[] obj;
            string html = str.CreateURL(str.User,
                "u", u,
                "m", m,
                "type", userType,
                "event_days",event_days
                );

            obj = JsonConvert.DeserializeObject<OsuUser[]>(utl.GetURL(html));
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
        /// <param name="b">Specify a beatmap_id to return score information from.</param>
        /// <param name="u">Specify a user_id or a username to return score information for.</param>
        /// <param name="m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, default value is 0.</param>
        /// <param name="mods">Specify a mod or mod combination (See https://github.com/ppy/osu-api/wiki#mods )</param>
        /// <param name="userType">Specify if u is a user_id or a username. Use string for usernames or id for user_ids. Optional, default behaviour is automatic recognition (may be problematic for usernames made up of digits only).</param>
        /// <param name="limit">Amount of results from the top (range between 1 and 100 - defaults to 50).</param>
        /// <returns>Fetch Scores.</returns>
        public OsuScore[] GetScore(long b, string u = null, mode? m = null, long? mods = null, typeUser? userType = null, int? limit = null)
        {
            OsuScore[] obj;
            string html = str.CreateURL(str.Scores,
                "b", b,
                "u", u,
                "m", m,
                "mods", mods,
                "type", userType,
                "limit", limit);

            obj = JsonConvert.DeserializeObject<OsuScore[]>(utl.GetURL(html));
            utl.ErrorHandler(obj);

            return obj;
        }

        /// <summary>
        /// Return informations about a user best scores.
        /// </summary>
        /// <param name="u">Specify a user_id or a username to return score information for.</param>
        /// <param name="m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, default value is 0.</param>
        /// <param name="limit">Amount of results from the top (range between 1 and 100 - defaults to 50).</param>
        /// <param name="userType">Specify if u is a user_id or a username. Use string for usernames or id for user_ids. Optional, default behaviour is automatic recognition (may be problematic for usernames made up of digits only).</param>
        /// <returns>Fetch user best scores.</returns>
        public OsuUserBest[] GetUserBest(string u, mode? m = null, int? limit = null, typeUser? userType = null)
        {
            OsuUserBest[] obj;
            string html = str.CreateURL(str.UserBest,
                "u", u,
                "m", m,
                "limit", limit,
                "type", userType);

            obj = JsonConvert.DeserializeObject<OsuUserBest[]>(utl.GetURL(html));
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].accuracy = utl.accuracyCalculator(
                    GetBeatmap(obj[i].beatmap_id, isSet: false),
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
        /// <param name="u">Specify a user_id or a username to return score information for.</param>
        /// <param name="m">Mode (0 = osu!, 1 = Taiko, 2 = CtB, 3 = osu!mania). Optional, default value is 0.</param>
        /// <param name="limit">Amount of results from the top (range between 1 and 100 - defaults to 50).</param>
        /// <param name="userType">Specify if u is a user_id or a username. Use string for usernames or id for user_ids. Optional, default behaviour is automatic recognition (may be problematic for usernames made up of digits only).</param>
        /// <returns>Fetch user recent scores.</returns>
        public OsuUserRecent[] GetUserRecent(string u, mode? m = null, int? limit = null, typeUser? userType = null)
        {
            OsuUserRecent[] obj;
            string html = str.CreateURL(str.UserRecent,
                "u", u,
                "m", m,
                "limit", limit,
                "type", userType);

            obj = JsonConvert.DeserializeObject<OsuUserRecent[]>(utl.GetURL(html));

            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].accuracy = utl.accuracyCalculator(
                    GetBeatmap(obj[i].beatmap_id, isSet: false),
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
        /// <param name="mp">Match id to get information from.</param>
        /// <returns>Fetch multiplayer lobby.</returns>
        public OsuMatch GetMatch(int mp)
        {
            OsuMatch obj;
            string html = str.CreateURL(str.Match,
                "mp", mp);

            obj = JsonConvert.DeserializeObject<OsuMatch>(utl.GetURL(html));

            utl.ErrorHandler(obj);

            return obj;
        }

        /// <summary>
        /// Return informations about a replay.
        /// </summary>
        /// <param name="m">The mode the score was played in.</param>
        /// <param name="b">The beatmap ID (not beatmap set ID!) in which the replay was played.</param>
        /// <param name="u">The user that has played the beatmap.</param>
        /// <param name="userType">Specify if u is a user_id or a username. Use string for usernames or id for user_ids. Optional, default behaviour is automatic recognition (may be problematic for usernames made up of digits only).</param>
        /// <param name="mods">Specify a mod or mod combination (.</param>
        /// <returns>Fetch replay data.</returns>
        public OsuReplay GetReplay(mode m, long b, string u, long? mods = null, typeUser? userType = null)
        {
            OsuReplay obj;
            string html = str.CreateURL(str.Replay,
                "m", m,
                "b", b,
                "u", u,
                "type", userType,
                "mods", mods);

            obj = JsonConvert.DeserializeObject<OsuReplay>(utl.GetURL(html));
            utl.ErrorHandler(obj);
            replayReq++;

            // Add Replay limit to the HttpHandler.
            var nowTime = DateTime.UtcNow;
            if (!bypassReplayLimit) if (replayLimit == replayReq) throw new Exception("Replay limit per minute exceeded");
            if (nowTime >= oldTime + new TimeSpan(0, 0, 1, 0))
            {
                replayReq = 0;
                oldTime = nowTime;
            }
            return obj;
            // Note that the binary data you get when you decode above base64-string, is not the contents of an.osr-file.It is the LZMA stream referred to by the osu-wiki here:
            // The remaining data contains information about mouse movement and key presses in an wikipedia:LZMA stream(https://osu.ppy.sh/wiki/Osr_(file_format)#Format)
        }

        /// <summary>
        /// Return all the bytes needed to create a .osr file.
        /// </summary>
        /// <param name="m">The mode the score was played in.</param>
        /// <param name="b">The beatmap ID (not beatmap set ID!) in which the replay was played.</param>
        /// <param name="u">The user that has played the beatmap.</param>
        /// <param name="mods">Specify a mod or mod combination (See https://github.com/ppy/osu-api/wiki#mods )</param>
        /// <param name="userType">Specify if u is a user_id or a username. Use string for usernames or id for user_ids. Optional, default behaviour is automatic recognition (may be problematic for usernames made up of digits only).</param>
        /// <returns>.osr file bytes.</returns>
        public byte[] GetReplay(mode m, string u, long b, long? mods=null, typeUser? userType = null)
        {
            var replay = GetReplay(m, b, u, mods, userType);
            var sc = GetScore(b,u,m ,mods);
            var bt = GetBeatmap(b, isSet: false);
            var score = sc[0];
            var beatmap = bt[0];

            var binWriter = new BinaryWriter(new MemoryStream());
            var bin = new BinHandler(binWriter);
            var binReader = new BinaryReader(binWriter.BaseStream);

            var replayHashData = bin.MD5Hash(score.maxcombo + "osu" + score.username + beatmap.MD5 + score.score + score.rank);
            var content = Convert.FromBase64String(replay.content);
            var mode = Convert.ToInt32(m).ToString();

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
                bin.writeInteger(                                               // Write enabled mods.           
                    Convert.ToInt32(modsCalculator(                             // Convert mods to int.
                    score.enabled_mods.ToList())));                             // Cast mods array to List.
            }
            bin.writeString("");                                                // Write lifebar hp. (Unknown)
            bin.writeDate(score.date);                                          // Write replay timestamp.
            bin.writeInteger(content.Length);                                   // Write replay content lenght.

            binWriter.Write(content);                                           // Write replay content.

            binWriter.Write(Convert.ToInt64(score.score_id));                   // Write score id.
            binWriter.Write(BitConverter.GetBytes(Convert.ToUInt32(0)), 4, 0);  // Write null byte.

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
        public long? modsCalculator(List<Mods> mods)
        {
            long? flag = 0;
            foreach (var mod in mods)
            { flag += Convert.ToInt64(mod); }
            if (flag == 0) flag = null;
            return flag;
        }
    }
}
