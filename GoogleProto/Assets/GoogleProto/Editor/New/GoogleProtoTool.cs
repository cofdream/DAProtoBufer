using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System;


namespace DAGoogleProto
{
    public static class GoogleProtoTool
    {
        internal static DAGoogleProtoConfigData Config { get; private set; }
        static GoogleProtoTool()
        {
            LoadConfig();
        }
        public static void LoadConfig()
        {
            string ConfigPath;
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(GoogleProtoTool).Assembly);

            if (packageInfo != null)// Package环境
                ConfigPath = packageInfo.assetPath;
            else
                ConfigPath = "Assets/GoogleProto";
            string assetPath = ConfigPath + "/DAGoogleProtoConfigData.asset";
            // 加载文件
            if (File.Exists(assetPath))
            {
                Config = AssetDatabase.LoadAssetAtPath<DAGoogleProtoConfigData>(assetPath);
            }
            else
            {
                Config = ScriptableObject.CreateInstance<DAGoogleProtoConfigData>();
                InitDefautPath(Config);
                AssetDatabase.CreateAsset(Config, assetPath);    
                AssetDatabase.ImportAsset(assetPath);
            }
        }


        internal static void InitDefautPath(DAGoogleProtoConfigData config)
        {
            config.RootPath = Directory.GetParent(Application.dataPath).FullName + "/DAGoogleProto";
            if (Directory.Exists(config.RootPath) == false) Directory.CreateDirectory(config.RootPath);

            config.GenerateProtoPath = config.RootPath + "/AutoGenerate/Proto";
            if (Directory.Exists(config.GenerateProtoPath) == false) Directory.CreateDirectory(config.GenerateProtoPath);

            config.ExcelPath = config.RootPath + "/Excel";
            if (Directory.Exists(config.ExcelPath) == false) Directory.CreateDirectory(config.ExcelPath);

            config.GenerateScriptPath = config.RootPath + "/Script";
            if (Directory.Exists(config.GenerateScriptPath) == false) Directory.CreateDirectory(config.GenerateScriptPath);

            config.GenerateScriptDllPath = Application.dataPath + "/GoogleProto/AutoGenerate/Dll";
            if (Directory.Exists(config.GenerateScriptDllPath) == false) Directory.CreateDirectory(config.GenerateScriptDllPath);


            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(GoogleProtoTool).Assembly);
            if (packageInfo != null)// Package环境
                config.ProtocFilePath = packageInfo.resolvedPath + "/.ProtoTool/protoc-3.8.0-win64/protoc.exe";
            else
                config.ProtocFilePath = Application.dataPath + "/GoogleProto/.ProtoTool/protoc-3.8.0-win64/protoc.exe";


            EditorUtility.SetDirty(Config);
        }
        public static void CheckConfigPath()
        {
            if (Directory.Exists(Config.RootPath) == false) Directory.CreateDirectory(Config.RootPath);
            if (Directory.Exists(Config.GenerateProtoPath) == false) Directory.CreateDirectory(Config.GenerateProtoPath);
            if (Directory.Exists(Config.ExcelPath) == false) Directory.CreateDirectory(Config.ExcelPath);
            if (Directory.Exists(Config.GenerateScriptPath) == false) Directory.CreateDirectory(Config.GenerateScriptPath);
            if (Directory.Exists(Config.GenerateScriptDllPath) == false) Directory.CreateDirectory(Config.GenerateScriptDllPath);
            if (File.Exists(Config.ProtocFilePath) == false) Debug.LogError($"Protoc:\" {Config.ProtocFilePath} \"File path not exists!");
        }

        public static void LoadAllWorksheet(string excelPath, Action<OfficeOpenXml.ExcelWorksheet> callback)
        {
            if (Directory.Exists(excelPath) == false)
            {
                Debug.LogError($"路径不存在：{excelPath}");
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
                        foreach (var excelName in excelNames)
                            if (excelName.Equals(worksheet.Name))
                                throw new Exception("存在相同工作簿名称的表: " + worksheet.Name);
                        excelNames.Add(worksheet.Name);

                        callback(worksheet);
                    }
                }
        }
    }
}