using System;
using System.IO;
using System.Text.Json;
using VK_API.VkWrapper;
using VK_API.VkWrapper.Accounts;

namespace VK_API
{
    class Program
    {
        static void Main(string[] args)
        {
            string accountJson;
#if DEBUG
            using (StreamReader reader = new StreamReader("Account.txt"))
            {
                accountJson = reader.ReadToEnd();
            }
#else
            accountJson = Environment.GetEnvironmentVariable("Account");
#endif
            GlobalSettings.Account = JsonSerializer.Deserialize<Account>(accountJson);

            AudioProvider audioProvider = new AudioProvider();
            audioProvider.Download(audioProvider.GetM3u8());


        }
    }
}
