using System;
using System.IO;
using System.Reflection;

namespace DA.Protobuf
{
    public partial class ProtobufConfigData
    {

        #region 
        //public const string Double_ = "double";
        //public const string Float_ = "float";
        //public const string Int32_ = "int32";
        //public const string Int64_ = "int64";
        //public const string UInt32_ = "uint32";
        //public const string UInt64_ = "uint64";
        //public const string SInt32_ = "sint32";
        //public const string SInt64_ = "sint64";
        //public const string Fixed32_ = "fixed32";
        //public const string Fixed64_ = "fixed64";
        //public const string SFixed32_ = "sfixed32";
        //public const string SFixed64_ = "sfixed64";
        //public const string Bool_ = "bool";
        //public const string String_ = "string";
        //public const string Bytes_ = "bytes";

        //public const string DoubleArray = "double[]";
        //public const string FloatArray = "float[]";
        //public const string Int32Array = "int32[]";
        //public const string Int64Array = "int64[]";
        //public const string UInt32Array = "uint32[]";
        //public const string UInt64Array = "uint64[]";
        //public const string SInt32Array = "sint32[]";
        //public const string SInt64Array = "sint64[]";
        //public const string Fixed32Array = "fixed32[]";
        //public const string Fixed64Array = "fixed64[]";
        //public const string SFixed32Array = "sfixed32[]";
        //public const string SFixed64Array = "sfixed64[]";
        //public const string BoolArray = "bool[]";
        //public const string StringArray = "string[]";
        //public const string BytesArray = "bytes[]";

        public readonly string[] VariableType = new[] {
            "double", "float", "int32", "int64", "uint32", "uint64", "sint32", "sint64", "fixed32", "fixed64","sfixed32", "sfixed64", "bool", "string", "bytes",
            "double[]", "float[]", "int32[]", "int64[]", "uint32[]", "uint64[]", "sint32[]", "sint64[]", "fixed32[]", "fixed64[]","sfixed32[]", "sfixed64[]", "bool[]", "string[]", "bytes[]",
        };
        #endregion

        public string RootPath;
        public string GenerateProtoPath;
        public string GenerateScriptPath;
        public string GenerateScriptDllFilePath;
        public string ExcelPath;
        public string ProtobufScriptsPath;

        public string ProtocFilePath;

        public string ProtoDllName = "DA.Generate.Protobuf.dll";

        public string enumWorksheet_ = "enum.";

        public char Split = ';';
        public string IgnoreWorkSheet = "#";

        public string CSNamespace = "DA.Protobuf";

        public int TypeRow = 2;//字段类型在第几行
        public int NameRow = 3;//字段名在第几行
        public int DataRow = 4;//数据在第几行
    }
}