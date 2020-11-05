using System.IO;
using UnityEngine;

namespace DAProto
{
    internal static class ConfigPath
    {
        /// <summary>
        /// Excel 存放路径
        /// </summary>
        internal static string Excel_Path = null;

        /// <summary>
        /// Proto 解释文件路径
        /// </summary>
        internal static string Proto_Path = null;

        /// <summary>
        /// 生成的脚本根路径
        /// </summary>
        internal static string Script_Path = null;

        internal static string CSharp_path = null;
        internal static string Cpp_Path = null;
        internal static string Go_Path = null;
        internal static string Java_Path = null;
        internal static string Python_Path = null;

        /// <summary>
        /// 生成的数据文件路径
        /// </summary>
        internal static string Data_Path = null;

        /// <summary>
        /// cs和Goole.Protobuf脚本的dll路径
        /// </summary>
        internal static string ProtoDll_Path = null;

        /// <summary>
        /// 生成proto文件的工具
        /// </summary>
        internal static string ProtoExe_Path = null;

        /// <summary>
        /// Google.Protobuf的cs文件
        /// </summary>
        internal static string GoogleProtoCS_Path = null;

        internal static string GoogleDll_Path = null;

        internal const string CSNamespace = "DAProto";

        public static void InitConfigPath(string projectPath)
        {
            string path = projectPath + @"\BuildProto";

            Excel_Path = path + @"\Excel";

            Proto_Path = path + @"\Generate\Proto";

            Script_Path = path + @"\Generate\Script";

            CSharp_path = Script_Path + @"\cs";
            Cpp_Path = Script_Path + @"\cpp";
            Go_Path = Script_Path + @"\go";
            Java_Path = Script_Path + @"\java";
            Python_Path = Script_Path + @"\python";

            // 数据文件存放路径
            Data_Path = projectPath + @"\Assets\Resources\DataConfig";

            // dll生成路径
            ProtoDll_Path = projectPath + @"\Assets\Pulagin\DAProtobuf\" + CSNamespace + ".dll";
            // googledll的路径
            GoogleDll_Path = projectPath + @"\Assets\Pulagin\GoogoleProtobuf_3.8.0\Google.Protobuf.dll";

            // protoc.exe
            ProtoExe_Path = projectPath + @"\Assets\.ProtoTool\protoc-3.8.0-win64\protoc.exe";
        }
    }
}
