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

        private const string ConfigPath = "Assets/GoogleProto/Data/DAGoogleProtoConfigData.asset";
        internal static DAGoogleProtoConfigData Config { get; private set; }
        static GoogleProtoTool()
        {
            LoadConfig();
        }
        public static void LoadConfig()
        {
            // 加载文件
            if (File.Exists(ConfigPath))
            {
                Config = AssetDatabase.LoadAssetAtPath<DAGoogleProtoConfigData>(ConfigPath);
            }
            else
            {
                if (Directory.Exists(ConfigPath) == false)
                {
                    Directory.CreateDirectory(ConfigPath);
                }
                Config = ScriptableObject.CreateInstance<DAGoogleProtoConfigData>();
                InitDefautPath(Config);
                AssetDatabase.CreateAsset(Config, ConfigPath);
                AssetDatabase.ImportAsset(ConfigPath);
            }
        }

        private static void InitDefautPath(DAGoogleProtoConfigData config)
        {
            config.RootPath = Directory.GetParent(Application.dataPath).FullName + "/DAGoogleProto";
            if (Directory.Exists(config.RootPath) == false) Directory.CreateDirectory(config.RootPath);

            config.GenerateProtoPath = config.RootPath + "/AutoGenerate/Proto";
            if (Directory.Exists(config.GenerateProtoPath) == false) Directory.CreateDirectory(config.GenerateProtoPath);

            config.ExcelPath = config.RootPath + "/Excel";
            if (Directory.Exists(config.ExcelPath) == false) Directory.CreateDirectory(config.ExcelPath);


        }
        public static void CheckConfigPath()
        {
            if (Directory.Exists(Config.RootPath) == false) Directory.CreateDirectory(Config.RootPath);
            if (Directory.Exists(Config.GenerateProtoPath) == false) Directory.CreateDirectory(Config.GenerateProtoPath);
            if (Directory.Exists(Config.ExcelPath) == false) Directory.CreateDirectory(Config.ExcelPath);
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