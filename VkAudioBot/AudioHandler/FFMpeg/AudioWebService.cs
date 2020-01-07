using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VK_API.VkWrapper;

namespace VK_API.AudioHandler.FFMpeg
{
    class AudioWebService
    {
        private const string headerM3U8 = "#EXTM3U";

        public static void Load(Uri uri, string tmpSubPath)
        {
            string baseUrl = uri.AbsoluteUri.Split(Def.IndexM3u8).First();
            Dictionary<string, byte[]> bufferTs = new Dictionary<string, byte[]>();
            using (var client = new HttpClient(new SocketsHttpHandler()
            {
                Proxy = GlobalSettings.UseProxy ? ProxyProvider.Proxy : null
            }))
            {
                Task<string> data = client.GetStringAsync(uri.AbsoluteUri);
                StringBuilder indexBuilder = new StringBuilder(headerM3U8 + "\n");
                string[] lines = data.Result.Split('\n');
                if (lines.Any())
                {
                    const int defaultTargetDuration = 100;
                    var targetDuration = defaultTargetDuration;
                    if (lines[0] != headerM3U8)
                    {
                        throw new InvalidOperationException("The provided URL does not link to a well-formed M3U8 playlist.");
                    }
                    int keyFileNumber = 1;
                    int tsFileNumber = 1;
                    for (var i = 1; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        if (line.StartsWith("#"))
                        {
                            string lineData = line.Substring(1);

                            string[] split = lineData.Split(':');

                            string name = split[0];
                            string value = "";
                            if (split.Length > 1)
                            {
                                value = split[1];
                            }

                            if (line.StartsWith("#EXT-X-KEY:METHOD=AES-128,URI="))
                            {
                                string keyUrl = line.Split("\"")[1];
                                Task<byte[]> bufKey = client.GetByteArrayAsync(keyUrl);
                                string keyFile = Path.Combine(Def.Tmp, tmpSubPath, $"key[{ keyFileNumber++}].ts");
                                using (FileStream file = new FileStream(keyFile, FileMode.Create))
                                {
                                    file.Write(bufKey.Result);
                                }

                                indexBuilder.Append(line.Split("\"")[0]);
                                indexBuilder.Append($"\"{(GlobalSettings.IsUnix ? keyFile : keyFile.Replace("\\", "\\\\"))}\"\n");
                                continue;
                            }

                            switch (name)
                            {
                                case "EXTINF":
                                    string nextLine = lines[i + 1];
                                    if (!bufferTs.ContainsKey(nextLine))
                                    {
                                        Task<byte[]> bytes = client.GetByteArrayAsync(baseUrl + nextLine);
                                        string fileTs = Path.Combine(Def.Tmp, tmpSubPath, $"{tsFileNumber++}.ts");
                                        bufferTs.Add(fileTs, bytes.Result);
                                        indexBuilder.Append($"{line}\n");
                                        indexBuilder.Append($"{fileTs}\n");
                                    }
                                    break;
                                case "EXT-X-TARGETDURATION":
                                    if (targetDuration == defaultTargetDuration)
                                    {
                                        targetDuration = int.Parse(value);
                                    }
                                    indexBuilder.Append($"{line}\n");
                                    break;
                                default:
                                    indexBuilder.Append($"{line}\n");
                                    break;
                                    //case "EXT-X-ALLOW-CACHE":
                                    //    break;


                                    //case "EXT-X-VERSION":
                                    //    break;

                                    //case "EXT-X-MEDIA-SEQUENCE":
                                    //    break;
                            }
                        }
                    }
                    //wait for a new part of the stream to appear if we're lucky.
                    Task.Delay(targetDuration * 1000 / 2);
                }
                else
                {
                    throw new InvalidOperationException("The provided URL does not contain any data.");
                }
                foreach (KeyValuePair<string, byte[]> ts in bufferTs)
                {
                    using (FileStream fileTs = new FileStream(ts.Key, FileMode.Create))
                    {
                        fileTs.Write(ts.Value);
                    }
                }

                using (StreamWriter writer = new StreamWriter(Path.Combine(Def.Tmp, tmpSubPath, Def.IndexM3u8), false))
                {
                    writer.Write(indexBuilder);
                }

            }
        }
    }
}
