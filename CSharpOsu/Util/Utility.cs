using CSharpOsu.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using CSharpOsu.Util.Enums;
using Newtonsoft.Json;

namespace CSharpOsu.Util
{
    internal class Utility
    {
        HttpClient client;
        private bool throwIfNull;

        public Utility(HttpClient? _client, bool _throwIfNull) { client = _client; throwIfNull = _throwIfNull; }
        public string GetUrl(string url)
        {
            try
            {
                var json = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                if (throwIfNull)
                    if (json == "[]") { throw new Exception("No objects have been found for those arguments"); }
                return json;
            }
            catch (WebException ex)
            {
                throw new WebException(ex.Message);
            }
        }

        public void ErrorHandler(Model obj) {
            if (obj.error != null)
            {
                throw new Exception(obj.error);
            }
        }

        public void ErrorHandler(Model[] objs)
        {
            foreach (var obj in objs)
            {
                if (obj.error != null)
                {
                    throw new Exception(obj.error);
                }
            }
        }

        public float accuracyCalculator(OsuBeatmap[] bt, long count50, long count100, long count300, long countmiss, long countkatu,long countgeki)
        {
            var mapMode = (mode)Convert.ToInt32(bt[0].mode);
            float totalPointsOfHits;
            float totalNumberOfHits;
            float accuracy;
            // Logical switch for every mode
            switch (mapMode)
            {
                case mode.osu:
                    totalPointsOfHits = (count50) * 50 + (count100) * 100 + (count300) * 300;
                    totalNumberOfHits = (countmiss) + (count50) + (count100) + (count300);

                    accuracy = totalPointsOfHits / (totalNumberOfHits * 300);
                    break;

                case mode.Taiko:
                    totalPointsOfHits = ((count100) * 0.5f + (count300) * 1) * 300;
                    totalNumberOfHits = (countmiss) + (count100) + (count300);

                    accuracy = totalPointsOfHits / (totalNumberOfHits * 300);
                    break;

                case mode.CtB:
                    totalPointsOfHits = (count50 + count100 + count300);
                    totalNumberOfHits = (countmiss) + (count50) + (count100) + (count300) + (countkatu);

                    accuracy = totalPointsOfHits / totalNumberOfHits;
                    break;

                case mode.osuMania:
                    totalPointsOfHits = (count50) * 50 + (count100) * 100 + (countkatu) * 200 + (count300 + countgeki) * 300;
                    totalNumberOfHits = (countmiss) + (count50) + (count100) + (countkatu) + (count300) + (countgeki);

                    accuracy = totalPointsOfHits / (totalNumberOfHits * 300);
                    break;

                default:
                    accuracy = 0;
                    break;
            }
            return (accuracy * 100);
        }
        }
    }
