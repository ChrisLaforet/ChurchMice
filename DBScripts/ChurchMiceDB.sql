USE [ChurchMice]
GO
/****** Object:  Table [dbo].[User]    Script Date: 4/6/2023 8:14:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [varchar](50) NOT NULL,
	[Username] [varchar](20) NOT NULL,
	[Fullname] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[CreateDate] [date] NOT NULL,
	[PasswordHash] [varchar](1000) NULL,
	[LastLoginDatetime] [datetime] NULL,
	[ResetKey] [varchar](50) NULL,
	[ResetExpirationDatetime] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_User] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 4/6/2023 8:14:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [varchar](50) NOT NULL,
	[RoleLevel] [int] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_UserRole] UNIQUE NONCLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_User]
GO
/****** Object:  Table [dbo].[UserToken]    Script Date: 4/6/2023 8:14:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserToken](
	[ID] [varchar](50) NOT NULL,
	[UserID] [varchar](50) NOT NULL,
	[TokenKey] [varchar](255) NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[Expired] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_UserToken] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserToken]  WITH CHECK ADD  CONSTRAINT [FK_UserToken_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserToken] CHECK CONSTRAINT [FK_UserToken_User]
GO

/****** Object:  Table [dbo].[EmailQueue]    Script Date: 4/10/2023 7:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailQueue](
	[ID] [varchar](50) NOT NULL,
	[EmailRecipient] [varchar](255) NOT NULL,
	[EmailSender] [varchar](255) NOT NULL,
	[EmailSubject] [varchar](255) NULL,
	[EmailBody] [varchar](max) NOT NULL,
	[SentDatetime] [datetime2](7) NOT NULL,
	[AttemptDatetime] [datetime2](7) NULL,
	[TotalAttempts] [int] NOT NULL,
	[AttachmentFilename] [varchar](255) NULL,
 CONSTRAINT [PK_EmailQueue] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Member]    Script Date: 4/10/2023 7:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Member](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](30) NOT NULL,
	[LastName] [varchar](30) NOT NULL,
	[Email] [varchar](255) NULL,
	[HomePhone] [varchar](20) NULL,
	[MobilePhone] [varchar](20) NULL,
	[MailingAddress1] [varchar](100) NULL,
	[MailingAddress2] [varchar](100) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](2) NULL,
	[Zip] [varchar](10) NULL,
	[Birthday] [varchar](20) NULL,
	[Anniversary] [varchar](20) NULL,
	[MemberSince] [date] NULL,
	[Created] [datetime2](7) NOT NULL,
	[UserID] [varchar](50) NULL,
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Member]  WITH CHECK ADD  CONSTRAINT [FK_Member_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Member] CHECK CONSTRAINT [FK_Member_User]
GO


/****** Object:  Table [dbo].[MemberImage]    Script Date: 8/9/2023 5:09:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MemberImage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MemberID] [int] NOT NULL,
	[Image] [varchar](max) NOT NULL,
	[ImageType] [varchar](50) NOT NULL,
	[UploadDate] [datetime2](7) NOT NULL,
	[UploadUserID] [varchar](50) NOT NULL,
	[ApproveDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MemberImage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[MemberImage] ADD  CONSTRAINT [DF_MemberImage_ImageType]  DEFAULT ('image/png') FOR [ImageType]
GO

ALTER TABLE [dbo].[MemberImage]  WITH CHECK ADD  CONSTRAINT [FK_MemberImage_Member] FOREIGN KEY([MemberID])
REFERENCES [dbo].[Member] ([ID])
GO

ALTER TABLE [dbo].[MemberImage] CHECK CONSTRAINT [FK_MemberImage_Member]
GO

ALTER TABLE [dbo].[MemberImage]  WITH CHECK ADD  CONSTRAINT [FK_MemberImage_User] FOREIGN KEY([UploadUserID])
REFERENCES [dbo].[User] ([ID])
GO

ALTER TABLE [dbo].[MemberImage] CHECK CONSTRAINT [FK_MemberImage_User]
GO
