using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using lw.WebTools;
using System.Threading;
using System.Xml;
using lw.Data;



namespace SABIS.Controls
{
    public class SABISNewsArabicDataSource : SABISNewsDataSource
    {
         string _feedUrl = null;
         public override string FeedUrl
         {
             get
             {
                 if (_feedUrl == null)
                 {
                     _feedUrl = lw.WebTools.Config.GetFromWebConfig(cte.ArabicRssFeed);
                     if (String.IsNullOrWhiteSpace(_feedUrl))
                     {
                         _feedUrl = cte.DefaultArabicRssFeed;
                     }
                 }

                 return _feedUrl;
             }
         }

         string _cacheNews = null;
         public override string CacheNews
         {
             get
             {
                 if (_cacheNews == null)
                 {
                     _cacheNews = "SABISNewsArabic";
                 }

                 return _cacheNews;
             }
             set
             {
                 _cacheNews = value;
             }
         }
    }
}