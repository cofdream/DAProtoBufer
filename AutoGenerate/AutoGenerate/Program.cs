using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoGenerate
{
    class Program
    {
        static void Main(string[] args)
        {
            int updateProjectTime = 1000;
            string path = @"E:\Git\Test\TestPull";

            CheckFileChange(Directory.GetFiles(path, "*.xlsx"));

            while (true)
            {
                Console.WriteLine(DateTime.Now.ToString());

                UpdateFiles(path);

                var filePaths = Directory.GetFiles(path, "*.xlsx");
                var changeFiles = CheckFileChange(filePaths);
                if (changeFiles.Length > 0)
                {
                    Console.WriteLine("Generate");
                }
                else
                {
                    Console.WriteLine("Not Generate");
                }

                Thread.Sleep(updateProjectTime);

                Console.WriteLine();
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine();
            }
        }
        private static void UpdateFiles(string path)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();

            process.StandardInput.WriteLine("cd " + path);
            process.StandardInput.AutoFlush = true;


            process.StandardInput.WriteLine("git pull");
            process.StandardInput.AutoFlush = true;

            process.StandardInput.WriteLine("exit");

            Console.WriteLine(process.StandardOutput.ReadToEnd());

            process.WaitForExit();
        }


        private static Dictionary<string, string> fileHashDic = new Dictionary<string, string>();
        private static string[] CheckFileChange(string[] files)
        {
            List<string> changeFiles = new List<string>();
            foreach (var file in files)
            {
                string newFileHash = GetFileHash(file);

                if (fileHashDic.TryGetValue(file, out var fileHash))
                {
                    if (newFileHash.Equals(fileHash))
                    {
                        Console.WriteLine("normal," + file);
                    }
                    else
                    {
                        Console.WriteLine("change," + file);
                        changeFiles.Add(file);
                        fileHashDic[file] = newFileHash;
                    }
                }
                else
                {
                    fileHashDic.Add(file, newFileHash);
                    Console.WriteLine("Add," + file);
                    changeFiles.Add(file);
                }
            }
            return changeFiles.ToArray();
        }

        private static StringBuilder stringBuilder = new StringBuilder();
        private static string GetFileHash(string file)
        {
            stringBuilder.Clear();

            byte[] streamHash;

            var content = File.ReadAllBytes(file);
            MD5 md5 = new MD5CryptoServiceProvider();
            streamHash = md5.ComputeHash(content);

            foreach (var item in streamHash)
            {
                stringBuilder.Append(item.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}
