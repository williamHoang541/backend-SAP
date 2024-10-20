USE [master]
GO
/****** Object:  Database [SAPelearning]    Script Date: 10/13/2024 8:14:12 AM ******/
CREATE DATABASE [SAPelearning]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SAPelearning', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\SAPelearning.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SAPelearning_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\SAPelearning_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [SAPelearning] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SAPelearning].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SAPelearning] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SAPelearning] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SAPelearning] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SAPelearning] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SAPelearning] SET ARITHABORT OFF 
GO
ALTER DATABASE [SAPelearning] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SAPelearning] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SAPelearning] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SAPelearning] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SAPelearning] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SAPelearning] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SAPelearning] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SAPelearning] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SAPelearning] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SAPelearning] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SAPelearning] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SAPelearning] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SAPelearning] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SAPelearning] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SAPelearning] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SAPelearning] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SAPelearning] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SAPelearning] SET RECOVERY FULL 
GO
ALTER DATABASE [SAPelearning] SET  MULTI_USER 
GO
ALTER DATABASE [SAPelearning] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SAPelearning] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SAPelearning] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SAPelearning] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SAPelearning] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SAPelearning] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'SAPelearning', N'ON'
GO
ALTER DATABASE [SAPelearning] SET QUERY_STORE = ON
GO
ALTER DATABASE [SAPelearning] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [SAPelearning]
GO
/****** Object:  Table [dbo].[Certificate]    Script Date: 10/13/2024 8:14:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CertificateName] [varchar](255) NULL,
	[Description] [varchar](500) NULL,
	[Level] [varchar](50) NULL,
	[Environment] [varchar](50) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CertificateModule]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificateModule](
	[CertificateId] [int] NOT NULL,
	[ModuleId] [int] NOT NULL,
 CONSTRAINT [PK_CertificateModule] PRIMARY KEY CLUSTERED 
(
	[CertificateId] ASC,
	[ModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CertificateQuestion]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificateQuestion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TopicId] [int] NULL,
	[QuestionText] [varchar](1000) NULL,
	[Answer] [varchar](255) NULL,
	[IsCorrect] [bit] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CertificateSampleTest]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificateSampleTest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CertificateId] [int] NULL,
	[SampleTestName] [varchar](255) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CertificateTestAttempt]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificateTestAttempt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](255) NULL,
	[SampleTestId] [int] NULL,
	[AttemptDate] [datetime] NULL,
	[Score] [float] NULL,
	[CorrectAnswers] [int] NULL,
	[TotalAnswers] [int] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CertificateTestQuestion]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificateTestQuestion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SampleTestId] [int] NULL,
	[QuestionId] [int] NULL,
	[DisplayInTest] [int] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InstructorId] [int] NULL,
	[CertificateId] [int] NULL,
	[CourseName] [varchar](255) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Mode] [varchar](50) NULL,
	[Price] [float] NULL,
	[TotalStudent] [int] NULL,
	[EnrollmentDate] [date] NULL,
	[Location] [varchar](255) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseMaterial]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseMaterial](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[MaterialName] [varchar](255) NULL,
	[FileMaterial] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseSession]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseSession](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[InstructorId] [int] NULL,
	[TopicId] [int] NULL,
	[SessionName] [varchar](255) NULL,
	[SessionDescription] [varchar](500) NULL,
	[SessionDate] [datetime] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollment]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](255) NULL,
	[CourseId] [int] NULL,
	[EnrollmentDate] [date] NULL,
	[Price] [float] NULL,
	[Status] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Instructor]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instructor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](255) NULL,
	[Fullname] [varchar](255) NULL,
	[Email] [varchar](255) NULL,
	[Phonenumber] [varchar](50) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnrollmentId] [int] NULL,
	[Amount] [float] NULL,
	[PaymentDate] [datetime] NULL,
	[TransactionId] [int] NULL,
	[Status] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SapModule]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SapModule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleName] [varchar](255) NULL,
	[ModuleDescription] [varchar](500) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TopicArea]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TopicArea](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CertificateId] [int] NULL,
	[TopicName] [varchar](255) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/13/2024 8:14:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [varchar](255) NOT NULL,
	[Username] [varchar](255) NULL,
	[Email] [varchar](255) NULL,
	[Password] [varchar](255) NULL,
	[RegistrationDate] [date] NULL,
	[Role] [varchar](255) NULL,
	[Fullname] [varchar](255) NULL,
	[Education] [varchar](255) NULL,
	[Phonenumber] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[LastLogin] [datetime] NULL,
	[IsOnline] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Instructor] ON 

INSERT [dbo].[Instructor] ([Id], [UserId], [Fullname], [Email], [Phonenumber], [Status]) VALUES (1, N'Ie67bc', N'+_update', N'string', N'string', 1)
SET IDENTITY_INSERT [dbo].[Instructor] OFF
GO
INSERT [dbo].[User] ([Id], [Username], [Email], [Password], [RegistrationDate], [Role], [Fullname], [Education], [Phonenumber], [Gender], [LastLogin], [IsOnline]) VALUES (N'a00001', N'admin', N'admin@sapelearning', N'$2a$11$xc7GyDW6ai9uX3g/f2qT6.70knlj/QxCjHd4FCbs4tLd5cR9qfVKq', CAST(N'2024-10-04' AS Date), N'admin', N'admin', N'FPT', N'0938333340', N'male', CAST(N'2024-10-12T23:20:01.943' AS DateTime), 1)
INSERT [dbo].[User] ([Id], [Username], [Email], [Password], [RegistrationDate], [Role], [Fullname], [Education], [Phonenumber], [Gender], [LastLogin], [IsOnline]) VALUES (N'Ie67bc', N'instructor', N'string', N'$2a$11$vDdgpCFRaAOhFr.iP8nJBuVtEKFRGIT6LQm7/6J9Nfvb7/BXOuAMq', CAST(N'2024-10-04' AS Date), N'instructor', N'+_update', N'SAP', N'string', N'male', CAST(N'2024-10-04T15:09:57.617' AS DateTime), 0)
INSERT [dbo].[User] ([Id], [Username], [Email], [Password], [RegistrationDate], [Role], [Fullname], [Education], [Phonenumber], [Gender], [LastLogin], [IsOnline]) VALUES (N'Sf7ac7', N'student', NULL, N'$2a$11$dsHbOCasN84FriMKUHhTLe7h0aDLosARPZCSShwnROn3TwFMCEouS', CAST(N'2024-10-04' AS Date), N'student', NULL, NULL, NULL, NULL, CAST(N'2024-10-04T14:52:29.223' AS DateTime), 0)
GO
ALTER TABLE [dbo].[CertificateModule]  WITH CHECK ADD  CONSTRAINT [FK_CertificateModule_Certificate] FOREIGN KEY([CertificateId])
REFERENCES [dbo].[Certificate] ([Id])
GO
ALTER TABLE [dbo].[CertificateModule] CHECK CONSTRAINT [FK_CertificateModule_Certificate]
GO
ALTER TABLE [dbo].[CertificateModule]  WITH CHECK ADD  CONSTRAINT [FK_CertificateModule_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[SapModule] ([Id])
GO
ALTER TABLE [dbo].[CertificateModule] CHECK CONSTRAINT [FK_CertificateModule_Module]
GO
ALTER TABLE [dbo].[CertificateQuestion]  WITH CHECK ADD  CONSTRAINT [FK_CertificateQuestion_Topic] FOREIGN KEY([TopicId])
REFERENCES [dbo].[TopicArea] ([Id])
GO
ALTER TABLE [dbo].[CertificateQuestion] CHECK CONSTRAINT [FK_CertificateQuestion_Topic]
GO
ALTER TABLE [dbo].[CertificateSampleTest]  WITH CHECK ADD  CONSTRAINT [FK_CertificateSampleTest_Certificate] FOREIGN KEY([CertificateId])
REFERENCES [dbo].[Certificate] ([Id])
GO
ALTER TABLE [dbo].[CertificateSampleTest] CHECK CONSTRAINT [FK_CertificateSampleTest_Certificate]
GO
ALTER TABLE [dbo].[CertificateTestAttempt]  WITH CHECK ADD  CONSTRAINT [FK_CertificateTestAttempt_SampleTest] FOREIGN KEY([SampleTestId])
REFERENCES [dbo].[CertificateSampleTest] ([Id])
GO
ALTER TABLE [dbo].[CertificateTestAttempt] CHECK CONSTRAINT [FK_CertificateTestAttempt_SampleTest]
GO
ALTER TABLE [dbo].[CertificateTestAttempt]  WITH CHECK ADD  CONSTRAINT [FK_CertificateTestAttempt_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[CertificateTestAttempt] CHECK CONSTRAINT [FK_CertificateTestAttempt_User]
GO
ALTER TABLE [dbo].[CertificateTestQuestion]  WITH CHECK ADD  CONSTRAINT [FK_CertificateTestQuestion_Question] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[CertificateQuestion] ([Id])
GO
ALTER TABLE [dbo].[CertificateTestQuestion] CHECK CONSTRAINT [FK_CertificateTestQuestion_Question]
GO
ALTER TABLE [dbo].[CertificateTestQuestion]  WITH CHECK ADD  CONSTRAINT [FK_CertificateTestQuestion_SampleTest] FOREIGN KEY([SampleTestId])
REFERENCES [dbo].[CertificateSampleTest] ([Id])
GO
ALTER TABLE [dbo].[CertificateTestQuestion] CHECK CONSTRAINT [FK_CertificateTestQuestion_SampleTest]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Certificate] FOREIGN KEY([CertificateId])
REFERENCES [dbo].[Certificate] ([Id])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Certificate]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Instructor] FOREIGN KEY([InstructorId])
REFERENCES [dbo].[Instructor] ([Id])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Instructor]
GO
ALTER TABLE [dbo].[CourseMaterial]  WITH CHECK ADD  CONSTRAINT [FK_CourseMaterial_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([Id])
GO
ALTER TABLE [dbo].[CourseMaterial] CHECK CONSTRAINT [FK_CourseMaterial_Course]
GO
ALTER TABLE [dbo].[CourseSession]  WITH CHECK ADD  CONSTRAINT [FK_CourseSession_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([Id])
GO
ALTER TABLE [dbo].[CourseSession] CHECK CONSTRAINT [FK_CourseSession_Course]
GO
ALTER TABLE [dbo].[CourseSession]  WITH CHECK ADD  CONSTRAINT [FK_CourseSession_Instructor] FOREIGN KEY([InstructorId])
REFERENCES [dbo].[Instructor] ([Id])
GO
ALTER TABLE [dbo].[CourseSession] CHECK CONSTRAINT [FK_CourseSession_Instructor]
GO
ALTER TABLE [dbo].[CourseSession]  WITH CHECK ADD  CONSTRAINT [FK_CourseSession_Topic] FOREIGN KEY([TopicId])
REFERENCES [dbo].[TopicArea] ([Id])
GO
ALTER TABLE [dbo].[CourseSession] CHECK CONSTRAINT [FK_CourseSession_Topic]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([Id])
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_Course]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_User]
GO
ALTER TABLE [dbo].[Instructor]  WITH CHECK ADD  CONSTRAINT [FK_Instructor_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Instructor] CHECK CONSTRAINT [FK_Instructor_User]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_Enrollment] FOREIGN KEY([EnrollmentId])
REFERENCES [dbo].[Enrollment] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_Enrollment]
GO
ALTER TABLE [dbo].[TopicArea]  WITH CHECK ADD  CONSTRAINT [FK_CertificateTopicArea_Certificate] FOREIGN KEY([CertificateId])
REFERENCES [dbo].[Certificate] ([Id])
GO
ALTER TABLE [dbo].[TopicArea] CHECK CONSTRAINT [FK_CertificateTopicArea_Certificate]
GO
USE [master]
GO
ALTER DATABASE [SAPelearning] SET  READ_WRITE 
GO
