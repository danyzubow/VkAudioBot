using System;
using System.Diagnostics;
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
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"ls\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            Console.WriteLine(result);

            process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"chmod 777 Tools/FFMpeg/ffmpeg\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            result = process.StandardOutput.ReadToEnd();
            Console.WriteLine(result);
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
