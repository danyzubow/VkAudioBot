using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using VK_API.VkWrapper;
using VK_API.VkWrapper.Accounts;

namespace VK_API
{
    class Program
    {
        static void Main(string[] args)
        {

            AudioProvider audioProvider = new AudioProvider();
            audioProvider.Download(audioProvider.GetM3u8());

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
