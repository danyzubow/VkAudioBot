using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace VK_API.VkWrapper
{
    static class ProxyProvider
    {
        // static string proxtUrl = "http://82.208.72.215:9650"; //амстердамC:\Projects\VK_API\VkAudioBot\VkWrapper\ProxyProvider.cs
        //  public static string proxtUrl = "http://171.25.255.187:8080";
         public static string proxtUrl = "http://89.189.172.88:8080";
        // public static string proxtUrl = "http://195.9.183.26:8080";
        //   public static string proxtUrl = "http://91.216.164.251:80";

       // public static string proxtUrl = "https://frpxa.com:443";


        //  public static string proxtUrl = "http://217.23.7.11:8971";


        private static WebProxy proxy;

        public static WebProxy Proxy
        {
            get => proxy != null ? proxy : new WebProxy(proxtUrl);
        }

        public static string GetCurrentProxy(bool fullUrl)
        {
            if (fullUrl) return proxtUrl;
            else return proxtUrl.Split("//").Last();
        }

        public static void ConfigurateProxy(IServiceCollection container)
        {
            container.AddSingleton<IWebProxy, WebProxy>(provider => Proxy);
            FlurlHttp.Configure(settings =>
            {
                settings.HttpClientFactory = new ProxyHttpClientFactory(proxtUrl);
            });
        }
        public class ProxyHttpClientFactory : DefaultHttpClientFactory
        {
            private string _address;

            public ProxyHttpClientFactory(string address)
            {
                _address = address;
            }

            public override HttpMessageHandler CreateMessageHandler()
            {
                return new HttpClientHandler
                {
                    Proxy = new WebProxy(_address),
                    UseProxy = true
                };
            }
        }
    }
}
