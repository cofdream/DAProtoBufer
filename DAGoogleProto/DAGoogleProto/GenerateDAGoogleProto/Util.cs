using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DAGoogleProto
{
    public static class Util
    {
        public static event Action<string> LogAction = Console.WriteLine;
        public static event Action<string> LogErrorAction = Console.WriteLine;
        internal static void Log(string content)
        {
            LogAction?.Invoke(content);
        }
        internal static void LogError(string content)
        {
            LogErrorAction?.Invoke(content);
        }

        internal static string CMD(string str)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding("GB2312");
            process.Start();

            process.StandardInput.WriteLine(str);
            process.StandardInput.AutoFlush = true;
            process.StandardInput.WriteLine("exit");

            string output = "cmd:\n" + str + "\n\n" + process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            return output;
        }

        public static void Serizlization<T>(T target, string filePath) where T : new()
        {
            if (target == null) return;

            var toStringFuncDic = GetToStringFuncDictionary();
            string template = "{0}={1}";
            if (File.Exists(filePath) == false)
            {
                Log($"not exists: {filePath} , Created . ");

                var fieldInfos = target.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

                Type boolType = typeof(bool);
                using (var streamWriter = File.CreateText(filePath))
                {
                    foreach (var fieldInfo in fieldInfos)
                    {
                        if (toStringFuncDic.TryGetValue(fieldInfo.FieldType, out var toStringFunc))
                            streamWriter.WriteLine(string.Format(template, fieldInfo.Name, toStringFunc(fieldInfo.GetValue(target))));
                        else
                            Log("字段类型无法反序列化" + fieldInfo.FieldType);
                    }
                }
            }
            else
            {
                Type type = target.GetType();
                char equals = '=';
                var lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var datas = line.Split(equals);
                    if (datas.Length < 1) continue;

                    var fieldInfo = type.GetField(datas[0], BindingFlags.Public | BindingFlags.Instance);
                    if (fieldInfo == null) continue;

                    if (toStringFuncDic.TryGetValue(fieldInfo.FieldType, out var toStringFunc))
                        lines[i] = string.Format(template, fieldInfo.Name, toStringFunc(fieldInfo.GetValue(target)));
                    else
                        Log("字段类型无法反序列化" + fieldInfo.FieldType);
                }
                File.WriteAllLines(filePath, lines);
            }
        }

        public static object Deserizlization<T>(string filePath) where T : new()
        {
            if (File.Exists(filePath) == false)
            {
                LogError("LoadConfig error! " + filePath);
                return null;
            }

            Type type = typeof(T);
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;


            var TypeDic = GetChangeFuncDictionary();

            var obj = new T();
            var lines = File.ReadAllLines(filePath);
            char equals = '=';
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var datas = line.Split(equals);
                if (datas.Length < 2) continue;
                string fileName = datas[0];
                string value = datas[1];

                var fieldInfo = type.GetField(fileName, bindingFlags);
                if (fieldInfo == null) continue;

                var fieldInfoType = fieldInfo.FieldType;

                if (TypeDic.TryGetValue(fieldInfoType, out var ParseFunc) == false)
                {
                    Log("字段类型无法反序列化" + fieldInfoType);
                    continue;
                }

                fieldInfo.SetValue(obj, ParseFunc(value));
            }

            return obj;
        }

        public static Dictionary<Type, Func<string, object>> GetChangeFuncDictionary()
        {
            return new Dictionary<Type, Func<string, object>>()
            {
                { typeof(int),    ParseInt     },
                { typeof(long),   ParseLong    },
                { typeof(uint),   ParseUInt    },
                { typeof(ulong),  ParseULong   },
                { typeof(float),  ParseFloat   },
                { typeof(double), ParseDouble  },
                { typeof(bool),   ParseBool    },
                { typeof(string), ParseString  },
            };
        }
        public static Dictionary<Type, Func<object, string>> GetToStringFuncDictionary()
        {
            return new Dictionary<Type, Func<object, string>>()
            {
                { typeof(int),    ObjToString   },
                { typeof(long),   ObjToString   },
                { typeof(uint),   ObjToString   },
                { typeof(ulong),  ObjToString   },
                { typeof(float),  ObjToString   },
                { typeof(double), ObjToString   },
                { typeof(bool),   BoolToString  },
                { typeof(string), ObjToString   },
            };
        }

        #region to string
        private static string ObjToString(object value)
        {
            return value == null ? string.Empty : value.ToString();
        }
        private static string BoolToString(object value)
        {
            return (bool)value ? "0" : "1";
        }
        #endregion

        #region Parse to 
        private static object ParseInt(string value)
        {
            return int.Parse(value);
        }
        private static object ParseLong(string value)
        {
            return long.Parse(value);
        }
        private static object ParseUInt(string value)
        {
            return uint.Parse(value);
        }
        private static object ParseULong(string value)
        {
            return ulong.Parse(value);
        }

        private static object ParseFloat(string value)
        {
            return float.Parse(value);
        }
        private static object ParseDouble(string value)
        {
            return double.Parse(value);
        }

        private static object ParseBool(string value)
        {
            return value == "0" || string.IsNullOrWhiteSpace(value) ? false : true;
        }
        private static object ParseString(string value)
        {
            return value;
        }
        #endregion
    }
}
