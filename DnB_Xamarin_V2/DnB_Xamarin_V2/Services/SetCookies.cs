using System.Net;

namespace DnB_Xamarin_V2.Services
{
    internal sealed class SetCookies
    {
        public CookieContainer CookieGetBassBlog()
        {
            CookieContainer cookieContainer = new CookieContainer();

            cookieContainer.Add(new Cookie("lang", "english") { Domain = "bassblog.pro" });
            cookieContainer.Add(new Cookie("_jsuid", "2965100633") { Domain = "bassblog.pro" });
            cookieContainer.Add(new Cookie("sc_https%3A%2F%2Fbassblog.pro%2Fwelcome", "0") { Domain = "bassblog.pro" });
            cookieContainer.Add(new Cookie("sc_https%3A%2F%2Fbassblog.pro%2Ffeatured", "3792") { Domain = "bassblog.pro" });

            return cookieContainer;
        }
    }
}