USE [master]
GO
/****** Object:  Database [AppChat]    Script Date: 06/12/2022 10:42:53 AM ******/
CREATE DATABASE [AppChat]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AppChat', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\AppChat.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AppChat_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\AppChat_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [AppChat] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AppChat].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AppChat] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AppChat] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AppChat] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AppChat] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AppChat] SET ARITHABORT OFF 
GO
ALTER DATABASE [AppChat] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AppChat] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AppChat] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AppChat] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AppChat] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AppChat] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AppChat] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AppChat] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AppChat] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AppChat] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AppChat] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AppChat] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AppChat] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AppChat] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AppChat] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AppChat] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AppChat] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AppChat] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AppChat] SET  MULTI_USER 
GO
ALTER DATABASE [AppChat] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AppChat] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AppChat] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AppChat] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AppChat] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AppChat] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [AppChat] SET QUERY_STORE = OFF
GO
USE [AppChat]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 06/12/2022 10:42:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](max) NOT NULL,
	[Password] [varchar](max) NOT NULL,
	[FullName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Account_Group]    Script Date: 06/12/2022 10:42:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account_Group](
	[AccountId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_Account_Group] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Group]    Script Date: 06/12/2022 10:42:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MessageGroup]    Script Date: 06/12/2022 10:42:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessageGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[TimeSend] [datetime] NOT NULL,
 CONSTRAINT [PK_MessageGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MessageUser]    Script Date: 06/12/2022 10:42:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessageUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SenderId] [int] NOT NULL,
	[ReceiveId] [int] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[TimeSend] [datetime] NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName]) VALUES (1, N'vytruong', N'123', N'Trương Nhật Vy')
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName]) VALUES (2, N'huyhieu', N'123', N'Khương Huy Hiếu')
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName]) VALUES (3, N'hoangtan', N'123', N'Hoàng Tân')
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName]) VALUES (4, N'kimluong', N'123', N'Thái Kim Lương')
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName]) VALUES (5, N'kimluong1', N'123', N'Là Kim Lương')
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName]) VALUES (6, N'kimluong2', N'123', N'Cũng Là Kim Lương')
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName]) VALUES (7, N'tattuy', N'123', N'Hồ Tất Tùy')
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
INSERT [dbo].[Account_Group] ([AccountId], [GroupId]) VALUES (1, 1)
INSERT [dbo].[Account_Group] ([AccountId], [GroupId]) VALUES (2, 1)
INSERT [dbo].[Account_Group] ([AccountId], [GroupId]) VALUES (3, 1)
INSERT [dbo].[Account_Group] ([AccountId], [GroupId]) VALUES (4, 2)
INSERT [dbo].[Account_Group] ([AccountId], [GroupId]) VALUES (5, 2)
INSERT [dbo].[Account_Group] ([AccountId], [GroupId]) VALUES (6, 2)
GO
SET IDENTITY_INSERT [dbo].[Group] ON 

INSERT [dbo].[Group] ([Id], [Name]) VALUES (1, N'GroupHocHanh')
INSERT [dbo].[Group] ([Id], [Name]) VALUES (2, N'GroupChoiGame')
SET IDENTITY_INSERT [dbo].[Group] OFF
GO
SET IDENTITY_INSERT [dbo].[MessageGroup] ON 

INSERT [dbo].[MessageGroup] ([Id], [GroupId], [Message], [TimeSend]) VALUES (1, 1, N'Chào group học hành nha', CAST(N'2022-11-10T11:24:57.777' AS DateTime))
INSERT [dbo].[MessageGroup] ([Id], [GroupId], [Message], [TimeSend]) VALUES (2, 1, N'nay có ai học gì không', CAST(N'2022-11-10T11:24:57.777' AS DateTime))
INSERT [dbo].[MessageGroup] ([Id], [GroupId], [Message], [TimeSend]) VALUES (3, 2, N'group gì tối ngày toàn game', CAST(N'2022-11-10T11:24:57.777' AS DateTime))
INSERT [dbo].[MessageGroup] ([Id], [GroupId], [Message], [TimeSend]) VALUES (4, 2, N'không game thì cũng banh bóng', CAST(N'2022-11-10T11:24:57.777' AS DateTime))
INSERT [dbo].[MessageGroup] ([Id], [GroupId], [Message], [TimeSend]) VALUES (6, 2, N'chán thật sự', CAST(N'2022-11-10T11:24:57.780' AS DateTime))
SET IDENTITY_INSERT [dbo].[MessageGroup] OFF
GO
SET IDENTITY_INSERT [dbo].[MessageUser] ON 

INSERT [dbo].[MessageUser] ([Id], [SenderId], [ReceiveId], [Message], [TimeSend]) VALUES (1, 1, 2, N'Vy chào Hiếu', CAST(N'2022-11-10T11:24:57.777' AS DateTime))
INSERT [dbo].[MessageUser] ([Id], [SenderId], [ReceiveId], [Message], [TimeSend]) VALUES (2, 2, 1, N'Hiếu chào Vy', CAST(N'2022-11-10T11:24:57.777' AS DateTime))
INSERT [dbo].[MessageUser] ([Id], [SenderId], [ReceiveId], [Message], [TimeSend]) VALUES (3, 1, 2, N'rep chậm vậy', CAST(N'2022-11-10T11:24:57.780' AS DateTime))
INSERT [dbo].[MessageUser] ([Id], [SenderId], [ReceiveId], [Message], [TimeSend]) VALUES (4, 2, 1, N'mới on cha nội', CAST(N'2022-11-10T11:24:57.780' AS DateTime))
SET IDENTITY_INSERT [dbo].[MessageUser] OFF
GO
ALTER TABLE [dbo].[Account_Group]  WITH CHECK ADD  CONSTRAINT [FK_Account_Group_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Account_Group] CHECK CONSTRAINT [FK_Account_Group_Account]
GO
ALTER TABLE [dbo].[Account_Group]  WITH CHECK ADD  CONSTRAINT [FK_Account_Group_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([Id])
GO
ALTER TABLE [dbo].[Account_Group] CHECK CONSTRAINT [FK_Account_Group_Group]
GO
ALTER TABLE [dbo].[MessageGroup]  WITH CHECK ADD  CONSTRAINT [FK_MessageGroup_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([Id])
GO
ALTER TABLE [dbo].[MessageGroup] CHECK CONSTRAINT [FK_MessageGroup_Group]
GO
ALTER TABLE [dbo].[MessageUser]  WITH CHECK ADD  CONSTRAINT [FK_MessageUser_Account] FOREIGN KEY([SenderId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[MessageUser] CHECK CONSTRAINT [FK_MessageUser_Account]
GO
ALTER TABLE [dbo].[MessageUser]  WITH CHECK ADD  CONSTRAINT [FK_MessageUser_Account1] FOREIGN KEY([ReceiveId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[MessageUser] CHECK CONSTRAINT [FK_MessageUser_Account1]
GO
USE [master]
GO
ALTER DATABASE [AppChat] SET  READ_WRITE 
GO
