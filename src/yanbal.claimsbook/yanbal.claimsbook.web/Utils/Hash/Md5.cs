using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using yanbal.claimsbook.data.Models;

namespace yanbal.claimsbook.web.Utils.Hash
{
    public static class Md5
    {
        private const string key = "A!9HHhi%XjjYY4YP2@Nob009X";

        public static string Md5Encrypt(this string text)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        public static string Md5Decrypt(this string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipher);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }

        public static Claimer Md5Encript(this Claimer claimer)
        {
            return new Claimer()
            {
                ID = claimer.ID,
                DocumentTypeID = claimer.DocumentTypeID,
                DocumentNumber = claimer.DocumentNumber.Md5Encrypt(),
                Names = claimer.Names.Md5Encrypt(),
                PaternalSurname = claimer.PaternalSurname.Md5Encrypt(),
                MaternalSurname = claimer.MaternalSurname.Md5Encrypt(),
                AnswerTypeID = claimer.AnswerTypeID,
                PhoneNumber = claimer.PhoneNumber.Md5Encrypt(),
                EMail = claimer.EMail.Md5Encrypt(),
                Address = claimer.Address.Md5Encrypt(),
                GeoZoneID = claimer.GeoZoneID
            };
        }

        public static Claimer Md5Decrypt(this Claimer claimer)
        {
            return new Claimer()
            {
                ID = claimer.ID,
                DocumentTypeID = claimer.DocumentTypeID,
                DocumentNumber = claimer.DocumentNumber.Md5Decrypt(),
                Names = claimer.Names.Md5Decrypt(),
                PaternalSurname = claimer.PaternalSurname.Md5Decrypt(),
                MaternalSurname = claimer.MaternalSurname.Md5Decrypt(),
                AnswerTypeID = claimer.AnswerTypeID,
                PhoneNumber = claimer.PhoneNumber.Md5Decrypt(),
                EMail = claimer.EMail.Md5Decrypt(),
                Address = claimer.Address.Md5Decrypt(),
                GeoZoneID = claimer.GeoZoneID
            };
        }
    }
}
