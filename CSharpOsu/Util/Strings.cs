using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpOsu.Util.Enums;

namespace CSharpOsu.Util.Strings
{
    internal class Strings
    {
        public string Key { get; internal set; }

        protected virtual string osuApiUrl => "https://osu.ppy.sh/api/";
        public string osuDowload = "https://osu.ppy.sh/d/";
        public string osuDirect = "osu://s/";

        public string osuBeatmap = "get_beatmaps?";
        public string osuScores = "get_scores?";
        public string osuUser = "get_user?";
        public string osuUserBest = "get_user_best?";
        public string osuUserRecent = "get_user_recent?";
        public string osuMatch = "get_match?";
        public string osuReplay = "get_replay?";

        public string Beatmap() { return osuApiUrl + osuBeatmap + "k=" + Key; }
        public string User(string id) { return osuApiUrl + osuUser + "k=" + Key + "&u=" + id; }
        public string Score(long beatmap) { return osuApiUrl + osuScores + "k=" + Key + "&b=" + beatmap; }
        public string User_Best(string user) { return osuApiUrl + osuUserBest + "k=" + Key + "&u=" + user; }
        public string User_Recent(string user) { return osuApiUrl + osuUserRecent + "k=" + Key + "&u=" + user; }
        public string Match(int match) { return osuApiUrl + osuMatch + "k=" + Key + "&mp=" + match; }
        public string Replay(mode mode, long beatmap, string user) { return osuApiUrl + osuReplay + "k=" + Key + "&m=" + (int)mode + "&b=" + beatmap + "&u=" + user; }

        // Thanks to Game4all and his circles.NET project (https://github.com/Game4all/circles.NET)
        // Not implemented yet.
        internal string CreateURL(string endpoint, params object[] queryStrings)
        {
            var sb = new StringBuilder();
            sb.Append(osuApiUrl);
            sb.Append(endpoint);

            for (int i = 0; i < queryStrings.Length; i += 2) //query strings are given this way [0] = QueryStringName, [1] = QueryStringValue
            {
                if (i == 0) //first parameter should always be API key
                {
                    sb.Append($"{queryStrings[i].ToString()}={queryStrings[i + 1].ToString()}");
                    continue;
                }

                if (queryStrings[i + 1] != null) //if the query string value is != null , let's add it to the url
                    sb.Append($"&{queryStrings[i].ToString()}={queryStrings[i + 1].ToString()}");
            }
            return sb.ToString();
        }
    }
}
