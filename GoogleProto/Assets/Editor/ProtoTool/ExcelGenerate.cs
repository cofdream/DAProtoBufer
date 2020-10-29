using OfficeOpenXml;
using System.IO;

namespace DAProtoBuf
{
    public class ExcelGenerate
    {
        static ExcelGenerate()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public static void Generate(string excelPath)
        {
            string excelName = Path.GetFileNameWithoutExtension(excelPath);

            using (ExcelPackage excel = new ExcelPackage())
            {
                var excelWorksheet = excel.Workbook.Worksheets.Add(excelName);

                ExcelInit(excelWorksheet);

                string dirPath = Path.GetDirectoryName(excelPath);
                if (Directory.Exists(dirPath) == false)
                {
                    Directory.CreateDirectory(dirPath);
                }
                excel.SaveAs(new FileInfo(excelPath));
            }
        }
        private static void ExcelInit(ExcelWorksheet sheet)
        {
            int row = 1;
            int maxCol = 5;

            GenerateTip(sheet, row, maxCol);
            row++;
            GenerateType(sheet, row, maxCol);
            row++;
            GenerateFieldName(sheet, row, maxCol);
            row++;
            GenerateField(sheet, row, maxCol);
        }

        private static void GenerateTip(ExcelWorksheet sheet, int row, int maxCol)
        {
            ExcelRange range = sheet.Cells[row, 1];

            range.Value = "id唯一";
        }

        private static void GenerateType(ExcelWorksheet sheet, int row, int maxCol)
        {
            sheet.Cells[row, 1].Value = ProtoConfig.VariableType[2];

            for (int col = 1; col < maxCol; col++)
            {
                string address = ExcelCellBase.GetAddress(row, col);

                var validationList = sheet.DataValidations.AddListValidation(address);

                // 添加序列类型的数据验证
                foreach (var formula in ProtoConfig.VariableType)
                {
                    validationList.Formula.Values.Add(formula);
                }

                // 添加错误数据提示
                validationList.ShowErrorMessage = true;
                validationList.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.warning;
                validationList.ErrorTitle = "类型错误";
                validationList.Error = "当前类型不是限定类型,你确定你输入的类型是正确的吗？" +
                    "\n(自定义枚举和内嵌Message忽略)";

                // todo 添加数组类型分割提示
            }
        }

        private static void GenerateFieldName(ExcelWorksheet sheet, int row, int maxCol)
        {
            ExcelRange range = sheet.Cells[row, 1];
            range.Value = "id";
        }
        private static void GenerateField(ExcelWorksheet sheet, int row, int maxCol)
        {
            ExcelRange range = sheet.Cells[row, 1];
            range.AddComment($"开头为 {ProtoConfig.Ignore} 的字段会被忽略", "Develop").AutoFit = true;
        }

    }
}

