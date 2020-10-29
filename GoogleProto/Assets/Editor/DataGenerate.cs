using Google.Protobuf;
using Google.Protobuf.Collections;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DAProto
{
    class DataGenerate
    {
        static Dictionary<string, Func<string, object>> ChangeFuncDic = null;

        static Assembly assembly = null;

        static readonly string csNamespace = ConfigPath.CSNamespace + ".";

        const string boolFalse = "0";

        const string add = "Add";
        static readonly Type intType = typeof(int);

        static readonly string dataName = ConfigPath.Data_Path + "/{0}.bytes";
        static DataGenerate()
        {
            assembly = Assembly.LoadFrom(ConfigPath.ProtoDll_Path);

            // todo 枚举
            ChangeFuncDic = new Dictionary<string, Func<string, object>>()
            {
                { ProtoConfig.int32_, ParseInt },
                { ProtoConfig.int64_, ParseLong },

                { ProtoConfig.uint32_, ParseUInt },
                { ProtoConfig.uint64_, ParseULong },

                { ProtoConfig.sint32_, ParseInt },
                { ProtoConfig.sint64_, ParseLong },

                { ProtoConfig.fixed32_, ParseUInt },
                { ProtoConfig.fixed64_, ParseULong },

                { ProtoConfig.sfixed32_, ParseInt },
                { ProtoConfig.sfixed64_, ParseLong },

                { ProtoConfig.float_, ParseFloat },
                { ProtoConfig.double_, ParseDouble },

                { ProtoConfig.bool_, ParseBool },
                { ProtoConfig.string_, ParseString },
                //{ ProtoConfig.bytes_, v=> ByteString.CopyFromUtf8(v) }, //不一定使用后续在考虑

                { ProtoConfig.int32_s, ParseArrayInt },
                { ProtoConfig.int64_s, ParseArrayLong },

                { ProtoConfig.uint32_s, ParseArrayUInt },
                { ProtoConfig.uint64_s, ParseArrayULong },

                { ProtoConfig.sint32_s, ParseArrayInt },
                { ProtoConfig.sint64_s, ParseArrayLong },

                { ProtoConfig.fixed32_s, ParseArrayUInt },
                { ProtoConfig.fixed64_s, ParseArrayULong },

                { ProtoConfig.sfixed32_s, ParseArrayInt },
                { ProtoConfig.sfixed64_s, ParseArrayLong },

                { ProtoConfig.float_s,  ParseArrayFloat },
                { ProtoConfig.double_s,  ParseArrayDouble },

                { ProtoConfig.bool_s, ParseArrayBool },
                { ProtoConfig.string_s, ParseArrayString },
            };

            if (Directory.Exists(ConfigPath.Data_Path) == false)
            {
                Directory.CreateDirectory(ConfigPath.Data_Path);
            }
        }

        public static void Generate(ExcelWorksheet worksheet)
        {
            string sheetName = worksheet.Name;
            if (sheetName.StartsWith(ProtoConfig.Ignore))
                return;

            // 表内容为空不处理
            var addressBase = worksheet.Dimension;
            if (addressBase == null)
                return;


            string temp = csNamespace + "Excel_" + sheetName;
            object datdArray = assembly.CreateInstance(temp);

            if (datdArray == null)
                return;

            Type dataArrayType = datdArray.GetType();
            FieldInfo datafield = dataArrayType.GetField("data_", BindingFlags.NonPublic | BindingFlags.Instance);

            Type dataType = datafield.FieldType;

            object data = datafield.GetValue(datdArray);


            // 读取每一行数据，创建对象，赋值，转为二进制文件

            var excelRange = worksheet.Cells;

            var cellAddress = addressBase.End;

            int endRow = cellAddress.Row;
            int endColumn = cellAddress.Column;

            int startRow = ProtoConfig.DataRow;

            int typeRow = ProtoConfig.TypeRow;
            int nameRow = ProtoConfig.NameRow;

            // todo List优化,表定义在函数外部，重复利用List
            List<Func<string, object>> fileValueFuncs = new List<Func<string, object>>(endRow);
            List<string> fieldNames = new List<string>(endRow);
            List<string> typeNames = new List<string>(endRow);

            string instanceNamespace = csNamespace + worksheet.Name;

            for (int column = 1; column <= endColumn; column++)
            {
                //Todo 表内内嵌message情况处理

                fileValueFuncs.Add(GetTypetoFieldFunc(excelRange[typeRow, column].Text));
                fieldNames.Add(excelRange[nameRow, column].Text + "_");
                typeNames.Add(excelRange[typeRow, column].Text);
            }

            // 遍历每个单元
            for (int row = startRow; row <= endRow; row++)
            {
                object message = assembly.CreateInstance(instanceNamespace);
                Type messageType = message.GetType();
                for (int column = 1; column <= endColumn; column++)
                {
                    int index = column - 1;
                    Func<string, object> func = fileValueFuncs[index];
                    if (func == null)
                    {
                        continue;
                    }
                    var fieldValue = func(excelRange[row, column].Text);


                    FieldInfo fieldInfo = messageType.GetField(fieldNames[index], BindingFlags.NonPublic | BindingFlags.Instance);

                    fieldInfo.SetValue(message, fieldValue);
                }

                MethodInfo dataAddMethod = dataType.GetMethod(add, new Type[] { intType, messageType });
                dataAddMethod.Invoke(data, new object[] { row, message });
            }

            using (var output = File.Create(string.Format(dataName, sheetName)))
            {
                MessageExtensions.WriteTo((IMessage)datdArray, output);
            }
        }

        private static Func<string, object> GetTypetoFieldFunc(string type)
        {
            if (ChangeFuncDic.TryGetValue(type, out Func<string, object> func) == false)
                Console.WriteLine("当前类型无法在字典内获取，请检查！  " + type);

            return func;
        }

        #region Value
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
            return ParseBool2(value);
        }
        private static object ParseString(string value)
        {
            return ParseString2(value);
        }
        #endregion


        #region Array
        private static object ParseArray<T>(string value, Func<string, T> parse)
        {
            string[] values = value.Split(ProtoConfig.Split);
            int length = values.Length;
            RepeatedField<T> field = new RepeatedField<T>();
            for (int i = 0; i < length; i++)
            {
                field.Add(parse(values[i]));
            }
            return field;
        }

        private static object ParseArrayInt(string value)
        {
            return ParseArray(value, int.Parse);
        }
        private static object ParseArrayLong(string value)
        {
            return ParseArray(value, long.Parse);
        }
        private static object ParseArrayUInt(string value)
        {
            return ParseArray(value, uint.Parse);
        }
        private static object ParseArrayULong(string value)
        {
            return ParseArray(value, ulong.Parse);
        }

        private static object ParseArrayFloat(string value)
        {
            return ParseArray(value, float.Parse);
        }
        private static object ParseArrayDouble(string value)
        {
            return ParseArray(value, double.Parse);
        }
        private static object ParseArrayBool(string value)
        {
            return ParseArray(value, ParseBool2);
        }
        private static object ParseArrayString(string value)
        {
            return ParseArray(value, ParseString2);
        }
        #endregion

        private static bool ParseBool2(string value)
        {
            return value != boolFalse;
        }

        private static string ParseString2(string value)
        {
            return value;
        }
    }
}