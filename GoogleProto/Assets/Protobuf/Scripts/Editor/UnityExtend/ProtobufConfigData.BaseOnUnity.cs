using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DA.Protobuf
{
    public partial class ProtobufConfigData : ScriptableObject
    {
        public static string GetConfigPath()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(ProtobufConfigData).Assembly);
            if (packageInfo != null)
                return packageInfo.assetPath;
            else
                return "Assets/Protobuf";
        }
        public static string GetAssetPath()
        {
            return GetConfigPath() + "/DA.ProtobufConfigData.asset";
        }


        public void InitDefautPath()
        {
            RootPath = Directory.GetParent(Application.dataPath).FullName + "/DAGoogleProto";
            if (Directory.Exists(RootPath) == false) Directory.CreateDirectory(RootPath);

            GenerateProtoPath = RootPath + "/AutoGenerate/Proto";
            if (Directory.Exists(GenerateProtoPath) == false) Directory.CreateDirectory(GenerateProtoPath);

            ExcelPath = RootPath + "/Excel";
            if (Directory.Exists(ExcelPath) == false) Directory.CreateDirectory(ExcelPath);

            GenerateScriptPath = Application.dataPath + "/Protobuf/AutoGenerate/Script"; 
            if (Directory.Exists(GenerateScriptPath) == false) Directory.CreateDirectory(GenerateScriptPath);

            GenerateScriptDllFilePath = Application.dataPath + "/Protobuf/AutoGenerate";
            if (Directory.Exists(GenerateScriptDllFilePath) == false) Directory.CreateDirectory(GenerateScriptDllFilePath);
            

            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(Util).Assembly);

            if (packageInfo != null)
                ProtocFilePath = packageInfo.resolvedPath + "/.protoc-3.8.0-win64/protoc.exe";
            else
                ProtocFilePath = Application.dataPath + "/Protobuf/.protoc-3.8.0-win64/protoc.exe";

            if (packageInfo != null)
                ProtobufScriptsPath = packageInfo.resolvedPath + "/Scripts/Runtime/protobuf3.8.0";
            else
                ProtobufScriptsPath = Application.dataPath + "/Protobuf/Scripts/Runtime/protobuf3.8.0";

            AssetDatabase.ImportAsset(GenerateScriptPath);
            AssetDatabase.ImportAsset(GenerateScriptDllFilePath);


            AssetDatabase.ImportAsset(GetAssetPath());
        }
        public void CheckConfigPath()
        {
            if (Directory.Exists(RootPath) == false) Directory.CreateDirectory(RootPath);
            if (Directory.Exists(GenerateProtoPath) == false) Directory.CreateDirectory(GenerateProtoPath);
            if (Directory.Exists(ExcelPath) == false) Directory.CreateDirectory(ExcelPath);
            if (Directory.Exists(GenerateScriptPath) == false) Directory.CreateDirectory(GenerateScriptPath);
            if (Directory.Exists(GenerateScriptDllFilePath) == false) Directory.CreateDirectory(GenerateScriptDllFilePath);
            if (File.Exists(ProtocFilePath) == false) Util.LogError($"Protoc:\" {ProtocFilePath} \"file path not exists!");
            if (Directory.Exists(ProtobufScriptsPath) == false) Util.LogError($"protobuf script:\" {ProtobufScriptsPath} \"file path not exists!");

            AssetDatabase.ImportAsset(GenerateScriptPath);
            AssetDatabase.ImportAsset(GenerateScriptDllFilePath);
        }
    }
}