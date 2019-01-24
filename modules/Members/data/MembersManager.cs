using System;
using System.Data;
using System.Collections;
using System.IO;

using lw.Data;
using lw.Utils;
using lw.CTE;
using lw.CTE.Enum;
using lw.WebTools;
using lw.Content;
using lw.Network;
using lw.Members.Security;

namespace lw.Members
{
    public class MembersManager : DirectorBase
    {
        public MembersManager()
            : base(cte.lib)
        {
        }
        public DataTable GetMembersWithProfile(string cond)
        {
            cond = cond != "" ? " where " + cond : "";
            return DBUtils.GetDataSet("select * from MemberView" + cond, cte.lib).Tables[0];
        }
        public DataTable GetMemberByID(int id)
        {
            return DBUtils.GetDataSet("select * from Members where memberid=" + id.ToString(), cte.lib).Tables[0];
        }

        public DataRow GetMemberProfile(int MemberId)
        {
            DataTable dt = GetMembersWithProfile(string.Format("MemberId={0}", MemberId));
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
        public DataRow GetMemberProfile(string UserName)
        {
            DataTable dt = GetMembersWithProfile(string.Format("UserName='{0}'", StringUtils.SQLEncode(UserName)));
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
        public MembersDs.MembersDataTable GetMembers(string cond)
        {
            MembersAdp adp = new MembersAdp();
            return adp.GetDataCondition(cond);
        }
        public MembersDs.MembersRow GetMember(int MemberId)
        {
            DataTable dt = GetMembers(string.Format("MemberId={0}", MemberId));
            return dt.Rows.Count > 0 ? (MembersDs.MembersRow)dt.Rows[0] : null;
        }
        public MembersDs.MembersRow GetMember(string UserName)
        {
            DataTable dt = GetMembers(string.Format("userName='{0}' or Email='{0}'", StringUtils.SQLEncode(UserName)));
            return dt.Rows.Count > 0 ? (MembersDs.MembersRow)dt.Rows[0] : null;
        }
        public MembersDs.MembersRow GetMemberByGuId(string GuId)
        {
            DataTable dt = GetMembers(string.Format("GeuId='{0}'", StringUtils.SQLEncode(GuId)));
            return dt.Rows.Count > 0 ? (MembersDs.MembersRow)dt.Rows[0] : null;
        }
        public MembersDs.MembersRow AddMember(string userName,
            string email, string AlternateEmail,
            string password,
            string SecretQuestion, string SecretAnswer,
            string title,
            string firstName, string middleName, string lastName,
            Gender? gender, DateTime? dateOfBirth, int? status, int Privacy,
            string Comments, string PrivateComments, int? ChangedBy, bool? JoinNewsletter)
        {
            return AddMember(userName, email, AlternateEmail, password, SecretQuestion, SecretAnswer, title, firstName,
                middleName, lastName, gender, dateOfBirth, status, Privacy, Comments, PrivateComments, ChangedBy, JoinNewsletter, "", "");
        }
        public MembersDs.MembersRow AddMember(string userName,
            string email, string AlternateEmail,
            string password,
            string SecretQuestion, string SecretAnswer,
            string title,
            string firstName, string middleName, string lastName,
            Gender? gender, DateTime? dateOfBirth, int? status, int Privacy,
            string Comments, string PrivateComments, int? ChangedBy, bool? JoinNewsletter, string FullName, string NativeFullName)
        {
            string cond = "";
            if (email == "noemail@sabis.net")
            {
                cond = string.Format("Username='{0}'",
                    StringUtils.SQLEncode(userName));
            }
            else
            {
                cond = string.Format("UserName='{0}' or Email='{1}'",
                    StringUtils.SQLEncode(userName),
                    StringUtils.SQLEncode(email));
            }

            if (GetMembers(cond).Rows.Count > 0)
            {
                if (email != null && email.Trim() != "")
                {
                    ErrorContext.Add("validation", ContentManager.ErrorMsg(Errors.UserAlreadyExists));
                    return null;
                }
            }

            MembersDsTableAdapters.MembersTableAdapter adp = new lw.Members.MembersDsTableAdapters.MembersTableAdapter();
            MembersDs.MembersDataTable dt = adp.AddMember(
                System.Guid.NewGuid(),
                userName, EncryptPassword(password), status,
                title,
                firstName, middleName, lastName, (short?)gender, email, AlternateEmail,
                dateOfBirth,
                DateTime.Now, DateTime.Now, DateTime.Now, false, SecretQuestion,
                SecretAnswer, ChangedBy, JoinNewsletter, Privacy, FullName, NativeFullName);
            return (MembersDs.MembersRow)dt.Rows[0];
        }

        public int UpdateMember(int MemberId, string userName,
                     string password,
                     string email, string AlternateEmail,
                     string SecretQuestion, string SecretAnswer,
            string title,
            string firstName,
            string middleName,
                     string lastName, Gender? gender,
                    DateTime? dateOfBirth,
                    int? status, int? ChangedBy,
                     bool? JoinNewsletter,
                     string PrivateComments, string Comments)
        {
            return UpdateMember(MemberId, userName, password, email, AlternateEmail, SecretQuestion, SecretAnswer, title, firstName, middleName, lastName, gender, dateOfBirth, status, ChangedBy, JoinNewsletter, PrivateComments, Comments, null);
        }

        public int UpdateMember(int MemberId, string userName,
                     string password,
                     string email, string AlternateEmail,
                     string SecretQuestion, string SecretAnswer,
            string title,
            string firstName,
            string middleName,
                     string lastName, Gender? gender,
                    DateTime? dateOfBirth,
                    int? status, int? ChangedBy,
                     bool? JoinNewsletter,
                     string PrivateComments, string Comments, string FullName)
        {
            /* or Email='{1}' */
            string cond = string.Format("(UserName='{0}' or Email='{1}') and MemberId<>{2}",
                  StringUtils.SQLEncode(userName),
                  StringUtils.SQLEncode(email),
                  MemberId);
            if (GetMembers(cond).Rows.Count > 0 && email != "")
            {
                ErrorContext.Add("validation", ContentManager.ErrorMsg(Errors.UserAlreadyExists));
                return -1;
            }
            MembersDsTableAdapters.MembersTableAdapter adp = new lw.Members.MembersDsTableAdapters.MembersTableAdapter();

            MembersDs.MembersRow member = (MembersDs.MembersRow)this.GetMember(MemberId);

            member.UserName = userName;
            member.Email = email;

            if (!string.IsNullOrEmpty(password))
                member.Password = EncryptPassword(password);

            if (AlternateEmail != null)
                member.AlternateEmail = AlternateEmail;

            if (SecretQuestion != null)
                member.SecretQuestion = SecretQuestion;

            if (SecretAnswer != null)
                member.SecretQuestionAnswer = SecretAnswer;

            if (title != null)
                member.Title = title;

            if (firstName != null)
                member.FirstName = firstName;

            if (lastName != null)
                member.LastName = lastName;

            if (gender != null)
                member.Gender = (short)gender;

            if (dateOfBirth != null)
                member.DateOfBirth = dateOfBirth.Value;

            if (status != null)
                member.Status = status.Value;

            if (JoinNewsletter != null)
                member.JoinNewsletter = JoinNewsletter.Value;

            if (ChangedBy != null)
                member.ChangedBy = ChangedBy.Value;

            if (PrivateComments != null)
                member.PrivateComments = PrivateComments;

            if (Comments != null)
                member.Comments = Comments;

            if (middleName != null)
                member.MiddleName = middleName;

            member.LastModified = DateTime.Now;

            return adp.Update(member);
        }


        /*
		public int UpdateMember(int MemberId, string userName,
	string password,
	string email,
	string SecretQuestion, string SecretAnswer, string firstName,
	string lastName, UserStatus stat, int ChangedBy)
		{
			return UpdateMember(MemberId, userName, password, email, SecretQuestion, SecretAnswer, firstName, lastName, stat, ChangedBy);
		}
		*/
        public void DeleteMember(int MemberId)
        {
            lw.Members.MembersDsTableAdapters.MembersTableAdapter t = new lw.Members.MembersDsTableAdapters.MembersTableAdapter();
            t.DeleteMember(MemberId);
        }

        public void GenerateUniqueUserName(int MemberId)
        {
            GenerateUniqueUserName(GetMember(MemberId));
        }

        public void GenerateUniqueUserName(MembersDs.MembersRow member)
        {
            string sql = String.Format("Update Members set UserName='{0}' where MemberId={1}",
                StringUtils.ToURL(String.Format("{0}-{1}-{2}", member.FirstName, member.LastName, member.MemberId)),
                member.MemberId);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public void ResizeMemberPicture(DataRow member, string picture)
        {
            int MemberId = (int)member["MemberId"];
            string imagesPath = Path.Combine(WebContext.Server.MapPath("~"), lw.CTE.MembersSettings.MemberPicturesFolder);



            if (picture != null)
            {
                string _path = Path.Combine(imagesPath, member["UserName"].ToString());

                string path = Path.Combine(_path, picture);



                if (File.Exists(path))
                {

                    string original = picture.ToLower().Replace(".jpg", "-o.jpg");
                    string db = picture.Replace(".Jpg", ".jpg");
                    File.Copy(path, Path.Combine(_path, original));
                    File.Delete(path);
                    File.Copy(Path.Combine(_path, original), Path.Combine(_path, db));
                    File.Delete(Path.Combine(_path, original));

                    string mpath = Path.Combine(Path.Combine(imagesPath, member["UserName"].ToString()), member["Picture"].ToString().ToLower().Replace(".jpg", "-m.Jpg"));
                    if (File.Exists(mpath))
                        File.Delete(mpath);
                    string spath = Path.Combine(Path.Combine(imagesPath, member["UserName"].ToString()), member["Picture"].ToString().ToLower().Replace(".jpg", "-s.Jpg"));
                    if (File.Exists(spath))
                        File.Delete(spath);

                    if (File.Exists(path))
                    {
                        lw.GraphicUtils.ImageUtils.Resize(path, path.Replace(".Jpg", ".jpg"), MembersSettings.ProfilePictureWidth, MembersSettings.ProfilePictureHeight);
                    }

                    lw.GraphicUtils.ImageUtils.Resize(path, path.ToLower().Replace(".jpg", "-m.jpg"),
                        MembersSettings.MProfilePictureWidth, MembersSettings.MProfilePictureHeight);
                    lw.GraphicUtils.ImageUtils.CropImage(path, path.ToLower().Replace(".jpg", "-s.jpg"),
                        MembersSettings.SProfilePictureWidth, MembersSettings.SProfilePictureHeight, GraphicUtils.ImageUtils.AnchorPosition.Default);
                }

                string sql = string.Format("Update Members set Picture = '{1}' where MemberId={0}",
                MemberId, picture.ToLower());
                DBUtils.ExecuteQuery(sql, cte.lib);
            }


        }


        public void UpdateMemberPicture(DataRow member, string picturePath, bool DeleteOldPicture)
        {
            string newPicture = "";
            int MemberId = (int)member["MemberId"];
            string imagesPath = Path.Combine(WebContext.Server.MapPath("~"), lw.CTE.MembersSettings.MemberPicturesFolder);

            if (picturePath != null)
            {
                newPicture = string.Format("{0}-{1}-{2}.jpg",
                    StringUtils.ToURL(member["FirstName"].ToString()),
                    StringUtils.ToURL(member["LastName"].ToString()),
                    System.Guid.NewGuid().ToString().Substring(16)
                );


                string path = Path.Combine(imagesPath, member["UserName"].ToString());

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = Path.Combine(path, newPicture);

                File.Copy(picturePath, path);

                lw.GraphicUtils.ImageUtils.Resize(path, path, MembersSettings.ProfilePictureWidth, MembersSettings.ProfilePictureHeight);
                lw.GraphicUtils.ImageUtils.Resize(path, path.Replace(".jpg", "-m.jpg"),
                    MembersSettings.MProfilePictureWidth, MembersSettings.MProfilePictureHeight);
                lw.GraphicUtils.ImageUtils.CropImage(path, path.Replace(".jpg", "-s.jpg"),
                    MembersSettings.SProfilePictureWidth, MembersSettings.SProfilePictureHeight, GraphicUtils.ImageUtils.AnchorPosition.Default);

                //lw.GraphicUtils.ImageUtils.Resize(path, path, MembersSettings.ProfilePictureWidth, MembersSettings.ProfilePictureHeight);
                //lw.GraphicUtils.ImageUtils.CropImage(path, path.Replace(".Jpg", "-m.Jpg"),
                //	MembersSettings.MProfilePictureWidth, MembersSettings.MProfilePictureHeight, GraphicUtils.ImageUtils.AnchorPosition.Default);
                //lw.GraphicUtils.ImageUtils.CropImage(path, path.Replace(".Jpg", "-s.Jpg"),
                //	MembersSettings.SProfilePictureWidth, MembersSettings.SProfilePictureHeight, GraphicUtils.ImageUtils.AnchorPosition.Default);

                DeleteOldPicture = true;
            }
            if (DeleteOldPicture)
            {
                if (member["Picture"] != DBNull.Value && !String.IsNullOrEmpty(member["Picture"].ToString())
                    && member["Picture"].ToString() != newPicture)
                {
                    string path = Path.Combine(imagesPath, member["Picture"].ToString());
                    if (File.Exists(path))
                        File.Delete(path);
                }
                member["Picture"] = newPicture;
            }

            string sql = string.Format("Update Members set Picture = '{1}' where MemberId={0}",
                    MemberId, newPicture);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }


        public void UpdateMemberPicture(MembersDs.MembersRow member, System.Web.HttpPostedFile picture, bool DeleteOldPicture)
        {
            UpdateMemberPicture(member, picture, DeleteOldPicture, false);
        }


        public void UpdateMemberPicture(MembersDs.MembersRow member, System.Web.HttpPostedFile picture, bool DeleteOldPicture, bool useGUID)
        {
            try
            {
                string newPicture = "";
                int MemberId = member.MemberId;
                string imagesPath = Path.Combine(WebContext.Server.MapPath("~"), lw.CTE.MembersSettings.MemberPicturesFolder);

                if (picture != null && picture.ContentLength == 0 && !DeleteOldPicture)
                    return;

                if (picture != null && picture.InputStream.Length > 0)
                {
                    newPicture = string.Format("{0}-{1}-{2}.jpg",
                        StringUtils.ToURL(member.FirstName),
                        StringUtils.ToURL(member.LastName),
                        System.Guid.NewGuid().ToString().Substring(16)
                    );

                    string path = "";

                    if (useGUID)
                        path = Path.Combine(imagesPath, StringUtils.ToURL(member.Geuid));
                    else
                        path = Path.Combine(imagesPath, StringUtils.ToURL(member.UserName));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, newPicture);

                    picture.SaveAs(path);

                    lw.GraphicUtils.ImageUtils.Resize(path, path, MembersSettings.ProfilePictureWidth, MembersSettings.ProfilePictureHeight);
                    lw.GraphicUtils.ImageUtils.Resize(path, path.Replace(".jpg", "-m.jpg"),
                        MembersSettings.MProfilePictureWidth, MembersSettings.MProfilePictureHeight);
                    lw.GraphicUtils.ImageUtils.CropImage(path, path.Replace(".jpg", "-s.jpg"),
                        MembersSettings.SProfilePictureWidth, MembersSettings.SProfilePictureHeight, GraphicUtils.ImageUtils.AnchorPosition.Default);

                    DeleteOldPicture = true;
                }
                if (DeleteOldPicture)
                {
                    if (member["Picture"] != DBNull.Value && !String.IsNullOrEmpty(member.Picture)
                        && member["Picture"].ToString() != newPicture)
                    {
                        string path = "";
                        string sPath = "";
                        string mPath = "";

                        if (useGUID)
                            path = sPath = mPath = Path.Combine(imagesPath, StringUtils.ToURL(member.Geuid));
                        else
                            path = sPath = mPath = Path.Combine(imagesPath, StringUtils.ToURL(member.UserName));


                        path = Path.Combine(path, member.Picture);

                        if (File.Exists(path))
                            File.Delete(path);


                        mPath = Path.Combine(mPath, member.Picture.Replace(".jpg", "-m.jpg"));

                        if (File.Exists(mPath))
                            File.Delete(mPath);

                        sPath = Path.Combine(sPath, member.Picture.Replace(".jpg", "-s.jpg"));

                        if (File.Exists(sPath))
                            File.Delete(sPath);
                    }
                    member.Picture = newPicture;
                }

                string sql = string.Format("Update Members set Picture = '{1}' where MemberId={0}",
                        MemberId, newPicture);
                DBUtils.ExecuteQuery(sql, cte.lib);
            }
            catch (Exception e)
            {
                lw.WebTools.ErrorHandler.HandleError(e);
            }
        }

        public string GetMemberPassword(string email)
        {
            return DecryptPassword(GetMember(email)["Password"].ToString());
        }

        /// <summary>
        /// Updates the member main picture and returns the image as string
        /// </summary>
        /// <param name="MemberId">Id of the related member</param>
        /// <param name="picture">
        /// If set to null, the function returns the actual present picture in the DB, if no picture in 
        /// DB the function returns null
        /// </param>
        /// <param name="DeleteOldPicture"></param>
        public string MemberPicture(int MemberId, System.Web.HttpPostedFile picture, bool DeleteOldPicture)
        {
            MembersDs.MembersRow member = GetMember(MemberId);
            try
            {
                if (picture != null)
                {
                    string newPicture = "";
                    string imagesPath = Path.Combine(WebContext.Server.MapPath("~"), lw.CTE.MembersSettings.MemberPicturesFolder);

                    if (picture != null && picture.ContentLength == 0 && !DeleteOldPicture)
                        return null;

                    if (picture != null && picture.InputStream.Length > 0)
                    {
                        newPicture = string.Format("{0}-{1}-{2}.jpg",
                            StringUtils.ToURL(member.FirstName),
                            StringUtils.ToURL(member.LastName),
                            System.Guid.NewGuid().ToString().Substring(16)
                        );


                        string path = Path.Combine(imagesPath, member.UserName);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        path = Path.Combine(path, newPicture);

                        picture.SaveAs(path);

                        lw.GraphicUtils.ImageUtils.Resize(path, path, MembersSettings.ProfilePictureWidth, MembersSettings.ProfilePictureHeight);
                        lw.GraphicUtils.ImageUtils.Resize(path, path.Replace(".jpg", "-m.jpg"),
                            MembersSettings.MProfilePictureWidth, MembersSettings.MProfilePictureHeight);
                        lw.GraphicUtils.ImageUtils.CropImage(path, path.Replace(".jpg", "-s.jpg"),
                            MembersSettings.SProfilePictureWidth, MembersSettings.SProfilePictureHeight, GraphicUtils.ImageUtils.AnchorPosition.Default);

                        DeleteOldPicture = true;
                    }
                    if (DeleteOldPicture)
                    {
                        if (member["Picture"] != DBNull.Value && !String.IsNullOrEmpty(member.Picture)
                            && member["Picture"].ToString() != newPicture)
                        {
                            string path = Path.Combine(imagesPath, member.Picture);
                            if (File.Exists(path))
                                File.Delete(path);
                        }
                        member.Picture = newPicture;
                    }

                    string sql = string.Format("Update Members set Picture = '{1}' where MemberId={0}",
                            MemberId, newPicture);
                    DBUtils.ExecuteQuery(sql, cte.lib);
                    return newPicture;
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(member.Picture))
                        return member.Picture;
                    else return null;
                }
            }
            catch (Exception e)
            {
                lw.WebTools.ErrorHandler.HandleError(e);
            }
            return null;
        }


        public void UpdateMemberPicture(int MemberId, System.Web.HttpPostedFile picture, bool DeleteOldPicture)
        {
            UpdateMemberPicture(MemberId, picture, DeleteOldPicture, false);
        }


        public void UpdateMemberPicture(int MemberId, System.Web.HttpPostedFile picture, bool DeleteOldPicture, bool useGUID)
        {
            MembersDs.MembersRow member = GetMember(MemberId);
            UpdateMemberPicture(member, picture, DeleteOldPicture, useGUID);
        }

        public void UpdatePrivacySettings(MembersDs.MembersRow member, int Privacy)
        {
            member.Privacy = Privacy;
            UpdatePrivacySettings(member.MemberId, Privacy);
        }

        public void UpdatePrivacySettings(int MemberId, int Privacy)
        {
            string sql = string.Format("Update Members set Privacy={1} where MemberId={0}", MemberId, Privacy);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public void UpdatePassword(int MemberId, string newPassword)
        {
            UpdatePassword(MemberId, newPassword, false);
        }

        public void UpdatePassword(int MemberId, string newPassword, bool UpdateTime)
        {
            string cond = string.Format(", PasswordChangeDate='{0}' ", DateTime.Now);

            string sql = string.Format("Update Members Set Password='{0}' {2} where MemberId={1}",
                StringUtils.SQLEncode(EncryptPassword(newPassword)), MemberId, UpdateTime ? cond : "");
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public void UpdateSecuritySettings(int MemberId, string password, string secretQuestion, string secretAnswer, string email, string alterNameEmail)
        {
            string sql = string.Format(@"Update Members Set {0} SecretQuestion='{1}', 
						SecretQuestionAnswer='{2}', Email='{3}', AlternateEmail='{4}' where MemberId={5}",
                String.IsNullOrWhiteSpace(password) ? "" : String.Format("Password='{0}', ", StringUtils.SQLEncode(EncryptPassword(password))),
                StringUtils.SQLEncode(secretQuestion),
                StringUtils.SQLEncode(secretAnswer),
                StringUtils.SQLEncode(email),
                StringUtils.SQLEncode(alterNameEmail),
                MemberId);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public void UpdateSecretQuestion(int MemberId, string secretQuestion, string secretAnswer)
        {
            string sql = string.Format("Update Members Set SecretQuestion='{0}', SecretQuestionAnswer='{1}' where MemberId={2}",
                StringUtils.SQLEncode(secretQuestion), StringUtils.SQLEncode(secretAnswer), MemberId);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public void UpdateMemberStatus(int MemberId, UserStatus status)
        {
            string sql = string.Format("Update Members Set Status={0} where MemberId={1}",
                (int)status, MemberId);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public static void UpdateOnlineStatus(int MemberId, bool online)
        {
            string sql = string.Format("Update Members Set Online={0}{2} where MemberId={1}",
                online ? 1 : 0, MemberId, online ? ", LastLogin=getDate()" : "");
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public static void UpdateRoles(int MemberId, Roles roles)
        {
            string sql = string.Format("Update Members Set Roles={0} where MemberId={1}",
                (int)roles, MemberId);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public static UserStatus GetUserStatus(DataRow member)
        {
            return GetUserStatus(member["Status"].ToString());
        }

        public static UserStatus GetUserStatus(string _status)
        {
            return (UserStatus)Enum.Parse(typeof(UserStatus), _status);
        }

        public static UserStatus GetUserStatus(int _status)
        {
            return GetUserStatus(_status.ToString());
        }

        public string EncryptPassword(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
                return password;

            Config cfg = new Config();
            string key = cfg.GetKey(CTE.parameters.MemberPasswordEncryption);
            if (!string.IsNullOrEmpty(key))
                return Cryptography.Encrypt(password, key);
            else
                return password;
        }

        public string DecryptPassword(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
                return password;

            Config cfg = new Config();
            string key = cfg.GetKey(CTE.parameters.MemberPasswordEncryption);
            if (!string.IsNullOrEmpty(key))
                return Cryptography.Decrypt(password, key);
            else
                return password;
        }

        public bool IsCorrectPassword(string password, DataRow member)
        {
            return EncryptPassword(password) == member["Password"].ToString();
        }

        public void UpdatePasswordChangeDate(int MemberId)
        {
            string sql = string.Format("Update Members Set PasswordChangeDate='{0}' where MemberId={1}",
                DateTime.Now, MemberId);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public void UpdateMemberStatus(int MemberId, int status)
        {
            string sql = string.Format("Update Members Set Status={0} where MemberId={1}",
                status, MemberId);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public void UpdateMemberAboutMe(int MemberId, string aboutme)
        {
            string sql = string.Format("Update MemberProfile Set AboutMe='{0}' where MemberId={1}",
                aboutme, MemberId);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

        public void UpdateAlternatePicture(int MemberId, string alternatePicture)
        {
            string sql = string.Format("Update Members Set AlternatePicture='{0}' where MemberId={1}",
                alternatePicture, MemberId);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }
        public void UpdateEmailAddress(int MemberId, string Email)
        {
            string sql = string.Format("Update Members Set Email='{0}' where MemberId={1}",
                StringUtils.SQLEncode(Email), MemberId);
            DBUtils.ExecuteQuery(sql, cte.lib);
        }
    }
}