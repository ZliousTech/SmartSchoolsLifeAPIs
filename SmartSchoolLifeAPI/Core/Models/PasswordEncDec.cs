using System;
using System.Text;

namespace SmartSchoolLifeAPI.Core.Models
{
    public class PasswordEncDec
    {
        public static string Encrypt(string Password)
        {
            string sMessage = string.Empty;
            byte[] encode = new byte[Password.Length - 1];
            encode = Encoding.UTF8.GetBytes(Password);
            sMessage = Convert.ToBase64String(encode);
            return sMessage;

        }
        public static string Decrypt(string EncryptPassword)
        {
            string DecryptPassword = string.Empty;
            UTF8Encoding EncodePassword = new UTF8Encoding();
            Decoder Decode = EncodePassword.GetDecoder();
            byte[] ToDecode_Byte = Convert.FromBase64String(EncryptPassword);
            int CharCount = Decode.GetCharCount(ToDecode_Byte, 0, ToDecode_Byte.Length);
            char[] Decoded_Char = new char[CharCount - 1 + 1];
            Decode.GetChars(ToDecode_Byte, 0, ToDecode_Byte.Length, Decoded_Char, 0);
            DecryptPassword = new string(Decoded_Char);
            return DecryptPassword;
        }

    }
}