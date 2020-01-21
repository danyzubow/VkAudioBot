using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using VK_API.AudioHandler.FFMpeg;
using VK_API.VkWrapper.Accounts;
using VkAudioBot.VkWrapper.Models;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VK_API.VkWrapper
{
    public class AudioProvider
    {
        private VkApi vkApi;

        #region privateMethodVkNet.AudioBypassService

        private void Login(VkApi vkApi)
        {
            try
            {
                vkApi.Authorize(new ApiAuthParams()
                {
                    AccessToken = "ec09efb9564b29ffe6a8887c5abbc8cf396613386200fcf2ba66dc13decdb50487aab0de2d3e3aef2a63b",
                    //   ApplicationId = 123456,
                    //   Login = GlobalSettings.Account.Login,
                    //  Password = GlobalSettings.Account.Password,
                    //   Settings = Settings.All,
                });
            }
            catch (CaptchaNeededException cEx)
            {
                Console.WriteLine(cEx);
                throw;
            }

        }
        #endregion

        #region Public prop

        public Uri GetM3u8()
        {
            VkCollection<Audio> v = vkApi.Audio.Get(new AudioGetParams() { AccessKey = "id556153348" });
            Audio audio = v[0];
            Console.WriteLine(audio.Url.AbsolutePath);
            return audio.Url;
        }

        public List<AudioInfoModel> GetPlayList(string vkId)
        {
            VkCollection<Audio> pList = vkApi.Audio.Get(new AudioGetParams() { AccessKey = vkId });
            return pList.ToList().Select(a =>
                  new AudioInfoModel(a.Title, a.Artist, ((a.Url != null) && (!string.IsNullOrEmpty(a.Url.AbsoluteUri))), pList.IndexOf(a))).ToList();
        }

        public string GetPlayListJson(string vkId)
        {
            return JsonSerializer.Serialize(GetPlayList(vkId));
        }

        public void Download(Uri m3u8)
        {
            string subPath = "a1_tmp";
            string path = Path.Combine(Def.Tmp, subPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            AudioWebService.Load(m3u8, subPath);
            FFMpegWrapper.ConvertM3u8ToMp3(subPath);
        }

        public void DownloadFFMpeg(Uri m3u8, bool useProxy)
        {
            string proxy = useProxy ? $"-http_proxy {ProxyProvider.GetCurrentProxy(true)}" : "";

            RealAction(@"tools\ffmpeg.exe",
                "-threads 0 " + proxy + " -i " + "\"" + m3u8.AbsoluteUri + "\"" + " -c copy -y -f mpegts " + "\"" + "music.ts" + "\"");
        }

        private void RealAction(string StartFileName, string StartFileArg)
        {
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = StartFileName;
            CmdProcess.StartInfo.Arguments = StartFileArg;

            CmdProcess.StartInfo.CreateNoWindow = true;
            CmdProcess.StartInfo.UseShellExecute = false;
            CmdProcess.StartInfo.RedirectStandardInput = true;
            CmdProcess.StartInfo.RedirectStandardOutput = true;
            CmdProcess.StartInfo.RedirectStandardError = true;
            //CmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;  

            //CmdProcess.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            //CmdProcess.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

            CmdProcess.EnableRaisingEvents = true;                      // 启用Exited事件  
                                                                        // CmdProcess.Exited += new EventHandler(CmdProcess_Exited);   // 注册进程结束事件  

            CmdProcess.Start();
            //  ffmpegid = CmdProcess.Id;//获取ffmpeg.exe的进程ID
            CmdProcess.BeginOutputReadLine();
            CmdProcess.BeginErrorReadLine();

            // 如果打开注释，则以同步方式执行命令，此例子中用Exited事件异步执行。  
            // CmdProcess.WaitForExit();       

        }

        #endregion
        public AudioProvider()
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

            IServiceCollection container = new ServiceCollection();
            if (GlobalSettings.UseProxy) ProxyProvider.ConfigurateProxy(container);
            container.AddAudioBypass();
            vkApi = new VkApi(container);
            Login(vkApi);
        }

    }
}
