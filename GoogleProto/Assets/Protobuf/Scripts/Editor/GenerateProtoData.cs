using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DA.Protobuf
{
    public class GenerateProtoData
    {
        private Assembly assembly = Assembly.LoadFile(Util.Config.GenerateScriptDllFilePath + "/" + Util.Config.ProtoDllName);
        public void GenerateData(ExcelWorksheet worksheet)
        {
            var config = Util.Config;

            string sheetName = worksheet.Name;
            var addressBase = worksheet.Dimension;

            var excelRange = worksheet.Cells;
            var cellAddress = addressBase.End;

            int endRow = cellAddress.Row;
            int endColumn = cellAddress.Column;

            int 
            int startRow = config.DataRow;
            int typeRow = config.TypeRow;
            int nameRow = config.NameRow;

            List<Func<string, object>> fileValueFuncs = new List<Func<string, object>>(endRow);
            List<string> fieldNames = new List<string>(endRow);
            List<string> typeNames = new List<string>(endRow);

            // 创建当前工作簿的 cs对象
            object sheetInstance = assembly.CreateInstance(config.CSNamespace + $".{sheetName}_Map");
            if (sheetInstance == null)
            {
                Util.LogError("创建工作簿对象失败");
                return;
            }

            var sheetMapDataFieldInfo = sheetInstance.GetType().GetField("data_", BindingFlags.NonPublic | BindingFlags.Instance);


            // 读取数据
            string sheetColumnClassName = config.CSNamespace + "." + sheetName;

            // 遍历每个单元
            for (int row = startRow; row <= endRow; row++)
            {
                object message = assembly.CreateInstance(sheetColumnClassName);

                for (int i = 0; i < length; i++)
                {

                }
            }

        }
    }
}