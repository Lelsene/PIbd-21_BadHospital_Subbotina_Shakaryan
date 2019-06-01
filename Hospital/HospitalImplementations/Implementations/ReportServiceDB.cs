using HospitalServiceDAL.BindingModels;
using HospitalServiceDAL.Interfaces;
using HospitalServiceDAL.ViewModels;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public List<PatientTreatmentsViewModel> GetPatientTreatments(ReportBindingModel model, int PatientId)
        {
            return context.Treatments
                            .Include(rec => rec.Patient)
                            .Where(rec => rec.Patient.Id == PatientId)
                            .Select(rec => new PatientTreatmentsViewModel
                            {
                                FIO = rec.Patient.FIO,
                                isReserved = rec.isReserved,
                                prescriptionList = rec.TreatmentPrescriptions.Select(recp => new PrescriptionViewModel
                                {
                                    PrescriptionMedications = recp.Prescription.PrescriptionMedications.Select(recm => new PrescriptionMedicationViewModel
                                    {
                                        MedicationName = recm.Medication.Name,
                                        CountMedications = recm.CountMedications * recp.Count
                                    }).ToList()
                                }).ToList()
                            }).ToList();
        }


        public List<PatientTreatmentsViewModel> GetPatientsTreatments(ReportBindingModel model)
        {
            return context.Treatments
                            .Include(rec => rec.Patient)
                            .Where(rec => rec.Date >= model.DateFrom && rec.Date <= model.DateTo && rec.isReserved == true)
                            .Select(rec => new PatientTreatmentsViewModel
                            {
                                FIO = rec.Patient.FIO,
                                isReserved = rec.isReserved,
                                prescriptionList = rec.TreatmentPrescriptions.Select(recp => new PrescriptionViewModel
                                {
                                    PrescriptionMedications = recp.Prescription.PrescriptionMedications.Select(recm => new PrescriptionMedicationViewModel
                                    {
                                        MedicationName = recm.Medication.Name,
                                        CountMedications = recm.CountMedications * recp.Count
                                    }).ToList()
                                }).ToList()
                            }).ToList();
        }

        public void SavePatientTreatments(ReportBindingModel model, int PatientId)
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

                excelcells = excelcells.get_Offset(1, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Пациент";
                excelcells = excelcells.get_Offset(0, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Лекарство";
                excelcells = excelcells.get_Offset(0, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Количество";
                excelcells = excelcells.get_Offset(0, -1);

                var dict = GetPatientTreatments(model, PatientId);
                int i = 0;
                if (dict != null)
                {
                    foreach (var pt in dict)
                    {
                        if (i == dict.Count - 1)
                        {
                            excelcells = excelcells.get_Offset(2, 0);
                            excelcells.Value2 = pt.FIO;
                            excelcells = excelcells.get_Offset(0, 1);
                            if (pt.prescriptionList.Count() > 0)
                            {
                                foreach (var pr in pt.prescriptionList)
                                {
                                    var excelBorder =
                                    excelworksheet.get_Range(excelcells,
                                                excelcells.get_Offset(pr.PrescriptionMedications.Count() - 1, 1));
                                    excelBorder.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                    excelBorder.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                                    excelBorder.HorizontalAlignment = Constants.xlCenter;
                                    excelBorder.VerticalAlignment = Constants.xlCenter;
                                    excelBorder.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                                                            Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                                                            Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic,
                                                            1);
                                    foreach (var pm in pr.PrescriptionMedications)
                                    {
                                        excelcells.Value2 = pm.MedicationName;
                                        excelcells = excelcells.get_Offset(0, 1);
                                        excelcells.Value2 = pm.CountMedications;
                                        excelcells = excelcells.get_Offset(1, -1);
                                    }
                                }
                            }
                            excelcells = excelcells.get_Offset(0, -1);
                        }
                        i++;
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
            }
        }

        public void SavePatientsTreatments(ReportBindingModel model)
        {
            SaveRequestLoad(model);
            Thread.Sleep(5);
            var excel = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                excel.Workbooks.Open(model.FileName, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing);
                Sheets excelsheets = excel.Workbooks[1].Worksheets;
                var excelworksheet = (Worksheet)excelsheets.get_Item(1);
                excelworksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                excelworksheet.PageSetup.CenterHorizontally = true;
                excelworksheet.PageSetup.CenterVertically = true;
                Microsoft.Office.Interop.Excel.Range excelcells = excelworksheet.get_Range("B1", "B1");
                int lastRow = excelworksheet.UsedRange.Rows.Count;

                excelcells = excelcells.get_Offset(lastRow + 1, 0);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Пациент";
                excelcells = excelcells.get_Offset(0, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Лекарство";
                excelcells = excelcells.get_Offset(0, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Количество";
                excelcells = excelcells.get_Offset(-1, -2);

                var dict = GetPatientsTreatments(model);
                if (dict != null)
                {
                    foreach (var pt in dict)
                    {
                        excelcells = excelcells.get_Offset(2, 0);
                        excelcells.Value2 = pt.FIO;
                        excelcells = excelcells.get_Offset(0, 1);
                        if (pt.prescriptionList.Count() > 0)
                        {
                            foreach (var pr in pt.prescriptionList)
                            {
                                var excelBorder =
                                excelworksheet.get_Range(excelcells,
                                            excelcells.get_Offset(pr.PrescriptionMedications.Count() - 1, 1));
                                excelBorder.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                excelBorder.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                                excelBorder.HorizontalAlignment = Constants.xlCenter;
                                excelBorder.VerticalAlignment = Constants.xlCenter;
                                excelBorder.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                                                        Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                                                        Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic,
                                                        1);
                                foreach (var pm in pr.PrescriptionMedications)
                                {
                                    excelcells.Value2 = pm.MedicationName;
                                    excelcells = excelcells.get_Offset(0, 1);
                                    excelcells.Value2 = pm.CountMedications;
                                    excelcells = excelcells.get_Offset(1, -1);
                                }
                            }
                        }
                        excelcells = excelcells.get_Offset(0, -1);
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
            }

        }

        public List<RequestLoadViewModel> GetRequestLoad(ReportBindingModel model)
        {
            return context.Requests.Where(rec => rec.Date >= model.DateFrom && rec.Date <= model.DateTo)
                            .ToList()
                            .GroupJoin(
                                    context.RequestMedications.ToList(),
                                    request => request,
                                    requestMedications => requestMedications.Request,
                                    (request, requestMedications) =>
            new RequestLoadViewModel
            {
                RequestName = request.RequestName,
                Medications = requestMedications.Select(r => new Tuple<string, int>(r.MedicationName, r.CountMedications))
            }).ToList();
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
                excelcells = excelcells.get_Offset(0, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Название заявки";
                excelcells = excelcells.get_Offset(0, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Лекарство";
                excelcells = excelcells.get_Offset(0, 1);
                excelcells.Interior.Color = Color.Yellow;
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Количество";
                excelcells = excelcells.get_Offset(-2, -2);

                var dict = GetRequestLoad(model);
                if (dict != null)
                {
                    excelcells = excelworksheet.get_Range("C2", "C2");
                    foreach (var elem in dict)
                    {
                        excelcells = excelcells.get_Offset(2, -1);
                        excelcells.ColumnWidth = 30;
                        excelcells.Value2 = elem.RequestName;
                        excelcells = excelcells.get_Offset(0, 1);
                        if (elem.Medications.Count() > 0)
                        {
                            var excelBorder =
                                excelworksheet.get_Range(excelcells,
                                            excelcells.get_Offset(elem.Medications.Count() - 1, 1));
                            excelBorder.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                            excelBorder.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                            excelBorder.HorizontalAlignment = Constants.xlCenter;
                            excelBorder.VerticalAlignment = Constants.xlCenter;
                            excelBorder.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                                                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                                                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic,
                                                    1);

                            foreach (var listElem in elem.Medications)
                            {
                                excelcells.Value2 = listElem.Item1;
                                excelcells.ColumnWidth = 20;
                                excelcells.get_Offset(0, 1).Value2 = listElem.Item2;
                                excelcells = excelcells.get_Offset(1, 0);
                            }
                        }
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
            }

        }
    }
}
