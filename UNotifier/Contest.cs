using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UNotifier;

namespace Codeforces
{
    [DataContract]
    public class Contest
    {
        [DataMember] public int id { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public int startTimeSeconds { get; set; }
        [DataMember] public string phase { get; set; }

        public string startDate
        {
            get
            {
                DateTime date = (new DateTime(1970, 1, 1)).AddSeconds(startTimeSeconds).ToLocalTime();
                return $"Начинается {date.ToShortDateString()} в {date.ToShortTimeString()}";
            }
        }

        public bool watched;

        public string background
        {
            get
            {
                return watched ? "White" : "Silver";
            }
        }
        /*
        public Contest(int id, string name, int startTimeSeconds, string phase)
        {
            this.id = id;
            this.name = name;
            this.startTimeSeconds = startTimeSeconds;
            this.phase = phase;
        }
        */

        public static List<Contest> GetContests()
        {
            List<Contest> result = new List<Contest>();

            var request = WebRequest.CreateHttp("http://codeforces.com/api/contest.list");
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception();
                }

                var dataStream = response.GetResponseStream();
                var serializer = new DataContractJsonSerializer(typeof(JsonResponce<Contest>));

                var responce = (JsonResponce<Contest>)serializer.ReadObject(dataStream);
                foreach (var item in responce.result)
                {
                    if (item.phase != "BEFORE")
                    {
                        break;
                    }

                    item.watched = false;
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
