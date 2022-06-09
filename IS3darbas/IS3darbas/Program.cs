using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace IS3darbas
{
    public class encrytDecrypt
    {
        private static RSACryptoServiceProvider rsacsp = new RSACryptoServiceProvider(2048);
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        public encrytDecrypt()
        {
            _privateKey = rsacsp.ExportParameters(true);
            _publicKey = rsacsp.ExportParameters(false);
        }

        public string GetPublicKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw,_publicKey);
            return sw.ToString();
        }

        public string Encrypt(string plainText)
        {
            rsacsp = new RSACryptoServiceProvider();
            rsacsp.ImportParameters(_publicKey);
            var data = Encoding.Unicode.GetBytes(plainText);

            var cypher = rsacsp.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }

        public string Decrypt(string cypherText)
        {
            var dataBytes = Convert.FromBase64String(cypherText);
            rsacsp.ImportParameters(_privateKey);
            var plainText = rsacsp.Decrypt(dataBytes, false);
            return Encoding.Unicode.GetString(plainText);
        }

    }
    

    class Program
    {
        static void Main(string[] args)
        {
            encrytDecrypt rsa = new encrytDecrypt();
            string cypher = string.Empty;

            Console.WriteLine($"Viesas raktas: {rsa.GetPublicKey()} \n");

            Console.WriteLine("Tekstas:");
            var text = Console.ReadLine();

            if (!string.IsNullOrEmpty(text))
            {
                cypher = rsa.Encrypt(text);
                Console.WriteLine($"Sifruotas tekstas: \n {cypher} \n");
            }

            Console.WriteLine("Spauskite enter jeigu norite desifruoti");
            Console.ReadLine();
            var plainText = rsa.Decrypt(cypher);



            Console.WriteLine($"Desifruotas tekstas: {plainText}");
            Console.ReadLine();



        }
    }
}
