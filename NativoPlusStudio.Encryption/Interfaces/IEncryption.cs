using System;

namespace NativoPlusStudio.Encryption.Interfaces
{
    public interface IEncryption
    {
        /// <summary>
        /// Decrypts a string using AES Symmetric algorithm
        /// </summary>
        /// <param name="encryptedText">Text to be decrypted</param>
        /// <param name="encryptionKeyGenerator">this parameter is a function you can pass to this method to use your own logic of generating the encrypted key that will be used to return the encrypted text</param>
        /// <returns></returns>
        string Decrypt(string encryptedText, Func<string, string, (string, string)> encryptionKeyGenerator = null);
        /// <summary>
        /// Encrypts a string using AES Symmetric algorithm
        /// </summary>
        /// <param name="text">Text to be encrypted</param>
        /// <param name="encryptionKeyGenerator">this parameter is a function you can pass to this method to use your own logic of generating the encrypted key that will be used to return the decrypted text. if you leave this null it will use the internal logic.</param>
        /// <returns></returns>
        string Encrypt(string text, Func<string, string, (string, string)> encryptionKeyGenerator = null);
    }
}