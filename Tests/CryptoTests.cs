using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.X509Crypto;
using Org.X509Crypto.Dto;
using Org.X509Crypto.Services;

namespace Tests {
    [TestClass]
    public class CryptoTests {
        CertificateDto cert;
        String testText = "Hello world";

        [TestInitialize]
        public void Initialize() {
            cert = CertService.CreateX509CryptCertificate("Crypto Tests", X509Context.UserFull);
        }

        [TestMethod]
        public void TestEncryptString() {
            var service = new EncryptionService(cert);
            EncryptedSecretDto dto = service.EncryptText(testText);
            string recoveredText = service.DecryptText(dto);
            Assert.AreEqual(testText, recoveredText);
        }
    }
}
