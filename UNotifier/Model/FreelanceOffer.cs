using AngleSharp.Parser.Html;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UNotifier.Properties;

namespace Freelance
{
    public class FreelanceOffer : INotifyPropertyChanged
    {
        public string Link { get; set; }
        public string Theme { get; set; }
        public string Details { get; set; }
        public string Price { get; set; }

        public string TitleString
        {
            get
            {
                return $"{Theme} - {Price}";
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static async Task<List<FreelanceOffer>> GetOffers()
        {
            string data = await GetHtml().ConfigureAwait(false);
            return await ParseHtml(data).ConfigureAwait(false);
        }

        private static async Task<string> GetHtml()
        {
            string urlAddress = "http://freelance.ua/?pc=1";
            string data = "";
            
            try
            {
                var request = WebRequest.CreateHttp(urlAddress);
                using (var response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false))
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return data;
                    }

                    Stream receiveStream = response.GetResponseStream();
                    using (StreamReader readStream = response.CharacterSet == null ? new StreamReader(receiveStream) :
                                                                                     new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet)))
                    {
                        data = await readStream.ReadToEndAsync().ConfigureAwait(false);
                    }
                }
            }
            catch { }
            return data;
        }

        private static async Task<List<FreelanceOffer>> ParseHtml(string source)
        {
            var result = new List<FreelanceOffer>();
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(source).ConfigureAwait(false);
            
            var proposals = document.GetElementsByClassName("j-order");
            bool reached = false;

            foreach (var item in proposals)
            {
                try
                {
                    FreelanceOffer offer = new FreelanceOffer();
                    var title = item.GetElementsByClassName("l-project-title").First().GetElementsByTagName("a").First();
                    offer.Theme = title.InnerHtml;
                    offer.Link = title.GetAttribute("href");
                    offer.Price = item.GetElementsByClassName("l-price_na").Concat(item.GetElementsByClassName("l-price")).First().InnerHtml;
                    offer.Details = item.GetElementsByTagName("p").First().InnerHtml;

                    if (!reached)
                    {
                        reached = offer.Theme == Settings.Default.LastFreelanceTheme;
                    }

                    offer.Watched = reached;

                    result.Add(offer);
                }
                catch
                {
                    continue;
                }
            }

            Settings.Default.LastFreelanceTheme = result.FirstOrDefault().Theme;
            Settings.Default.Save();

            return result;
        }

        public void SetWatched()
        {
            Watched = true;
        }
    }
}
