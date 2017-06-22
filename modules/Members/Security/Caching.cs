using System;
using System.Web.Caching;

using lw.CTE;
using lw.WebTools;

namespace lw.Members.Security
{
	public class Caching
	{
		public static void UpdateUserCache()
		{
           // return;
			if (WebContext.Profile != null && WebContext.Profile.UserLogged)
			{
				string CacheKey = CteCache.UserOnlinePrefix + WebContext.Profile.UserId.ToString();
				CacheItemRemovedCallback r = new CacheItemRemovedCallback(TimeoutUser);

				lw.Members.MembersManager.UpdateOnlineStatus(WebContext.Profile.UserId, true);

				//Difference between Cache.Insert and Cache.Add
				//Cache.Insert invoke > Remove then Add
				//Cache.Add invoke > Add (error when item already exists)
				//But from testing it just updates the key to the new value.

				WebContext.Cache.Add(
					CacheKey, WebContext.Profile.UserId,
					null, DateTime.Now.AddMinutes(CTE.Caching.UserOnlineTimeout),
					Cache.NoSlidingExpiration,
					CacheItemPriority.High, r);
			}
		}

		public static void TimeoutUser(string key, object v, System.Web.Caching.CacheItemRemovedReason r)
		{
         //   return;
			if (r == CacheItemRemovedReason.Removed
				|| r == CacheItemRemovedReason.DependencyChanged || 
				r == CacheItemRemovedReason.Expired)
			{
				string userId = v.ToString();
				
				WebContext.CreateHttpContext();
				
				try
				{
					MembersManager.UpdateOnlineStatus(Int32.Parse(userId), false);
				}
				catch (Exception Ex)
				{
					throw (Ex);
				}
			}
		}
	}
}
