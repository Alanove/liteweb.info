//using System;
//using System.IO;
//using System.Net;
//using System.Security.Cryptography;
//using System.Text;
//using Twitterizer;
//using System.Web;
//using lw.Data;


//namespace lw.SocialMedia
//{
//	public static class TwitterTools
//	{
//		public static void PostMessageToTwitter(int pageId,string message, string imagePath)
//		{
//			OAuthTokens accesstoken = new OAuthTokens()
//			{
//				AccessToken = lw.WebTools.Config.GetFromWebConfig(lw.CTE.parameters.TwitterToken),
//				AccessTokenSecret = lw.WebTools.Config.GetFromWebConfig(lw.CTE.parameters.TwitterSecretToken),
//				ConsumerKey = lw.WebTools.Config.GetFromWebConfig(lw.CTE.parameters.TwitterConsumerKey),
//				ConsumerSecret = lw.WebTools.Config.GetFromWebConfig(lw.CTE.parameters.TwitterConsumerSecret)

//			};

//			StatusUpdateOptions opts = new StatusUpdateOptions();
//			if(!string.IsNullOrEmpty(imagePath))
//			{
//				byte[] photo = File.ReadAllBytes(HttpContext.Current.Server.MapPath("/"+imagePath));

//				TwitterResponse<TwitterStatus> response = TwitterStatus.UpdateWithMedia(accesstoken, message, photo);﻿
                
//			}
//			else
//			{
//				TwitterResponse<TwitterStatus> response = TwitterStatus.Update(accesstoken, message);
//			}

//			string sql = "INSERT INTO [SocialMediaPost] ([PageId], [DatePosted], [Type]) VALUES ({0}, '{1}', {2});";
//			sql = string.Format(sql, pageId, DateTime.Now, (int)SocialMediaTypes.Twitter);
//			DBUtils.ExecuteQuery(sql, cte.lib);
//		}
//	}
//}
