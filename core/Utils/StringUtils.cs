using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web;

namespace lw.Utils
{
	/// <summary>
	/// A helper class to be used when dealing with strings.
	/// </summary>
	public static class StringUtils
	{
		/// <summary>
		/// Converts the string into a url friendly, by replacing illegal characters with the parameter replacement
		/// </summary>
		/// <param name="str">Input string</param>
		/// <param name="replacement">Replacement String</param>
		/// <returns>URL Friendly string</returns>
		public static string ToURL(string str, string replacement)
		{
			if (replacement == null)
				replacement = "-";
			str = str.ToLower();
			str = StripOutHtmlTags(str);

			str = str.Replace("&reg;", "");

			var r = new Regex("\\W");
			var r1 = new Regex("\\s+");

			string before = "‡¿‚¬‰ƒ·¡È…Ë»Í ÎÀÏÃÓŒÔœÚ“Ù‘ˆ÷˘Ÿ˚€¸‹Á«íÒ/Û:";
			string after = "aAaAaAaAeEeEeEeEiIiIiIoOoOoOuUuUuUcC-n-o ";

			string cleaned = str;

			for (int i = 0; i < before.Length; i++)
			{
				cleaned = Regex.Replace(cleaned, before[i].ToString(), after[i].ToString());
			}


			cleaned = r1.Replace(r.Replace(cleaned, " ").Trim(), " ");

			cleaned = cleaned.Replace(" ", replacement);
			string ret = cleaned;


			if (replacement != "")
			{
				var r3 = new Regex(replacement + "+");

				ret = r3.Replace(cleaned, replacement);
			}
			return ret.Length > 55 ? ret.Substring(0, 55) : ret;
		}

		/// <summary>
		///     Convverts the string into a url friendly one, by replacing illegal characters with the parameter replacement
		/// </summary>
		/// <param name="str">Input string</param>
		/// <returns>URL Friendly string</returns>
		public static string ToURL(string str)
		{
			return ToURL(str, "-");
		}

		/// <summary>
		///     Convverts the string into a url friendly one, by replacing illegal characters with the parameter replacement
		/// </summary>
		/// <param name="obj">Input object</param>
		/// <returns>URL Friendly string</returns>
		public static string ToURL(object obj)
		{
			return ToURL(obj.ToString(), "-");
		}

		/// <summary>
		///     Convverts the string into a url friendly one, by replacing illegal characters with the parameter replacement
		/// </summary>
		/// <param name="obj">An object that is transformed to a string</param>
		/// <param name="replacement">Replacement String</param>
		/// <returns>URL Friendly string</returns>
		public static string ToURL(object obj, string replacement)
		{
			return ToURL(obj.ToString(), replacement);
		}

		/// <summary>
		///     Convverts the string into a url friendly one, by replacing illegal characters with the parameter replacement
		/// </summary>
		/// <param name="url">Input string</param>
		/// <returns>URL Friendly string</returns>
		public static string URLEncode(string url)
		{
			return ToURL(url, "-");
		}

		/// <summary>
		///     Decaprecated use JSonSerialize <seealso cref="JSonSerialize" />
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string JsonEncode(string s)
		{
			if (s == null || s.Length == 0)
			{
				return "\"\"";
			}
			char c;
			int i;
			int len = s.Length;
			var sb = new StringBuilder(len + 4);
			string t;

			sb.Append('"');
			for (i = 0; i < len; i += 1)
			{
				c = s[i];
				if ((c == '\\') || (c == '"'))
				{
					sb.Append('\\');
					sb.Append(c);
				}
				else if (c == '\b')
					sb.Append("\\b");
				else if (c == '\t')
					sb.Append("\\t");
				else if (c == '\n')
					sb.Append("\\n");
				else if (c == '\f')
					sb.Append("\\f");
				else if (c == '\r')
					sb.Append("\\r");
				else
				{
					if (c < ' ')
					{
						//t = "000" + Integer.toHexString(c); 
						var tmp = new string(c, 1);
						t = "000" + int.Parse(tmp, NumberStyles.HexNumber);
						sb.Append("\\u" + t.Substring(t.Length - 4));
					}
					else
					{
						sb.Append(c);
					}
				}
			}
			sb.Append('"');
			return sb.ToString();
		}

		public static string AjaxEncode(string s)
		{
			return JsonEncode(s);
		}

		/// <summary>
		///     Converts a string to money in $
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string ToMoneyString(object obj)
		{
			string str = obj.ToString();
			try
			{
				decimal mon = decimal.Parse(str);
				return string.Format("$ {0: ###,0.00}", mon);
			}
			catch
			{
				return str;
			}
		}

		/// <summary>
		/// Removed HTML tags from the string
		/// </summary>
		/// <param name="s">The entry string</param>
		/// <returns>Output, stripped out from any HTML tags</returns>
		public static string StripOutHtmlTags(string s)
		{
			if (s == null)
				return "";
			var r = new Regex(RegularExpressions.HTMLTags);
			return r.Replace(s, "");
		}

		/// <summary>
		/// Encodes a string into SQL friendly string, preventing it from SQL injection
		/// </summary>
		/// <param name="s">The entry string</param>
		/// <returns>The output ready to use with SQL string</returns>
		public static string SQLEncode(string s)
		{
			if (s == null)
				return "";
			return s.Replace("'", "''");
		}

		/// <summary>
		/// Trankates a string by removed the last set of characters and limiting it to length.
		/// If the string length is less than <paramref name="length"/> it will be returned as is
		/// If the string length is more it will be trankated.
		/// The last word will be removed and replaced with <paramref name="end"/>
		/// </summary>
		/// <param name="s">Entry string</param>
		/// <param name="length">The length to be transkated</param>
		/// <param name="end">The closing sentence (can be: ...)</param>
		/// <returns>Returns the trankated string</returns>
		public static string Trankate(string s, int length, string end)
		{
			s = StripOutHtmlTags(s);
			if (s.Length > length)
			{
				s = s.Substring(0, length - 3);
				var regex = new Regex("\\s([a-z_0-9.&;])*$", RegexOptions.IgnoreCase);
				return regex.Replace(s, end);
			}
			return s;
		}

		/// <summary>
		/// Trankates a string by removed the last set of characters and limiting it to length.
		/// If the string length is less than <paramref name="length"/> it will be returned as is
		/// If the string length is more it will be trankated.
		/// The last word will be removed
		/// </summary>
		/// <param name="s">Entry string</param>
		/// <param name="length">The length to be transkated</param>
		/// <returns>Returns the trankated string</returns>
		public static string Trankate(string s, int length)
		{
			return Trankate(s, length, "");
		}

		/// <summary>
		/// Gets the file name with extension from a full path string
		/// </summary>
		/// <param name="path">The full path where the file is located</param>
		/// <returns>the file name</returns>
		public static string GetFileName(string path)
		{
			return Path.GetFileName(path);
		}

		/// <summary>
		/// Gets the extension from the entry path
		/// </summary>
		/// <param name="path">The full path where the file is located</param>
		/// <returns>the returned extension</returns>
		public static string GetFileExtension(string path)
		{
			return Path.GetExtension(path).Substring(1);
		}

		/// <summary>
		/// Gets the file name without extension from a full path string
		/// </summary>
		/// <param name="path">The full path where the file is located</param>
		/// <returns>the file name</returns>
		public static string GetFriendlyFileName(string path)
		{
			return Path.GetFileNameWithoutExtension(path);
		}

		/// <summary>
		/// Generates and returns a random text of length <paramref name="len"/>
		/// </summary>
		/// <param name="len">The length of the generated text</param>
		/// <returns>The generated random text</returns>
		public static string GenerateRandomText(int len)
		{
			char[] letters = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".ToCharArray();
			var sb = new StringBuilder();

			var ran = new Random();

			for (int i = 0; i < len; i++)
			{
				sb.Append(letters[ran.Next(letters.Length)]);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Capitilizes the first character of each word in a speified string.
		/// ex: "i will be there" will return "I Will Be There"
		/// </summary>
		/// <param name="str">The entry string</param>
		/// <returns>The transformed and capitilized text</returns>
		public static string CapitilizeFirstChar(string str)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
		}

		/// <summary>
		/// Generates a password from <paramref name="firstName" /> and <paramref name="email"/> adding some random characters
		/// </summary>
		/// <param name="firstName">First Name</param>
		/// <param name="email">Email</param>
		/// <returns>The password</returns>
		public static string GeneratePassword(string firstName, string email)
		{
			string password = "";
			var r = new Random();
			int rand = r.Next(1000, 900000);
			if (firstName.Length > 2)
			{
				password += firstName.Substring(0, 2);
			}
			password += rand.ToString();
			if (email.Length > 2)
			{
				password += email.Substring(0, 2);
			}
			return password + GeneratePassword();
		}

		/// <summary>
		/// Simply generates a random password that contains only numbers
		/// </summary>
		/// <returns>The password</returns>
		public static string GeneratePassword()
		{
			string password = "";
			var r = new Random();
			int rand = r.Next(1000, 90000000);
			/*if(firstName.Length>2)
			{
				password += firstName.Substring(0,2);
			}*/
			password = rand.ToString();
			/*if(email.Length>2)
			{
				password += firstName.Substring(0,2);
			}*/
			return password;
		}

		/// <summary>
		/// Generates a random password that contains both letters and numbers upper case and lower case
		/// </summary>
		/// <returns>The password</returns>
		public static string GeneratePassword2()
		{
			string password = "";
			int rand1;
			int rand2;
			string firstname = "ABC234DertSaFRT345YBTBGtrtertFRTBND45345DSJDU445TYRTYPR45dfg234234";
			string email = "FBFJC2323e92010FKGJ3245rtjgYT3455RPOiqwuD5345jffjs4534lala983457";
			var r = new Random();
			int rand = r.Next(1000, 900000000);
			int rand3 = r.Next(1, 60);
			int rand4 = r.Next(1, 80);
			try
			{
				if (rand3 > rand4)
				{
					rand1 = rand4;
					rand2 = rand3;
				}
				else
				{
					rand1 = rand3;
					rand2 = rand4;
				}
				if (rand1 != rand2)
				{
					if (firstname.Length > 0)
					{
						if (rand1 + rand2 > firstname.Length)
						{
							rand2 = firstname.Length - rand1;
						}
						password += firstname.Substring(rand1, rand2);
					}

					if (email.Length > 0)
					{
						if (rand1 + rand2 > email.Length)
						{
							rand2 = email.Length - rand1;
						}
						password += email.Substring(rand1, rand2);
					}
				}
				if (rand1 == rand2)
				{
					password = rand.ToString();
					return password;
				}
			}
			catch (Exception)
			{
				password = rand.ToString();
				return password;
			}
			if (password.Length > 0)
			{
				try
				{
					var r2 = new Random();
					int rando = r.Next(0, password.Length);
					int rando2 = r.Next(1, password.Length);
					int rando3;
					if (rando < rando2)
					{
					}
					else
					{
						rando3 = rando2;
						rando2 = rando;
						rando = rando3;
					}
					if (rando == rando2)
					{
						password = rand.ToString();
						return password;
					}
					if (rando + rando2 > password.Length)
					{
						rando2 = password.Length - rando;
					}
					password = password.Substring(rando, rando2);
					if (password.Length > 7)
					{
						return password.Substring(0, 7);
					}
					return password;
				}
				catch (Exception)
				{
					password = rand.ToString();
					return password;
				}
			}
			password = rand.ToString();
			return password;
		}

		/// <summary>
		/// Masks a string and shows only part of that string, the rest is covered by x or *
		/// Used for hiding credit card numbers or passwords.
		/// Ex: 1234-5678-9123 will show xxxx-xxxx-9123
		/// </summary>
		/// <param name="input">The entry string to be masked</param>
		/// <param name="star">The star character can be x or * or any other character</param>
		/// <param name="length">The length of the visible characters</param>
		/// <returns>The masked string</returns>
		public static string MaskString(string input, char star, int length)
		{
			string last = input.Length > length ? input.Substring(input.Length - length) : "";
			int len = input.Length;
			return last.PadLeft(len, star);
		}

		/// <summary>
		/// Indicates the if the following object and when transformed to string is null, empty, or contains only white spaces.
		/// </summary>
		/// <param name="o">Object</param>
		/// <returns>True or false</returns>
		public static bool IsNullOrWhiteSpace(object o)
		{
			return string.IsNullOrWhiteSpace(o.ToString());
		}

		/// <summary>
		/// Indicates the if the following string and when transformed to string is null, empty, or contains only white spaces.
		/// </summary>
		/// <param name="s">String</param>
		/// <returns>True or false</returns>
		public static bool IsNullOrWhiteSpace(string s)
		{
			return string.IsNullOrEmpty(s) || string.IsNullOrEmpty(s.Trim());
		}

		/// <summary>
		/// Replaces NewLines with <Br /> to be used in HTML pages
		/// </summary>
		/// <param name="str">Entry string</param>
		/// <returns>Transformed HTML String</returns>
		public static string PutBR(string str)
		{
			str = str.Replace(Environment.NewLine, "<br />");
			return str;
		}

		/// <summary>
		/// Will return any object in the form of a Javascript JSon object {a:[1,2,3]...}
		/// </summary>
		/// <param name="obj">Any Object</param>
		/// <returns>string Json String</returns>
		public static string JSonSerialize(object obj)
		{
			return JsonConvert.SerializeObject(
				obj,
				Formatting.None,
				new JsonSerializerSettings
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					Converters = new List<JsonConverter>
					{
						new JavaScriptDateTimeConverter()
					}
				}
				);
		}

		/// <summary>
		/// Tests if a string has some unicode characters.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool ContainsUnicodeCharacter(string input)
		{
			const int MaxAnsiCode = 255;

			return input.ToCharArray().Any(c => c > MaxAnsiCode);
		}

		/// <summary>
		///     Splits a keyword into small search strings
		///     This method is not recommended but can be used in extreme cases
		///     Ex: search for alain haddad will return name like '%alain%' or name like '%haddad%'
		/// </summary>
		/// <param name="keywords">the input keyword</param>
		/// <param name="field">the name of the field</param>
		/// <returns>Search String</returns>
		public static string SplitSearchKeyword(string keywords, string field)
		{
			string[] temp = keywords.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

			var ret = new StringBuilder("");
			string sep = "";

			foreach (string str in temp)
			{
				ret.Append(sep);
				ret.Append(field + " like N'%" + SQLEncode(str) + "%'");

				sep = " or ";
			}

			ret.Append("");

			return ret.ToString();
		}


		/// <summary>
		///     Replaces Æ with <sup>Æ</sup>
		/// </summary>
		/// <param name="str">Input string</param>
		/// <returns></returns>
		public static string AddSup(string str)
		{
			str = str.Replace("<sup>&amp;reg;</sup>", "Æ");
			str = str.Replace("<sup>Æ</sup>", "Æ");
			str = str.Replace("&amp;reg;", "&reg;");
			str = str.Replace("&reg;", "<sup>&reg;</sup>");
			str = str.Replace("Æ", "<sup>&reg;</sup>");

			return str;
		}

		/// <summary>
		///     Replaces the text only outside html tags and does not touch any text that is inside a tag
		/// </summary>
		/// <param name="me">The main string me.ReplaceTextOutsideHthmlTags</param>
		/// <param name="patern">The entry Patern</param>
		/// <param name="replacement">The replacement Patern</param>
		/// <returns>The modified string</returns>
		public static string ReplaceTextOutsideHthmlTags(this string me, string patern, string replacement)
		{
			return me.ReplaceTextOutsideHthmlTags(patern, replacement, RegexOptions.IgnoreCase);
		}

		/// <summary>
		///     Replaces the text only outside html tags and does not touch any text that is inside a tag
		/// </summary>
		/// <param name="me">The main string me.ReplaceTextOutsideHthmlTags</param>
		/// <param name="patern">The entry Patern</param>
		/// <param name="replacement">The replacement Patern</param>
		/// <param name="options">Defines the Regex options for the replace</param>
		/// <returns>The modified string</returns>
		public static string ReplaceTextOutsideHthmlTags(this string me, string patern, string replacement, RegexOptions options)
		{
			var reg = new Regex(@"" + patern + @"(?=[^<>]*<)");

			return Regex.Replace(me, reg.ToString(), replacement, options);
		}

		/// <summary>
		/// Acts like String.Replace but case insensitive
		/// </summary>
		/// <param name="me">The main string me.ReplaceIgnoreCase</param>
		/// <param name="Patern">The entry Patern</param>
		/// <param name="Replacement">The replacement Patern</param>
		/// <returns>The modified string</returns>
		public static string ReplaceIgnoreCase(this string me, string patern, string replacement)
		{
			return Regex.Replace(me, patern, replacement, RegexOptions.IgnoreCase);
		}

        public static string EnsureString(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
		

		/// <summary>
		/// Ensures that SABIS is not italic
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static string ReplaceItalicSabis(string b)
		{
			b = b.ReplaceTextOutsideHthmlTags("sabis", "<bdi>sabis</bdi>", RegexOptions.CultureInvariant);
			b = b.ReplaceTextOutsideHthmlTags("SABIS", "<bdi>SABIS</bdi>", RegexOptions.CultureInvariant);
			b = b.ReplaceTextOutsideHthmlTags("<bdi>SABIS</bdi><sup>&reg;</sup>", "<bdi>SABIS<sup>&reg;</sup></bdi>", RegexOptions.CultureInvariant);
			return b;
		}

		/// <summary>
		/// adds super script to th, st, and rd when they come after a number
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static string ReplaceNbSup(string b)
		{
			for (int i = 0; i < b.Length; i++)
			{
				if (i + 1 < b.Length && i + 2 < b.Length)
				{
					string afterNum = b[i + 1].ToString() + b[i + 2].ToString();
					string sup = string.Empty;

                    switch (afterNum)
                    {
                        case "th ":
                        case "st ":
                        case "rd ":
                        case "nd ":
                            sup = "<sup>" + afterNum + "</sup> ";
                            break;
                        case "th.":
                        case "st.":
                        case "rd.":
                        case "nd.":
                            sup = "<sup>" + afterNum + "</sup>.";
                            break;
                        default:
                            sup = "";
                            break;
                    }
					if (Char.IsDigit(b[i]) && !string.IsNullOrWhiteSpace(sup))
					{
						b = b.Replace(b[i].ToString() + afterNum, b[i].ToString() + sup);
					}
				}
			}
			return b;
        }

        /// <summary>
        /// Gets the FileName from the given file and transforms it into a URL friendly name
        /// </summary>
        /// <param name="file">The posted file to take the name from</param>
        /// <returns>string: file name in a URL friendly format</returns>
        public static string FileNameToURL(HttpPostedFile file)
        {
            string name = Path.GetFileName(file.FileName);
            string ext = Path.GetExtension(file.FileName);
            string nameToURL = ToURL(name);

            return nameToURL + ext;
        }
    }
}