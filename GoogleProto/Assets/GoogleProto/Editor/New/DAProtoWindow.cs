using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DAGoogleProto
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
            GoogleProtoTool.CheckConfigPath();
        }
        private void OnGUI()
        {
            if (GUILayout.Button("Generate Proto"))
            {
                GoogleProtoTool.LoadAllWorksheet(GoogleProtoTool.Config.ExcelPath, GenerateProto.Generate);
            }

            if (GUILayout.Button("Generate Proto"))
            {
                GoogleProtoTool.LoadAllWorksheet(GoogleProtoTool.Config.ExcelPath, GenerateProto.Generate);
            }
        }
    }
}
