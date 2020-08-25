
using DVG.WIS.Utilities;

namespace FileManager.Common
{
    public class Config
    {

        public static string AESKey = AppSettings.Instance.GetString("AES_Key");

        public static string AESIV = AppSettings.Instance.GetString("AES_IV");

        public static string UploadDomain = AppSettings.Instance.GetString("UploadDomain");

        public static string ViewDomain = AppSettings.Instance.GetString("DomainImage");

        public static string UploadHandler = AppSettings.Instance.GetString("UploadHandler");

        public static string UploadProject = AppSettings.Instance.GetString("UploadProject");

        public static string LoadFileApi = AppSettings.Instance.GetString("LoadFileApi");

        public static string FullUploadHandler
        {
            get
            {
                return string.Concat(UploadDomain, UploadHandler);
            }
        }

        public static string FullLoadFileApi
        {
            get
            {
                return string.Concat(UploadDomain, LoadFileApi);
            }
        }
    }
}
