namespace OMR.Core.Helpers
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// OMR # 7/14/2012
    /// MD5, SHA1, SHA256 and SHA512 Hash Calculation & Check Helper
    /// </summary>
    public class HashHelper
    {
        public enum HashTypes
        {
            MD5,
            SHA1,
            SHA256,
            SHA512
        }

        /// <summary>
        /// Returns hash algorithm using hash type
        /// </summary>
        /// <param name="hashType"></param>
        /// <returns></returns>
        private static HashAlgorithm GetHashAlgorithm(HashTypes hashType)
        {
            HashAlgorithm hashAlgoritm = null;

            switch (hashType)
            {
                case HashTypes.MD5:
                    hashAlgoritm = new MD5CryptoServiceProvider();
                    break;
                case HashTypes.SHA1:
                    hashAlgoritm = new SHA1Managed();
                    break;
                case HashTypes.SHA256:
                    hashAlgoritm = new SHA256Managed();
                    break;
                case HashTypes.SHA512:
                    hashAlgoritm = new SHA512Managed();
                    break;
                default:
                    throw new NotImplementedException(hashType.ToString());
            }

            return hashAlgoritm;
        }

        /// <summary>
        /// Returns hash length using hash type
        /// </summary>
        /// <param name="hashType"></param>
        /// <returns></returns>
        private static int GetLength(HashTypes hashType)
        {
            int length;

            switch (hashType)
            {
                case HashTypes.MD5:
                    length = 32;
                    break;
                case HashTypes.SHA1:
                    length = 40;
                    break;
                case HashTypes.SHA256:
                    length = 64;
                    break;
                case HashTypes.SHA512:
                    length = 128;
                    break;
                default:
                    throw new NotImplementedException(hashType.ToString());
            }

            return length;
        }

        /// <summary>
        /// Return hash from salt text using hash type
        /// </summary>
        /// <param name="text"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static string ComputeHash(string text, HashTypes hashType)
        {
            var unicodeEncoding = new UnicodeEncoding();
            var message = unicodeEncoding.GetBytes(text);

            var ha = GetHashAlgorithm(hashType);
            var hashValue = ha.ComputeHash(message);

            var sb = new StringBuilder(GetLength(hashType));

            foreach (byte h in hashValue)
                sb.AppendFormat("{0:X2}", h);

            return sb.ToString();
        }

        /// <summary>
        /// Return hash from stream using hash type
        /// </summary>
        /// <param name="text"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static string ComputeHash(Stream stream, HashTypes hashType)
        {
            var ha = GetHashAlgorithm(hashType);
            var hashValue = ha.ComputeHash(stream);

            var sb = new StringBuilder(GetLength(hashType));

            foreach (byte h in hashValue)
                sb.AppendFormat("{0:X2}", h);

            return sb.ToString();
        }

        /// <summary>
        /// Return hash from byte array using hash type
        /// </summary>
        /// <param name="text"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static string ComputeHash(byte[] bytes, HashTypes hashType)
        {
            var ha = GetHashAlgorithm(hashType);
            var hashValue = ha.ComputeHash(bytes);

            var sb = new StringBuilder(GetLength(hashType));

            foreach (byte h in hashValue)
                sb.AppendFormat("{0:X2}", h);

            return sb.ToString();
        }

        /// <summary>
        /// Checks hash is valid. Compares orginal and hashed using hashType
        /// </summary>
        /// <param name="original"></param>
        /// <param name="hashString"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static bool CheckHash(string original, string hashString, HashTypes hashType)
        {
            return (ComputeHash(original, hashType) == hashString);
        }

        /// <summary>
        /// Checks hash is valid. Compares orginal and hashed using hashType
        /// </summary>
        /// <param name="original"></param>
        /// <param name="hashString"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static bool CheckHash(Stream original, string hashString, HashTypes hashType)
        {
            return (ComputeHash(original, hashType) == hashString);
        }

        /// <summary>
        /// Checks hash is valid. Compares orginal and hashed using hashType
        /// </summary>
        /// <param name="original"></param>
        /// <param name="hashString"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static bool CheckHash(byte[] original, string hashString, HashTypes hashType)
        {
            return (ComputeHash(original, hashType) == hashString);
        }
    }
}
