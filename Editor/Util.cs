
using System;

namespace DAProto
{
    internal static class Util
    {
        private static Action<string> log = Console.WriteLine;
        /// <summary>
        /// 日志打印回调
        /// </summary>
        public static Action<string> LogAction { set { log = value; } }
        internal static void Log(string content)
        {
            log(content.ToString());
        }

        internal static string Cmd(string str)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();

            process.StandardInput.WriteLine(str);
            process.StandardInput.AutoFlush = true;
            process.StandardInput.WriteLine("exit");

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            // UnityEngine.Debug.Log(output);
            return output;
        }
    }
}
