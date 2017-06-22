using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace lw.Utils
{

	/// <summary>
	/// Class helper to be used with .Net's built in hash encryption methods
	/// </summary>
	public class Hash
	{
		/// <summary>
		/// Enum declaring possible hash types
		/// </summary>
		public enum HashType : int
		{
			MD5,
			SHA1,
			SHA256,
			SHA512
		}

		/// <summary>
		/// Returns the hash encryption of the entry text
		/// </summary>
		/// <param name="text">Text to be hashed</param>
		/// <param name="hashType">The <see cref="HashType"/>Hash type</param>
		/// <returns>Hashed string</returns>
		public static string GetHash(string text, HashType hashType)
		{
			string hashString;
			switch (hashType)
			{
				case HashType.MD5:
					hashString = GetMD5(text);
					break;
				case HashType.SHA1:
					hashString = GetSHA1(text);
					break;
				case HashType.SHA256:
					hashString = GetSHA256(text);
					break;
				case HashType.SHA512:
					hashString = GetSHA512(text);
					break;
				default:
					hashString = "Invalid Hash Type";
					break;
			}
			return hashString;
		}

		/// <summary>
		/// Checks if the hash is correct
		/// </summary>
		/// <param name="original">Original string</param>
		/// <param name="hashString">Hash string</param>
		/// <param name="hashType"><see cref="HashType"/>Hash type</param>
		/// <returns>True if good hash, false if not</returns>
		public static bool CheckHash(string original, string hashString, HashType hashType)
		{
			string originalHash = GetHash(original, hashType);
			return (originalHash == hashString);
		}

		/// <summary>
		/// Get the MD5 hash string of the entry text
		/// </summary>
		/// <param name="text">Original string</param>
		/// <returns>Hashed string</returns>
		private static string GetMD5(string text)
		{
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] hashValue;
			byte[] message = UE.GetBytes(text);

			MD5 hashString = new MD5CryptoServiceProvider();
			string hex = "";

			hashValue = hashString.ComputeHash(message);
			foreach (byte x in hashValue)
			{
				hex += String.Format("{0:x2}", x);
			}
			return hex;
		}

		/// <summary>
		/// Get the SHA1 hash string of the entry text
		/// </summary>
		/// <param name="text">Original string</param>
		/// <returns>Hashed string</returns>
		private static string GetSHA1(string text)
		{
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] hashValue;
			byte[] message = UE.GetBytes(text);

			SHA1Managed hashString = new SHA1Managed();
			string hex = "";

			hashValue = hashString.ComputeHash(message);
			foreach (byte x in hashValue)
			{
				hex += String.Format("{0:x2}", x);
			}
			return hex;
		}
		/// <summary>
		/// Get the SHA256 hash string of the entry text
		/// </summary>
		/// <param name="text">Original string</param>
		/// <returns>Hashed string</returns>
		private static string GetSHA256(string text)
		{
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] hashValue;
			byte[] message = UE.GetBytes(text);

			SHA256Managed hashString = new SHA256Managed();
			string hex = "";

			hashValue = hashString.ComputeHash(message);
			foreach (byte x in hashValue)
			{
				hex += String.Format("{0:x2}", x);
			}
			return hex;
		}
		/// <summary>
		/// Get the SHA512 hash string of the entry text
		/// </summary>
		/// <param name="text">Original string</param>
		/// <returns>Hashed string</returns>
		private static string GetSHA512(string text)
		{
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] hashValue;
			byte[] message = UE.GetBytes(text);

			SHA512Managed hashString = new SHA512Managed();
			string hex = "";

			hashValue = hashString.ComputeHash(message);
			foreach (byte x in hashValue)
			{
				hex += String.Format("{0:x2}", x);
			}
			return hex;
		}
	}
}
