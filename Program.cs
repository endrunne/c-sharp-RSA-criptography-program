using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Console48
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("########################## Iniciando o Programa ######################");
            //CSP with a new 2048 bit rsa key pair
            var csp = new RSACryptoServiceProvider(3072); // Tamanho de bits da chave, isso é o suficiente para 128 char
            var privKey = csp.ExportParameters(true); // Chave privada 
            var pubKey = csp.ExportParameters(false); // Chave publica
            System.Console.WriteLine("Insira a mensagem a ser criptografada: ");
            // Texto inserido pelo usuário;
            var textoInserido = System.Console.ReadLine();
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(pubKey);
            // Get Bytes do texto inserido;
            var bytesTextoInserido = System.Text.Encoding.Unicode.GetBytes(textoInserido);
            // Criptografando os bytes do texto original;
            var bytesCiframento = csp.Encrypt(bytesTextoInserido, false); // false faz com que o módulo não seja exportado
            // Converte os bytes cifrados pra base64;
            var textoCifrado = Convert.ToBase64String(bytesCiframento);
            bytesCiframento = Convert.FromBase64String(textoCifrado);
            // O código abaixo exporta o texto criptografado em um documento XML
            /*
            string[] createText = { textoCifrado };
            string desktop = @"E:\cypher.xml";
            using (System.IO.StreamWriter sw = File.CreateText(desktop))
            {
                Console.WriteLine("| status: Exportando |");
                try
                {
                    sw.WriteLine(textoCifrado);
                }
                finally
                {
                    Console.WriteLine("| status: |");
                }
            }*/
            System.Console.WriteLine("Mensagem criptografada:\n\n");
            System.Console.WriteLine(textoCifrado);
            System.Console.ReadLine();
            // Bloco onde vai ocorrer a descriptografia
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privKey);
            bytesTextoInserido = csp.Decrypt(bytesCiframento, false);
            var textoDecifrado = System.Text.Encoding.Unicode.GetString(bytesTextoInserido);
            Console.WriteLine("Você deseja decifrar a Mensagem?");
            var requisicaoD = Console.ReadLine().ToLower();
            if (requisicaoD == "s" || requisicaoD == "sim")
            {
                Console.WriteLine("\nMensagem descriptografada: {0}", textoDecifrado);
                Console.ReadLine();
            }
            else { Environment.Exit(0); }
        }
    }
}

