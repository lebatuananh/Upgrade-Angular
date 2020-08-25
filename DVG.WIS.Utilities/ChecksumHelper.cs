using DVG.WIS.Utilities.Serialization;
using System;
using System.Collections;
using System.ComponentModel;

namespace DVG.WIS.Utilities
{
    public class ChecksumResult
    {
        public ChecksumResult()
        {
            this.IsValid = false;
            this.ErrorCode = ChecksumErrorCode.Invalid;
            this.Message = Utils.GetEnumDescription(this.ErrorCode);
        }

        public ChecksumResult(ChecksumErrorCode errorCode)
        {
            this.IsValid = errorCode == ChecksumErrorCode.Success;
            this.ErrorCode = errorCode;
            this.Message = Utils.GetEnumDescription(errorCode);
        }

        public string Message { get; set; }
        public bool IsValid { get; set; }
        public ChecksumErrorCode ErrorCode { get; set; }

        public enum ChecksumErrorCode
        {
            [Description("Valid token")]
            Success = 0,
            [Description("Invalid token")]
            Invalid = 1,
            [Description("Token has been expired")]
            Expired = 2
        }
    }

    [Serializable]
    public class ChecksumModel<T>
    {
        public ChecksumModel(T entity, int expiredInMinute = 15)
        {
            string jsonEntity = NewtonJson.Serialize(entity);

            this.HashMD5 = Utils.CalculateMD5Hash(jsonEntity);
            this.ExpiredDate = System.DateTime.Now.AddMinutes(expiredInMinute);
        }

        public string HashMD5 { get; set; }

        public System.DateTime ExpiredDate { get; set; }

        public override string ToString()
        {
            return NewtonJson.Serialize(this);
        }

        public bool IsExpired()
        {
            return Utils.DateDiff(ExpiredDate, System.DateTime.Now, "m") > 0;
        }
    }

    public class ChecksumHelper
    {
        private static string privateKey = AppSettings.Instance.GetString("API_CheckSum_PrivateKey", Utils.CalculateMD5Hash("123456"));
        private static int checksumExpiredInMinute = AppSettings.Instance.GetInt32("API_CheckSum_ExpiredInMinute", 15);

        public static void SetPrivateKey(string key)
        {
            privateKey = key;
        }

        public static void SetExpiredInMinute(int expiredInMinute)
        {
            checksumExpiredInMinute = expiredInMinute;
        }

        public static string GenToken<T>(T entity, int? expiredInMinute = null)
        {
            string token = string.Empty;

            if (!expiredInMinute.HasValue) expiredInMinute = checksumExpiredInMinute;

            if (entity != null && !entity.Equals(default(T)))
            {
                ChecksumModel<T> checksumModel = new ChecksumModel<T>(entity, expiredInMinute.Value);

                token = Crypton.EncryptByKey(checksumModel.ToString(), privateKey);
            }

            return token;
        }

        public static ChecksumResult ValidateToken<T>(T entity, string token)
        {
            ChecksumResult result = new ChecksumResult();

            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    ChecksumModel<T> requestModel = new ChecksumModel<T>(entity);

                    string decryptString = Crypton.DecryptByKey(token, privateKey);

                    ChecksumModel<T> checksumDecrypt = NewtonJson.Deserialize<ChecksumModel<T>>(decryptString);

                    if (!checksumDecrypt.IsExpired())
                    {
                        bool valid = requestModel.HashMD5.Equals(checksumDecrypt.HashMD5);
                        if (valid)
                        {
                            result = new ChecksumResult(ChecksumResult.ChecksumErrorCode.Success);
                        }
                    }
                    else
                    {
                        result = new ChecksumResult(ChecksumResult.ChecksumErrorCode.Expired);
                    }
                }
                catch
                {
                    // Logs
                }
            }

            return result;
        }
    }
}
