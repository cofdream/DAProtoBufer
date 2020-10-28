using UnityEngine;
using UnityEditor;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using ICSharpCode.SharpZipLib.Checksum;
using System;

namespace DAProtoBuf
{
    public static class ZipTool
    {  
        public static void Decompress(string compressFile, string decompressDir, string password, Func<string, bool> overWrite)
        {
            FileStream compressFileStream = File.OpenRead(compressFile);

            decompressDir = Directory.CreateDirectory(decompressDir).FullName;// 不去判断目录是否存在，目录不存在会在本地路径下去创建，这会导致路径错误。

            using (ZipInputStream zipInputStream = new ZipInputStream(compressFileStream))
            {
                while (true)
                {
                    ZipEntry zipEntry = zipInputStream.GetNextEntry();
                    if (zipEntry == null)
                        break;

                    string filePath = Path.Combine(decompressDir, zipEntry.Name);
                    string directory = Path.GetDirectoryName(filePath);

                    Directory.CreateDirectory(directory);

                    if (zipEntry.Name.EndsWith("/"))//不是文件夹
                        continue;

                    if (File.Exists(filePath) && overWrite(filePath) == false)
                    {
                        continue;
                    }

                    using (FileStream fileStream = File.Create(filePath))
                    {
                        byte[] data = new byte[4096];

                        while (true)
                        {
                            int size = zipInputStream.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                fileStream.Write(data, 0, size);
                            }
                            else
                                break;
                        }
                    }
                }
            }
        }
    }
}