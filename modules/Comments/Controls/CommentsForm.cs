using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

using lw.Comments;
using lw.WebTools;
using lw.Utils;
using lw.Network;
using lw.Content;
using lw.Forms.Controls;
using lw.Base;

using lw.Members;



namespace lw.Comments.Controls
{
	class resp
	{
		public string CommentId;
		public string ParentId;
		public string RelationId;
		public string DateCreated;
		public string Title;
		public string CommentText;
		public string MemberId;
		public string FirstName;
		public string LastName;
		public string Username;
		public string Picture;
		public string TableName;
	}

	public class CommentsForm : Form
	{
		public bool _bound = false;
		int? _relationId = null;
		string _tableName = "";
		bool _membersOnly = false;
		string _relateTo = "ID";
		bool _noRelations = false;
		bool _isReply = false;
		CommentsDataSource dataSrc = null;

		public CommentsForm()
		{
		}

		public override void DataBind()
		{

			dataSrc = this.Parent as CommentsDataSource;

			if (!_bound)
			{
				_bound = true;

				CommentsManager cMgr = new CommentsManager();

				Comments.Comments_Tables_View table = cMgr.GetTableDetails(TableName);
				if (table == null)
				{
					CommentsSetup cs = new CommentsSetup();
					if (MembersOnly)
						cs.CreateCommentsTableWithMembers(TableName, RelateTo, 1);
					else
						cs.CreateCommentsTableNoMembers(TableName, RelateTo, 1);

					table = cMgr.GetTableDetails(TableName);
				}

				//HtmlInputHidden RelationId = new HtmlInputHidden();
				//RelationId.ID = "RelationId";
				//RelationId.Value = "-2";
				//this.Controls.Add(RelationId);

				HtmlInputHidden ParentId = new HtmlInputHidden();
				ParentId.ID = "ParentId";
				if (this.dataSrc == null)
					ParentId.Value = "-1";
				else
					ParentId.Value = dataSrc.ParentId.ToString();
				this.Controls.Add(ParentId);


				if (!NoRelations)
				{
					object relationValue = ControlUtils.GetBoundedDataField(this.NamingContainer, table.RelationField);

					RelationId = (int)relationValue;
				}
				if (AjaxSubmit)
				{
					AjaxResponse resp = new AjaxResponse();

					if (this.IsPostBack)
					{
						resp.callBack = this.AjaxCallback;
						this.Validate();
						resp.valid = this.IsValid.Value;

						if (resp.valid)
						{
							if (this.MembersOnly)
							{
								string text = WebContext.Request["CommentText"];
								if (!String.IsNullOrWhiteSpace(text))
								{
									int comment = cMgr.AddMemberComment(TableName,
										Int32.Parse(ParentId.Value), RelationId,
										WebContext.Request["CommentTitle"],
										text, WebContext.Profile.UserId, CommentType.Text);

									var dr = cMgr.GetTopComment(TableName, RelationId, Int32.Parse(ParentId.Value), "Order By CommentId Desc");

									MembersManager mMgr = new MembersManager();

									resp r = new resp();

									string mId = dr["MemberId"].ToString();

									if (!String.IsNullOrWhiteSpace(mId))
									{
										var member = mMgr.GetMember(Int32.Parse(mId));

										r.FirstName = member.FirstName;
										r.LastName = member.LastName;
										r.Username = member.UserName;
										r.Picture = member.Picture;
									}
									
									r.CommentId = dr["CommentId"].ToString();
									r.ParentId = dr["ParentId"].ToString();
									r.RelationId = dr["RelationId"].ToString();
									r.DateCreated = string.Format("{0:MMM dd, yyyy} at {0:h:mm:tt}", dr["DateCreated"]);
									r.Title = dr["Title"].ToString();
									r.CommentText = dr["CommentText"].ToString();
									r.MemberId = dr["MemberId"].ToString();
									r.TableName = TableName;

									resp.data = r;
									resp.message = "Text successfully added";
									resp.error = null;
								}
								else
								{
									resp.message = "Please write your text before submitting and try again.";
									resp.error = "true";
								}
							}
							else
							{
								NameValueCollection post = GetValues();
								ContentManager pMgr = new ContentManager();

								CustomPage thisPage = this.Page as CustomPage;
								if (thisPage == null)
									thisPage = new CustomPage();

								if (String.IsNullOrEmpty(post["Name"]))
								{
									Validated = false;
									ErrorContext.Add("validation-name", ContentManager.ErrorMsg("Name-Validation", thisPage.Language));
								}
								if (String.IsNullOrEmpty(post["Email"]) || !Validation.IsEmail(post["Email"]))
								{
									Validated = false;
									ErrorContext.Add("validation-email", ContentManager.ErrorMsg("Email-Validation", thisPage.Language));
								}
								if (String.IsNullOrEmpty(post["Title"]))
								{
									Validated = false;
									ErrorContext.Add("validation-comment", ContentManager.ErrorMsg("Comment-Validation", thisPage.Language));
								}
								if (String.IsNullOrEmpty(post["Comment"]))
								{
									Validated = false;
									ErrorContext.Add("validation-comment", ContentManager.ErrorMsg("Comment-Validation", thisPage.Language));
								}
								if (Validated != null && Validated.Value)
								{
									int commentId = -1;
									try
									{
										commentId = cMgr.AddNoMemberComment(TableName, -1,
											RelationId,
											post["Title"],
											post["Comment"], WebContext.Profile.UserGuid,
											post["Name"], post["Website"],
											post["Country"], WebContext.Request.ServerVariables["REMOTE_ADDR"],
											post["avatar"], CommentType.Text);
									}
									catch (Exception ex)
									{
										ErrorContext.Add(ex);
									}
									if (commentId > 0)
									{
										if (!String.IsNullOrEmpty(table.Email))
										{
											Config cfg = new Config();
											Mail mail = new Mail("Comments");
											mail.Subject = post["Name"] + " submitted a comment to " + table.TableName;

											NameValueCollection dic = new NameValueCollection();
											dic["Name"] = post["Name"];
											dic["Email"] = post["Email"];
											dic["Comment"] = StringUtils.PutBR(post["Comment"].ToString());

											if (!String.IsNullOrWhiteSpace(post["Website"]))
												dic["Website"] = post["Website"];

											ArrayList files = cMgr.AddFiles(TableName, commentId);

											StringBuilder sb = new StringBuilder();

											if (files.Count > 0)
											{
												sb.Append("<dl><dt>Files:</dt><dd>");
												foreach (string s in files)
												{
													string temp = s;
													if (temp.IndexOf("~") == 0)
														temp = temp.Substring(1);

													sb.Append(String.Format("<a href=\"http://{0}/{1}\">{2}</a>",
														WebTools.WebContext.ServerName,
														s.Replace("\\", "/").Substring(1),
														Path.GetFileName(temp)));
													sb.Append("<br />");
												}
												sb.Append("</dd></dl>");
											}

											dic["Files"] = sb.ToString();
											sb.Clear();

											sb.Append(string.Format(@"<p>
	<a href=""http://{0}/Prv/proxies/comments/status.ashx?commentId={1}&table={2}&action=approve"">Approve</a>
	- <a href=""http://{0}/Prv/proxies/comments/status.ashx?commentId={1}&table={2}&action=edit"">Edit</a>
	- <a href=""http://{0}/Prv/proxies/comments/status.ashx?CommentId={1}&table={2}&action=delete"">Delete</a>
</p>", WebTools.WebContext.ServerName, commentId, table.TableName));

											dic["Permission"] = sb.ToString();

											mail.Data = dic;
											mail.To = table.Email;

											/*
											mail.From = cfg.GetKey("EmailsFrom");

											StringBuilder sb = new StringBuilder();

											sb.Append("<dl>");
											sb.Append(String.Format("<dt>Name: </dt><dd>{0}</dd>", post["Name"]));
											sb.Append(String.Format("<dt>Email: </dt><dd>{0}</dd>", post["Email"]));

											if(!String.IsNullOrWhiteSpace(post["Website"]))
												sb.Append(String.Format("<dt>Website: </dt><dd>{0}</dd>", post["Website"]));


											ArrayList files = cMgr.AddFiles(TableName, commentId);

											if (files.Count > 0)
											{
												sb.Append("<dt>Files:</dt><dd>");
												foreach (string s in files)
												{
													String.Format("<a href=\"{0}/{1}\">{2}</a>",
														WebTools.WebContext.ServerName,
														s.Replace("\\", "/"),
														Path.GetFileName(s));
												}
												sb.Append("</dd>");
											}

											sb.Append(String.Format("<dt>Comment: </dt><dd>{0}</dd>", StringUtils.PutBR(post["Comment"])));
											sb.Append("</dl>");

											sb.Append(string.Format(@"<p>
											<a href=""http://{0}/Prv/proxies/comments/status.ashx?commentId={1}&table={2}&action=approve"">Approve</a>
											- <a href=""http://{0}/Prv/proxies/comments/status.ashx?commentId={1}&table={2}&action=edit"">Edit</a>
											- <a href=""http://{0}/Prv/proxies/comments/status.ashx?CommentId={1}&table={2}&action=delete"">Delete</a>
										</p>", WebTools.WebContext.ServerName, commentId, table.TableName));

											mail.Body = sb.ToString();
											mail.Subject = post["Name"] + " submitted a comment to " + table.TableName;
											*/
											mail.Send();
										}
										SuccessContext.Add("comment-submitted", ContentManager.Message("comment-submitted"));
										this.Visible = false;
									}
								}
							}
						}
						resp.WriteJson(true);
					}
				}
				else
				{
					if (this.MembersOnly)
					{
						string text = WebContext.Request["CommentText"];
						if (!String.IsNullOrWhiteSpace(text))
						{
							cMgr.AddMemberComment(TableName,
								Int32.Parse(ParentId.Value), RelationId,
								WebContext.Request["CommentTitle"],
								text, WebContext.Profile.UserId, CommentType.Text);

							WebContext.Response.Redirect(WebContext.Request.RawUrl);
						}
					}
					else
					{
						NameValueCollection post = GetValues();
						ContentManager pMgr = new ContentManager();

						CustomPage thisPage = this.Page as CustomPage;
						if (thisPage == null)
							thisPage = new CustomPage();

						if (String.IsNullOrEmpty(post["Name"]))
						{
							Validated = false;
							ErrorContext.Add("validation-name", ContentManager.ErrorMsg("Name-Validation", thisPage.Language));
						}
						if (String.IsNullOrEmpty(post["Email"]) || !Validation.IsEmail(post["Email"]))
						{
							Validated = false;
							ErrorContext.Add("validation-email", ContentManager.ErrorMsg("Email-Validation", thisPage.Language));
						}
						if (String.IsNullOrEmpty(post["Title"]))
						{
							Validated = false;
							ErrorContext.Add("validation-comment", ContentManager.ErrorMsg("Comment-Validation", thisPage.Language));
						}
						if (String.IsNullOrEmpty(post["Comment"]))
						{
							Validated = false;
							ErrorContext.Add("validation-comment", ContentManager.ErrorMsg("Comment-Validation", thisPage.Language));
						}
						if (Validated != null && Validated.Value)
						{
							int commentId = -1;
							try
							{
								commentId = cMgr.AddNoMemberComment(TableName, -1,
									RelationId,
									post["Title"],
									post["Comment"], WebContext.Profile.UserGuid,
									post["Name"], post["Website"],
									post["Country"], WebContext.Request.ServerVariables["REMOTE_ADDR"],
									post["avatar"], CommentType.Text);
							}
							catch (Exception ex)
							{
								ErrorContext.Add(ex);
							}
							if (commentId > 0)
							{
								if (!String.IsNullOrEmpty(table.Email))
								{
									Config cfg = new Config();
									Mail mail = new Mail("Comments");
									mail.Subject = post["Name"] + " submitted a comment to " + table.TableName;

									NameValueCollection dic = new NameValueCollection();
									dic["Name"] = post["Name"];
									dic["Email"] = post["Email"];
									dic["Comment"] = StringUtils.PutBR(post["Comment"].ToString());

									if (!String.IsNullOrWhiteSpace(post["Website"]))
										dic["Website"] = post["Website"];

									ArrayList files = cMgr.AddFiles(TableName, commentId);

									StringBuilder sb = new StringBuilder();

									if (files.Count > 0)
									{
										sb.Append("<dl><dt>Files:</dt><dd>");
										foreach (string s in files)
										{
											string temp = s;
											if (temp.IndexOf("~") == 0)
												temp = temp.Substring(1);

											sb.Append(String.Format("<a href=\"http://{0}/{1}\">{2}</a>",
												WebTools.WebContext.ServerName,
												s.Replace("\\", "/").Substring(1),
												Path.GetFileName(temp)));
											sb.Append("<br />");
										}
										sb.Append("</dd></dl>");
									}

									dic["Files"] = sb.ToString();
									sb.Clear();

									sb.Append(string.Format(@"<p>
								<a href=""http://{0}/Prv/proxies/comments/status.ashx?commentId={1}&table={2}&action=approve"">Approve</a>
								- <a href=""http://{0}/Prv/proxies/comments/status.ashx?commentId={1}&table={2}&action=edit"">Edit</a>
								- <a href=""http://{0}/Prv/proxies/comments/status.ashx?CommentId={1}&table={2}&action=delete"">Delete</a>
							</p>", WebTools.WebContext.ServerName, commentId, table.TableName));

									dic["Permission"] = sb.ToString();

									mail.Data = dic;
									mail.To = table.Email;

									/*
									mail.From = cfg.GetKey("EmailsFrom");

									StringBuilder sb = new StringBuilder();

									sb.Append("<dl>");
									sb.Append(String.Format("<dt>Name: </dt><dd>{0}</dd>", post["Name"]));
									sb.Append(String.Format("<dt>Email: </dt><dd>{0}</dd>", post["Email"]));

									if(!String.IsNullOrWhiteSpace(post["Website"]))
										sb.Append(String.Format("<dt>Website: </dt><dd>{0}</dd>", post["Website"]));


									ArrayList files = cMgr.AddFiles(TableName, commentId);

									if (files.Count > 0)
									{
										sb.Append("<dt>Files:</dt><dd>");
										foreach (string s in files)
										{
											String.Format("<a href=\"{0}/{1}\">{2}</a>",
												WebTools.WebContext.ServerName,
												s.Replace("\\", "/"),
												Path.GetFileName(s));
										}
										sb.Append("</dd>");
									}

									sb.Append(String.Format("<dt>Comment: </dt><dd>{0}</dd>", StringUtils.PutBR(post["Comment"])));
									sb.Append("</dl>");

									sb.Append(string.Format(@"<p>
									<a href=""http://{0}/Prv/proxies/comments/status.ashx?commentId={1}&table={2}&action=approve"">Approve</a>
									- <a href=""http://{0}/Prv/proxies/comments/status.ashx?commentId={1}&table={2}&action=edit"">Edit</a>
									- <a href=""http://{0}/Prv/proxies/comments/status.ashx?CommentId={1}&table={2}&action=delete"">Delete</a>
								</p>", WebTools.WebContext.ServerName, commentId, table.TableName));

									mail.Body = sb.ToString();
									mail.Subject = post["Name"] + " submitted a comment to " + table.TableName;
									*/
									mail.Send();
								}
								SuccessContext.Add("comment-submitted", ContentManager.Message("comment-submitted"));
								this.Visible = false;
							}
						}
					}
				}
			}
			base.DataBind();
		}

		public string TableName
		{
			get { return _tableName; }
			set { _tableName = value; }
		}
		public int? RelationId
		{
			get
			{
				if (_relationId == null)
				{
					_relationId = (int)ControlUtils.GetBoundedDataField(this.NamingContainer, RelateTo);
				}
				return _relationId;
			}
			set
			{
				_relationId = value;
			}
		}
		public bool MembersOnly
		{
			get { return _membersOnly; }
			set { _membersOnly = value; }
		}
		public string RelateTo
		{
			get { return _relateTo; }
			set { _relateTo = value; }
		}
		public bool NoRelations
		{
			get { return _noRelations; }
			set { _noRelations = value; }
		}

		public bool IsReply
		{
			get
			{
				return _isReply;
			}
			set
			{
				_isReply = value;
			}
		}
	}
}

