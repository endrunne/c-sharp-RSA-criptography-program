using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
//namespace TokenG
//{
	class TokenGenerator
	{
		static void Main(string[] args)
		{
			// Gera a senha (token) com base no Primeiro nome, ultimo nome e ID da guarda costeira
			Console.Write("Primeiro nome: ");
			var nome = Console.ReadKey();
			Console.Write("Ultimo nome: ");
			var ultimoNome =Console.ReadKey();
			Console.Write("ID: ");
			var id = Console.ReadLine();
			var privateKey = GetPrivateKey();
			var inputToToken = ""+nome+""+ultimoNome+""+id;
			var token = new Token(inputToToken, DateTime.Now.AddMinutes(1)); //Pega a hora do sistema e usa como parametro
			var tokenString = token.GetTokenString(privateKey);
			//exporta o token em xml pra area de trabalho
			string[] createXML = { tokenString };
			using (System.IO.StreamWriter t = File.CreateText(@"C:\Users\endrunne\Desktop\token.xml"))
			{
				Console.WriteLine("| status: Exportando |");
				try
				{
					t.WriteLine(tokenString);
				}
				finally
				{
					Console.WriteLine("| status: |");
				}
			}
			Console.WriteLine(tokenString);
			Console.ReadLine();
		}
		private static string GetPrivateKey()
		{
			using (var rsa = new RSACryptoServiceProvider())
			{
				return rsa.ToXmlString(true);
			}
		}
	class Token
	{
		public readonly string Value;
		public readonly DateTime Expires;
		public readonly byte[] Data;
		public byte[] Signature { private set; get; }
		public Token(string value, DateTime expires)
		{
			Value = value;
			Expires = expires;
			using (var ms = new MemoryStream())
			using (var writer = new BinaryWriter(ms))
			{
				writer.Write(Expires.Ticks);
				writer.Write(Value);
				Data = ms.ToArray();
			}
		}

		private void Sign(string privateKey)
		{
			using (var rsa = new RSACryptoServiceProvider())
			using (var sha1 = new SHA1CryptoServiceProvider())
			{
				rsa.FromXmlString(privateKey);
				Signature = rsa.SignData(Data, sha1);
			}
		}

		public string GetTokenString(string privateKey)
		{
			if (Signature == null)
			{
				Sign(privateKey);
			}
			return Convert.ToBase64String(Data.Concat(Signature).ToArray());
		}

		public static Token FromTokenString(string tokenString, string key)
		{
			var buffer = Convert.FromBase64String(tokenString);
			var data = buffer.Take(buffer.Length - 128).ToArray();
			var sig = buffer.Skip(data.Length).Take(128).ToArray();
			using (var rsa = new RSACryptoServiceProvider())
			using (var sha1 = new SHA1CryptoServiceProvider())
			{
				rsa.FromXmlString(key);
				if (rsa.VerifyData(data, sha1, sig))
				{
					using (var ms = new MemoryStream(data))
					using (var reader = new BinaryReader(ms))
					{
						var ticks = reader.ReadInt64();
						var value = reader.ReadString();
						var expires = new DateTime(ticks);
						if (expires > DateTime.Now)
						{
							return new Token(value, expires);
						}
					}
				}
			}
			return null;
		}
	}
}