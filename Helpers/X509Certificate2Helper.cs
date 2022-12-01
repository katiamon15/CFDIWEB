using System.Security.Cryptography.X509Certificates;

namespace CFDIWEB.Helpers
{

    public static class X509Certificate2Helper
    {
        public static X509Certificate2 GetCertificate(byte[] certificate, string password)
        {
            return new X509Certificate2(certificate,
                password,
                X509KeyStorageFlags.UserKeySet);
        }
    }
}
