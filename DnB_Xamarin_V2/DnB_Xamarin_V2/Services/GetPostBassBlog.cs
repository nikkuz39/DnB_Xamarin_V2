using DnB_Xamarin_V2.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DnB_Xamarin_V2.Services
{
    internal sealed class GetPostBassBlog
    {
        private static WebProxy proxy = new WebProxy("127.0.0.1:8888");
        private static CookieContainer getCookieContainer = new CookieContainer();

        private static CookieContainer postCookieContainer = new CookieContainer();
        private static string tokenId = "";
        private static string lastSongId = "";

        public async Task<List<Song>> GetDataBassBlogFeatured()
        {
            GetRequest getRequest = new GetRequest("https://bassblog.pro/featured");
            getRequest.Accept = "*/*";
            getRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36";
            getRequest.Referer = "https://bassblog.pro/featured";
            getRequest.Host = "bassblog.pro";
            //getRequest.Proxy = proxy;

            getRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            getRequest.Headers.Add("Accept-Language", "ru,en;q=0.9,en-GB;q=0.8,en-US;q=0.7");

            getRequest.Headers.Add("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"102\", \"Google Chrome\";v=\"102\"");
            getRequest.Headers.Add("sec-ch-ua-mobile", "?0");
            getRequest.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            getRequest.Headers.Add("Sec-Fetch-Dest", "document");
            getRequest.Headers.Add("Sec-Fetch-Mode", "navigate");
            getRequest.Headers.Add("Sec-Fetch-Site", "none");
            getRequest.Headers.Add("Sec-Fetch-User", "?1");
            getRequest.Headers.Add("Upgrade-Insecure-Requests", "1");

            SetCookies setCookies = new SetCookies();
            getCookieContainer = setCookies.CookieGetBassBlog();

            getRequest.Run(getCookieContainer);

            List<Song> songs = new List<Song>(30);

            if (getRequest.Response != null)
            {
                postCookieContainer = getRequest.ResponseCookie;
                GetTokenId(getRequest.Response);

                songs = await GetListSongs(getRequest.Response);

                return songs;
            }

            return songs;
        }

        public async Task<List<Song>> PostDataBassBlogFeatured()
        {
            PostRequest postRequest = new PostRequest("https://bassblog.pro/requests/load_explore.php?featured");
            postRequest.Data = $"start={lastSongId}&token_id={tokenId}";
            postRequest.Accept = "*/*";
            postRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36";
            postRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            postRequest.Referer = "https://bassblog.pro/featured";
            postRequest.Host = "bassblog.pro";
            //postRequest.Proxy = proxy;

            postRequest.Headers.Add("Origin", "https://bassblog.pro");
            postRequest.Headers.Add("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"102\", \"Google Chrome\";v=\"102\"");
            postRequest.Headers.Add("sec-ch-ua-mobile", "?0");
            postRequest.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            postRequest.Headers.Add("Sec-Fetch-Dest", "empty");
            postRequest.Headers.Add("Sec-Fetch-Mode", "cors");
            postRequest.Headers.Add("Sec-Fetch-Site", "same-origin");

            postRequest.Run(postCookieContainer);

            List<Song> songs = new List<Song>(30);

            if (postRequest.Response != null)
            {
                songs = await GetListSongs(postRequest.Response);

                return songs;
            }

            return songs;
        }

        private async Task<List<Song>> GetListSongs(string request)
        {
            List<Song> songList = new List<Song>(30);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(request);

            try
            {
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//div[@class='song-container']"))
                {
                    Song song = new Song();

                    song.IdSong = node.SelectSingleNode(".//div[@data-track-name]").GetAttributeValue("data-track-id", "");
                    song.ImageUrl = node.SelectSingleNode(".//div//a//img").GetAttributeValue("src", "");
                    song.NameSong = node.SelectSingleNode(".//div//a//img").GetAttributeValue("alt", "");
                    song.SongUrl = node.SelectSingleNode(".//div[@data-track-name]").GetAttributeValue("data-track-url", "");
                    song.MusicalStyle = node.SelectSingleNode(".//div[@class='song-tag']//a[@href]").InnerHtml;
                    song.TotalTime = node.SelectSingleNode(".//div[@class='jp-total-time']").InnerHtml;

                    songList.Add(song);
                }

                lastSongId = songList.LastOrDefault().IdSong;

                return songList;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Class: GetPostBassBlog, Method: GetListSongs / Error", ex.Message, "Ok");
            }

            return songList;
        }

        private async void GetTokenId(string request)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(request);

            try
            {
                tokenId = GetSubstring(htmlDoc.DocumentNode.SelectSingleNode("//script[@type='text/javascript']").InnerText, "token_id = \"", "\";");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Class: GetPostBassBlog, Method: GetTokenId / Error", ex.Message, "Ok");
            }
        }

        private string GetSubstring(string stringSource, string stringStart, string stringEnd)
        {
            if (stringSource.Contains(stringStart) && stringSource.Contains(stringEnd))
            {
                int startIndex = 0;
                int endIndex = 0;

                startIndex = stringSource.IndexOf(stringStart, 0) + stringStart.Length;
                endIndex = stringSource.IndexOf(stringEnd, startIndex);

                return stringSource.Substring(startIndex, endIndex - startIndex);
            }

            return "";
        }
    }
}