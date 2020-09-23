using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utils
{
    public class Security
    {
        public class PasswordHash
        {
            public static string Create(byte[] salt, string password)
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                  password: password,
                  // the salt used for derivation process 
                  salt: salt,

                  // random function for key derivation using SHA256 hash function
                  prf: KeyDerivationPrf.HMACSHA256,
                  // number of iteractions of random function      
                  iterationCount: 10000,
                  // desired length in bytes
                  numBytesRequested: 256 / 8));
                return hashed;
            }
        }
    }
}
