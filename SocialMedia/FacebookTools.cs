using lw.WebTools;
using Facebook;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lw.Data;

namespace lw.SocialMedia
{
    public static class FacebookTools
    {
		/// <summary>
		/// Posts the specified page to the Facebook related to the website.
		/// </summary>
		/// <param name="pageId">The Page ID (From the Database)</param>
		/// <param name="link">The link that should be shared.</param>
		/// <param name="message">The message that goes with the link</param>
        public static void PostToPage(int pageId,string link,string message)
        {
            var fb = new FacebookClient(lw.WebTools.Config.GetFromWebConfig(lw.CTE.parameters.FacebookAccessToken));

            
            fb.PostCompleted += (o, e) =>
            {
                if (e.Error == null)
                {
                    var result = (IDictionary<string, object>)e.GetResultData();
                    var newPostId = (string)result["id"];
                    string sql = "INSERT INTO [SocialMediaPost] ([PageId], [DatePosted], [Type]) VALUES ({0}, '{1}', {2});";
                    sql = string.Format(sql, pageId, DateTime.Now, (int)SocialMediaTypes.Facebook);
                    DBUtils.ExecuteQuery(sql, cte.lib);
                    
                }
            };

            var parameters = new Dictionary<string, object>();
            parameters["link"] = link;
            parameters["message"] = message;
            fb.PostTaskAsync("me/feed", parameters);
        }
    }
}
