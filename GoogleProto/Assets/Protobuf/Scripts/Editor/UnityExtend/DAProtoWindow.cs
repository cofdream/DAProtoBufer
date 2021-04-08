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
            Util.Init();
            Util.Config.CheckConfigPath();
        }
        string dllPath;
        private void OnGUI()
        {
            dllPath = GUILayout.TextField(dllPath);
            if (GUILayout.Button("Refresh"))
            {
                AssetDatabase.ImportAsset(dllPath);
            }

            if (GUILayout.Button("Init Util"))
            {
                Util.Init();
            }
            if (GUILayout.Button("Load Config"))
            {
                Util.LoadConfig();
            }
            if (GUILayout.Button("Init ConfigPath"))
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
                new GenerateDll().CompleDll();
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
