using System;
using System.Data;
using System.Text;
using System.Web.UI;
using lw.Base;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;

namespace lw.Testimonials.Controls
{
	public class TestimonialsDataSource : CustomDataSource
	{
		bool _bound = false;
		TestimonialStatus? status = null;
		string type = null;
		TestimonialsSort _Sort = TestimonialsSort.IdDesc;
		int? max = null;

		public TestimonialsDataSource()
		{
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			BuildQuery();


			base.DataBind();
		}


		/// <summary>
		/// Builds the select command for the data source
		/// </summary>
		public void BuildQuery()
		{
			this.ConfigFile = cte.ConfigFile;
		}

		public override string OrderBy
		{
			get
			{
				return EnumHelper.GetDescription(Sort);
			}
		}

		DataTable data;
		public override object Data
		{
			get
			{
				data = base.Data as DataTable;
				string cond = "";
				TestimonialsManager tMgr = new TestimonialsManager();
				DataView dv;
				
				if (status == null)
				{
					if (CMSMode == false)
					{
						cond += " And Display = " + (int)TestimonialStatus.MainPage;
					}
				}else if (!String.IsNullOrWhiteSpace(status.ToString()))
				{
					cond += " And Display = " + (int)status;
				}
				if (type != null)
				{
					dv = tMgr.GetTestimonialGroups("GroupName = '" + type + "'");
					cond += " And GroupId = " + Int32.Parse(dv[0]["GroupId"].ToString());
				}
				else
				{
					if (CMSMode == false)
					{
						dv = tMgr.GetTestimonialGroups("GroupName = 'General'");
						cond += " And GroupId = " + Int32.Parse(dv[0]["GroupId"].ToString());
					}
				}

				if (CMSMode != null && CMSMode.Value)
				{
					string q = WebContext.Request["q"];

					if (!string.IsNullOrWhiteSpace(q))
						cond += string.Format(" and (Text like '%{0}%' or From like '%{0}%')", StringUtils.SQLEncode(q));
				}

				if (cond.Length > 0)
				{
					cond = cond.ToString().Substring(5);
				}

				dv = new DataView(data, cond, "", DataViewRowState.CurrentRows);
				dv.Sort = this.OrderBy;

				if (max != null)
				{
					dv = GetTopDataViewRows(dv, Int32.Parse(max.ToString()));
				}
				return dv;
			}
			set
			{
				base.Data = value;
			}
		}
		private DataView GetTopDataViewRows(DataView dv, Int32 n)
		{
			DataTable dt = dv.Table.Clone();

			for (int i = 0; i < n; i++)
			{
				if (i >= dv.Count)
				{
					break;
				}
				dt.ImportRow(dv[i].Row);
			}
			return new DataView(dt, dv.RowFilter, dv.Sort, dv.RowStateFilter);
		}

		bool _hasData;
		public override bool HasData
		{
			get {
				return _hasData;
			}
		}


		#region Properties


		string _customCondition = "";
		public string CustomCondition
		{
			get { return _customCondition; }
			set { _customCondition = value; }
		}

		public string Type
		{
			get
			{
				return type;
			}
			set { type = value; }
		}


		public TestimonialStatus? Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value;
			}
		}

		public TestimonialsSort Sort
		{
			get { return _Sort; }
			set { _Sort = value; }
		}

		public int? Max
		{
			get
			{
				return max;
			}
			set
			{
				max = value;
			}
		}
        bool? _CMSMode = false;
        /// <summary>
        /// Indicates if the tag is used inside the CMS
        /// Can also be set in web.config.
        /// </summary>
        public bool? CMSMode
        {
            get
            {
                if (_CMSMode == null)
                {
                    string val = Config.GetFromWebConfig(lw.CTE.parameters.CMSMode);
                    _CMSMode = !String.IsNullOrEmpty(val) && bool.Parse(val.Trim());
                }
                return _CMSMode;
            }
            set
            {
                _CMSMode = value;
            }
        }
		#endregion
	}
}