GO

/****** Object:  Table [dbo].[PageExtendedDataProperty]    Script Date: 6/3/2018 1:52:55 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PageExtendedDataProperty](
	[DataPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[DataPropertyName] [nvarchar](50) NULL,
 CONSTRAINT [PK_PageExtendedDataProperty] PRIMARY KEY CLUSTERED 
(
	[DataPropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

GO

/****** Object:  Table [dbo].[PageExtendedDataPropertyValue]    Script Date: 6/3/2018 1:53:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PageExtendedDataPropertyValue](
	[PageID] [int] NOT NULL,
	[DataPropertyID] [int] NOT NULL,
	[DataPropertyValue] [ntext] NULL,
 CONSTRAINT [PK_PageExtendedDataPropertyValue] PRIMARY KEY CLUSTERED 
(
	[PageID] ASC,
	[DataPropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

GO
/****** Object:  Trigger [dbo].[Page_Delete]    Script Date: 6/3/2018 1:54:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER TRIGGER [dbo].[Page_Delete]
   ON  [dbo].[Pages]
   AFTER DELETE
AS 
BEGIN

Delete from PageDataPropertyValue where PageId in (select PageID from deleted);
Delete from PageExtendedDataPropertyValue where PageId in (select PageID from deleted);
Delete From HashTags_Relations where RelateTo in (select PageID from deleted) and RelationType in( 1)
Delete from PageImages where PageId in (select PageID from deleted);

END



