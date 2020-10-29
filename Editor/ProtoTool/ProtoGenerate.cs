using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DAProtoBuf
{
    class ProtoGenerate
    {
        private const string headerTemplate =
@"// Auto generated 

syntax = ""proto3"";
package packageName;

// [START java_declaration]
option java_package = ""com.DAProtobuf.DAProtobuf"";
option java_outer_classname = ""{0}"";
// [END java_declaration]

// [START csharp_declaration]
option csharp_namespace = ""{1}""; 
// [END csharp_declaration]";

        private const string messageTemplate = @"
message {0}
{{{1}
}}";

        private const string MapTemplate = @"
message Excel_{0}
{{
    map<int32,{1}> {2} = 1;
}}";
        private const string FieldTemplate = @"
    {0} {1} = {2};";

        private const string FieldArrayTemplate = @"
    repeated {0} {1} = {2};";

        private const string EnumTemplate =
@"enum {0}
{{{1}
}}";
        private static readonly string ProtoTemplate = ConfigPath.Proto_Path + @"\{0}.proto";

        static ProtoGenerate()
        {
            if (Directory.Exists(ConfigPath.Proto_Path) == false)
            {
                Directory.CreateDirectory(ConfigPath.Proto_Path);
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

            StringBuilder stringBuilder = new StringBuilder();

            // 添加 “头” 文件，java 需要使用
            stringBuilder.AppendLine(string.Format(headerTemplate, sheetName, ConfigPath.CSNamespace));

            int endColumn = addressBase.End.Column;

            ExcelRange range = worksheet.Cells;

            //判断是否为 枚举表
            string type = range[ProtoConfig.TypeRow, 1].Text;
            if (type.StartsWith(ProtoConfig.enum_))
            {
                int endRow = worksheet.Dimension.End.Row;
                AddEnumMessage(range, ProtoConfig.DataRow, endRow, type.Substring(ProtoConfig.enum_.Length), stringBuilder);
            }
            else
            {
                AddMessage(range, 1, endColumn, ProtoConfig.TypeRow, ProtoConfig.NameRow, sheetName, stringBuilder);

                stringBuilder.Append(string.Format(MapTemplate, sheetName, sheetName, "Data"));
            }

             File.WriteAllText(string.Format(ProtoTemplate, worksheet.Name), stringBuilder.ToString());
        }

        private static void AddMessage(ExcelRange range, int startColumn, int endColumn, int typeRow, int nameRow, string messageName, StringBuilder stringBuilder)
        {
            string message = string.Empty;
            int index = 0;
            for (int column = startColumn; column <= endColumn; column++)
            {
                var type = range[typeRow, column].Text;
                var name = range[nameRow, column].Text;

                if (ProtoConfig.VariableType.Contains(type) == false)
                {
                    //另起一个Message
                    //GetMessage();
                }
                else
                {
                    index++;
                    if (type.EndsWith("]"))
                    {
                        message += string.Format(FieldArrayTemplate, type.Split('[')[0], name, index.ToString());
                    }
                    else
                    {
                        message += string.Format(FieldTemplate, type, name, index.ToString());
                    }
                }
            }

            stringBuilder.AppendLine(string.Format(messageTemplate, messageName, message));
        }

        private static void AddEnumMessage(ExcelRange range, int startRaw, int endRow, string name, StringBuilder stringBuilder)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = startRaw; i < endRow; i++)
            {
                sb.Append(string.Format(FieldTemplate, string.Empty, range[i, 1].Text, range[i, 2].Text));
            }
            stringBuilder.Append(string.Format(EnumTemplate, name, sb.ToString()));
        }
    }
}
