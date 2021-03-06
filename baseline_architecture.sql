USE [master]
GO
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'BASELINE_DATABASE')
	DROP DATABASE BASELINE_DATABASE;
GO
/****** Object:  Database [putsa_db]    Script Date: 2015-12-11 16:33:45 ******/
CREATE DATABASE [BASELINE_DATABASE]
 CONTAINMENT = NONE
COLLATE SQL_Latin1_General_CP1_CI_AS
GO
ALTER DATABASE [BASELINE_DATABASE] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BASELINE_DATABASE].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BASELINE_DATABASE] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET ARITHABORT OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BASELINE_DATABASE] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BASELINE_DATABASE] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BASELINE_DATABASE] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BASELINE_DATABASE] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BASELINE_DATABASE] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [BASELINE_DATABASE] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BASELINE_DATABASE] SET  MULTI_USER 
GO
ALTER DATABASE [BASELINE_DATABASE] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BASELINE_DATABASE] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BASELINE_DATABASE] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BASELINE_DATABASE] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [BASELINE_DATABASE] SET DELAYED_DURABILITY = DISABLED 
GO
USE [BASELINE_DATABASE]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[__TransactionHistory]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__TransactionHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.__TransactionHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[2013-09-18 Gatuadresser-postnummer]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[2013-09-18 Gatuadresser-postnummer](
	[Column 0] [varchar](50) NULL,
	[Column 1] [varchar](50) NULL,
	[Column 2] [varchar](50) NULL,
	[Column 3] [varchar](50) NULL,
	[Column 4] [varchar](50) NULL,
	[Column 5] [varchar](50) NULL,
	[Column 6] [varchar](50) NULL,
	[Column 7] [varchar](50) NULL,
	[Column 8] [varchar](50) NULL,
	[Column 9] [varchar](50) NULL,
	[Column 10] [varchar](50) NULL,
	[Column 11] [varchar](50) NULL,
	[Column 12] [varchar](50) NULL,
	[Column 13] [varchar](50) NULL,
	[Column 14] [varchar](50) NULL,
	[Column 15] [varchar](50) NULL,
	[Column 16] [varchar](50) NULL,
	[Column 17] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[AccountNumber] [int] NOT NULL,
	[Designation] [nvarchar](max) NULL,
	[AccountType] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Accounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Banks]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BankId] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Banks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BGMAXFileHistories]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BGMAXFileHistories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BGMAXFilename] [nvarchar](max) NULL,
	[BGMAXTimestamp] [nvarchar](max) NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.BGMAXFileHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BGMAXPaymentInvoiceConnections]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BGMAXPaymentInvoiceConnections](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BGMAXFilename] [nvarchar](max) NULL,
	[BGMAXTimestamp] [nvarchar](max) NULL,
	[AutomaticallyConnected] [bit] NOT NULL,
	[InvoiceId] [int] NULL,
	[SenderBGNr] [nvarchar](max) NULL,
	[ProvidedOCR] [nvarchar](max) NULL,
	[PayedAmount] [decimal](18, 2) NOT NULL,
	[BGCLpNr] [nvarchar](max) NULL,
	[InformationText] [nvarchar](max) NULL,
	[PayersName] [nvarchar](max) NULL,
	[PayersNameExtra] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[ZipCode] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[Country] [nvarchar](max) NULL,
	[CountryCode] [nvarchar](max) NULL,
	[CorporateNumber] [nvarchar](max) NULL,
	[DeductionCode] [nvarchar](max) NULL,
	[DebitAccountId] [int] NOT NULL,
	[CreditAccountId] [int] NOT NULL,
	[PaymentType] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.BGMAXPaymentInvoiceConnections] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CleaningObjectPrices]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CleaningObjectPrices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Modification] [float] NOT NULL,
	[CleaningObjectId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
	[FreeText] [nvarchar](max) NULL,
	[ServiceGroupId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CleaningObjectPrices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CleaningObjects]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CleaningObjects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Invoicable] [bit] NOT NULL,
	[OtherInfo] [nvarchar](max) NULL,
	[CustomerId] [int] NULL,
	[TeamId] [int] NULL,
	[PostalAddressModelId] [int] NOT NULL,
	[AppartmentNo] [nvarchar](max) NULL,
	[GateCode] [nvarchar](max) NULL,
	[AlarmCode] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[IsNew] [bit] NOT NULL,
	[InfoBeforeCleaning] [nvarchar](max) NULL,
	[InfoDuringCleaning] [nvarchar](max) NULL,
	[InfoAfterCleaning] [nvarchar](max) NULL,
	[InvoiceReference] [nvarchar](max) NULL,
	[RouteIndex] [int] NULL,
 CONSTRAINT [PK_dbo.CleaningObjects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RUT] [decimal](18, 2) NOT NULL,
	[InvoiceReference] [bit] NOT NULL,
	[Notify] [bit] NOT NULL,
	[PersonId] [int] NOT NULL,
	[CleaningObjectId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Contacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customers]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentTerms] [int] NOT NULL,
	[InvoiceMethod] [int] NOT NULL,
	[IsInvoicable] [bit] NOT NULL,
	[IsInactive] [bit] NOT NULL,
	[IsCreditBlocked] [bit] NOT NULL,
	[PersonId] [int] NOT NULL,
	[BankId] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomPATypeConns]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomPATypeConns](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomPostalAddressTypeId] [int] NOT NULL,
	[PersonPostalAddressModelId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CustomPATypeConns] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomPostalAddressTypes]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomPostalAddressTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.CustomPostalAddressTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Deviations]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deviations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Start] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[IsNotAvailable] [bit] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[VehicleId] [int] NULL,
	[WorkerId] [int] NULL,
	[TeamId] [int] NULL,
 CONSTRAINT [PK_dbo.Deviations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InvoiceContacts]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceContacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceId] [int] NOT NULL,
	[RUT] [decimal](18, 2) NOT NULL,
	[PersonalNo] [nvarchar](max) NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[MobilePhone] [nvarchar](max) NULL,
	[WorkPhone] [nvarchar](max) NULL,
	[PrivatePhone] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[CompanyName] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[PostalCode] [nvarchar](max) NULL,
	[PostalAddress] [nvarchar](max) NULL,
	[PostalAddress2] [nvarchar](max) NULL,
	[StreetNo] [nvarchar](max) NULL,
	[NoPersonalNoValidation] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.InvoiceContacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InvoiceRows]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceRows](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceId] [int] NOT NULL,
	[ArticleText] [nvarchar](max) NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[Count] [decimal](18, 2) NOT NULL,
	[DeliveryDate] [datetime] NOT NULL,
	[RUT] [bit] NOT NULL,
	[Vat] [bit] NOT NULL,
	[AccountId] [int] NOT NULL,
	[ServiceCategory] [int] NOT NULL,
	[TransactionId] [int] NULL,
 CONSTRAINT [PK_dbo.InvoiceRows] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Invoices]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [int] NOT NULL,
	[InvoiceOCR] [nvarchar](max) NULL,
	[CustomerId] [int] NOT NULL,
	[CustomerName] [nvarchar](max) NOT NULL,
	[CustomerPersonNumber] [nvarchar](max) NULL,
	[CustomerInvoiceTown] [nvarchar](max) NOT NULL,
	[CustomerInvoicePostalCode] [nvarchar](max) NOT NULL,
	[CustomerInvoicePostalAddress] [nvarchar](max) NOT NULL,
	[CustomerInvoicePostalAddress2] [nvarchar](max) NULL,
	[CustomerDeliveryTown] [nvarchar](max) NOT NULL,
	[CustomerDeliveryPostalCode] [nvarchar](max) NOT NULL,
	[CustomerDeliveryPostalAddress] [nvarchar](max) NOT NULL,
	[CustomerInvoiceMethod] [int] NOT NULL,
	[CustomerEmail] [nvarchar](max) NULL,
	[RUTCustomer] [bit] NOT NULL,
	[CustomerPersonType] [int] NOT NULL,
	[CustomerPaymentTerms] [int] NOT NULL,
	[ApprovedDate] [datetime] NOT NULL,
	[InvoiceDate] [datetime] NOT NULL,
	[WorkCost] [decimal](18, 2) NOT NULL,
	[ReferenceInvoiceId] [int] NULL,
	[Period] [int] NOT NULL,
	[CleaningObjectId] [int] NULL,
	[WorkOrderId] [int] NULL,
	[NoPersonalNoValidation] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Invoices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IssueHistories]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IssueHistories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IssueId] [int] NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Changed] [datetime] NOT NULL,
	[Assignee_Id] [int] NULL,
	[User_Id] [int] NULL,
	[IssueType] [int] NOT NULL,
 CONSTRAINT [PK_dbo.IssueHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Issues]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Issues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[FinishedDate] [datetime] NULL,
	[Private] [bit] NOT NULL,
	[CustomerId] [int] NULL,
	[AssigneeId] [int] NULL,
	[CleaningObjectId] [int] NULL,
	[CreatorId] [int] NULL,
	[IssueType] [int] NOT NULL,
	[InvoiceId] [int] NULL,
	[WorkOrderId] [int] NULL,
 CONSTRAINT [PK_dbo.Issues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NextInvoiceNumbers]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NextInvoiceNumbers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NextAvailableInvoiceNumber] [int] NOT NULL,
 CONSTRAINT [PK_dbo.NextInvoiceNumbers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Periods]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Periods](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WeekFrom] [int] NOT NULL,
	[WeekTo] [int] NOT NULL,
	[ValidFrom] [datetime] NOT NULL,
	[ValidForYear] [int] NOT NULL,
	[ScheduleId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Periods] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PersonPostalAddressModels]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonPostalAddressModels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[PersonId] [int] NOT NULL,
	[PostalAddressModelId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.PersonPostalAddressModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Persons]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonalNo] [nvarchar](max) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[MobilePhone] [nvarchar](max) NULL,
	[WorkPhone] [nvarchar](max) NULL,
	[PrivatePhone] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[PersonType] [int] NOT NULL,
	[CompanyName] [nvarchar](max) NULL,
	[NoPersonalNoValidation] [bit] NOT NULL,
	[NoTaxReductionAfter] [datetime] NULL,
 CONSTRAINT [PK_dbo.Persons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PostalAddressModels]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostalAddressModels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StreetNo] [nvarchar](max) NULL,
	[AddressType] [nvarchar](max) NULL,
	[AppartmentNo] [int] NULL,
	[Address2] [nvarchar](max) NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[PostalCodeModelId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.PostalAddressModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PostalCodeModels]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostalCodeModels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostalCode] [nvarchar](10) NULL,
	[PostalCodeType] [nvarchar](max) NULL,
	[PostalAddress] [nvarchar](50) NULL,
	[StreetNoLowest] [nvarchar](max) NULL,
	[StreetNoHighest] [nvarchar](max) NULL,
	[City] [nvarchar](40) NULL,
	[TypeOfPlacement] [nvarchar](max) NULL,
	[StateCode] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[MunicipalityCode] [nvarchar](max) NULL,
	[Municipality] [nvarchar](max) NULL,
	[ParishCode] [nvarchar](max) NULL,
	[Parish] [nvarchar](max) NULL,
	[GateLowest] [nvarchar](max) NULL,
	[GateHighest] [nvarchar](max) NULL,
	[City2] [nvarchar](max) NULL,
	[IsNotValid] [bit] NOT NULL,
	[ScheduleId] [int] NULL,
 CONSTRAINT [PK_dbo.PostalCodeModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Prices]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Cost] [int] NOT NULL,
	[ServiceGroupId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Prices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Schedules]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[_ValidFrom] [datetime] NOT NULL,
	[_ValidTo] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Schedules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServiceGroups]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceGroups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[From] [datetime] NOT NULL,
	[To] [datetime] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.ServiceGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Services]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Category] [int] NOT NULL,
	[RUT] [bit] NOT NULL,
	[Vat] [bit] NOT NULL,
	[AccountId] [int] NOT NULL,
	[CalcPercentage] [bit] NOT NULL,
	[CalcHourly] [bit] NOT NULL,
	[CalcArea] [bit] NOT NULL,
	[CalcNone] [bit] NOT NULL,
	[From] [bit] NOT NULL,
	[Circa] [bit] NOT NULL,
	[VisibleOnInvoice] [bit] NOT NULL,
	[IsSalarySetting] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[SubCategoryId] [int] NULL,
	[SettableByCleaners] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Services] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Settings]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[Blob] [varbinary](max) NULL,
 CONSTRAINT [PK_dbo.Settings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SingleCleanings]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SingleCleanings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateFrom] [datetime] NOT NULL,
	[DateTo] [datetime] NOT NULL,
	[Info] [nvarchar](max) NULL,
	[CleaningObjectId] [int] NOT NULL,
	[TeamId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SingleCleanings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SingleCleaningServices]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SingleCleaningServices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NOT NULL,
	[SingleCleaningId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SingleCleaningServices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SubCategories]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Category] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SubCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Subscriptions]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscriptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CleaningObjectId] [int] NOT NULL,
	[IsInactive] [bit] NOT NULL,
	[ActiveChangedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Subscriptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SubscriptionServices]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubscriptionServices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubscriptionId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
	[PeriodNo] [int] NOT NULL,
	[IsPermanent] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.SubscriptionServices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SubscriptionServiceStatus]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubscriptionServiceStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubscriptionServiceId] [int] NOT NULL,
	[SubscriptionServiceStatusDescriptionId] [int] NOT NULL,
	[Year] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SubscriptionServiceStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SubscriptionServiceStatusDescriptions]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubscriptionServiceStatusDescriptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SubscriptionServiceStatusDescriptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemLogs]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[SystemLogType] [int] NOT NULL,
	[LogText] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SystemLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TableLocks]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TableLocks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Table] [nvarchar](max) NULL,
	[IsLocked] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.TableLocks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Teams]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teams](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[WorkerLimit] [int] NOT NULL,
	[VehicleId] [int] NULL,
 CONSTRAINT [PK_dbo.Teams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TempOrders]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TempOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceId] [int] NOT NULL,
	[TransactionNmbr] [int] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[EventText] [nvarchar](max) NULL,
	[TransactionType] [int] NOT NULL,
	[ReferenseType] [int] NOT NULL,
	[Reference1] [nvarchar](max) NULL,
	[Reference2] [nvarchar](max) NULL,
	[PersonalNo] [nvarchar](max) NULL,
	[UserId] [int] NOT NULL,
	[DueDate] [datetime] NULL,
	[AccountingDate] [datetime] NULL,
	[TaxReductionDecided] [bit] NOT NULL,
	[InvoiceFreeText] [nvarchar](max) NULL,
	[LinkedInvoiceId] [int] NULL,
 CONSTRAINT [PK_dbo.Transactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TransactionValues]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionValues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
	[Value] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_dbo.TransactionValues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsedTaxReductionRequestNumbers]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsedTaxReductionRequestNumbers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[UsedNumber] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UsedTaxReductionRequestNumbers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[Permissions] [int] NOT NULL,
	[TeamId] [int] NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleHistories]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleHistories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateEntered] [datetime] NOT NULL,
	[Kilometers] [int] NOT NULL,
	[VehicleId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.VehicleHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vehicles]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegNo] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[AgreementStartDate] [datetime] NULL,
	[AgreementEndDate] [datetime] NULL,
	[Status] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[Brand] [nvarchar](max) NULL,
	[VehicleModel] [nvarchar](max) NULL,
	[ManufacturingYear] [int] NULL,
 CONSTRAINT [PK_dbo.Vehicles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Workers]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Workers](
	[UserId] [int] NOT NULL,
	[PersonalNo] [nvarchar](max) NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[WorkPhone] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Zip] [nvarchar](max) NULL,
	[PrivatePhone] [nvarchar](max) NULL,
	[PrivateEMail] [nvarchar](max) NULL,
	[ClosestContactName] [nvarchar](max) NULL,
	[ClosestContactPhone] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[curRole] [nvarchar](max) NULL,
	[StartDate] [datetime] NOT NULL,
	[CurActive] [bit] NOT NULL,
	[EndDate] [datetime] NULL,
	[TeamId] [int] NULL,
 CONSTRAINT [PK_dbo.Workers] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WorkOrderResources]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderResources](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderId] [int] NOT NULL,
	[TeamId] [int] NOT NULL,
	[VehicleId] [int] NOT NULL,
	[ResourceRegisteredDate] [datetime] NOT NULL,
	[VehicleMileageStart] [int] NULL,
	[VehicleMileageEnd] [int] NULL,
 CONSTRAINT [PK_dbo.WorkOrderResources] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WorkOrderResourceWorkers]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderResourceWorkers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderResourceId] [int] NOT NULL,
	[WorkerUserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.WorkOrderResourceWorkers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WorkOrders]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Period] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[RouteIndex] [int] NOT NULL,
	[Changed] [bit] NOT NULL,
	[FinishedDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[TodoBeforeFinished] [bit] NULL,
	[CleaningObjectId] [int] NOT NULL,
	[StartDate] [datetime] NULL,
	[TimeBooked] [bit] NOT NULL,
	[SingleCleaningType] [int] NOT NULL,
	[CustomerIsNew] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.WorkOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WorkOrderServices]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderServices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OriginalCost] [int] NOT NULL,
	[ModifiedCost] [int] NULL,
	[TempAdded] [bit] NOT NULL,
	[TempRemoved] [bit] NOT NULL,
	[TempPermanent] [bit] NOT NULL,
	[WorkOrderId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
	[Count] [decimal](18, 2) NOT NULL,
	[SubscriptionServiceStatus] [int] NOT NULL,
 CONSTRAINT [PK_dbo.WorkOrderServices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WorkOrderTimeReports]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderTimeReports](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[From] [datetime] NOT NULL,
	[To] [datetime] NULL,
	[WorkOrderId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.WorkOrderTimeReports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_UserId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[BGMAXFileHistories]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceId] ON [dbo].[BGMAXPaymentInvoiceConnections]
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[BGMAXPaymentInvoiceConnections]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CleaningObjectId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_CleaningObjectId] ON [dbo].[CleaningObjectPrices]
(
	[CleaningObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceGroupId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_ServiceGroupId] ON [dbo].[CleaningObjectPrices]
(
	[ServiceGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_ServiceId] ON [dbo].[CleaningObjectPrices]
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CustomerId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_CustomerId] ON [dbo].[CleaningObjects]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PostalAddressModelId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_PostalAddressModelId] ON [dbo].[CleaningObjects]
(
	[PostalAddressModelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TeamId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_TeamId] ON [dbo].[CleaningObjects]
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CleaningObjectId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_CleaningObjectId] ON [dbo].[Contacts]
(
	[CleaningObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PersonId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_PersonId] ON [dbo].[Contacts]
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BankId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_BankId] ON [dbo].[Customers]
(
	[BankId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PersonId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_PersonId] ON [dbo].[Customers]
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CustomPostalAddressTypeId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_CustomPostalAddressTypeId] ON [dbo].[CustomPATypeConns]
(
	[CustomPostalAddressTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PersonPostalAddressModelId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_PersonPostalAddressModelId] ON [dbo].[CustomPATypeConns]
(
	[PersonPostalAddressModelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TeamId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_TeamId] ON [dbo].[Deviations]
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_VehicleId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_VehicleId] ON [dbo].[Deviations]
(
	[VehicleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkerId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_WorkerId] ON [dbo].[Deviations]
(
	[WorkerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceId] ON [dbo].[InvoiceContacts]
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceId] ON [dbo].[InvoiceRows]
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionId] ON [dbo].[InvoiceRows]
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CleaningObjectId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_CleaningObjectId] ON [dbo].[Invoices]
(
	[CleaningObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ReferenceInvoiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_ReferenceInvoiceId] ON [dbo].[Invoices]
(
	[ReferenceInvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrderId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrderId] ON [dbo].[Invoices]
(
	[WorkOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Assignee_Id]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_Assignee_Id] ON [dbo].[IssueHistories]
(
	[Assignee_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_User_Id]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_User_Id] ON [dbo].[IssueHistories]
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IssueTypeIx]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IssueTypeIx] ON [dbo].[Issues]
(
	[IssueType] ASC
)
INCLUDE ( 	[Id],
	[Title],
	[Description],
	[Status],
	[Priority],
	[StartDate],
	[FinishedDate],
	[Private],
	[CustomerId],
	[CleaningObjectId],
	[CreatorId],
	[InvoiceId],
	[WorkOrderId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AssigneeId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_AssigneeId] ON [dbo].[Issues]
(
	[AssigneeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CleaningObjectId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_CleaningObjectId] ON [dbo].[Issues]
(
	[CleaningObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatorId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_CreatorId] ON [dbo].[Issues]
(
	[CreatorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CustomerId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_CustomerId] ON [dbo].[Issues]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceId] ON [dbo].[Issues]
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrderId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrderId] ON [dbo].[Issues]
(
	[WorkOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ScheduleId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_ScheduleId] ON [dbo].[Periods]
(
	[ScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PersonId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_PersonId] ON [dbo].[PersonPostalAddressModels]
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PostalAddressModelId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_PostalAddressModelId] ON [dbo].[PersonPostalAddressModels]
(
	[PostalAddressModelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [TypeIX]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [TypeIX] ON [dbo].[PersonPostalAddressModels]
(
	[Type] ASC,
	[Id] ASC,
	[PersonId] ASC,
	[PostalAddressModelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [FullNamnIx]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [FullNamnIx] ON [dbo].[Persons]
(
	[FirstName] ASC,
	[LastName] ASC
)
INCLUDE ( 	[Id],
	[PersonalNo],
	[MobilePhone],
	[WorkPhone],
	[Email],
	[CompanyName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [LastNameIx]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [LastNameIx] ON [dbo].[Persons]
(
	[LastName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [PersonTypeIX]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [PersonTypeIX] ON [dbo].[Persons]
(
	[PersonType] ASC
)
INCLUDE ( 	[Id],
	[PersonalNo],
	[FirstName],
	[LastName],
	[MobilePhone],
	[WorkPhone],
	[PrivatePhone],
	[CompanyName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PostalCodeModelId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_PostalCodeModelId] ON [dbo].[PostalAddressModels]
(
	[PostalCodeModelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [CityIx]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [CityIx] ON [dbo].[PostalCodeModels]
(
	[City] ASC
)
INCLUDE ( 	[Id],
	[PostalCode],
	[PostalAddress]) WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [FullAddressIx]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [FullAddressIx] ON [dbo].[PostalCodeModels]
(
	[PostalCode] ASC,
	[PostalAddress] ASC,
	[City] ASC
)
INCLUDE ( 	[Id],
	[ScheduleId]) WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
GO
/****** Object:  Index [IX_ScheduleId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_ScheduleId] ON [dbo].[PostalCodeModels]
(
	[ScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [PostalAddressIx]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [PostalAddressIx] ON [dbo].[PostalCodeModels]
(
	[PostalAddress] ASC
)
INCLUDE ( 	[Id],
	[PostalCode],
	[City],
	[ScheduleId]) WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [PostalCodeIx]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [PostalCodeIx] ON [dbo].[PostalCodeModels]
(
	[PostalCode] ASC
)
INCLUDE ( 	[Id],
	[PostalAddress],
	[City]) WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceGroupId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_ServiceGroupId] ON [dbo].[Prices]
(
	[ServiceGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_ServiceId] ON [dbo].[Prices]
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AccountId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_AccountId] ON [dbo].[Services]
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_SubCategoryId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_SubCategoryId] ON [dbo].[Services]
(
	[SubCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CleaningObjectId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_CleaningObjectId] ON [dbo].[SingleCleanings]
(
	[CleaningObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TeamId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_TeamId] ON [dbo].[SingleCleanings]
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_ServiceId] ON [dbo].[SingleCleaningServices]
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_SingleCleaningId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_SingleCleaningId] ON [dbo].[SingleCleaningServices]
(
	[SingleCleaningId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CleaningObjectId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_CleaningObjectId] ON [dbo].[Subscriptions]
(
	[CleaningObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_ServiceId] ON [dbo].[SubscriptionServices]
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_SubscriptionId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_SubscriptionId] ON [dbo].[SubscriptionServices]
(
	[SubscriptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_SubscriptionServiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_SubscriptionServiceId] ON [dbo].[SubscriptionServiceStatus]
(
	[SubscriptionServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_SubscriptionServiceStatusDescriptionId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_SubscriptionServiceStatusDescriptionId] ON [dbo].[SubscriptionServiceStatus]
(
	[SubscriptionServiceStatusDescriptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_VehicleId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_VehicleId] ON [dbo].[Teams]
(
	[VehicleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceId] ON [dbo].[Transactions]
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_LinkedInvoiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_LinkedInvoiceId] ON [dbo].[Transactions]
(
	[LinkedInvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[Transactions]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AccountId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_AccountId] ON [dbo].[TransactionValues]
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionId] ON [dbo].[TransactionValues]
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TeamId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_TeamId] ON [dbo].[Users]
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_VehicleId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_VehicleId] ON [dbo].[VehicleHistories]
(
	[VehicleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TeamId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_TeamId] ON [dbo].[Workers]
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[Workers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TeamId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_TeamId] ON [dbo].[WorkOrderResources]
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_VehicleId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_VehicleId] ON [dbo].[WorkOrderResources]
(
	[VehicleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrderId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrderId] ON [dbo].[WorkOrderResources]
(
	[WorkOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkerUserId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_WorkerUserId] ON [dbo].[WorkOrderResourceWorkers]
(
	[WorkerUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrderResourceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrderResourceId] ON [dbo].[WorkOrderResourceWorkers]
(
	[WorkOrderResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CleaningObjectId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_CleaningObjectId] ON [dbo].[WorkOrders]
(
	[CleaningObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_ServiceId] ON [dbo].[WorkOrderServices]
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrderId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrderId] ON [dbo].[WorkOrderServices]
(
	[WorkOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrderId]    Script Date: 2015-12-21 15:14:32 ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrderId] ON [dbo].[WorkOrderTimeReports]
(
	[WorkOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BGMAXPaymentInvoiceConnections] ADD  DEFAULT ((0)) FOR [UserId]
GO
ALTER TABLE [dbo].[CleaningObjectPrices] ADD  DEFAULT ((1)) FOR [ServiceGroupId]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT ('2015-12-17 09:57:09') FOR [CreatedDate]
GO
ALTER TABLE [dbo].[InvoiceContacts] ADD  DEFAULT ((0)) FOR [NoPersonalNoValidation]
GO
ALTER TABLE [dbo].[Invoices] ADD  DEFAULT ((0)) FOR [Period]
GO
ALTER TABLE [dbo].[Invoices] ADD  DEFAULT ((0)) FOR [NoPersonalNoValidation]
GO
ALTER TABLE [dbo].[IssueHistories] ADD  DEFAULT ((1)) FOR [IssueType]
GO
ALTER TABLE [dbo].[Issues] ADD  DEFAULT ((1)) FOR [IssueType]
GO
ALTER TABLE [dbo].[Persons] ADD  DEFAULT ((0)) FOR [NoPersonalNoValidation]
GO
ALTER TABLE [dbo].[Services] ADD  DEFAULT ((1)) FOR [SettableByCleaners]
GO
ALTER TABLE [dbo].[Subscriptions] ADD  DEFAULT ('2015-12-17 09:57:09') FOR [ActiveChangedDate]
GO
ALTER TABLE [dbo].[SubscriptionServices] ADD  DEFAULT ((0)) FOR [IsPermanent]
GO
ALTER TABLE [dbo].[TableLocks] ADD  DEFAULT ((0)) FOR [IsLocked]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT ((0)) FOR [TaxReductionDecided]
GO
ALTER TABLE [dbo].[WorkOrders] ADD  DEFAULT ((0)) FOR [TimeBooked]
GO
ALTER TABLE [dbo].[WorkOrders] ADD  DEFAULT ((0)) FOR [SingleCleaningType]
GO
ALTER TABLE [dbo].[WorkOrders] ADD  DEFAULT ((0)) FOR [CustomerIsNew]
GO
ALTER TABLE [dbo].[WorkOrderServices] ADD  DEFAULT ((0)) FOR [Count]
GO
ALTER TABLE [dbo].[WorkOrderServices] ADD  DEFAULT ((0)) FOR [SubscriptionServiceStatus]
GO
ALTER TABLE [dbo].[BGMAXFileHistories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BGMAXFileHistories_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[BGMAXFileHistories] CHECK CONSTRAINT [FK_dbo.BGMAXFileHistories_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[BGMAXPaymentInvoiceConnections]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BGMAXPaymentInvoiceConnections_dbo.Invoices_InvoiceId] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoices] ([Id])
GO
ALTER TABLE [dbo].[BGMAXPaymentInvoiceConnections] CHECK CONSTRAINT [FK_dbo.BGMAXPaymentInvoiceConnections_dbo.Invoices_InvoiceId]
GO
ALTER TABLE [dbo].[BGMAXPaymentInvoiceConnections]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BGMAXPaymentInvoiceConnections_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[BGMAXPaymentInvoiceConnections] CHECK CONSTRAINT [FK_dbo.BGMAXPaymentInvoiceConnections_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[CleaningObjectPrices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.CleaningObjectPrices_dbo.CleaningObjects_CleaningObjectId] FOREIGN KEY([CleaningObjectId])
REFERENCES [dbo].[CleaningObjects] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CleaningObjectPrices] CHECK CONSTRAINT [FK_dbo.CleaningObjectPrices_dbo.CleaningObjects_CleaningObjectId]
GO
ALTER TABLE [dbo].[CleaningObjectPrices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.CleaningObjectPrices_dbo.ServiceGroups_ServiceGroupId] FOREIGN KEY([ServiceGroupId])
REFERENCES [dbo].[ServiceGroups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CleaningObjectPrices] CHECK CONSTRAINT [FK_dbo.CleaningObjectPrices_dbo.ServiceGroups_ServiceGroupId]
GO
ALTER TABLE [dbo].[CleaningObjectPrices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.CleaningObjectPrices_dbo.Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CleaningObjectPrices] CHECK CONSTRAINT [FK_dbo.CleaningObjectPrices_dbo.Services_ServiceId]
GO
ALTER TABLE [dbo].[CleaningObjects]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.CleaningObjects_dbo.Customers_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
GO
ALTER TABLE [dbo].[CleaningObjects] CHECK CONSTRAINT [FK_dbo.CleaningObjects_dbo.Customers_CustomerId]
GO
ALTER TABLE [dbo].[CleaningObjects]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.CleaningObjects_dbo.PostalAddressModels_PostalAddressModelId] FOREIGN KEY([PostalAddressModelId])
REFERENCES [dbo].[PostalAddressModels] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CleaningObjects] CHECK CONSTRAINT [FK_dbo.CleaningObjects_dbo.PostalAddressModels_PostalAddressModelId]
GO
ALTER TABLE [dbo].[CleaningObjects]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.CleaningObjects_dbo.Teams_TeamId] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[CleaningObjects] CHECK CONSTRAINT [FK_dbo.CleaningObjects_dbo.Teams_TeamId]
GO
ALTER TABLE [dbo].[Contacts]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Contacts_dbo.CleaningObjects_CleaningObjectId] FOREIGN KEY([CleaningObjectId])
REFERENCES [dbo].[CleaningObjects] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Contacts] CHECK CONSTRAINT [FK_dbo.Contacts_dbo.CleaningObjects_CleaningObjectId]
GO
ALTER TABLE [dbo].[Contacts]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Contacts_dbo.Persons_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Contacts] CHECK CONSTRAINT [FK_dbo.Contacts_dbo.Persons_PersonId]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Customers_dbo.Banks_BankId] FOREIGN KEY([BankId])
REFERENCES [dbo].[Banks] ([Id])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_dbo.Customers_dbo.Banks_BankId]
GO
ALTER TABLE [dbo].[Customers]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Customers_dbo.Persons_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_dbo.Customers_dbo.Persons_PersonId]
GO
ALTER TABLE [dbo].[CustomPATypeConns]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CustomPATypeConns_dbo.CustomPostalAddressTypes_CustomPostalAddressTypeId] FOREIGN KEY([CustomPostalAddressTypeId])
REFERENCES [dbo].[CustomPostalAddressTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomPATypeConns] CHECK CONSTRAINT [FK_dbo.CustomPATypeConns_dbo.CustomPostalAddressTypes_CustomPostalAddressTypeId]
GO
ALTER TABLE [dbo].[CustomPATypeConns]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CustomPATypeConns_dbo.PersonPostalAddressModels_PersonPostalAddressModelId] FOREIGN KEY([PersonPostalAddressModelId])
REFERENCES [dbo].[PersonPostalAddressModels] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomPATypeConns] CHECK CONSTRAINT [FK_dbo.CustomPATypeConns_dbo.PersonPostalAddressModels_PersonPostalAddressModelId]
GO
ALTER TABLE [dbo].[Deviations]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Deviations_dbo.Teams_TeamId] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[Deviations] CHECK CONSTRAINT [FK_dbo.Deviations_dbo.Teams_TeamId]
GO
ALTER TABLE [dbo].[Deviations]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Deviations_dbo.Vehicles_VehicleId] FOREIGN KEY([VehicleId])
REFERENCES [dbo].[Vehicles] ([Id])
GO
ALTER TABLE [dbo].[Deviations] CHECK CONSTRAINT [FK_dbo.Deviations_dbo.Vehicles_VehicleId]
GO
ALTER TABLE [dbo].[Deviations]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Deviations_dbo.Workers_WorkerId] FOREIGN KEY([WorkerId])
REFERENCES [dbo].[Workers] ([UserId])
GO
ALTER TABLE [dbo].[Deviations] CHECK CONSTRAINT [FK_dbo.Deviations_dbo.Workers_WorkerId]
GO
ALTER TABLE [dbo].[InvoiceContacts]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.InvoiceContacts_dbo.Invoices_InvoiceId] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceContacts] CHECK CONSTRAINT [FK_dbo.InvoiceContacts_dbo.Invoices_InvoiceId]
GO
ALTER TABLE [dbo].[InvoiceRows]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.InvoiceRows_dbo.Invoices_InvoiceId] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceRows] CHECK CONSTRAINT [FK_dbo.InvoiceRows_dbo.Invoices_InvoiceId]
GO
ALTER TABLE [dbo].[InvoiceRows]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.InvoiceRows_dbo.Transactions_TransactionId] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transactions] ([Id])
GO
ALTER TABLE [dbo].[InvoiceRows] CHECK CONSTRAINT [FK_dbo.InvoiceRows_dbo.Transactions_TransactionId]
GO
ALTER TABLE [dbo].[Invoices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Invoices_dbo.CleaningObjects_CleaningObjectId] FOREIGN KEY([CleaningObjectId])
REFERENCES [dbo].[CleaningObjects] ([Id])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_dbo.Invoices_dbo.CleaningObjects_CleaningObjectId]
GO
ALTER TABLE [dbo].[Invoices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Invoices_dbo.Invoices_ReferenceInvoiceId] FOREIGN KEY([ReferenceInvoiceId])
REFERENCES [dbo].[Invoices] ([Id])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_dbo.Invoices_dbo.Invoices_ReferenceInvoiceId]
GO
ALTER TABLE [dbo].[Invoices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Invoices_dbo.WorkOrders_WorkOrderId] FOREIGN KEY([WorkOrderId])
REFERENCES [dbo].[WorkOrders] ([Id])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_dbo.Invoices_dbo.WorkOrders_WorkOrderId]
GO
ALTER TABLE [dbo].[IssueHistories]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.IssueHistories_dbo.Users_Assignee_Id] FOREIGN KEY([Assignee_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[IssueHistories] CHECK CONSTRAINT [FK_dbo.IssueHistories_dbo.Users_Assignee_Id]
GO
ALTER TABLE [dbo].[IssueHistories]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.IssueHistories_dbo.Users_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[IssueHistories] CHECK CONSTRAINT [FK_dbo.IssueHistories_dbo.Users_User_Id]
GO
ALTER TABLE [dbo].[Issues]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Issues_dbo.CleaningObjects_CleaningObject_Id] FOREIGN KEY([CleaningObjectId])
REFERENCES [dbo].[CleaningObjects] ([Id])
GO
ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_dbo.Issues_dbo.CleaningObjects_CleaningObject_Id]
GO
ALTER TABLE [dbo].[Issues]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Issues_dbo.Customers_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
GO
ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_dbo.Issues_dbo.Customers_CustomerId]
GO
ALTER TABLE [dbo].[Issues]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Issues_dbo.Invoices_InvoiceId] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoices] ([Id])
GO
ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_dbo.Issues_dbo.Invoices_InvoiceId]
GO
ALTER TABLE [dbo].[Issues]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Issues_dbo.Users_Assignee_Id] FOREIGN KEY([AssigneeId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_dbo.Issues_dbo.Users_Assignee_Id]
GO
ALTER TABLE [dbo].[Issues]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Issues_dbo.Users_Creator_Id] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_dbo.Issues_dbo.Users_Creator_Id]
GO
ALTER TABLE [dbo].[Issues]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Issues_dbo.WorkOrders_WorkOrderId] FOREIGN KEY([WorkOrderId])
REFERENCES [dbo].[WorkOrders] ([Id])
GO
ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_dbo.Issues_dbo.WorkOrders_WorkOrderId]
GO
ALTER TABLE [dbo].[Periods]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Periods_dbo.Schedules_ScheduleId] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedules] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Periods] CHECK CONSTRAINT [FK_dbo.Periods_dbo.Schedules_ScheduleId]
GO
ALTER TABLE [dbo].[PersonPostalAddressModels]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.PersonPostalAddressModels_dbo.Persons_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonPostalAddressModels] CHECK CONSTRAINT [FK_dbo.PersonPostalAddressModels_dbo.Persons_PersonId]
GO
ALTER TABLE [dbo].[PersonPostalAddressModels]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.PersonPostalAddressModels_dbo.PostalAddressModels_PostalAddressModelId] FOREIGN KEY([PostalAddressModelId])
REFERENCES [dbo].[PostalAddressModels] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonPostalAddressModels] CHECK CONSTRAINT [FK_dbo.PersonPostalAddressModels_dbo.PostalAddressModels_PostalAddressModelId]
GO
ALTER TABLE [dbo].[PostalAddressModels]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.PostalAddressModels_dbo.PostalCodeModels_PostalCodeModelId] FOREIGN KEY([PostalCodeModelId])
REFERENCES [dbo].[PostalCodeModels] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PostalAddressModels] CHECK CONSTRAINT [FK_dbo.PostalAddressModels_dbo.PostalCodeModels_PostalCodeModelId]
GO
ALTER TABLE [dbo].[PostalCodeModels]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.PostalCodeModels_dbo.Schedules_ScheduleId] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedules] ([Id])
GO
ALTER TABLE [dbo].[PostalCodeModels] CHECK CONSTRAINT [FK_dbo.PostalCodeModels_dbo.Schedules_ScheduleId]
GO
ALTER TABLE [dbo].[Prices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Prices_dbo.ServiceGroups_ServiceGroupId] FOREIGN KEY([ServiceGroupId])
REFERENCES [dbo].[ServiceGroups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Prices] CHECK CONSTRAINT [FK_dbo.Prices_dbo.ServiceGroups_ServiceGroupId]
GO
ALTER TABLE [dbo].[Prices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Prices_dbo.Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Prices] CHECK CONSTRAINT [FK_dbo.Prices_dbo.Services_ServiceId]
GO
ALTER TABLE [dbo].[Services]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Services_dbo.Accounts_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_dbo.Services_dbo.Accounts_AccountId]
GO
ALTER TABLE [dbo].[Services]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Services_dbo.SubCategories_SubCategoryId] FOREIGN KEY([SubCategoryId])
REFERENCES [dbo].[SubCategories] ([Id])
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_dbo.Services_dbo.SubCategories_SubCategoryId]
GO
ALTER TABLE [dbo].[SingleCleanings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SingleCleanings_dbo.CleaningObjects_CleaningObjectId] FOREIGN KEY([CleaningObjectId])
REFERENCES [dbo].[CleaningObjects] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SingleCleanings] CHECK CONSTRAINT [FK_dbo.SingleCleanings_dbo.CleaningObjects_CleaningObjectId]
GO
ALTER TABLE [dbo].[SingleCleanings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SingleCleanings_dbo.Teams_TeamId] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SingleCleanings] CHECK CONSTRAINT [FK_dbo.SingleCleanings_dbo.Teams_TeamId]
GO
ALTER TABLE [dbo].[SingleCleaningServices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SingleCleaningServices_dbo.Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SingleCleaningServices] CHECK CONSTRAINT [FK_dbo.SingleCleaningServices_dbo.Services_ServiceId]
GO
ALTER TABLE [dbo].[SingleCleaningServices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SingleCleaningServices_dbo.SingleCleanings_SingleCleaningId] FOREIGN KEY([SingleCleaningId])
REFERENCES [dbo].[SingleCleanings] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SingleCleaningServices] CHECK CONSTRAINT [FK_dbo.SingleCleaningServices_dbo.SingleCleanings_SingleCleaningId]
GO
ALTER TABLE [dbo].[Subscriptions]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Subscriptions_dbo.CleaningObjects_CleaningObjectId] FOREIGN KEY([CleaningObjectId])
REFERENCES [dbo].[CleaningObjects] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_dbo.Subscriptions_dbo.CleaningObjects_CleaningObjectId]
GO
ALTER TABLE [dbo].[SubscriptionServices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.SubscriptionServices_dbo.Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubscriptionServices] CHECK CONSTRAINT [FK_dbo.SubscriptionServices_dbo.Services_ServiceId]
GO
ALTER TABLE [dbo].[SubscriptionServices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.SubscriptionServices_dbo.Subscriptions_SubscriptionId] FOREIGN KEY([SubscriptionId])
REFERENCES [dbo].[Subscriptions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubscriptionServices] CHECK CONSTRAINT [FK_dbo.SubscriptionServices_dbo.Subscriptions_SubscriptionId]
GO
ALTER TABLE [dbo].[SubscriptionServiceStatus]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SubscriptionServiceStatus_dbo.SubscriptionServices_SubscriptionServiceId] FOREIGN KEY([SubscriptionServiceId])
REFERENCES [dbo].[SubscriptionServices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubscriptionServiceStatus] CHECK CONSTRAINT [FK_dbo.SubscriptionServiceStatus_dbo.SubscriptionServices_SubscriptionServiceId]
GO
ALTER TABLE [dbo].[SubscriptionServiceStatus]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SubscriptionServiceStatus_dbo.SubscriptionServiceStatusDescriptions_SubscriptionServiceStatusDescriptionId] FOREIGN KEY([SubscriptionServiceStatusDescriptionId])
REFERENCES [dbo].[SubscriptionServiceStatusDescriptions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubscriptionServiceStatus] CHECK CONSTRAINT [FK_dbo.SubscriptionServiceStatus_dbo.SubscriptionServiceStatusDescriptions_SubscriptionServiceStatusDescriptionId]
GO
ALTER TABLE [dbo].[Teams]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Teams_dbo.Vehicles_VehicleId] FOREIGN KEY([VehicleId])
REFERENCES [dbo].[Vehicles] ([Id])
GO
ALTER TABLE [dbo].[Teams] CHECK CONSTRAINT [FK_dbo.Teams_dbo.Vehicles_VehicleId]
GO
ALTER TABLE [dbo].[Transactions]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Transactions_dbo.Invoices_InvoiceId] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_dbo.Transactions_dbo.Invoices_InvoiceId]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Transactions_dbo.Invoices_LinkedInvoiceId] FOREIGN KEY([LinkedInvoiceId])
REFERENCES [dbo].[Invoices] ([Id])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_dbo.Transactions_dbo.Invoices_LinkedInvoiceId]
GO
ALTER TABLE [dbo].[Transactions]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Transactions_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_dbo.Transactions_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[TransactionValues]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.TransactionValues_dbo.Accounts_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TransactionValues] CHECK CONSTRAINT [FK_dbo.TransactionValues_dbo.Accounts_AccountId]
GO
ALTER TABLE [dbo].[TransactionValues]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.TransactionValues_dbo.Transactions_TransactionId] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transactions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TransactionValues] CHECK CONSTRAINT [FK_dbo.TransactionValues_dbo.Transactions_TransactionId]
GO
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.Teams_TeamId] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.Teams_TeamId]
GO
ALTER TABLE [dbo].[VehicleHistories]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.VehicleHistories_dbo.Vehicles_VehicleId] FOREIGN KEY([VehicleId])
REFERENCES [dbo].[Vehicles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VehicleHistories] CHECK CONSTRAINT [FK_dbo.VehicleHistories_dbo.Vehicles_VehicleId]
GO
ALTER TABLE [dbo].[Workers]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Workers_dbo.Teams_TeamId] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[Workers] CHECK CONSTRAINT [FK_dbo.Workers_dbo.Teams_TeamId]
GO
ALTER TABLE [dbo].[Workers]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Workers_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Workers] CHECK CONSTRAINT [FK_dbo.Workers_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[WorkOrderResources]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.WorkOrderResources_dbo.Teams_TeamId] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderResources] CHECK CONSTRAINT [FK_dbo.WorkOrderResources_dbo.Teams_TeamId]
GO
ALTER TABLE [dbo].[WorkOrderResources]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.WorkOrderResources_dbo.Vehicles_VehicleId] FOREIGN KEY([VehicleId])
REFERENCES [dbo].[Vehicles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderResources] CHECK CONSTRAINT [FK_dbo.WorkOrderResources_dbo.Vehicles_VehicleId]
GO
ALTER TABLE [dbo].[WorkOrderResources]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.WorkOrderResources_dbo.WorkOrders_WorkOrderId] FOREIGN KEY([WorkOrderId])
REFERENCES [dbo].[WorkOrders] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderResources] CHECK CONSTRAINT [FK_dbo.WorkOrderResources_dbo.WorkOrders_WorkOrderId]
GO
ALTER TABLE [dbo].[WorkOrderResourceWorkers]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.WorkOrderResourceWorkers_dbo.Workers_WorkerUserId] FOREIGN KEY([WorkerUserId])
REFERENCES [dbo].[Workers] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderResourceWorkers] CHECK CONSTRAINT [FK_dbo.WorkOrderResourceWorkers_dbo.Workers_WorkerUserId]
GO
ALTER TABLE [dbo].[WorkOrderResourceWorkers]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.WorkOrderResourceWorkers_dbo.WorkOrderResources_WorkOrderResourceId] FOREIGN KEY([WorkOrderResourceId])
REFERENCES [dbo].[WorkOrderResources] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderResourceWorkers] CHECK CONSTRAINT [FK_dbo.WorkOrderResourceWorkers_dbo.WorkOrderResources_WorkOrderResourceId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.WorkOrders_dbo.CleaningObjects_CleaningObjectId] FOREIGN KEY([CleaningObjectId])
REFERENCES [dbo].[CleaningObjects] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_dbo.WorkOrders_dbo.CleaningObjects_CleaningObjectId]
GO
ALTER TABLE [dbo].[WorkOrderServices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.WorkOrderServices_dbo.Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderServices] CHECK CONSTRAINT [FK_dbo.WorkOrderServices_dbo.Services_ServiceId]
GO
ALTER TABLE [dbo].[WorkOrderServices]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.WorkOrderServices_dbo.WorkOrders_WorkOrderId] FOREIGN KEY([WorkOrderId])
REFERENCES [dbo].[WorkOrders] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderServices] CHECK CONSTRAINT [FK_dbo.WorkOrderServices_dbo.WorkOrders_WorkOrderId]
GO
ALTER TABLE [dbo].[WorkOrderTimeReports]  WITH CHECK ADD  CONSTRAINT [FK_dbo.WorkOrderTimeReports_dbo.WorkOrders_WorkOrderId] FOREIGN KEY([WorkOrderId])
REFERENCES [dbo].[WorkOrders] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderTimeReports] CHECK CONSTRAINT [FK_dbo.WorkOrderTimeReports_dbo.WorkOrders_WorkOrderId]
GO
/****** Object:  StoredProcedure [dbo].[CleaningObjectsPerCustomer]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joakim Tornhill
-- Create date: 2014-09-16
-- Description:	Return all rows matching customerNo, personalNo or Name 
-- =============================================
CREATE PROCEDURE [dbo].[CleaningObjectsPerCustomer] 
	@PersNo nvarchar(13)  = Null, 
	@CustNo int = 0,  
	@FirstName nvarchar(100)  = Null, 
	@LastName nvarchar(100)  = Null
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (@PersNo Is Null)
	BEGIN
	   SET @PersNo = 'X'
	END

	IF (@CustNo Is Null)
	BEGIN
	   SET @CustNo = 0
	END
	
	IF (@FirstName Is Null)
	BEGIN
	   SET @FirstName = 'X'
	END

	IF (@LastName Is Null)
	BEGIN
	   SET @LastName = 'X'
	END

    SELECT CO.Id,PostalAddress + ' ' + PAM.StreetNo As PostalAddress,PostalCode,City,CO.Invoicable,CO.OtherInfo,CO.IsActive FROM PostalCodeModels As PCM 
	INNER JOIN PostalAddressModels As PAM
	ON PCM.Id = PAM.PostalCodeModelId
	INNER JOIN CleaningObjects AS CO
	ON PAM.Id = CO.PostalAddressModelId
	INNER JOIN Customers AS CUS
	ON CUS.Id = CO.CustomerId
	INNER JOIN Persons AS PER
	ON PER.Id = CUS.PersonId
	WHERE Per.PersonalNo = '' + @PersNo + ''
	OR CUS.Id = @CustNo
	OR (FirstName Like '' + @FirstName + '' AND LastName LIKE '' + @LastName + '')


END


GO
/****** Object:  StoredProcedure [dbo].[GetTotalValuesCalendarPeriod]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Joakim Tornhill
-- Create date: 2014-11-19
-- Description:	Generates real time values on the calendar view for deviations on
-- 1: Planned ordervalue on chosen calendar period
-- 2: Handled amount of Workorders for given calendar period
-- NB! These figures will almost never be the same since cleaners will change prices and sincce calendar period seldom equals cleaning period
-- =============================================
CREATE PROCEDURE [dbo].[GetTotalValuesCalendarPeriod]
   @StartDate DateTime = Null,
   @EndDate   DateTime = Null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @StartWeek int = 0
	Declare @EndWeek int = 0
	Declare @Year Int = 0
	Declare @Temp_Id Int = 0
	Declare @TeamId Int = 0
	Declare @PlannedValue Int = 0
	Declare @EarnedValue Int = 0

	IF(@StartDate IS NULL)
	BEGIN
	  SET @StartDate = GetDate()
	END

	IF(@EndDate IS NULL)
	BEGIN
	  SET @EndDate = GetDate()
	END

	SET @StartWeek = DatePart(week, @StartDate) 
	SET @EndWeek =  DatePart(week, @EndDate) 

	SET @Year = DatePart(year,@StartDate)


	IF OBJECT_ID('dbo.TempTeams', 'U') IS NOT NULL
    DROP TABLE dbo.TempTeams

	-- temporary result table

	CREATE TABLE "TempTeams" (
	"id" integer IDENTITY(1,1) PRIMARY KEY,
	"TeamId" int,
	"Name" nvarchar(max) NOT NULL,
	"VehicleId" int,
	"PlannedValue" int,
	"EarnedValue" int
    )

	INSERT INTO TempTeams (TeamId,Name,VehicleId,PlannedValue,EarnedValue)
	SELECT Id,Name,VehicleId,0,0 FROM Teams

	SELECT @Temp_Id = min(id) from TempTeams

	-- loop through every team and set correct values on them.

	WHILE @Temp_Id Is Not Null
	BEGIN

		SELECT @TeamId = [TeamId]
	    FROM TempTeams where id = @Temp_Id

		--Planned Value (estimated order valur for given period)

		SELECT @PlannedValue = sum(cost*Modification) 
		FROM PostalCodeModels PM
		INNER JOIN PostalAddressModels PAM
		ON PAM.PostalCodeModelId = PM.Id
		INNER JOIN CleaningObjects CO
		ON CO.PostalAddressModelId = PAM.Id
		INNER JOIN Teams T
		ON CO.TeamId = T.Id
		INNER JOIN Subscriptions S
		ON S.CleaningObjectId = CO.Id
		INNER JOIN SubscriptionServices SS
		ON S.Id = SS.SubscriptionId
		INNER JOIN Periods P
		ON P.ScheduleId = PM.ScheduleId
		INNER JOIN Prices Pric
		ON (Pric.ServiceId = SS.ServiceId AND  Pric.ServiceGroupId = (SELECT Top 1 Id FROM ServiceGroups WHERE GETDATE()>=[From] AND GETDATE()<=[To]))
		INNER JOIN CleaningObjectPrices COP
		ON (COP.CleaningObjectId = Co.Id AND COP.ServiceId =SS.ServiceId)
		WHERE P.WeekFROM>= @StartWeek AND P.WeekTo<= @EndWeek
		AND P.ValidFrom = (SELECT MAX(ValidFrom) FROM Periods WHERE ValidFrom <= GetDate())
		AND SS.PeriodNo = (SELECT count(Id) FROM Periods WHERE WeekTo<= @EndWeek AND ScheduleId = PM.ScheduleId)
		AND T.ID = @TeamId
		

		--Earned value (what the cleaners have finished during given period)

		SELECT @EarnedValue = Sum(ISNull(ModifiedCost,OriginalCost)) FROM WorkOrders WO 
		INNER JOIN WorkOrderServices WOS
		ON WO.Id = WOS.WorkOrderId
		INNER JOIN CleaningObjects CO
		ON CO.Id = WO.CleaningObjectId
		INNER JOIN PostalAddressModels PAM
		ON CO.PostalAddressModelId = PAM.Id
		INNER JOIN PostalCodeModels PM
		ON PAM.PostalCodeModelId = PM.Id
		WHERE WO.Status = 3 AND [Year] = @Year
		AND Period = (SELECT count(Id) FROM Periods WHERE WeekTo<= @EndWeek AND ScheduleId = PM.ScheduleId)
		AND TeamId = @TeamId

		UPDATE TempTeams SET PlannedValue = @PlannedValue, EarnedValue = @EarnedValue WHERE id = @Temp_Id

		SELECT @Temp_Id = min(id) from TempTeams WHERE Id> @Temp_Id

	END

	SELECT '{Text:"' + [Name] + '; '+ FORMAT(IsNull(PlannedValue,0),'C','sv-SE') + ', (' + FORMAT(IsNull(EarnedValue,0),'C','sv-SE') + 
	')",a_attr:{DataId:' + Cast(TeamId As nvarchar)  + 
	',PlannedValue:' + CAST(IsNull(PlannedValue,0) AS nvarchar) + ',EarnedValue:' + CAST(IsNull(EarnedValue,0) AS nvarchar) +
	',NodeType:0,Name:"' + [Name] + '",GetChildrenUrl:"/Planning/GetResourcesForDeviationTree"},children:true}'
	As [Text]
	FROM TempTeams TT

END





GO
/****** Object:  StoredProcedure [dbo].[SearchCustomer]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joakim Tornhill
-- Create date: 2014-10-20
-- Description:	Return all rows matching customerNo, personalNo or Name 
-- =============================================
CREATE PROCEDURE [dbo].[SearchCustomer]
    @CustNo nvarchar(13)  = Null, 
	@PersNo nvarchar(13)  = Null, 
	@FirstName nvarchar(100)  = Null, 
	@LastName nvarchar(100)  = Null,
	@Address nvarchar(100)  = Null,
	@PostalCode nvarchar(20)  = Null,
	@City nvarchar(100)  = Null
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SQLQuery VARCHAR(2500)

	SET @SQLQuery = 'SELECT Id As DataId, Type, PersonalNo,FirstName,LastName,Address,PostalCode,City,COId FROM
        (SELECT C.Id,''Kund'' As Type, P.PersonalNo,P.FirstName,P.LastName,CONCAT(PCM.PostalAddress,'' '',PAM.StreetNo) As Address,PCM.PostalCode,PCM.City,CO.Id AS COId FROM Customers C
        INNER JOIN Persons P 
        ON C.PersonId = P.Id
        INNER JOIN CleaningObjects CO
        ON CO.CustomerId = C.Id
        INNER JOIN PostalAddressModels PAM
        ON CO.PostalAddressModelId = PAM.Id
        INNER JOIN PostalCodeModels PCM
        ON PAM.PostalCodeModelId= PCM.Id
        INNER JOIN PersonPostalAddressModels PPAM
        ON PPAM.PostalAddressModelId = PAM.Id
        WHERE (PPAM.TYPE & 2)<> 0
        UNION ALL
        SELECT C.Id,''Avisering'' As Type, P.PersonalNo,P.FirstName,P.LastName,CONCAT(PCM.PostalAddress,'' '',PAM.StreetNo) As Address,PCM.PostalCode,PCM.City,CO.Id AS COId 
        FROM Contacts C
        INNER JOIN Persons P 
        ON C.PersonId = P.Id
        INNER JOIN CleaningObjects CO
        ON CO.Id = C.CleaningObjectId
        INNER JOIN PostalAddressModels PAM
        ON CO.PostalAddressModelId = PAM.Id
        INNER JOIN PostalCodeModels PCM
        ON PAM.PostalCodeModelId= PCM.Id
        INNER JOIN PersonPostalAddressModels PPAM
        ON PPAM.PostalAddressModelId = PAM.Id
        WHERE (PPAM.TYPE & 1)<> 0 AND (PPAM.TYPE & 2)= 0 AND C.Notify = 1) X
		WHERE Id > 0 '

    IF(NOT @CustNo IS NULL)
	BEGIN
	    SET @SQLQuery = @SQLQuery + ' AND Id = ' + @CustNo + ' '
	END
	
	IF(NOT @PersNo IS NULL)
	BEGIN
	    SET @SQLQuery = @SQLQuery + ' AND PersonalNo = ''' + @PersNo + ''' '
	END
	
	IF(NOT @FirstName IS NULL)
	BEGIN
	    SET @SQLQuery = @SQLQuery + ' AND FirstName LIKE ''' + @FirstName + '%'' '  
	END

	IF(NOT @LastName IS NULL)
	BEGIN
	    SET @SQLQuery = @SQLQuery + ' AND LastName LIKE ''' + @LastName + '%'' '
	END

	IF(NOT @Address IS NULL)
	BEGIN
	    SET @SQLQuery = @SQLQuery + ' AND Address LIKE ''' + @Address + '%'' '
	END

	IF(NOT @PostalCode IS NULL)
	BEGIN
	    SET @SQLQuery = @SQLQuery + ' AND PostalCode LIKE ''' + @PostalCode + '%'' '
	END

	IF(NOT @City IS NULL)
	BEGIN
	    SET @SQLQuery = @SQLQuery + ' AND City LIKE ''' + @City + '%'' '
	END

	EXECUTE(@SQLQuery)
END




GO
/****** Object:  StoredProcedure [dbo].[SumPerAddressOnCleaningObjects]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joakim Tornhill
-- Create date: 2014-09-18
-- Description:	Shows sum per street on CleaningObjects filtered by postalCode
-- =============================================
CREATE PROCEDURE [dbo].[SumPerAddressOnCleaningObjects] @PostalCodes nvarchar(10) 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	SELECT DISTINCT CONVERT(NVARCHAR(50), NEWID()) As id,PostalAddress As Street,PostalCode,City,Round(Sum(P.Cost*COP.Modification),0) as TotSum, (SELECT NAME FROM Teams WHERE Id = Co.TeamId)  as TeamName, (SELECT Name FROM Schedules WHERE Id = PCM.ScheduleId) As ScheduleName
	FROM PostalCodeModels AS PCM
	INNER JOIN PostalAddressModels PAM
	ON PCM.id = PAM.PostalCodeModelId
	INNER JOIN CleaningObjects CO
	ON PAM.Id = CO.PostalAddressModelId
	INNER JOIN CleaningObjectPrices COP
	ON CO.Id = COP.CleaningObjectId
	INNER JOIN Services AS S
	ON S.Id = COP.ServiceId 
	INNER JOIN Prices As P
	ON P.ServiceId = S.Id AND P.ServiceGroupId = (SELECT Top 1 Id FROM ServiceGroups WHERE GETDATE()>=[From] AND GETDATE()<=[To])
	WHERE S.Category IN(1,3) AND PostalCode = '' + @PostalCodes  + ''
	GROUP BY PostalAddress,PostalCode,City,teamid,scheduleid
END




GO
/****** Object:  StoredProcedure [dbo].[SumPerPostalCodeOnCleaningObjects]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Joakim Tornhill
-- Create date: 2014-09-10
-- Description:	Return Price/PostalCode on all CleaningObjects for categori 1 (Grundpris) and 3 (tillägg)
-- =============================================
CREATE PROCEDURE [dbo].[SumPerPostalCodeOnCleaningObjects] 
    @OnlyPriced BIT = 0,  --Only show rows connected to CleaningObjects
	@ScheduleFlag Bit = 0, --Check if Query should be depended on Schedule
	@ScheduleId Int = Null , --Id Depended, use the id for the Schedule
	@PostalAddress nvarchar(200) = Null, --Geographical seaarch
	@PostalCode nvarchar(10) = Null,
    @City nvarchar(100) = Null,
    @Municipality nvarchar(100) = Null,
    @Parish nvarchar(100) = Null,
    @State nvarchar(100) = Null,
	@SchemaExists Bit = 0
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SQLQuery VARCHAR(2500)
    DECLARE @SQLScheduleInner NVARCHAR(500) = ''
    DECLARE @SQLScheduleOuter NVARCHAR(500) = ''
  
    SET @SQLQuery ='SELECT DISTINCT CONVERT(NVARCHAR(50), newId()) As id,PostalCode,City, CAST(Round(Sum(P.Cost*COP.Modification),0) AS DECIMAL) As TotSum, ScheduleId'

	IF(@OnlyPriced = 1)
	BEGIN
	  SET @SQLQuery =@SQLQuery + ', (select Name from Schedules where Schedules.id=ScheduleId) As Name '
	END

	SET @SQLQuery = @SQLQuery +'  FROM PostalCodeModels AS PC INNER JOIN PostalAddressModels AS PAM
		ON PC.Id = PAM.PostalCodeModelId
		INNER JOIN CleaningObjects AS CO
		ON PAM.Id = CO.PostalAddressModelId
		INNER JOIN CleaningObjectPrices AS COP
		ON CO.Id = COP.CleaningObjectId
		INNER JOIN Services AS S
		ON S.Id = COP.ServiceId 
		INNER JOIN Prices As P
		ON P.ServiceId = S.Id AND P.ServiceGroupId = (SELECT Top 1 Id FROM ServiceGroups WHERE GETDATE()>=[From] AND GETDATE()<=[To])
		JOIN Subscriptions subs ON subs.CleaningObjectId = CO.Id
		WHERE S.Category IN(1,3)
		AND subs.IsInactive = 0' 
		IF (@SchemaExists=1)
		BEGIN
		  SET @SQLQuery = @SQLQuery + ' AND Not ScheduleId Is Null '
		END


   IF(@ScheduleFlag=1)
   BEGIN
		IF(@ScheduleId is NULL)
		BEGIN
		SET @SQLScheduleInner = ' AND PC.ScheduleID Is Null ' 
		SET @SQLScheduleOuter = ' AND ScheduleID Is Null'
		END
		ELSE
		BEGIN
		SET @SQLScheduleInner = ' AND PC.ScheduleID = ' + CAST(@ScheduleId AS NVARCHAR(20))
		SET @SQLScheduleOuter = ' AND ScheduleID = ' + CAST(@ScheduleId AS NVARCHAR(20))
		END
   END		
   
   --Start add SQL for region search--

   --DECLARE @Municipality = Null


   IF(NOT @PostalCode is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PC.PostalCode LIKE ''' + @PostalCode + '%'' '
	   SET @SQLScheduleOuter = @SQLScheduleOuter +' AND PostalCode LIKE ''' + @PostalCode + '%'' '
   END

   IF(NOT @PostalAddress is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PC.PostalAddress LIKE ''' + @PostalAddress + '%'' '
	   SET @SQLScheduleOuter = @SQLScheduleOuter +' AND PostalAddress LIKE ''' + @PostalAddress + '%'' '
   END

   IF(NOT @State is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PC.State LIKE ''' + @State + '%'' '
	   SET @SQLScheduleOuter = @SQLScheduleOuter +' AND State LIKE ''' + @State + '%'' '
   END

   IF(NOT @Parish is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PC.Parish LIKE ''' + @Parish + '%'' '
	   SET @SQLScheduleOuter = @SQLScheduleOuter +' AND Parish LIKE ''' + @Parish + '%'' '
   END

   IF(NOT @City is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PC.City LIKE ''' + @City + '%'' '
	   SET @SQLScheduleOuter = @SQLScheduleOuter +' AND City LIKE ''' + @City + '%'' '
   END

   IF(NOT @Municipality is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PC.Municipality LIKE ''' + @Municipality + '%'' '
	   SET @SQLScheduleOuter = @SQLScheduleOuter +' AND Municipality LIKE ''' + @Municipality + '%'' '
   END

   --End add SQL for region search--

   	SET @SQLQuery = @SQLQuery +  @SQLScheduleInner +' GROUP BY PostalCode,City,ScheduleId'

    IF(@OnlyPriced = 0)
	BEGIN
		SET @SQLQuery = '
		SELECT CONVERT(NVARCHAR(50), newId()) As id,PostalCode,City,CAST(Round(Sum(TotSum),0) AS DECIMAL) As TotSum, ScheduleId, (select Name from Schedules where Schedules.id=ScheduleId) As Name FROM (
		SELECT DISTINCT PostalCode As id,PostalCode,City,0 As TotSum, ScheduleId FROM PostalCodeModels WHERE PostalCodeType IN(''AL'',''AT'')'
		+ @SQLScheduleOuter + ' UNION ALL ' + @SQLQuery + ') X '
		IF (@SchemaExists=1)
		BEGIN
		  SET @SQLQuery = @SQLQuery + ' WHERE Not ScheduleId Is Null '
		END

		SET @SQLQuery = @SQLQuery + 'GROUP BY PostalCode,City,ScheduleId		'

		EXECUTE(@SQLQuery)
	END
	ELSE
	BEGIN
		PRINT @SQLQuery
		EXECUTE(@SQLQuery)
	END

END







GO
/****** Object:  StoredProcedure [dbo].[SumPerPostalCodeOnTeams]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Joakim Tornhill
-- Create date: 2014-09-10
-- Description:	Return Price/PostalCode on all CleaningObjects for categori 1 (Grundpris) and 3 (tillägg)
-- =============================================
CREATE PROCEDURE [dbo].[SumPerPostalCodeOnTeams]
 	@ScheduleFlag Bit = 0, --Check if Query should be depended on Schedule
	@ScheduleId Int = Null , --Id Depended, use the id for the Schedule
	@PostalAddress nvarchar(200) = Null, --Geographical seaarch
	@PostalCode nvarchar(10) = Null,
    @City nvarchar(100) = Null,
    @Municipality nvarchar(100) = Null,
    @Parish nvarchar(100) = Null,
    @State nvarchar(100) = Null,
	@SchemaExists Bit = 0,
	@TeamNotExists Bit = 0
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SQLQuery VARCHAR(2500)
    DECLARE @SQLScheduleInner NVARCHAR(500) = ''
  
    SET @SQLQuery ='SELECT CONVERT(NVARCHAR(50), NEWID()) 
		AS id, 
	PostalCode,City, CAST(Round(Sum(P.Cost*COP.Modification),0) AS DECIMAL) as TotSum, 
	(SELECT NAME FROM Teams WHERE Id = Co.TeamId)  as TeamName, (SELECT Name FROM Schedules WHERE Id = PCM.ScheduleId) As ScheduleName
	FROM PostalCodeModels AS PCM
	INNER JOIN PostalAddressModels PAM
	ON PCM.id = PAM.PostalCodeModelId
	INNER JOIN CleaningObjects CO
	ON PAM.Id = CO.PostalAddressModelId
	INNER JOIN CleaningObjectPrices COP
	ON CO.Id = COP.CleaningObjectId
	INNER JOIN Services AS S
	ON S.Id = COP.ServiceId 
	INNER JOIN Prices As P
	ON P.ServiceId = S.Id AND P.ServiceGroupId = (SELECT Top 1 Id FROM ServiceGroups WHERE GETDATE()>=[From] AND GETDATE()<=[To])
	JOIN Subscriptions subs ON subs.CleaningObjectId = CO.Id
	WHERE S.Category IN(1,3)
	AND subs.IsInactive = 0'

	IF (@SchemaExists=1)
	BEGIN
  	  SET @SQLQuery = @SQLQuery + ' AND Not ScheduleId Is Null '
	END

	IF(@TeamNotExists=1)
	BEGIN
  	  SET @SQLQuery = @SQLQuery + ' AND CO.TeamId Is Null '
	END

   IF(@ScheduleFlag=1)
   BEGIN
		IF(@ScheduleId is NULL)
		BEGIN
			SET @SQLScheduleInner = ' AND PCM.ScheduleID Is Null ' 
		END
		ELSE
		BEGIN
			SET @SQLScheduleInner = ' AND PCM.ScheduleID = ' + CAST(@ScheduleId AS NVARCHAR(20))
		END
   END		
   
   --Start add SQL for region search--

   --DECLARE @Municipality = Null


   IF(NOT @PostalCode is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PCM.PostalCode LIKE ''' + @PostalCode + '%'' '
   END

   IF(NOT @PostalAddress is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PCM.PostalAddress LIKE ''' + @PostalAddress + '%'' '
   END

   IF(NOT @State is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PCM.State LIKE ''' + @State + '%'' '
   END

   IF(NOT @Parish is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PCM.Parish LIKE ''' + @Parish + '%'' '
   END

   IF(NOT @City is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PCM.City LIKE ''' + @City + '%'' '
   END

   IF(NOT @Municipality is null)
   BEGIN
	   SET @SQLScheduleInner = @SQLScheduleInner +' AND PCM.Municipality LIKE ''' + @Municipality + '%'' '
   END


   	SET @SQLQuery = @SQLQuery +  @SQLScheduleInner +' GROUP BY PostalCode,City,teamid,scheduleid'

 	PRINT @SQLQuery
	EXECUTE(@SQLQuery)
	

END




GO
/****** Object:  StoredProcedure [dbo].[SumTeamPerAddress]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joakim Tornhill
-- Create date: 2014-09-18
-- Description:	Total sum on one team on all addresses for one postalcode on services from category 1 and 3.
-- =============================================
CREATE PROCEDURE [dbo].[SumTeamPerAddress] @TeamId int, @PostalCode nvarchar(10)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT PCM.PostalAddress as id,PCM.PostalAddress + ' (' + FORMAT(Round(Sum(P.Cost*COP.Modification),0),'C','sv-SE') + ')' As [text] FROM Teams
	INNER JOIN CleaningObjects AS CO
	ON Teams.Id = CO.TeamId
	INNER JOIN PostalAddressModels PAM
	ON PAM.Id = CO.PostalAddressModelId
	INNER JOIN PostalCodeModels PCM
	ON PCM.Id = PAM.PostalCodeModelId
	INNER JOIN CleaningObjectPrices COP
	ON CO.Id = COP.CleaningObjectId
	INNER JOIN Services AS S
	ON S.Id = COP.ServiceId 
	INNER JOIN Prices As P
	ON P.ServiceId = S.Id AND P.ServiceGroupId = (SELECT Top 1 Id FROM ServiceGroups WHERE GETDATE()>=[From] AND GETDATE()<=[To])
	WHERE S.Category IN(1,3) AND Teams.Id = @TeamId AND PCM.PostalCode = '' + @PostalCode + ''
	GROUP BY PCM.PostalAddress
END




GO
/****** Object:  StoredProcedure [dbo].[SumTeamPerPostalCode]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joakim Tornhill
-- Create date: 2014-09-18
-- Description:	total sum for a team on each postalcode for services in category 1 and 3.
-- =============================================
CREATE PROCEDURE [dbo].[SumTeamPerPostalCode] @TeamId int 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT PCM.PostalCode as id,PCM.PostalCode + ' (' + FORMAT(Round(Sum(P.Cost*COP.Modification),0),'C','sv-SE') + ')' As [text] FROM Teams
	INNER JOIN CleaningObjects AS CO
	ON Teams.Id = CO.TeamId
	INNER JOIN PostalAddressModels PAM
	ON PAM.Id = CO.PostalAddressModelId
	INNER JOIN PostalCodeModels PCM
	ON PCM.Id = PAM.PostalCodeModelId
	INNER JOIN CleaningObjectPrices COP
	ON CO.Id = COP.CleaningObjectId
	INNER JOIN Services AS S
	ON S.Id = COP.ServiceId 
	INNER JOIN Prices As P
	ON P.ServiceId = S.Id AND P.ServiceGroupId = (SELECT Top 1 Id FROM ServiceGroups WHERE GETDATE()>=[From] AND GETDATE()<=[To])
	WHERE S.Category IN(1,3) AND Teams.Id = @TeamId
	GROUP BY PCM.PostalCode
END




GO
/****** Object:  StoredProcedure [dbo].[SumTotPerSchedule]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Joakim Tornhill
-- Create date: 2014-09-11
-- Description:	Creates the total sum for each Schedule based on one cleaning on price (Grundpris+tillägg)
-- =============================================
CREATE PROCEDURE [dbo].[SumTotPerSchedule]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id,Name, CAST(Round(Sum(TotSum),0) AS DECIMAL) As TotSum FROM
	(SELECT DISTINCT Id,Name,0 As TotSum FROM Schedules
	UNION ALL
	SELECT DISTINCT SCH.Id,SCH.Name,Sum(P.Cost*COP.Modification) As ToSum FROM Schedules AS SCH
	INNER JOIN PostalCodeModels PC ON
	SCH.Id = PC.ScheduleId
	INNER JOIN PostalAddressModels AS PAM
	ON PC.Id = PAM.PostalCodeModelId
	INNER JOIN CleaningObjects AS CO
	ON PAM.Id = CO.PostalAddressModelId
	INNER JOIN CleaningObjectPrices AS COP
	ON CO.Id = COP.CleaningObjectId
	INNER JOIN Services AS S
	ON S.Id = COP.ServiceId 
	INNER JOIN Prices As P
	ON P.ServiceId = S.Id AND P.ServiceGroupId = (SELECT Top 1 Id FROM ServiceGroups WHERE GETDATE()>=[From] AND GETDATE()<=[To])
	WHERE S.Category IN(1,3)
	GROUP BY SCH.Id,SCH.Name) X
	GROUP BY Id,Name
END





GO
/****** Object:  StoredProcedure [dbo].[SumTotPerTeam]    Script Date: 2015-12-21 15:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joakim Tornhill
-- Create date: 2014-09-18
-- Description:	Total sum per team and postalcode for services category 1 and 3
-- =============================================
CREATE PROCEDURE [dbo].[SumTotPerTeam] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CAST(id as nvarchar) as id,[text]  + ' (' + FORMAT(Sum(TotSum),'C','sv-SE') + ')' As [text], CAST(CASE WHEN Sum(TotSum) = 0 THEN 0 ELSE 1 END AS bit) AS children FROM
	(SELECT Teams.Id As id,Teams.Name AS [text], 0 As TotSum FROM Teams
	UNION all
	SELECT DISTINCT Teams.Id As id,Teams.Name As [text],Round(Sum(P.Cost*COP.Modification),0) As TotSum FROM Teams
	INNER JOIN CleaningObjects AS CO
	ON Teams.Id = CO.TeamId
	INNER JOIN CleaningObjectPrices COP
	ON CO.Id = COP.CleaningObjectId
	INNER JOIN Services AS S
	ON S.Id = COP.ServiceId 
	INNER JOIN Prices As P
	ON P.ServiceId = S.Id AND P.ServiceGroupId = (SELECT Top 1 Id FROM ServiceGroups WHERE GETDATE()>=[From] AND GETDATE()<=[To])
	WHERE S.Category IN(1,3)
	GROUP BY Teams.Id,Teams.Name) X
	GROUP BY id,[text]
	END




GO
USE [master]
GO
ALTER DATABASE [putsa_db] SET  READ_WRITE 
GO
