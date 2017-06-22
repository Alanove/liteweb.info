using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using lw.CTE;
using lw.CTE.Enum;
using lw.Data;
using lw.GraphicUtils;
using lw.Utils;
using lw.WebTools;

namespace lw.Articles.LINQ
{
    public class NewsTypesManager : LINQManager
    {
        public NewsTypesManager()
            : base(cte.lib)
        {
        }

        public int AddNewsType(System.Web.HttpRequest req)
        {
            if (StringUtils.IsNullOrWhiteSpace(req.Form["Name"]))
                return -1;

            Status status = Status.Disabled;
            if (req.Form["Status"] == "on")
                status = Status.Enabled;

            return AddNewsType(req.Form["Name"], status, req.Files["Image"], req.Form["Description"], Parsers.Int(req.Form["ParentCategories"]),
                Parsers.Short(req.Form["ThumbWidth"]), Parsers.Short(req.Form["ThumbHeight"]),
                Parsers.Short(req.Form["MediumWidth"]), Parsers.Short(req.Form["MediumHeight"]),
                Parsers.Short(req.Form["LargeWidth"]), Parsers.Short(req.Form["LargeHeight"]),
                -1, -1);
        }

        public int AddNewsType(string Name, Status Status, HttpPostedFile Image, string Description, int? ParentId,
        short? ThumbWidth, short? ThumbHeight, short? MediumWidth, short? MediumHeight, short? LargeWidth, short? LargeHeight, int? TemplateId,
            int? Permission)
        {
            if (GetType(Name) != null)
                return -1;

            NewsType type = new NewsType
            {
                Name = Name,
                UniqueName = StringUtils.ToURL(Name),
                Status = (byte)Status,
                Description = Description,
                ParentId = ParentId,
                ThumbWidth = ThumbWidth,
                ThumbHeight = ThumbHeight,
                MediumWidth = MediumWidth,
                MediumHeight = MediumHeight,
                LargeWidth = LargeWidth,
                LargeHeight = LargeHeight,
                DateCreated = DateTime.Now,
                LastModified = DateTime.Now,
                TemplateId = TemplateId,
                Ranking = 0,
                UserRating = 0,
                Views = 0,
                Permission = Permission
            };

            NewsTypesData.NewsTypes.InsertOnSubmit(type);
            NewsTypesData.SubmitChanges();

            if (Image != null && Image.ContentLength > 0)
            {
                string ImageName = Path.GetFileNameWithoutExtension(Image.FileName);
                ImageName = string.Format("{0}_{1}{2}", ImageName, type.TypeId,
                    Path.GetExtension(Image.FileName));

                string path = WebContext.Server.MapPath(Path.Combine(WebContext.StartDir, Folders.NewsTypesFolder));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, ImageName);

                Image.SaveAs(path);

                if (ThumbWidth > 0 && ThumbHeight > 0)
                {
                    ImageUtils.CropImage(path, path, (int)ThumbWidth, (int)ThumbHeight, ImageUtils.AnchorPosition.Default);
                }

                type.Image = ImageName;

                NewsTypesData.SubmitChanges();

            }
            return type.TypeId;
        }

        public int UpdateNewsType(int TypeId, System.Web.HttpRequest req)
        {
            Status status = Status.Disabled;
            if (req.Form["Status"] == "on")
                status = Status.Enabled;

            int? Ranking = null, UserRating = null, Views = null;

            if (!String.IsNullOrEmpty(req["Ranking"]))
                Ranking = Int32.Parse(req["Ranking"]);

            if (!String.IsNullOrEmpty(req["UserRating"]))
                UserRating = Int32.Parse(req["UserRating"]);

            if (!String.IsNullOrEmpty(req["Views"]))
                Views = Int32.Parse(req["Views"]);


            bool DeleteImage = false;

            if (req.Form["DeleteImage"] != null)
                DeleteImage = req.Form["DeleteImage"] == "on";

            return UpdateNewsType(TypeId, req.Form["Name"], status, req.Files["Image"], req.Form["Description"], Parsers.Int(req.Form["ParentCategories"]),
                Parsers.Short(req.Form["ThumbWidth"]), Parsers.Short(req.Form["ThumbHeight"]),
                Parsers.Short(req.Form["MediumWidth"]), Parsers.Short(req.Form["MediumHeight"]),
                Parsers.Short(req.Form["LargeWidth"]), Parsers.Short(req.Form["LargeHeight"]),
                -1, -1, DeleteImage, Ranking, UserRating, Views);
        }

        public int UpdateNewsType(int TypeId, string Name, Status Status, HttpPostedFile Image, string Description, int? ParentId,
    short? ThumbWidth, short? ThumbHeight, short? MediumWidth, short? MediumHeight, short? LargeWidth, short? LargeHeight, int? TemplateId,
            int? Permission, bool DeleteOldImage, int? Ranking, int? UserRating, int? Views)
        {
            var type = GetType(TypeId);

            type.Name = Name;
            type.UniqueName = StringUtils.ToURL(Name);
            type.Status = (byte)Status;
            type.Description = Description;
            type.ParentId = ParentId;
            type.ThumbWidth = ThumbWidth;
            type.ThumbHeight = ThumbHeight;
            type.MediumWidth = MediumWidth;
            type.MediumHeight = MediumHeight;
            type.LargeWidth = LargeWidth;
            type.LargeHeight = LargeHeight;
            type.LastModified = DateTime.Now;
            type.TemplateId = TemplateId;

            if (Ranking != null)
                type.Ranking = Ranking.Value;
            if (UserRating != null)
                type.UserRating = UserRating.Value;
            if (Views != null)
                type.Views = Views.Value;
            type.Permission = Permission;

            NewsTypesData.SubmitChanges();

            string path = WebContext.Server.MapPath(Path.Combine(WebContext.StartDir, Folders.NewsTypesFolder));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var oldImage = type.Image;

            DeleteOldImage = DeleteOldImage || (Image != null && Image.ContentLength > 0);

            if (DeleteOldImage && !StringUtils.IsNullOrWhiteSpace(type.Image))
            {
                if (File.Exists(Path.Combine(path, oldImage)))
                    File.Delete(Path.Combine(path, oldImage));
                type.Image = "";
            }

            if (Image != null && Image.ContentLength > 0)
            {
                string ImageName = Path.GetFileNameWithoutExtension(Image.FileName);
                ImageName = string.Format("{0}_{1}{2}", ImageName, type.TypeId,
                    Path.GetExtension(Image.FileName));

                string _path = Path.Combine(path, ImageName);

                Image.SaveAs(_path);

				Config cfg = new Config();
				Dimension dim = new Dimension(cfg.GetKey(lw.CTE.Settings.NewsCategory_ImageSize));


				if (dim.Valid)
                {
                    ImageUtils.CropImage(_path, _path, dim.IntWidth, dim.IntHeight, ImageUtils.AnchorPosition.Default);
                }

                type.Image = ImageName;

            }
            NewsTypesData.SubmitChanges();
            return TypeId;
        }


        public bool DeleteNewsType(int TypeId)
        {
            DataView types = GetChildrenNewsTypes(TypeId);
            foreach (DataRowView drv in types)
                DeleteNewsType((Int32)drv["TypeId"]);

            try
            {
                var type = GetType(TypeId);

                if (!StringUtils.IsNullOrWhiteSpace(type.Image))
                {
                    string path = WebContext.Server.MapPath(Path.Combine(WebContext.StartDir, Folders.NewsTypesFolder));

                    if (Directory.Exists(path))
                    {
                        var Image = type.Image;
                        if (File.Exists(Path.Combine(path, Image)))
                            File.Delete(Path.Combine(path, Image));
                    }
                }

                NewsTypesData.NewsTypes.DeleteOnSubmit(type);
                NewsTypesData.SubmitChanges();
            }
            catch (Exception ex)
            {
                ErrorContext.Add("delete-newstype", "Unable to delete news type.<br />" + ex.Message);
                return false;
            }

            return true;
        }


        /// <summary>
        /// Returns all news types
        /// </summary>
        /// <returns></returns>
        public IQueryable<NewsType> GetTypes()
        {
            return from newsType in NewsTypesData.NewsTypes
                   select newsType;
        }


        /// <summary>
        /// returns all the children for the specified news type.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IQueryable<NewsType> GetTypes(string parent)
        {
            int parentId = -1;

            NewsType _parent = GetType(parent);
            if (_parent != null)
                parentId = _parent.TypeId;

            return from newsType in GetTypes()
                   where
                    newsType.ParentId == parentId
					orderby newsType.Ranking descending
                   select newsType;
        }

        /// <summary>
        /// change the ranking for the current record and updates the other ones accordingly.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="d"></param>
        /// <param name="ss"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public void UpdateRanking(int id, int d, int ss, int? pid)
        {
            if ((d != 1) && (d != -1))
            {
                throw new Exception("Direction value incorrect");
            }
            IQueryable<New> allNews = null;
            IQueryable<NewsType> allTypes = null;
            if (pid.HasValue && pid.Value > 0)
            {
                NewsManager nMgr = new NewsManager();
                New selectedRecord = nMgr.NewsData.News.SingleOrDefault(x => x.NewsId == id);
                if (selectedRecord == null)
                {
                    throw new Exception("Record not found");
                }
                allNews = from newsTypeView in nMgr.NewsData.News
                             where
                              newsTypeView.NewsType == pid
                           select newsTypeView;
                // increasing priority
                if (d == -1)
                {
                    //select all where priority > currentPriority and limiting results to maximum the stepsize
                    var recordsToUpdate = (from s in allNews
                                           where s.Ranking > selectedRecord.Ranking
                                           orderby s.Ranking
                                           select s).Take(ss);
                    if (recordsToUpdate.Count() > 0)
                    {
                        int maxPriority = Convert.ToInt32(recordsToUpdate.Max(x => x.Ranking));
                        foreach (var s in recordsToUpdate)
                        {
                            s.Ranking--;
                        }
                        selectedRecord.Ranking = maxPriority;
                        nMgr.Save();
                    }
                }
                // lowering priority
                if (d == 1)
                {
                    if (selectedRecord == null)
                    {
                        throw new Exception("Record not found");
                    }
                    //select all where priority < currentPriority and limiting results to maximum the stepsize
                    var recordsToUpdate = (from s in allNews
                                           where s.Ranking < selectedRecord.Ranking
                                           orderby s.Ranking descending
                                           select s).Take(ss);
                    if (recordsToUpdate.Count() > 0)
                    {
                        int minimumPriority = Convert.ToInt32(recordsToUpdate.Min(x => x.Ranking));
                        foreach (var s in recordsToUpdate)
                        {
                            s.Ranking++;
                        }
                        selectedRecord.Ranking = minimumPriority;
                        nMgr.Save();
                    }
                }
            }
            else
            {
                NewsType selectedRecord = GetType(id);
                allTypes = from newsType in GetTypes()
                             where
                              newsType.ParentId == 12
                             select newsType;
                if (selectedRecord == null)
                {
                    throw new Exception("Record not found");
                }
                // increasing priority
                if (d == -1)
                {
                    //select all where priority > currentPriority and limiting results to maximum the stepsize
                    var recordsToUpdate = (from s in allTypes
                                           where s.Ranking > selectedRecord.Ranking
                                           orderby s.Ranking
                                           select s).Take(ss);
                    if (recordsToUpdate.Count() > 0)
                    {
                        int maxPriority = Convert.ToInt32(recordsToUpdate.Max(x => x.Ranking));
                        foreach (var s in recordsToUpdate)
                        {
                            s.Ranking--;
                        }
                        selectedRecord.Ranking = maxPriority;
                        this.Save();
                    }
                }
                // lowering priority
                if (d == 1)
                {
                    //select all where priority < currentPriority and limiting results to maximum the stepsize
                    var recordsToUpdate = (from s in allTypes
                                           where s.Ranking < selectedRecord.Ranking
                                           orderby s.Ranking descending
                                           select s).Take(ss);
                    if (recordsToUpdate.Count() > 0)
                    {
                        int minimumPriority = Convert.ToInt32(recordsToUpdate.Min(x => x.Ranking));
                        foreach (var s in recordsToUpdate)
                        {
                            s.Ranking++;
                        }
                        selectedRecord.Ranking = minimumPriority;
                        this.Save();
                    }
                }
            }
        }

        public void UpdateMenuRanking(string data)
        {
            var parents = data.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);
            var rootSql = "update NewsTypes set Ranking = rank from (values ";
            var newsSql = "update News set Ranking = rank, NewsType = parent from (values ";
            var parentMax = 10000;
            var childrenMax = 20000;
            foreach (var p in parents)
            {
                var children = p.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                rootSql += "(" + children[0] + ", " + parentMax-- + "),";
                if (children.Length > 1)
                {
                    newsSql += "(" + children[1] + ", " + childrenMax-- + ", " + children[0] + "),";
                }
            }
            if (rootSql.EndsWith(","))
            {
                rootSql = rootSql.Substring(0, rootSql.Length - 1);
            }
            rootSql += " ) p(i, rank) where i=TypeId;";
            if (newsSql.EndsWith(","))
            {
                newsSql = newsSql.Substring(0, newsSql.Length - 1);
            }
            newsSql += " ) p(i, rank, parent) where i=NewsId;";

            DBUtils.GetDataSet(rootSql, cte.lib);
            DBUtils.GetDataSet(newsSql, cte.lib);
        }

        public int GetNewsTypesMaxRank()
        {
            int returnedMenu = 1;
            try
            {
                returnedMenu = Convert.ToInt32(NewsTypesData.NewsTypes.Max(s => s.Ranking));
                return returnedMenu;
            }
            catch
            {
                return returnedMenu;
            }
        }

        public int GetNewsMaxRank(NewsManager nMgr)
        {
            int returnedMenu = 1;
            try
            {
                returnedMenu = Convert.ToInt32(nMgr.NewsData.News.Max(s => s.Ranking));
                return returnedMenu;
            }
            catch
            {
                return returnedMenu;
            }
        }

        public NewsType GetType(int TypeId)
        {
            return NewsTypesData.NewsTypes.Single(temp => temp.TypeId == TypeId);
        }

        public NewsType GetType(string Name)
        {
            var query = from nt in NewsTypesData.NewsTypes
                        where nt.Name == Name ||
                        nt.UniqueName == Name
                        select nt;
            if (query.Count() > 0)
                return query.Single();
            return null;
        }

        public DataView GetChildrenNewsTypes(int TypeId)
        {
            return GetChildrenNewsTypes(TypeId, "");
        }

        public DataView GetChildrenNewsTypes(int TypeId, string cond)
        {
            if (cond != "")
                cond = " And " + cond;
            DataView cats = GetNewsTypesView(string.Format("ParentId={0}{1}", TypeId, cond));

            return cats;
        }

        public DataView GetRootNewsTypes()
        {
            return GetNewsTypesView(string.Format("ParentId={0}", -1));
        }

        public DataView GetNewsTypesView(string cond)
        {
            if (cond.Trim() != "")
                cond = " Where " + cond;


            cond = string.Format("select * from NewsTypesView {0}",
                        cond);

            return DBUtils.GetDataSet(cond.ToString(), cte.lib).Tables[0].DefaultView;
        }

        public DataTable GetNewsTypes(string cond)
        {
            if (cond.Trim() != "")
                cond = " Where " + cond;

            cond = string.Format("select * from NewsTypes {0}",
                        cond);

            return DBUtils.GetDataSet(cond.ToString(), cte.lib).Tables[0];
        }

        public DataRow GetNewsType(int TypeId)
        {
            DataTable cats = GetNewsTypes(string.Format("TypeId={0}", TypeId));

            if (cats.Rows.Count > 0)
                return (DataRow)cats.Rows[0];
            return null;
        }

        public DataRow GetNewsTypeView(string NewsType)
        {
            DataView cats = GetNewsTypesView(string.Format("UniqueName=N'{0}' or Name=N'{0}'", StringUtils.SQLEncode(NewsType)));

            if (cats.Count > 0)
                return (DataRow)cats.Table.Rows[0];
            return null;
        }

        public DataRow GetNewsTypeView(int TypeId)
        {
            DataView cats = GetNewsTypesView(string.Format("TypeId={0}", TypeId));

            if (cats.Count > 0)
                return (DataRow)cats.Table.Rows[0];
            return null;
        }


        #region Static Methods

        public static string GetThumbImage(DataRow news)
        {
            if (String.IsNullOrEmpty(news["Image"].ToString()))
                return "";
            return Path.Combine("/" + Folders.NewsTypesFolder, news["Image"].ToString());
        }

        #endregion

        #region Variables

        public NewsTypesDataContext NewsTypesData
        {
            get
            {
                if (_dataContext == null)
                    _dataContext = new NewsTypesDataContext(this.Connection);
                return (NewsTypesDataContext)_dataContext;
            }
        }

        #endregion
    }
}
