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

		internal static string Cmd(string str)
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
