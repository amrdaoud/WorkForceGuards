/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Activities]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Color] [nvarchar](450) NULL,
	[IsPhone] [bit] NOT NULL,
	[IsAbsence] [bit] NOT NULL,
	[IsPaid] [bit] NOT NULL,
	[IsWorkTime] [bit] NOT NULL,
	[IsBreak] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DisableEdit] [bit] NOT NULL,
	[IsUndefined] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[NeedBackup] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdateDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Activities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Assets]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Assets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Barcode] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Specs] [nvarchar](max) NULL,
	[IsDisabled] [bit] NOT NULL,
	[AssetTypeId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Assets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetTerms]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetTerms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_AssetTerms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetTermValues]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetTermValues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Count] [float] NOT NULL,
	[IsOptional] [bit] NOT NULL,
	[AssetTypeId] [int] NOT NULL,
	[AssetTermId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_AssetTermValues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetTypes]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_AssetTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttendanceTypes]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttendanceTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](450) NULL,
	[IsAbsence] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DisableEdit] [bit] NOT NULL,
	[Hidden] [bit] NOT NULL,
	[DefaultActivityId] [int] NULL,
 CONSTRAINT [PK_AttendanceTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BkpDailyAttendances]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BkpDailyAttendances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DailyAttendanceId] [int] NOT NULL,
	[Alias] [nvarchar](max) NULL,
	[ActionTime] [datetime2](7) NOT NULL,
	[StaffMemberId] [int] NOT NULL,
	[ScheduleId] [int] NOT NULL,
	[Day] [datetime2](7) NOT NULL,
	[AttendanceTypeId] [int] NOT NULL,
	[TransportationRouteId] [int] NULL,
	[HeadOfSectionId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_BkpDailyAttendances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BkpScheduleDetails]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BkpScheduleDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DailyAttendanceId] [int] NOT NULL,
	[ActivityId] [int] NOT NULL,
	[IntervalId] [int] NOT NULL,
	[Duration] [float] NULL,
	[ScheduleId] [int] NOT NULL,
 CONSTRAINT [PK_BkpScheduleDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[breakTypeOptions]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[breakTypeOptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StaffMemberId] [int] NOT NULL,
	[ScheduleId] [int] NOT NULL,
	[AttendenceTypeId] [int] NOT NULL,
	[TransportationRouteId] [int] NULL,
	[IsApproved] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_breakTypeOptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[colors]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[colors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ColorName] [nvarchar](450) NULL,
	[ColorCode] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_colors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DailyAttendances]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DailyAttendances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StaffMemberId] [int] NOT NULL,
	[ScheduleId] [int] NOT NULL,
	[Day] [datetime2](7) NOT NULL,
	[AttendanceTypeId] [int] NOT NULL,
	[TransportationRouteId] [int] NULL,
	[HeadOfSectionId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[HaveBackup] [bit] NOT NULL,
 CONSTRAINT [PK_DailyAttendances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[dayOffOptions]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dayOffOptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StaffMemberId] [int] NOT NULL,
	[ScheduleId] [int] NOT NULL,
	[DayOne] [nvarchar](max) NULL,
	[DayTwo] [nvarchar](max) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_dayOffOptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ForecastDetails]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForecastDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ForecastId] [int] NOT NULL,
	[IntervalId] [int] NOT NULL,
	[DayoffWeek] [nvarchar](max) NULL,
	[EmployeeCount] [int] NOT NULL,
 CONSTRAINT [PK_ForecastDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ForecastHistories]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForecastHistories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Day] [datetime2](7) NOT NULL,
	[Offered] [int] NOT NULL,
	[Duration] [float] NOT NULL,
	[IntervalId] [int] NOT NULL,
 CONSTRAINT [PK_ForecastHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ForeCastings]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForeCastings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScheduleId] [int] NOT NULL,
	[IntervalId] [int] NOT NULL,
	[Day] [datetime2](7) NOT NULL,
	[EmployeeCount] [int] NOT NULL,
 CONSTRAINT [PK_ForeCastings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Forecasts]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Forecasts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[DurationTolerance] [float] NOT NULL,
	[OfferedTolerance] [float] NOT NULL,
	[ServiceLevel] [float] NOT NULL,
	[ServiceTime] [int] NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
	[ExceptDates] [nvarchar](max) NULL,
	[IsSaved] [bit] NOT NULL,
 CONSTRAINT [PK_Forecasts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HeadOfSections]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HeadOfSections](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Alias] [nvarchar](50) NOT NULL,
	[Gender] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Language] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_HeadOfSections] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Intervals]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Intervals](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TimeMap] [time](7) NOT NULL,
	[OrderMap] [int] NOT NULL,
	[CoverMap] [int] NOT NULL,
	[Tolerance] [float] NOT NULL,
 CONSTRAINT [PK_Intervals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IpccAgents]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IpccAgents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StaffMemberEmployeeId] [int] NOT NULL,
	[StaffMemberName] [nvarchar](max) NULL,
	[UtcDate] [datetime2](7) NOT NULL,
	[LocalDate] [datetime2](7) NOT NULL,
	[Duration] [int] NOT NULL,
 CONSTRAINT [PK_IpccAgents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[ContactPhone] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScheduleDetail]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DailyAttendanceId] [int] NOT NULL,
	[ActivityId] [int] NOT NULL,
	[IntervalId] [int] NOT NULL,
	[Duration] [float] NULL,
	[ScheduleId] [int] NOT NULL,
	[BackupStaffId] [int] NULL,
 CONSTRAINT [PK_ScheduleDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedules]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](450) NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsPublish] [bit] NOT NULL,
	[ForecastId] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Schedules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[shiftRules]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[shiftRules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StartAfter] [int] NOT NULL,
	[EndBefore] [int] NOT NULL,
	[BreakBetween] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_shiftRules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shifts]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shifts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[EarlyStartIntervalId] [int] NULL,
	[LateEndIntervalId] [int] NULL,
	[Duration] [float] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Shifts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StaffMembers]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StaffMembers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[LeaveDate] [datetime2](7) NOT NULL,
	[HireDate] [datetime2](7) NOT NULL,
	[EstimatedLeaveDays] [int] NULL,
	[Religion] [nvarchar](max) NULL,
	[StaffTypeId] [int] NOT NULL,
	[TransportationRouteId] [int] NULL,
	[LocationId] [int] NULL,
	[HeadOfSectionId] [int] NOT NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[EmployeeId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Alias] [nvarchar](50) NOT NULL,
	[Gender] [nvarchar](max) NULL,
	[Email] [nvarchar](450) NULL,
	[Language] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_StaffMembers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StaffTypes]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StaffTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_StaffTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubLocations]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubLocations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[LocationId] [int] NOT NULL,
 CONSTRAINT [PK_SubLocations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwapRequestDetails]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwapRequestDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SwapRequestId] [int] NOT NULL,
	[InvolvedAlias] [nvarchar](max) NOT NULL,
	[IsApproved] [bit] NULL,
	[IsDeclined] [bit] NULL,
	[Reason] [nvarchar](max) NULL,
	[IssueDate] [datetime2](7) NOT NULL,
	[CloseDate] [datetime2](7) NULL,
 CONSTRAINT [PK_SwapRequestDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwapRequests]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwapRequests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScheduleId] [int] NULL,
	[SourceDailyAttendanceId] [int] NULL,
	[DestinationDailyAttendanceId] [int] NULL,
	[IssueDate] [datetime2](7) NOT NULL,
	[StatusId] [nvarchar](450) NULL,
	[RequesterAlias] [nvarchar](max) NOT NULL,
	[RequesterName] [nvarchar](max) NULL,
	[ResponderName] [nvarchar](max) NULL,
 CONSTRAINT [PK_SwapRequests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwapRequestStatuses]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwapRequestStatuses](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[IsComplete] [bit] NOT NULL,
	[IsSuccess] [bit] NOT NULL,
 CONSTRAINT [PK_SwapRequestStatuses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TmpForecastDetails]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TmpForecastDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TmpForecastId] [int] NOT NULL,
	[IntervalId] [int] NOT NULL,
	[DayoffWeek] [nvarchar](max) NULL,
	[EmployeeCount] [int] NOT NULL,
 CONSTRAINT [PK_TmpForecastDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TmpScheduleDetails]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TmpScheduleDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DailyAttendanceId] [int] NOT NULL,
	[ActivityId] [int] NOT NULL,
	[IntervalId] [int] NOT NULL,
	[Duration] [float] NOT NULL,
 CONSTRAINT [PK_TmpScheduleDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransportationRoutes]    Script Date: 11/29/2022 4:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransportationRoutes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[SubLocationId] [int] NULL,
	[ArriveIntervalId] [int] NOT NULL,
	[DepartIntervalId] [int] NULL,
	[IsIgnored] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_TransportationRoutes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20221127120851_initial', N'5.0.9')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20221129090212_sublocation', N'5.0.9')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20221129091117_newdatabase', N'5.0.9')
GO
SET IDENTITY_INSERT [dbo].[Intervals] ON 

INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (1, CAST(N'00:00:00' AS Time), 0, 0, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (2, CAST(N'00:15:00' AS Time), 1, 1, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (3, CAST(N'00:30:00' AS Time), 2, 2, 0.6)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (4, CAST(N'00:45:00' AS Time), 3, 3, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (5, CAST(N'01:00:00' AS Time), 4, 4, 1.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (6, CAST(N'01:15:00' AS Time), 5, 5, 0.9)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (7, CAST(N'01:30:00' AS Time), 6, 6, 1.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (8, CAST(N'01:45:00' AS Time), 7, 7, 1.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (9, CAST(N'02:00:00' AS Time), 8, 8, 0.9)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (10, CAST(N'02:15:00' AS Time), 9, 9, 0.8)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (11, CAST(N'02:30:00' AS Time), 10, 10, 2.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (12, CAST(N'02:45:00' AS Time), 11, 11, 2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (13, CAST(N'03:00:00' AS Time), 12, 12, 3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (14, CAST(N'03:15:00' AS Time), 13, 13, 2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (15, CAST(N'03:30:00' AS Time), 14, 14, 2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (16, CAST(N'03:45:00' AS Time), 15, 15, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (17, CAST(N'04:00:00' AS Time), 16, 16, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (18, CAST(N'04:15:00' AS Time), 17, 17, 1.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (19, CAST(N'04:30:00' AS Time), 18, 18, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (20, CAST(N'04:45:00' AS Time), 19, 19, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (21, CAST(N'05:00:00' AS Time), 20, 20, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (22, CAST(N'05:15:00' AS Time), 21, 21, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (23, CAST(N'05:30:00' AS Time), 22, 22, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (24, CAST(N'05:45:00' AS Time), 23, 23, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (25, CAST(N'06:00:00' AS Time), 24, 24, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (26, CAST(N'06:15:00' AS Time), 25, 25, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (27, CAST(N'06:30:00' AS Time), 26, 26, 1.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (28, CAST(N'06:45:00' AS Time), 27, 27, 2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (29, CAST(N'07:00:00' AS Time), 28, 28, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (30, CAST(N'07:15:00' AS Time), 29, 29, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (31, CAST(N'07:30:00' AS Time), 30, 30, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (32, CAST(N'07:45:00' AS Time), 31, 31, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (33, CAST(N'08:00:00' AS Time), 32, 32, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (34, CAST(N'08:15:00' AS Time), 33, 33, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (35, CAST(N'08:30:00' AS Time), 34, 34, 0.4)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (36, CAST(N'08:45:00' AS Time), 35, 35, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (37, CAST(N'09:00:00' AS Time), 36, 36, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (38, CAST(N'09:15:00' AS Time), 37, 37, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (39, CAST(N'09:30:00' AS Time), 38, 38, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (40, CAST(N'09:45:00' AS Time), 39, 39, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (41, CAST(N'10:00:00' AS Time), 40, 40, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (42, CAST(N'10:15:00' AS Time), 41, 41, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (43, CAST(N'10:30:00' AS Time), 42, 42, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (44, CAST(N'10:45:00' AS Time), 43, 43, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (45, CAST(N'11:00:00' AS Time), 44, 44, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (46, CAST(N'11:15:00' AS Time), 45, 45, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (47, CAST(N'11:30:00' AS Time), 46, 46, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (48, CAST(N'11:45:00' AS Time), 47, 47, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (49, CAST(N'12:00:00' AS Time), 48, 48, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (50, CAST(N'12:15:00' AS Time), 49, 49, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (51, CAST(N'12:30:00' AS Time), 50, 50, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (52, CAST(N'12:45:00' AS Time), 51, 51, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (53, CAST(N'13:00:00' AS Time), 52, 52, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (54, CAST(N'13:15:00' AS Time), 53, 53, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (55, CAST(N'13:30:00' AS Time), 54, 54, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (56, CAST(N'13:45:00' AS Time), 55, 55, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (57, CAST(N'14:00:00' AS Time), 56, 56, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (58, CAST(N'14:15:00' AS Time), 57, 57, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (59, CAST(N'14:30:00' AS Time), 58, 58, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (60, CAST(N'14:45:00' AS Time), 59, 59, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (61, CAST(N'15:00:00' AS Time), 60, 60, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (62, CAST(N'15:15:00' AS Time), 61, 61, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (63, CAST(N'15:30:00' AS Time), 62, 62, 0.7)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (64, CAST(N'15:45:00' AS Time), 63, 63, 0.6)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (65, CAST(N'16:00:00' AS Time), 64, 64, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (66, CAST(N'16:15:00' AS Time), 65, 65, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (67, CAST(N'16:30:00' AS Time), 66, 66, 0.4)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (68, CAST(N'16:45:00' AS Time), 67, 67, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (69, CAST(N'17:00:00' AS Time), 68, 68, 0.6)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (70, CAST(N'17:15:00' AS Time), 69, 69, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (71, CAST(N'17:30:00' AS Time), 70, 70, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (72, CAST(N'17:45:00' AS Time), 71, 71, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (73, CAST(N'18:00:00' AS Time), 72, 72, 0.4)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (74, CAST(N'18:15:00' AS Time), 73, 73, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (75, CAST(N'18:30:00' AS Time), 74, 74, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (76, CAST(N'18:45:00' AS Time), 75, 75, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (77, CAST(N'19:00:00' AS Time), 76, 76, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (78, CAST(N'19:15:00' AS Time), 77, 77, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (79, CAST(N'19:30:00' AS Time), 78, 78, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (80, CAST(N'19:45:00' AS Time), 79, 79, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (81, CAST(N'20:00:00' AS Time), 80, 80, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (82, CAST(N'20:15:00' AS Time), 81, 81, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (83, CAST(N'20:30:00' AS Time), 82, 82, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (84, CAST(N'20:45:00' AS Time), 83, 83, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (85, CAST(N'21:00:00' AS Time), 84, 84, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (86, CAST(N'21:15:00' AS Time), 85, 85, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (87, CAST(N'21:30:00' AS Time), 86, 86, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (88, CAST(N'21:45:00' AS Time), 87, 87, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (89, CAST(N'22:00:00' AS Time), 88, 88, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (90, CAST(N'22:15:00' AS Time), 89, 89, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (91, CAST(N'22:30:00' AS Time), 90, 90, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (92, CAST(N'22:45:00' AS Time), 91, 91, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (93, CAST(N'23:00:00' AS Time), 92, 92, 0.6)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (94, CAST(N'23:15:00' AS Time), 93, 93, 0.4)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (95, CAST(N'23:30:00' AS Time), 94, 94, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (96, CAST(N'23:45:00' AS Time), 95, 95, 0)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (97, CAST(N'00:00:00' AS Time), 96, 0, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (98, CAST(N'00:15:00' AS Time), 97, 1, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (99, CAST(N'00:30:00' AS Time), 98, 2, 0.6)
GO
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (100, CAST(N'00:45:00' AS Time), 99, 3, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (101, CAST(N'01:00:00' AS Time), 100, 4, 1.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (102, CAST(N'01:15:00' AS Time), 101, 5, 0.9)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (103, CAST(N'01:30:00' AS Time), 102, 6, 1.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (104, CAST(N'01:45:00' AS Time), 103, 7, 1.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (105, CAST(N'02:00:00' AS Time), 104, 8, 0.9)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (106, CAST(N'02:15:00' AS Time), 105, 9, 0.8)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (107, CAST(N'02:30:00' AS Time), 106, 10, 2.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (108, CAST(N'02:45:00' AS Time), 107, 11, 2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (109, CAST(N'03:00:00' AS Time), 108, 12, 3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (110, CAST(N'03:15:00' AS Time), 109, 13, 2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (111, CAST(N'03:30:00' AS Time), 110, 14, 2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (112, CAST(N'03:45:00' AS Time), 111, 15, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (113, CAST(N'04:00:00' AS Time), 112, 16, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (114, CAST(N'04:15:00' AS Time), 113, 17, 1.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (115, CAST(N'04:30:00' AS Time), 114, 18, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (116, CAST(N'04:45:00' AS Time), 115, 19, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (117, CAST(N'05:00:00' AS Time), 116, 20, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (118, CAST(N'05:15:00' AS Time), 117, 21, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (119, CAST(N'05:30:00' AS Time), 118, 22, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (120, CAST(N'05:45:00' AS Time), 119, 23, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (121, CAST(N'06:00:00' AS Time), 120, 24, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (122, CAST(N'06:15:00' AS Time), 121, 25, 1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (123, CAST(N'06:30:00' AS Time), 122, 26, 1.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (124, CAST(N'06:45:00' AS Time), 123, 27, 2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (125, CAST(N'07:00:00' AS Time), 124, 28, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (126, CAST(N'07:15:00' AS Time), 125, 29, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (127, CAST(N'07:30:00' AS Time), 126, 30, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (128, CAST(N'07:45:00' AS Time), 127, 31, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (129, CAST(N'08:00:00' AS Time), 128, 32, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (130, CAST(N'08:15:00' AS Time), 129, 33, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (131, CAST(N'08:30:00' AS Time), 130, 34, 0.4)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (132, CAST(N'08:45:00' AS Time), 131, 35, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (133, CAST(N'09:00:00' AS Time), 132, 36, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (134, CAST(N'09:15:00' AS Time), 133, 37, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (135, CAST(N'09:30:00' AS Time), 134, 38, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (136, CAST(N'09:45:00' AS Time), 135, 39, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (137, CAST(N'10:00:00' AS Time), 136, 40, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (138, CAST(N'10:15:00' AS Time), 137, 41, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (139, CAST(N'10:30:00' AS Time), 138, 42, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (140, CAST(N'10:45:00' AS Time), 139, 43, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (141, CAST(N'11:00:00' AS Time), 140, 44, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (142, CAST(N'11:15:00' AS Time), 141, 45, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (143, CAST(N'11:30:00' AS Time), 142, 46, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (144, CAST(N'11:45:00' AS Time), 143, 47, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (145, CAST(N'12:00:00' AS Time), 144, 48, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (146, CAST(N'12:15:00' AS Time), 145, 49, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (147, CAST(N'12:30:00' AS Time), 146, 50, 0.3)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (148, CAST(N'12:45:00' AS Time), 147, 51, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (149, CAST(N'13:00:00' AS Time), 148, 52, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (150, CAST(N'13:15:00' AS Time), 149, 53, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (151, CAST(N'13:30:00' AS Time), 150, 54, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (152, CAST(N'13:45:00' AS Time), 151, 55, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (153, CAST(N'14:00:00' AS Time), 152, 56, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (154, CAST(N'14:15:00' AS Time), 153, 57, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (155, CAST(N'14:30:00' AS Time), 154, 58, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (156, CAST(N'14:45:00' AS Time), 155, 59, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (157, CAST(N'15:00:00' AS Time), 156, 60, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (158, CAST(N'15:15:00' AS Time), 157, 61, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (159, CAST(N'15:30:00' AS Time), 158, 62, 0.7)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (160, CAST(N'15:45:00' AS Time), 159, 63, 0.6)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (161, CAST(N'16:00:00' AS Time), 160, 64, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (162, CAST(N'16:15:00' AS Time), 161, 65, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (163, CAST(N'16:30:00' AS Time), 162, 66, 0.4)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (164, CAST(N'16:45:00' AS Time), 163, 67, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (165, CAST(N'17:00:00' AS Time), 164, 68, 0.6)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (166, CAST(N'17:15:00' AS Time), 165, 69, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (167, CAST(N'17:30:00' AS Time), 166, 70, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (168, CAST(N'17:45:00' AS Time), 167, 71, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (169, CAST(N'18:00:00' AS Time), 168, 72, 0.4)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (170, CAST(N'18:15:00' AS Time), 169, 73, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (171, CAST(N'18:30:00' AS Time), 170, 74, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (172, CAST(N'18:45:00' AS Time), 171, 75, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (173, CAST(N'19:00:00' AS Time), 172, 76, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (174, CAST(N'19:15:00' AS Time), 173, 77, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (175, CAST(N'19:30:00' AS Time), 174, 78, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (176, CAST(N'19:45:00' AS Time), 175, 79, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (177, CAST(N'20:00:00' AS Time), 176, 80, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (178, CAST(N'20:15:00' AS Time), 177, 81, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (179, CAST(N'20:30:00' AS Time), 178, 82, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (180, CAST(N'20:45:00' AS Time), 179, 83, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (181, CAST(N'21:00:00' AS Time), 180, 84, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (182, CAST(N'21:15:00' AS Time), 181, 85, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (183, CAST(N'21:30:00' AS Time), 182, 86, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (184, CAST(N'21:45:00' AS Time), 183, 87, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (185, CAST(N'22:00:00' AS Time), 184, 88, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (186, CAST(N'22:15:00' AS Time), 185, 89, 0.2)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (187, CAST(N'22:30:00' AS Time), 186, 90, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (188, CAST(N'22:45:00' AS Time), 187, 91, 0.5)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (189, CAST(N'23:00:00' AS Time), 188, 92, 0.6)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (190, CAST(N'23:15:00' AS Time), 189, 93, 0.4)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (191, CAST(N'23:30:00' AS Time), 190, 94, 0.1)
INSERT [dbo].[Intervals] ([Id], [TimeMap], [OrderMap], [CoverMap], [Tolerance]) VALUES (192, CAST(N'23:45:00' AS Time), 191, 95, 0)
SET IDENTITY_INSERT [dbo].[Intervals] OFF
GO
SET IDENTITY_INSERT [dbo].[Locations] ON 

INSERT [dbo].[Locations] ([Id], [Name], [Address], [ContactPhone], [IsDeleted]) VALUES (1, N'Damascuse', N'syria', N'0993996440', 0)
INSERT [dbo].[Locations] ([Id], [Name], [Address], [ContactPhone], [IsDeleted]) VALUES (2, N'Hama', N'syria', N'0993996440', 0)
SET IDENTITY_INSERT [dbo].[Locations] OFF
GO
SET IDENTITY_INSERT [dbo].[SubLocations] ON 

INSERT [dbo].[SubLocations] ([Id], [Name], [LocationId]) VALUES (1, N'Sahnaia', 1)
INSERT [dbo].[SubLocations] ([Id], [Name], [LocationId]) VALUES (2, N'Alasi', 2)
INSERT [dbo].[SubLocations] ([Id], [Name], [LocationId]) VALUES (3, N'Damascuse_Jaramana_1/1/2022 12:00:00 AM_2/1/2022 12:00:00 AM', 1)
INSERT [dbo].[SubLocations] ([Id], [Name], [LocationId]) VALUES (5, N'Damascuse_Jaramana_1/1/2022 12:00:00 AM_2/1/2022 12:00:00 AM', 1)
SET IDENTITY_INSERT [dbo].[SubLocations] OFF
GO
SET IDENTITY_INSERT [dbo].[TransportationRoutes] ON 

INSERT [dbo].[TransportationRoutes] ([Id], [Name], [Description], [SubLocationId], [ArriveIntervalId], [DepartIntervalId], [IsIgnored], [IsDeleted]) VALUES (6, N'Hama_Alasi_51_75', NULL, 2, 51, 75, 0, 0)
SET IDENTITY_INSERT [dbo].[TransportationRoutes] OFF
GO
ALTER TABLE [dbo].[Assets]  WITH CHECK ADD  CONSTRAINT [FK_Assets_AssetTypes_AssetTypeId] FOREIGN KEY([AssetTypeId])
REFERENCES [dbo].[AssetTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Assets] CHECK CONSTRAINT [FK_Assets_AssetTypes_AssetTypeId]
GO
ALTER TABLE [dbo].[Assets]  WITH CHECK ADD  CONSTRAINT [FK_Assets_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Assets] CHECK CONSTRAINT [FK_Assets_Locations_LocationId]
GO
ALTER TABLE [dbo].[AssetTermValues]  WITH CHECK ADD  CONSTRAINT [FK_AssetTermValues_AssetTerms_AssetTermId] FOREIGN KEY([AssetTermId])
REFERENCES [dbo].[AssetTerms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AssetTermValues] CHECK CONSTRAINT [FK_AssetTermValues_AssetTerms_AssetTermId]
GO
ALTER TABLE [dbo].[AssetTermValues]  WITH CHECK ADD  CONSTRAINT [FK_AssetTermValues_AssetTypes_AssetTypeId] FOREIGN KEY([AssetTypeId])
REFERENCES [dbo].[AssetTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AssetTermValues] CHECK CONSTRAINT [FK_AssetTermValues_AssetTypes_AssetTypeId]
GO
ALTER TABLE [dbo].[AttendanceTypes]  WITH CHECK ADD  CONSTRAINT [FK_AttendanceTypes_Activities_DefaultActivityId] FOREIGN KEY([DefaultActivityId])
REFERENCES [dbo].[Activities] ([Id])
GO
ALTER TABLE [dbo].[AttendanceTypes] CHECK CONSTRAINT [FK_AttendanceTypes_Activities_DefaultActivityId]
GO
ALTER TABLE [dbo].[BkpDailyAttendances]  WITH CHECK ADD  CONSTRAINT [FK_BkpDailyAttendances_AttendanceTypes_AttendanceTypeId] FOREIGN KEY([AttendanceTypeId])
REFERENCES [dbo].[AttendanceTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BkpDailyAttendances] CHECK CONSTRAINT [FK_BkpDailyAttendances_AttendanceTypes_AttendanceTypeId]
GO
ALTER TABLE [dbo].[BkpScheduleDetails]  WITH CHECK ADD  CONSTRAINT [FK_BkpScheduleDetails_Activities_ActivityId] FOREIGN KEY([ActivityId])
REFERENCES [dbo].[Activities] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BkpScheduleDetails] CHECK CONSTRAINT [FK_BkpScheduleDetails_Activities_ActivityId]
GO
ALTER TABLE [dbo].[BkpScheduleDetails]  WITH CHECK ADD  CONSTRAINT [FK_BkpScheduleDetails_BkpDailyAttendances_DailyAttendanceId] FOREIGN KEY([DailyAttendanceId])
REFERENCES [dbo].[BkpDailyAttendances] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BkpScheduleDetails] CHECK CONSTRAINT [FK_BkpScheduleDetails_BkpDailyAttendances_DailyAttendanceId]
GO
ALTER TABLE [dbo].[BkpScheduleDetails]  WITH CHECK ADD  CONSTRAINT [FK_BkpScheduleDetails_Intervals_IntervalId] FOREIGN KEY([IntervalId])
REFERENCES [dbo].[Intervals] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BkpScheduleDetails] CHECK CONSTRAINT [FK_BkpScheduleDetails_Intervals_IntervalId]
GO
ALTER TABLE [dbo].[breakTypeOptions]  WITH CHECK ADD  CONSTRAINT [FK_breakTypeOptions_AttendanceTypes_AttendenceTypeId] FOREIGN KEY([AttendenceTypeId])
REFERENCES [dbo].[AttendanceTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[breakTypeOptions] CHECK CONSTRAINT [FK_breakTypeOptions_AttendanceTypes_AttendenceTypeId]
GO
ALTER TABLE [dbo].[breakTypeOptions]  WITH CHECK ADD  CONSTRAINT [FK_breakTypeOptions_Schedules_ScheduleId] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedules] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[breakTypeOptions] CHECK CONSTRAINT [FK_breakTypeOptions_Schedules_ScheduleId]
GO
ALTER TABLE [dbo].[breakTypeOptions]  WITH CHECK ADD  CONSTRAINT [FK_breakTypeOptions_StaffMembers_StaffMemberId] FOREIGN KEY([StaffMemberId])
REFERENCES [dbo].[StaffMembers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[breakTypeOptions] CHECK CONSTRAINT [FK_breakTypeOptions_StaffMembers_StaffMemberId]
GO
ALTER TABLE [dbo].[breakTypeOptions]  WITH CHECK ADD  CONSTRAINT [FK_breakTypeOptions_TransportationRoutes_TransportationRouteId] FOREIGN KEY([TransportationRouteId])
REFERENCES [dbo].[TransportationRoutes] ([Id])
GO
ALTER TABLE [dbo].[breakTypeOptions] CHECK CONSTRAINT [FK_breakTypeOptions_TransportationRoutes_TransportationRouteId]
GO
ALTER TABLE [dbo].[DailyAttendances]  WITH CHECK ADD  CONSTRAINT [FK_DailyAttendances_AttendanceTypes_AttendanceTypeId] FOREIGN KEY([AttendanceTypeId])
REFERENCES [dbo].[AttendanceTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DailyAttendances] CHECK CONSTRAINT [FK_DailyAttendances_AttendanceTypes_AttendanceTypeId]
GO
ALTER TABLE [dbo].[DailyAttendances]  WITH CHECK ADD  CONSTRAINT [FK_DailyAttendances_HeadOfSections_HeadOfSectionId] FOREIGN KEY([HeadOfSectionId])
REFERENCES [dbo].[HeadOfSections] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DailyAttendances] CHECK CONSTRAINT [FK_DailyAttendances_HeadOfSections_HeadOfSectionId]
GO
ALTER TABLE [dbo].[DailyAttendances]  WITH CHECK ADD  CONSTRAINT [FK_DailyAttendances_Schedules_ScheduleId] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedules] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DailyAttendances] CHECK CONSTRAINT [FK_DailyAttendances_Schedules_ScheduleId]
GO
ALTER TABLE [dbo].[DailyAttendances]  WITH CHECK ADD  CONSTRAINT [FK_DailyAttendances_StaffMembers_StaffMemberId] FOREIGN KEY([StaffMemberId])
REFERENCES [dbo].[StaffMembers] ([Id])
GO
ALTER TABLE [dbo].[DailyAttendances] CHECK CONSTRAINT [FK_DailyAttendances_StaffMembers_StaffMemberId]
GO
ALTER TABLE [dbo].[DailyAttendances]  WITH CHECK ADD  CONSTRAINT [FK_DailyAttendances_TransportationRoutes_TransportationRouteId] FOREIGN KEY([TransportationRouteId])
REFERENCES [dbo].[TransportationRoutes] ([Id])
GO
ALTER TABLE [dbo].[DailyAttendances] CHECK CONSTRAINT [FK_DailyAttendances_TransportationRoutes_TransportationRouteId]
GO
ALTER TABLE [dbo].[dayOffOptions]  WITH CHECK ADD  CONSTRAINT [FK_dayOffOptions_Schedules_ScheduleId] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedules] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[dayOffOptions] CHECK CONSTRAINT [FK_dayOffOptions_Schedules_ScheduleId]
GO
ALTER TABLE [dbo].[dayOffOptions]  WITH CHECK ADD  CONSTRAINT [FK_dayOffOptions_StaffMembers_StaffMemberId] FOREIGN KEY([StaffMemberId])
REFERENCES [dbo].[StaffMembers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[dayOffOptions] CHECK CONSTRAINT [FK_dayOffOptions_StaffMembers_StaffMemberId]
GO
ALTER TABLE [dbo].[ForecastDetails]  WITH CHECK ADD  CONSTRAINT [FK_ForecastDetails_Forecasts_ForecastId] FOREIGN KEY([ForecastId])
REFERENCES [dbo].[Forecasts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ForecastDetails] CHECK CONSTRAINT [FK_ForecastDetails_Forecasts_ForecastId]
GO
ALTER TABLE [dbo].[ForecastDetails]  WITH CHECK ADD  CONSTRAINT [FK_ForecastDetails_Intervals_IntervalId] FOREIGN KEY([IntervalId])
REFERENCES [dbo].[Intervals] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ForecastDetails] CHECK CONSTRAINT [FK_ForecastDetails_Intervals_IntervalId]
GO
ALTER TABLE [dbo].[ForecastHistories]  WITH CHECK ADD  CONSTRAINT [FK_ForecastHistories_Intervals_IntervalId] FOREIGN KEY([IntervalId])
REFERENCES [dbo].[Intervals] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ForecastHistories] CHECK CONSTRAINT [FK_ForecastHistories_Intervals_IntervalId]
GO
ALTER TABLE [dbo].[ForeCastings]  WITH CHECK ADD  CONSTRAINT [FK_ForeCastings_Intervals_IntervalId] FOREIGN KEY([IntervalId])
REFERENCES [dbo].[Intervals] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ForeCastings] CHECK CONSTRAINT [FK_ForeCastings_Intervals_IntervalId]
GO
ALTER TABLE [dbo].[ForeCastings]  WITH CHECK ADD  CONSTRAINT [FK_ForeCastings_Schedules_ScheduleId] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedules] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ForeCastings] CHECK CONSTRAINT [FK_ForeCastings_Schedules_ScheduleId]
GO
ALTER TABLE [dbo].[ScheduleDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleDetail_Activities_ActivityId] FOREIGN KEY([ActivityId])
REFERENCES [dbo].[Activities] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScheduleDetail] CHECK CONSTRAINT [FK_ScheduleDetail_Activities_ActivityId]
GO
ALTER TABLE [dbo].[ScheduleDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleDetail_DailyAttendances_DailyAttendanceId] FOREIGN KEY([DailyAttendanceId])
REFERENCES [dbo].[DailyAttendances] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScheduleDetail] CHECK CONSTRAINT [FK_ScheduleDetail_DailyAttendances_DailyAttendanceId]
GO
ALTER TABLE [dbo].[ScheduleDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleDetail_Intervals_IntervalId] FOREIGN KEY([IntervalId])
REFERENCES [dbo].[Intervals] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScheduleDetail] CHECK CONSTRAINT [FK_ScheduleDetail_Intervals_IntervalId]
GO
ALTER TABLE [dbo].[ScheduleDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleDetail_Schedules_ScheduleId] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedules] ([Id])
GO
ALTER TABLE [dbo].[ScheduleDetail] CHECK CONSTRAINT [FK_ScheduleDetail_Schedules_ScheduleId]
GO
ALTER TABLE [dbo].[ScheduleDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleDetail_StaffMembers_BackupStaffId] FOREIGN KEY([BackupStaffId])
REFERENCES [dbo].[StaffMembers] ([Id])
GO
ALTER TABLE [dbo].[ScheduleDetail] CHECK CONSTRAINT [FK_ScheduleDetail_StaffMembers_BackupStaffId]
GO
ALTER TABLE [dbo].[Shifts]  WITH CHECK ADD  CONSTRAINT [FK_Shifts_Intervals_EarlyStartIntervalId] FOREIGN KEY([EarlyStartIntervalId])
REFERENCES [dbo].[Intervals] ([Id])
GO
ALTER TABLE [dbo].[Shifts] CHECK CONSTRAINT [FK_Shifts_Intervals_EarlyStartIntervalId]
GO
ALTER TABLE [dbo].[Shifts]  WITH CHECK ADD  CONSTRAINT [FK_Shifts_Intervals_LateEndIntervalId] FOREIGN KEY([LateEndIntervalId])
REFERENCES [dbo].[Intervals] ([Id])
GO
ALTER TABLE [dbo].[Shifts] CHECK CONSTRAINT [FK_Shifts_Intervals_LateEndIntervalId]
GO
ALTER TABLE [dbo].[StaffMembers]  WITH CHECK ADD  CONSTRAINT [FK_StaffMembers_HeadOfSections_HeadOfSectionId] FOREIGN KEY([HeadOfSectionId])
REFERENCES [dbo].[HeadOfSections] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StaffMembers] CHECK CONSTRAINT [FK_StaffMembers_HeadOfSections_HeadOfSectionId]
GO
ALTER TABLE [dbo].[StaffMembers]  WITH CHECK ADD  CONSTRAINT [FK_StaffMembers_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[StaffMembers] CHECK CONSTRAINT [FK_StaffMembers_Locations_LocationId]
GO
ALTER TABLE [dbo].[StaffMembers]  WITH CHECK ADD  CONSTRAINT [FK_StaffMembers_StaffTypes_StaffTypeId] FOREIGN KEY([StaffTypeId])
REFERENCES [dbo].[StaffTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StaffMembers] CHECK CONSTRAINT [FK_StaffMembers_StaffTypes_StaffTypeId]
GO
ALTER TABLE [dbo].[StaffMembers]  WITH CHECK ADD  CONSTRAINT [FK_StaffMembers_TransportationRoutes_TransportationRouteId] FOREIGN KEY([TransportationRouteId])
REFERENCES [dbo].[TransportationRoutes] ([Id])
GO
ALTER TABLE [dbo].[StaffMembers] CHECK CONSTRAINT [FK_StaffMembers_TransportationRoutes_TransportationRouteId]
GO
ALTER TABLE [dbo].[SubLocations]  WITH CHECK ADD  CONSTRAINT [FK_SubLocations_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubLocations] CHECK CONSTRAINT [FK_SubLocations_Locations_LocationId]
GO
ALTER TABLE [dbo].[SwapRequestDetails]  WITH CHECK ADD  CONSTRAINT [FK_SwapRequestDetails_SwapRequests_SwapRequestId] FOREIGN KEY([SwapRequestId])
REFERENCES [dbo].[SwapRequests] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SwapRequestDetails] CHECK CONSTRAINT [FK_SwapRequestDetails_SwapRequests_SwapRequestId]
GO
ALTER TABLE [dbo].[SwapRequests]  WITH CHECK ADD  CONSTRAINT [FK_SwapRequests_DailyAttendances_DestinationDailyAttendanceId] FOREIGN KEY([DestinationDailyAttendanceId])
REFERENCES [dbo].[DailyAttendances] ([Id])
GO
ALTER TABLE [dbo].[SwapRequests] CHECK CONSTRAINT [FK_SwapRequests_DailyAttendances_DestinationDailyAttendanceId]
GO
ALTER TABLE [dbo].[SwapRequests]  WITH CHECK ADD  CONSTRAINT [FK_SwapRequests_DailyAttendances_SourceDailyAttendanceId] FOREIGN KEY([SourceDailyAttendanceId])
REFERENCES [dbo].[DailyAttendances] ([Id])
GO
ALTER TABLE [dbo].[SwapRequests] CHECK CONSTRAINT [FK_SwapRequests_DailyAttendances_SourceDailyAttendanceId]
GO
ALTER TABLE [dbo].[SwapRequests]  WITH CHECK ADD  CONSTRAINT [FK_SwapRequests_Schedules_ScheduleId] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedules] ([Id])
GO
ALTER TABLE [dbo].[SwapRequests] CHECK CONSTRAINT [FK_SwapRequests_Schedules_ScheduleId]
GO
ALTER TABLE [dbo].[SwapRequests]  WITH CHECK ADD  CONSTRAINT [FK_SwapRequests_SwapRequestStatuses_StatusId] FOREIGN KEY([StatusId])
REFERENCES [dbo].[SwapRequestStatuses] ([Id])
GO
ALTER TABLE [dbo].[SwapRequests] CHECK CONSTRAINT [FK_SwapRequests_SwapRequestStatuses_StatusId]
GO
ALTER TABLE [dbo].[TmpForecastDetails]  WITH CHECK ADD  CONSTRAINT [FK_TmpForecastDetails_Intervals_IntervalId] FOREIGN KEY([IntervalId])
REFERENCES [dbo].[Intervals] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TmpForecastDetails] CHECK CONSTRAINT [FK_TmpForecastDetails_Intervals_IntervalId]
GO
ALTER TABLE [dbo].[TmpScheduleDetails]  WITH CHECK ADD  CONSTRAINT [FK_TmpScheduleDetails_Activities_ActivityId] FOREIGN KEY([ActivityId])
REFERENCES [dbo].[Activities] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TmpScheduleDetails] CHECK CONSTRAINT [FK_TmpScheduleDetails_Activities_ActivityId]
GO
ALTER TABLE [dbo].[TmpScheduleDetails]  WITH CHECK ADD  CONSTRAINT [FK_TmpScheduleDetails_DailyAttendances_DailyAttendanceId] FOREIGN KEY([DailyAttendanceId])
REFERENCES [dbo].[DailyAttendances] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TmpScheduleDetails] CHECK CONSTRAINT [FK_TmpScheduleDetails_DailyAttendances_DailyAttendanceId]
GO
ALTER TABLE [dbo].[TmpScheduleDetails]  WITH CHECK ADD  CONSTRAINT [FK_TmpScheduleDetails_Intervals_IntervalId] FOREIGN KEY([IntervalId])
REFERENCES [dbo].[Intervals] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TmpScheduleDetails] CHECK CONSTRAINT [FK_TmpScheduleDetails_Intervals_IntervalId]
GO
ALTER TABLE [dbo].[TransportationRoutes]  WITH CHECK ADD  CONSTRAINT [FK_TransportationRoutes_Intervals_ArriveIntervalId] FOREIGN KEY([ArriveIntervalId])
REFERENCES [dbo].[Intervals] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TransportationRoutes] CHECK CONSTRAINT [FK_TransportationRoutes_Intervals_ArriveIntervalId]
GO
ALTER TABLE [dbo].[TransportationRoutes]  WITH CHECK ADD  CONSTRAINT [FK_TransportationRoutes_Intervals_DepartIntervalId] FOREIGN KEY([DepartIntervalId])
REFERENCES [dbo].[Intervals] ([Id])
GO
ALTER TABLE [dbo].[TransportationRoutes] CHECK CONSTRAINT [FK_TransportationRoutes_Intervals_DepartIntervalId]
GO
ALTER TABLE [dbo].[TransportationRoutes]  WITH CHECK ADD  CONSTRAINT [FK_TransportationRoutes_SubLocations_SubLocationId] FOREIGN KEY([SubLocationId])
REFERENCES [dbo].[SubLocations] ([Id])
GO
ALTER TABLE [dbo].[TransportationRoutes] CHECK CONSTRAINT [FK_TransportationRoutes_SubLocations_SubLocationId]
GO
