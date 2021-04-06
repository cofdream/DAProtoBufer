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

        private Vector2 backgroundPosition;
        private GUIStyle backgroundGUIStyle = (GUIStyle)"box";

        private bool createExcelToogle = false;

        private void OnEnable()
        {
            Util.LogAction = Debug.Log;

            PathConfig();
        }

        private void OnGUI()
        {
            backgroundPosition = GUILayout.BeginScrollView(backgroundPosition, backgroundGUIStyle);
            {
                createExcelToogle = GUILayout.Toggle(createExcelToogle, "Create Excel", EditorStyles.foldout);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Excel 文件名");
                    excelTemplateName = EditorGUILayout.TextField(excelTemplateName);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal((GUIStyle)"box");
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("excel 文件名");
                    excelTemplateName = GUILayout.TextField(excelTemplateName, 30);
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("生成Excel模板文件"))
                {
                    string excelPath = string.Format(ConfigPath.Excel_Path + @"\{0}.xlsx", excelTemplateName);
                    if (File.Exists(excelPath))
                    {
                        EditorUtility.DisplayDialog("存在同名的Excel", excelPath, "确认");
                        Debug.Log("存在同名的Excel \n" + excelPath);//让路径可粘贴
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
                        if (EditorPrefs.GetBool("kAutoRefresh") == false)
                        {

                            if (EditorUtility.DisplayDialog("编译CSharp文件为Dll结束", "是否刷新项目资源", "刷新"))
                            {
                                AssetDatabase.Refresh();
                            }
                        }
                        else
                        {
                            AssetDatabase.Refresh();
                        }
                    }

                    if (GUILayout.Button("4.生成二进制数据文件"))
                    {
                        GenerateProtoData();

                        if (EditorPrefs.GetBool("kAutoRefresh") == false)
                        {
                            if (EditorUtility.DisplayDialog("二进制文件生成结束", "是否刷新项目资源", "刷新"))
                            {
                                AssetDatabase.Refresh();
                            }
                        }
                        else
                        {
                            AssetDatabase.Refresh();
                        }
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

                    if (EditorPrefs.GetBool("kAutoRefresh") == false)
                    {
                        if (EditorUtility.DisplayDialog("二进制文件生成结束", "是否刷新项目资源", "刷新"))
                        {
                            AssetDatabase.Refresh();
                        }
                    }
                    else
                    {
                        AssetDatabase.Refresh();
                    }
                }
            }
            GUILayout.EndVertical();
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
                string log = CSharpDllExport.Execute();
                Debug.Log(log);
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
                //EPPlusTool.Execute(DataGenerate.Generate);
                EditorUtility.DisplayProgressBar("生成二进制数据文件", "Generate bytes data File...", 1);
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

        private void PathConfig()
        {
#if PROTO_TEST
            // 本地测试环境是
            string projectPath = Path.Combine(Application.dataPath, "../");

            string path = projectPath + @"\BuildProto";

            ConfigPath.Excel_Path = path + @"\Excel";

            ConfigPath.Proto_Path = path + @"\Generate\Proto";

            ConfigPath.Script_Path = path + @"\Generate\Script";

            ConfigPath.CSharp_path = ConfigPath.Script_Path + @"\cs";

            // 数据文件存放路径
            ConfigPath.Data_Path = projectPath + @"\Assets\Resources\DataConfig";

            // dll生成路径
            ConfigPath.ProtoDll_Path = projectPath + @"\Assets\Plugins\DAProtobuf\" + ConfigPath.CSNamespace + ".dll";
            // googledll的路径
            ConfigPath.GoogleDll_Path = projectPath + @"\Assets\Plugins\GoogoleProtobuf_3.8.0\Google.Protobuf.dll";

            // protoc.exe
            ConfigPath.ProtoExe_Path = projectPath + @"\Assets\.ProtoTool\protoc-3.8.0-win64\protoc.exe";
#endif
            string projectPath = Path.Combine(Application.dataPath, "../");

            string path = projectPath + @"\BuildProto";

            ConfigPath.Excel_Path = path + @"\Excel";

            ConfigPath.Proto_Path = path + @"\Generate\Proto";

            ConfigPath.Script_Path = path + @"\Generate\Script";

            ConfigPath.CSharp_path = ConfigPath.Script_Path + @"\cs";

            // 数据文件存放路径
            ConfigPath.Data_Path = projectPath + @"\Assets\Resources\DataConfig";

            // dll生成路径
            ConfigPath.ProtoDll_Path = projectPath + @"\Assets\Plugins\DAProtobuf\" + ConfigPath.CSNamespace + ".dll";

            // googledll的路径
            //ConfigPath.GoogleDll_Path = typeof(Google.Protobuf.MessageExtensions).Assembly.Location;
            // protoc.exe
            ConfigPath.ProtoExe_Path = Path.Combine(ConfigPath.GoogleDll_Path, "../", "../", "../", @".ProtoTool\protoc-3.8.0-win64\protoc.exe");
        }


        [MenuItem("Tools/DAProtoTool")]
        static void OpenDAProtoTool()
        {
            var window = GetWindow<DAProtoTool>();
            window.Show();
        }
    }
}
