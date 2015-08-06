using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lynex.BillMaster.Extension
{
    public static class PasswordHelper
    {
        private const int SaltValueSize = 4;

        public static string GenerateSalt()
        {
            //UnicodeEncoding utf16 = new UnicodeEncoding();
            //Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            //// Create an array of random values.

            //byte[] saltValue = new byte[SaltValueSize];

            //random.NextBytes(saltValue);

            //// Convert the salt value to a string. Note that the resulting string
            //// will still be an array of binary values and not a printable string. 
            //// Also it does not convert each byte to a double byte.

            //string saltValueString = utf16.GetString(saltValue);

            //// Return the salt value as a string.

            //return saltValueString;


            var salt = string.Empty;
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Buffer storage.
                byte[] data = new byte[1];

                // Ten iterations.
                for (int i = 0; i < SaltValueSize; i++)
                {
                    // Fill buffer.
                    rng.GetBytes(data);

                    // Convert to int 32.
                    salt+= BitConverter.ToString(data, 0);
//salt += .ToString(data, 0);
                }
            }
            return salt;
        }

        public static string GetHash(string clearData, string saltValue, HashAlgorithm hash)
        {
            var encoding = new UnicodeEncoding();

            if (clearData != null && hash != null)
            {
                // If the salt string is null or the length is invalid then
                // create a new valid salt value.

                if (saltValue == null)
                {
                    // Generate a salt string.
                    saltValue = GenerateSalt();
                }

                // Convert the salt string and the password string to a single
                // array of bytes. Note that the password string is Unicode and
                // therefore may or may not have a zero in every other byte.

                byte[] binarySaltValue = new byte[SaltValueSize];

                binarySaltValue[0] = byte.Parse(saltValue.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[1] = byte.Parse(saltValue.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[2] = byte.Parse(saltValue.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[3] = byte.Parse(saltValue.Substring(6, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);

                byte[] valueToHash = new byte[SaltValueSize + encoding.GetByteCount(clearData)];
                byte[] binaryPassword = encoding.GetBytes(clearData);

                // Copy the salt value and the password to the hash buffer.

                binarySaltValue.CopyTo(valueToHash, 0);
                binaryPassword.CopyTo(valueToHash, SaltValueSize);

                byte[] hashValue = hash.ComputeHash(valueToHash);

                // The hashed password is the salt plus the hash value (as a string).

                var hashedPassword = string.Empty;

                foreach (byte hexdigit in hashValue)
                {
                    hashedPassword += hexdigit.ToString("X2", CultureInfo.InvariantCulture.NumberFormat);
                }

                // Return the hashed password as a string.

                return hashedPassword;
            }

            return null;
        }
    }
}
