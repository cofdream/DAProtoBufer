using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DAGoogleProto
{
    public class GenerateDll
    {
        public static void CompleDll()
        {
            string path = @"E:\Git\DAProtoBufer\GoogleProto\Assets\GoogleProto\.ProtoTool\Google.Protobuf.3.8.0";

            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.WarningLevel = 3;

            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");

            parameters.OutputAssembly = path + "/DAGoogleProto.dll";

            var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);

            CompilerResults results = codeDomProvider.CompileAssemblyFromFile(parameters, files);

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError CompErr in results.Errors)
                {
                    Debug.LogError("Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine);
                }
            }

            var files2 = Directory.GetFiles(@"C:\Users\v_cqqcchen\Desktop\TestUnity\DAGoogleProto\Script", "*.cs", SearchOption.AllDirectories);
            parameters.OutputAssembly = path + "/DAProto.dll";

            List<string> temp = new List<string>();
            temp.AddRange(files);
            temp.AddRange(files2);


            CompilerResults results2 = codeDomProvider.CompileAssemblyFromFile(parameters, temp.ToArray());

            if (results2.Errors.Count > 0)
            {
                foreach (CompilerError CompErr in results2.Errors)
                {
                    Debug.LogError("Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine);
                }
            }
        }
    }
}
