using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FindExcelTool : EditorWindow
{
    [MenuItem("Tools/Find Excel Tool")]
    private static void OpenFindExcelTool()
    {
        FindExcelTool tool = CreateWindow<FindExcelTool>();
        tool.Show();
    }

    private string excelWorkName;
    [SerializeField] private List<string> excelPathRoots;
    private SerializedObject serializedObject;
    private SerializedProperty serializedProperty;
    private bool activePathSelect = false;

    private void OnEnable()
    {
        string root = System.IO.Directory.GetParent(Application.dataPath).FullName;
        excelPathRoots = new List<string>()
        {
        };

        serializedObject = new SerializedObject(this);
        serializedProperty = serializedObject.FindProperty("excelPathRoots");

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }
    private void OnGUI()
    {
        GUILayout.BeginVertical("box");
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedProperty, new GUIContent("excel paths"), false);
            serializedObject.ApplyModifiedProperties();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Excel work sheet name");

                excelWorkName = GUILayout.TextField(excelWorkName);

                if (GUILayout.Button("Find All"))
                {
                    // todo 开新线程去处理
                    Debug.Log("Searching...");
                    foreach (var root in excelPathRoots)
                    {
                        var excelPaths = System.IO.Directory.GetFiles(root, "*.xlsx");
                        foreach (var excelPath in excelPaths)
                        {
                            using (ExcelPackage excel = new ExcelPackage(new System.IO.FileInfo(excelPath)))
                            {
                                foreach (var worksheet in excel.Workbook.Worksheets)
                                {
                                    if (excelWorkName.Equals(worksheet.Name))
                                    {
                                        Debug.Log($"Search: {excelWorkName} in {excelPath}.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private class ExcelPath : ScriptableObject
    {
        public string path;
    }

}
