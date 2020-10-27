using System.IO;

namespace DAProtoBuf
{
    class ScriptGenerate
    {
        //-I 是--proto_path 的缩写

        const string cSharpCmdTemplate = @"{0} -I={1} --csharp_out={2} {3} --csharp_opt=file_extension=.pb.cs";

        const string cppCmdTemplate = @"{0} -I={1} --cpp_out={2} {3}";

        const string pythonCmdTemplate = @"{0} -I={1} --python_out={2} {3}";

        const string gonCmdTemplate = @"{0} -I={1} --objc_out={2} {3}";

        const string javaCmdTemplate = @"{0} -I={1} --java_out={2} {3}";

        static ScriptGenerate()
        {
            string protoPath = ConfigPath.Proto_Path;
            if (Directory.Exists(protoPath) == false)
            {
                Directory.CreateDirectory(protoPath);
            }
        }

        public static void Execute(ScriptType scriptType)
        {
            if ((scriptType & ScriptType.CSharp) != 0)
            {
                Generate(cSharpCmdTemplate, ConfigPath.CSharp_path);
            }

            if ((scriptType & ScriptType.Cpp) != 0)
            {
                Generate(cppCmdTemplate, ConfigPath.Cpp_Path);
            }

            if ((scriptType & ScriptType.Python) != 0)
            {
                Generate(pythonCmdTemplate, ConfigPath.Python_Path);
            }

            if ((scriptType & ScriptType.Go) != 0)
            {
                Generate(gonCmdTemplate, ConfigPath.Go_Path);
            }

            if ((scriptType & ScriptType.Java) != 0)
            {
                Generate(javaCmdTemplate, ConfigPath.Java_Path);
            }
        }

        private static void Generate(string template, string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            string[] files = Directory.GetFiles(ConfigPath.Proto_Path, "*.proto", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var filePath = files[i];

                string cmd = string.Format(template, ConfigPath.ProtoExe_Path, ConfigPath.Proto_Path, path, filePath);
                Util.Cmd(cmd);
            }
        }

        public enum ScriptType
        {
            None = 0,
            Cpp = 1 << 0,
            CSharp = 1 << 1,
            Go = 1 << 2,
            Java = 1 << 3,
            Python = 1 << 4,
        }
    }
}
