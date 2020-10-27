
namespace DAProtoBuf
{
    internal static class Util
    {
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
