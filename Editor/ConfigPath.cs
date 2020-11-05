using System.IO;
using UnityEngine;

namespace DAProto
{
    public static class ConfigPath
    {
        /// <summary>
        /// Excel 存放路径
        /// </summary>
        public static string Excel_Path = null;

        /// <summary>
        /// Proto 解释文件路径
        /// </summary>
        public static string Proto_Path = null;

        /// <summary>
        /// 生成的脚本根路径
        /// </summary>
        public static string Script_Path = null;

        public static string CSharp_path = null;
        public static string Cpp_Path = null;
        public static string Go_Path = null;
        public static string Java_Path = null;
        public static string Python_Path = null;

        /// <summary>
        /// 生成的数据文件路径
        /// </summary>
        public static string Data_Path = null;

        /// <summary>
        /// cs和Goole.Protobuf脚本的dll路径
        /// </summary>
        public static string ProtoDll_Path = null;

        /// <summary>
        /// 生成proto文件的工具
        /// </summary>
        public static string ProtoExe_Path = null;

        /// <summary>
        /// Google.Protobuf的cs文件
        /// </summary>
        public static string GoogleProtoCS_Path = null;

        public static string GoogleDll_Path = null;

        public const string CSNamespace = "DAProto";
    }
}
