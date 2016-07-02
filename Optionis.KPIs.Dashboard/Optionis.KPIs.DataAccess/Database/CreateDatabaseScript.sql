CREATE TABLE [dbo].[Deployments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReleaseId] [int] NOT NULL,
	[DeploymentDate] [datetime] NOT NULL CONSTRAINT [DF_Deploymen_DeploymentDate]  DEFAULT (getdate()),
	[Version] [nvarchar](25) NOT NULL,
	[DeploymentStatus] [nvarchar](25) NULL,
 CONSTRAINT [PK_Deployments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Issues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReleaseId] [int] NOT NULL,
	[IssueId] [nvarchar](25) NULL,
	[Title] [nvarchar](255) NULL,
	[Link] [nvarchar](255) NULL,
 CONSTRAINT [PK_Issues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Releases](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_Releases_Created]  DEFAULT (getdate()),
	[CreatedBy] [nvarchar](100) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Application] [nvarchar](100) NOT NULL,
	[Comments] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Releases] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_Users_Created]  DEFAULT (getdate()),
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Users_UserName] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Deployments]  WITH CHECK ADD  CONSTRAINT [FK_Deployments_Releases] FOREIGN KEY([ReleaseId])
REFERENCES [dbo].[Releases] ([Id])
GO

ALTER TABLE [dbo].[Deployments] CHECK CONSTRAINT [FK_Deployments_Releases]
GO

ALTER TABLE [dbo].[Issues]  WITH CHECK ADD  CONSTRAINT [FK_Issues_Releases] FOREIGN KEY([ReleaseId])
REFERENCES [dbo].[Releases] ([Id])
GO

ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_Issues_Releases]
GO
