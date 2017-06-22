
/****** Object:  Table [dbo].[Dataform]    Script Date: 2/17/2016 5:45:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Dataform](
	[FormId] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[FormType] [nvarchar](25) NULL,
	[IP] [nvarchar](16) NULL,
	[Email] [varchar](150) NULL,
	[Name] [nvarchar](250) NULL,
	[DateCreated] [datetime] NULL,
	[Data] [ntext] NULL,
	[Attachements] [ntext] NULL,
 CONSTRAINT [PK_Dataform] PRIMARY KEY CLUSTERED 
(
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Dataform] ADD  CONSTRAINT [DF_Dataform_Geuid]  DEFAULT (newid()) FOR [Guid]
GO

-------------
-------------


/****** Object:  FullTextCatalog [Search_DataForm]    Script Date: 2/17/2016 5:45:47 PM ******/
CREATE FULLTEXT CATALOG [Search_DataForm]WITH ACCENT_SENSITIVITY = OFF

GO


ALTER TABLE [dbo].[Dataform]
ADD Status int
Go

ALTER TABLE [dbo].[Dataform]
ADD History xml
Go
