using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VK_API.VkWrapper
{
    class WebService
    {
        public static void DownloadAllTs(string url, string TmpPath, bool useProxy)
        {
            string baseUrl = url.Split("index.m3u8").First();
            Dictionary<string, byte[]> buffer = new Dictionary<string, byte[]>();
            using (var client = new HttpClient(new SocketsHttpHandler()
            {
                Proxy = useProxy ? ProxyProvider.Proxy : null
            }))
            {
                int keyNumber = 1;

                Task<string> data = client.GetStringAsync(url);

                var lines = data.Result.Split('\n');
                if (lines.Any())
                {
                    const int defaultTargetDuration = 100;
                    var targetDuration = defaultTargetDuration;

                    var firstLine = lines[0];
                    if (firstLine != "#EXTM3U")
                    {
                        throw new InvalidOperationException(
                            "The provided URL does not link to a well-formed M3U8 playlist.");
                    }

                    for (var i = 1; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        if (line.StartsWith("#"))
                        {
                            var lineData = line.Substring(1);

                            var split = lineData.Split(':');

                            var name = split[0];
                            string value = "";
                            if (split.Length > 1)
                            {
                                value = split[1];
                            }

                            if (line.StartsWith("#EXT-X-KEY:METHOD=AES-128,URI="))
                            {
                                string keyUrl = line.Split("\"")[1];
                                Task<byte[]> bufKey = client.GetByteArrayAsync(keyUrl);
                                using (FileStream file = new FileStream($"ts\\key[{keyNumber}].ts", FileMode.Create))
                                {
                                    keyNumber++;
                                    file.Write(bufKey.Result);
                                }
                            }

                            switch (name)
                            {
                                case "EXT-X-TARGETDURATION":
                                    if (targetDuration == defaultTargetDuration)
                                    {
                                        targetDuration = int.Parse(value);
                                    }
                                    break;

                                //oh, how sweet. a header for us to entirely ignore. we'll always use cache.
                                case "EXT-X-ALLOW-CACHE":
                                    break;


                                case "EXT-X-VERSION":
                                    break;

                                case "EXT-X-MEDIA-SEQUENCE":
                                    break;

                                case "EXTINF":
                                    var nextLine = lines[i + 1];
                                    if (!buffer.ContainsKey(nextLine))
                                    {

                                        var bytes = client.GetByteArrayAsync(baseUrl + nextLine);
                                        string str = Encoding.ASCII.GetString(bytes.Result);
                                        //   Console.WriteLine(str);
                                        buffer.Add(nextLine, bytes.Result);
                                    }
                                    break;
                            }
                        }
                    }

                    //wait for a new part of the stream to appear if we're lucky.
                    Task.Delay(targetDuration * 1000 / 2);
                }
                else
                {
                    throw new InvalidOperationException(
                        "The provided URL does not contain any data.");
                }
                List<byte> buf = new List<byte>();

                int index = 1;
                foreach (byte[] item in buffer.Values)
                {

                    foreach (byte it in item)
                    {
                        buf.Add(it);
                    }
                    using (FileStream file = new FileStream($"ts\\{index}.ts", FileMode.Create))
                    {
                        file.Write(buf.ToArray());
                    }
                    buf.Clear();
                    index++;
                }
            }
        }
    }
}
