USE [TF]
GO
/****** Object:  Table [dbo].[TaskStageTemplate]    Script Date: 01/25/2016 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskStageTemplate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Executors] [varchar](500) NOT NULL,
	[Approvers] [varchar](500) NOT NULL,
 CONSTRAINT [PK_TaskStageTemplate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TaskStage]    Script Date: 01/25/2016 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskStage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[TaskStatus] [int] NOT NULL,
	[TemplateID] [int] NOT NULL,
	[ActualExec] [varchar](50) NULL,
	[ExecTime] [datetime] NULL,
	[ActualAppr] [varchar](50) NULL,
	[ApprTime] [datetime] NULL,
	[Remark] [varchar](500) NULL,
 CONSTRAINT [PK_TaskStage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TaskInfo]    Script Date: 01/25/2016 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NOT NULL,
	[FlowID] [int] NOT NULL,
	[Sponsor] [varchar](50) NULL,
	[Remark] [varchar](500) NULL,
 CONSTRAINT [PK_TaskInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FlowTemplate]    Script Date: 01/25/2016 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FlowTemplate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Stages] [varchar](500) NOT NULL,
 CONSTRAINT [PK_FlowTemplate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Flow]    Script Date: 01/25/2016 17:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Flow](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[CurrentIndex] [int] NOT NULL,
	[TemplateID] [int] NOT NULL,
	[Stages] [varchar](500) NOT NULL,
	[Remark] [varchar](50) NULL,
 CONSTRAINT [PK_Flow] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_Flow_CurrentIndex]    Script Date: 01/25/2016 17:38:55 ******/
ALTER TABLE [dbo].[Flow] ADD  CONSTRAINT [DF_Flow_CurrentIndex]  DEFAULT ((-1)) FOR [CurrentIndex]
GO
/****** Object:  Default [DF_TaskInfo_EntityId]    Script Date: 01/25/2016 17:38:55 ******/
ALTER TABLE [dbo].[TaskInfo] ADD  CONSTRAINT [DF_TaskInfo_EntityId]  DEFAULT ((0)) FOR [EntityId]
GO
/****** Object:  Default [DF_TaskInfo_FlowID]    Script Date: 01/25/2016 17:38:55 ******/
ALTER TABLE [dbo].[TaskInfo] ADD  CONSTRAINT [DF_TaskInfo_FlowID]  DEFAULT ((0)) FOR [FlowID]
GO
/****** Object:  Default [DF_TaskStage_Status]    Script Date: 01/25/2016 17:38:55 ******/
ALTER TABLE [dbo].[TaskStage] ADD  CONSTRAINT [DF_TaskStage_Status]  DEFAULT ((0)) FOR [TaskStatus]
GO
/****** Object:  Default [DF_TaskStage_TemplateID]    Script Date: 01/25/2016 17:38:55 ******/
ALTER TABLE [dbo].[TaskStage] ADD  CONSTRAINT [DF_TaskStage_TemplateID]  DEFAULT ((0)) FOR [TemplateID]
GO
