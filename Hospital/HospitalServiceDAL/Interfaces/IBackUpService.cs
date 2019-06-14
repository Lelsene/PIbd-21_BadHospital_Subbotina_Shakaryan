namespace HospitalServiceDAL.Interfaces
{
    public interface IBackUpService
    {
        void AdminBackUpXML();

        void AdminBackUpJSON();

        void SendEmail(string mailAddress, string subject, string text, string[] path);
    }
}
