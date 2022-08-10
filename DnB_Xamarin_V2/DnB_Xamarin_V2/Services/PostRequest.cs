using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace DnB_Xamarin_V2.Services
{
    internal sealed class PostRequest
    {
        private HttpWebRequest webRequest;
        private string urlAddress;

        public Dictionary<string, string> Headers { get; set; }

        public string Response { get; set; }
        public string Accept { get; set; }
        public string Host { get; set; }
        public string Data { get; set; }
        public string ContentType { get; set; }
        public WebProxy Proxy { get; set; }
        public string Referer { get; set; }
        public string UserAgent { get; set; }

        public PostRequest(string address)
        {
            urlAddress = address;
            Headers = new Dictionary<string, string>();
        }

        public async void Run(CookieContainer cookieContainer)
        {
            webRequest = (HttpWebRequest)WebRequest.Create(urlAddress);
            webRequest.CookieContainer = cookieContainer;
            webRequest.Method = "Post";
            webRequest.Proxy = Proxy;
            webRequest.UserAgent = UserAgent;
            webRequest.Accept = Accept;
            webRequest.Host = Host;
            webRequest.ContentType = ContentType;
            webRequest.Referer = Referer;


            using (Stream stream = webRequest.GetRequestStream())
            {
                byte[] sentData = Encoding.UTF8.GetBytes(Data);
                stream.Write(sentData, 0, sentData.Length);
            }

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
                        }
                    }
                }

                webResponse.Close();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Class: PostRequest, Method: Run / Error", ex.Message, "Ok");
            }
            finally
            {
                if (webResponse != null)
                    webResponse.Close();
            }
        }
    }
}