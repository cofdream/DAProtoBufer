using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DA.Protobuf
{
    public class DAProtoWindow : EditorWindow
    {
        [MenuItem("Tools/DA")]
        static void OpenWindow()
        {
            GetWindow<DAProtoWindow>().Show();
        }

        private void OnEnable()
        {
            ProtobufTool.Config.CheckConfigPath();
        }
        private void OnGUI()
        {
            if (GUILayout.Button("Load Config"))
            {
                ProtobufTool.LoadConfig();
            }
            if (GUILayout.Button("Init Proto"))
            {
                ProtobufTool.Config.InitDefautPath();
            }
            if (GUILayout.Button("Generate Proto"))
            {
                ProtobufTool.LoadAllWorksheet(ProtobufTool.Config.ExcelPath, new GenerateProto().GenerateProtoFiles);
            }
            if (GUILayout.Button("Generate Script"))
            {
                new GenertatScript().GenerateScripts(GenertatScript.ScriptType.CSharp);
            }
            if (GUILayout.Button("Generate Dll"))
            {
                new GenerateDll().CompleDll(ProtobufTool.Config.ProtoDllName);

                var dataPath = Application.dataPath;
                if (ProtobufTool.Config.GenerateScriptDllFilePath.StartsWith(dataPath))
                {
                    var dllAssetPath = ProtobufTool.Config.GenerateScriptDllFilePath.Substring(dataPath.Length - 6);
                    AssetDatabase.ImportAsset(dllAssetPath + $"/{ProtobufTool.Config.ProtoDllName}");
                    Debug.Log(dllAssetPath);
                }
            }
            if (GUILayout.Button("Generate Data"))
            {
                ProtobufTool.LoadAllWorksheet(ProtobufTool.Config.ExcelPath, new GenerateProtoData().GenerateData);
            }


            if (GUILayout.Button("Test"))
            {
                ProtobufTool.CheckProtobufAssemblyDefinition();
            }

        }
    }
}
