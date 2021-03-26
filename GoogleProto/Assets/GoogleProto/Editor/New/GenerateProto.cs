using OfficeOpenXml;
using System.IO;
using System.Linq;
using System.Text;

namespace DAGoogleProto
{
    internal static class GenerateProto
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
    map<int32,{1}> Data = 1;
}}";

        private const string FieldTemplate = @"
    {0} {1} = {2};";

        private const string FieldArrayTemplate = @"
    repeated {0} {1} = {2};";

        private const string EnumTemplate =
    @"enum {0}
{{{1}
}}";

        public static void Generate(ExcelWorksheet worksheet)
        {
            var config = GoogleProtoTool.Config;
            string protoTemplate = config.GenerateProtoPath + @"\{0}.proto";


            string sheetName = worksheet.Name;
            if (sheetName.StartsWith(config.IgnoreWorkSheet))
                return;

            // 表内容为空不处理
            var addressBase = worksheet.Dimension;
            if (addressBase == null)
                return;


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
                string message = GetMessage(range, 1, endColumn, config.TypeRow, config.NameRow);

                stringBuilder.AppendLine(string.Format(messageTemplate, sheetName, message));

                stringBuilder.Append(string.Format(MapTemplate, sheetName, sheetName));
            }

            File.WriteAllText(string.Format(protoTemplate, worksheet.Name), stringBuilder.ToString());
        }

        private static string GetMessage(ExcelRange range, int startColumn, int endColumn, int typeRow, int nameRow)
        {
            string message = string.Empty;
            int index = 0;
            for (int column = startColumn; column <= endColumn; column++)
            {
                var type = range[typeRow, column].Text;
                var name = range[nameRow, column].Text;

                if (GoogleProtoTool.Config.VariableType.Contains(type))
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
                else
                {
                    // 如果检测到是 其他类型 的数据，做另外的处理
                }
            }
            return message;
        }

        private static string GetEnumMessage(ExcelRange range, int startRaw, int endRow)
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