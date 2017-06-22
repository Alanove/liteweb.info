using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using lw.Data;
using lw.Utils;
using lw.CTE;
using lw.WebTools;
using lw.GraphicUtils;

namespace lw.Networking
{
	public class NetworksManager : LINQManager
	{
		public NetworksManager() : base(cte.lib)
		{
		}


		#region Get
        // Gets network's image path
        public static string GetImagesPath(int NetworkId)
        {
            string path = WebContext.Root + "/" + CTE.Folders.Networks;
            if (NetworkId > 0)
                path = string.Format("{0}/Network{1}", path, NetworkId);
            return path;
        }

        public static string GetImagesPath()
        {
            return GetImagesPath(-1);
        }

		/// <summary>
		/// Selects the network with a specific name having parentId
		/// If no network is found null is returned
		/// </summary>
		/// <param name="name">Name or the unique name (url name) of the network</param>
		/// <param name="parentId">The Id of the parent network</param>
		/// <returns><seealso cref="Network"/> Object</returns>
		public Network GetNetwork(string name, int parentId)
		{
			var query = from network in NetworkData.Networks
						where 
							(network.Name == name || network.UniqueName == name)
							&& 
 							network.ParentId == parentId
						select network;
			if (query.Count() > 0)
				return query.Single();
			return null;
		}


		/// <summary>
		/// Selects the network with a specific id
		/// If no network is found null is returned
		/// </summary>
		/// <param name="networkId">The NetworkId</param>
		/// <returns><seealso cref="Network"/> Object</returns>
		public Network GetNetwork(int networkId)   
		{
			var query = from network in NetworkData.Networks
						where
							network.NetworkId == networkId
						select network;
			if (query.Count() > 0)
				return query.Single();
			return null;
		}

		/// <summary>
		/// Selects the first network with a specific name 
		/// If no network is found null is returned
		/// </summary>
		/// <param name="name">Name or the unique name (url name) of the network</param>
		/// <returns><seealso cref="Network"/> Object</returns>
		public Network GetNetwork(string name)
		{
			var query = from network in NetworkData.Networks
						where
							(network.Name == name || network.UniqueName == name)
						select network;
			if (query.Count() > 0)
				return query.First();
			return null;
		}

		/// <summary>
		/// Selects the last Network added with a specific status
		/// If no network is found null is returned
		/// </summary>
		/// <param name="name">Status of the Network (Enabled/Disabled)</param>
		public Network GetNetworkByStatus(int? status)
		{
			var query = from network in NetworkData.Networks
						where
							(network.Status == (int)lw.CTE.Enum.Status.Enabled)
						orderby network.NetworkId descending
						select network;
			
			if (query.Count() > 0)
				return query.First();
			return null;
		}
				
		/// <summary>
		/// return all the networks
		/// </summary>
		public IQueryable<Network> GetNetworks()
		{
			var query = from network in NetworkData.Networks
						select network;

			return query;
		}

		/// <summary>
		/// return all the networks related to a certain parentId
		/// </summary>
		public IQueryable<Network> GetNetworks(int? ParentId)
		{
			var query = from network in NetworkData.Networks
						where network.ParentId == ParentId
						select network;

			return query;
		}

		public DataTable GetNetworkDT(string cond)
		{
			StringBuilder sql = new StringBuilder("select * from Networks");

			cond = cond.Trim();
			System.Text.RegularExpressions.Regex regexp = new System.Text.RegularExpressions.Regex("^and");
			cond = regexp.Replace(cond, "");

			if (!String.IsNullOrEmpty(cond))
				sql.Append(string.Format(" where {0}", cond));


			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
		}

		public DataTable GetNetworkDescendants(int NetworkId)
		{
			return DBUtils.GetDataSet(string.Format("select * from dbo.Networks_GetDescendants({0})",
				NetworkId), lw.Networking.cte.lib).Tables[0];
		}

        // get network ancestor
        public DataTable GetNetworkAncestors(int NetworkId)
        {
            return DBUtils.GetDataSet(string.Format("select * from dbo.Network_GetAncestors({0})",
                NetworkId), lw.Networking.cte.lib).Tables[0];
        }


		#endregion

	
		#region Create

		/// <summary>
		/// Adds a network without an image
		/// </summary>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="parentId"></param>
		/// <param name="operatorId"></param>
		/// <param name="website"></param>
		/// <param name="xmlField"></param>
		/// <returns></returns>
		public Network AddNetwork(string name, string description, int parentId, int operatorId, string website, string xmlField)
		{
			return AddNetwork(name, (int)lw.CTE.Enum.Status.Disabled, description, parentId, operatorId, website, xmlField);
		}

		/// <summary>
		/// Adds a network without an image
		/// </summary>
		/// <param name="name"></param>
		/// <param name="status"></param>
		/// <param name="description"></param>
		/// <param name="parentId"></param>
		/// <param name="operatorId"></param>
		/// <param name="website"></param>
		/// <param name="xmlField"></param>
		/// <returns></returns>
		public Network AddNetwork(string name, int? status, string description, int parentId, int operatorId, string website, string xmlField)
		{
			if (GetNetwork(name, parentId) != null)
			{
				Errors.Add(new Exception("Another network with the same name already exists for this parent network"));
				return null;
			}

			var ParentUniqueName = GetNetwork(parentId);

			Network network = new Network
			{
				Name = name,
				Description = description,
				ParentId = parentId,
				UniqueName = StringUtils.ToURL(ParentUniqueName != null ? string.Format("{0}-{1}", ParentUniqueName.UniqueName, name) : name),
				DateCreated = DateTime.Now,
				LastModified = DateTime.Now,
				OperatorId = operatorId,
				Website = website,
				XmlField = System.Xml.Linq.XElement.Parse(xmlField),
				Status = status
			};

			NetworkData.Networks.InsertOnSubmit(network);
			Save();
			
			return network;
		}

		/// <summary>
		/// Adds a network with an image.
		/// For default Image sizes please use 0.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="parentId"></param>
		/// <param name="operatorId"></param>
		/// <param name="website"></param>
		/// <param name="xmlField"></param>
		/// <returns></returns>
		public Network AddNetwork(string name, int? status, string description, int parentId, int operatorId,
			string website, string xmlField, int largeWidth, int largeHeight, int thumbWidth,
			int thumbHeight, int mediumWidth, int mediumHeight, HttpPostedFile image)
		{
			
			if (GetNetwork(name, parentId) != null)
			{
				Errors.Add(new Exception("Another network with the same name already exists for this parent network"));
				return null;
			}

			var ParentUniqueName = GetNetwork(parentId);
			var UName = StringUtils.ToURL(ParentUniqueName != null ? string.Format("{0}-{1}", ParentUniqueName.UniqueName, name) : name);
			var ImageName = UName + Path.GetExtension(image.FileName);

			Network network = new Network
			{
				Name = name,
				Description = description,
				ParentId = parentId,
				UniqueName = UName,
				DateCreated = DateTime.Now,
				LastModified = DateTime.Now,
				OperatorId = operatorId,
				Website = website,
				XmlField = System.Xml.Linq.XElement.Parse(xmlField),
				Status = status
			};

			NetworkData.Networks.InsertOnSubmit(network);
			Save();

			if (image.ContentLength > 0)
			{
				var net = GetNetwork(name);
				int networkId = net.NetworkId;
				UpdateImage(networkId, image, false, true, network.UniqueName, largeWidth,
					largeHeight, thumbWidth, thumbHeight, mediumWidth, mediumHeight);
			}
			network.Image = ImageName;
			Save();

			return network;
		}

		public Network UpdateNetwork(int network, string description)
		{
			var n = GetNetwork(network);

			return UpdateNetwork(network, n.Name, n.Status, DateTime.Now, description, Int32.Parse(n.ParentId.ToString()), Int32.Parse(n.OperatorId.ToString()),
                n.Website.ToString(), n.XmlField.ToString(), 0, 0, 0, 0, 0, 0, true, null);
		}

		public Network UpdateNetwork(int network, string title, DateTime date, string description, int parentId, int operatorId,
			string website, string xmlField, int largeWidth, int largeHeight, int thumbWidth,
			int thumbHeight, int mediumWidth, int mediumHeight, bool DeleteImage, HttpPostedFile Image)
		{
			return UpdateNetwork(network, title, null, date, description, parentId, operatorId,
				website, xmlField, largeWidth, largeHeight, thumbWidth,
				thumbHeight, mediumWidth, mediumHeight, DeleteImage, Image);
		}

		public Network UpdateNetwork(int network, string title, int? status, DateTime date, string description, int parentId, int operatorId,
			string website, string xmlField, int largeWidth, int largeHeight, int thumbWidth,
            int thumbHeight, int mediumWidth, int mediumHeight, bool DeleteImage, HttpPostedFile Image)
		{
			var n = GetNetwork(network);
            if (xmlField == null || xmlField == "")
			{
				xmlField = "<xml />";
			}
			n.Name = title;
			n.Description = description;
			n.ParentId = parentId;
			n.OperatorId = operatorId;
			n.Website = website;
			n.XmlField = System.Xml.Linq.XElement.Parse(xmlField);
			n.LastModified = DateTime.Now;
			n.Status = status;

			NetworkData.SubmitChanges();

            string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.Networks);
            path = System.IO.Path.Combine(path, string.Format("Network{0}", n.NetworkId));

			string largePath = "";
			string mediumPath = "";
			string thumbPath = "";
			string originalPath = "";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);

				Directory.CreateDirectory(Path.Combine(path, "Large"));
				Directory.CreateDirectory(Path.Combine(path, "Thumb"));
				Directory.CreateDirectory(Path.Combine(path, "Medium"));
			}
			else
			{
				largePath = Path.Combine(path, "Large");
				if (!Directory.Exists(largePath))
					Directory.CreateDirectory(largePath);

				thumbPath = Path.Combine(path, "Thumb");
				if (!Directory.Exists(thumbPath))
					Directory.CreateDirectory(thumbPath);

				mediumPath = Path.Combine(path, "Medium");
				if (!Directory.Exists(mediumPath))
					Directory.CreateDirectory(mediumPath);

			}

            bool del = (DeleteImage || (Image != null && Image.ContentLength > 0))
                        && n.Image != null && n.Image.ToString() != "";

            bool updateData = false;
            if (del)
            {
				originalPath = Path.Combine(path, n.Image);
				largePath = Path.Combine(largePath, n.Image);
				mediumPath = Path.Combine(mediumPath, n.Image);
				thumbPath = Path.Combine(thumbPath, n.Image);

				if (File.Exists(originalPath))
					File.Delete(originalPath);

				if (File.Exists(largePath))
					File.Delete(largePath);

				if (File.Exists(mediumPath))
					File.Delete(mediumPath);

				if (File.Exists(thumbPath))
					File.Delete(thumbPath);

                n.Image = "";
                updateData = true;
            }

            if (Image != null && Image.ContentLength > 0)
            {
                Config cfg = new Config();

                string fileName = StringUtils.GetFileName(Image.FileName);

				string ImageName = string.Format("{0}\\{1}", path, fileName);
				string largeImageName = string.Format("{0}\\Large/{1}", path, fileName);
				string thumbImageName = string.Format("{0}\\Thumb/{1}", path, fileName);
				string mediumImageName = string.Format("{0}\\Medium/{1}", path, fileName);

				Image.SaveAs(ImageName);

				if (largeHeight > 0 && largeWidth > 0)
				{
					lw.GraphicUtils.ImageUtils.CropImage(ImageName, largeImageName, largeWidth, largeHeight, ImageUtils.AnchorPosition.Default);
				}
				if (thumbHeight > 0 && thumbWidth > 0)
				{
					lw.GraphicUtils.ImageUtils.CropImage(ImageName, thumbImageName, thumbWidth, thumbHeight, ImageUtils.AnchorPosition.Default);
				}
				if (mediumHeight > 0 && mediumWidth > 0)
				{
					lw.GraphicUtils.ImageUtils.CropImage(ImageName, mediumImageName, mediumWidth, mediumHeight, ImageUtils.AnchorPosition.Default);
				}

                if (cfg.GetKey("NetworkImage") == "on")
                {
                    try
                    {
                        int _Width = Int32.Parse(cfg.GetKey("NetworkImageWidth"));
                        int _Height = Int32.Parse(cfg.GetKey("NetworkImageHeight"));

                        //	ImageUtils.Resize(ImageName, ImageName, _Width, _Height);
						lw.GraphicUtils.ImageUtils.CreateThumb(ImageName, largeImageName, _Width, _Height);

                        int Width = Int32.Parse(cfg.GetKey("NetworkImageThumbWidth"));
                        int Height = Int32.Parse(cfg.GetKey("NetworkImageThumbHeight"));
                        lw.GraphicUtils.ImageUtils.CropImage(ImageName, thumbImageName, Width, Height, ImageUtils.AnchorPosition.Default);
                        //	lw.GraphicUtils.ImageUtils.Resize(ImageName, thumbImageName, Width, Height);
						//lw.GraphicUtils.ImageUtils.CreateThumb(ImageName, thumbImageName, Width, Height);
						int MWidth = Int32.Parse(cfg.GetKey("NetworkImageMediumWidth"));
						int MHeight = Int32.Parse(cfg.GetKey("NetworkImageMediumHeight"));
						lw.GraphicUtils.ImageUtils.CropImage(ImageName, mediumImageName, MWidth, MHeight, ImageUtils.AnchorPosition.Default);

                    }
                    catch (Exception ex)
                    {
                        ErrorContext.Add("resize-image", "Unable to resize album image.<br><span class=hid>" + ex.Message + "</span>");
                    }
                }

                n.Image = fileName;
                updateData = true;
            }

            if (updateData)
                NetworkData.SubmitChanges();

			return n;
		}

		public Network UpdateNetworkXml(int network, string xmlField)
		{
			var n = GetNetwork(network);
			
			n.XmlField = System.Xml.Linq.XElement.Parse(xmlField);
			n.LastModified = DateTime.Now;
			
			NetworkData.SubmitChanges();

			return n;
		}
		public Network UpdateNetworkName(int network, string title)
		{
			var n = GetNetwork(network);

			n.Name = title;
			NetworkData.SubmitChanges();

			return n;
		}

		/// <summary>
		/// Removes the network
		/// </summary>
		/// <param name="networkId"></param>
		public void RemoveNetwork(int networkId)
		{
			string path = Folders.Networks;
			path = Path.Combine(path, string.Format("Network{0}", networkId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);
			var q = from rel in NetworkData.Networks
					where rel.NetworkId == networkId
					select rel;
			if (q.Count() == 0)
			{
				Errors.Add(new Exception("Network already deleted"));
				return;
			}
			if (Directory.Exists(path))
			{
				try
				{
					Directory.Delete(path, true);
				}
				catch (Exception ex)
				{
					lw.Error.Handler.HandleError(ex);
					if (Directory.Exists(path))
						Directory.Delete(path, true);
				}
			}

			NetworkData.Networks.DeleteOnSubmit(q.First());
			Save();
		}

		/// <summary>
		/// Default Image sizes is 0.
		/// </summary>>
		public void UpdateImage(int networkId, Stream BytesStream, string fileName, bool autoResize,
			bool deleteOld, int largeWidth, int largeHeight, int thumbWidth, int thumbHeight,
			int mediumWidth, int mediumHeight)
		{
			try
			{
				if (BytesStream.Length != 0)
				{
					string path = WebContext.Server.MapPath("~/temp");
					path = Path.Combine(path, fileName);

					lw.Utils.IO.SaveStream(BytesStream, path);

					UpdateImage(networkId, path, false, true, largeWidth, largeHeight,
						thumbWidth, thumbHeight, mediumWidth, mediumHeight);
				}
			}
			catch (Exception ex)
			{
				ErrorContext.Add("add-image", "Unable to add image.<br><span class=hid>" + ex.Message + "</span>");
			}
		}

		public void UpdateImage(int networkId, HttpPostedFile defaultImage,
			bool autoResize, bool deleteOld, string uniqueName, int largeWidth,
			int largeHeight, int thumbWidth, int thumbHeight, int mediumWidth, int mediumHeight)
		{
			string path = Folders.Networks; 
			path = Path.Combine(path, string.Format("Network{0}", networkId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (!string.IsNullOrEmpty(defaultImage.FileName))
			{
				path = Path.Combine(path, uniqueName + Path.GetExtension(defaultImage.FileName));
				defaultImage.SaveAs(path);
			}

			if (deleteOld && defaultImage.ContentLength == 0)
				path = "";

			UpdateImage(networkId, path, autoResize, deleteOld, largeWidth,
				largeHeight, thumbWidth, thumbHeight, mediumWidth, mediumHeight);
		}


		public void UpdateImage(int networkId, string networkImage,
			bool autoResize, bool deleteOld, int largeWidth, int largeHeight,
			int thumbWidth, int thumbHeight, int mediumWidth, int  mediumHeight)
		{
			NetworksManager nMgr = new NetworksManager();
			Networking.Network networkDetail = nMgr.GetNetwork(networkId);

			string imageName = networkDetail.Image;

			string path = Folders.Networks;
			path = Path.Combine(path, string.Format("Network{0}", networkId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			deleteOld = deleteOld || !String.IsNullOrEmpty(networkImage);

			if (deleteOld && !StringUtils.IsNullOrWhiteSpace(networkDetail.Image))
			{
				string largePath = Path.Combine(path, "Large");
				string mediumPath = Path.Combine(path, "Medium");
				string thumbPath = Path.Combine(path, "Thumb");

				if (File.Exists(Path.Combine(path, networkDetail.Image)))
				{
					File.Delete(Path.Combine(path, networkDetail.Image));
				}
				if (File.Exists(Path.Combine(largePath, networkDetail.Image)))
				{
					File.Delete(Path.Combine(largePath, networkDetail.Image));
				}
				if (File.Exists(Path.Combine(mediumPath, networkDetail.Image)))
				{
					File.Delete(Path.Combine(mediumPath, networkDetail.Image));
				}
				if (File.Exists(Path.Combine(thumbPath, networkDetail.Image)))
				{
					File.Delete(Path.Combine(thumbPath, networkDetail.Image));
				}

				networkDetail.Image = "";
			}
			if (File.Exists(networkImage))
			{
				string Name = networkDetail.Image;
				string ImageName = Path.GetFileName(networkImage);

				string largePath = Path.Combine(path, "Large");
				if (!Directory.Exists(largePath))
					Directory.CreateDirectory(largePath);

				string thumbPath = Path.Combine(path, "Thumb");
				if (!Directory.Exists(thumbPath))
					Directory.CreateDirectory(thumbPath);

				string mediumPath = Path.Combine(path, "Medium");
				if (!Directory.Exists(mediumPath))
					Directory.CreateDirectory(mediumPath);


				largePath = string.Format("{0}\\Large/{1}", path, ImageName);
				thumbPath = string.Format("{0}\\Thumb/{1}", path, ImageName);
				mediumPath = string.Format("{0}\\Medium/{1}", path, ImageName);

				File.Copy(networkImage, largePath);
				//File.Delete(networkImage);
				
				// Thumb get Croped for cases that use precise dimensions (Width AND Height)
				// Medium Resized depending on the the Width or the Height
				// Large Resize only if AutoResize Checked

				if (thumbHeight > 0 || thumbWidth > 0)
					ImageUtils.CropImage(largePath, thumbPath, thumbWidth, thumbHeight, ImageUtils.AnchorPosition.Default);

				if (mediumHeight > 0 || mediumHeight > 0)
					ImageUtils.CropImage(largePath, mediumPath, mediumWidth, mediumHeight, ImageUtils.AnchorPosition.Default);

				if (largeWidth > 0 || largeHeight > 0 && autoResize)
					ImageUtils.Resize(largePath, largePath, largeWidth, largeHeight);

				networkDetail.Image = imageName;
			}

			nMgr.Save();
		}

		#endregion

		#region Variables


		public SocialNetworksDataContextDataContext NetworkData
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new SocialNetworksDataContextDataContext(Connection);
				return (SocialNetworksDataContextDataContext)_dataContext;
			}
		}

		#endregion
	}
}
