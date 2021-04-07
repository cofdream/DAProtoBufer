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
            Util.Config.CheckConfigPath();
        }
        private void OnGUI()
        {
            if (GUILayout.Button("Load Config"))
            {
                Util.LoadConfig();
            }
            if (GUILayout.Button("Init Proto"))
            {
                Util.Config.InitDefautPath();
            }
            if (GUILayout.Button("Generate Proto"))
            {
                Util.LoadAllWorksheet(Util.Config.ExcelPath, new GenerateProto().GenerateProtoFiles);
            }
            if (GUILayout.Button("Generate Script"))
            {
                new GenertatScript().GenerateScripts(GenertatScript.ScriptType.CSharp);
            }
            if (GUILayout.Button("Generate Dll"))
            {
                new GenerateDll().CompleDll(Util.Config.ProtoDllName);

                var dataPath = Application.dataPath;
                if (Util.Config.GenerateScriptDllFilePath.StartsWith(dataPath))
                {
                    var dllAssetPath = Util.Config.GenerateScriptDllFilePath.Substring(dataPath.Length - 6);
                    AssetDatabase.ImportAsset(dllAssetPath + $"/{Util.Config.ProtoDllName}");
                    Debug.Log(dllAssetPath);
                }
            }
            if (GUILayout.Button("Generate Data"))
            {
                Util.LoadAllWorksheet(Util.Config.ExcelPath, new GenerateProtoData().GenerateData);
            }


            if (GUILayout.Button("Test"))
            {
                
            }

        }
    }
}
