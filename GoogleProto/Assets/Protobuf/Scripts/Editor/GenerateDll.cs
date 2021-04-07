using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

namespace DA.Protobuf
{
    public class GenerateDll
    {
        public void CompleDll(string dllName)
        {

            string protobufScriptPath = Util.Config.ProtobufScriptsPath;
            string outputPath = Util.Config.GenerateScriptDllFilePath + "/" + dllName;
            string generateScriptPath = Util.Config.GenerateScriptPath;

            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.WarningLevel = 3;

            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");

            parameters.OutputAssembly = outputPath;


            var protobufScriptPaths = Directory.GetFiles(protobufScriptPath, "*.cs", SearchOption.AllDirectories);
            var generateScriptPaths = Directory.GetFiles(generateScriptPath, "*.cs", SearchOption.AllDirectories);
            List<string> temp = new List<string>(protobufScriptPaths.Length + generateScriptPaths.Length);
            temp.AddRange(protobufScriptPaths);
            temp.AddRange(generateScriptPaths);


            CompilerResults results2 = codeDomProvider.CompileAssemblyFromFile(parameters, temp.ToArray());

            if (results2.Errors.Count > 0)
            {
                foreach (CompilerError CompErr in results2.Errors)
                {
                    Util.LogError("Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine);
                }
            }
        }

    }
}
