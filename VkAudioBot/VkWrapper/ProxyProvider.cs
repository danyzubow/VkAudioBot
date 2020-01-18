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
       // static string proxtUrl = "http://82.208.72.215:9650"; //амстердам
        static string proxtUrl = "http://91.191.250.142:31059";
        private static WebProxy proxy;

        private static string[] proxtUrls =
        {
            "167.99.57.138	:8080      ",
            "167.71.101.134	:3128      ",
            "165.22.9.55	:3128      ",
            "198.50.177.44	:44699     ",
            "68.183.147.115	:8080      ",
            "167.71.242.25	:3128      ",
            "68.183.111.90	:80        ",
            "167.99.114.19	:3128      ",
            "167.71.167.227	:3128      ",
            "165.22.186.89	:80        ",
            "192.99.203.93	:35289     ",
            "167.99.4.200	:8080      ",
            "134.209.126.148:	8080      ",
            "165.22.180.241	:3128      ",
            "149.56.1.48	:8181          ",
            "54.39.53.104	:3128      ",
            "159.65.182.70	:8888      ",
            "162.243.25.182	:11591     ",
            "3.84.27.209	:8080          ",
            "165.22.33.143	:8080      ",
            "167.71.176.9	:1080      ",
            "159.65.244.25	:8080      ",
            "68.183.155.113	:8080	  ",
            "68.183.195.222	:8080      ",
            "198.50.145.28	:80        ",
            "142.44.242.38	:8888      ",
            "51.79.65.157	:3128      ",
            "34.74.43.175	:8080      ",
            "198.50.152.64	:23500     ",
            "51.79.24.36	:8080          ",
            //"188.130.255.5  : 80    ",
            //"188.130.255.10 : 80   ",
            //"158.255.51.212 : 56759",
            //"188.130.255.8  : 80   ",
            //"188.32.48.236  : 8081 ",
            //"188.130.255.20 : 80   ",
            //"217.168.76.230 : 33032",
            //"188.130.255.13 : 80   ",
            //"95.161.177.249 : 8081 ",
            //"81.3.140.131   : 4145 ",
            //"195.34.15.98   : 59098",
            //"80.76.240.168  : 31964",
            //"217.168.76.77  : 33032",
            //"83.219.150.189 : 4145 ",
            //"188.130.255.14 : 80   ",
            //"109.111.157.76 : 4145 ",
            //"188.130.255.6  : 80   ",
            //"83.219.139.66  : 4145 ",
            //"83.219.156.231 : 4145 ",
            //"195.211.219.47 : 30847",
            //"178.213.82.162 : 4145 ",
            //"83.219.149.44  : 4145 ",
            //"89.175.153.158 : 4145 ",
            //"188.130.255.17 : 80   ",
            //"83.219.139.93  : 4145 ",
            //"83.219.143.254 : 4145 ",
            //"83.219.149.199 : 4145 ",
            //"188.130.255.15 : 80   ",
            //"89.175.129.145 : 46716",
            //"178.176.192.37 : 4145 ",
            //"176.32.177.30  : 51870"
        };
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
