﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

//namespace Console48
//{
    class Program
    {
        static void Main(string[] args)
        {
            // Aqui vai requerer o username e o token pra poder usar o sistema de criptografia
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ATENÇÃO ESSE PROGRAMA FAZ PARTE DO SERVIÇO DA GUARDA COSTEIRA BRASILEIRA\nQUALQUER PESSOA QUE CARREGAR ESSA TELA DEVERÁ ESTAR CIENTE DOS RISCOS E OBRIGAÇÕES COM O MESMO");
            Console.Write("Login: ");
            var login = Console.ReadLine();
            Console.Write("Token: ");
            var token = Console.ReadLine();
            // pega o arquivo xml da area de trabalho e atribui a uma string
            System.IO.Stream t = File.Open(@"C:\Users\endrunne\Desktop\token.xml", FileMode.Open);
            StreamReader tokenReader = new StreamReader(t);
            string xmltokenToString = tokenReader.ReadLine();
            if(token == xmltokenToString)
            {
                Console.Clear();
                Console.Write("Insira 1 para criptografar ou 2 Para descriptografar: ");
                var cryOrDecry = Console.ReadLine().ToLower();
                Console.WriteLine("Insira a mensagem a ser criptografada: ");
                // Texto inserido pelo usuário;
                var textoInserido = System.Console.ReadLine();
                EncryptionAndDecry(textoInserido);
            }else{Environment.Exit(0);}
        }
        
        static void EncryptionAndDecry(string textoInserido)
        {
            var csp = new RSACryptoServiceProvider(3072);
            var chavePrivada = csp.ExportParameters(true);
            var chavePublica = csp.ExportParameters(false);
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(chavePublica);
            // Get Bytes do texto inserido;
            var bytesTextoInserido = System.Text.Encoding.Unicode.GetBytes(textoInserido);
            // Criptografando os bytes do texto original;
            var bytesCiframento = csp.Encrypt(bytesTextoInserido, false); // false faz com que o módulo não seja exportado
            // Converte os bytes cifrados pra base64;
            var textoCifrado = Convert.ToBase64String(bytesCiframento);
            bytesCiframento = Convert.FromBase64String(textoCifrado);
            // O código abaixo exporta o texto criptografado em um documento XML
            string[] createText = { textoCifrado };
            string desktop = @"C:\Users\endrunne\Desktop\cypher.xml";
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
            }
            Console.WriteLine("Você deseja decifrar a mensagem?");
            var requisicaoD = Console.ReadLine().ToLower();
            if (requisicaoD == "sim" || requisicaoD == "s")
            {
                System.IO.Stream xf = File.Open(@"C:\Users\endrunne\Desktop\cypher.xml", FileMode.Open);
                StreamReader leitor = new StreamReader(xf);
                string xmlToString = leitor.ReadLine();
                System.Console.WriteLine("\nMensagem criptografada:\n");
                System.Console.WriteLine(textoCifrado);
                // Bloco onde vai ocorrer a descriptografia
                csp = new RSACryptoServiceProvider();
                csp.ImportParameters(chavePrivada);
                bytesTextoInserido = csp.Decrypt(bytesCiframento, false);
                var textoDecifrado = System.Text.Encoding.Unicode.GetString(bytesTextoInserido);
                Console.Write("\n1 - Para arquivo interno ou 2 - Para arquivo externo\n");
                var opcao = Console.ReadLine();
                // Switch com as opções
                switch (opcao)
                {
                    case "1":
                        Console.Write("\nMensagem descriptografada: {0}", textoDecifrado);
                        Console.ReadLine();
                        break;
                    case "2":
                        csp = new RSACryptoServiceProvider();
                        csp.ImportParameters(chavePrivada);
                        var xmlBytesCifrados = System.Text.Encoding.Unicode.GetBytes(xmlToString);
                        var xmlByeee = Convert.FromBase64String(xmlToString); //deu certo
                        //bytesCiframento = Convert.FromBase64String(textoCifrado);
                        var xmlBytesDecifrados = csp.Decrypt(xmlByeee, false);
                        var xmlDecifrado = System.Text.Encoding.Unicode.GetString(xmlBytesDecifrados);
                        Console.Write("\nMensagem descriptografada: {0}", xmlDecifrado);
                        Console.ReadLine();
                        break;
                }
            }else{Environment.Exit(0);}
        }
    }
//}
