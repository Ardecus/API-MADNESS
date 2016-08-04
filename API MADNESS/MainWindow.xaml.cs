using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Windows.Controls;
using System;

namespace API_MADNESS
{
    [DataContract]
    public class Context
    {
        [DataMember]
        public int id;
        [DataMember]
        public string name;
        [DataMember]
        public int startTimeSeconds;
        [DataMember]
        public string phase;
    }

    [DataContract]
    public class Responce
    {
        [DataMember]
        public string status;
        [DataMember]
        public List<Context> result;
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HttpWebRequest request = WebRequest.CreateHttp("http://codeforces.com/api/contest.list");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return;
                }

                Stream dataStream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(Responce));
                Responce res = (Responce)serializer.ReadObject(dataStream);

                foreach (var item in res.result)
                {
                    if (item.phase != "BEFORE")
                    {
                        break;
                    }

                    DateTime starts = DateFromUnix(item.startTimeSeconds);

                    ContentControl challenge = new ContentControl();
                    challenge.Tag = item.name;
                    challenge.Content = "Starts " + starts.ToShortDateString() + " at " + starts.ToShortTimeString();
                    challenge.ContentTemplate = (DataTemplate)FindResource("CodeforcesTemplate");

                    CodeforcesList.Children.Add(challenge);
                }
            }
        }

        public static DateTime DateFromUnix(int unixTimeStamp)
        {
            return DateTime.MinValue.AddSeconds(unixTimeStamp).AddDays(-1).ToLocalTime();
        }
    }
}
