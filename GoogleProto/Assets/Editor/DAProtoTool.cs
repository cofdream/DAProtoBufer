using DAProtoBuf;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class DAProtoTool : EditorWindow
{
    // todo 功能
    // 自动生成Excel模板文件
    // 导出Excel的相关文件   可以检测文件是否被修改MD5
    // proto的忽略

    // 对枚举的处理需要优化
    // sheet内嵌Message的处理

    private string excelTemplateName = "Template";

    private string buildProtoPath;
    private string googldDllPath;
    private void Awake()
    {
        buildProtoPath = Path.Combine(Application.dataPath, "../BuildProto/Tool");
        googldDllPath = Application.dataPath + "/Pulagin/GoogoleProtobuf";
    }

    private void OnGUI()
    {
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

        GUILayout.Space(8);
        if (GUILayout.Button("解压缩Proto的相关配置文件"))
            DecompressDAProto();
    }

    private void DecompressDAProto()
    {
       // EditorUtility.DisplayProgressBar("开始下载");

        string zipPath = Path.Combine(Application.dataPath, "../", "/DAProtoTemp");
        Directory.CreateDirectory(zipPath);

        System.Net.WebClient myWebClient = new System.Net.WebClient();
        var content = myWebClient.DownloadData(@"https://github.com/cofdream/DAProtoBufer/raw/main/GoogleProto/Tool/Protoc.zip");
        string pZip = zipPath + "/Protoc.zip";
        using (FileStream fileStream = new FileStream(pZip, System.IO.FileMode.CreateNew))
        {
            fileStream.Write(content, 0, content.Length);
        }
        try
        {
            ZipTool.Decompress(pZip, buildProtoPath, null, OverWrite);
        }
        catch (Exception e)
        {
            throw e;
        }
        File.Delete(pZip);

        content = myWebClient.DownloadData(@"https://github.com/cofdream/DAProtoBufer/raw/main/GoogleProto/Tool/GoogleDll.zip");
        string gZip = zipPath + "/GoogleDll.zip";
        using (FileStream fileStream = new FileStream(gZip, System.IO.FileMode.CreateNew))
        {
            fileStream.Write(content, 0, content.Length);
        }
        try
        {
            ZipTool.Decompress(gZip, googldDllPath, null, OverWrite);
        }
        catch (Exception e)
        {
            throw e;
        }
        File.Delete(gZip);

        Directory.Delete(zipPath);
    }
    private bool OverWrite(string path)
    {
        Debug.LogWarning($"文件已存在解压缩保存失败,{path}");
        return false;
    }

    private void GenerateProtoFile()
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

    private void GenerateCSFile()
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

    private void GenerateCSDll()
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

    private void GenerateProtoData()
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
