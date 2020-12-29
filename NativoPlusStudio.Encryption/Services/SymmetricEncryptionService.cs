using NativoPlusStudio.Encryption.Configuration;
using NativoPlusStudio.Encryption.Helpers;
using NativoPlusStudio.Encryption.Interfaces;
using Serilog;
using System;
using System.Security.Cryptography;
using System.Text;

namespace NativoPlusStudio.Encryption.Services
{
    public class SymmetricEncryptionService : IEncryption
    {
        private readonly EncryptionConfiguration _encryptionConfiguration;
        private readonly ILogger _logger;
        public SymmetricEncryptionService(EncryptionConfiguration encryptionConfiguration = null, ILogger logger = null)
        {
            if (logger == null)
            {
                _logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();
            }
            else
            {
                _logger = logger;
            }
            
            if (encryptionConfiguration == null)
            {
                _encryptionConfiguration = new EncryptionConfiguration();
            }
            else
            {
                _encryptionConfiguration = encryptionConfiguration;
            }
        }

        public string Encrypt(string text, Func<string, string, (string, string)> encryptionKeyGenerator = null)
        {
            _logger.Information("#Encrypt start");
            if (string.IsNullOrEmpty(text))
            {
                _logger.Information("#Encrypt the text to encrypt is empty");
                return string.Empty;
            }

            var (Key, IVBase64) = encryptionKeyGenerator == null
                ? GenerateSymmetricEncryptionKeyIV(_encryptionConfiguration.PrimaryPrivateKey, _encryptionConfiguration.SecondaryPrivateKey)
                : encryptionKeyGenerator(_encryptionConfiguration.PrimaryPrivateKey, _encryptionConfiguration.SecondaryPrivateKey);

            if (string.IsNullOrEmpty(Key) || string.IsNullOrEmpty(IVBase64))
            {
                _logger.Information("#Encrypt configure the key through DI or pass the key and IV parameters");
                return string.Empty;
            }

            Aes cipher = Key.CreateCipher();
            cipher.IV = Convert.FromBase64String(IVBase64);

            ICryptoTransform cryptTransform = cipher.CreateEncryptor();
            byte[] plaintext = Encoding.UTF8.GetBytes(text);
            byte[] cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

            return Convert.ToBase64String(cipherText);
        }

        public string Decrypt(string encryptedText, Func<string, string,(string, string)> encryptionKeyGenerator = null)
        {
            _logger.Information("#Decrypt start");

            if (string.IsNullOrEmpty(encryptedText))
            {
                _logger.Information("#Decrypt the text to decrypt is empty");
                return string.Empty;
            }
            
            var (Key, IVBase64) = encryptionKeyGenerator == null 
                ? GenerateSymmetricEncryptionKeyIV(_encryptionConfiguration.PrimaryPrivateKey, _encryptionConfiguration.SecondaryPrivateKey) 
                : encryptionKeyGenerator(_encryptionConfiguration.PrimaryPrivateKey, _encryptionConfiguration.SecondaryPrivateKey);

            if (string.IsNullOrEmpty(Key) || string.IsNullOrEmpty(IVBase64))
            {
                _logger.Information("#Decrypt configure the key through DI or pass the key and IV parameters");

                return string.Empty;
            }

            Aes cipher = Key.CreateCipher();
            cipher.IV = Convert.FromBase64String(IVBase64);

            ICryptoTransform cryptTransform = cipher.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] plainBytes = cryptTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
        
        private (string KeyBase64, string IVBase64) GenerateSymmetricEncryptionKeyIV(string primaryKey, string secondaryKey = null)
        {
            _logger.Information("#GenerateSymmetricEncryptionKeyIV start");

            //make sure the key has 32 characters
            var key = GetEncodedText(primaryKey, 32); // 32 characters
            //make sure the iv has 16 characters
            var iv = GetEncodedText(string.IsNullOrEmpty(secondaryKey) ? primaryKey : secondaryKey, 16); // 16 characters
            return (key, iv);
        }

        private string GetEncodedText(string value, int length)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            length = length <= 0 ? value.Length : length;

            //make sure the text length is always the length sent in the parameter
            value = AppendString(value, length);
            var base64 = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(value)
            );
            return base64;
        }

        private string AppendString(string value, int length)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            length = length <= 0 ? value.Length : length;

            StringBuilder builder = new StringBuilder(length);
            while (builder.Length < length)
            {
                builder.Append(value);
            }
            return builder
                .ToString()
                .Substring(0, length);
        }
    }
}
