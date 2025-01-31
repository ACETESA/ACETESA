﻿using System;
using System.Text;
using System.Security.Cryptography;

namespace Acetesa.TomaPedidos.Transversal.Extensions
{
    public static class SimpleHash
    {
        public static string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
        {
            //if salt is not supplied then generate it on d fly
            if (saltBytes == null)
            {
                //Define min and max salt sizes
                const int minSaltSize = 4;
                const int maxSaltSize = 8;

                //Generate random number for the size of the salt
                var random = new Random();
                var saltSize = random.Next(minSaltSize, maxSaltSize);

                saltBytes = new byte[saltSize];

                //initialize a random number generator
                var rng = new RNGCryptoServiceProvider();

                //Fill the salt with crytography strong byte values
                rng.GetNonZeroBytes(saltBytes);
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            //allocate array, which will hold the plain text and salt
            var plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            //copy plain text bytes into resulting array
            for (var i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            //append salt bytes to the resulting array
            for (var i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            //Because we support multiple hashing algorithm, we must define
            //hash object as a common (abstract) base class. We will specify the
            //actual hashing algorithm class later during object creation
            HashAlgorithm hash;

            //Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            //Initialize appropriate hashing algorithm class
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            //Compute hash value of our plain text with appended salt
            var hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            //create array which will hold hash and original salt byte
            var hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            //copy hash bytes into resulting array
            for (var i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            //append salt bytes to the result
            for (var i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            //convert result into a based64 encoded string
            var hashValue = Convert.ToBase64String(hashWithSaltBytes);

            //return the result
            return hashValue;
        }

        public static bool VerifyHash(string plainText, string hashAlgorithm, string hashValue)
        {
            try
            {
                //convert base64-encoded hash value into a byte array
                var hashWithSaltBytes = Convert.FromBase64String(hashValue);

                //we must know size of hash (without salt);
                int hashSizeInBits;

                //Make sure that hashing algorithm name is specified
                if (hashAlgorithm == null)
                    hashAlgorithm = "";

                //size of hash is based on the specified algorithm
                switch (hashAlgorithm.ToUpper())
                {
                    case "SHA1":
                        hashSizeInBits = 160;
                        break;

                    case "SHA256":
                        hashSizeInBits = 256;
                        break;

                    case "SHA384":
                        hashSizeInBits = 384;
                        break;

                    case "SHA512":
                        hashSizeInBits = 512;
                        break;

                    default:
                        hashSizeInBits = 128;
                        break;
                }

                //Convert size of hash from bits to bytes.
                var hashSizeInBytes = hashSizeInBits / 8;

                //Make sure that the specified hash value is long enough
                if (hashWithSaltBytes.Length < hashSizeInBytes)
                    return false;

                //Allocate array to hold original salt bytes retreived
                var saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

                for (var i = 0; i < saltBytes.Length; i++)
                    saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

                //compute a new hash string
                var expectedHashString = ComputeHash(plainText, hashAlgorithm, saltBytes);

                //if the computed hash matches the specified hash
                //then plaintext value must be correct

                return (hashValue == expectedHashString);
            }
            catch
            {
                return false;
            }
        }
    }

}
