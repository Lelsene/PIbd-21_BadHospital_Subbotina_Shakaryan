namespace HospitalServiceDAL.Interfaces
{
    public interface IBackUpService
    {
        void AdminBackUpXML();

        void AdminBackUpJSON();

        void PatientBackUpXML(int PatientId);

        void PatientBackUpJSON(int PatientId);

        void SendEmail(string mailAddress, string subject, string text, string[] path);
    }
}
