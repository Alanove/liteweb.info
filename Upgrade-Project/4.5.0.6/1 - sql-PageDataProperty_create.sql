------------- Karim


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PageDataProperty](
	[DataPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[DataPropertyName] [nvarchar](50) NULL,
 CONSTRAINT [PK_PageDataProperty] PRIMARY KEY CLUSTERED 
(
	[DataPropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/******************************************************************/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PageDataPropertyValue](
	[PageID] [int] NOT NULL,
	[DataPropertyID] [int] NOT NULL,
	[DataPropertyValue] [nvarchar](50) NULL,
 CONSTRAINT [PK_PageDataPropertyValue] PRIMARY KEY CLUSTERED 
(
	[PageID] ASC,
	[DataPropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

