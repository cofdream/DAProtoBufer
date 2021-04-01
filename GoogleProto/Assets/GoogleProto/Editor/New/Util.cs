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



        internal static void CMDNewThreading(string str)
        {
            // 新开线程防止锁死
            var newThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CMD));
            newThread.Start(str);
        }
        private static void CMD(object obj)
        {
            Log(CMD(obj as string));
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

            string output = "cmd:\n" + str + "\n\n" + process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output;
        }
    }
}
