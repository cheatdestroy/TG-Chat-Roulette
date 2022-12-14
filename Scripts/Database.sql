USE [ChatBot]
GO
/****** Object:  Table [ChatBot].[ActionData]    Script Date: 10/19/2022 2:00:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ChatBot].[ActionData](
	[ChatId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[CurrentAction] [int] NULL,
	[CurrentStep] [int] NULL,
	[LastUpdate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ChatId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ChatBot].[ChatRooms]    Script Date: 10/19/2022 2:00:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ChatBot].[ChatRooms](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstUserId] [bigint] NOT NULL,
	[SecondUserId] [bigint] NOT NULL,
	[NumberMessagesFirstUser] [int] NOT NULL,
	[NumberMessagesSecondUser] [int] NOT NULL,
	[StatusRoom] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[InitiatorEndId] [bigint] NULL,
 CONSTRAINT [PK__ChatRoom__CC30F18F15B0B3C6] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[FirstUserId] ASC,
	[SecondUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ChatBot].[Users]    Script Date: 10/19/2022 2:00:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ChatBot].[Users](
	[UserId] [bigint] NOT NULL,
	[Gender] [int] NULL,
	[Age] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK__Users__1788CC4C3F5B44DE] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ChatBot].[UserSettings]    Script Date: 10/19/2022 2:00:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ChatBot].[UserSettings](
	[UserId] [bigint] NOT NULL,
	[PreferredGender] [int] NULL,
	[PreferredAge] [int] NULL,
	[PreferredChatType] [int] NULL,
 CONSTRAINT [PK__UserSett__1788CC4C3F92F6DF] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ChatBot].[ActionData] ADD  DEFAULT (NULL) FOR [CurrentAction]
GO
ALTER TABLE [ChatBot].[ActionData] ADD  DEFAULT (NULL) FOR [CurrentStep]
GO
ALTER TABLE [ChatBot].[ActionData] ADD  DEFAULT (getdate()) FOR [LastUpdate]
GO
ALTER TABLE [ChatBot].[ChatRooms] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [ChatBot].[ChatRooms] ADD  DEFAULT ((0)) FOR [NumberMessagesFirstUser]
GO
ALTER TABLE [ChatBot].[ChatRooms] ADD  DEFAULT ((0)) FOR [NumberMessagesSecondUser]
GO
ALTER TABLE [ChatBot].[ChatRooms] ADD  DEFAULT (getdate()) FOR [StartDate]
GO
ALTER TABLE [ChatBot].[ChatRooms] ADD  DEFAULT (NULL) FOR [EndDate]
GO
ALTER TABLE [ChatBot].[ChatRooms] ADD  DEFAULT (NULL) FOR [InitiatorEndId]
GO
ALTER TABLE [ChatBot].[Users] ADD  DEFAULT (NULL) FOR [Gender]
GO
ALTER TABLE [ChatBot].[Users] ADD  DEFAULT (NULL) FOR [Age]
GO
ALTER TABLE [ChatBot].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [ChatBot].[UserSettings] ADD  DEFAULT (NULL) FOR [PreferredGender]
GO
ALTER TABLE [ChatBot].[UserSettings] ADD  DEFAULT (NULL) FOR [PreferredAge]
GO
ALTER TABLE [ChatBot].[UserSettings] ADD  DEFAULT (NULL) FOR [PreferredChatType]
GO
ALTER TABLE [ChatBot].[ActionData]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [ChatBot].[Users] ([UserId])
GO
ALTER TABLE [ChatBot].[ChatRooms]  WITH CHECK ADD  CONSTRAINT [FK__ChatRooms__First__5BE2A6F2] FOREIGN KEY([FirstUserId])
REFERENCES [ChatBot].[Users] ([UserId])
GO
ALTER TABLE [ChatBot].[ChatRooms] CHECK CONSTRAINT [FK__ChatRooms__First__5BE2A6F2]
GO
ALTER TABLE [ChatBot].[ChatRooms]  WITH CHECK ADD  CONSTRAINT [FK__ChatRooms__Secon__5CD6CB2B] FOREIGN KEY([SecondUserId])
REFERENCES [ChatBot].[Users] ([UserId])
GO
ALTER TABLE [ChatBot].[ChatRooms] CHECK CONSTRAINT [FK__ChatRooms__Secon__5CD6CB2B]
GO
ALTER TABLE [ChatBot].[UserSettings]  WITH CHECK ADD  CONSTRAINT [FK__UserSetti__UserI__5AEE82B9] FOREIGN KEY([UserId])
REFERENCES [ChatBot].[Users] ([UserId])
GO
ALTER TABLE [ChatBot].[UserSettings] CHECK CONSTRAINT [FK__UserSetti__UserI__5AEE82B9]
GO
