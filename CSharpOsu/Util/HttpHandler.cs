using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CSharpOsu.Util
{
    internal class HttpHandler
    {
        public HttpClient client = new HttpClient();
        public bool throwIfNull = false;
        public bool bypassLimit = false;
        public short limit = 60;
        public TimeSpan timeInterval = new TimeSpan(0, 0, 1, 0);

        private short requestCount = 0;
        private DateTime oldTime = DateTime.UtcNow;
        public HttpHandler() {}

        public string GetURL(string url)
        {
            var nowTime = DateTime.UtcNow;
            if (!bypassLimit) if (requestCount == limit) throw new Exception("Requests limit exceeded");
            try
            {
                var response = client.GetAsync(url);
                var json = response.Result.Content.ReadAsStringAsync().Result;
                if (throwIfNull) if (json == "[]") throw new Exception("No objects have been found for those arguments");

                requestCount++;
                if (nowTime >= oldTime + timeInterval)
                { requestCount = 0; oldTime = nowTime; }

                return json;
            }
            catch (WebException ex){throw new WebException(ex.Message);}
        }
    }
}
