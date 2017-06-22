using System;
using lw.Operators.Security;
using lw.WebTools;
using lw.Network;
using lw.CTE.Enum;


namespace lw.cms
{
	/// <summary>
	/// This page must be reffered withing all the cms pages
	/// </summary>

	public class CMSTools
	{
        /// <summary>
        /// sendNotification to notification email when actions are done through the CMS
        /// <param name="ObjectType">Type of the Object submitted </param>
        /// <param name="ObjectId">Object Id as saved in database</param>
        /// <param name="ChangeAction">Action done on the object</param>
        /// <param name="Change">Description of the change done on the object</param>
        /// </summary>
        public static void SendNotification(SiteSections? ObjectType, int ObjectId, CMSActions? ChangeAction, string Change)
        {

            Config cfg = new Config();
            string emailTo = cfg.GetKey("NotificationEmail");

            MailManager mMgr = new MailManager();

            string subject = "CMS change Notification in " + ObjectType;

            string siteName = cfg.GetKey("SiteName");

            string mailTo = cfg.GetKey(CTE.parameters.Notification_Email);
            //siteName,objectType,objectId,changeAction,change
            string mailBody = string.Format("<h1>{0}</h1><p><table><tr><td>Member:</td><td>{5}</td></tr><tr> <td>Change in:</td><td>{1} , ID: {2}</td> </tr> <tr> <td>Action:</td><td>{3}</td> </tr> <tr> <td>Change:</td><td>{4}</td> </tr></p>", siteName, ObjectType, ObjectId.ToString(), ChangeAction, Change, WebContext.Profile.dbUserName);

            mMgr.SendMail(null, mailTo, subject, mailBody);

        }
	}
}