using System;
using System.Security.Cryptography;

namespace Org.X509Crypto.Dto;
class AesCryptDto : IDisposable {
    const int KEY_SIZE    = 256;
    const int BLOCK_SIZE  = 128;

    public Aes Aes { get; set; }
    public ICryptoTransform Transform { get; set; }

    public static AesCryptDto CreateEncryptor() {
        var result = new AesCryptDto();
        result.Aes = Aes.Create();
        result.Aes.KeySize = KEY_SIZE;
        result.Aes.BlockSize = BLOCK_SIZE;
        result.Aes.Mode = CipherMode.CBC;
        result.Transform = result.Aes.CreateEncryptor();

        return result;
        //var payLoad = new AesCryptDto {
        //    Aes = new AesManaged {
        //        KeySize = KEY_SIZE,
        //        BlockSize = BLOCK_SIZE,
        //        Mode = CipherMode.CBC
        //    }
        //};
        //payLoad.Transform = payLoad.Aes.CreateEncryptor();
        //return payLoad;
    }

    public static AesCryptDto CreateDecryptor(byte[] symmetricKey, byte[] iv) {
        var result = new AesCryptDto();
        result.Aes = Aes.Create();
        result.Aes.KeySize = KEY_SIZE;
        result.Aes.BlockSize = BLOCK_SIZE;
        result.Aes.Mode = CipherMode.CBC;
        result.Aes.Key = symmetricKey;
        result.Aes.IV = iv;

        return result;
        //var payLoad = new AesCryptDto {
        //    Aes = new AesManaged {
        //        KeySize = KEY_SIZE,
        //        BlockSize = BLOCK_SIZE,
        //        Mode = CipherMode.CBC,
        //        Key = symmetricKey,
        //        IV = iv
        //    }
        //};
        //payLoad.Transform = payLoad.Aes.CreateDecryptor();
        //return payLoad;
    }

    public void Dispose() {
        Transform.Dispose();
        Aes.Dispose();
        Aes = null;
    }
}
