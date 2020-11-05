using System.IO;
using UnityEngine;

namespace DAProto
{
    internal class CSharpDllExport
    {
        // vs目录下的 csc.exe 的路径 尽量不使用C目录下的，
        // F:\Visual Studio\MSBuild\Current\Bin\Roslyn

        // /out:        // 输出文件路径
        // /recurse:    // 搜索编译源文件的子目录（子目录？不太明白）
        // /reference:  // 从包含集合的文件中导入元数据
        // /target:     // 指定输出文件的格式
        // /doc:        // 把处理的文档注释为XML文件 


        const string vs_csc_Path = @"D:\Program Files\Visual Studio2019\MSBuild\Current\Bin\Roslyn\csc.exe";

        static string cmd = string.Format(@"""{0}""", vs_csc_Path) +
                            " /out:" + ConfigPath.ProtoDll_Path +
                            " /doc:" + Path.Combine(ConfigPath.ProtoDll_Path, "../" + ConfigPath.CSNamespace + ".xml") +
                             " /target:library" +
                            @" /reference:" + ConfigPath.GoogleDll_Path +
                            " /recurse:" + ConfigPath.CSharp_path + "/*.cs";

        public static string Execute()
        {
            Directory.CreateDirectory(Path.Combine(ConfigPath.ProtoDll_Path, "../"));

            if (File.Exists(vs_csc_Path))
            {
                return Util.Cmd(cmd);
            }
            else
            {
                Debug.LogError("csc 文件不存在,请重新配置csc路径。 " + vs_csc_Path);
                return null;
            }
        }
    }
}