using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace HospitalImplementations.Implementations
{
    public class ReportServiceDB : IReportService
    {
        private HospitalDBContext context;

        public ReportServiceDB(HospitalDBContext context)
        {
            this.context = context;
        }

        public ReportServiceDB()
        {
            this.context = new HospitalDBContext();
        }

        public List<ReportViewModel> GetReport(ReportBindingModel model)
        {
            List<ReportViewModel> list = new List<ReportViewModel>();

            list.AddRange(GetRequests(model));

            list.AddRange(GetTreatments(model, -1));

            return list;
        }

        public List<ReportViewModel> GetRequests(ReportBindingModel model)
        {
            List<ReportViewModel> list = new List<ReportViewModel>();

            foreach (var o in context.Requests.Where(rec => rec.Date >= model.DateFrom && rec.Date <= model.DateTo))
            {
                int i = 0;
                foreach (var med in context.RequestMedications.Where(rec => rec.RequestId == o.Id))
                {
                    ReportViewModel rep = new ReportViewModel();
                    if (i < 1)
                    {
                        rep.FIO = "Admin";
                        rep.Title = o.RequestName;
                        rep.Date = o.Date.ToShortDateString();
                    }
                    else
                    {
                        rep.FIO = " ";
                        rep.Title = " ";
                        rep.Date = " ";
                    }
                    rep.MedicationName = med.MedicationName;
                    rep.MedicationCount = med.CountMedications;
                    list.Add(rep);
                    i++;
                }
            }
            return list;
        }

        public List<ReportViewModel> GetTreatments(ReportBindingModel model, int PatientId)
        {
            List<ReportViewModel> list = new List<ReportViewModel>();

            if (PatientId != -1)
            {
                foreach (var o in context.Treatments.Where(rec => rec.Date >= model.DateFrom && rec.Date <= model.DateTo && rec.PatientId == PatientId))
                {
                    foreach (var p in context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == o.Id))
                    {
                        int i = 0;
                        foreach (var med in context.PrescriptionMedications.Where(rec => rec.PrescriptionId == p.PrescriptionId))
                        {
                            ReportViewModel rep = new ReportViewModel();
                            if (i < 1)
                            {
                                rep.FIO = context.Patients.FirstOrDefault(rec => rec.Id == o.PatientId).FIO;
                                rep.Title = o.Title;
                                rep.Date = o.Date.ToShortDateString();
                            }
                            else
                            {
                                rep.FIO = " ";
                                rep.Title = " ";
                                rep.Date = " ";
                            }
                            rep.MedicationName = med.MedicationName;
                            rep.MedicationCount = med.CountMedications * p.Count;
                            list.Add(rep);
                            i++;
                        }
                    }
                }
            }
            else
            {
                foreach (var o in context.Treatments.Where(rec => rec.Date >= model.DateFrom && rec.Date <= model.DateTo))
                {
                    foreach (var p in context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == o.Id))
                    {
                        int i = 0;
                        foreach (var med in context.PrescriptionMedications.Where(rec => rec.PrescriptionId == p.PrescriptionId))
                        {
                            ReportViewModel rep = new ReportViewModel();
                            if (i < 1)
                            {
                                rep.FIO = context.Patients.FirstOrDefault(rec => rec.Id == o.PatientId).FIO;
                                rep.Title = o.Title;
                                rep.Date = o.Date.ToShortDateString();
                            }
                            else
                            {
                                rep.FIO = " ";
                                rep.Title = " ";
                                rep.Date = " ";
                            }
                            rep.MedicationName = med.MedicationName;
                            rep.MedicationCount = med.CountMedications * p.Count;
                            list.Add(rep);
                            i++;
                        }
                    }
                }
            }
            return list;
        }

        public void SaveReport(ReportBindingModel model)
        {
            FileStream fs = new FileStream(model.FileName, FileMode.OpenOrCreate, FileAccess.Write);

            //создаем документ, задаем границы, связываем документ и поток
            iTextSharp.text.Document doc = new iTextSharp.text.Document();

            try
            {
                //открываем файл для работы                
                doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                doc.Open();
                BaseFont baseFont = BaseFont.CreateFont("G:\\Desktop\\ПИбд\\4 СЕМЕСТР\\ТП\\TIMCYR.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                //вставляем заголовок
                var phraseTitle = new Phrase("Отчет",
                new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
                iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(phraseTitle)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 12
                };
                doc.Add(paragraph);

                var phrasePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString() +
                                              " по " + model.DateTo.Value.ToShortDateString(),
                                              new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD));

                paragraph = new iTextSharp.text.Paragraph(phrasePeriod)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 12
                };
                doc.Add(paragraph);

                //вставляем таблицу, задаем количество столбцов, и ширину колонок
                PdfPTable table = new PdfPTable(5)
                {
                    TotalWidth = 800F
                };
                table.SetTotalWidth(new float[] { 160, 100, 120, 180, 120 });

                //вставляем шапку
                PdfPCell cell = new PdfPCell();
                var fontForCellBold = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
                table.AddCell(new PdfPCell(new Phrase("Название", fontForCellBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                table.AddCell(new PdfPCell(new Phrase("Дата", fontForCellBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                table.AddCell(new PdfPCell(new Phrase("Имя", fontForCellBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                table.AddCell(new PdfPCell(new Phrase("Лекарство", fontForCellBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                table.AddCell(new PdfPCell(new Phrase("Количество", fontForCellBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                //заполняем таблицу
                var list = GetReport(model);
                var fontForCells = new iTextSharp.text.Font(baseFont, 10);

                foreach (var el in list)
                {
                    cell = new PdfPCell(new Phrase(el.Title, fontForCells));
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(el.Date, fontForCells));
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(el.FIO, fontForCells));
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(el.MedicationName, fontForCells));
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(el.MedicationCount.ToString(), fontForCells));
                    table.AddCell(cell);
                }

                doc.Add(table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                doc.Close();
                Thread.Sleep(5);
            }
        }

        public void SavePatientTreatments(ReportBindingModel model, int PatientId)
        {
            //var excel = new Microsoft.Office.Interop.Excel.Application();
            //try
            //{
            //    if (File.Exists(model.FileName))
            //    {
            //        excel.Workbooks.Open(model.FileName, Type.Missing, Type.Missing, Type.Missing,
            //            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            //            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            //            Type.Missing);
            //    }
            //    else
            //    {
            //        excel.SheetsInNewWorkbook = 1;
            //        excel.Workbooks.Add(Type.Missing);
            //        excel.Workbooks[1].SaveAs(model.FileName, XlFileFormat.xlExcel8, Type.Missing,
            //            Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing,
            //            Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //    }
            //    Sheets excelsheets = excel.Workbooks[1].Worksheets;
            //    var excelworksheet = (Worksheet)excelsheets.get_Item(1);
            //    excelworksheet.Cells.Clear();
            //    excelworksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
            //    excelworksheet.PageSetup.CenterHorizontally = true;
            //    excelworksheet.PageSetup.CenterVertically = true;
            //    Microsoft.Office.Interop.Excel.Range excelcells = excelworksheet.get_Range("A1", "E1");
            //    excelcells.Merge(Type.Missing);
            //    excelcells.Font.Bold = true;
            //    excelcells.Value2 = "Отчет";
            //    excelcells.RowHeight = 25;
            //    excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //    excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
            //    excelcells.Font.Name = "Times New Roman";
            //    excelcells.Font.Size = 14;

            //    excelcells = excelworksheet.get_Range("A2", "E2");
            //    excelcells.Merge(Type.Missing);
            //    excelcells.Value2 = DateTime.Now.ToShortDateString();
            //    excelcells.RowHeight = 20;
            //    excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //    excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
            //    excelcells.Font.Name = "Times New Roman";
            //    excelcells.Font.Size = 12;

            //    excelcells = excelcells.get_Offset(1, 1);
            //    excelcells.Interior.Color = Color.Yellow;
            //    excelcells.Font.Bold = true;
            //    excelcells.Value2 = "Пациент";
            //    excelcells = excelcells.get_Offset(0, 1);
            //    excelcells.Interior.Color = Color.Yellow;
            //    excelcells.Font.Bold = true;
            //    excelcells.Value2 = "Лекарство";
            //    excelcells = excelcells.get_Offset(0, 1);
            //    excelcells.Interior.Color = Color.Yellow;
            //    excelcells.Font.Bold = true;
            //    excelcells.Value2 = "Количество";
            //    excelcells = excelcells.get_Offset(0, -1);

            //    var dict = GetPatientTreatments(model, PatientId);
            //    int i = 0;
            //    if (dict != null)
            //    {
            //        foreach (var pt in dict)
            //        {
            //            if (i == dict.Count - 1)
            //            {
            //                excelcells = excelcells.get_Offset(2, 0);
            //                excelcells.Value2 = pt.FIO;
            //                excelcells = excelcells.get_Offset(0, 1);
            //                if (pt.prescriptionList.Count() > 0)
            //                {
            //                    foreach (var pr in pt.prescriptionList)
            //                    {
            //                        var excelBorder =
            //                        excelworksheet.get_Range(excelcells,
            //                                    excelcells.get_Offset(pr.PrescriptionMedications.Count() - 1, 1));
            //                        excelBorder.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            //                        excelBorder.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
            //                        excelBorder.HorizontalAlignment = Constants.xlCenter;
            //                        excelBorder.VerticalAlignment = Constants.xlCenter;
            //                        excelBorder.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
            //                                                Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
            //                                                Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic,
            //                                                1);
            //                        foreach (var pm in pr.PrescriptionMedications)
            //                        {
            //                            excelcells.Value2 = pm.MedicationName;
            //                            excelcells = excelcells.get_Offset(0, 1);
            //                            excelcells.Value2 = pm.CountMedications;
            //                            excelcells = excelcells.get_Offset(1, -1);
            //                        }
            //                    }
            //                }
            //                excelcells = excelcells.get_Offset(0, -1);
            //            }
            //            i++;
            //        }
            //    }
            //    excel.Workbooks[1].Save();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    throw;
            //}
            //finally
            //{
            //    excel.Quit();
            //    Thread.Sleep(5);
            //}
        }

        public void SaveRequestLoad(ReportBindingModel model)
        {
            var excel = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                if (File.Exists(model.FileName))
                {
                    excel.Workbooks.Open(model.FileName, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing);
                }
                else
                {
                    excel.SheetsInNewWorkbook = 1;
                    excel.Workbooks.Add(Type.Missing);
                    excel.Workbooks[1].SaveAs(model.FileName, XlFileFormat.xlExcel8, Type.Missing,
                        Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }

                Sheets excelsheets = excel.Workbooks[1].Worksheets;
                var excelworksheet = (Worksheet)excelsheets.get_Item(1);
                excelworksheet.Cells.Clear();
                excelworksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                excelworksheet.PageSetup.CenterHorizontally = true;
                excelworksheet.PageSetup.CenterVertically = true;
                Microsoft.Office.Interop.Excel.Range excelcells = excelworksheet.get_Range("A1", "E1");
                excelcells.Merge(Type.Missing);
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Отчет";
                excelcells.RowHeight = 25;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 14;

                excelcells = excelworksheet.get_Range("A2", "E2");
                excelcells.Merge(Type.Missing);
                excelcells.Value2 = DateTime.Now.ToShortDateString();
                excelcells.RowHeight = 20;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 12;

                excelcells = excelworksheet.get_Range("A3", "A3");
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.ColumnWidth = 15;
                excelcells.Value2 = "Название";
                excelcells = excelcells.get_Offset(0, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.ColumnWidth = 15;
                excelcells.Value2 = "Дата";
                excelcells = excelcells.get_Offset(0, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.ColumnWidth = 15;
                excelcells.Value2 = "Имя";
                excelcells = excelcells.get_Offset(0, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.ColumnWidth = 15;
                excelcells.Value2 = "Лекарство";
                excelcells = excelcells.get_Offset(0, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.ColumnWidth = 15;
                excelcells.Value2 = "Количество";
                excelcells = excelworksheet.get_Range("A4", "A4");

                var list = GetRequests(model);

                if (list != null)
                {
                    foreach (var el in list)
                    {
                        excelcells.Value2 = el.Title;
                        excelcells = excelcells.get_Offset(0, 1);

                        excelcells.Value2 = el.Date;
                        excelcells = excelcells.get_Offset(0, 1);

                        excelcells.Value2 = el.FIO;
                        excelcells = excelcells.get_Offset(0, 1);

                        excelcells.Value2 = el.MedicationName;
                        excelcells = excelcells.get_Offset(0, 1);

                        excelcells.Value2 = el.MedicationCount;
                        excelcells = excelcells.get_Offset(1, -4);
                    }
                }
                excel.Workbooks[1].Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                excel.Quit();
                Thread.Sleep(5);
            }
        }
    }
}