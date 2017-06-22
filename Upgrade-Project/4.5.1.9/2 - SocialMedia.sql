/****** Object:  Table [dbo].[SocialMediaPost]    Script Date: 8/23/2016 3:38:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SocialMediaPost](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PageId] [int] NULL,
	[DatePosted] [datetime] NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_SocialMediaPost] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
