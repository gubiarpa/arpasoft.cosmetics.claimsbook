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

        public static string Encrypt(this string text)
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

        public static string Decrypt(this string cipher)
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

        public static Claimer Encript(this Claimer claimer)
        {
            return new Claimer()
            {
                ID = claimer.ID,
                DocumentTypeID = claimer.DocumentTypeID,
                DocumentNumber = claimer.DocumentNumber.Encrypt(),
                Names = claimer.Names.Encrypt(),
                PaternalSurname = claimer.PaternalSurname.Encrypt(),
                MaternalSurname = claimer.MaternalSurname.Encrypt(),
                AnswerTypeID = claimer.AnswerTypeID,
                PhoneNumber = claimer.PhoneNumber.Encrypt(),
                EMail = claimer.EMail.Encrypt(),
                Address = claimer.Address.Encrypt(),
                GeoZoneID = claimer.GeoZoneID
            };
        }

        public static Claimer Decrypt(this Claimer claimer)
        {
            return new Claimer()
            {
                ID = claimer.ID,
                DocumentTypeID = claimer.DocumentTypeID,
                DocumentNumber = claimer.DocumentNumber.Decrypt(),
                Names = claimer.Names.Decrypt(),
                PaternalSurname = claimer.PaternalSurname.Decrypt(),
                MaternalSurname = claimer.MaternalSurname.Decrypt(),
                AnswerTypeID = claimer.AnswerTypeID,
                PhoneNumber = claimer.PhoneNumber.Decrypt(),
                EMail = claimer.EMail.Decrypt(),
                Address = claimer.Address.Decrypt(),
                GeoZoneID = claimer.GeoZoneID
            };
        }
    }
}
