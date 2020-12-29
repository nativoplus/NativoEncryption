using System;

namespace NativoPlusStudio.Encryption.Interfaces
{
    public interface IEncryption
    {
        /// <summary>
        /// Decrypts a string using AES Symmetric algorithm
        /// </summary>
        /// <param name="encryptedText">Text to be decrypted</param>
        /// <param name="ivBase64">This is the base 64 initiated vector that was used to encrypt the text</param>
        /// <param name="keyBase64">This is the base 64 key that was used to encrypt the text</param>
        /// <returns></returns>
        string Decrypt(string encryptedText, Func<string, string, (string, string)> encryptionKeyGenerator = null);
        /// <summary>
        /// Encrypts a string using AES Symmetric algorithm
        /// </summary>
        /// <param name="text">Text to be encrypted</param>
        /// <param name="ivBase64">This is the base 64 initiated vector to be used to encrypt the text</param>
        /// <param name="keyBase64">This is the base 64 key to be used to encrypt the text</param>
        /// <returns></returns>
        string Encrypt(string text, Func<string, string, (string, string)> encryptionKeyGenerator = null);
    }
}