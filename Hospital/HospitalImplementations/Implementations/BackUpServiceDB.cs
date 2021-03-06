﻿using HospitalModel;
using HospitalServiceDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Json;
using System.Xml;

namespace HospitalImplementations.Implementations
{
    public class BackUpServiceDB : IBackUpService
    {
        private HospitalDBContext context;

        public BackUpServiceDB(HospitalDBContext context)
        {
            this.context = context;
        }

        public BackUpServiceDB()
        {
            this.context = new HospitalDBContext();
        }

        public void AdminBackUpXML()
        {
            var medications = context.Medications.ToList();
            var presmed = context.PrescriptionMedications.ToList();
            var prescriptions = context.Prescriptions.ToList();

            string pathM = "C:\\Users\\Шонова\\Desktop\\MedicationsXML.xml";
            string pathPM = "C:\\Users\\Шонова\\Desktop\\PrescriptionMedicationsXML.xml";
            string pathP = "C:\\Users\\Шонова\\Desktop\\PrescriptionsXML.xml";

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (FileStream fileStream = new FileStream(pathM, FileMode.Create))
            {
                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    writer.WriteStartElement("Medications");
                    foreach (var med in medications)
                    {
                        writer.WriteStartElement("Medication");
                        writer.WriteElementString("Id", med.Id.ToString());
                        writer.WriteElementString("Name", med.Name);
                        writer.WriteElementString("Price", med.Price.ToString());
                        writer.WriteElementString("Count", med.Count.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }

            using (FileStream fileStream = new FileStream(pathPM, FileMode.Create))
            {
                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    writer.WriteStartElement("PrescriptionMedications");
                    foreach (var pm in presmed)
                    {
                        writer.WriteStartElement("PrescriptionMedication");
                        writer.WriteElementString("Id", pm.Id.ToString());
                        writer.WriteElementString("PrescriptionId", pm.PrescriptionId.ToString());
                        writer.WriteElementString("MedicationId", pm.MedicationId.ToString());
                        writer.WriteElementString("MedicationName", pm.MedicationName);
                        writer.WriteElementString("CountMedications", pm.CountMedications.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }

            using (FileStream fileStream = new FileStream(pathP, FileMode.Create))
            {
                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    writer.WriteStartElement("Prescriptions");
                    foreach (var pres in prescriptions)
                    {
                        writer.WriteStartElement("Prescription");
                        writer.WriteElementString("Id", pres.Id.ToString());
                        writer.WriteElementString("Title", pres.Title);
                        writer.WriteElementString("Price", pres.Price.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
            SendEmail("lelsene@mail.ru", "Бекап БД в формате XML", "", new string[] { pathM, pathPM, pathP });
        }

        public void AdminBackUpJSON()
        {
            string pathM = "C:\\Users\\Шонова\\Desktop\\MedicationsJSON.json";
            string pathPM = "C:\\Users\\Шонова\\Desktop\\PrescriptionMedicationsJSON.json";
            string pathP = "C:\\Users\\Шонова\\Desktop\\PrescriptionsJSON.json";

            var medications = context.Medications.ToList();
            var presmed = context.PrescriptionMedications.ToList();
            var prescriptions = context.Prescriptions.ToList();

            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Medication>));
            using (FileStream fs = new FileStream(pathM, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, medications);
            }

            jsonFormatter = new DataContractJsonSerializer(typeof(List<PrescriptionMedication>));
            using (FileStream fs = new FileStream(pathPM, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, presmed);
            }

            jsonFormatter = new DataContractJsonSerializer(typeof(List<Prescription>));
            using (FileStream fs = new FileStream(pathP, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, prescriptions);
            }

            SendEmail("lelsene@mail.ru", "Бекап БД в формате JSON", "", new string[] { pathM, pathPM, pathP });
        }

        public void PatientBackUpXML(int PatientId)
        {
            var treatments = context.Treatments.Where(rec => rec.PatientId == PatientId).ToList();
            var trprs = new List<TreatmentPrescription>();
            foreach (var t in treatments)
            {
                foreach (var m in context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == t.Id).ToList())
                {
                    trprs.Add(m);
                }
            }
            var prescriptions = context.Prescriptions.ToList();

            string pathT = "C:\\Users\\Шонова\\Desktop\\TreatmentsXML.xml";
            string pathTP = "C:\\Users\\Шонова\\Desktop\\TreatmentPrescriptionsXML.xml";
            string pathP = "C:\\Users\\Шонова\\Desktop\\PrescriptionsXML.xml";

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (FileStream fileStream = new FileStream(pathT, FileMode.Create))
            {
                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    writer.WriteStartElement("Treatments");
                    foreach (var treat in treatments)
                    {
                        writer.WriteStartElement("Treatment");
                        writer.WriteElementString("Id", treat.Id.ToString());
                        writer.WriteElementString("Date", treat.Date.ToShortDateString());
                        writer.WriteElementString("Title", treat.Title);
                        writer.WriteElementString("TotalCost", treat.TotalCost.ToString());
                        writer.WriteElementString("isReserved", treat.isReserved.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }

            using (FileStream fileStream = new FileStream(pathTP, FileMode.Create))
            {
                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    writer.WriteStartElement("TreatmentPrescriptions");
                    foreach (var tp in trprs)
                    {
                        writer.WriteStartElement("TreatmentPrescription");
                        writer.WriteElementString("Id", tp.Id.ToString());
                        writer.WriteElementString("TreatmentId", tp.TreatmentId.ToString());
                        writer.WriteElementString("PrescriptionId", tp.PrescriptionId.ToString());
                        writer.WriteElementString("PrescriptionTitle", tp.PrescriptionTitle);
                        writer.WriteElementString("Count", tp.Count.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }

            using (FileStream fileStream = new FileStream(pathP, FileMode.Create))
            {
                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    writer.WriteStartElement("Prescriptions");
                    foreach (var pres in prescriptions)
                    {
                        writer.WriteStartElement("Prescription");
                        writer.WriteElementString("Id", pres.Id.ToString());
                        writer.WriteElementString("Title", pres.Title);
                        writer.WriteElementString("Price", pres.Price.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }

            Patient patient = context.Patients.FirstOrDefault(rec => rec.Id == PatientId);
            SendEmail(patient.Email, "Бекап БД в формате XML", "", new string[] { pathT, pathTP, pathP });
        }

        public void PatientBackUpJSON(int PatientId)
        {
            string pathT = "C:\\Users\\Шонова\\Desktop\\TreatmentsJSON.json";
            string pathTP = "C:\\Users\\Шонова\\Desktop\\TreatmentPrescriptionsJSON.json";
            string pathP = "C:\\Users\\Шонова\\Desktop\\PrescriptionsJSON.json";


            var treatments = context.Treatments.Where(rec => rec.PatientId == PatientId).ToList();
            var trprs = new List<TreatmentPrescription>();
            foreach (var t in treatments)
            {
                foreach (var m in context.TreatmentPrescriptions.Where(rec => rec.TreatmentId == t.Id).ToList())
                {
                    trprs.Add(m);
                }
            }
            var prescriptions = context.Prescriptions.ToList();

            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Treatment>));
            using (FileStream fs = new FileStream(pathT, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, treatments);
            }

            jsonFormatter = new DataContractJsonSerializer(typeof(List<TreatmentPrescription>));
            using (FileStream fs = new FileStream(pathTP, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, trprs);
            }

            jsonFormatter = new DataContractJsonSerializer(typeof(List<Prescription>));
            using (FileStream fs = new FileStream(pathP, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, prescriptions);
            }

            Patient patient = context.Patients.FirstOrDefault(rec => rec.Id == PatientId);
            SendEmail(patient.Email, "Бекап БД в формате JSON", "", new string[] { pathT, pathTP, pathP });
        }

        public void SendEmail(string mailAddress, string subject, string text, string[] path)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = null;
            try
            {
                objMailMessage.From = new MailAddress("labwork15kafis@gmail.com");
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                foreach (var el in path)
                {
                    objMailMessage.Attachments.Add(new Attachment(el));
                }

                objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new
                NetworkCredential("labwork15kafis@gmail.com", "passlab15");

                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }
    }
}
