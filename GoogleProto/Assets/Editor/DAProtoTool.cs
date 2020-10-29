using System;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace DAProto
{
    public class DAProtoTool : EditorWindow
    {
        // todo 功能
        // 自动生成Excel模板文件
        // 导出Excel的相关文件   可以检测文件是否被修改MD5
        // proto的忽略

        // 对枚举的处理需要优化
        // sheet内嵌Message的处理
        // 后续很可能变成调用外部程序，感觉引用的dll会有问题，
        // 也许不适合打成Package

        private string excelTemplateName = "Template";

        private void Awake()
        {
            ConfigPath.InitConfigPath(Directory.GetParent(Application.dataPath).FullName);
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
                    string excelPath = string.Format(ConfigPath.Excel_Path + @"\{0}.xlsx", excelTemplateName);
                    if (File.Exists(excelPath))
                    {
                        EditorUtility.DisplayDialog("存在重复的Excel", excelPath, "确认");
                    }
                    else
                    {
                        ExcelGenerate.Generate(excelPath);
                    }

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


                GUILayout.Space(8);

                if (GUILayout.Button("一键生成对应文件"))
                {
                    GenerateProtoFile();
                    GenerateCSFile();
                    GenerateCSDll();
                    GenerateProtoData();
                }
            }
            GUILayout.EndVertical();

            GUILayout.Space(8);
            if (GUILayout.Button("初始化Protoc工具"))
                DecompressDAProtoc();
        }


        #region 初始配置文件生成
        private const string protocURL = "https://github.com/cofdream/DAProtoBufer/raw/main/GoogleProto/Tool/Protoc.zip";
        private void DecompressDAProtoc()
        {
            string downloadPath = Path.Combine(Application.dataPath, "../", "/DAProtoTemp");
            Directory.CreateDirectory(downloadPath);

            EditorUtility.DisplayProgressBar("Donwnload", "下载 Protoc.zip 资源文件中...", 0);

            var webClient = new WebClient();

            var content = webClient.DownloadData(protocURL);
            string pZip = downloadPath + "/Protoc.zip";
            using (FileStream fileStream = new FileStream(pZip, System.IO.FileMode.CreateNew))
            {
                fileStream.Write(content, 0, content.Length);
            }

            EditorUtility.DisplayProgressBar("Decompress", "解压缩相关配置文件中...", 0);

            string buildProtoPath = Path.Combine(Application.dataPath, "../", "/BuildProto/Tool");
            string googldDllPath = Application.dataPath + "/Pulagin/GoogoleProtobuf";

            try
            {
                ZipTool.Decompress(pZip, buildProtoPath, null, OverWrite);
            }
            catch (Exception e)
            {
                throw e;
            }

            File.Delete(pZip);

            Directory.Delete(downloadPath);

            EditorUtility.ClearProgressBar();
        }

        private bool OverWrite(string path)
        {
            Debug.LogWarning($"文件已存在解压缩配置文件保存失败,{path}");
            return false;
        }
        #endregion


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
}
