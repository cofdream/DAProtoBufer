using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace DAProtoBuf
{
    internal class EPPlusTool
    {
        static string ExcelPath = ConfigPath.Excel_Path;
        const string xlsx = "*.xlsx";
        static EPPlusTool()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (Directory.Exists(ExcelPath) == false)
            {
                Directory.CreateDirectory(ExcelPath);
            }
        }

        public static void Execute(Action<ExcelWorksheet> callback)
        {
            
            Load(xlsx, callback);
            //Generate("*.xls", callback);//不支持xls todo 为当前的读取配置表做一个 接口，然后相关代码已接口形式调用
        }

        private static void Load(string searchPattern, Action<ExcelWorksheet> callback)
        {
            string[] excelFilePaths = Directory.GetFiles(ExcelPath, searchPattern, SearchOption.AllDirectories);
            List<string> excelNames = new List<string>(excelFilePaths.Length);

            foreach (var filePath in excelFilePaths)
                using (ExcelPackage excel = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheets worksheets = excel.Workbook.Worksheets;
                    foreach (var worksheet in worksheets)
                    {
                        foreach (var excelName in excelNames)
                            if (excelName.Equals(worksheet.Name))
                                throw new Exception("存在相同工作簿名称的表: " + worksheet.Name);
                        callback(worksheet);
                        excelNames.Add(worksheet.Name);
                    }
                }
        }
    }
}