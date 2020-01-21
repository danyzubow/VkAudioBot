using System;
using System.Diagnostics;
using System.IO;

namespace VK_API.AudioHandler.FFMpeg
{
    class FFMpegWrapper
    {
        private const string FFMpegWin = "Tools\\FFMpeg\\ffmpeg.exe";
        private const string FFMpegUnix = "Tools/FFMpeg/ffmpeg";
        private const string FileOutTs = "audio.ts";
        private const string FileOutMP3 = "out.mp3";
        private const string ArgExtractAudioFile = " -i \"{0}\" -acodec copy \"{1}\"";
        //private const string ArgOneM4aFile = " -i {0} -acodec aac {1}";
        private const int WaitForExitMilisec = 60000; //60sec

        public static bool ConvertM3u8ToMp3(string subPath)
        {
            string workFolder = Path.Combine(Def.Tmp, subPath);
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = GlobalSettings.IsUnix ? FFMpegUnix : FFMpegWin;
            CmdProcess.StartInfo.Arguments = string.Format(ArgExtractAudioFile, Path.Combine(workFolder, Def.IndexM3u8),
                Path.Combine(workFolder, FileOutMP3));

            //CmdProcess.StartInfo.CreateNoWindow = true;
            // CmdProcess.StartInfo.UseShellExecute = false;
            // CmdProcess.StartInfo.RedirectStandardInput = true;
            // CmdProcess.StartInfo.RedirectStandardOutput = true;
            CmdProcess.StartInfo.RedirectStandardError = true;
            // CmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            //    CmdProcess.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            //    CmdProcess.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

            //  CmdProcess.EnableRaisingEvents = true;
            //   CmdProcess.Exited += new EventHandler(CmdProcess_Exited);   

            CmdProcess.Start();
            if (CmdProcess.WaitForExit(WaitForExitMilisec))
            {
                return true;
                //CmdProcess.StartInfo.Arguments = string.Format(ArgOneM4aFile, Path.Combine(workFolder, FileOutTs),
                //    Path.Combine(workFolder, FileOutMP3));
                //CmdProcess.Start();
                //if (CmdProcess.WaitForExit(WaitForExitMilisec))
                //{
                //    return true;
                //}
            }

            return false;
            //     ffmpegid = CmdProcess.Id;
            //   CmdProcess.BeginOutputReadLine();
            //    CmdProcess.BeginErrorReadLine();
        }

        public static void Initialize()
        {
            Process process = new Process()
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
            string result = process.StandardOutput.ReadToEnd();
            Console.WriteLine(result);


        }
    }
}
