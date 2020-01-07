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
        private const string FileOutM4A = "out.m4a";
        private const string ArgOneTsFile = " -i \"{0}\" -c copy -y -f mpegts \"{1}\"";
        private const string ArgOneM4aFile = " -i {0} -acodec aac {1}";
        private const int WaitForExitMilisec = 60000; //60sec
        public static bool ConvertM3u8ToM4a(string subPath)
        {
            string workFolder = Path.Combine(Def.Tmp, subPath);
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = GlobalSettings.IsUnix ? FFMpegUnix : FFMpegWin;
            CmdProcess.StartInfo.Arguments = string.Format(ArgOneTsFile, Path.Combine(workFolder, Def.IndexM3u8), Path.Combine(workFolder, FileOutTs));
            Console.WriteLine(CmdProcess.StartInfo.FileName);
            Console.WriteLine(CmdProcess.StartInfo.Arguments);
            CmdProcess.StartInfo.CreateNoWindow = true;
            CmdProcess.StartInfo.UseShellExecute = false;
            CmdProcess.StartInfo.RedirectStandardInput = true;
            CmdProcess.StartInfo.RedirectStandardOutput = true;
            CmdProcess.StartInfo.RedirectStandardError = true;
            CmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            //    CmdProcess.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            //    CmdProcess.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

            CmdProcess.EnableRaisingEvents = true;
            //   CmdProcess.Exited += new EventHandler(CmdProcess_Exited);   

            CmdProcess.Start();
            if (CmdProcess.WaitForExit(WaitForExitMilisec))
            {
                CmdProcess.StartInfo.Arguments = string.Format(ArgOneM4aFile, Path.Combine(workFolder, FileOutTs), Path.Combine(workFolder, FileOutM4A));
                CmdProcess.Start();
                if (CmdProcess.WaitForExit(WaitForExitMilisec))
                {
                    return true;
                }
            }
            return false;
            //     ffmpegid = CmdProcess.Id;
            //   CmdProcess.BeginOutputReadLine();
            //    CmdProcess.BeginErrorReadLine();
        }


    }
}
