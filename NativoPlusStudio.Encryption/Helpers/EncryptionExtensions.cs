using System;
using System.Security.Cryptography;

namespace NativoPlusStudio.Encryption.Helpers
{
    public static class EncryptionExtensions
    {
        public static Aes CreateCipher(this string keyBase64)
        {
            // Default values: Keysize 256, Padding PKC27
            Aes cipher = Aes.Create();
            cipher.Mode = CipherMode.CBC;  // Ensure the integrity of the ciphertext if using CBC

            cipher.Padding = PaddingMode.ISO10126;
            cipher.Key = Convert.FromBase64String(keyBase64);

            return cipher;
        }
    }
}
