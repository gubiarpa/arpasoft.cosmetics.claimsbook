using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using yanbal.claimsbook.data.Models;

namespace yanbal.claimsbook.web.Utils.Hash
{
    public static class Sha256
    {
        #region Algorithm
        private const string password = "y4nb4l#cl41m5.2020$";

        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="plainText">String to be encrypted</param>
        /// <param name="password">Password</param>
        public static string Sha256Encrypt(this string plainText, string password = password)
        {
            if (plainText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = String.Empty;
            }

            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        /// <summary>
        /// Decrypt a string.
        /// </summary>
        /// <param name="encryptedText">String to be decrypted</param>
        /// <param name="password">Password used during encryption</param>
        /// <exception cref="FormatException"></exception>
        public static string Sha256Decrypt(this string encryptedText, string password = password)
        {
            if (encryptedText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = String.Empty;
            }

            // Get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        #endregion

        #region Extension
        public static Claimer Sha256Encript(this Claimer claimer)
        {
            return new Claimer()
            {
                ID = claimer.ID,
                DocumentTypeID = claimer.DocumentTypeID,
                DocumentNumber = claimer.DocumentNumber.Sha256Encrypt(),
                Names = claimer.Names.Sha256Encrypt(),
                PaternalSurname = claimer.PaternalSurname.Sha256Encrypt(),
                MaternalSurname = claimer.MaternalSurname.Sha256Encrypt(),
                AnswerTypeID = claimer.AnswerTypeID,
                PhoneNumber = claimer.PhoneNumber.Sha256Encrypt(),
                EMail = claimer.EMail.Sha256Encrypt(),
                Address = claimer.Address.Sha256Encrypt(),
                GeoZoneID = claimer.GeoZoneID
            };
        }

        public static Claimer Sha256Decrypt(this Claimer claimer)
        {
            return new Claimer()
            {
                ID = claimer.ID,
                DocumentTypeID = claimer.DocumentTypeID,
                DocumentNumber = claimer.DocumentNumber.Sha256Decrypt(),
                Names = claimer.Names.Sha256Decrypt(),
                PaternalSurname = claimer.PaternalSurname.Sha256Decrypt(),
                MaternalSurname = claimer.MaternalSurname.Sha256Decrypt(),
                AnswerTypeID = claimer.AnswerTypeID,
                PhoneNumber = claimer.PhoneNumber.Sha256Decrypt(),
                EMail = claimer.EMail.Sha256Decrypt(),
                Address = claimer.Address.Sha256Decrypt(),
                GeoZoneID = claimer.GeoZoneID
            };
        }
        #endregion
    }
}
