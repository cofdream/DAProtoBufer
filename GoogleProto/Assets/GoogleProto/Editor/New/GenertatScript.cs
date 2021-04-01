using System.IO;

namespace DAGoogleProto
{
    public class GenertatScript
    {
        private const string cSharpCmdTemplate = "{0} -I={1} --csharp_out={2} {3} --csharp_opt=file_extension=.pb.cs";
        public enum ScriptType
        {
            None = 0,
            Cpp = 1 << 0,
            CSharp = 1 << 1,
            Go = 1 << 2,
            Java = 1 << 3,
            Python = 1 << 4,
        }

        public static void Execute(ScriptType scriptType)
        {
            DAGoogleProtoConfigData config = GoogleProtoTool.Config;
            if ((scriptType & ScriptType.CSharp) != 0)
            {
                GenerateProtos(cSharpCmdTemplate, config.ProtocFilePath, config.GenerateProtoPath, config.GenerateScriptPath);
            }
        }
        public static void GenerateProtos(string template, string protocFilePath, string protoPath, string scriptSavePath)
        {
            string[] files = Directory.GetFiles(protoPath, "*.proto");
            foreach (string filePath in files)
            {
                GenerateProto(template, protocFilePath, protoPath, scriptSavePath, filePath);
            }
        }
        public static void GenerateProto(string template, string protocFilePath, string protoPath, string scriptSavePath, string protoFilePath)
        {
            string cmd = string.Format(template, protocFilePath, protoPath, scriptSavePath, protoFilePath);
            Util.Log(Util.CMDNewThreading(cmd));
        }
    } 
}