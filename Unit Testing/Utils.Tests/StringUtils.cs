using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using lw.Utils;

namespace lw.Utils.Tests
{
	[TestClass]
	public class StringUtilsTests
	{
		public TestContext TestContext { get; set; }

		[TestMethod]
		[TestCategory("ToURL")]
		[Owner("Alain")]
		public void ToUrlSimple()
		{
			string url = StringUtils.ToURL("abc def");
			Assert.AreEqual("abc-def", url);
		}

		[TestMethod]
		[TestCategory("ToURL")]
		[Owner("Alain")]
		public void ToUrlSimpleSecondEnptyParameter()
		{
			string url = StringUtils.ToURL("abc def", "");
			Assert.AreEqual("abcdef", url);
		}

		[TestMethod]
		[TestCategory("ToURL")]
		[Owner("Alain")]
		public void ToUrlLSimpleSecondParameter()
		{
			string url = StringUtils.ToURL("abc def", "~");
			Assert.AreEqual("abc~def", url);
		}

		[TestMethod]
		[TestCategory("ToURL")]
		[Owner("Alain")]
		public void ToUrlComplicated()
		{
			string url = StringUtils.ToURL("àÀâÂäÄáÁéÉèÈêÊëËìÌîÎïÏòÒôÔöÖùÙûÛüÜçÇ’ñ/ó:");
			Assert.AreEqual("aaaaaaaaeeeeeeeeiiiiiioooooouuuuuucc-n-o", url);
		}

		[TestMethod]
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
			"|DataDirectory|\\TestUtils.xml", "ToUrl", DataAccessMethod.Sequential)]
		[Owner("Alain")]
		public void ToUrlDataDriven()
		{
			string entry = this.TestContext.DataRow["entry"].ToString();
			string replacement = null;
			if (this.TestContext.DataRow["replacement"] != DBNull.Value)
				replacement = this.TestContext.DataRow["replacement"].ToString();
			string result = this.TestContext.DataRow["result"].ToString();

			Assert.AreEqual(result, StringUtils.ToURL(entry, replacement));
		}


		[TestMethod]
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
			"|DataDirectory|\\TestUtils.xml", "StripOutHtmlTags", DataAccessMethod.Sequential)]
		[Owner("Alain")]
		public void StripOutHtmlTagsTest()
		{
			string entry = this.TestContext.DataRow["entry"].ToString();
			string result = this.TestContext.DataRow["result"].ToString();

			Assert.AreEqual(result, StringUtils.StripOutHtmlTags(entry), result);
		}	
	}
}