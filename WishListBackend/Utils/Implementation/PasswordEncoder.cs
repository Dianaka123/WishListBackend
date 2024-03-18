using System.Text;
using System.Security.Cryptography;
using WishListBackend.Other.Interfaces;

namespace WishListBackend.Other.Implementation
{
    public class PasswordEncoder : IPasswordEncoder
    {
        private const string EncryptionKey = "MAKVKKBNI99212";

        private byte[] _salt = [0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76];

        public string Encode(string password)
        {
            var passwordInBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes encryptAlgorythm = new Rfc2898DeriveBytes(EncryptionKey, _salt);
                encryptor.Key = encryptAlgorythm.GetBytes(32);
                encryptor.IV = encryptAlgorythm.GetBytes(16);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(passwordInBytes, 0, passwordInBytes.Length);
                        cryptoStream.Close();
                    }
                    password = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            return password;
        }

        public string Decode(string encryptedPassword)
        {
            var encryptedPasswordString = Convert.FromBase64String(encryptedPassword);
            var decryptedPassword = "";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes encryptAlgorothm = new Rfc2898DeriveBytes(EncryptionKey, _salt);
                encryptor.Key = encryptAlgorothm.GetBytes(32);
                encryptor.IV = encryptAlgorothm.GetBytes(16);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedPasswordString, 0, encryptedPasswordString.Length);
                        cryptoStream.Close();
                    }
                    decryptedPassword = Encoding.Unicode.GetString(memoryStream.ToArray());
                }
            }
            return decryptedPassword;
        }
    }
}
