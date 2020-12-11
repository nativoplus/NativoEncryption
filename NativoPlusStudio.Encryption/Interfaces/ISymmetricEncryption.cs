namespace NativoPlusStudio.Encryption.Interfaces
{
    public interface ISymmetricEncryption
    {
        /// <summary>
        /// Decrypts a string using AES Symmetric algorithm
        /// </summary>
        /// <param name="encryptedText">Text to be decrypted</param>
        /// <param name="ivBase64">This is the base 64 initiated vector that was used to encrypt the text</param>
        /// <param name="keyBase64">This is the base 64 key that was used to encrypt the text</param>
        /// <returns></returns>
        string Decrypt(string encryptedText, string ivBase64 = null, string keyBase64 = null);
        /// <summary>
        /// Encrypts a string using AES Symmetric algorithm
        /// </summary>
        /// <param name="text">Text to be encrypted</param>
        /// <param name="ivBase64">This is the base 64 initiated vector to be used to encrypt the text</param>
        /// <param name="keyBase64">This is the base 64 key to be used to encrypt the text</param>
        /// <returns></returns>
        string Encrypt(string text, string ivBase64 = null, string keyBase64 = null);
        /// <summary>
        /// This method accepts a string, transforms it to two strings, one of 32 length and another of 16 length and converts them to base 64 strings and returns them.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (string KeyBase64, string IVBase64) GenerateSymmetricEncryptionKeyIV(string value);
    }
}