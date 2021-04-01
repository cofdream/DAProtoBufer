using System;
using UnityEngine;

namespace DAGoogleProto
{
    internal static class Util
    {
        public static Action<string> LogAction { get; set; } = Debug.Log;
        public static Action<string> LogErrorAction { get; set; } = Debug.LogError;
        internal static void Log(string content)
        {
            LogAction?.Invoke(content);
        }

        internal static void LogError(string content)
        {
            LogErrorAction?.Invoke(content);
        }



        private static string cmd_output;
        internal static string CMDNewThreading(object str)
        {
            // 新开线程防止锁死
            var newThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CMD));
            newThread.Start(str);

            return cmd_output;
        }
        private static void CMD(object obj)
        {
            cmd_output = CMD(obj.ToString());
        }

        internal static string CMD(string str)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding("GB2312");
            process.Start();

            process.StandardInput.WriteLine(str);
            process.StandardInput.AutoFlush = true;
            process.StandardInput.WriteLine("exit");

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output;
        }
    }
}
