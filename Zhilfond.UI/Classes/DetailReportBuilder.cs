using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using DAL.Interfaces;
using DAL.Repositories;
using System.ComponentModel;
using DAL.Models;
using DAL;
using System.Data.Objects.SqlClient;
using System.Text.RegularExpressions;

namespace UI.Classes
{
    public class DetailReportBuilder
    {
        static readonly IReportTemplatesRepository _repository = new ReportTemplatesRepository();

        public void Generate(int id_elem, int nTemplateID, int nReportType, MemoryStream outputStream)
        {
            Dictionary<string, string> dictValues = CreateDict(id_elem, nReportType);
            AddHeaderInfoToDict(dictValues);
            Build(dictValues, nTemplateID, outputStream);
        }

        private void AddHeaderInfoToDict(Dictionary<string, string> dictValues)
        {
            dictValues.Add("[Дата отчета].[Дата]", DateTime.Now.ToLongDateString());
        }

        private void Build(Dictionary<string, string> dictValues, int nTemplateID, MemoryStream outputStream)
        {
            try
            {
                // Получаем файл шаблона из БД            
                ReportTemplate rtTemplate = _repository.GetById(nTemplateID);

                // Отправляем полученный файл в поток
                outputStream.Write(rtTemplate.Data, 0, rtTemplate.Data.Length);
                outputStream.Position = 0;

                // Получаем книгу
                XLWorkbook wbTemplateBook = new XLWorkbook(outputStream);

                foreach (IXLWorksheet wsSheet in wbTemplateBook.Worksheets)
                {
                    if (wsSheet.Visibility == XLWorksheetVisibility.Visible)
                    {
                        int firstRow = wsSheet.FirstRowUsed().RowNumber();
                        int lastRow = wsSheet.LastRowUsed().RowNumber();
                        int fisrtCol = wsSheet.FirstColumn().ColumnNumber();
                        int lastCol = wsSheet.LastColumnUsed().ColumnNumber();

                        for (int currRow = firstRow; currRow <= lastRow; currRow++)
                        {
                            for (int currCol = fisrtCol; currCol <= lastCol; currCol++)
                            {
                                string sBuff = wsSheet.Cell(currRow, currCol).Value.ToString();

                                if (String.IsNullOrEmpty(sBuff)) continue;

                                MatchCollection mcFields = Regex.Matches(sBuff, @"\[(.*?)\]\.\[(.*?)\]");

                                if (mcFields.Count == 0) continue;

                                foreach (Match match in mcFields)
                                {
                                    string dictValue = string.Empty;
                                    if (dictValues.TryGetValue(match.Value, out dictValue))
                                    {
                                        sBuff = sBuff.Replace(match.Value, dictValue);
                                    }
                                    else
                                    {
                                        sBuff = sBuff.Replace(match.Value, "[no data]");
                                    }
                                }

                                wsSheet.Cell(currRow, currCol).Value = sBuff;
                            }
                        }
                    }
                }

                wbTemplateBook.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("UI.Classes.DetailReportBuilder.Build(Dictionary<string, string> dictValues, int nTemplateID, MemoryStream outputStream) Exception: {0}", ex.Message));
            }
        }

        private Dictionary<string, string> CreateDict(int id_elem, int nReportType)
        {
            try
            {
                var dict = new Dictionary<string, string>();

                using (var context = new UserContext())
                {
                    using (var ctx = new UserContext())
                    {
                        var groupHouses = from g in ctx.GroupHouses where g.Id_group == id_elem select g.Id_house;

                        var rows = (nReportType == 1) ? (from flds in ctx.Fields
                                                         join f in ctx.Forms on flds.Id_form equals f.Id
                                                         join v in ctx.Values on new { fldsId = flds.Id, fId = f.Id } equals new { fldsId = v.Id_field, fId = v.Id_form }
                                                         where v.Id_house == id_elem
                                                         select new { key = "[" + f.Title + "].[" + flds.Title + "]", val = v.Value, t = flds.Id_dict })
                                                 : (from flds in ctx.Fields
                                                    join f in ctx.Forms on flds.Id_form equals f.Id
                                                    join v in ctx.Values on new { fldsId = flds.Id, fId = f.Id } equals new { fldsId = v.Id_field, fId = v.Id_form }
                                                    where groupHouses.Contains(v.Id_house)
                                                    select new { key = "[" + f.Title + "].[" + flds.Title + "]", val = v.Value, t = flds.Id_dict });

                        foreach (var r in rows)
                        {
                            if (dict.ContainsKey(r.key))
                            {
                                if (dict[r.key] != "#error")
                                {
                                    if (r.t == 1002 && !string.IsNullOrEmpty(r.val))
                                    {
                                        int tmp = 0;
                                        if (int.TryParse(r.val, out tmp))
                                            dict[r.key] = (int.Parse(dict[r.key]) + tmp).ToString();
                                        else
                                            dict[r.key] = "#error";
                                    }
                                    else if (r.t == 1003 && !string.IsNullOrEmpty(r.val))
                                    {
                                        double tmp = 0;
                                        if (double.TryParse(r.val, out tmp))
                                            dict[r.key] = (double.Parse(dict[r.key]) + tmp).ToString();
                                        else
                                            dict[r.key] = "#error";
                                    }
                                    else
                                    {
                                        dict[r.key] = "#error";
                                    }
                                }
                            }
                            else
                            {
                                if (r.t == 1002)
                                {
                                    if (string.IsNullOrEmpty(r.val))
                                    {
                                        dict.Add(r.key, "0");
                                        continue;
                                    }
                                    int tmp = 0;
                                    if (int.TryParse(r.val, out tmp))
                                        dict.Add(r.key, tmp.ToString());
                                    else
                                        dict.Add(r.key, "#error");
                                }
                                else if (r.t == 1003)
                                {
                                    if (string.IsNullOrEmpty(r.val))
                                    {
                                        dict.Add(r.key, "0");
                                        continue;
                                    }
                                    double tmp = 0;
                                    if (double.TryParse(r.val, out tmp))
                                        dict.Add(r.key, tmp.ToString());
                                    else
                                        dict.Add(r.key, "#error");
                                }
                                else if (r.t >= 1001 && r.t <= 1004)
                                {
                                    dict.Add(r.key, r.val);
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(r.val))
                                    {
                                        dict.Add(r.key, "");
                                        continue;
                                    }
                                    int tmp = 0;
                                    if (int.TryParse(r.val, out tmp))
                                    {
                                        var val = context.DictsValues.Find(tmp);
                                        if (val != null)
                                            dict.Add(r.key, val.Value);
                                    }
                                    else
                                        dict.Add(r.key, "#error");
                                }
                            }
                        }
                    }
                }

                return dict;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("UI.Classes.DetailReportBuilder.CreateDict(int id_elem, int nReportType) Exception: {0}", ex.Message));
            }
        }
    }
}