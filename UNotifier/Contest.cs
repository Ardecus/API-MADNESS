using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UNotifier;
using UNotifier.Properties;

namespace Codeforces
{
    [DataContract]
    public class Contest : INotifyPropertyChanged
    {
        const string newEntry = "Silver",
                     watchedEntry = "White";

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
                return watched ? watchedEntry : newEntry;
            }
            set
            {
                watched = value == watchedEntry;
                OnPropertyChanged("background");
            }
        }

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

                bool reached = false;

                foreach (var item in responce.result)
                {
                    if (item.phase != "BEFORE")
                    {
                        break;
                    }

                    if (!reached)
                    {
                        reached = item.id == Settings.Default.LastCodeforcesId;
                    }
                    item.watched = reached;
                    result.Add(item);
                }

                Settings.Default.LastCodeforcesId = responce.result[0].id;
                Settings.Default.Save();
            }

            return result;
        }
        
        public void SetWatched()
        {
            background = watchedEntry;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
