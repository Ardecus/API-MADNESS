using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using UNotifier.Properties;

namespace Codeforces
{
    [DataContract]
    public class CodeforcesContest : INotifyPropertyChanged
    {
        [DataContract]
        private class JsonResponse<T>
        {
            [DataMember] public string status;
            [DataMember] public List<T> result;
        }

        [DataMember] public int id { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public int startTimeSeconds { get; set; }
        [DataMember] public string phase { get; set; }

        public string Name { get { return name; } }

        private bool watched;
        public bool Watched
        {
            get
            {
                return watched;
            }
            set
            {
                watched = value;
                OnPropertyChanged($"{nameof(Watched)}");
            }
        }
        
        public string StartDateString
        {
            get
            {
                DateTime date = (new DateTime(1970, 1, 1)).AddSeconds(startTimeSeconds).ToLocalTime();
                return $"Начинается {date.ToShortDateString()} в {date.ToShortTimeString()}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static async Task<List<CodeforcesContest>> GetContests()
        {
            var result = new List<CodeforcesContest>();

            try
            {
                var request = WebRequest.CreateHttp("http://codeforces.com/api/contest.list");
                using (var response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result;
                    }

                    var serializer = new DataContractJsonSerializer(typeof(JsonResponse<CodeforcesContest>));
                    var received = (JsonResponse<CodeforcesContest>)serializer.ReadObject(response.GetResponseStream());
                    bool reached = false;

                    foreach (var contest in received.result)
                    {
                        if (contest.phase != "BEFORE")
                        {
                            break;
                        }

                        if (!reached)
                        {
                            reached = contest.id == Settings.Default.LastCodeforcesId;
                        }

                        contest.Watched = reached;

                        result.Add(contest);
                    }

                    Settings.Default.LastCodeforcesId = received.result.FirstOrDefault().id;
                    Settings.Default.Save();
                }
            }
            catch { }

            return result;
        }

        public void SetWatched()
        {
            Watched = true;
        }
    }
}
