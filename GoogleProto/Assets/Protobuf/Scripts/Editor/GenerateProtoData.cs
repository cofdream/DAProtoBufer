using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DA.Protobuf
{
    public class GenerateProtoData
    {

        public void GenerateData(ExcelWorksheet worksheet)
        {
            var config = ProtobufTool.Config;
            string sheetName = worksheet.Name;

            var assembly = Assembly.Load("DA.Generate.Protobuf");
        }
    }
}