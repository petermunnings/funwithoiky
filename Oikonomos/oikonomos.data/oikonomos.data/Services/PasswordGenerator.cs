using System;
using System.Security.Cryptography;


namespace oikonomos.data.Services
{
    public enum RandomPasswordOptions
    {
        Alpha = 1,
        Numeric = 2,
        AlphaNumeric = Alpha + Numeric,
        AlphaNumericSpecial = 4
    }

    public class RandomPasswordGenerator
    {
        // Define default password length.   
        private static int DEFAULT_PASSWORD_LENGTH = 8;

        //No characters that are confusing: i, I, l, L, o, O, 0, 1, u, v   

        private static string PASSWORD_CHARS_Alpha =
                                   "abcdefghjkmnpqrstwxyzABCDEFGHJKMNPQRSTWXYZ";
        private static string PASSWORD_CHARS_NUMERIC = "23456789";
        private static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

        #region Overloads

        /// <SUMMARY>   
        /// Generates a random password with the default length.   
        /// </SUMMARY>   
        /// <RETURNS>Randomly generated password.</RETURNS>   
        public static string Generate()
        {
            return Generate(DEFAULT_PASSWORD_LENGTH,
                            RandomPasswordOptions.AlphaNumericSpecial);
        }

        /// <SUMMARY>   
        /// Generates a random password with the default length.   
        /// </SUMMARY>   
        /// <RETURNS>Randomly generated password.</RETURNS>   
        public static string Generate(RandomPasswordOptions option)
        {
            return Generate(DEFAULT_PASSWORD_LENGTH, option);
        }

        /// <SUMMARY>   
        /// Generates a random password with the default length.   
        /// </SUMMARY>   
        /// <RETURNS>Randomly generated password.</RETURNS>   
        public static string Generate(int passwordLength)
        {
            return Generate(DEFAULT_PASSWORD_LENGTH,
                            RandomPasswordOptions.AlphaNumericSpecial);
        }

        /// <SUMMARY>   
        /// Generates a random password.   
        /// </SUMMARY>   
        /// <RETURNS>Randomly generated password.</RETURNS>   
        public static string Generate(int passwordLength,
                                      RandomPasswordOptions option)
        {
            return GeneratePassword(passwordLength, option);
        }

        #endregion


        /// <SUMMARY>   
        /// Generates the password.   
        /// </SUMMARY>   
        /// <RETURNS></RETURNS>   
        private static string GeneratePassword(int passwordLength,
                                               RandomPasswordOptions option)
        {
            if (passwordLength < 0) return null;

            var passwordChars = GetCharacters(option);

            if (string.IsNullOrEmpty(passwordChars)) return null;

            var password = new char[passwordLength];

            var random = GetRandom();

            for (int i = 0; i < passwordLength; i++)
            {
                var index = random.Next(passwordChars.Length);
                var passwordChar = passwordChars[index];

                password[i] = passwordChar;
            }

            return new string(password);
        }



        /// <SUMMARY>   
        /// Gets the characters selected by the option   
        /// </SUMMARY>   
        /// <RETURNS></RETURNS>   
        private static string GetCharacters(RandomPasswordOptions option)
        {
            switch (option)
            {
                case RandomPasswordOptions.Alpha:
                    return PASSWORD_CHARS_Alpha;
                case RandomPasswordOptions.Numeric:
                    return PASSWORD_CHARS_NUMERIC;
                case RandomPasswordOptions.AlphaNumeric:
                    return PASSWORD_CHARS_Alpha + PASSWORD_CHARS_NUMERIC;
                case RandomPasswordOptions.AlphaNumericSpecial:
                    return PASSWORD_CHARS_Alpha + PASSWORD_CHARS_NUMERIC +
                                 PASSWORD_CHARS_SPECIAL;
                default:
                    break;
            }
            return string.Empty;
        }

        /// <SUMMARY>   
        /// Gets a random object with a real random seed   
        /// </SUMMARY>   
        /// <RETURNS></RETURNS>   
        private static Random GetRandom()
        {
            // Use a 4-byte array to fill it with random bytes and convert it then   
            // to an integer value.   
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.   
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.   
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.   
            return new Random(seed);
        }


    }
}  

