using DAProtoBuf;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DAProtoTool : EditorWindow
{
    // todo 功能
    // 自动生成Excel模板文件
    // 导出Excel的相关文件   可以检测文件是否被修改MD5
    // proto的忽略

    // 对枚举的处理需要优化
    // sheet内嵌Message的处理

    private string excelTemplateName = "Template";

    private bool isInit = false;



    private void OnGUI()
    {
        if (isInit == false)
        {
            // 检查目录
            isInit = CheckDAProto();
        }
        if (isInit == false)
        {
            if (GUILayout.Button("初始化Proto的相关配置文件"))
            {
                InitDAProto();
                isInit = true;
            }
            else
            {
                return;
            }
        }


        GUILayout.BeginHorizontal((GUIStyle)"box");
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("excel 文件名");
                excelTemplateName = GUILayout.TextField(excelTemplateName, 30);
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("生成Excel模板文件"))
            {
                ExcelGenerate.Generate(string.Format(ConfigPath.Excel_Path + @"\{0}.xlsx", excelTemplateName));
            }
        }
        GUILayout.EndHorizontal();



        GUILayout.BeginVertical((GUIStyle)"box");
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("1.生成Proto文件"))
                {
                    GenerateProtoFile();
                }

                if (GUILayout.Button("2.生成CSharp文件"))
                {
                    GenerateCSFile();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(2);

            GUILayout.BeginHorizontal();
            {

                if (GUILayout.Button("3.编译CSharp文件为Dll"))
                {
                    GenerateCSDll();
                }

                if (GUILayout.Button("4.生成二进制数据文件"))
                {
                    GenerateProtoData();
                }
            }
            GUILayout.EndHorizontal();

        }
        GUILayout.EndVertical();

        GUILayout.Space(8);

        if (GUILayout.Button("一键生成对应文件"))
        {
            GenerateProtoFile();
            GenerateCSFile();
            GenerateCSDll();
            GenerateProtoData();
        }
    }

    private static bool CheckDAProto()
    {
        // 检查相关工具文件是否存在

        return false;
    }

    private static void InitDAProto()
    {
        // 解压缩对应目录下的压缩包
        // assets文件夹外部去创建Tool目录
        // google的Dll解压缩
    }

    private static void GenerateProtoFile()
    {
        try
        {
            EditorUtility.DisplayProgressBar("生成Proto文件", "Generate Proto...", 0);
            EPPlusTool.Execute(ProtoGenerate.Generate);
            EditorUtility.DisplayProgressBar("生成Proto文件", "Generate Proto...", 1);
            EditorUtility.ClearProgressBar();
        }
        catch (System.Exception e)
        {
            throw e;
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    private static void GenerateCSFile()
    {
        try
        {
            EditorUtility.DisplayProgressBar("生成CSharp文件", "Generate CS File...", 0);
            ScriptGenerate.Execute(ScriptGenerate.ScriptType.CSharp);
            EditorUtility.DisplayProgressBar("生成CSharp文件", "Generate CS File...", 1);
            EditorUtility.ClearProgressBar();
        }
        catch (System.Exception e)
        {
            throw e;
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    private static void GenerateCSDll()
    {
        try
        {
            EditorUtility.DisplayProgressBar("编译CSharp文件为Dll", "Generate CS Dll...", 0);
            CSharpDllExport.Execute();
            EditorUtility.DisplayProgressBar("编译CSharp文件为Dll", "Generate CS Dll...", 1);
            EditorUtility.ClearProgressBar();
        }
        catch (System.Exception e)
        {
            throw e;
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

    }

    private static void GenerateProtoData()
    {
        try
        {
            EditorUtility.DisplayProgressBar("生成二进制数据文件", "Generate bytes data File...", 0);
            EPPlusTool.Execute(DataGenerate.Generate);
            EditorUtility.DisplayProgressBar("生成二进制数据文件", "Generate bytes data File...", 1);
            EditorUtility.ClearProgressBar();

            if (EditorUtility.DisplayDialog("二进制文件生成结束", "是否刷新项目资源", "刷新"))
            {
                AssetDatabase.Refresh();
            }
        }
        catch (System.Exception e)
        {
            throw e;
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }


    [MenuItem("Tools/DAProtoTool")]
    static void OpenDAProtoTool()
    {
        var window = GetWindow<DAProtoTool>();
        window.Show();
    }
}
