
/****** Object:  View [dbo].[MemberView]    Script Date: 7/22/2016 10:51:42 AM ******/
DROP VIEW [dbo].[MemberView]
GO

/****** Object:  View [dbo].[MemberView]    Script Date: 7/22/2016 10:51:42 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[MemberView]
AS
SELECT        m.Geuid, m.MemberId, m.UserName, m.Password, m.Status, m.Email, m.DateCreated, m.LastModified, m.LastLogin, m.Online, m.SecretQuestion, m.SecretQuestionAnswer, 
                         m.FirstName + '  ' + m.LastName AS Name, m.FirstName, m.MiddleName, m.LastName, m.DateOfBirth, m.Picture, m.Privacy, m.Title, m.Gender, m.AlternateEmail, m.FullName, m.JoinNewsletter, m.Comments, 
                         m.NativeFullName, m.Roles, m.PasswordChangeDate, m.PrivateComments, m.ChangedBy
FROM            dbo.Members AS m LEFT OUTER JOIN
                         dbo.MemberProfile AS mp ON m.MemberId = mp.MemberId
GO
