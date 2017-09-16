using System;
using System.IO;
using System.Security.Cryptography;
namespace RsaCryptography
{
    static class Program
    {
        static void Main()
        {
            //CSP with a new 2048 bit rsa key pair
            var csp = new RSACryptoServiceProvider(2048);
            var privKey = csp.ExportParameters(true);
            var pubKey = csp.ExportParameters(false);
            string pubKeyString;
            {
                var sw = new System.IO.StringWriter();
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                xs.Serialize(sw, pubKey);
                pubKeyString = sw.ToString();
                System.Console.ReadLine();
            }
            {
                var sr = new System.IO.StringReader(pubKeyString);
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                pubKey = (RSAParameters)xs.Deserialize(sr);
            }
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(pubKey);
            System.Console.WriteLine("Insira a mensagem a ser criptografada:\n ");
            // Original code var plainTextData = "foobar";
            var plainTextData = System.Console.ReadLine();
            var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);
            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);
            var cypherText = Convert.ToBase64String(bytesCypherText);
            bytesCypherText = Convert.FromBase64String(cypherText);

            //string path = @"C:\Users\endrunne\Desktop";
            //string[] createText = { cypherText };

            string desktop = @"C:\Users\endrunne\Desktop\cypher.pem";
            using (System.IO.StreamWriter sw = File.CreateText(desktop))
            {
                Console.WriteLine("| status: Exportando |");
                try
                {
                    sw.WriteLine(cypherText);
                }
               // catch(Exception ex)
                //{
                //    Console.WriteLine(ex.ToString());
              //  }
                finally
                {
                    Console.WriteLine("| status: |");
                }
            }
            // decrypt proccess
            using (var reader = File.OpenText(@"C:\Users\endrunne\Desktop\cypher.pem"));
            {
                var x = new PemReader(reader);
                var y = (RsaKeyParameters)x.ReadObject();
                thingee = (RSACryptoServiceProvider)RSACryptoServiceProvider.Create();
                var pa = new RSAParameters();
                pa.Modulus = y.Modulus.ToByteArray();
                pa.Exponent = y.Exponent.ToByteArray();
                thingee.ImportParameters(pa);
            }


            System.Console.WriteLine("\nMensagem criptografada:\n\n");
            System.Console.WriteLine(cypherText);
            System.Console.ReadLine();
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privKey);
            bytesPlainTextData = csp.Decrypt(bytesCypherText, false);
            plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
            System.Console.WriteLine("Você quer descriptografar essa mensagem? (Entre com Sim ou não)\n");
            string feed = System.Console.ReadLine();
            if (feed == "sim")
            {
                System.Console.WriteLine("\nA palavra criptografada é:\n\n ");
                System.Console.WriteLine(plainTextData);
                System.Console.ReadLine();
            }else{
                System.Console.WriteLine("Adeus Garoto!");
                System.Console.ReadLine();
            } 
        }
    }
}

