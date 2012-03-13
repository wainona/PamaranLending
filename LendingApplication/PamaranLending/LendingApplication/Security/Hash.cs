using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace LendingApplication
{
    public class Hash
    {
        private HashAlgorithm hash;

        private Hash(Type type)
        {
            hash = (HashAlgorithm)Activator.CreateInstance(type);
        }

        public static Hash Create<T>()
            where T : HashAlgorithm
        {
            return new Hash(typeof(T));
        }

        public static Hash Create(Type type)
        {
            if (type.IsSubclassOf(typeof(HashAlgorithm)))
                return new Hash(type);
            else
                throw new ArgumentException("The \"type\" parameter should be of type HashAlgorithm");
        }

        public byte[] Compute(byte[] plainBytes)
        {
            return hash.ComputeHash(plainBytes);
        }

        public string Compute(string plainText)
        {
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);
            return Convert.ToBase64String(hash.ComputeHash(plainBytes));
        }

        public bool Verify(byte[] hash, byte[] plainBytes)
        {
            byte[] expectedHash = Compute(plainBytes);
            return Convert.ToBase64String(expectedHash) == Convert.ToBase64String(hash);
        }

        public bool Verify(string hash, string plainText)
        {
            string expectedHash = Compute(plainText);
            return expectedHash == hash;
        }

        public override string ToString()
        {
            return hash.GetType().BaseType.Name;
        }
    }

    public static class SimplifiedHash
    {
        public enum HashType
        {
            md5,
            sha1,
            sha256,
            sha348,
            sha512
        }

        /// <summary>
        /// Generates a hash algorithm relative to the type being used.
        /// </summary>
        /// <param name="plainTxt">The value to be hash. This function does not 
        ///     check whether the plain txt(txtToEncrypt) parameters is null.
        /// </param>
        /// <param name="type">The type of hash algorithm to use.</param>
        /// <returns></returns>
        public static string ComputeHash(string plainText, HashType type)
        {
            HashAlgorithm hash = null;

            switch (type)
            {
                case HashType.sha1:
                    hash = new SHA1Managed();
                    break;
                case HashType.sha256:
                    hash = new SHA256Managed();
                    break;
                case HashType.sha348:
                    hash = new SHA384Managed();
                    break;
                case HashType.sha512:
                    hash = new SHA512Managed();
                    break;
                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);
            return Convert.ToBase64String(hash.ComputeHash(plainBytes));
        }

        /// <summary>
        /// Checks the validity of hashvalue and plain txt.
        /// </summary>
        /// <param name="plainTxt">The original txt.</param>
        /// <param name="hashValue">The hashvalue computed from the original text</param>
        /// <returns>True if the plain txt is not modified</returns>
        public static bool VerifyHash(string plainTxt, byte[] hashValue, HashType type)
        {
            // compare the plain txt that arrived with the computed hash value.
            return ComputeHash(plainTxt, type) == Convert.ToBase64String(hashValue);
        }

        /// <summary>
        /// Checks the validity of hashvalue and plain txt.
        /// </summary>
        /// <param name="plainTxt">The original txt.</param>
        /// <param name="hashValue">The hashvalue computed from the original text</param>
        /// <returns>True if the plain txt is not modified</returns>
        public static bool VerifyHash(string plainTxt, string hashValue, HashType type)
        {
            // compare the plain txt that arrived with the computed hash value.
            string expectedHashString = ComputeHash(plainTxt, type);
            return expectedHashString == hashValue;
        }
    }
}