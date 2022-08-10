using System.Collections.Generic;
using System.IO;
using System.Net;
using Xamarin.Forms;

namespace DnB_Xamarin_V2.Services
{
    internal sealed class GetRequest
    {
        private HttpWebRequest webRequest;
        private string urlAddress;
        public CookieContainer ResponseCookie;

        public Dictionary<string, string> Headers { get; set; }

        public string Response { get; set; }
        public string Accept { get; set; }
        public string Host { get; set; }
        public WebProxy Proxy { get; set; }
        public string Referer { get; set; }
        public string UserAgent { get; set; }

        public GetRequest(string address)
        {
            urlAddress = address;
            ResponseCookie = new CookieContainer();
            Headers = new Dictionary<string, string>();
        }

        public async void Run(CookieContainer cookieContainer)
        {
            webRequest = (HttpWebRequest)WebRequest.Create(urlAddress);
            webRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            webRequest.CookieContainer = cookieContainer;
            webRequest.Method = "Get";
            webRequest.Proxy = Proxy;
            webRequest.UserAgent = UserAgent;
            webRequest.Accept = Accept;
            webRequest.Host = Host;
            webRequest.Referer = Referer;


            foreach (var header in Headers)
            {
                webRequest.Headers.Add(header.Key, header.Value);
            }


            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            try
            {
                using (Stream stream = webResponse.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            Response = reader.ReadToEnd();

                            foreach (Cookie cookie in webResponse.Cookies)
                            {
                                ResponseCookie.Add(new Cookie(cookie.Name, cookie.Value) { Domain = "bassblog.pro" });
                            }
                        }
                    }
                }

                webResponse.Close();
            }
            catch (WebException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Class: GetRequest, Method: Run / Error", ex.Message, "Ok");
            }
            finally
            {
                if (webResponse != null)
                    webResponse.Close();
            }
        }
    }
}