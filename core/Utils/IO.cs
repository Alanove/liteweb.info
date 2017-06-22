using System.Collections.Specialized;
using System.IO;

namespace lw.Utils
{

	/// <summary>
	/// Provides static utilities associated with input and output on files
	/// </summary>
	public class IO
	{
		/// <summary>
		/// Saves a stream into a file
		/// </summary>
		/// <param name="stream">The input stream</param>
		/// <param name="fileName">The file location where the input stream should be saved</param>
		public static void SaveStream(Stream stream, string fileName)
		{
			MemoryStream ret = new MemoryStream();

			byte[] buffer = new byte[stream.Length];
			int bytesRead;
			while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
			{
				ret.Write(buffer, 0, count: bytesRead);
			}
			ret.Position = 0;

			SaveMemoryStream(ret, fileName);
		}

		/// <summary>
		/// Saves a memory stream into a file
		/// </summary>
		/// <param name="ms">The input memory stream</param>
		/// <param name="fileName">The file location where the input stream should be saved</param>
		public static void SaveMemoryStream(MemoryStream ms, string fileName)
		{
			FileStream outStream = File.OpenWrite(fileName);
			ms.WriteTo(outStream);
			outStream.Flush();
			outStream.Close();
		}

		/// <summary>
		/// Reads a file into a memory stream
		/// </summary>
		/// <param name="fileName">The file location where the input stream should read</param>
		/// <returns></returns>
		public static MemoryStream ReadMemoryStream(string fileName)
		{
			FileStream inStream = File.OpenRead(fileName);
			MemoryStream storeStream = new MemoryStream();

			storeStream.SetLength(inStream.Length);
			inStream.Read(storeStream.GetBuffer(), 0, (int)inStream.Length);

			storeStream.Flush();
			inStream.Close();

			return storeStream;
		}

		/// <summary>
		/// Reads a file into a memory stream
		/// </summary>
		/// <param name="fileInfo">FileInfo</param>
		/// <returns></returns>
		public static MemoryStream ReadMemoryStream(FileInfo fileInfo)
		{
			FileStream inStream = fileInfo.OpenRead();
			MemoryStream storeStream = new MemoryStream();

			storeStream.SetLength(inStream.Length);
			inStream.Read(storeStream.GetBuffer(), 0, (int)inStream.Length);

			storeStream.Flush();
			inStream.Close();

			return storeStream;
		}

		/// <summary>
		/// Returns the mime type of a specified file
		/// </summary>
		/// <param name="fileName">The file name</param>
		/// <returns>the mime type</returns>
		public static string GetMimeType(string fileName)
		{
			string extension = StringUtils.GetFileExtension(fileName);
			string mimeType = "";

			NameValueCollection mimeTypes = new NameValueCollection();
			mimeTypes.Add("pdf", "Application/pdf");
			mimeTypes.Add("txt", "text/plain");
			mimeTypes.Add("html", "text/html");
			mimeTypes.Add("exe", "application/octet-stream");
			mimeTypes.Add("zip", "application/zip");
			mimeTypes.Add("doc", "application/msword");
			mimeTypes.Add("xls", "application/vnd.ms-excel");
			mimeTypes.Add("ppt", "application/vnd.ms-powerpoint");
			mimeTypes.Add("gif", "image/gif");
			mimeTypes.Add("png", "image/png");
			mimeTypes.Add("jpeg", "image/jpg");
			mimeTypes.Add("jpg", "image/jpg");
			mimeTypes.Add("php", "text/plain");
			mimeTypes.Add("js", "text/plain");
			mimeTypes.Add("xml", "text/xml");
			mimeTypes.Add("mp4", "video/mp4");
			mimeTypes.Add("mp3", "audio/mpeg");
			mimeTypes.Add("mov", "video/quicktime");

			if (mimeTypes[extension] != null)
				mimeType = mimeTypes[extension];

			return mimeType;
		}

	}
}
