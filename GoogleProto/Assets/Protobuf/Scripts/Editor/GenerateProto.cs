using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DA.Protobuf
{
    public class GenerateProto
    {
        /* Java 头文件
// [START java_declaration]
option java_package = ""com.DAProtobuf.DAProtobuf"";
option java_outer_classname = ""{0}"";
// [END java_declaration]
         */

        // C# 头文件
        private const string headerCSharpTemplate =
@"// Auto generated 
syntax = ""proto3"";
package packageName;

// [START csharp_declaration]
option csharp_namespace = ""{0}""; 
// [END csharp_declaration]";
        private const string messageTemplate = @"
message {0}
{{{1}
}}";

        private const string MapTemplate = @"
message {0}_Map
{{
    map<int32,{1}> data = 1;
}}";

        private const string FieldTemplate = @"
    {0} {1} = {2};";

        private const string FieldArrayTemplate = @"
    repeated {0} {1} = {2};";

        private const string EnumTemplate =
    @"enum {0}
{{{1}
}}";

        public void GenerateProtoFiles(ExcelWorksheet worksheet)
        {
            var config = Util.Config;
            string protoTemplate = config.GenerateProtoPath + @"\{0}.proto";
            string sheetName = worksheet.Name;
            var addressBase = worksheet.Dimension;

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(string.Format(headerCSharpTemplate, config.CSNamespace));

            int endColumn = addressBase.End.Column;

            ExcelRange range = worksheet.Cells;

            //判断是否为 枚举表
            string type = range[config.TypeRow, 1].Text;
            if (type.StartsWith(config.enumWorksheet_))
            {
                int endRow = worksheet.Dimension.End.Row;
                string enumMessage = GetEnumMessage(range, config.DataRow, endRow);

                stringBuilder.Append(string.Format(EnumTemplate, type.Substring(config.enumWorksheet_.Length), enumMessage));
            }
            else
            {
                AddMessage(range, 1, endColumn, config.TypeRow, config.NameRow, sheetName, config.Split, stringBuilder);
            }

            File.WriteAllText(string.Format(protoTemplate, worksheet.Name), stringBuilder.ToString());
        }

        private void AddMessage(ExcelRange range, int startColumn, int endColumn, int typeRow, int nameRow, string sheetName, char split, StringBuilder stringBuilder)
        {
            string messageFieldInfo = string.Empty;
            int index = 0;
            for (int column = startColumn; column <= endColumn; column++)
            {
                var type = range[typeRow, column].Text;
                var name = range[nameRow, column].Text;

                if (Util.Config.VariableType.Contains(type))
                {
                    index++;
                    if (type.EndsWith("]"))
                    {
                        messageFieldInfo += string.Format(FieldArrayTemplate, type.Split('[')[0], name, index.ToString());
                    }
                    else
                    {
                        messageFieldInfo += string.Format(FieldTemplate, type, name, index.ToString());
                    }
                }
                else
                {
                    string[] typeSplit = type.Split(split);

                    int arrayElementCount = int.Parse(typeSplit[1]);
                    int arrayFileCount = int.Parse(typeSplit[2]);

                    if (typeSplit.Length == 5)
                    {
                        //Map
                        int keyIndex = int.Parse(typeSplit[4]);
                        if (keyIndex > arrayElementCount)
                        {
                            Util.LogError($"表格：{sheetName} ,的类型：{type} 配置错误, map的key 超过了元素的总数量");

                        }

                    }
                    else if (typeSplit.Length == 3)
                    {
                        //Array

                    }
                    else
                    {
                        Util.LogError($"表格：{sheetName} ,的类型：{type} 配置错误");
                    }

                    
                    if (commentary.Length >= 2)
                    {
                        if (!int.TryParse(commentary[0], out arrayFileCount))
                        {
                            throw new Exception($"表格：{sheetName} ,的类型：{type} 未在注释内填写数据项数量");
                        }
                        if (!int.TryParse(commentary[1], out arrayElementCount))
                        {
                            throw new Exception($"表格：{sheetName} ,的类型：{type} 未在注释内填写数据集合长度");
                        }
                        string arrayType = sheetName + "_" + type + "_Array";
                        index++;
                        messageFieldInfo += string.Format(FieldArrayTemplate, arrayType, name, index.ToString());
                        string arrayMessageFieldInfo = string.Empty;
                        int arrayIndex = 0;
                        column++;
                        for (int i = 0; i < arrayFileCount; i++)
                        {
                            type = range[typeRow, column + i].Text;
                            name = range[nameRow, column + i].Text;
                            if (Util.Config.VariableType.Contains(type))
                            {
                                arrayIndex++;
                                if (type.EndsWith("]"))
                                {
                                    arrayMessageFieldInfo += string.Format(FieldArrayTemplate, type.Split('[')[0], name, arrayIndex.ToString());
                                }
                                else
                                {
                                    arrayMessageFieldInfo += string.Format(FieldTemplate, type, name, arrayIndex.ToString());
                                }
                            }
                            else
                            {
                                Util.LogError($"表格：{sheetName} ,第 {typeRow} 行 {column + i} 列元素配置错误。");
                            }
                        }
                        column += arrayFileCount * arrayElementCount;
                        stringBuilder.Append(string.Format(messageTemplate, arrayType, arrayMessageFieldInfo));
                        continue;
                    }
                    Util.LogError($"表格：{sheetName} ,的类型：{type} 未在注释内填写数据项数量");
                }
            }

            stringBuilder.AppendLine(string.Format(messageTemplate, sheetName, messageFieldInfo));

            stringBuilder.Append(string.Format(MapTemplate, sheetName, sheetName));
        }

        private string GetEnumMessage(ExcelRange range, int startRaw, int endRow)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = startRaw; i < endRow; i++)
            {
                stringBuilder.Append(string.Format(FieldTemplate, string.Empty, range[i, 1].Text, range[i, 2].Text));
            }
            return stringBuilder.ToString();
        }
    }
}