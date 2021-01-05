using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativoPlusStudio.Encryption.Interfaces;

namespace NativoPlusStudio.EncryptionTests
{
    [TestClass]
    public class EncryptionTest : BaseTestConfiguration
    {
        private readonly IEncryption _symmetricEncryption;

        public EncryptionTest()
        {
            _symmetricEncryption = serviceProvider.GetRequiredService<IEncryption>();
        }
        
        [TestMethod]
        public void TestSymmetricEncryptMethod()
        {
            var textToEncrypt = "Hello World!";

            var response = SymmetricEncrypt(textToEncrypt);

            Assert.IsTrue(response != null);                      
        }
        
        public string SymmetricEncrypt(string text)
        {

            var response = _symmetricEncryption.Encrypt(text);

            return response;
        }

        [TestMethod]
        public void TestSymmetricEncryptAndDecrypt()
        {
            var textToEncrypt = "Hello World!";
            var encryptedText = SymmetricEncrypt(textToEncrypt);
            var decryptedMessage = _symmetricEncryption.Decrypt(encryptedText);

            Assert.IsTrue(!string.IsNullOrEmpty(decryptedMessage));
            Assert.IsTrue(!string.IsNullOrEmpty(encryptedText));
            Assert.IsTrue(decryptedMessage == textToEncrypt);
        }
        
        [TestMethod]
        public void TestDecryptingUnencryptedText()
        {
            var textToEncrypt = "Hello World!";
            var decryptedMessage = _symmetricEncryption.Decrypt(textToEncrypt);

            Assert.IsTrue(!string.IsNullOrEmpty(decryptedMessage));
            Assert.IsTrue(decryptedMessage == textToEncrypt);
        }
    }
}
