using System.Collections.Generic;
using System.IO;
using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.Compilation;
using UnityEditorInternal;

namespace DA.Protobuf
{
    public static class ProtobufTool
    {
        internal static ProtobufConfigData Config { get; private set; }
        static ProtobufTool()
        {
            Util.LogAction += Debug.Log;
            Util.LogErrorAction += Debug.LogError;
            LoadConfig();
        }
        public static void LoadConfig()
        {
            if (File.Exists(ProtobufConfigData.GetAssetPath()))
            {
                Config = AssetDatabase.LoadAssetAtPath<ProtobufConfigData>(ProtobufConfigData.GetAssetPath());
            }
            else
            {
                Config = ScriptableObject.CreateInstance<ProtobufConfigData>();
                AssetDatabase.CreateAsset(Config, ProtobufConfigData.GetAssetPath());
                Config.InitDefautPath();
            }
        }

        public static void LoadAllWorksheet(string excelPath, Action<OfficeOpenXml.ExcelWorksheet> callback)
        {
            if (Directory.Exists(excelPath) == false)
            {
                Util.LogError($"路径不存在：{excelPath}");
                return;
            }

            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            string[] excelFilePaths = Directory.GetFiles(excelPath, "*.xlsx", SearchOption.AllDirectories);
            List<string> excelNames = new List<string>(excelFilePaths.Length);

            foreach (var filePath in excelFilePaths)
                using (var excel = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath)))
                {
                    OfficeOpenXml.ExcelWorksheets worksheets = excel.Workbook.Worksheets;
                    foreach (var worksheet in worksheets)
                    {
                        if (worksheet.Name.StartsWith(Config.IgnoreWorkSheet))
                        {
                            Util.Log($"忽略 {worksheet.Name} ,path: {filePath}");
                            continue;
                        }
                        if (worksheet.Dimension == null)
                        {
                            Util.Log($"{worksheet.Name} 内容为空，path：{filePath}");
                            continue;
                        }
                        foreach (var excelName in excelNames)
                            if (excelName.Equals(worksheet.Name))
                                throw new Exception("存在相同工作簿名称的表: " + worksheet.Name);
                        excelNames.Add(worksheet.Name);

                        callback(worksheet);
                    }
                }
        }
        public static void CheckProtobufAssemblyDefinition()
        {
            string path = "Assets/Protobuf/AutoGenerate/DA.Protobuf.Generate.asmdef";

            AssemblyDefinitionAsset assemblyDefinition = new AssemblyDefinitionAsset("");
            assemblyDefinition.name = "Test";
            EditorUtility.SetDirty(assemblyDefinition);
            AssetDatabase.ImportAsset(path);

            
            //assemblyDefinition.name = "DA.Protobuf.Generate";
        string a =     "{
    "name": "DA.Protobuf.Generate",
    "references": [
        "GUID:2a4ff60c96c7b82418286dd7e8fd55fa"
    ],
    "includePlatforms": [],
    "excludePlatforms": [],
    "allowUnsafeCode": false,
    "overrideReferences": false,
    "precompiledReferences": [],
    "autoReferenced": false,
    "defineConstraints": [],
    "versionDefines": [],
    "noEngineReferences": false
}"
        }
    }
}