using System.IO;

namespace DAGoogleProto
{
    public class GenertatScript
    {
        public enum ScriptType
        {
            None = 0,
            Cpp = 1 << 0,
            CSharp = 1 << 1,
            Go = 1 << 2,
            Java = 1 << 3,
            Python = 1 << 4,
        }

        //-I 是--proto_path 的缩写

        public string cSharpCmdTemplate = "{0} -I={1} --csharp_out={2} {3} --csharp_opt=file_extension=.cs";

        public string cppCmdTemplate = @"{0} -I={1} --cpp_out={2} {3}";

        public string pythonCmdTemplate = @"{0} -I={1} --python_out={2} {3}";

        public string gonCmdTemplate = @"{0} -I={1} --objc_out={2} {3}";

        public string javaCmdTemplate = @"{0} -I={1} --java_out={2} {3}";

        public void GenerateScripts(ScriptType scriptType)
        {
            DAGoogleProtoConfigData config = GoogleProtoTool.Config;
            if ((scriptType & ScriptType.CSharp) != 0)
            {
                GenerateScripts(cSharpCmdTemplate, config.ProtocFilePath, config.GenerateProtoPath, config.GenerateScriptPath);
            }
        }
        public void GenerateScripts(string template, string protocFilePath, string protoPath, string scriptSavePath)
        {
            string[] files = Directory.GetFiles(protoPath, "*.proto");
            foreach (string filePath in files)
            {
                GenerateScriptByProto(template, protocFilePath, protoPath, scriptSavePath, filePath);
            }
        }
        public void GenerateScriptByProto(string template, string protocFilePath, string protoPath, string scriptSavePath, string protoFilePath)
        {
            string cmd = string.Format(template, protocFilePath, protoPath, scriptSavePath, protoFilePath);
            Util.Log(Util.CMD(cmd));
        }
    }
}