using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using DAL.Interfaces;
using DAL.Repositories;

namespace UI.Classes
{
    public class ReportBuilder
    {
        static readonly IReportValuesRepository _repository = new ReportValuesRepository();

        public void Generate(int id_group, int id_template, MemoryStream outputStream)
        {
            XLWorkbook workbook = new XLWorkbook();
            workbook.Properties.Comments = Guid.NewGuid().ToString();

            var sheet = workbook.Worksheets.Add("Отчет");

            //Заголовок
            sheet.Cell(1, 1).SetValue("ИД дома");
            sheet.Cell(1, 1).Style.Font.Bold = true;


            int col = 1;
            int row = 2;
            int oldrow = row;
            int max = row;
            int curid = 0;
            string curfield = "";
            foreach (var rv in _repository.Get(id_group, id_template))
            {
                if (curid != rv.Id_house)
                {
                    curid = rv.Id_house;
                    col = 1;
                    row = max;
                    oldrow = row;
                    sheet.Cell(row, col).SetValue(rv.Id_house);
                    //col++;
                }

                if (curfield != rv.Form + "." + rv.Field)
                {
                    col++;
                    curfield = rv.Form + "." + rv.Field;
                    sheet.Cell(1, col).SetValue(curfield);
                    sheet.Cell(1, col).Style.Font.Bold = true;
                    sheet.Columns(col, col).Width = 30;
                    row = oldrow;
                }
                sheet.Cell(row, col).SetValue(rv.Value);
                row++;

                if (row > max)
                    max = row;
            }


            workbook.SaveAs(outputStream);
            outputStream.Seek(0, SeekOrigin.Begin);
        }
    }
}