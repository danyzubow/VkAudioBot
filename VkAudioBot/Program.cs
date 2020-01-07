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
            if (GlobalSettings.IsUnix)
            {
                accountJson = Environment.GetEnvironmentVariable("Account");
            }
            else
            {
                using (StreamReader reader = new StreamReader("Account.txt"))
                {
                    accountJson = reader.ReadToEnd();
                }
            }
            GlobalSettings.Account = JsonSerializer.Deserialize<Account>(accountJson);

            AudioProvider audioProvider = new AudioProvider();
            audioProvider.Download(audioProvider.GetM3u8());


        }
    }
}
