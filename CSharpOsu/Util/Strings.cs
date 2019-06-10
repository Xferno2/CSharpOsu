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
        public Strings(string key){
            Key = key;
        }

        public string Key { get; internal set; }
        protected virtual string APIUrl => "https://osu.ppy.sh/api/";
        public string Dowload = "https://osu.ppy.sh/d/";
        public string Direct = "osu://s/";

        public string Beatmap = "get_beatmaps?";
        public string Scores = "get_scores?";
        public string User = "get_user?";
        public string UserBest = "get_user_best?";
        public string UserRecent = "get_user_recent?";
        public string Match = "get_match?";
        public string Replay = "get_replay?";

        // Thanks to Game4all and his circles.NET project (https://github.com/Game4all/circles.NET)
        internal string CreateURL(string endpoint, params object[] queryStrings)
        {
            var sb = new StringBuilder();
            sb.Append(APIUrl);
            sb.Append(endpoint);
            sb.Append("k=" + Key);
            for (int i = 0; i < queryStrings.Length; i += 2) //query strings are given this way [0] = QueryStringName, [1] = QueryStringValue
            {
                if (queryStrings[i + 1] != null) //if the query string value is != null , let's add it to the url
                    sb.Append($"&{queryStrings[i].ToString()}={queryStrings[i + 1].ToString()}");
            }
            return sb.ToString();
        }
    }
}
