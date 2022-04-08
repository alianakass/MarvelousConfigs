using System.Security.Cryptography;

namespace MarvelousConfigs.BLL.Crypter
{
    public static class Crypter
    {
        private static Aes _handler { get; }

        static Crypter()
        {
            _handler = Aes.Create();
            _handler.Key = Convert.FromBase64String("lB2BxrJdI4UUjK3KEZyQ0obuSgavB1SYJuAFq9oVw0Y=");
            _handler.IV = Convert.FromBase64String("6lra6ceX26Fazwj1R4PCOg==");
        }

        public static string Encrypt(string source)
        {
            using (var mem = new MemoryStream())
            using (var stream = new CryptoStream(mem, _handler.CreateEncryptor(_handler.Key, _handler.IV),
                CryptoStreamMode.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(source);
                }
                return Convert.ToBase64String(mem.ToArray());
            }

        }

        public static string Decrypt(string source)
        {
            var data = Convert.FromBase64String(source);
            using (var mem = new MemoryStream(data))
            using (var crypto = new CryptoStream(mem, _handler.CreateDecryptor(_handler.Key, _handler.IV),
                CryptoStreamMode.Read))
            using (var reader = new StreamReader(crypto))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
