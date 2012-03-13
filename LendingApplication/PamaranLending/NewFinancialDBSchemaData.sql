EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'Financial'
GO
USE [master]
GO
ALTER DATABASE [Financial] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
USE [master]
GO
/****** Object:  Database [Financial]    Script Date: 11/03/2011 18:06:15 ******/
DROP DATABASE [Financial]
GO
CREATE DATABASE [Financial]
GO


USE [Financial]
GO
/****** Object:  Table [dbo].[ReceivableType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReceivableType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RECEIVABLETYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ReceivableType] ON
INSERT [dbo].[ReceivableType] ([Id], [Name]) VALUES (1, N'Principal')
INSERT [dbo].[ReceivableType] ([Id], [Name]) VALUES (2, N'Interest')
INSERT [dbo].[ReceivableType] ([Id], [Name]) VALUES (3, N'Past Due')
SET IDENTITY_INSERT [dbo].[ReceivableType] OFF
/****** Object:  Table [dbo].[ReceivableStatusType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReceivableStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RECEIVABLESTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ReceivableStatusType] ON
INSERT [dbo].[ReceivableStatusType] ([Id], [Name]) VALUES (1, N'Open')
INSERT [dbo].[ReceivableStatusType] ([Id], [Name]) VALUES (2, N'Partially Paid')
INSERT [dbo].[ReceivableStatusType] ([Id], [Name]) VALUES (3, N'Fully Paid')
INSERT [dbo].[ReceivableStatusType] ([Id], [Name]) VALUES (4, N'Cancelled')
SET IDENTITY_INSERT [dbo].[ReceivableStatusType] OFF
/****** Object:  Table [dbo].[SubmittedDocumentStatusType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SubmittedDocumentStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SUBMITTEDDOCUMENTSTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SubmittedDocumentStatusType] ON
INSERT [dbo].[SubmittedDocumentStatusType] ([Id], [Name]) VALUES (1, N'Pending: Approval')
INSERT [dbo].[SubmittedDocumentStatusType] ([Id], [Name]) VALUES (2, N'Approved')
INSERT [dbo].[SubmittedDocumentStatusType] ([Id], [Name]) VALUES (3, N'Rejected')
SET IDENTITY_INSERT [dbo].[SubmittedDocumentStatusType] OFF
/****** Object:  Table [dbo].[ReceiptStatusType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReceiptStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RECEIPTSTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ReceiptStatusType] ON
INSERT [dbo].[ReceiptStatusType] ([Id], [Name]) VALUES (1, N'Open')
INSERT [dbo].[ReceiptStatusType] ([Id], [Name]) VALUES (2, N'Applied')
INSERT [dbo].[ReceiptStatusType] ([Id], [Name]) VALUES (3, N'Closed')
INSERT [dbo].[ReceiptStatusType] ([Id], [Name]) VALUES (4, N'Cancelled')
SET IDENTITY_INSERT [dbo].[ReceiptStatusType] OFF
/****** Object:  Table [dbo].[Receipt]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Receipt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReceivedFromName] [varchar](50) NULL,
	[ReceiptBalance] [numeric](15, 2) NULL,
 CONSTRAINT [PK_Receipt_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductStatusType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PRODUCTSTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ProductStatusType] ON
INSERT [dbo].[ProductStatusType] ([Id], [Name]) VALUES (1, N'New')
INSERT [dbo].[ProductStatusType] ([Id], [Name]) VALUES (2, N'Active')
INSERT [dbo].[ProductStatusType] ([Id], [Name]) VALUES (3, N'Inactive')
INSERT [dbo].[ProductStatusType] ([Id], [Name]) VALUES (4, N'Retired')
SET IDENTITY_INSERT [dbo].[ProductStatusType] OFF
/****** Object:  Table [dbo].[TelecommunicationsNumberType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TelecommunicationsNumberType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_TELECOMMUNICATIONSNUMBERTYP] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[TelecommunicationsNumberType] ON
INSERT [dbo].[TelecommunicationsNumberType] ([Id], [Name]) VALUES (1, N'Personal Mobile Number')
INSERT [dbo].[TelecommunicationsNumberType] ([Id], [Name]) VALUES (2, N'Business Mobile Number')
INSERT [dbo].[TelecommunicationsNumberType] ([Id], [Name]) VALUES (3, N'Business Fax Number')
INSERT [dbo].[TelecommunicationsNumberType] ([Id], [Name]) VALUES (4, N'Home Phone Number')
INSERT [dbo].[TelecommunicationsNumberType] ([Id], [Name]) VALUES (5, N'Business Phone Number')
SET IDENTITY_INSERT [dbo].[TelecommunicationsNumberType] OFF
/****** Object:  Table [dbo].[UserAccountStatusType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAccountStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_USERACCOUNTSTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[UserAccountStatusType] ON
INSERT [dbo].[UserAccountStatusType] ([Id], [Name]) VALUES (1, N'Active')
INSERT [dbo].[UserAccountStatusType] ([Id], [Name]) VALUES (2, N'Inactive')
SET IDENTITY_INSERT [dbo].[UserAccountStatusType] OFF
/****** Object:  Table [dbo].[UserAccountType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccountType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_UserAccountType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[UserAccountType] ON
INSERT [dbo].[UserAccountType] ([Id], [Name]) VALUES (1, N'Admin')
INSERT [dbo].[UserAccountType] ([Id], [Name]) VALUES (2, N'Super Admin')
INSERT [dbo].[UserAccountType] ([Id], [Name]) VALUES (3, N'Accountant')
INSERT [dbo].[UserAccountType] ([Id], [Name]) VALUES (4, N'Teller')
SET IDENTITY_INSERT [dbo].[UserAccountType] OFF
/****** Object:  Table [dbo].[TransactionTaskType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransactionTaskType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TRANSACTIONTASKTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SystemSettingType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemSettingType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](256) NOT NULL,
 CONSTRAINT [PK_SYSTEMSETTINGTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SystemSettingType] ON
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (1, N'Grace Period')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (2, N'Invoice Generation Timing')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (3, N'Demand Collection After')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (4, N'Age Limit of Borrower')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (5, N'Allowable Number of Diminishing Balance Loan Per Customer')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (6, N'Allowable Number of Straight Line Loan Per Customer')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (7, N'Percentage of Loan Amount Paid')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (8, N'Period in Calculating Penalty')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (9, N'Years of Loans to be Deleted')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (10, N'remaining number of payments for the remaining payments to be paid in full during pay off for amortizated loans')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (11, N'Date Payment Option')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (12, N'Clerk''s Maximum Honorable Amount')
INSERT [dbo].[SystemSettingType] ([Id], [Name]) VALUES (13, N'Advance Change No Interest Start Day')
SET IDENTITY_INSERT [dbo].[SystemSettingType] OFF
/****** Object:  Table [dbo].[UnitOfMeasureType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UnitOfMeasureType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentUomTypeId] [int] NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_UNITOFMEASURETYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[UnitOfMeasureType] ON
INSERT [dbo].[UnitOfMeasureType] ([Id], [ParentUomTypeId], [Name]) VALUES (1, NULL, N'Time Unit')
INSERT [dbo].[UnitOfMeasureType] ([Id], [ParentUomTypeId], [Name]) VALUES (2, NULL, N'Length Unit')
INSERT [dbo].[UnitOfMeasureType] ([Id], [ParentUomTypeId], [Name]) VALUES (3, NULL, N'Derived Unit')
INSERT [dbo].[UnitOfMeasureType] ([Id], [ParentUomTypeId], [Name]) VALUES (4, NULL, N'Time Frequency')
SET IDENTITY_INSERT [dbo].[UnitOfMeasureType] OFF
/****** Object:  Table [dbo].[SpecificPaymentType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SpecificPaymentType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_RECEIPTTYPE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SpecificPaymentType] ON
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (1, N'Loan Payment')
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (2, N'Fee Payment')
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (3, N'Check for Rediscounting')
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (4, N'Reloan Deduction')
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (5, N'Rediscounting')
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (6, N'Interest Payment')
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (7, N'Check for Encashment')
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (8, N'Encashment')
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (9, N'Loan Disbursement')
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (10, N'Other Disbursement')
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (11, N'Change')
INSERT [dbo].[SpecificPaymentType] ([Id], [Name]) VALUES (12, N'Advance Change')
SET IDENTITY_INSERT [dbo].[SpecificPaymentType] OFF
/****** Object:  Table [dbo].[SourceOfIncome]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SourceOfIncome](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SOURCEOFINCOME] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SourceOfIncome] ON
INSERT [dbo].[SourceOfIncome] ([Id], [Name]) VALUES (1, N'Business')
INSERT [dbo].[SourceOfIncome] ([Id], [Name]) VALUES (2, N'Pension')
INSERT [dbo].[SourceOfIncome] ([Id], [Name]) VALUES (3, N'Allowance')
INSERT [dbo].[SourceOfIncome] ([Id], [Name]) VALUES (4, N'Allotment')
SET IDENTITY_INSERT [dbo].[SourceOfIncome] OFF
/****** Object:  Table [dbo].[RoleType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RoleType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentRoleTypeId] [int] NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ROLETYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[RoleType] ON
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (1, NULL, N'Party Role Type')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (2, NULL, N'Loan Application Role Type')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (3, NULL, N'Agreement Role Type')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (4, NULL, N'Asset Role Type')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (5, NULL, N'Financial Account Role Type')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (6, NULL, N'Financial Institution')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (7, 1, N'Customer')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (8, 1, N'Contact')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (9, 1, N'Employee')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (10, 1, N'Employer')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (11, 1, N'Spouse')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (12, 1, N'Taxpayer')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (13, 6, N'Bank')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (14, 1, N'Lending Institution')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (15, 2, N'Borrower')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (16, 2, N'Co-Borrower')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (17, 2, N'Guarantor')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (18, 2, N'Processed By')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (19, 2, N'Approved By')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (20, 4, N'Fully Own')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (21, 4, N'Partially Own')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (22, 4, N'Mortgagee')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (23, 5, N'Owner')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (24, 5, N'Co-Owner')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (25, 3, N'Borrower')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (26, 3, N'Co-Borrower')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (27, 3, N'Guarantor')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (28, 3, N'Approved By')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (29, 3, N'Witnessed By')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (30, 3, N'Legal Council')
INSERT [dbo].[RoleType] ([Id], [ParentRoleTypeId], [Name]) VALUES (31, 5, N'Guarantor')
SET IDENTITY_INSERT [dbo].[RoleType] OFF
/****** Object:  Table [dbo].[RequiredDocumentType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RequiredDocumentType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK_REQUIREDDOCUMENTTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[RequiredDocumentType] ON
INSERT [dbo].[RequiredDocumentType] ([Id], [Name]) VALUES (1, N'Land Title (Owner’s Copy)')
INSERT [dbo].[RequiredDocumentType] ([Id], [Name]) VALUES (2, N'Certificate of Employment')
INSERT [dbo].[RequiredDocumentType] ([Id], [Name]) VALUES (3, N'3 Months Payslip')
INSERT [dbo].[RequiredDocumentType] ([Id], [Name]) VALUES (4, N'Tax Clearance')
INSERT [dbo].[RequiredDocumentType] ([Id], [Name]) VALUES (5, N'Certification')
INSERT [dbo].[RequiredDocumentType] ([Id], [Name]) VALUES (6, N'Community Tax (Res. Cert.) Photocopy')
INSERT [dbo].[RequiredDocumentType] ([Id], [Name]) VALUES (7, N'Sketch/Location Plan')
INSERT [dbo].[RequiredDocumentType] ([Id], [Name]) VALUES (8, N'T. I. N. No.')
INSERT [dbo].[RequiredDocumentType] ([Id], [Name]) VALUES (9, N'Affidavit Of Non-Tenancy (For Agricultural Land Only)')
INSERT [dbo].[RequiredDocumentType] ([Id], [Name]) VALUES (10, N'Two (2) Valid ID With Picture')
SET IDENTITY_INSERT [dbo].[RequiredDocumentType] OFF
/****** Object:  Table [dbo].[PersonNameType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PersonNameType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PERSONNAMETYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[PersonNameType] ON
INSERT [dbo].[PersonNameType] ([Id], [Name]) VALUES (1, N'Title')
INSERT [dbo].[PersonNameType] ([Id], [Name]) VALUES (2, N'First Name')
INSERT [dbo].[PersonNameType] ([Id], [Name]) VALUES (3, N'Middle Name')
INSERT [dbo].[PersonNameType] ([Id], [Name]) VALUES (4, N'Last Name')
INSERT [dbo].[PersonNameType] ([Id], [Name]) VALUES (5, N'Nick Name')
INSERT [dbo].[PersonNameType] ([Id], [Name]) VALUES (6, N'Name Suffix')
INSERT [dbo].[PersonNameType] ([Id], [Name]) VALUES (7, N'Mother''s Maiden Name')
SET IDENTITY_INSERT [dbo].[PersonNameType] OFF
/****** Object:  Table [dbo].[ProductFeatureCategory]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductFeatureCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PRODUCTFEATURECATEGORY] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ProductFeatureCategory] ON
INSERT [dbo].[ProductFeatureCategory] ([Id], [Name]) VALUES (1, N'Collateral Requirement')
INSERT [dbo].[ProductFeatureCategory] ([Id], [Name]) VALUES (2, N'Loan Limit')
INSERT [dbo].[ProductFeatureCategory] ([Id], [Name]) VALUES (3, N'Loan Term')
INSERT [dbo].[ProductFeatureCategory] ([Id], [Name]) VALUES (4, N'Interest Computation Mode')
INSERT [dbo].[ProductFeatureCategory] ([Id], [Name]) VALUES (5, N'Method of Charging Interest')
INSERT [dbo].[ProductFeatureCategory] ([Id], [Name]) VALUES (6, N'Interest Rate')
INSERT [dbo].[ProductFeatureCategory] ([Id], [Name]) VALUES (7, N'Past Due Interest Rate')
INSERT [dbo].[ProductFeatureCategory] ([Id], [Name]) VALUES (8, N'Fee')
SET IDENTITY_INSERT [dbo].[ProductFeatureCategory] OFF
/****** Object:  Table [dbo].[PettyCashLoanAppStatusType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PettyCashLoanAppStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PETTY_CASH_LOAN_APP_STATUS_2] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[PettyCashLoanAppStatusType] ON
INSERT [dbo].[PettyCashLoanAppStatusType] ([Id], [Name]) VALUES (1, N'Pending: Approval')
INSERT [dbo].[PettyCashLoanAppStatusType] ([Id], [Name]) VALUES (2, N'Approved')
INSERT [dbo].[PettyCashLoanAppStatusType] ([Id], [Name]) VALUES (3, N'Rejected')
INSERT [dbo].[PettyCashLoanAppStatusType] ([Id], [Name]) VALUES (4, N'Pending: In Funding')
INSERT [dbo].[PettyCashLoanAppStatusType] ([Id], [Name]) VALUES (5, N'Closed')
INSERT [dbo].[PettyCashLoanAppStatusType] ([Id], [Name]) VALUES (6, N'Cancelled')
INSERT [dbo].[PettyCashLoanAppStatusType] ([Id], [Name]) VALUES (7, N'Pending: Endorsement')
SET IDENTITY_INSERT [dbo].[PettyCashLoanAppStatusType] OFF
/****** Object:  Table [dbo].[VehicleType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VehicleType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VehicleType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[VehicleType] ON
INSERT [dbo].[VehicleType] ([Id], [Name]) VALUES (1, N'Car')
INSERT [dbo].[VehicleType] ([Id], [Name]) VALUES (2, N'Motorcycle')
INSERT [dbo].[VehicleType] ([Id], [Name]) VALUES (3, N'Tricycle')
INSERT [dbo].[VehicleType] ([Id], [Name]) VALUES (4, N'Jeepney')
INSERT [dbo].[VehicleType] ([Id], [Name]) VALUES (5, N'Ship')
INSERT [dbo].[VehicleType] ([Id], [Name]) VALUES (6, N'Boat')
INSERT [dbo].[VehicleType] ([Id], [Name]) VALUES (7, N'Aircraft')
SET IDENTITY_INSERT [dbo].[VehicleType] OFF
/****** Object:  Table [dbo].[FinancialAccountType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FinancialAccountType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_FINANCIALACCOUNTTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[FinancialAccountType] ON
INSERT [dbo].[FinancialAccountType] ([Id], [Name]) VALUES (1, N'Loan Account')
INSERT [dbo].[FinancialAccountType] ([Id], [Name]) VALUES (2, N'Deposit Account')
SET IDENTITY_INSERT [dbo].[FinancialAccountType] OFF
/****** Object:  Table [dbo].[FinancialProduct]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FinancialProduct](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[IntroductionDate] [datetime] NOT NULL,
	[SalesDiscontinuationDate] [datetime] NULL,
	[Comment] [varchar](max) NULL,
 CONSTRAINT [PK_FINANCIALPRODUCT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[FinancialProduct] ON
INSERT [dbo].[FinancialProduct] ([Id], [Name], [IntroductionDate], [SalesDiscontinuationDate], [Comment]) VALUES (1, N'Salary Loan', CAST(0x000080B000000000 AS DateTime), NULL, N'')
SET IDENTITY_INSERT [dbo].[FinancialProduct] OFF
/****** Object:  Table [dbo].[FinancialAcctNotificationTyp]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FinancialAcctNotificationTyp](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_FINANCIALACCTNOTIFICATIONTY] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[FinancialAcctNotificationTyp] ON
INSERT [dbo].[FinancialAcctNotificationTyp] ([Id], [Name]) VALUES (1, N'Invoice')
INSERT [dbo].[FinancialAcctNotificationTyp] ([Id], [Name]) VALUES (2, N'Statement')
SET IDENTITY_INSERT [dbo].[FinancialAcctNotificationTyp] OFF
/****** Object:  Table [dbo].[FinlAcctTransType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FinlAcctTransType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentFinancialAcctTransTypeId] [int] NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_FINLACCTTRANSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[FinlAcctTransType] ON
INSERT [dbo].[FinlAcctTransType] ([Id], [ParentFinancialAcctTransTypeId], [Name]) VALUES (1, NULL, N'Account Payment')
INSERT [dbo].[FinlAcctTransType] ([Id], [ParentFinancialAcctTransTypeId], [Name]) VALUES (2, NULL, N'Account Fee')
INSERT [dbo].[FinlAcctTransType] ([Id], [ParentFinancialAcctTransTypeId], [Name]) VALUES (3, NULL, N'Deposit')
INSERT [dbo].[FinlAcctTransType] ([Id], [ParentFinancialAcctTransTypeId], [Name]) VALUES (4, NULL, N'Withdrawal')
INSERT [dbo].[FinlAcctTransType] ([Id], [ParentFinancialAcctTransTypeId], [Name]) VALUES (5, NULL, N'Financial Account Adjustment')
SET IDENTITY_INSERT [dbo].[FinlAcctTransType] OFF
/****** Object:  Table [dbo].[EmployeePositionType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmployeePositionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_EmployeePositionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[EmployeePositionType] ON
INSERT [dbo].[EmployeePositionType] ([Id], [Name]) VALUES (1, N'Owner')
INSERT [dbo].[EmployeePositionType] ([Id], [Name]) VALUES (2, N'Teller')
INSERT [dbo].[EmployeePositionType] ([Id], [Name]) VALUES (3, N'Accountant')
INSERT [dbo].[EmployeePositionType] ([Id], [Name]) VALUES (4, N'System Administrator')
SET IDENTITY_INSERT [dbo].[EmployeePositionType] OFF
/****** Object:  Table [dbo].[ElectronicAddressType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ElectronicAddressType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ELECTRONICADDRESSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ElectronicAddressType] ON
INSERT [dbo].[ElectronicAddressType] ([Id], [Name]) VALUES (1, N'Personal Email Address')
INSERT [dbo].[ElectronicAddressType] ([Id], [Name]) VALUES (2, N'Business Email Address')
SET IDENTITY_INSERT [dbo].[ElectronicAddressType] OFF
/****** Object:  Table [dbo].[EducAttainmentType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EducAttainmentType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_EDUCATTAINMENTTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[EducAttainmentType] ON
INSERT [dbo].[EducAttainmentType] ([Id], [Name]) VALUES (1, N'Elementary Graduate')
INSERT [dbo].[EducAttainmentType] ([Id], [Name]) VALUES (2, N'High School Graduate')
INSERT [dbo].[EducAttainmentType] ([Id], [Name]) VALUES (3, N'College Undergraduate')
INSERT [dbo].[EducAttainmentType] ([Id], [Name]) VALUES (4, N'College Graduate')
INSERT [dbo].[EducAttainmentType] ([Id], [Name]) VALUES (5, N'Post-Graduate')
SET IDENTITY_INSERT [dbo].[EducAttainmentType] OFF
/****** Object:  Table [dbo].[DistrictType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DistrictType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_DistrictType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[DistrictType] ON
INSERT [dbo].[DistrictType] ([Id], [Name]) VALUES (1, N'City')
INSERT [dbo].[DistrictType] ([Id], [Name]) VALUES (2, N'Elementary')
INSERT [dbo].[DistrictType] ([Id], [Name]) VALUES (3, N'High School')
INSERT [dbo].[DistrictType] ([Id], [Name]) VALUES (4, N'Insular')
INSERT [dbo].[DistrictType] ([Id], [Name]) VALUES (5, N'COA')
INSERT [dbo].[DistrictType] ([Id], [Name]) VALUES (6, N'FIDA')
INSERT [dbo].[DistrictType] ([Id], [Name]) VALUES (7, N'MSU')
SET IDENTITY_INSERT [dbo].[DistrictType] OFF
/****** Object:  Table [dbo].[FinAcctTransStatusType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FinAcctTransStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_FINACCTTRANSSTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[FinAcctTransStatusType] ON
INSERT [dbo].[FinAcctTransStatusType] ([Id], [Name]) VALUES (1, N'Posted')
INSERT [dbo].[FinAcctTransStatusType] ([Id], [Name]) VALUES (2, N'On Hold')
SET IDENTITY_INSERT [dbo].[FinAcctTransStatusType] OFF
/****** Object:  Table [dbo].[FinAcctTransRelType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FinAcctTransRelType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_FINACCTTRANSRELTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ExchangeRateType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeRateType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ExchangeRateType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ExchangeRateType] ON
INSERT [dbo].[ExchangeRateType] ([Id], [Name]) VALUES (1, N'Buying')
INSERT [dbo].[ExchangeRateType] ([Id], [Name]) VALUES (2, N'Selling')
INSERT [dbo].[ExchangeRateType] ([Id], [Name]) VALUES (3, N'Average')
INSERT [dbo].[ExchangeRateType] ([Id], [Name]) VALUES (4, N'Spot')
SET IDENTITY_INSERT [dbo].[ExchangeRateType] OFF
/****** Object:  Table [dbo].[DisbursementType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DisbursementType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_DISBURSEMENTTYPE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[DisbursementType] ON
INSERT [dbo].[DisbursementType] ([Id], [Name]) VALUES (1, N'Loan Disbursement')
INSERT [dbo].[DisbursementType] ([Id], [Name]) VALUES (2, N'Other Loan Disbursement')
INSERT [dbo].[DisbursementType] ([Id], [Name]) VALUES (3, N'Encashment')
INSERT [dbo].[DisbursementType] ([Id], [Name]) VALUES (4, N'Change')
INSERT [dbo].[DisbursementType] ([Id], [Name]) VALUES (5, N'Rediscounting')
INSERT [dbo].[DisbursementType] ([Id], [Name]) VALUES (6, N'Advance Change')
SET IDENTITY_INSERT [dbo].[DisbursementType] OFF
/****** Object:  Table [dbo].[DisbursementVcrStatusType]    Script Date: 02/22/2012 10:28:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DisbursementVcrStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_DISBURSEMENTVCRSTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[DisbursementVcrStatusType] ON
INSERT [dbo].[DisbursementVcrStatusType] ([Id], [Name]) VALUES (1, N'Pending')
INSERT [dbo].[DisbursementVcrStatusType] ([Id], [Name]) VALUES (2, N'Approved')
INSERT [dbo].[DisbursementVcrStatusType] ([Id], [Name]) VALUES (3, N'Rejected')
INSERT [dbo].[DisbursementVcrStatusType] ([Id], [Name]) VALUES (4, N'Cancelled')
INSERT [dbo].[DisbursementVcrStatusType] ([Id], [Name]) VALUES (5, N'Partially Disbursed')
INSERT [dbo].[DisbursementVcrStatusType] ([Id], [Name]) VALUES (6, N'Fully Disbursed')
SET IDENTITY_INSERT [dbo].[DisbursementVcrStatusType] OFF
/****** Object:  Table [dbo].[DemandLetterStatusType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DemandLetterStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NULL,
 CONSTRAINT [PK_DEMANDLETTERSTATUSTYPE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[DemandLetterStatusType] ON
INSERT [dbo].[DemandLetterStatusType] ([Id], [Name]) VALUES (1, N'Require First Demand Letter')
INSERT [dbo].[DemandLetterStatusType] ([Id], [Name]) VALUES (2, N'First Demand Letter Sent')
INSERT [dbo].[DemandLetterStatusType] ([Id], [Name]) VALUES (3, N'Require Final Demand Letter')
INSERT [dbo].[DemandLetterStatusType] ([Id], [Name]) VALUES (4, N'Final Demand Letter Sent')
SET IDENTITY_INSERT [dbo].[DemandLetterStatusType] OFF
/****** Object:  Table [dbo].[COVTransactionType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COVTransactionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_COVTransactionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[COVTransactionType] ON
INSERT [dbo].[COVTransactionType] ([Id], [Name]) VALUES (1, N'Deposit To Vault')
INSERT [dbo].[COVTransactionType] ([Id], [Name]) VALUES (2, N'Withdraw From Vault')
SET IDENTITY_INSERT [dbo].[COVTransactionType] OFF
/****** Object:  Table [dbo].[CustomerStatusType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomerStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CUSTOMERSTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[CustomerStatusType] ON
INSERT [dbo].[CustomerStatusType] ([Id], [Name]) VALUES (1, N'New')
INSERT [dbo].[CustomerStatusType] ([Id], [Name]) VALUES (2, N'Active')
INSERT [dbo].[CustomerStatusType] ([Id], [Name]) VALUES (3, N'Delinquent')
INSERT [dbo].[CustomerStatusType] ([Id], [Name]) VALUES (4, N'Subprime')
INSERT [dbo].[CustomerStatusType] ([Id], [Name]) VALUES (5, N'Inactive')
SET IDENTITY_INSERT [dbo].[CustomerStatusType] OFF
/****** Object:  Table [dbo].[CustomerCategoryType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomerCategoryType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NULL,
 CONSTRAINT [PK_CUSTOMERCATEGORYTYPE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[CustomerCategoryType] ON
INSERT [dbo].[CustomerCategoryType] ([Id], [Name]) VALUES (1, N'Teacher')
INSERT [dbo].[CustomerCategoryType] ([Id], [Name]) VALUES (2, N'Others')
SET IDENTITY_INSERT [dbo].[CustomerCategoryType] OFF
/****** Object:  Table [dbo].[Currency]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Currency](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Symbol] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
 CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Currency] ON
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (1, N'PHP', N'Philippine Peso')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (2, N'USD', N'United States Dollar')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (3, N'GBP', N'British Pound')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (4, N'JPY', N'Japanese Yen')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (6, N'KRW', N'Korean Won')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (7, N'EUR', N'Euro')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (8, N'BHD', N'Bahraini Dinar')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (9, N'CNY', N'Chinese Yuan')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (10, N'DKK', N'Danish Krone')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (11, N'INR', N'Indian Rupee')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (12, N'KWD', N'Kuwaiti Dinar')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (13, N'QAR', N'Qatari Riyal')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (14, N'AUD', N'Australian Dollar')
INSERT [dbo].[Currency] ([Id], [Symbol], [Description]) VALUES (15, N'CAD', N'Canadian Dollar')
SET IDENTITY_INSERT [dbo].[Currency] OFF
/****** Object:  Table [dbo].[Country]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Country](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryTelephoneCode] [varchar](10) NULL,
	[Name] [varchar](max) NOT NULL,
 CONSTRAINT [PK_COUNTRY] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Country] ON
INSERT [dbo].[Country] ([Id], [CountryTelephoneCode], [Name]) VALUES (1, N'+63', N'Philippines')
SET IDENTITY_INSERT [dbo].[Country] OFF
/****** Object:  Table [dbo].[ChequeStatusType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChequeStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CHEQUESTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ChequeStatusType] ON
INSERT [dbo].[ChequeStatusType] ([Id], [Name]) VALUES (1, N'Received')
INSERT [dbo].[ChequeStatusType] ([Id], [Name]) VALUES (2, N'Deposited')
INSERT [dbo].[ChequeStatusType] ([Id], [Name]) VALUES (3, N'Cleared')
INSERT [dbo].[ChequeStatusType] ([Id], [Name]) VALUES (4, N'Bounced')
INSERT [dbo].[ChequeStatusType] ([Id], [Name]) VALUES (5, N'Cancelled')
INSERT [dbo].[ChequeStatusType] ([Id], [Name]) VALUES (6, N'On Hold')
SET IDENTITY_INSERT [dbo].[ChequeStatusType] OFF
/****** Object:  Table [dbo].[BillAmount]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillAmount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_BillAmount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BillAmount] ON
INSERT [dbo].[BillAmount] ([Id], [Amount]) VALUES (1, CAST(1000.00 AS Decimal(18, 2)))
INSERT [dbo].[BillAmount] ([Id], [Amount]) VALUES (2, CAST(500.00 AS Decimal(18, 2)))
INSERT [dbo].[BillAmount] ([Id], [Amount]) VALUES (3, CAST(200.00 AS Decimal(18, 2)))
INSERT [dbo].[BillAmount] ([Id], [Amount]) VALUES (4, CAST(100.00 AS Decimal(18, 2)))
INSERT [dbo].[BillAmount] ([Id], [Amount]) VALUES (5, CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[BillAmount] ([Id], [Amount]) VALUES (6, CAST(10.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[BillAmount] OFF
/****** Object:  Table [dbo].[BankAccountType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BankAccountType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_BANKACCOUNTTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[BankAccountType] ON
INSERT [dbo].[BankAccountType] ([Id], [Name]) VALUES (1, N'Savings')
INSERT [dbo].[BankAccountType] ([Id], [Name]) VALUES (2, N'Current')
INSERT [dbo].[BankAccountType] ([Id], [Name]) VALUES (3, N'Time Deposit')
SET IDENTITY_INSERT [dbo].[BankAccountType] OFF
/****** Object:  Table [dbo].[BankStatusType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BankStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_BANKSTATUSTYPE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[BankStatusType] ON
INSERT [dbo].[BankStatusType] ([Id], [Name]) VALUES (1, N'Active')
INSERT [dbo].[BankStatusType] ([Id], [Name]) VALUES (2, N'Inactive')
SET IDENTITY_INSERT [dbo].[BankStatusType] OFF
/****** Object:  Table [dbo].[AdjustmentType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AdjustmentType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[IsCreditIndicator] [bit] NOT NULL,
 CONSTRAINT [PK_ADJUSTMENTTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[AdjustmentType] ON
INSERT [dbo].[AdjustmentType] ([Id], [Name], [IsCreditIndicator]) VALUES (1, N'Waived Penalty', 1)
INSERT [dbo].[AdjustmentType] ([Id], [Name], [IsCreditIndicator]) VALUES (2, N'Waive', 1)
INSERT [dbo].[AdjustmentType] ([Id], [Name], [IsCreditIndicator]) VALUES (3, N'Rebate', 1)
INSERT [dbo].[AdjustmentType] ([Id], [Name], [IsCreditIndicator]) VALUES (4, N'Additional Interest', 1)
SET IDENTITY_INSERT [dbo].[AdjustmentType] OFF
/****** Object:  Table [dbo].[AddressType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AddressType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ADDRESSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[AddressType] ON
INSERT [dbo].[AddressType] ([Id], [Name]) VALUES (1, N'Postal Address')
INSERT [dbo].[AddressType] ([Id], [Name]) VALUES (2, N'Telecommunication Number')
INSERT [dbo].[AddressType] ([Id], [Name]) VALUES (3, N'Electronic Address')
SET IDENTITY_INSERT [dbo].[AddressType] OFF
/****** Object:  Table [dbo].[AgreementType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AgreementType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_AGREEMENTTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[AgreementType] ON
INSERT [dbo].[AgreementType] ([Id], [Name]) VALUES (1, N'Loan Agreement')
INSERT [dbo].[AgreementType] ([Id], [Name]) VALUES (2, N'Compromise Agreement')
SET IDENTITY_INSERT [dbo].[AgreementType] OFF
/****** Object:  Table [dbo].[AssetType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[IsAppraisableIndicator] [bit] NOT NULL,
 CONSTRAINT [PK_ASSETTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[AssetType] ON
INSERT [dbo].[AssetType] ([Id], [Name], [IsAppraisableIndicator]) VALUES (1, N'Land', 1)
INSERT [dbo].[AssetType] ([Id], [Name], [IsAppraisableIndicator]) VALUES (2, N'Jewelry', 0)
INSERT [dbo].[AssetType] ([Id], [Name], [IsAppraisableIndicator]) VALUES (3, N'Vehicle', 1)
INSERT [dbo].[AssetType] ([Id], [Name], [IsAppraisableIndicator]) VALUES (4, N'Machine', 0)
INSERT [dbo].[AssetType] ([Id], [Name], [IsAppraisableIndicator]) VALUES (5, N'Bank Account', 1)
INSERT [dbo].[AssetType] ([Id], [Name], [IsAppraisableIndicator]) VALUES (6, N'Others', 0)
SET IDENTITY_INSERT [dbo].[AssetType] OFF
/****** Object:  Table [dbo].[ApplicationType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_APPLICATIONTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ApplicationType] ON
INSERT [dbo].[ApplicationType] ([Id], [Name]) VALUES (1, N'Loan Application')
SET IDENTITY_INSERT [dbo].[ApplicationType] OFF
/****** Object:  Table [dbo].[LoanModificationType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LoanModificationType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_LOANMODIFICATIONTYPE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[LoanModificationType] ON
INSERT [dbo].[LoanModificationType] ([Id], [Name]) VALUES (1, N'Split')
INSERT [dbo].[LoanModificationType] ([Id], [Name]) VALUES (2, N'Consolidate')
INSERT [dbo].[LoanModificationType] ([Id], [Name]) VALUES (3, N'Change ICM')
INSERT [dbo].[LoanModificationType] ([Id], [Name]) VALUES (4, N'Additional Loan')
INSERT [dbo].[LoanModificationType] ([Id], [Name]) VALUES (5, N'Change Interest')
SET IDENTITY_INSERT [dbo].[LoanModificationType] OFF
/****** Object:  Table [dbo].[LoanModificationStatusType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LoanModificationStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_LOANMODIFICATIONSTATUSTYPE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[LoanModificationStatusType] ON
INSERT [dbo].[LoanModificationStatusType] ([Id], [Name]) VALUES (1, N'Pending: Approval')
INSERT [dbo].[LoanModificationStatusType] ([Id], [Name]) VALUES (2, N'Approved')
INSERT [dbo].[LoanModificationStatusType] ([Id], [Name]) VALUES (3, N'Rejected')
SET IDENTITY_INSERT [dbo].[LoanModificationStatusType] OFF
/****** Object:  Table [dbo].[LoanDisbursementType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanDisbursementType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_LoanDisbursementType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[LoanDisbursementType] ON
INSERT [dbo].[LoanDisbursementType] ([Id], [Name]) VALUES (1, N'First Availment')
INSERT [dbo].[LoanDisbursementType] ([Id], [Name]) VALUES (2, N'Additional')
INSERT [dbo].[LoanDisbursementType] ([Id], [Name]) VALUES (3, N'Advance Change')
SET IDENTITY_INSERT [dbo].[LoanDisbursementType] OFF
/****** Object:  Table [dbo].[LoanApplicationStatusType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LoanApplicationStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_LOANAPPLICATIONSTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[LoanApplicationStatusType] ON
INSERT [dbo].[LoanApplicationStatusType] ([Id], [Name]) VALUES (1, N'Pending: Approval')
INSERT [dbo].[LoanApplicationStatusType] ([Id], [Name]) VALUES (2, N'Pending: In Funding')
INSERT [dbo].[LoanApplicationStatusType] ([Id], [Name]) VALUES (3, N'Rejected')
INSERT [dbo].[LoanApplicationStatusType] ([Id], [Name]) VALUES (4, N'Cancelled')
INSERT [dbo].[LoanApplicationStatusType] ([Id], [Name]) VALUES (5, N'Closed')
INSERT [dbo].[LoanApplicationStatusType] ([Id], [Name]) VALUES (6, N'Pending: Endorsement')
INSERT [dbo].[LoanApplicationStatusType] ([Id], [Name]) VALUES (7, N'Approved')
INSERT [dbo].[LoanApplicationStatusType] ([Id], [Name]) VALUES (8, N'Restructured')
SET IDENTITY_INSERT [dbo].[LoanApplicationStatusType] OFF
/****** Object:  Table [dbo].[LoanAccountStatusType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LoanAccountStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_LOANACCOUNTSTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[LoanAccountStatusType] ON
INSERT [dbo].[LoanAccountStatusType] ([Id], [Name]) VALUES (1, N'Current')
INSERT [dbo].[LoanAccountStatusType] ([Id], [Name]) VALUES (2, N'Delinquent')
INSERT [dbo].[LoanAccountStatusType] ([Id], [Name]) VALUES (3, N'Paid-Off')
INSERT [dbo].[LoanAccountStatusType] ([Id], [Name]) VALUES (4, N'Written-Off')
INSERT [dbo].[LoanAccountStatusType] ([Id], [Name]) VALUES (5, N'Under Litigation')
INSERT [dbo].[LoanAccountStatusType] ([Id], [Name]) VALUES (6, N'Restructured')
INSERT [dbo].[LoanAccountStatusType] ([Id], [Name]) VALUES (7, N'Pending: Endorsement')
SET IDENTITY_INSERT [dbo].[LoanAccountStatusType] OFF
/****** Object:  Table [dbo].[LandType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LandType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](1024) NULL,
 CONSTRAINT [PK_LANDTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[LandType] ON
INSERT [dbo].[LandType] ([Id], [Name], [Description]) VALUES (1, N'Urban', N'Urban')
INSERT [dbo].[LandType] ([Id], [Name], [Description]) VALUES (2, N'Agricultural', N'Agricultural')
INSERT [dbo].[LandType] ([Id], [Name], [Description]) VALUES (3, N'Mineral', N'Mineral')
INSERT [dbo].[LandType] ([Id], [Name], [Description]) VALUES (4, N'Timberland', N'Timberland')
SET IDENTITY_INSERT [dbo].[LandType] OFF
/****** Object:  Table [dbo].[Holiday]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Holiday](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](500) NULL,
	[Notes] [varchar](500) NULL,
 CONSTRAINT [PK_HOLIDAY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InterestType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InterestType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_InterestType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[InterestType] ON
INSERT [dbo].[InterestType] ([Id], [Name]) VALUES (1, N'Fixed')
INSERT [dbo].[InterestType] ([Id], [Name]) VALUES (2, N'Zero')
INSERT [dbo].[InterestType] ([Id], [Name]) VALUES (3, N'Percentage')
SET IDENTITY_INSERT [dbo].[InterestType] OFF
/****** Object:  Table [dbo].[IdentificationType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdentificationType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](1024) NULL,
 CONSTRAINT [PK_IDENTIFICATIONTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[IdentificationType] ON
INSERT [dbo].[IdentificationType] ([Id], [Name], [Description]) VALUES (1, N'Drivers License', NULL)
INSERT [dbo].[IdentificationType] ([Id], [Name], [Description]) VALUES (2, N'Company ID', NULL)
INSERT [dbo].[IdentificationType] ([Id], [Name], [Description]) VALUES (3, N'SSS ID', NULL)
INSERT [dbo].[IdentificationType] ([Id], [Name], [Description]) VALUES (4, N'Postal ID', NULL)
SET IDENTITY_INSERT [dbo].[IdentificationType] OFF
/****** Object:  Table [dbo].[HomeOwnershipType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HomeOwnershipType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_HOMEOWNERSHIPTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[HomeOwnershipType] ON
INSERT [dbo].[HomeOwnershipType] ([Id], [Name]) VALUES (1, N'Owned')
INSERT [dbo].[HomeOwnershipType] ([Id], [Name]) VALUES (2, N'Rented')
INSERT [dbo].[HomeOwnershipType] ([Id], [Name]) VALUES (3, N'Living with Relatives')
INSERT [dbo].[HomeOwnershipType] ([Id], [Name]) VALUES (4, N'Company Provided')
SET IDENTITY_INSERT [dbo].[HomeOwnershipType] OFF
/****** Object:  Table [dbo].[GenderType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GenderType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_GENDERTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[GenderType] ON
INSERT [dbo].[GenderType] ([Id], [Name]) VALUES (1, N'Female')
INSERT [dbo].[GenderType] ([Id], [Name]) VALUES (2, N'Male')
SET IDENTITY_INSERT [dbo].[GenderType] OFF
/****** Object:  Table [dbo].[FormType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FormType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_FormType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[FormType] ON
INSERT [dbo].[FormType] ([Id], [Name]) VALUES (1, N'Transaction Slip')
INSERT [dbo].[FormType] ([Id], [Name]) VALUES (2, N'Payment Form')
INSERT [dbo].[FormType] ([Id], [Name]) VALUES (3, N'Promissory Note')
INSERT [dbo].[FormType] ([Id], [Name]) VALUES (4, N'SPA')
INSERT [dbo].[FormType] ([Id], [Name]) VALUES (5, N'Encashment Form')
INSERT [dbo].[FormType] ([Id], [Name]) VALUES (6, N'Rediscounting Form')
INSERT [dbo].[FormType] ([Id], [Name]) VALUES (7, N'Change Form')
INSERT [dbo].[FormType] ([Id], [Name]) VALUES (8, N'Other Disbursement Form')
SET IDENTITY_INSERT [dbo].[FormType] OFF
/****** Object:  Table [dbo].[NonProductFee]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NonProductFee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NOT NULL,
 CONSTRAINT [PK_NonProductFee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[NonProductFee] ON
INSERT [dbo].[NonProductFee] ([Id], [Name]) VALUES (1, N'Rentals')
INSERT [dbo].[NonProductFee] ([Id], [Name]) VALUES (2, N'Bank Interest')
INSERT [dbo].[NonProductFee] ([Id], [Name]) VALUES (3, N'DAR')
INSERT [dbo].[NonProductFee] ([Id], [Name]) VALUES (4, N'Personal Checks')
INSERT [dbo].[NonProductFee] ([Id], [Name]) VALUES (5, N'Titles')
INSERT [dbo].[NonProductFee] ([Id], [Name]) VALUES (6, N'BIR')
INSERT [dbo].[NonProductFee] ([Id], [Name]) VALUES (7, N'Budget')
INSERT [dbo].[NonProductFee] ([Id], [Name]) VALUES (8, N'BAS')
INSERT [dbo].[NonProductFee] ([Id], [Name]) VALUES (9, N'OS')
INSERT [dbo].[NonProductFee] ([Id], [Name]) VALUES (10, N'Court')
SET IDENTITY_INSERT [dbo].[NonProductFee] OFF
/****** Object:  Table [dbo].[NationalityType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NationalityType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_NATIONALITYTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[NationalityType] ON
INSERT [dbo].[NationalityType] ([Id], [Name]) VALUES (1, N'Filipino')
INSERT [dbo].[NationalityType] ([Id], [Name]) VALUES (2, N'Others')
SET IDENTITY_INSERT [dbo].[NationalityType] OFF
/****** Object:  Table [dbo].[MaritalStatusType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MaritalStatusType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MARITALSTATUSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[MaritalStatusType] ON
INSERT [dbo].[MaritalStatusType] ([Id], [Name]) VALUES (1, N'Single')
INSERT [dbo].[MaritalStatusType] ([Id], [Name]) VALUES (2, N'Married')
INSERT [dbo].[MaritalStatusType] ([Id], [Name]) VALUES (3, N'Legally Separated')
INSERT [dbo].[MaritalStatusType] ([Id], [Name]) VALUES (4, N'Divorced')
INSERT [dbo].[MaritalStatusType] ([Id], [Name]) VALUES (5, N'Annulled')
INSERT [dbo].[MaritalStatusType] ([Id], [Name]) VALUES (6, N'Widowed')
SET IDENTITY_INSERT [dbo].[MaritalStatusType] OFF
/****** Object:  Table [dbo].[OrganizationType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrganizationType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_ORGANIZATIONTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[OrganizationType] ON
INSERT [dbo].[OrganizationType] ([Id], [Name], [Description]) VALUES (1, N'Internal', NULL)
INSERT [dbo].[OrganizationType] ([Id], [Name], [Description]) VALUES (2, N'External', NULL)
SET IDENTITY_INSERT [dbo].[OrganizationType] OFF
/****** Object:  Table [dbo].[PartyRelType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartyRelType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PARTYRELTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[PartyRelType] ON
INSERT [dbo].[PartyRelType] ([Id], [Name]) VALUES (1, N'Employment')
INSERT [dbo].[PartyRelType] ([Id], [Name]) VALUES (2, N'Customer Relationship')
INSERT [dbo].[PartyRelType] ([Id], [Name]) VALUES (3, N'Contact Relationship')
INSERT [dbo].[PartyRelType] ([Id], [Name]) VALUES (4, N'Spousal Relationship')
SET IDENTITY_INSERT [dbo].[PartyRelType] OFF
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](256) NULL,
 CONSTRAINT [PK_PRODUCTCATEGORY] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ProductCategory] ON
INSERT [dbo].[ProductCategory] ([Id], [Name], [Description]) VALUES (1, N'Loan Product', NULL)
SET IDENTITY_INSERT [dbo].[ProductCategory] OFF
/****** Object:  Table [dbo].[PostalAddressType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PostalAddressType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_POSTALADDRESSTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[PostalAddressType] ON
INSERT [dbo].[PostalAddressType] ([Id], [Name]) VALUES (1, N'Home Address')
INSERT [dbo].[PostalAddressType] ([Id], [Name]) VALUES (2, N'Business Address')
INSERT [dbo].[PostalAddressType] ([Id], [Name]) VALUES (3, N'Billing Address')
INSERT [dbo].[PostalAddressType] ([Id], [Name]) VALUES (4, N'Birthplace')
INSERT [dbo].[PostalAddressType] ([Id], [Name]) VALUES (5, N'Property Location')
SET IDENTITY_INSERT [dbo].[PostalAddressType] OFF
/****** Object:  Table [dbo].[PaymentType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PaymentType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_PAYMENTTYPE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[PaymentType] ON
INSERT [dbo].[PaymentType] ([Id], [Name]) VALUES (1, N'Receipt')
INSERT [dbo].[PaymentType] ([Id], [Name]) VALUES (2, N'Disbursement')
INSERT [dbo].[PaymentType] ([Id], [Name]) VALUES (3, N'Loan Payment')
INSERT [dbo].[PaymentType] ([Id], [Name]) VALUES (4, N'Fee Payment')
SET IDENTITY_INSERT [dbo].[PaymentType] OFF
/****** Object:  Table [dbo].[PaymentMethodType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PaymentMethodType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PAYMENTMETHODTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[PaymentMethodType] ON
INSERT [dbo].[PaymentMethodType] ([Id], [Name]) VALUES (1, N'Cash')
INSERT [dbo].[PaymentMethodType] ([Id], [Name]) VALUES (2, N'Pay Check')
INSERT [dbo].[PaymentMethodType] ([Id], [Name]) VALUES (3, N'Personal Check')
INSERT [dbo].[PaymentMethodType] ([Id], [Name]) VALUES (4, N'ATM')
SET IDENTITY_INSERT [dbo].[PaymentMethodType] OFF
/****** Object:  Table [dbo].[OtherDisbursementParticular]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OtherDisbursementParticular](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NOT NULL,
 CONSTRAINT [PK_OtherDisbursementParticular] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[OtherDisbursementParticular] ON
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (1, N'Salaries & Wages')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (2, N'Taxes/Licenses')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (3, N'SSS/PAG-IBIG/Philhealth')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (4, N'Petty Cash/Misc. Expenses')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (5, N'SOP')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (6, N'Telephone Expense')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (7, N'Car Registration')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (8, N'Philam Life Insurance')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (9, N'Light & Water Expense')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (10, N'Honorarium')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (11, N'Bookkeeper')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (12, N'Atty''s Fees/Demand Letters etc.')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (13, N'Guard Allowances/Salaries')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (14, N'Kismet Cable/DSL')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (15, N'Computer Program')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (16, N'Scholarship Grants')
INSERT [dbo].[OtherDisbursementParticular] ([Id], [Name]) VALUES (17, N'Edificio del Rey Rental')
SET IDENTITY_INSERT [dbo].[OtherDisbursementParticular] OFF
/****** Object:  Table [dbo].[PartyType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartyType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PARTYTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[PartyType] ON
INSERT [dbo].[PartyType] ([Id], [Name]) VALUES (1, N'Person')
INSERT [dbo].[PartyType] ([Id], [Name]) VALUES (2, N'Organization')
SET IDENTITY_INSERT [dbo].[PartyType] OFF
/****** Object:  Table [dbo].[PartyRoleType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartyRoleType](
	[RoleTypeId] [int] NOT NULL,
 CONSTRAINT [PK_PARTYROLETYPE] PRIMARY KEY NONCLUSTERED 
(
	[RoleTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (7)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (8)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (9)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (10)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (11)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (12)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (13)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (14)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (15)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (16)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (17)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (18)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (19)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (20)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (21)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (22)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (23)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (24)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (25)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (26)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (27)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (28)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (29)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (30)
INSERT [dbo].[PartyRoleType] ([RoleTypeId]) VALUES (31)
/****** Object:  Table [dbo].[Party]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Party](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyTypeId] [int] NOT NULL,
 CONSTRAINT [PK_PARTY] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Party] ON
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (2, 1)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (3, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (4, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (5, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (6, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (7, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (8, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (9, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (10, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (11, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (12, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (13, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (14, 2)
INSERT [dbo].[Party] ([Id], [PartyTypeId]) VALUES (15, 1)
SET IDENTITY_INSERT [dbo].[Party] OFF
/****** Object:  Table [dbo].[ForExDetail]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForExDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[PaymentMethodTypeId] [int] NOT NULL,
	[ParentForExDetailId] [int] NULL,
 CONSTRAINT [PK_ForeignExchangeAmount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationType] [int] NOT NULL,
	[ApplicationDate] [datetime] NOT NULL,
 CONSTRAINT [PK_APPLICATION] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassificationType]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ClassificationType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StationNumber] [varchar](20) NULL,
	[District] [varchar](50) NOT NULL,
	[DistrictTypeId] [int] NULL,
 CONSTRAINT [PK_CLASSIFICATIONTYPE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ClassificationType] ON
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (1, N'', N'Alegria', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (2, N'', N'Balangasan ', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (3, N'', N'Balintawak', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (4, N'', N'Baloyboan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (5, N'', N'Banale', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (6, N'', N'Bogo', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (7, N'', N'Bomba', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (8, N'', N'Buenavista', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (9, N'', N'Bulatok', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (10, N'', N'Bulawan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (11, N'', N'Camalig', 1)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (12, N'', N'Dampalan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (13, N'', N'Danlugan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (14, N'', N'Dao', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (15, N'', N'Datagan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (16, N'', N'Deborok', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (17, N'', N'Ditoray', 3)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (18, N'', N'Dumagoc', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (19, NULL, N'Gatas', 1)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (20, N'', N'Gubac', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (21, N'', N'Gubang', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (22, N'', N'Kagawasan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (23, N'', N'Kahayagan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (24, N'', N'Kalasan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (25, N'', N'Kawit ', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (26, N'', N'La Suerte', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (27, N'', N'Lala', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (28, N'', N'Lapidian', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (29, N'', N'Lenienza', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (30, N'', N'Lison Valley', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (31, N'', N'Lourdes', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (32, N'', N'Lower Sibatang', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (33, N'', N'Lumad', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (34, NULL, N'Lumbia', 1)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (35, N'', N'Macasing', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (36, N'', N'Manga', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (37, N'', N'Muricay', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (38, N'', N'Napolan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (39, N'', N'Palpalan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (40, N'', N'Pedulonan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (41, N'', N'Poloyagan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (42, NULL, N'San Francisco ', 1)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (43, N'', N'San Jose ', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (44, N'', N'San Pedro', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (45, N'', N'Santa Lucia', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (47, N'', N'Santiago', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (48, N'', N'Santo Niño', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (49, N'', N'Tawagan Sur', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (50, N'', N'Tiguma', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (51, N'', N'Tuburan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (52, N'', N'Tulangan', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (53, N'', N'Upper Sibatang', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (54, N'', N'White Beach', 2)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (55, N'', N'Lala NHS', 3)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (56, N'', N'Lison Valley NHS', 3)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (57, N'', N'Manga NHS', 3)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (58, N'', N'Napolan NHS', 3)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (59, N'', N'Tawagan Sur NHS', 3)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (60, N'', N'Lower Sibatang Annex', 3)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (61, N'', N'Lakewood', 1)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (62, N'', N'Lakewood NHS', 3)
INSERT [dbo].[ClassificationType] ([Id], [StationNumber], [District], [DistrictTypeId]) VALUES (63, N'', N'Banale NHS', 3)
SET IDENTITY_INSERT [dbo].[ClassificationType] OFF
/****** Object:  Table [dbo].[ExchangeRate]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeRate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyFromId] [int] NOT NULL,
	[CurrencyToId] [int] NOT NULL,
	[ExchangeRateTypeId] [int] NOT NULL,
	[Rate] [decimal](18, 4) NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ExchangeRate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ExchangeRate] ON
INSERT [dbo].[ExchangeRate] ([Id], [CurrencyFromId], [CurrencyToId], [ExchangeRateTypeId], [Rate], [Date], [IsActive]) VALUES (1, 2, 1, 2, CAST(43.0000 AS Decimal(18, 4)), CAST(0x00009F9500000000 AS DateTime), 0)
INSERT [dbo].[ExchangeRate] ([Id], [CurrencyFromId], [CurrencyToId], [ExchangeRateTypeId], [Rate], [Date], [IsActive]) VALUES (2, 2, 1, 1, CAST(42.0000 AS Decimal(18, 4)), CAST(0x00009F9500000000 AS DateTime), 0)
INSERT [dbo].[ExchangeRate] ([Id], [CurrencyFromId], [CurrencyToId], [ExchangeRateTypeId], [Rate], [Date], [IsActive]) VALUES (3, 2, 1, 2, CAST(43.0000 AS Decimal(18, 4)), CAST(0x00009F9600000000 AS DateTime), 1)
INSERT [dbo].[ExchangeRate] ([Id], [CurrencyFromId], [CurrencyToId], [ExchangeRateTypeId], [Rate], [Date], [IsActive]) VALUES (4, 2, 1, 1, CAST(42.0000 AS Decimal(18, 4)), CAST(0x00009F9600000000 AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[ExchangeRate] OFF
/****** Object:  Table [dbo].[DisbursementVcrStatTypeAssoc]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DisbursementVcrStatTypeAssoc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FromStatusTypeId] [int] NOT NULL,
	[ToStatusTypeId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_DISBURSEMENTVCRSTATTYPEASSO] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[DisbursementVcrStatTypeAssoc] ON
INSERT [dbo].[DisbursementVcrStatTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (1, 1, 2, CAST(0x00009F2400C0BC8E AS DateTime), NULL)
INSERT [dbo].[DisbursementVcrStatTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (2, 1, 3, CAST(0x00009F2400C0BC8E AS DateTime), NULL)
INSERT [dbo].[DisbursementVcrStatTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (3, 1, 4, CAST(0x00009F2400C0BC8E AS DateTime), NULL)
INSERT [dbo].[DisbursementVcrStatTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (4, 2, 5, CAST(0x00009F2400C0BC8E AS DateTime), NULL)
INSERT [dbo].[DisbursementVcrStatTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (5, 2, 6, CAST(0x00009F2400C0BC8E AS DateTime), NULL)
INSERT [dbo].[DisbursementVcrStatTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (6, 2, 4, CAST(0x00009F2400C0BC8E AS DateTime), NULL)
INSERT [dbo].[DisbursementVcrStatTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (7, 5, 6, CAST(0x00009F2400C0BC8E AS DateTime), NULL)
INSERT [dbo].[DisbursementVcrStatTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (8, 5, 5, CAST(0x00009F2400C0BC8E AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[DisbursementVcrStatTypeAssoc] OFF
/****** Object:  Table [dbo].[ReceiptStatusTypeAssoc]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceiptStatusTypeAssoc](
	[ReceiptStatusTypeAssociation] [int] IDENTITY(1,1) NOT NULL,
	[FromStatusTypeId] [int] NOT NULL,
	[ToStatusTypeId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_RECEIPTSTATUSTYPEASSOC] PRIMARY KEY NONCLUSTERED 
(
	[ReceiptStatusTypeAssociation] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ReceiptStatusTypeAssoc] ON
INSERT [dbo].[ReceiptStatusTypeAssoc] ([ReceiptStatusTypeAssociation], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (1, 1, 2, CAST(0x00009F2400C0BCA6 AS DateTime), NULL)
INSERT [dbo].[ReceiptStatusTypeAssoc] ([ReceiptStatusTypeAssociation], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (2, 1, 3, CAST(0x00009F2400C0BCA6 AS DateTime), NULL)
INSERT [dbo].[ReceiptStatusTypeAssoc] ([ReceiptStatusTypeAssociation], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (3, 1, 4, CAST(0x00009F2400C0BCA6 AS DateTime), NULL)
INSERT [dbo].[ReceiptStatusTypeAssoc] ([ReceiptStatusTypeAssociation], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (4, 2, 3, CAST(0x00009F2400C0BCA6 AS DateTime), NULL)
INSERT [dbo].[ReceiptStatusTypeAssoc] ([ReceiptStatusTypeAssociation], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (5, 2, 4, CAST(0x00009F2400C0BCA6 AS DateTime), NULL)
INSERT [dbo].[ReceiptStatusTypeAssoc] ([ReceiptStatusTypeAssociation], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (6, 3, 4, CAST(0x00009F2400C0BCA6 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[ReceiptStatusTypeAssoc] OFF
/****** Object:  Table [dbo].[PettyCashLoanAppStatusTypeAssoc]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PettyCashLoanAppStatusTypeAssoc](
	[PettyCashLoanApplicationSta] [int] IDENTITY(1,1) NOT NULL,
	[FromStatusTypeId] [int] NOT NULL,
	[ToStatusTypeId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_PETTYCASHLOANAPPSTATUSTYPEA] PRIMARY KEY NONCLUSTERED 
(
	[PettyCashLoanApplicationSta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanAccountStatusTypeAssoc]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanAccountStatusTypeAssoc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FromStatusTypeId] [int] NOT NULL,
	[ToStatusTypeId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_LOANACCOUNTSTATUSTYPEASSOC] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[LoanAccountStatusTypeAssoc] ON
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (1, 1, 2, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (2, 1, 6, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (3, 1, 3, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (4, 2, 3, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (5, 2, 4, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (6, 2, 5, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (7, 2, 6, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (8, 5, 3, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (9, 5, 4, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (10, 5, 6, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (11, 7, 1, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (12, 7, 2, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (13, 7, 5, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (14, 7, 4, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (15, 7, 3, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (16, 6, 6, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (17, 6, 3, CAST(0x00009F2400000000 AS DateTime), NULL)
INSERT [dbo].[LoanAccountStatusTypeAssoc] ([Id], [FromStatusTypeId], [ToStatusTypeId], [EffectiveDate], [EndDate]) VALUES (18, 6, 2, CAST(0x00009F2400C0BC9E AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[LoanAccountStatusTypeAssoc] OFF
/****** Object:  Table [dbo].[ProductCategoryClassification]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategoryClassification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductCategoryId] [int] NOT NULL,
	[FinancialProductId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_PRODUCTCATEGORYCLASSIFICATI] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ProductCategoryClassification] ON
INSERT [dbo].[ProductCategoryClassification] ([Id], [ProductCategoryId], [FinancialProductId], [EffectiveDate], [EndDate]) VALUES (1, 1, 1, CAST(0x00009F8301264C25 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[ProductCategoryClassification] OFF
/****** Object:  Table [dbo].[UnitOfMeasure]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UnitOfMeasure](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UomTypeId] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_UNITOFMEASURE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[UnitOfMeasure] ON
INSERT [dbo].[UnitOfMeasure] ([Id], [UomTypeId], [Name]) VALUES (1, 1, N'Day/s')
INSERT [dbo].[UnitOfMeasure] ([Id], [UomTypeId], [Name]) VALUES (2, 1, N'Week/s')
INSERT [dbo].[UnitOfMeasure] ([Id], [UomTypeId], [Name]) VALUES (3, 1, N'Month/s')
INSERT [dbo].[UnitOfMeasure] ([Id], [UomTypeId], [Name]) VALUES (4, 1, N'Year/s')
INSERT [dbo].[UnitOfMeasure] ([Id], [UomTypeId], [Name]) VALUES (5, 2, N'Hectares')
INSERT [dbo].[UnitOfMeasure] ([Id], [UomTypeId], [Name]) VALUES (6, 3, N'Square Meter')
INSERT [dbo].[UnitOfMeasure] ([Id], [UomTypeId], [Name]) VALUES (7, 4, N'Daily')
INSERT [dbo].[UnitOfMeasure] ([Id], [UomTypeId], [Name]) VALUES (8, 4, N'Weekly')
INSERT [dbo].[UnitOfMeasure] ([Id], [UomTypeId], [Name]) VALUES (9, 4, N'Semi-Monthly')
INSERT [dbo].[UnitOfMeasure] ([Id], [UomTypeId], [Name]) VALUES (10, 4, N'Monthly')
INSERT [dbo].[UnitOfMeasure] ([Id], [UomTypeId], [Name]) VALUES (11, 4, N'Annually')
SET IDENTITY_INSERT [dbo].[UnitOfMeasure] OFF
/****** Object:  Table [dbo].[ProductCategoryRollup]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategoryRollup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentProductCategoryId] [int] NULL,
	[ChildProductCategoryId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_PRODUCTCATEGORYROLLUP] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductStatus]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusTypeId] [int] NOT NULL,
	[FinancialProductId] [int] NOT NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[Remarks] [varchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_PRODUCTSTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ProductStatus] ON
INSERT [dbo].[ProductStatus] ([Id], [StatusTypeId], [FinancialProductId], [TransitionDateTime], [Remarks], [IsActive]) VALUES (1, 2, 1, CAST(0x00009F8301264C25 AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[ProductStatus] OFF
/****** Object:  Table [dbo].[ProductRequiredDocument]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductRequiredDocument](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinancialProductId] [int] NOT NULL,
	[RequiredDocumentTypeId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_PRODUCTREQUIREDDOCUMENT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ProductRequiredDocument] ON
INSERT [dbo].[ProductRequiredDocument] ([Id], [FinancialProductId], [RequiredDocumentTypeId], [EffectiveDate], [EndDate]) VALUES (1, 1, 1, CAST(0x00009F8301264C25 AS DateTime), NULL)
INSERT [dbo].[ProductRequiredDocument] ([Id], [FinancialProductId], [RequiredDocumentTypeId], [EffectiveDate], [EndDate]) VALUES (2, 1, 2, CAST(0x00009F8301264C25 AS DateTime), NULL)
INSERT [dbo].[ProductRequiredDocument] ([Id], [FinancialProductId], [RequiredDocumentTypeId], [EffectiveDate], [EndDate]) VALUES (3, 1, 3, CAST(0x00009F8301264C25 AS DateTime), NULL)
INSERT [dbo].[ProductRequiredDocument] ([Id], [FinancialProductId], [RequiredDocumentTypeId], [EffectiveDate], [EndDate]) VALUES (4, 1, 4, CAST(0x00009F8301264C25 AS DateTime), NULL)
INSERT [dbo].[ProductRequiredDocument] ([Id], [FinancialProductId], [RequiredDocumentTypeId], [EffectiveDate], [EndDate]) VALUES (5, 1, 5, CAST(0x00009F8301264C25 AS DateTime), NULL)
INSERT [dbo].[ProductRequiredDocument] ([Id], [FinancialProductId], [RequiredDocumentTypeId], [EffectiveDate], [EndDate]) VALUES (6, 1, 6, CAST(0x00009F8301264C25 AS DateTime), NULL)
INSERT [dbo].[ProductRequiredDocument] ([Id], [FinancialProductId], [RequiredDocumentTypeId], [EffectiveDate], [EndDate]) VALUES (7, 1, 7, CAST(0x00009F8301264C25 AS DateTime), NULL)
INSERT [dbo].[ProductRequiredDocument] ([Id], [FinancialProductId], [RequiredDocumentTypeId], [EffectiveDate], [EndDate]) VALUES (8, 1, 8, CAST(0x00009F8301264C25 AS DateTime), NULL)
INSERT [dbo].[ProductRequiredDocument] ([Id], [FinancialProductId], [RequiredDocumentTypeId], [EffectiveDate], [EndDate]) VALUES (9, 1, 9, CAST(0x00009F8301264C25 AS DateTime), NULL)
INSERT [dbo].[ProductRequiredDocument] ([Id], [FinancialProductId], [RequiredDocumentTypeId], [EffectiveDate], [EndDate]) VALUES (10, 1, 10, CAST(0x00009F8301264C25 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[ProductRequiredDocument] OFF
/****** Object:  Table [dbo].[ReceiptStatus]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReceiptStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReceiptId] [int] NOT NULL,
	[ReceiptStatusTypeId] [int] NULL,
	[Remarks] [varchar](max) NULL,
	[TransitionDateTime] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_RECEIPTSTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductFeature]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductFeature](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductFeatCatId] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](256) NULL,
 CONSTRAINT [PK_PRODUCTFEATURE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ProductFeature] ON
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (1, 1, N'Secured', N'Secured')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (2, 1, N'Unsecured', N'Unsecured')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (3, 2, N'Maximum Loanable Amount', N'Maximum Loanable Amount')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (4, 2, N'Minimum Loanable Amount', N'Minimum Loanable Amount')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (5, 3, N'Maximum Loan Term', N'Maximum Loan Term')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (6, 3, N'Minimum Loan Term', N'Minimum Loan Term')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (7, 4, N'Straight Line Method', N'Straight Line Method')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (8, 4, N'Diminishing Balance Method', N'Diminishing Balance Method')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (9, 5, N'Add-on Interest', N'Add-on Interest')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (10, 5, N'Discounted Interest', N'Discounted Interest')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (11, 6, N'Monthly Interest Rate', N'Monthly Interest Rate')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (12, 6, N'Annual Interest Rate', N'Annual Interest Rate')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (13, 7, N'Monthly Past Due Interest Rate', N'Monthly Past Due Interest Rate')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (14, 7, N'Annual Past Due  Interest Rate', N'Annual Past Due  Interest Rate')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (15, 8, N'Documentary Stamp Tax', N'Documentary Stamp Tax')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (16, 8, N'Service Charge', N'Service Charge')
INSERT [dbo].[ProductFeature] ([Id], [ProductFeatCatId], [Name], [Description]) VALUES (17, 8, N'Loan Guarantee Fee', N'Loan Guarantee Fee')
SET IDENTITY_INSERT [dbo].[ProductFeature] OFF
/****** Object:  Table [dbo].[ProductCatFeatApplicability]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCatFeatApplicability](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductFeatureId] [int] NOT NULL,
	[ProductCategoryId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_PRODUCTCATFEATAPPLICABILITY] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TimeUnitConversion]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeUnitConversion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SourceUomId] [int] NOT NULL,
	[TargetUomId] [int] NOT NULL,
	[Multiplier] [decimal](10, 4) NOT NULL,
	[Offset] [decimal](10, 4) NULL,
 CONSTRAINT [PK_TIMEUNITCONVERSION] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[TimeUnitConversion] ON
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (1, 2, 1, CAST(7.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (2, 3, 2, CAST(4.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (3, 4, 3, CAST(12.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (4, 3, 1, CAST(30.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (5, 1, 2, CAST(1.0000 AS Decimal(10, 4)), CAST(7.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (6, 2, 3, CAST(1.0000 AS Decimal(10, 4)), CAST(4.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (7, 3, 4, CAST(1.0000 AS Decimal(10, 4)), CAST(12.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (8, 1, 3, CAST(1.0000 AS Decimal(10, 4)), CAST(30.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (9, 1, 7, CAST(1.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (10, 1, 8, CAST(1.0000 AS Decimal(10, 4)), CAST(7.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (11, 1, 9, CAST(1.0000 AS Decimal(10, 4)), CAST(15.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (12, 1, 10, CAST(1.0000 AS Decimal(10, 4)), CAST(30.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (13, 1, 11, CAST(1.0000 AS Decimal(10, 4)), CAST(365.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (14, 7, 1, CAST(1.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (15, 8, 1, CAST(7.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (16, 9, 1, CAST(15.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (17, 10, 1, CAST(30.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (18, 11, 1, CAST(365.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (19, 2, 7, CAST(7.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (20, 2, 8, CAST(1.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (21, 2, 9, CAST(1.0000 AS Decimal(10, 4)), CAST(2.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (22, 2, 10, CAST(1.0000 AS Decimal(10, 4)), CAST(4.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (23, 2, 11, CAST(1.0000 AS Decimal(10, 4)), CAST(52.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (24, 7, 2, CAST(1.0000 AS Decimal(10, 4)), CAST(7.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (25, 8, 2, CAST(1.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (26, 9, 2, CAST(2.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (27, 10, 2, CAST(4.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (28, 11, 2, CAST(52.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (29, 3, 7, CAST(30.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (30, 3, 8, CAST(4.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (31, 3, 9, CAST(2.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (32, 3, 10, CAST(1.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (33, 3, 11, CAST(1.0000 AS Decimal(10, 4)), CAST(12.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (34, 7, 3, CAST(1.0000 AS Decimal(10, 4)), CAST(30.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (35, 8, 3, CAST(1.0000 AS Decimal(10, 4)), CAST(4.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (36, 9, 3, CAST(1.0000 AS Decimal(10, 4)), CAST(2.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (37, 10, 3, CAST(1.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (38, 11, 3, CAST(12.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (39, 4, 7, CAST(365.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (40, 4, 8, CAST(52.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (41, 4, 9, CAST(24.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (42, 4, 10, CAST(12.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (43, 4, 11, CAST(1.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (44, 7, 4, CAST(1.0000 AS Decimal(10, 4)), CAST(365.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (45, 8, 4, CAST(1.0000 AS Decimal(10, 4)), CAST(52.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (46, 9, 4, CAST(1.0000 AS Decimal(10, 4)), CAST(24.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (47, 10, 4, CAST(1.0000 AS Decimal(10, 4)), CAST(12.0000 AS Decimal(10, 4)))
INSERT [dbo].[TimeUnitConversion] ([Id], [SourceUomId], [TargetUomId], [Multiplier], [Offset]) VALUES (48, 11, 4, CAST(1.0000 AS Decimal(10, 4)), CAST(1.0000 AS Decimal(10, 4)))
SET IDENTITY_INSERT [dbo].[TimeUnitConversion] OFF
/****** Object:  Table [dbo].[SystemSetting]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemSetting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SystemSettingTypeId] [int] NOT NULL,
	[UomId] [int] NULL,
	[Value] [varchar](50) NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_SYSTEMSETTING] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SystemSetting] ON
INSERT [dbo].[SystemSetting] ([Id], [SystemSettingTypeId], [UomId], [Value], [EffectiveDate], [EndDate]) VALUES (1, 1, 1, N'0', CAST(0x00009F24013F86BA AS DateTime), CAST(0x00009F240140D927 AS DateTime))
INSERT [dbo].[SystemSetting] ([Id], [SystemSettingTypeId], [UomId], [Value], [EffectiveDate], [EndDate]) VALUES (2, 2, 1, N'2', CAST(0x00009F24013F86C9 AS DateTime), NULL)
INSERT [dbo].[SystemSetting] ([Id], [SystemSettingTypeId], [UomId], [Value], [EffectiveDate], [EndDate]) VALUES (3, 3, 1, N'1', CAST(0x00009F24013F86CE AS DateTime), NULL)
INSERT [dbo].[SystemSetting] ([Id], [SystemSettingTypeId], [UomId], [Value], [EffectiveDate], [EndDate]) VALUES (4, 4, 4, N'18', CAST(0x00009F24013F86D5 AS DateTime), NULL)
INSERT [dbo].[SystemSetting] ([Id], [SystemSettingTypeId], [UomId], [Value], [EffectiveDate], [EndDate]) VALUES (5, 7, NULL, N'', CAST(0x00009F24013F86DA AS DateTime), NULL)
INSERT [dbo].[SystemSetting] ([Id], [SystemSettingTypeId], [UomId], [Value], [EffectiveDate], [EndDate]) VALUES (6, 8, NULL, N'After Maturity Date', CAST(0x00009F24013F86E0 AS DateTime), NULL)
INSERT [dbo].[SystemSetting] ([Id], [SystemSettingTypeId], [UomId], [Value], [EffectiveDate], [EndDate]) VALUES (7, 9, 4, N'', CAST(0x00009F24013F86E6 AS DateTime), NULL)
INSERT [dbo].[SystemSetting] ([Id], [SystemSettingTypeId], [UomId], [Value], [EffectiveDate], [EndDate]) VALUES (8, 6, NULL, N'2', CAST(0x00009F24013F86EA AS DateTime), NULL)
INSERT [dbo].[SystemSetting] ([Id], [SystemSettingTypeId], [UomId], [Value], [EffectiveDate], [EndDate]) VALUES (9, 5, NULL, N'2', CAST(0x00009F24013F86F0 AS DateTime), NULL)
INSERT [dbo].[SystemSetting] ([Id], [SystemSettingTypeId], [UomId], [Value], [EffectiveDate], [EndDate]) VALUES (10, 11, NULL, N'1', CAST(0x00009F24013F86F6 AS DateTime), NULL)
INSERT [dbo].[SystemSetting] ([Id], [SystemSettingTypeId], [UomId], [Value], [EffectiveDate], [EndDate]) VALUES (11, 1, 1, N'5', CAST(0x00009F240140D927 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[SystemSetting] OFF
/****** Object:  Table [dbo].[PettyCashLoanApplication]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PettyCashLoanApplication](
	[ApplicationId] [int] NOT NULL,
	[UomId] [int] NOT NULL,
	[LoanTermLength] [int] NOT NULL,
	[LoanAmount] [numeric](18, 2) NOT NULL,
	[InterestRate] [numeric](4, 2) NULL,
	[InterestRateDescription] [varchar](50) NULL,
	[PastDueInterestRate] [numeric](4, 2) NULL,
	[PastDueInterestRateDescript] [varchar](50) NULL,
	[IsInterestProductFeatureInd] [bit] NOT NULL,
	[IsPastDueProductFeatureInd] [bit] NOT NULL,
 CONSTRAINT [PK_PETTY_CASH_LOAN_APPLICATION2] PRIMARY KEY NONCLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductFeatureApplicability]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductFeatureApplicability](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinancialProductId] [int] NOT NULL,
	[ProductFeatureId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[Value] [numeric](18, 2) NULL,
 CONSTRAINT [PK_PRODUCTFEATUREAPPLICABILITY] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ProductFeatureApplicability] ON
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (1, 1, 6, CAST(0x00009F8301264C25 AS DateTime), NULL, NULL)
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (2, 1, 5, CAST(0x00009F8301264C25 AS DateTime), NULL, NULL)
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (3, 1, 15, CAST(0x00009F8301264C25 AS DateTime), NULL, NULL)
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (4, 1, 16, CAST(0x00009F8301264C25 AS DateTime), NULL, NULL)
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (5, 1, 17, CAST(0x00009F8301264C25 AS DateTime), NULL, NULL)
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (6, 1, 1, CAST(0x00009F8301264C25 AS DateTime), CAST(0x00009FFF00ABBD58 AS DateTime), NULL)
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (7, 1, 2, CAST(0x00009F8301264C25 AS DateTime), NULL, NULL)
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (8, 1, 8, CAST(0x00009F8301264C25 AS DateTime), CAST(0x00009FFF00ABBD58 AS DateTime), NULL)
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (9, 1, 7, CAST(0x00009F8301264C25 AS DateTime), NULL, NULL)
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (10, 1, 9, CAST(0x00009F8301264C25 AS DateTime), NULL, NULL)
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (11, 1, 10, CAST(0x00009F8301264C25 AS DateTime), CAST(0x00009FFF00ABBD58 AS DateTime), NULL)
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (12, 1, 4, CAST(0x00009F8301264C25 AS DateTime), CAST(0x00009FFF00ABBD58 AS DateTime), CAST(1000.00 AS Numeric(18, 2)))
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (13, 1, 3, CAST(0x00009F8301264C25 AS DateTime), NULL, CAST(1000000.00 AS Numeric(18, 2)))
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (14, 1, 11, CAST(0x00009F8301264C25 AS DateTime), NULL, CAST(4.00 AS Numeric(18, 2)))
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (15, 1, 11, CAST(0x00009F8301264C25 AS DateTime), NULL, CAST(3.00 AS Numeric(18, 2)))
INSERT [dbo].[ProductFeatureApplicability] ([Id], [FinancialProductId], [ProductFeatureId], [EffectiveDate], [EndDate], [Value]) VALUES (16, 1, 4, CAST(0x00009FFF00ABBD58 AS DateTime), NULL, CAST(10000.00 AS Numeric(18, 2)))
SET IDENTITY_INSERT [dbo].[ProductFeatureApplicability] OFF
/****** Object:  Table [dbo].[Denomination]    Script Date: 02/22/2012 10:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Denomination](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BillAmount] [decimal](18, 0) NOT NULL,
	[ForExDetailId] [int] NOT NULL,
	[SerialNumber] [varchar](50) NULL,
 CONSTRAINT [PK_Denomination] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[FinancialProductViewList]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[FinancialProductViewList] as

SELECT     dbo.FinancialProduct.Id, dbo.FinancialProduct.Name, dbo.FinancialProduct.IntroductionDate, dbo.FinancialProduct.SalesDiscontinuationDate, 
                      dbo.ProductStatusType.Name AS Status
FROM         dbo.FinancialProduct INNER JOIN
                      dbo.ProductStatus ON dbo.FinancialProduct.Id = dbo.ProductStatus.FinancialProductId INNER JOIN
                      dbo.ProductStatusType ON dbo.ProductStatusType.Id = dbo.ProductStatus.StatusTypeId
WHERE     (dbo.ProductStatus.TransitionDateTime IN
                          (SELECT     MAX(TransitionDateTime) AS Expr1
                            FROM          dbo.ProductStatus AS ProductStatus_1
                            GROUP BY FinancialProductId))
GO
/****** Object:  Table [dbo].[Agreement]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Agreement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AgreementTypeId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[ParentAgreementId] [int] NULL,
	[AgreementDate] [datetime] NOT NULL,
	[Description] [varchar](256) NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[Text] [varchar](max) NULL,
 CONSTRAINT [PK_AGREEMENT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LoanApplication]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LoanApplication](
	[ApplicationId] [int] NOT NULL,
	[LoanTermUomId] [int] NOT NULL,
	[PaymentModeUomId] [int] NOT NULL,
	[LoanTermLength] [int] NOT NULL,
	[LoanAmount] [numeric](18, 2) NOT NULL,
	[Purpose] [varchar](max) NULL,
	[IsReloanIndicator] [bit] NOT NULL,
	[Remarks] [varchar](max) NULL,
	[InterestRate] [numeric](4, 2) NULL,
	[InterestRateDescription] [varchar](50) NULL,
	[PastDueInterestRate] [numeric](4, 2) NULL,
	[PastDueInterestDescription] [varchar](50) NULL,
	[IsInterestProductFeatureInd] [bit] NOT NULL,
	[IsPastDueProductFeatureInd] [bit] NOT NULL,
 CONSTRAINT [PK_LOANAPPLICATION] PRIMARY KEY NONCLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Organization]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Organization](
	[PartyId] [int] NOT NULL,
	[OrganizationTypeId] [int] NULL,
	[OrganizationName] [varchar](50) NOT NULL,
	[DateEstablished] [datetime] NULL,
 CONSTRAINT [PK_ORGANIZATION] PRIMARY KEY NONCLUSTERED 
(
	[PartyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (3, 1, N'M. N. Pamaran Lending Investors, Inc.', CAST(0x0000722300000000 AS DateTime))
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (4, NULL, N'Allied Bank', NULL)
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (5, NULL, N'Banco de Oro', NULL)
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (6, NULL, N'Land Bank', NULL)
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (7, NULL, N'Bank of the Philippine Islands', NULL)
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (8, NULL, N'Philippine Veterans Bank', NULL)
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (9, NULL, N'Metrobank', NULL)
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (10, NULL, N'Philippine National Bank', NULL)
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (11, NULL, N'Philippine Savings Bank', NULL)
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (12, NULL, N'Rizal Commercial Banking Corporation', NULL)
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (13, NULL, N'Union Bank of the Philippines', NULL)
INSERT [dbo].[Organization] ([PartyId], [OrganizationTypeId], [OrganizationName], [DateEstablished]) VALUES (14, NULL, N'United Coconut Planters Bank', NULL)
/****** Object:  Table [dbo].[PartyRole]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartyRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyId] [int] NOT NULL,
	[RoleTypeId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_PARTYROLE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[PartyRole] ON
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (1, 2, 9, CAST(0x00009F2400000000 AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (3, 3, 14, CAST(0x00009F2400000000 AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (4, 4, 13, CAST(0x00009F9800FE8FBE AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (5, 5, 13, CAST(0x00009F9800FEDC2A AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (6, 6, 13, CAST(0x00009F9800FF3261 AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (7, 7, 13, CAST(0x00009F9800FF7935 AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (8, 8, 13, CAST(0x00009F980100001F AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (9, 9, 13, CAST(0x00009F9801003DEE AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (10, 10, 13, CAST(0x00009F980100CE62 AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (11, 11, 13, CAST(0x00009F9801011FA1 AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (12, 12, 13, CAST(0x00009F9801015AFD AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (13, 13, 13, CAST(0x00009F98010200D2 AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (14, 14, 13, CAST(0x00009F9801023E7E AS DateTime), NULL)
INSERT [dbo].[PartyRole] ([Id], [PartyId], [RoleTypeId], [EffectiveDate], [EndDate]) VALUES (15, 15, 7, CAST(0x00009FFF00AC03DE AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[PartyRole] OFF
/****** Object:  Table [dbo].[Person]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[PartyId] [int] NOT NULL,
	[GenderTypeId] [int] NULL,
	[NationalityTypeId] [int] NULL,
	[EducAttainmentTypeId] [int] NULL,
	[Birthdate] [datetime] NULL,
	[ImageFilename] [text] NULL,
 CONSTRAINT [PK_PERSON] PRIMARY KEY NONCLUSTERED 
(
	[PartyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Person] ([PartyId], [GenderTypeId], [NationalityTypeId], [EducAttainmentTypeId], [Birthdate], [ImageFilename]) VALUES (2, 1, 1, 1, CAST(0x0000816300000000 AS DateTime), NULL)
INSERT [dbo].[Person] ([PartyId], [GenderTypeId], [NationalityTypeId], [EducAttainmentTypeId], [Birthdate], [ImageFilename]) VALUES (15, 1, 1, 4, CAST(0x0000865100000000 AS DateTime), N'../../../Uploaded/Images/mjs221.jpg')
/****** Object:  Table [dbo].[Payment]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentMethodTypeId] [int] NOT NULL,
	[ParentPaymentId] [int] NULL,
	[ProcessedByPartyRoleId] [int] NOT NULL,
	[ProcessedToPartyRoleId] [int] NULL,
	[PaymentReferenceNumber] [varchar](50) NULL,
	[TransactionDate] [datetime] NOT NULL,
	[EntryDate] [datetime] NOT NULL,
	[TotalAmount] [numeric](18, 2) NOT NULL,
	[PaymentTypeId] [int] NOT NULL,
	[SpecificPaymentTypeId] [int] NOT NULL,
 CONSTRAINT [PK_PAYMENT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[PastDueInterestRateViewList]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: PastDueInterestRateViewList                            */
/*==============================================================*/
create view [dbo].[PastDueInterestRateViewList] (Description, PastDueRate) as
SELECT productfeature.name AS "Description",
	productfeatureapplicability.value AS "PastDueRate(%)"

FROM productfeature JOIN productfeatureapplicability ON productfeature.id = productfeatureapplicability.productfeatureid;
GO
/****** Object:  Table [dbo].[LoanTerm]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanTerm](
	[ProductFeatApplicabilityId] [int] NOT NULL,
	[UomId] [int] NOT NULL,
	[LoanTermLength] [int] NOT NULL,
 CONSTRAINT [PK_LOANTERM] PRIMARY KEY NONCLUSTERED 
(
	[ProductFeatApplicabilityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[LoanTerm] ([ProductFeatApplicabilityId], [UomId], [LoanTermLength]) VALUES (1, 3, 1)
INSERT [dbo].[LoanTerm] ([ProductFeatApplicabilityId], [UomId], [LoanTermLength]) VALUES (2, 3, 120)
/****** Object:  Table [dbo].[PartyRelationship]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartyRelationship](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstPartyRoleId] [int] NOT NULL,
	[SecondPartyRoleId] [int] NOT NULL,
	[PartyRelTypeId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_PARTYRELATIONSHIP] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[PartyRelationship] ON
INSERT [dbo].[PartyRelationship] ([Id], [FirstPartyRoleId], [SecondPartyRoleId], [PartyRelTypeId], [EffectiveDate], [EndDate]) VALUES (2, 1, 3, 1, CAST(0x00009F2400000000 AS DateTime), NULL)
INSERT [dbo].[PartyRelationship] ([Id], [FirstPartyRoleId], [SecondPartyRoleId], [PartyRelTypeId], [EffectiveDate], [EndDate]) VALUES (3, 15, 3, 2, CAST(0x00009FFF00AC03DE AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[PartyRelationship] OFF
/****** Object:  Table [dbo].[LoanModification]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanModification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[LoanModificationTypeId] [int] NOT NULL,
 CONSTRAINT [PK_LOANMODIFICATION] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanAgreement]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanAgreement](
	[AgreementId] [int] NOT NULL,
 CONSTRAINT [PK_LOANAGREEMENT] PRIMARY KEY NONCLUSTERED 
(
	[AgreementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaritalStatus]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaritalStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyId] [int] NOT NULL,
	[MaritalStatusTypeId] [int] NOT NULL,
	[NumberOfDependents] [int] NOT NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_MARITALSTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[MaritalStatus] ON
INSERT [dbo].[MaritalStatus] ([Id], [PartyId], [MaritalStatusTypeId], [NumberOfDependents], [TransitionDateTime], [IsActive]) VALUES (1, 15, 1, 0, CAST(0x00009FFF00AC03DE AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[MaritalStatus] OFF
/****** Object:  Table [dbo].[LoanApplicationStatus]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LoanApplicationStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusTypeId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[Remarks] [varchar](max) NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_LOANAPPLICATIONSTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LoanApplicationRole]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanApplicationRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
 CONSTRAINT [PK_LOANAPPLICATIONROLE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanApplicationFee]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LoanApplicationFee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[Particular] [varchar](50) NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_LOANAPPLICATIONFEE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LoanDisbursementVcr]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanDisbursementVcr](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AgreementId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[Balance] [numeric](18, 1) NULL,
 CONSTRAINT [PK_LOANDISBURSEMENTVCR] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HomeOwnership]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HomeOwnership](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HomeOwnershipTypeId] [int] NOT NULL,
	[PartyId] [int] NOT NULL,
	[ResidenceSince] [datetime] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_HOMEOWNERSHIP] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[HomeOwnership] ON
INSERT [dbo].[HomeOwnership] ([Id], [HomeOwnershipTypeId], [PartyId], [ResidenceSince], [EffectiveDate], [EndDate]) VALUES (1, 1, 15, CAST(0x00009FFF00000000 AS DateTime), CAST(0x00009FFF00AC03DE AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[HomeOwnership] OFF
/****** Object:  Table [dbo].[ForExCheque]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ForExCheque](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ForExDetailId] [int] NOT NULL,
	[BankPartyRoleId] [int] NOT NULL,
	[CheckDate] [datetime] NOT NULL,
	[CheckNumber] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ForExCheque] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ApplicationItem]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[ProdFeatApplicabilityId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[FeeComputedValue] [numeric](18, 2) NULL,
 CONSTRAINT [PK_APPLICATIONITEM] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Asset]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Asset](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AssetTypeId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[AcquisitionCost] [decimal](18, 2) NULL,
	[IsMortgaged] [bit] NOT NULL,
 CONSTRAINT [PK_ASSET] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Bank]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Bank](
	[PartyRoleId] [int] NOT NULL,
	[Branch] [varchar](50) NULL,
	[Acronym] [varchar](50) NULL,
 CONSTRAINT [PK_BANK] PRIMARY KEY NONCLUSTERED 
(
	[PartyRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Bank] ([PartyRoleId], [Branch], [Acronym]) VALUES (4, N'', N'Allied Bank')
INSERT [dbo].[Bank] ([PartyRoleId], [Branch], [Acronym]) VALUES (5, N'', N'BDO')
INSERT [dbo].[Bank] ([PartyRoleId], [Branch], [Acronym]) VALUES (6, N'', N'Land Bank')
INSERT [dbo].[Bank] ([PartyRoleId], [Branch], [Acronym]) VALUES (7, N'', N'BPI')
INSERT [dbo].[Bank] ([PartyRoleId], [Branch], [Acronym]) VALUES (8, N'', N'PVB')
INSERT [dbo].[Bank] ([PartyRoleId], [Branch], [Acronym]) VALUES (9, N'', N'Metrobank')
INSERT [dbo].[Bank] ([PartyRoleId], [Branch], [Acronym]) VALUES (10, N'', N'PNB')
INSERT [dbo].[Bank] ([PartyRoleId], [Branch], [Acronym]) VALUES (11, N'', N'PSBank')
INSERT [dbo].[Bank] ([PartyRoleId], [Branch], [Acronym]) VALUES (12, N'', N'RCBC')
INSERT [dbo].[Bank] ([PartyRoleId], [Branch], [Acronym]) VALUES (13, N'', N'UBP')
INSERT [dbo].[Bank] ([PartyRoleId], [Branch], [Acronym]) VALUES (14, N'', N'UCPB')
/****** Object:  Table [dbo].[AgreementRole]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgreementRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AgreementId] [int] NOT NULL,
	[PartyRoleId] [int] NOT NULL,
 CONSTRAINT [PK_AGREEMENTROLE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AgreementItem]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AgreementItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AgreementId] [int] NOT NULL,
	[LoanAmount] [numeric](18, 2) NOT NULL,
	[LoanTermLength] [int] NOT NULL,
	[LoanTermUom] [varchar](50) NOT NULL,
	[InterestComputationMode] [varchar](50) NOT NULL,
	[InterestRate] [numeric](4, 2) NOT NULL,
	[InterestRateDescription] [varchar](50) NOT NULL,
	[PastDueInterestRate] [numeric](4, 2) NULL,
	[PastDueInterestRateDescript] [varchar](50) NULL,
	[PaymentMode] [varchar](50) NULL,
	[MethodOfChargingInterest] [varchar](50) NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_AGREEMENTITEM] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CashOnVault]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CashOnVault](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[Amount] [decimal](18, 4) NOT NULL,
	[ClosedByPartyRoleId] [int] NOT NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CashOnVault] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompromiseAgreement]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompromiseAgreement](
	[AgreementId] [int] NOT NULL,
 CONSTRAINT [PK_COMPROMISEAGREEMENT] PRIMARY KEY NONCLUSTERED 
(
	[AgreementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[PartyRoleId] [int] NOT NULL,
	[CreditLimit] [numeric](18, 2) NULL,
	[Remarks] [varchar](max) NULL,
 CONSTRAINT [PK_CUSTOMER] PRIMARY KEY NONCLUSTERED 
(
	[PartyRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Customer] ([PartyRoleId], [CreditLimit], [Remarks]) VALUES (15, NULL, N'')
/****** Object:  Table [dbo].[COVTransaction]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COVTransaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProcessedByPartyRoleId] [int] NOT NULL,
	[Amount] [decimal](18, 4) NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[COVTransTypeId] [int] NOT NULL,
	[Remarks] [varchar](max) NULL,
	[TransactionDate] [datetime] NOT NULL,
	[EntryDate] [datetime] NOT NULL,
 CONSTRAINT [PK_COVDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ForeignExchange]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForeignExchange](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProcessedByPartyRoleId] [int] NOT NULL,
	[ProcessedToPartyRoleId] [int] NOT NULL,
	[Rate] [decimal](18, 4) NOT NULL,
	[OriginalAmount] [decimal](18, 4) NOT NULL,
	[OriginalCurrencyId] [int] NOT NULL,
	[ConvertedAmount] [decimal](18, 4) NOT NULL,
	[ConvertedCurrencyId] [int] NOT NULL,
	[EntryDate] [datetime] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ForeignExchange] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fee]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fee](
	[ProductFeatApplicabilityId] [int] NOT NULL,
	[ChargeAmount] [numeric](18, 2) NULL,
	[BaseAmount] [numeric](18, 2) NULL,
	[PercentageRate] [numeric](4, 2) NULL,
 CONSTRAINT [PK_FEE] PRIMARY KEY NONCLUSTERED 
(
	[ProductFeatApplicabilityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Fee] ([ProductFeatApplicabilityId], [ChargeAmount], [BaseAmount], [PercentageRate]) VALUES (3, CAST(10.00 AS Numeric(18, 2)), CAST(5000.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(4, 2)))
INSERT [dbo].[Fee] ([ProductFeatApplicabilityId], [ChargeAmount], [BaseAmount], [PercentageRate]) VALUES (4, CAST(100.00 AS Numeric(18, 2)), CAST(10000.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(4, 2)))
INSERT [dbo].[Fee] ([ProductFeatApplicabilityId], [ChargeAmount], [BaseAmount], [PercentageRate]) VALUES (5, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(1.00 AS Numeric(4, 2)))
/****** Object:  Table [dbo].[FinancialAccount]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FinancialAccount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AgreementId] [int] NOT NULL,
	[ParentFinancialAccountId] [int] NULL,
	[FinancialAccountTypeId] [int] NOT NULL,
	[Description] [varchar](256) NULL,
 CONSTRAINT [PK_FINANCIALACCOUNT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Employee](
	[PartyRoleId] [int] NOT NULL,
	[EmployeeIdNumber] [varchar](30) NULL,
	[Position] [varchar](50) NOT NULL,
	[SssNumber] [varchar](30) NULL,
	[GsisNumber] [varchar](30) NULL,
	[OwaNumber] [varchar](30) NULL,
	[PhicNumber] [varchar](30) NULL,
 CONSTRAINT [PK_EMPLOYEE] PRIMARY KEY NONCLUSTERED 
(
	[PartyRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Employee] ([PartyRoleId], [EmployeeIdNumber], [Position], [SssNumber], [GsisNumber], [OwaNumber], [PhicNumber]) VALUES (1, N'8', N'Clerk', NULL, NULL, NULL, NULL)
/****** Object:  Table [dbo].[PettyCashLoanItem]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PettyCashLoanItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[PaymentDueDate] [datetime] NOT NULL,
	[SourceOfPayment] [varchar](256) NOT NULL,
	[Memo] [varchar](256) NOT NULL,
	[Amount] [numeric](18, 2) NULL,
 CONSTRAINT [PK_PETTYCASHLOANITEM] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PersonName]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PersonName](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyId] [int] NOT NULL,
	[PersonNameTypeId] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_PERSONNAME] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[PersonName] ON
INSERT [dbo].[PersonName] ([Id], [PartyId], [PersonNameTypeId], [Name], [EffectiveDate], [EndDate]) VALUES (1, 2, 1, N'Mr.', CAST(0x00009F2400000000 AS DateTime), NULL)
INSERT [dbo].[PersonName] ([Id], [PartyId], [PersonNameTypeId], [Name], [EffectiveDate], [EndDate]) VALUES (2, 2, 2, N'Rey', CAST(0x00009F2400000000 AS DateTime), NULL)
INSERT [dbo].[PersonName] ([Id], [PartyId], [PersonNameTypeId], [Name], [EffectiveDate], [EndDate]) VALUES (3, 2, 4, N'Pamaran', CAST(0x00009F2400000000 AS DateTime), NULL)
INSERT [dbo].[PersonName] ([Id], [PartyId], [PersonNameTypeId], [Name], [EffectiveDate], [EndDate]) VALUES (4, 15, 2, N'Il Woo', CAST(0x00009FFF00AC03DE AS DateTime), NULL)
INSERT [dbo].[PersonName] ([Id], [PartyId], [PersonNameTypeId], [Name], [EffectiveDate], [EndDate]) VALUES (5, 15, 4, N'Jung', CAST(0x00009FFF00AC03DE AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[PersonName] OFF
/****** Object:  Table [dbo].[PersonIdentification]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PersonIdentification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdentificationTypeId] [int] NOT NULL,
	[PartyId] [int] NOT NULL,
	[IdNumber] [varchar](30) NOT NULL,
 CONSTRAINT [PK_PERSONIDENTIFICATION] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PettyCashLoanApplicationStatus]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PettyCashLoanApplicationStatus](
	[Id] [int] NOT NULL,
	[StatusTypeId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[Remarks] [varchar](max) NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_PETTYCASHLOANAPPLICATIONSTA] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Taxpayer]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Taxpayer](
	[PartyRoleId] [int] NOT NULL,
	[Tin] [varchar](50) NULL,
 CONSTRAINT [PK_TAXPAYER] PRIMARY KEY NONCLUSTERED 
(
	[PartyRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductCategoryFeatureFunctionalApplicability]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategoryFeatureFunctionalApplicability](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductCategoryId] [int] NULL,
	[ProductCatFeatApplicabilityId] [int] NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_PRODUCTCATEGORYFEATUREFUNCT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAccount]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAccount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserAccountTypeId] [int] NOT NULL,
	[PartyId] [int] NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[Username] [varchar](20) NOT NULL,
	[Password] [varchar](1000) NOT NULL,
	[SecurityQuestion] [varchar](max) NOT NULL,
	[SecurityAnswer] [varchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[HomePage] [varchar](500) NULL,
 CONSTRAINT [PK_USERACCOUNT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[UserAccount] ON
INSERT [dbo].[UserAccount] ([Id], [UserAccountTypeId], [PartyId], [EffectiveDate], [EndDate], [Username], [Password], [SecurityQuestion], [SecurityAnswer], [DateCreated], [HomePage]) VALUES (3, 2, 2, CAST(0x00009F2400000000 AS DateTime), NULL, N'admin', N'0DPiKuNIrrVmD8IUCuw1hQxNqZc=', N'Where did you meet your spouse?', N'admin', CAST(0x00009F2400000000 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[UserAccount] OFF
/****** Object:  Table [dbo].[SubmittedDocument]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SubmittedDocument](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductRequiredDocumentId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[DateSubmitted] [datetime] NOT NULL,
	[Description] [varchar](256) NULL,
 CONSTRAINT [PK_SUBMITTEDDOCUMENT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SpecimenSignature]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpecimenSignature](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyId] [int] NOT NULL,
	[DateUploaded] [datetime] NOT NULL,
	[ImageFilename] [text] NOT NULL,
 CONSTRAINT [PK_SPECIMENSIGNATURE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehicle]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Vehicle](
	[AssetId] [int] NOT NULL,
	[VehicleTypeId] [int] NOT NULL,
	[Brand] [varchar](50) NOT NULL,
	[Model] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReceiptPaymentAssoc]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceiptPaymentAssoc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReceiptId] [int] NOT NULL,
	[PaymentId] [int] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_ReceiptPaymentAssoc] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubmittedDocumentStatus]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SubmittedDocumentStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusTypeId] [int] NOT NULL,
	[SubmittedDocumentId] [int] NOT NULL,
	[Remarks] [varchar](max) NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SUBMITTEDDOCUMENTSTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserAccountStatus]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccountStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserAccountId] [int] NOT NULL,
	[StatusTypeId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_USERACCOUNTSTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[UserAccountStatus] ON
INSERT [dbo].[UserAccountStatus] ([Id], [UserAccountId], [StatusTypeId], [EffectiveDate], [EndDate]) VALUES (5, 3, 1, CAST(0x00009F2400000000 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[UserAccountStatus] OFF
/****** Object:  Table [dbo].[PaymentCurrencyAssoc]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentCurrencyAssoc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
 CONSTRAINT [PK_PaymentCurrencyAssoc] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinAcctTrans]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinAcctTrans](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinancialAcctTransTypeId] [int] NOT NULL,
	[FinancialAccountId] [int] NOT NULL,
	[PaymentId] [int] NULL,
	[TransactionDate] [datetime] NOT NULL,
	[EntryDate] [datetime] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_FINACCTTRANS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeePayment]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FeePayment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentId] [int] NOT NULL,
	[Particular] [varchar](50) NOT NULL,
	[FeeAmount] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_FeePayment_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Employment]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Employment](
	[PartyRelationshipId] [int] NOT NULL,
	[EmploymentStatus] [varchar](50) NOT NULL,
	[Salary] [varchar](50) NOT NULL,
 CONSTRAINT [PK_EMPLOYMENT] PRIMARY KEY NONCLUSTERED 
(
	[PartyRelationshipId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Employment] ([PartyRelationshipId], [EmploymentStatus], [Salary]) VALUES (2, N'Employed', N'50000')
/****** Object:  Table [dbo].[DocumentPage]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentPage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubmittedDocumentId] [int] NOT NULL,
	[ImageFilename] [text] NOT NULL,
	[ImageFilePath] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_DOCUMENTPAGE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialAccountRole]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialAccountRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[FinancialAccountId] [int] NOT NULL,
 CONSTRAINT [PK_FINANCIALACCOUNTROLE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialAccountProduct]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialAccountProduct](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinancialProductId] [int] NOT NULL,
	[FinancialAccountId] [int] NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_FINANCIALACCOUNTPRODUCT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Disbursement]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Disbursement](
	[PaymentId] [int] NOT NULL,
	[DisbursementTypeId] [int] NOT NULL,
	[DisbursedToName] [varchar](256) NULL,
 CONSTRAINT [PK_DISBURSEMENT] PRIMARY KEY NONCLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DisbursementVcrStatus]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DisbursementVcrStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DisbursementVoucherStatTypId] [int] NOT NULL,
	[LoanDisbursementVoucherId] [int] NOT NULL,
	[Remarks] [varchar](max) NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_DISBURSEMENTVCRSTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerStatus]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerStatusTypeId] [int] NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CUSTOMERSTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CustomerStatus] ON
INSERT [dbo].[CustomerStatus] ([Id], [CustomerStatusTypeId], [PartyRoleId], [TransitionDateTime], [IsActive]) VALUES (1, 1, 15, CAST(0x00009FFF00AC03DE AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[CustomerStatus] OFF
/****** Object:  Table [dbo].[CustomerSourceOfIncome]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerSourceOfIncome](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[SourceOfIncomeId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[Income] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_CUSTOMERSOURCEOFINCOME] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerClassification]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerClassification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[ClassificationTypeId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_CUSTOMERCLASSIFICATION] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CustomerClassification] ON
INSERT [dbo].[CustomerClassification] ([Id], [PartyRoleId], [ClassificationTypeId], [EffectiveDate], [EndDate]) VALUES (1, 15, 8, CAST(0x00009FFF00AC03DE AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[CustomerClassification] OFF
/****** Object:  Table [dbo].[CustomerCategory]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerCategoryType] [int] NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_CUSTOMERCATEGORY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CustomerCategory] ON
INSERT [dbo].[CustomerCategory] ([Id], [CustomerCategoryType], [PartyRoleId], [EffectiveDate], [EndDate]) VALUES (1, 1, 15, CAST(0x00009FFF00AC03DE AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[CustomerCategory] OFF
/****** Object:  Table [dbo].[Ctc]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ctc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[CtcNumber] [varchar](50) NOT NULL,
	[DateIssued] [datetime] NOT NULL,
	[IssuedWhere] [varchar](256) NOT NULL,
 CONSTRAINT [PK_CTC] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ControlNumbers]    Script Date: 02/22/2012 10:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ControlNumbers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FormTypeId] [int] NOT NULL,
	[LastControlNumber] [int] NOT NULL,
	[PaymentId] [int] NULL,
	[ApplicationId] [int] NULL,
 CONSTRAINT [PK_CONTROLNUMBERS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[concatname]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--function


CREATE function [dbo].[concatname](@p_id numeric,@p_name1 varchar(8000), @p_name2 varchar(8000), @p_name3 varchar(8000), 

@p_name4 varchar(8000))
returns varchar(8000)
as 
begin
	declare @out_name varchar(8000)
	select @out_name = case personnametype.Name when 'First Name' then isnull(cast(@out_name as nvarchar(max)) + ', ','') 

 else  isnull(cast(@out_name as nvarchar(max)) + ' ', '') end + case when PersonNameType.Name = 'Middle Name' then 

cast(SUBSTRING(PersonName.Name,1,1) as nvarchar(max)) + '.' else PersonName.Name end
	
	from PersonName,PersonNameType  
	where PersonName.PersonNameTypeId = PersonNameType.Id 
	and PersonName.EndDate is null and
	PersonNameType.Name in (@p_name1, @p_name2, @p_name3, @p_name4) and
	PartyId = @p_id order by case PersonNameType.Name When 'Last Name' then 1 when 'First Name' then 2 when 'Middle Name' 

then 3 else 4 end 
	return @out_name
end;
GO
/****** Object:  Table [dbo].[BankStatus]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[BankStatusTypeId] [int] NOT NULL,
	[EffectiveDate] [date] NULL,
	[EndDate] [date] NULL,
 CONSTRAINT [PK_BANKSTATUS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BankStatus] ON
INSERT [dbo].[BankStatus] ([Id], [PartyRoleId], [BankStatusTypeId], [EffectiveDate], [EndDate]) VALUES (1, 4, 1, CAST(0xF3340B00 AS Date), NULL)
INSERT [dbo].[BankStatus] ([Id], [PartyRoleId], [BankStatusTypeId], [EffectiveDate], [EndDate]) VALUES (2, 5, 1, CAST(0xF3340B00 AS Date), NULL)
INSERT [dbo].[BankStatus] ([Id], [PartyRoleId], [BankStatusTypeId], [EffectiveDate], [EndDate]) VALUES (3, 6, 1, CAST(0xF3340B00 AS Date), NULL)
INSERT [dbo].[BankStatus] ([Id], [PartyRoleId], [BankStatusTypeId], [EffectiveDate], [EndDate]) VALUES (4, 7, 1, CAST(0xF3340B00 AS Date), NULL)
INSERT [dbo].[BankStatus] ([Id], [PartyRoleId], [BankStatusTypeId], [EffectiveDate], [EndDate]) VALUES (5, 8, 1, CAST(0xF3340B00 AS Date), NULL)
INSERT [dbo].[BankStatus] ([Id], [PartyRoleId], [BankStatusTypeId], [EffectiveDate], [EndDate]) VALUES (6, 9, 1, CAST(0xF3340B00 AS Date), NULL)
INSERT [dbo].[BankStatus] ([Id], [PartyRoleId], [BankStatusTypeId], [EffectiveDate], [EndDate]) VALUES (7, 10, 1, CAST(0xF3340B00 AS Date), NULL)
INSERT [dbo].[BankStatus] ([Id], [PartyRoleId], [BankStatusTypeId], [EffectiveDate], [EndDate]) VALUES (8, 11, 1, CAST(0xF3340B00 AS Date), NULL)
INSERT [dbo].[BankStatus] ([Id], [PartyRoleId], [BankStatusTypeId], [EffectiveDate], [EndDate]) VALUES (9, 12, 1, CAST(0xF3340B00 AS Date), NULL)
INSERT [dbo].[BankStatus] ([Id], [PartyRoleId], [BankStatusTypeId], [EffectiveDate], [EndDate]) VALUES (10, 13, 1, CAST(0xF3340B00 AS Date), NULL)
INSERT [dbo].[BankStatus] ([Id], [PartyRoleId], [BankStatusTypeId], [EffectiveDate], [EndDate]) VALUES (11, 14, 1, CAST(0xF3340B00 AS Date), NULL)
SET IDENTITY_INSERT [dbo].[BankStatus] OFF
/****** Object:  Table [dbo].[BankAccount]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BankAccount](
	[AssetId] [int] NOT NULL,
	[BankAccountTypeId] [int] NOT NULL,
	[BankPartyRoleId] [int] NULL,
	[ApplicationId] [int] NULL,
	[AccountNumber] [varchar](25) NOT NULL,
	[AccountName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_BANKACCOUNT] PRIMARY KEY NONCLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Cheque]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cheque](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentId] [int] NOT NULL,
	[BankPartyRoleId] [int] NOT NULL,
	[CheckDate] [datetime] NOT NULL,
 CONSTRAINT [PK_CHEQUE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[PartyId] [int] NOT NULL,
	[AddressTypeId] [int] NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[AssetId] [int] NULL,
 CONSTRAINT [PK_ADDRESS] PRIMARY KEY NONCLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Address] ON
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (1, 2, 1, CAST(0x00009F1400000000 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (2, 3, 1, CAST(0x0000FD3F00000000 AS DateTime), CAST(0x00009F240140B556 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (3, 3, 2, CAST(0x00009F240135DA75 AS DateTime), CAST(0x00009F240140B556 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (4, 3, 2, CAST(0x00009F240135DA75 AS DateTime), CAST(0x00009F240140B556 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (5, 3, 2, CAST(0x00009F240135DA75 AS DateTime), CAST(0x00009F29010D9576 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (6, 3, 3, CAST(0x00009F240135DA75 AS DateTime), CAST(0x00009F240140B556 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (7, 3, 1, CAST(0x00009F240140B556 AS DateTime), CAST(0x00009F29010D9576 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (8, 3, 2, CAST(0x00009F240140B556 AS DateTime), CAST(0x00009F29010D9576 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (9, 3, 2, CAST(0x00009F240140B556 AS DateTime), CAST(0x00009F29010D9576 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (10, 3, 3, CAST(0x00009F240140B556 AS DateTime), CAST(0x00009F29010EE522 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (11, 3, 1, CAST(0x00009F29010D9576 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (12, 3, 2, CAST(0x00009F29010D9576 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (13, 3, 2, CAST(0x00009F29010D9576 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (14, 3, 3, CAST(0x00009F29010EE522 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (15, 4, 1, CAST(0x00009F9800FE8FBE AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (16, 4, 2, CAST(0x00009F9800FE8FBE AS DateTime), CAST(0x00009F9C00E7327B AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (17, 4, 2, CAST(0x00009F9800FE8FBE AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (18, 5, 1, CAST(0x00009F9800FEDC2A AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (19, 5, 2, CAST(0x00009F9800FEDC2A AS DateTime), CAST(0x00009F9C00E74461 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (20, 5, 2, CAST(0x00009F9800FEDC2A AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (21, 6, 1, CAST(0x00009F9800FF3261 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (22, 6, 2, CAST(0x00009F9800FF3261 AS DateTime), CAST(0x00009F9C00E76C48 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (23, 6, 2, CAST(0x00009F9800FF3261 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (24, 7, 1, CAST(0x00009F9800FF7935 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (25, 7, 2, CAST(0x00009F9800FF7935 AS DateTime), CAST(0x00009F9C00E76548 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (26, 7, 2, CAST(0x00009F9800FF7935 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (27, 8, 1, CAST(0x00009F980100001F AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (28, 8, 2, CAST(0x00009F980100001F AS DateTime), CAST(0x00009F9C00E8B833 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (29, 8, 2, CAST(0x00009F980100001F AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (30, 9, 1, CAST(0x00009F9801003DEE AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (31, 9, 2, CAST(0x00009F9801003DEE AS DateTime), CAST(0x00009F9C00E772AC AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (32, 9, 2, CAST(0x00009F9801003DEE AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (33, 10, 1, CAST(0x00009F980100CE62 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (34, 10, 2, CAST(0x00009F980100CE62 AS DateTime), CAST(0x00009F9C00E77F3F AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (35, 10, 2, CAST(0x00009F980100CE62 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (36, 11, 1, CAST(0x00009F9801011FA1 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (37, 11, 2, CAST(0x00009F9801011FA1 AS DateTime), CAST(0x00009F9C00E78F5D AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (38, 11, 2, CAST(0x00009F9801011FA1 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (39, 12, 1, CAST(0x00009F9801015AFD AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (40, 12, 2, CAST(0x00009F9801015AFD AS DateTime), CAST(0x00009F9C00E8C35A AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (41, 12, 2, CAST(0x00009F9801015AFD AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (42, 13, 1, CAST(0x00009F98010200D2 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (43, 13, 2, CAST(0x00009F98010200D2 AS DateTime), CAST(0x00009F9C00E8D022 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (44, 13, 2, CAST(0x00009F98010200D2 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (45, 14, 1, CAST(0x00009F9801023E7E AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (46, 14, 2, CAST(0x00009F9801023E7E AS DateTime), CAST(0x00009F9C00E8DCF4 AS DateTime), NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (47, 14, 2, CAST(0x00009F9801023E7E AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (48, 4, 2, CAST(0x00009F9C00E7327B AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (49, 4, 2, CAST(0x00009F9C00E7327B AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (50, 5, 2, CAST(0x00009F9C00E74461 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (51, 5, 2, CAST(0x00009F9C00E74461 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (52, 7, 2, CAST(0x00009F9C00E76548 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (53, 7, 2, CAST(0x00009F9C00E76548 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (54, 6, 2, CAST(0x00009F9C00E76C48 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (55, 6, 2, CAST(0x00009F9C00E76C48 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (56, 9, 2, CAST(0x00009F9C00E772AC AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (57, 9, 2, CAST(0x00009F9C00E772AC AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (58, 10, 2, CAST(0x00009F9C00E77F3F AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (59, 10, 2, CAST(0x00009F9C00E77F3F AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (60, 11, 2, CAST(0x00009F9C00E78F5D AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (61, 11, 2, CAST(0x00009F9C00E78F5D AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (62, 8, 2, CAST(0x00009F9C00E8B833 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (63, 8, 2, CAST(0x00009F9C00E8B833 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (64, 12, 2, CAST(0x00009F9C00E8C35A AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (65, 12, 2, CAST(0x00009F9C00E8C35A AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (66, 13, 2, CAST(0x00009F9C00E8D022 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (67, 13, 2, CAST(0x00009F9C00E8D022 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (68, 14, 2, CAST(0x00009F9C00E8DCF4 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (69, 14, 2, CAST(0x00009F9C00E8DCF4 AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (70, 15, 1, CAST(0x00009FFF00AC03DE AS DateTime), NULL, NULL)
INSERT [dbo].[Address] ([AddressId], [PartyId], [AddressTypeId], [EffectiveDate], [EndDate], [AssetId]) VALUES (71, 15, 3, CAST(0x00009FFF00AC03DE AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Address] OFF
/****** Object:  Table [dbo].[Addendum]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addendum](
	[AddendumId] [int] IDENTITY(1,1) NOT NULL,
	[AgreementId] [int] NULL,
	[AgreementItemId] [int] NULL,
 CONSTRAINT [PK_ADDENDUM] PRIMARY KEY NONCLUSTERED 
(
	[AddendumId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetRole]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AssetId] [int] NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[EquityValue] [numeric](18, 2) NULL,
 CONSTRAINT [PK_ASSETROLE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetAppraisal]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetAppraisal](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AssetId] [int] NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[DateAppraised] [datetime] NOT NULL,
	[MarketValue] [numeric](18, 2) NOT NULL,
	[AppraisedValue] [numeric](18, 2) NOT NULL,
	[Remarks] [varchar](max) NULL,
 CONSTRAINT [PK_ASSETAPPRAISAL] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AmortizationSchedule]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AmortizationSchedule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AgreementId] [int] NOT NULL,
	[ParentAmortizationScheduleId] [int] NULL,
	[DateGenerated] [datetime] NOT NULL,
	[LoanReleaseDate] [datetime] NOT NULL,
	[PaymentStartDate] [datetime] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_AMORTIZATIONSCHEDULE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanAccount]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanAccount](
	[FinancialAccountId] [int] NOT NULL,
	[LoanAmount] [numeric](18, 2) NOT NULL,
	[LoanBalance] [numeric](18, 2) NOT NULL,
	[CompoundedInterestAmount] [numeric](18, 2) NULL,
	[LoanReleaseDate] [datetime] NULL,
	[MaturityDate] [datetime] NULL,
	[InterestTypeId] [int] NULL,
 CONSTRAINT [PK_LOANACCOUNT] PRIMARY KEY NONCLUSTERED 
(
	[FinancialAccountId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Land]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Land](
	[AssetId] [int] NOT NULL,
	[UomId] [int] NOT NULL,
	[LandTypeId] [int] NOT NULL,
	[OctTctNumber] [varchar](25) NOT NULL,
	[LandArea] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_LAND] PRIMARY KEY NONCLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FormDetails]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FormDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FormTypeId] [int] NOT NULL,
	[LoanAppId] [int] NULL,
	[PaymentId] [int] NULL,
	[RoleString] [varchar](1000) NOT NULL,
	[PersonString] [varchar](1000) NULL,
	[Signature] [text] NULL,
 CONSTRAINT [PK_FormDetails_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ForeignExchangeDetailAssoc]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForeignExchangeDetailAssoc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ForExId] [int] NOT NULL,
	[ForExDetailId] [int] NOT NULL,
 CONSTRAINT [PK_ForeignExchangeDetailAssoc] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanModificationStatus]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LoanModificationStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoanModificationDatetime] [datetime] NOT NULL,
	[LoanModificationId] [int] NOT NULL,
	[LoanModificationStatusTypeId] [int] NOT NULL,
	[ModifiedBy] [int] NULL,
	[Remarks] [varchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_LOANMODIFICATIONSTATUS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LoanModificationPrevItems]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanModificationPrevItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoanModificationId] [int] NOT NULL,
	[OldFinancialAccountId] [int] NOT NULL,
 CONSTRAINT [PK_LOANMODIFICATIONPREVITEMS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Machine]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Machine](
	[AssetId] [int] NOT NULL,
	[MachineName] [varchar](50) NOT NULL,
	[Brand] [varchar](50) NOT NULL,
	[Model] [varchar](50) NOT NULL,
	[Capacity] [varchar](50) NULL,
 CONSTRAINT [PK_Machine] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LoanPayment]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanPayment](
	[PaymentId] [int] NOT NULL,
	[OwnerOutstandingLoan] [numeric](18, 2) NOT NULL,
	[OwnerOutstandingInterest] [numeric](18, 2) NOT NULL,
	[CoOwnerOutstandingLoan] [numeric](18, 2) NOT NULL,
	[CoOwnerOutstandingInterest] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_LOANPAYMENT] PRIMARY KEY NONCLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostalAddress]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PostalAddress](
	[AddressId] [int] NOT NULL,
	[CountryId] [int] NULL,
	[PostalAddressTypeId] [int] NULL,
	[StreetAddress] [varchar](max) NULL,
	[Barangay] [varchar](max) NULL,
	[Municipality] [varchar](max) NULL,
	[City] [varchar](max) NULL,
	[Province] [varchar](max) NULL,
	[State] [varchar](max) NULL,
	[PostalCode] [varchar](max) NULL,
	[IsPrimary] [bit] NOT NULL,
 CONSTRAINT [PK_POSTALADDRESS] PRIMARY KEY NONCLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (1, 1, 1, NULL, N'Barangay', N'Muncipality', NULL, N'Province', NULL, N'6000', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (2, 1, 2, NULL, N'Barangay', N'Muncipality', NULL, N'Province', NULL, N'6400', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (7, 1, 2, N'1st Street', N'San Pedro', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (11, 1, 2, N'085 B. Sabellano St.', N'San Pedro', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (15, 1, 2, N'Corner F.S. Pajares And Cabrera Streets', N'Santiago', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (18, 1, 2, N'F.S. Pajares Ave.', N'San Francisco', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (21, 1, 2, N'Cabatu Bldg. Jamisola Street', N'Santa Lucia', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (24, 1, 2, N'F.S. Pajares Ave.', N'San Francisco', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (27, 1, 2, N'JAVAVED Business Corp. Bldg., Rizal Avenue', N'Balangasan', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (30, 1, 2, N'Corner Rizal Avenue & J.S. Alano Streets', N'San Francisco', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (33, 1, 2, N'Purok Riverside', N'Balangasan', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (36, 1, 2, N'Mendoza Building, 222 J.P. Rizal Avenue', N'Gatas', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (39, 1, 2, N'RCBC Building, Rizal Avenue', N'San Francisco', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (42, 1, 2, N'Rizal Avenue', N'Balangasan', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (45, 1, 2, N'Jamisola Corner Bonifacio Streets', N'Santiago', NULL, N'Pagadian City', N'Zamboanga del Sur', NULL, N'7016', 1)
INSERT [dbo].[PostalAddress] ([AddressId], [CountryId], [PostalAddressTypeId], [StreetAddress], [Barangay], [Municipality], [City], [Province], [State], [PostalCode], [IsPrimary]) VALUES (70, 1, 1, NULL, N'Buenavista', NULL, N'Pagadian', NULL, NULL, N'7016', 1)
/****** Object:  View [dbo].[OtherDisbursementDetails]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[OtherDisbursementDetails]
AS
SELECT     dbo.Payment.TransactionDate AS [Date Disbursed], 

			dbo.concatname ((SELECT     PartyId
								  FROM         dbo.PartyRole
								  WHERE     (Id = dbo.Payment.ProcessedToPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS DisbursedTo, 
						  dbo.PaymentMethodType.Name AS [Payment Method], 
						  dbo.Payment.PaymentReferenceNumber AS [Check Number], 
			dbo.concatname  ((SELECT     PartyId
								  FROM         dbo.PartyRole 
								  WHERE     (Id = dbo.Payment.ProcessedByPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS DisbursedBy, 
								 dbo.Payment.Id AS PaymentId, 
			dbo.Disbursement.DisbursementTypeId
	FROM  dbo.Payment INNER JOIN
						  dbo.Disbursement ON dbo.Payment.Id = dbo.Disbursement.PaymentId INNER JOIN
						  dbo.DisbursementType ON dbo.DisbursementType.Id = dbo.Disbursement.DisbursementTypeId INNER JOIN
						  dbo.PaymentMethodType ON dbo.Payment.PaymentMethodTypeId = dbo.PaymentMethodType.Id 
	WHERE     (dbo.DisbursementType.Name = 'Other Loan Disbursement')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Payment"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 256
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Disbursement"
            Begin Extent = 
               Top = 6
               Left = 294
               Bottom = 95
               Right = 469
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DisbursementType"
            Begin Extent = 
               Top = 6
               Left = 733
               Bottom = 95
               Right = 893
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PaymentMethodType"
            Begin Extent = 
               Top = 96
               Left = 294
               Bottom = 185
               Right = 454
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PartyRole"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 245
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'OtherDisbursementDetails'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'OtherDisbursementDetails'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'OtherDisbursementDetails'
GO
/****** Object:  Table [dbo].[LoanStatement]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanStatement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinancialAccountId] [int] NOT NULL,
	[PaymentId] [int] NOT NULL,
	[TotalLoanBalance] [numeric](18, 2) NOT NULL,
	[DeficitAmount] [numeric](18, 2) NOT NULL,
	[NoOfInstallment] [numeric](2, 2) NULL,
 CONSTRAINT [PK_LOANSTATEMENT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanReAvailment]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanReAvailment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinancialAccountId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[LoanBalance] [numeric](18, 2) NOT NULL,
	[NoOfInstallments] [int] NULL,
 CONSTRAINT [PK_LOANREAVAILMENT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[PaymentHistoryViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: PaymentHistoryViewList                                 */
/*==============================================================*/
CREATE VIEW [dbo].[PaymentHistoryViewList] as
SELECT FinAcctTrans.TransactionDate		as	"Date",
		FinAcctTrans.PaymentId					as	"PaymentID",
		FinAcctTrans.Amount				as	"Amount",
		dbo.concatname((SELECT PartyId
                              FROM         dbo.PartyRole
                              WHERE     (Id = dbo.Payment.ProcessedByPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix')
										as	"CollectedBy",
		FinAcctTrans.FinancialAccountId  as "FinancialAccountId"
										
FROM FinAcctTrans 
	JOIN Payment ON FinAcctTrans.PaymentId = Payment.Id
	
WHERE
	FinAcctTrans.FinancialAcctTransTypeId = (select Id from FinlAcctTransType where Name = 'Account Payment')
	;
GO
/****** Object:  Table [dbo].[LoanAdjustment]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LoanAdjustment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinancialAccountId] [int] NOT NULL,
	[AdjustmentTypeId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[Remarks] [varchar](max) NULL,
 CONSTRAINT [PK_LOANADJUSTMENT] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[LoanDisbursementVoucherViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[LoanDisbursementVoucherViewList]
AS
SELECT loandisbursementvcr.id AS [VoucherId], loandisbursementvcr.date AS [Date], loandisbursementvcr.agreementid AS [LoanAgreementId], 
                      dbo.concatname(partyrole.partyid, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS [CustomerName], loandisbursementvcr.amount AS [Amount], 
                      disbursementvcrstatustype.name AS [Status]
FROM         loandisbursementvcr JOIN
                      disbursementvcrstatus ON loandisbursementvcr.id = disbursementvcrstatus.loandisbursementvoucherid JOIN
                      disbursementvcrstatustype ON disbursementvcrstatustype.id = disbursementvcrstatus.DisbursementVoucherStatTypId JOIN
                      agreement ON agreement.id = loandisbursementvcr.agreementid JOIN
                      agreementrole ON agreement.id = agreementrole.agreementid JOIN
                      partyrole ON partyrole.id = agreementrole.partyroleid JOIN
                      roletype ON roletype.id = partyrole.roletypeid JOIN
                      party ON party.id = partyrole.partyid 
                    WHERE     roletype.name = 'Borrower' and disbursementvcrstatus.IsActive = 1
UNION ALL
SELECT loandisbursementvcr.id AS [VoucherID], loandisbursementvcr.date AS [Date], loandisbursementvcr.agreementid AS [LoanAgreementId], 
                      organization.organizationname AS [CustomerName], loandisbursementvcr.amount AS [Amount], disbursementvcrstatustype.name AS [Status]
FROM         loandisbursementvcr JOIN
                      disbursementvcrstatus ON loandisbursementvcr.id = disbursementvcrstatus.loandisbursementvoucherid JOIN
                      disbursementvcrstatustype ON disbursementvcrstatustype.id = disbursementvcrstatus.DisbursementVoucherStatTypId JOIN
                      agreement ON agreement.id = loandisbursementvcr.agreementid JOIN
                      agreementrole ON agreement.id = agreementrole.agreementid JOIN
                      partyrole ON partyrole.id = agreementrole.partyroleid JOIN
                      roletype ON roletype.id = partyrole.roletypeid JOIN
                      party ON party.id = partyrole.partyid JOIN
                      organization ON organization.partyid = party.id
WHERE     roletype.name = 'Borrower' and disbursementvcrstatus.IsActive = 1;
GO
/****** Object:  Table [dbo].[LoanModificationNewItems]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanModificationNewItems](
	[NewFinancialAccountId] [int] NOT NULL,
	[LoanModificationPrevId] [int] NOT NULL,
 CONSTRAINT [PK_LOANMODIFICATIONNEWITEMS] PRIMARY KEY CLUSTERED 
(
	[NewFinancialAccountId] ASC,
	[LoanModificationPrevId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanDisbursement]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanDisbursement](
	[PaymentId] [int] NOT NULL,
	[LoanDisbursementTypeId] [int] NOT NULL,
	[LoanAmount] [numeric](18, 2) NOT NULL,
	[LoanBalance] [numeric](18, 2) NOT NULL,
	[InterestBalance] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_LOANDISBURSEMENT] PRIMARY KEY NONCLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[LoanApplicationViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: LoanApplicationViewList                                */
/*==============================================================*/
CREATE view [dbo].[LoanApplicationViewList] (LoanApplicationId, ApplicationDate, BorrowersName, LoanProduct, 

CollateralRequirement, Status) as

SELECT  DISTINCT   loanapplication.applicationid AS [LoanApplicationId], application.applicationdate AS [ApplicationDate], dbo.concatname(partyrole.partyid, 'Last Name', 'First Name', 
                      'Middle Name', 'Name Suffix') AS [BorrowersName], financialproduct.name AS [LoanProduct], productfeature.name AS [CollateralRequirement], 
                      loanapplicationstatustype.name AS [Status]
FROM         loanapplication JOIN
                      application ON loanapplication.applicationid = application.id JOIN
                      applicationitem ON applicationitem.applicationid = application.id JOIN
                      productfeatureapplicability ON productfeatureapplicability.id = applicationitem.prodfeatapplicabilityid JOIN
                      productfeature ON productfeature.id = productfeatureapplicability.productfeatureid JOIN
                      ProductFeatureCategory ON ProductFeature.ProductFeatCatId = ProductFeatureCategory.Id JOIN
                      financialproduct ON financialproduct.id = productfeatureapplicability.financialproductid JOIN
                      loanapplicationstatus ON loanapplicationstatus.applicationid = application.id JOIN
                      loanapplicationstatustype ON loanapplicationstatus.statustypeid = LoanApplicationStatusType.Id JOIN
                      loanapplicationrole ON loanapplicationrole.applicationid = application.id JOIN
                      partyrole ON partyrole.id = loanapplicationrole.partyroleid JOIN
                      roletype ON PartyRole.RoleTypeId = RoleType.Id JOIN
                      party ON party.id = partyrole.partyid
WHERE ProductFeatureCategory.Name = 'Collateral Requirement' AND LoanApplicationStatus.IsActive = 1 AND roletype.name = 'Borrower'
               
UNION ALL

SELECT   DISTINCT     loanapplication.applicationid AS [LoanApplicationId], application.applicationdate AS [ApplicationDate], organization.organizationname AS [BorrowersName], 
                      financialproduct.name AS [LoanProduct], productfeature.name AS [CollateralRequirement], loanapplicationstatustype.name AS [Status]
FROM         loanapplication JOIN
                      application ON loanapplication.applicationid = application.id JOIN
                      applicationitem ON applicationitem.applicationid = application.id JOIN
                      productfeatureapplicability ON productfeatureapplicability.id = applicationitem.prodfeatapplicabilityid JOIN
                      productfeature ON productfeature.id = productfeatureapplicability.productfeatureid JOIN
                      ProductFeatureCategory ON ProductFeature.ProductFeatCatId = ProductFeatureCategory.Id JOIN
                      financialproduct ON financialproduct.id = productfeatureapplicability.financialproductid JOIN
                      loanapplicationstatus ON loanapplicationstatus.applicationid = application.id JOIN
                      loanapplicationstatustype ON loanapplicationstatus.statustypeid = LoanApplicationStatusType.Id JOIN
                      loanapplicationrole ON loanapplicationrole.applicationid = application.id JOIN
                      partyrole ON partyrole.id = loanapplicationrole.partyroleid JOIN
                      party ON party.id = partyrole.partyid JOIN
                      roletype ON PartyRole.RoleTypeId = RoleType.Id JOIN
                      organization ON organization.partyid = party.id

WHERE ProductFeatureCategory.Name = 'Collateral Requirement' AND LoanApplicationStatus.IsActive = 1 AND roletype.name = 'Borrower';
GO
/****** Object:  View [dbo].[LoanAgreementViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: LoanAgreementViewList                                  */
/*==============================================================*/
CREATE view [dbo].[LoanAgreementViewList] as
SELECT agreement.id as "AgreementId",
	agreement.agreementdate as "Date",	
	dbo.concatname(partyrole.partyid, 'Last Name' , 'First Name','Middle Name','Name Suffix')  as "Name"

FROM agreement	JOIN agreementrole ON agreementrole.agreementid = agreement.id
		JOIN partyrole ON partyrole.id = agreementrole.partyroleid
		
UNION ALL


SELECT agreement.id as "AgreementId",
	agreement.agreementdate as "Date",
	organization.organizationname  as "Name"

FROM agreement	JOIN agreementrole ON agreementrole.agreementid = agreement.id
		JOIN partyrole ON partyrole.id = agreementrole.partyroleid
		JOIN organization ON organization.partyid = partyrole.partyid;
GO
/****** Object:  Table [dbo].[InterestItems]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterestItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoanId] [int] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_InterestItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanAccountStatus]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LoanAccountStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusTypeId] [int] NOT NULL,
	[FinancialAccountId] [int] NULL,
	[Remarks] [varchar](max) NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_LOANACCOUNTSTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AmortizationScheduleItem]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AmortizationScheduleItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AmortizationScheduleId] [int] NOT NULL,
	[ScheduledPaymentDate] [datetime] NOT NULL,
	[PrincipalPayment] [numeric](18, 2) NOT NULL,
	[InterestPayment] [numeric](18, 2) NOT NULL,
	[PrincipalBalance] [numeric](18, 2) NOT NULL,
	[TotalLoanBalance] [numeric](18, 2) NOT NULL,
	[IsBilledIndicator] [bit] NOT NULL,
 CONSTRAINT [PK_AMORTIZATIONSCHEDULEITEM] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[AllowedEmployeeViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* Table: [AllowedEmployeeViewList]                            */
/*==============================================================*/
CREATE view [dbo].[AllowedEmployeeViewList]
AS

SELECT DISTINCT 
	Party.Id as "PartyId",
	dbo.concatname(Party.Id, 'Last Name' , 'First Name','Middle Name','Name Suffix') as "Name"

FROM
	Party 
	JOIN PartyType ON Party.PartyTypeId = PartyType.Id
	JOIN Partyrole ON PartyRole.PartyId = Party.Id
	JOIN RoleType ON RoleType.Id = PartyRole.RoleTypeId

WHERE
	Party.PartyTypeId = PartyType.Id AND partyrole.PartyId = Party.Id AND RoleType.Id = Partyrole.RoleTypeId
	AND PartyType.Name = 'Person' 
	AND RoleType.Name IN ('Customer','Contact','Employer') 
	AND Party.Id NOT IN (SELECT DISTINCT Party.Id FROM Party
	JOIN PartyRole ON Party.Id = PartyRole.PartyId
	JOIN RoleType ON PartyRole.RoleTypeId = RoleType.Id
	JOIN PartyRelationship ON PartyRole.Id = PartyRelationship.FirstPartyRoleId
	JOIN PartyRelType ON PartyRelationship.PartyRelTypeId = PartyRelType.Id
	WHERE PartyRole.PartyId = Party.Id 
		AND PartyRole.RoleTypeId = RoleType.Id 
		AND RoleType.Name = 'Employee'
		AND PartyRelType.Name = 'Employment'
		AND PartyRelationship.EndDate IS NULL
		AND PartyRelationship.SecondPartyRoleId = (SELECT PartyRole.Id FROM PartyRole
												   JOIN RoleType ON PartyRole.RoleTypeId = RoleType.Id
												   WHERE RoleType.Name = 'Lending Institution'))
	AND PartyRole.EndDate IS NULL;
GO
/****** Object:  Table [dbo].[ChequeStatus]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChequeStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CheckStatusTypeId] [int] NOT NULL,
	[CheckId] [int] NOT NULL,
	[Remarks] [varchar](max) NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CHEQUESTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChequeLoanAssoc]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChequeLoanAssoc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChequeId] [int] NOT NULL,
	[FinancialAccountId] [int] NOT NULL,
 CONSTRAINT [PK_ChequeLoanAssoc_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChequeApplicationAssoc]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChequeApplicationAssoc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChequeId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
 CONSTRAINT [PK_ChequeApplicationAssoc_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[AgreementViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AgreementViewList]
AS
SELECT     loandisbursementvcr.agreementid AS [LoanAgreementID], dbo.concatname(dbo.PartyRole.PartyId, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') 
                      AS [CustomerName], financialproduct.name AS [LoanProduct], LoanDisbursementVcr.Id AS [LoanDisbursementVcrId]
FROM         loandisbursementvcr JOIN
                      agreement ON loandisbursementvcr.agreementid = agreement.id JOIN
                      agreementrole ON agreementrole.agreementid = agreement.id JOIN
                      application ON application.id = agreement.applicationid JOIN
                      applicationitem ON applicationitem.applicationid = application.id JOIN
                      productfeatureapplicability ON applicationitem.prodfeatapplicabilityid = productfeatureapplicability.id JOIN
                      financialproduct ON financialproduct.id = productfeatureapplicability.financialproductid JOIN
                      partyrole ON partyrole.id = agreementrole.partyroleid JOIN
                      roletype ON roletype.id = partyrole.roletypeid JOIN
                      party ON party.id = partyrole.partyid JOIN
                      disbursementvcrstatus ON disbursementvcrstatus.LoanDisbursementVoucherId = loandisbursementvcr.Id JOIN
                      disbursementvcrstatustype ON disbursementvcrstatus.DisbursementVoucherStatTypId = disbursementvcrstatustype.Id
WHERE	  disbursementvcrstatustype.Name = 'Pending' and disbursementvcrstatus.IsActive = 1
UNION ALL
SELECT     loandisbursementvcr.agreementid AS [LoanAgreementID], organization.OrganizationName AS [CustomerName], financialproduct.name AS [LoanProduct], 
                      LoanDisbursementVcr.Id AS [LoanDisbursementVcrId]
FROM         loandisbursementvcr JOIN
                      agreement ON loandisbursementvcr.agreementid = agreement.id JOIN
                      agreementrole ON agreementrole.agreementid = agreement.id JOIN
                      application ON application.id = agreement.applicationid JOIN
                      applicationitem ON applicationitem.applicationid = application.id JOIN
                      productfeatureapplicability ON applicationitem.prodfeatapplicabilityid = productfeatureapplicability.id JOIN
                      financialproduct ON financialproduct.id = productfeatureapplicability.financialproductid JOIN
                      partyrole ON partyrole.id = agreementrole.partyroleid JOIN
                      roletype ON roletype.id = partyrole.roletypeid JOIN
                      party ON party.id = partyrole.partyid JOIN
                      Organization ON Organization.partyId = party.id JOIN
                      disbursementvcrstatus ON disbursementvcrstatus.LoanDisbursementVoucherId = loandisbursementvcr.Id JOIN
                      disbursementvcrstatustype ON disbursementvcrstatus.DisbursementVoucherStatTypId = disbursementvcrstatustype.Id
WHERE	  disbursementvcrstatustype.Name = 'Pending' and disbursementvcrstatus.IsActive = 1;
GO
/****** Object:  View [dbo].[CollectionViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CollectionViewList]
AS
SELECT DISTINCT COALESCE (dbo.LoanPayment.PaymentId, dbo.FeePayment.PaymentId) AS CollectionId, dbo.Payment.TransactionDate AS Date, dbo.concatname
                          ((SELECT     PartyId
                              FROM         dbo.PartyRole
                              WHERE     (Id = dbo.Payment.ProcessedToPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS Customer, 
                      dbo.Payment.TotalAmount AS Amount, dbo.PaymentMethodType.Name AS PaymentType, dbo.concatname
                          ((SELECT     PartyId
                              FROM         dbo.PartyRole AS PartyRole_2
                              WHERE     (Id = dbo.Payment.ProcessedByPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS Collector
FROM         dbo.Payment LEFT OUTER JOIN
                      dbo.LoanPayment ON dbo.Payment.Id = dbo.LoanPayment.PaymentId LEFT OUTER JOIN
                      dbo.FeePayment ON dbo.FeePayment.PaymentId = dbo.Payment.Id INNER JOIN
                      dbo.PaymentMethodType ON dbo.Payment.PaymentMethodTypeId = dbo.PaymentMethodType.Id INNER JOIN
                      dbo.PartyRole AS PartyRole_1 ON dbo.Payment.ProcessedByPartyRoleId = PartyRole_1.Id
WHERE     (dbo.Payment.TotalAmount > 0) AND (dbo.LoanPayment.PaymentId IS NOT NULL) OR
                      (dbo.Payment.TotalAmount > 0) AND (dbo.FeePayment.PaymentId IS NOT NULL);
GO
/****** Object:  Table [dbo].[DisbursementItem]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DisbursementItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentId] [int] NOT NULL,
	[Particular] [varchar](50) NOT NULL,
	[PerItemAmount] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_DISBURSEMENTITEM] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[DisbursementDetailsView]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: DisbursementDetailsView                                */
/*==============================================================*/
CREATE view [dbo].[DisbursementDetailsView] as

SELECT Payment.TransactionDate as	"DateDisbursed",
	dbo.concatname((SELECT PartyId
					FROM dbo.PartyRole
					WHERE Id = dbo.Payment.ProcessedToPartyRoleId), 'Last Name' , 'First Name','Middle Name','Name Suffix') AS "DisbursedTo",	
		PaymentMethodType.Name			as	"PaymentMethod",
		Payment.PaymentReferenceNumber	as	"CheckNumber",		
	dbo.concatname((SELECT PartyId
					FROM dbo.PartyRole
					WHERE Id = dbo.Payment.ProcessedByPartyRoleId), 'Last Name' , 'First Name','Middle Name','Name Suffix') as "DisbursedBy"
														
FROM Payment JOIN Disbursement ON Payment.Id = Disbursement.PaymentId
	JOIN PaymentMethodType ON Payment.PaymentMethodTypeId = PaymentMethodType.Id
	JOIN dbo.PartyRole ON dbo.Payment.ProcessedByPartyRoleId = dbo.PartyRole.Id INNER JOIN
         dbo.Party ON dbo.PartyRole.PartyId = Party.Id INNER JOIN
         dbo.PartyType ON dbo.PartyType.Id = dbo.Party.PartyTypeId

WHERE     PartyType.Name = 'Person'

UNION ALL

SELECT   Payment.TransactionDate as	"DateDisbursed",
	dbo.concatname((SELECT PartyId
					FROM dbo.PartyRole
					WHERE Id = dbo.Payment.ProcessedToPartyRoleId), 'Last Name' , 'First Name','Middle Name','Name Suffix') AS "DisbursedTo",	
		PaymentMethodType.Name			as	"PaymentMethod",
		Payment.PaymentReferenceNumber	as	"CheckNumber",	
		dbo.Organization.OrganizationName AS DisbursedBy

FROM Payment JOIN Disbursement ON Payment.Id = Disbursement.PaymentId
		JOIN PaymentMethodType ON Payment.PaymentMethodTypeId = PaymentMethodType.Id INNER JOIN
                      dbo.PartyRole ON dbo.Payment.ProcessedByPartyRoleId = dbo.PartyRole.Id INNER JOIN
                      dbo.Party ON dbo.PartyRole.PartyId = Party.Id INNER JOIN
                      dbo.Organization ON dbo.Organization.PartyId = Party.Id INNER JOIN
                      dbo.PartyType ON dbo.PartyType.Id = dbo.Party.PartyTypeId

WHERE     PartyType.Name = 'Organization';
GO
/****** Object:  Table [dbo].[DemandLetter]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DemandLetter](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinancialAccountId] [int] NOT NULL,
	[DateSent] [datetime] NULL,
	[DatePromisedToPay] [datetime] NULL,
 CONSTRAINT [PK_DEMANDLETTER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinlAcctTransnStatus]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinlAcctTransnStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusTypeId] [int] NOT NULL,
	[FinancialAcctTransactionId] [int] NOT NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_FINLACCTTRANSNSTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialAcctNotification]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FinancialAcctNotification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinancialAcctNotificationId] [int] NULL,
	[FinancialAcctTransactionId] [int] NULL,
	[Date] [datetime] NOT NULL,
	[Message] [varchar](max) NOT NULL,
 CONSTRAINT [PK_FINANCIAL_ACCT_NOTIFICATION2] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FinAcctTransTask]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FinAcctTransTask](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionTaskTypeId] [int] NOT NULL,
	[UomId] [int] NULL,
	[FinancialAcctTransactionId] [int] NULL,
	[TaskCreationDate] [datetime] NULL,
	[RequestedDate] [datetime] NULL,
	[Description] [varchar](256) NOT NULL,
 CONSTRAINT [PK_FINACCTTRANSTASK] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ElectronicAddress]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ElectronicAddress](
	[AddressId] [int] NOT NULL,
	[ElectronicAddressTypeId] [int] NULL,
	[ElectronicAddressString] [varchar](100) NOT NULL,
	[IsPrimary] [bit] NOT NULL,
 CONSTRAINT [PK_ELECTRONICADDRESS] PRIMARY KEY NONCLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[ElectronicAddress] ([AddressId], [ElectronicAddressTypeId], [ElectronicAddressString], [IsPrimary]) VALUES (6, 2, N'mnpamaran@gmail.com', 1)
INSERT [dbo].[ElectronicAddress] ([AddressId], [ElectronicAddressTypeId], [ElectronicAddressString], [IsPrimary]) VALUES (10, 2, N'pamaranlending@yahoo.com', 1)
INSERT [dbo].[ElectronicAddress] ([AddressId], [ElectronicAddressTypeId], [ElectronicAddressString], [IsPrimary]) VALUES (14, 2, N'', 1)
INSERT [dbo].[ElectronicAddress] ([AddressId], [ElectronicAddressTypeId], [ElectronicAddressString], [IsPrimary]) VALUES (71, 1, N'', 1)
/****** Object:  View [dbo].[DisbursementViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[DisbursementViewList]
AS
SELECT distinct dbo.Disbursement.PaymentId AS DisbursementId, dbo.Payment.TransactionDate AS Date,  
dbo.concatname((SELECT     PartyId
                              FROM         dbo.PartyRole
                              WHERE     (Id = dbo.Payment.ProcessedToPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS DisbursedTo, 
dbo.Payment.TotalAmount AS Amount, 
dbo.PaymentMethodType.Name AS Type, 
dbo.concatname((SELECT     PartyId
                              FROM         dbo.PartyRole
                              WHERE     (Id = dbo.Payment.ProcessedByPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS DisbursedBy, 
dbo.Disbursement.DisbursementTypeId, 
dbo.DisbursementType.Name AS DisbursementType
FROM         dbo.Disbursement INNER JOIN
                      dbo.Payment ON dbo.Disbursement.PaymentId = dbo.Payment.Id INNER JOIN
                      dbo.DisbursementType ON dbo.DisbursementType.Id = dbo.Disbursement.DisbursementTypeId INNER JOIN
                      dbo.PaymentMethodType ON dbo.Payment.PaymentMethodTypeId = dbo.PaymentMethodType.Id INNER JOIN
                      dbo.PartyRole ON dbo.Payment.ProcessedByPartyRoleId = dbo.PartyRole.Id INNER JOIN
                      dbo.Party ON dbo.PartyRole.PartyId = dbo.Party.Id INNER JOIN
                      dbo.PartyType ON dbo.PartyType.Id = dbo.Party.PartyTypeId
WHERE     PartyType.Name = 'Person'
UNION ALL
SELECT   DISTINCT  dbo.Disbursement.PaymentId AS DisbursementId, dbo.Payment.TransactionDate AS Date, dbo.concatname
                          ((SELECT     PartyId
                              FROM         dbo.PartyRole
                              WHERE     (Id = dbo.Payment.ProcessedToPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS DisbursedTo, 
dbo.Payment.TotalAmount AS Amount, 
dbo.PaymentMethodType.Name AS Type, 
dbo.Organization.OrganizationName AS DisbursedBy, 
dbo.Disbursement.DisbursementTypeId, 
dbo.DisbursementType.Name AS DisbursementType

FROM         dbo.Disbursement INNER JOIN
                      dbo.Payment ON dbo.Disbursement.PaymentId = dbo.Payment.Id INNER JOIN
                      dbo.DisbursementType ON dbo.DisbursementType.Id = dbo.Disbursement.DisbursementTypeId INNER JOIN
                      dbo.PaymentMethodType ON dbo.Payment.PaymentMethodTypeId = dbo.PaymentMethodType.Id INNER JOIN
                      dbo.PartyRole ON dbo.Payment.ProcessedByPartyRoleId = dbo.PartyRole.Id INNER JOIN
                      dbo.Party ON dbo.PartyRole.PartyId = Party.Id INNER JOIN
                      dbo.Organization ON dbo.Organization.PartyId = Party.Id INNER JOIN
                      dbo.PartyType ON dbo.PartyType.Id = dbo.Party.PartyTypeId
WHERE     PartyType.Name = 'Organization';
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[7] 4[6] 2[28] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1560
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'DisbursementViewList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'DisbursementViewList'
GO
/****** Object:  Table [dbo].[Encashment]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Encashment](
	[PaymentId] [int] NOT NULL,
	[Surcharge] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_ENCASHMENT] PRIMARY KEY NONCLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinAcctTransRel]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinAcctTransRel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinancialAcctTransRelTypeId] [int] NOT NULL,
	[FromFinancialAcctTransactionId] [int] NOT NULL,
	[ToFinancialAcctTransactionId] [int] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_FINACCTTRANSREL] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[PettyCashLoanApplicationViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: PettyCashLoanApplicationViewList                       */
/*==============================================================*/
CREATE view [dbo].[PettyCashLoanApplicationViewList] (ApplicationId, ApplicationDate, Borrower_sName, Status) as

SELECT 	pettycashloanapplication.applicationid AS "ApplicationID",
	application.applicationdate AS "ApplicationDate",
	dbo.concatname(partyrole.partyid, 'Last Name' , 'First Name','Middle Name','Name Suffix') AS "Borrower’sName",
	pettycashloanappstatustype.name AS "Status"

FROM application JOIN pettycashloanapplication ON pettycashloanapplication.applicationid = application.id
		JOIN pettycashloanapplicationstatus ON pettycashloanapplicationstatus.applicationid = application.id
		JOIN pettycashloanappstatustype ON pettycashloanappstatustype.id = pettycashloanapplicationstatus.statustypeid 
		JOIN loanapplicationrole ON loanapplicationrole.applicationid = application.id
		JOIN partyrole ON partyrole.id =  loanapplicationrole.partyroleid
		JOIN party ON party.id = partyrole.partyid

union all

SELECT 	pettycashloanapplication.applicationid AS "ApplicationID",
	application.applicationdate AS "ApplicationDate",
	organization.organizationname AS "Borrower’sName",
	pettycashloanappstatustype.name AS "Status"


FROM application JOIN pettycashloanapplication ON pettycashloanapplication.applicationid = application.id
		JOIN pettycashloanapplicationstatus ON pettycashloanapplicationstatus.applicationid = application.id
		JOIN pettycashloanappstatustype ON pettycashloanappstatustype.id = pettycashloanapplicationstatus.statustypeid 
		JOIN loanapplicationrole ON loanapplicationrole.applicationid = application.id
		JOIN partyrole ON partyrole.id =  loanapplicationrole.partyroleid
		JOIN party ON party.id = partyrole.partyid
		JOIN organization ON organization.partyid = party.id;
GO
/****** Object:  View [dbo].[UserAccountsViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[UserAccountsViewList] as
SELECT 	useraccount.id as "UserAccountId", 
		UserAccount.Username as "UserName",
		useraccount.partyid as "HiddenPartyId",
		dbo.concatname(partyid, 'Last Name' , 'First Name','Middle Name','Name Suffix') as "NameOfUser",
		useraccountstatustype.name as "UserAccountStatus"

FROM useraccount JOIN useraccountstatus ON useraccount.id = useraccountstatus.useraccountid
					AND useraccount.EndDate is null 
                    AND useraccountstatus.EndDate is null
				JOIN useraccountstatustype ON  useraccountstatustype.id = useraccountstatus.statustypeid
GO
/****** Object:  Table [dbo].[TelecommunicationsNumber]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TelecommunicationsNumber](
	[AddressId] [int] NOT NULL,
	[TypeId] [int] NULL,
	[AreaCode] [varchar](5) NULL,
	[PhoneNumber] [varchar](15) NULL,
	[IsPrimary] [bit] NOT NULL,
 CONSTRAINT [PK_TELECOMMUNICATIONSNUMBER] PRIMARY KEY NONCLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (3, 5, N'', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (4, 5, N'', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (5, 3, N'', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (8, 5, N'32', N'4124124', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (9, 5, N'32', N'4124125', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (12, 5, N'62', N'215-1391', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (13, 3, N'62', N'214-3196', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (16, 5, N'62', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (17, 3, N'62', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (19, 5, N'62', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (20, 3, N'62', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (22, 5, N'62', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (23, 3, N'62', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (25, 5, N'62', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (26, 3, N'62', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (28, 5, N'62', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (29, 3, N'62', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (31, 5, N'62', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (32, 3, N'62', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (34, 5, N'62', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (35, 3, N'62', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (37, 5, N'62', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (38, 3, N'62', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (40, 5, N'62', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (41, 3, N'62', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (43, 5, N'62', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (44, 3, N'62', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (46, 5, N'62', N'', 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (47, 3, N'62', N'', 0)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (48, 5, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (49, 3, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (50, 5, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (51, 3, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (52, 5, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (53, 3, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (54, 5, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (55, 3, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (56, 5, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (57, 3, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (58, 5, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (59, 3, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (60, 5, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (61, 3, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (62, 5, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (63, 3, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (64, 5, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (65, 3, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (66, 5, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (67, 3, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (68, 5, N'62', NULL, 1)
INSERT [dbo].[TelecommunicationsNumber] ([AddressId], [TypeId], [AreaCode], [PhoneNumber], [IsPrimary]) VALUES (69, 3, N'62', NULL, 1)
/****** Object:  View [dbo].[SubmittedDocumentView]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SubmittedDocumentView] as

SELECT RequiredDocumentType.Name		as	"DocumentName",
		SubmittedDocument.DateSubmitted		as	"Date",
		SubmittedDocumentStatusType.Name	as	"Status",
		SubmittedDocument.Description		as	"Description"
	
	FROM SubmittedDocument JOIN SubmittedDocumentStatus ON SubmittedDocument.Id = SubmittedDocumentStatus.SubmittedDocumentId
			AND SubmittedDocumentStatus.IsActive = 1
		JOIN SubmittedDocumentStatusType ON SubmittedDocumentStatus.StatusTypeId = SubmittedDocumentStatusType.Id
		JOIN ProductRequiredDocument ON SubmittedDocument.ProductRequiredDocumentId = ProductRequiredDocument.Id
		JOIN RequiredDocumentType ON ProductRequiredDocument.RequiredDocumentTypeId = RequiredDocumentType.Id
		JOIN LoanApplication ON SubmittedDocument.ApplicationId = LoanApplication.ApplicationId
;
GO
/****** Object:  Table [dbo].[Receivable]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Receivable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReceivableTypeId] [int] NOT NULL,
	[FinancialAccountId] [int] NULL,
	[PaymentId] [int] NULL,
	[ValidityPeriod] [datetime] NOT NULL,
	[Date] [datetime] NOT NULL,
	[DueDate] [datetime] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[Balance] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_RECEIVABLE] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VoucherViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: VoucherViewList                                */
/*==============================================================*/
CREATE view [dbo].[VoucherViewList] as
SELECT DISTINCT 
                      loandisbursementvcr.id AS [VoucherId], loandisbursementvcr.agreementid AS [AgreementId], dbo.concatname(partyrole.partyid, 'Last Name', 'First Name', 
                      'Middle Name', 'Name Suffix') AS [CustomerName], loandisbursementvcr.Balance AS [LoanAmount], dbo.FinancialProduct.Name AS [LoanProduct], 
                      dbo.PartyRole.Id AS [CustomerID], dbo.FinancialProduct.Id AS [LoanProductId], DisbursementVcrStatusType.Name as type
FROM         loandisbursementvcr JOIN
                      disbursementvcrstatus ON loandisbursementvcr.id = disbursementvcrstatus.loandisbursementvoucherid JOIN
                      disbursementvcrstatustype ON disbursementvcrstatustype.id = disbursementvcrstatus.DisbursementVoucherStatTypId JOIN
                      agreement ON agreement.id = loandisbursementvcr.agreementid JOIN
                      agreementrole ON agreement.id = agreementrole.agreementid JOIN
                      partyrole ON partyrole.id = agreementrole.partyroleid JOIN
					  ApplicationItem on Agreement.ApplicationId = ApplicationItem.ApplicationId join
					  ProductFeatureApplicability on ApplicationItem.ProdFeatApplicabilityId = ProductFeatureApplicability.Id join
					  FinancialProduct on ProductFeatureApplicability.FinancialProductId = FinancialProduct.Id join
                      roletype ON roletype.id = partyrole.roletypeid JOIN
                      party ON party.id = partyrole.partyid JOIN
                      PartyType ON party.PartyTypeId = PartyType.Id
WHERE roletype.name = 'Borrower' AND disbursementvcrstatus.IsActive = 1  and (DisbursementVcrStatusType.Name = 'Approved' OR
                      DisbursementVcrStatusType.Name = 'Partially Disbursed')
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "LoanDisbursementVcr"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Agreement"
            Begin Extent = 
               Top = 6
               Left = 236
               Bottom = 125
               Right = 420
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DisbursementVcrStatus"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 245
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DisbursementVcrStatusType"
            Begin Extent = 
               Top = 126
               Left = 317
               Bottom = 215
               Right = 477
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AgreementRole"
            Begin Extent = 
               Top = 216
               Left = 317
               Bottom = 320
               Right = 477
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PartyRole"
            Begin Extent = 
               Top = 246
               Left = 38
               Bottom = 365
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RoleType"
            Begin Extent = 
               Top = 324
               Left = 236
               Bottom = 428
               Right ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VoucherViewList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'= 412
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FinancialAccountRole"
            Begin Extent = 
               Top = 366
               Left = 38
               Bottom = 470
               Right = 217
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FinancialAccountProduct"
            Begin Extent = 
               Top = 432
               Left = 255
               Bottom = 551
               Right = 434
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FinancialProduct"
            Begin Extent = 
               Top = 474
               Left = 38
               Bottom = 593
               Right = 248
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VoucherViewList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VoucherViewList'
GO
/****** Object:  View [dbo].[UsersViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: UsersViewList                                          */
/*==============================================================*/
CREATE view [dbo].[UsersViewList] as
SELECT DISTINCT party.id as "PartyId", 
	dbo.concatname(address.partyid, 'Last Name' , 'First Name','Middle Name','Name Suffix') as "Name",
	isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "Address"	 

FROM party JOIN address ON address.partyid = party.id
			AND Address.EndDate IS NULL
        JOIN PartyRole ON PartyRole.PartyId = party.Id
			AND partyRole.EndDate is NULL
		JOIN RoleType ON RoleType.Id = partyrole.RoleTypeId
			AND roletype.Name = 'Employee'
		JOIN partytype ON partytype.id = party.partytypeid
			AND partytype.name = 'Person'
		JOIN postaladdress ON postaladdress.addressid = address.addressid
			AND postaladdress.isprimary = 1
			AND PostalAddress.PostalAddressTypeId = (select id from PostalAddressType where Name = 'Home Address')
		JOIN country ON country.id = postaladdress.countryid
		JOIN PartyRelationship ON PartyRelationship.FirstPartyRoleId = partyrole.Id
			AND PartyRelationship.SecondPartyRoleId = 
				(SELECT ID FROM PartyRole WHERE partyrole.RoleTypeId = 
					(select ID from RoleType where Name = 'Lending Institution')
				AND PartyRole.EndDate IS NULL)
			AND PartyRelationship.PartyRelTypeId = (SELECT Id FROM PartyRelType WHERE Name = 'Employment')
			AND PartyRelationship.EndDate IS NULL
		JOIN Employment ON Employment.PartyRelationshipId = PartyRelationship.Id
			AND Employment.EmploymentStatus = 'Employed' 
		JOIN UserAccount ON UserAccount.PartyId = Party.Id
			OR NOT EXISTS (select ID from UserAccount where Party.Id = UserAccount.PartyId)
		JOIN UserAccountStatus ON UserAccountstatus.UserAccountId = UserAccount.Id
			AND ((UserAccountStatus.StatusTypeId = (select ID from UserAccountStatusType where name = 'Inactive') 
			AND UserAccountStatus.EndDate IS NULL)
			OR NOT EXISTS (select ID from UserAccount where Party.Id = UserAccount.PartyId))
;
GO
/****** Object:  Table [dbo].[ReceivableStatus]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceivableStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReceivableId] [int] NULL,
	[StatusTypeId] [int] NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_RECEIVABLESTATUS] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReceivableAdjustment]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReceivableAdjustment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReceivableId] [int] NOT NULL,
	[AdjustmentTypeId] [int] NOT NULL,
	[PartyRoleId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[Remarks] [varchar](max) NULL,
 CONSTRAINT [PK_RECEIVABLEADJUSTMENT] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReleaseStatement]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReleaseStatement](
	[ReleaseStatementId] [int] IDENTITY(1,1) NOT NULL,
	[FinancialAccountId] [int] NOT NULL,
	[PaymentId] [int] NOT NULL,
	[TotalLoanBalance] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_RELEASESTATEMENT] PRIMARY KEY CLUSTERED 
(
	[ReleaseStatementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[RediscountingViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[RediscountingViewList] as
SELECT     (SELECT     OrganizationName
                       FROM          dbo.Organization
                       WHERE      (PartyId =
                                                  (SELECT     PartyId
                                                    FROM          dbo.PartyRole
                                                    WHERE      (Id = dbo.Cheque.BankPartyRoleId)))) AS Bank, Payment_1.PaymentReferenceNumber AS [Check Number], 
                      dbo.Cheque.CheckDate AS [Check Date],
                          (SELECT     TotalAmount
                            FROM          dbo.Payment
                            WHERE      (Id = Payment_1.ParentPaymentId)) AS [Check Amount], Payment_1.TransactionDate AS [Date Disbursed], dbo.concatname
                          ((SELECT     PartyId
                              FROM         dbo.PartyRole
                              WHERE     (Id = Payment_1.ProcessedToPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS DisbursedTo, 
                      dbo.Encashment.Surcharge AS [Surcharge Fee], Payment_1.TotalAmount AS [Amount Disbursed], Payment_1.Id AS PaymentId, dbo.concatname
                          ((SELECT     PartyId
                              FROM         dbo.PartyRole
                              WHERE     (Id = Payment_1.ProcessedByPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS [Disbursed By], 
                      dbo.Disbursement.DisbursementTypeId
FROM         dbo.Payment AS Payment_1 INNER JOIN
                      dbo.PartyRole AS PartyRole_1 ON Payment_1.ProcessedByPartyRoleId = PartyRole_1.Id INNER JOIN
                      dbo.Disbursement ON Payment_1.Id = dbo.Disbursement.PaymentId INNER JOIN
                      dbo.DisbursementType ON dbo.DisbursementType.Id = dbo.Disbursement.DisbursementTypeId INNER JOIN
                      dbo.Encashment ON dbo.Disbursement.PaymentId = dbo.Encashment.PaymentId INNER JOIN
                      dbo.Cheque ON Payment_1.ParentPaymentId = dbo.Cheque.PaymentId INNER JOIN
                      dbo.RoleType ON PartyRole_1.RoleTypeId = dbo.RoleType.Id
WHERE     (dbo.DisbursementType.Name = 'Rediscounting')
GO
/****** Object:  Table [dbo].[PaymentApplication]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentApplication](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentId] [int] NOT NULL,
	[ReceivableId] [int] NULL,
	[FinancialAccountId] [int] NULL,
	[LoanDisbursementVoucherId] [int] NULL,
	[AmountApplied] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_PAYMENTAPPLICATION] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[EncashmentViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[EncashmentViewList] as
SELECT     ((Select Organization.OrganizationName From Organization WHERE 
(Organization.PartyId=(select PartyRole.PartyId from dbo.PartyRole where dbo.PartyRole.Id = Cheque.BankPartyRoleId))))
 AS
 [Bank], Payment_1.PaymentReferenceNumber AS [Check Number], dbo.Cheque.CheckDate AS [Check Date],
                          (SELECT     TotalAmount
                            FROM          dbo.Payment
                            WHERE      (Id = Payment_1.ParentPaymentId)) AS [Check Amount], Payment_1.TransactionDate AS [Date Disbursed], dbo.concatname
                          ((SELECT     PartyId
                              FROM         dbo.PartyRole
                              WHERE     (Id = Payment_1.ProcessedToPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS DisbursedTo, 
                      dbo.Encashment.Surcharge AS [Surcharge Fee], Payment_1.TotalAmount AS [Amount Disbursed], Payment_1.Id AS PaymentId, dbo.concatname
                          ((SELECT     PartyId
                              FROM         dbo.PartyRole
                              WHERE     (Id = Payment_1.ProcessedByPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS [Disbursed By], 
                      dbo.Disbursement.DisbursementTypeId
FROM         dbo.Payment AS Payment_1 INNER JOIN
                      dbo.PartyRole AS PartyRole_1 ON Payment_1.ProcessedByPartyRoleId = PartyRole_1.Id INNER JOIN
                      dbo.Disbursement ON Payment_1.Id = dbo.Disbursement.PaymentId INNER JOIN
                      dbo.DisbursementType ON dbo.DisbursementType.Id = dbo.Disbursement.DisbursementTypeId INNER JOIN
                      dbo.Encashment ON dbo.Disbursement.PaymentId = dbo.Encashment.PaymentId INNER JOIN
                      dbo.Cheque ON Payment_1.ParentPaymentId = dbo.Cheque.PaymentId INNER JOIN
                      dbo.RoleType ON PartyRole_1.RoleTypeId = dbo.RoleType.Id
WHERE     (dbo.DisbursementType.Name = 'Encashment');
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[16] 4[6] 2[49] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Payment_1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 256
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Disbursement"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 215
               Right = 213
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DisbursementType"
            Begin Extent = 
               Top = 6
               Left = 718
               Bottom = 95
               Right = 878
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Encashment"
            Begin Extent = 
               Top = 126
               Left = 251
               Bottom = 215
               Right = 411
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Organization"
            Begin Extent = 
               Top = 216
               Left = 245
               Bottom = 335
               Right = 429
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RoleType"
            Begin Extent = 
               Top = 336
               Left = 38
               Bottom = 440
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PartyRole_1"
            Begin Extent = 
               Top = 6
               Left = 294
               Bottom = 125
               Right = 454
            End
      ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'EncashmentViewList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'      DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cheque"
            Begin Extent = 
               Top = 216
               Left = 38
               Bottom = 335
               Right = 207
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 12
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'EncashmentViewList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'EncashmentViewList'
GO
/****** Object:  View [dbo].[EmployersViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: EmployersViewList                                      */
/*==============================================================*/
CREATE view [dbo].[EmployersViewList] as
SELECT	PartyType.Name		as	"PartyType",
		PartyRole.Id		as	"EmployerID",
		dbo.concatname(PartyRole.PartyId, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix')
							as	"Name",
		
		isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "BusinessAddress"
         
FROM	PartyRole JOIN Address ON PartyRole.PartyId = Address.PartyId 
			AND partyrole.roletypeid = (select id from roletype where name = 'Employer')
			AND partyrole.enddate is NULL 
			AND address.enddate is NULL 
		JOIN PostalAddress ON Address.AddressId = PostalAddress.AddressId  
		JOIN postaladdresstype ON postaladdresstype.id = postaladdress.postaladdresstypeid
			AND postaladdresstype.name = 'Business Address'
		JOIN Country ON postaladdress.CountryId = Country.Id 
		JOIN Party ON PartyRole.PartyId = Party.Id		
		JOIN PartyType ON Party.PartyTypeId = PartyType.Id
			AND PartyType.Name = 'Person'
		
UNION ALL

SELECT	PartyType.Name					as	"PartyType",
		PartyRole.Id					as	"EmployerID",
		Organization.OrganizationName	as	"Name",
		
		isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "Business Address"
         
FROM	PartyRole JOIN Address ON PartyRole.PartyId = Address.PartyId
			AND partyrole.roletypeid = (select id from roletype where name = 'Employer')
			AND partyrole.enddate is NULL 
			AND address.enddate is NULL 
		JOIN PostalAddress ON Address.AddressId = PostalAddress.AddressId  
		JOIN postaladdresstype ON postaladdresstype.id = postaladdress.postaladdresstypeid
			AND postaladdresstype.name = 'Business Address'
		JOIN Country ON postaladdress.CountryId = Country.Id 
		JOIN Party ON PartyRole.PartyId = Party.Id		
		JOIN PartyType ON Party.PartyTypeId = PartyType.Id
			AND PartyType.Name = 'Organization'
		JOIN Organization ON PartyRole.PartyId = Organization.PartyId
;
GO
/****** Object:  View [dbo].[EmployeeViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: EmployeeViewList                                       */
/*==============================================================*/
CREATE view [dbo].[EmployeeViewList] as

SELECT	DISTINCT PartyRole.Id			as	"EmployeeId",
	Employee.EmployeeIdNumber	as	"EmployeeIdNumber",
	dbo.concatname(partyrole.partyid, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix')
											as	"Name",
	
	isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(postaladdress.State as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "Address",
         
	Employment.EmploymentStatus	as	"EmploymentStatus"

FROM	PartyRole JOIN Address ON PartyRole.PartyId = Address.PartyId AND partyrole.roletypeid = (select id from roletype where name = 'Employee')
		JOIN Employee ON PartyRole.Id = Employee.PartyRoleId		
	JOIN PartyRelationship ON PartyRole.Id = PartyRelationship.FirstPartyRoleId 
		AND partyrelationship.secondpartyroleid = (SELECT id from partyrole where roletypeid = (select id from roletype where name = 'Lending Institution' ))
		AND partyrelationship.partyrelTypeid = (select id from partyreltype where name = 'Employment')
 		AND partyrelationship.enddate is NULL 	
	JOIN Employment ON PartyRelationship.Id = Employment.PartyRelationshipId
	JOIN PostalAddress ON Address.AddressId = PostalAddress.AddressId 
		AND address.enddate is NULL
		AND postaladdress.isprimary = 1
	JOIN PostalAddressType ON PostalAddress.PostalAddressTypeId = PostalAddressType.Id
		AND postaladdresstype.name = 'Home Address'
	JOIN Country ON PostalAddress.CountryId = Country.Id
;
GO
/****** Object:  Table [dbo].[DemandLetterStatus]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DemandLetterStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DemandLetterId] [int] NOT NULL,
	[DemandLetterStatusTypeId] [int] NOT NULL,
	[Remarks] [text] NULL,
	[TransitionDateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_DEMANDLETTERSTATUS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[CustomerWithLoanViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[CustomerWithLoanViewList] as

SELECT partyrole.id AS "CustomerId", 
	dbo.concatname(partyrole.partyid, 'Last Name' , 'First Name','Middle Name','Name Suffix') AS "Name",
	isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "Address",
	partytype.name AS "PartyType",
	customerstatustype.name AS "Status"

FROM partyrole 	JOIN personname ON partyrole.partyid = personname.partyid
			AND partyrole.roletypeid = (select id from roletype where name = 'Customer')
			AND personname.enddate IS NULL 
		JOIN party ON party.id = partyrole.partyid
		JOIN address ON personname.partyid = address.partyid 
			AND address.enddate is NULL 
		JOIN postaladdress ON address.addressid = postaladdress.addressid 
			AND postaladdress.isprimary = 1
		JOIN postaladdresstype ON postaladdresstype.id = postaladdress.postaladdresstypeid 
			AND postaladdresstype.name = 'Home Address'
		JOIN country ON postaladdress.countryid = country.id		
		JOIN partytype ON partytype.id = party.partytypeid
		    AND partytype.name = 'Person'
        JOIN customerstatus ON customerstatus.partyroleid = partyrole.id
		JOIN customerstatustype ON customerstatustype.id = customerstatus.customerstatustypeid	
		JOIN partyrelationship ON partyrelationship.firstpartyroleid = partyrole.id
			AND partyrelationship.partyreltypeid = (select id from partyreltype where name = 'Customer Relationship')
			AND partyrelationship.secondpartyroleid = (SELECT id from partyrole where roletypeid = (select id from roletype where name = 'Lending Institution' ) and enddate is null)
			AND partyrelationship.enddate IS NULL
		JOIN partyreltype ON partyreltype.id = partyrelationship.partyreltypeid
		JOIN roletype ON roletype.id = partyrole.roletypeid
		JOIN FinancialAccountRole ON FinancialAccountRole.PartyRoleId = PartyRole.Id
		JOIN FinancialAccount ON FinancialAccount.Id = FinancialAccountRole.FinancialAccountId
			AND financialaccount.Id is  not null
        JOIN LoanApplicationRole ON LoanapplicationRole.PartyRoleId = partyrole.id 
        JOIN LoanApplication ON Loanapplication.ApplicationId = LoanApplicationRole.ApplicationId 
        JOIN LoanApplicationStatus ON LoanApplicationStatus.ApplicationId = LoanApplication.ApplicationId 
        JOIN LoanApplicationStatusType ON LoanApplicationStatusType.Id = LoanApplicationStatus.StatusTypeId
            AND LoanApplicationStatusType.Name = 'Approved'
union all

SELECT partyrole.id AS "CustomerId", 
	organization.organizationname AS "Name",
	isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "Address", 
	partytype.name AS "PartyType",
	customerstatustype.name AS "Status"

FROM partyrole 	JOIN organization ON partyrole.partyid = organization.partyid 
			AND partyrole.roletypeid = (select id from roletype where name = 'Customer')
		JOIN address ON organization.partyid = address.partyid 
			AND address.enddate is NULL 
		JOIN postaladdress ON address.addressid = postaladdress.addressid 
		JOIN postaladdresstype ON postaladdresstype.id = postaladdress.postaladdresstypeid 
			AND postaladdresstype.name = 'Business Address'
		JOIN country ON postaladdress.countryid = country.id
		JOIN customerstatus ON customerstatus.partyroleid = partyrole.id
		JOIN customerstatustype ON customerstatus.customerstatustypeid = customerstatustype.id
		JOIN party ON party.id = organization.partyid
        JOIN partytype ON partytype.id = party.partytypeid	
		    AND partytype.name = 'Organization'
        JOIN partyrelationship ON partyrelationship.firstpartyroleid = partyrole.id
			AND partyrelationship.partyreltypeid = (select id from partyreltype where name = 'Customer Relationship')
			AND partyrelationship.secondpartyroleid = (SELECT id from partyrole where roletypeid = (select id from roletype where name = 'Lending Institution' )and enddate is null)
			AND partyrelationship.enddate IS NULL
		JOIN partyreltype ON partyreltype.id = partyrelationship.partyreltypeid	
		JOIN roletype ON roletype.id = partyrole.roletypeid
		JOIN FinancialAccountRole ON FinancialAccountRole.PartyRoleId = PartyRole.Id
		JOIN FinancialAccount ON FinancialAccount.Id = FinancialAccountRole.FinancialAccountId
			AND financialaccount.Id is  not null
        JOIN LoanApplicationRole ON LoanapplicationRole.PartyRoleId = partyrole.id 
        JOIN LoanApplication ON Loanapplication.ApplicationId = LoanApplicationRole.ApplicationId 
        JOIN LoanApplicationStatus ON LoanApplicationStatus.ApplicationId = LoanApplication.ApplicationId 
        JOIN LoanApplicationStatusType ON LoanApplicationStatusType.Id = LoanApplicationStatus.StatusTypeId
            AND LoanApplicationStatusType.Name = 'Approved'
;
GO
/****** Object:  View [dbo].[CustomerViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[CustomerViewList] (CustomerId, PartyId, Name, Address, PartyType, Status) as

SELECT DISTINCT partyrole.id AS "CustomerId", 
    party.Id AS "PartyId",
	dbo.concatname(partyrole.partyid, 'Last Name' , 'First Name','Middle Name','Name Suffix') AS "Name",
	isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "Address",
	partytype.name AS "PartyType",
	customerstatustype.name AS "Status"

FROM partyrole JOIN party ON party.id = partyrole.partyid	
			AND partyrole.roletypeid = (select id from roletype where name = 'Customer')
		JOIN address ON party.id = address.partyid 
			AND address.enddate is NULL 
		JOIN postaladdress ON address.addressid = postaladdress.addressid 
			AND postaladdress.isprimary = 1
		JOIN postaladdresstype ON postaladdresstype.id = postaladdress.postaladdresstypeid 
			AND postaladdresstype.name = 'Home Address'
		JOIN country ON postaladdress.countryid = country.id		
		JOIN partytype ON partytype.id = party.partytypeid
            AND partytype.name = 'Person'
		JOIN customerstatus ON customerstatus.partyroleid = partyrole.id
		JOIN customerstatustype ON customerstatustype.id = customerstatus.customerstatustypeid	
		JOIN partyrelationship ON partyrelationship.firstpartyroleid = partyrole.id
			AND partyrelationship.partyreltypeid = (select id from partyreltype where name = 'Customer Relationship')
			AND partyrelationship.secondpartyroleid = 
                (SELECT id from partyrole 
                where roletypeid = 
                (select id from roletype where name = 'Lending Institution' ) 
                and enddate is null)
			AND partyrelationship.enddate IS NULL
		JOIN partyreltype ON partyreltype.id = partyrelationship.partyreltypeid
		JOIN roletype ON roletype.id = partyrole.roletypeid
	WHERE customerstatus.isactive = 1	
union all

SELECT DISTINCT partyrole.id AS "CustomerId", 
    party.Id AS "PartyId",
    organization.organizationname AS "Name",
	isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "Address", 
	partytype.name AS "PartyType",
	customerstatustype.name AS "Status"

FROM partyrole 	JOIN organization ON partyrole.partyid = organization.partyid 
			AND partyrole.roletypeid = (select id from roletype where name = 'Customer')
		JOIN address ON organization.partyid = address.partyid 
			AND address.enddate is NULL 
		JOIN postaladdress ON address.addressid = postaladdress.addressid 
		JOIN postaladdresstype ON postaladdresstype.id = postaladdress.postaladdresstypeid 
			AND postaladdresstype.name = 'Business Address'
		JOIN country ON postaladdress.countryid = country.id
		JOIN customerstatus ON customerstatus.partyroleid = partyrole.id
		JOIN customerstatustype ON customerstatus.customerstatustypeid = customerstatustype.id
		JOIN party ON party.id = organization.partyid
		JOIN partytype ON partytype.id = party.partytypeid
               AND partytype.name = 'Organization'
		JOIN partyrelationship ON partyrelationship.firstpartyroleid = partyrole.id
			AND partyrelationship.partyreltypeid = (select id from partyreltype where name = 'Customer Relationship')
			AND partyrelationship.secondpartyroleid = 
                (SELECT id from partyrole where roletypeid = 
                    (select id from roletype where name = 'Lending Institution' )
                     and enddate is null)
			AND partyrelationship.enddate IS NULL
		JOIN partyreltype ON partyreltype.id = partyrelationship.partyreltypeid	
		JOIN roletype ON roletype.id = partyrole.roletypeid
WHERE customerstatus.isactive = 1	;
GO
/****** Object:  View [dbo].[CustomerAndCoBorrowerListView]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****CustomerAndCoBorrowerListView***/
CREATE VIEW [dbo].[CustomerAndCoBorrowerListView]
AS
SELECT DISTINCT 
                      PartyRole.PartyId AS [PartyId], PartyRole.Id AS [PartyRoleId], dbo.concatname(partyrole.partyid, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS [Name], 
                      isnull(cast(postaladdress.streetaddress AS nvarchar(max)) + ', ', '') + isnull(cast(postaladdress.barangay AS nvarchar(max)) + ', ', '') 
                      + isnull(cast(postaladdress.city AS nvarchar(max)) + ', ', '') + isnull(cast(postaladdress.municipality AS nvarchar(max)) + ', ', '') 
                      + isnull(cast(postaladdress.province AS nvarchar(max)) + ', ', '') + isnull(cast(country.name AS nvarchar(max)) + ' ', '') 
                      + isnull(cast(postaladdress.postalcode AS nvarchar(max)) + ' ', '') AS [Address], dbo.RoleType.Name as [Role]
FROM         partyrole JOIN
                      address ON partyrole.partyid = address.partyid AND (dbo.Address.EndDate IS NULL) AND (dbo.PartyRole.EndDate IS NULL) JOIN
                      dbo.AddressType ON dbo.AddressType.Id = dbo.Address.AddressTypeId AND (dbo.AddressType.Name = 'Postal Address') JOIN
                      postaladdress ON address.addressid = postaladdress.addressid AND (dbo.PostalAddress.IsPrimary = 1) JOIN
                      dbo.PostalAddressType ON dbo.PostalAddressType.Id = dbo.PostalAddress.PostalAddressTypeId AND (dbo.PostalAddressType.Name = 'Home Address') JOIN
                      country ON postaladdress.countryid = country.id JOIN
                      party ON partyrole.partyid = party.id JOIN
                      partytype ON party.partytypeid = partytype.id JOIN
                      dbo.RoleType ON dbo.RoleType.Id = dbo.PartyRole.RoleTypeId AND RoleType.Name in ('Contact', 'Employee', 'Employer', 'Spouse', 'Guarantor', 'Co-borrower', 'Customer')
UNION ALL
SELECT DISTINCT 
                      dbo.PartyRole.PartyId AS PartyId, dbo.PartyRole.Id AS PartyRoleId, dbo.concatname(dbo.PartyRole.PartyId, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') 
                      AS Name, ISNULL(CAST(dbo.PostalAddress.StreetAddress AS nvarchar(MAX)) + ', ', '') + ISNULL(CAST(dbo.PostalAddress.Barangay AS nvarchar(MAX)) + ', ', '') 
                      + ISNULL(CAST(dbo.PostalAddress.Municipality AS nvarchar(MAX)) + ', ', '') + ISNULL(CAST(dbo.PostalAddress.City AS nvarchar(MAX)) + ', ', '') 
                      + ISNULL(CAST(dbo.PostalAddress.Province AS nvarchar(MAX)) + ', ', '') + ISNULL(CAST(dbo.Country.Name AS nvarchar(MAX)) + ', ', '') 
                      + CAST(dbo.PostalAddress.PostalCode AS nvarchar(MAX)) AS [Address], dbo.RoleType.Name as [Role]
FROM         dbo.Party INNER JOIN
                      dbo.PersonName ON dbo.Party.Id = dbo.PersonName.PartyId INNER JOIN
                      dbo.Address ON dbo.Address.PartyId = dbo.Party.Id AND (dbo.Address.EndDate IS NULL) INNER JOIN
                      dbo.AddressType ON dbo.AddressType.Id = dbo.Address.AddressTypeId AND (dbo.AddressType.Name = 'Postal Address') INNER JOIN
                      dbo.PostalAddress ON dbo.PostalAddress.AddressId = dbo.Address.AddressId INNER JOIN
                      dbo.PostalAddressType ON dbo.PostalAddressType.Id = dbo.PostalAddress.PostalAddressTypeId AND (dbo.PostalAddressType.Name = 'Home Address') INNER JOIN
                      dbo.Country ON dbo.Country.Id = dbo.PostalAddress.CountryId INNER JOIN
                      dbo.PartyRole ON dbo.PartyRole.PartyId = dbo.Party.Id AND (dbo.PartyRole.EndDate IS NULL) INNER JOIN
                      dbo.RoleType ON dbo.RoleType.Id = dbo.PartyRole.RoleTypeId AND (dbo.RoleType.Name = 'Co-borrower')
GO
/****** Object:  View [dbo].[ContactViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  View [dbo].[ContactViewList]    Script Date: 07/22/2011 14:44:14 ******/
CREATE View [dbo].[ContactViewList] as
SELECT DISTINCT 
                      partyrole.id AS [PartyRoleId], partytype.name AS [PartyType], dbo.concatname(partyrole.partyid, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS [Name], 
                      isnull(cast(postaladdress.streetaddress AS nvarchar(max)) + ', ', '') + isnull(cast(postaladdress.barangay AS nvarchar(max)) + ', ', '') 
                      + isnull(cast(postaladdress.municipality AS nvarchar(max)) + ', ', '') + isnull(cast(postaladdress.city AS nvarchar(max)) + ', ', '') 
                      + isnull(cast(postaladdress.province AS nvarchar(max)) + ', ', '') + isnull(cast(country.name AS nvarchar(max)) + ', ', '') 
                      + cast(postaladdress.postalcode AS nvarchar(max)) AS [Address]
FROM         PartyRole JOIN
                      Address ON PartyRole.PartyId = Address.PartyId AND address.enddate IS NULL AND partyrole.roletypeid =
                          (SELECT     id
                            FROM          roletype
                            WHERE      name = 'Contact') JOIN
                      PostalAddress ON Address.AddressId = PostalAddress.AddressId AND PostalAddress.IsPrimary = 1 JOIN
                      postaladdresstype ON postaladdresstype.id = postaladdress.postaladdresstypeid AND postaladdresstype.name = 'Home Address' JOIN
                      Country ON PostalAddress.CountryId = Country.Id JOIN
                      Party ON PartyRole.PartyId = Party.Id JOIN
                      PartyType ON Party.PartyTypeId = PartyType.Id AND PartyType.Name = 'Person' JOIN
                      RoleType ON Partyrole.roletypeid = roletype.id JOIN
                      partyrelationship ON partyrelationship.firstpartyroleid = partyrole.id AND partyrelationship.partyreltypeid =
                          (SELECT     id
                            FROM          partyreltype
                            WHERE      name = 'Contact Relationship') AND partyrelationship.secondpartyroleid =
                          (SELECT     id
                            FROM          partyrole
                            WHERE      roletypeid =
                                                       (SELECT     id
                                                         FROM          roletype
                                                         WHERE      name = 'Lending Institution')) AND partyrelationship.enddate IS NULL JOIN
                      partyreltype ON partyreltype.id = partyrelationship.partyreltypeid
UNION ALL
SELECT DISTINCT 
                      PartyRole.Id AS [PartyRoleId], PartyType.Name AS [PartyType], Organization.OrganizationName AS [Name], isnull(cast(postaladdress.streetaddress AS nvarchar(max)) 
                      + ', ', '') + isnull(cast(postaladdress.barangay AS nvarchar(max)) + ', ', '') + isnull(cast(postaladdress.municipality AS nvarchar(max)) + ', ', '') 
                      + isnull(cast(postaladdress.city AS nvarchar(max)) + ', ', '') + isnull(cast(postaladdress.province AS nvarchar(max)) + ', ', '') 
                      + isnull(cast(country.name AS nvarchar(max)) + ', ', '') + cast(postaladdress.postalcode AS nvarchar(max)) AS [Address]
FROM         PartyRole JOIN
                      Address ON PartyRole.PartyId = Address.PartyId AND address.enddate IS NULL AND partyrole.roletypeid =
                          (SELECT     id
                            FROM          roletype
                            WHERE      name = 'Contact') JOIN
                      PostalAddress ON Address.AddressId = PostalAddress.AddressId JOIN
                      postaladdresstype ON postaladdresstype.id = postaladdress.postaladdresstypeid AND postaladdresstype.name = 'Business Address' JOIN
                      Country ON PostalAddress.CountryId = Country.Id JOIN
                      Party ON PartyRole.PartyId = Party.Id JOIN
                      Organization ON Party.Id = Organization.PartyId JOIN
                      PartyType ON Party.PartyTypeId = PartyType.Id AND PartyType.Name = 'Organization' JOIN
                      RoleType ON Partyrole.roletypeid = roletype.id JOIN
                      partyrelationship ON partyrelationship.firstpartyroleid = partyrole.id AND partyrelationship.partyreltypeid =
                          (SELECT     id
                            FROM          partyreltype
                            WHERE      name = 'Contact Relationship') AND partyrelationship.secondpartyroleid =
                          (SELECT     id
                            FROM          partyrole
                            WHERE      roletypeid =
                                                       (SELECT     id
                                                         FROM          roletype
                                                         WHERE      name = 'Lending Institution')) AND partyrelationship.enddate IS NULL;
GO
/****** Object:  View [dbo].[CoBorrowersViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[CoBorrowersViewList] as
SELECT DISTINCT dbo.Party.Id AS PartyId, 
    dbo.concatname(dbo.Party.Id, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS Name, 
	ISNULL(CAST(dbo.PostalAddress.StreetAddress AS nvarchar(MAX)) + ', ', '') 
    + ISNULL(CAST(dbo.PostalAddress.Barangay AS nvarchar(MAX)) + ', ', '') 
    + ISNULL(CAST(dbo.PostalAddress.Municipality AS nvarchar(MAX)) + ', ', '') 
    + ISNULL(CAST(dbo.PostalAddress.City AS nvarchar(MAX)) + ', ', '') 
	+ ISNULL(CAST(dbo.PostalAddress.Province AS nvarchar(MAX)) + ', ', '') 
    + ISNULL(CAST(dbo.Country.Name AS nvarchar(MAX)) + ', ', '') 
	+ CAST(dbo.PostalAddress.PostalCode AS nvarchar(MAX)) AS Address
    
	FROM dbo.Party INNER JOIN dbo.Address ON dbo.Address.PartyId = dbo.Party.Id 
		AND (dbo.Address.EndDate IS NULL) 		
	INNER JOIN dbo.AddressType ON dbo.AddressType.Id = dbo.Address.AddressTypeId 
		AND (dbo.AddressType.Name = 'Postal Address')
	INNER JOIN dbo.PostalAddress ON dbo.PostalAddress.AddressId = dbo.Address.AddressId 
	INNER JOIN dbo.PostalAddressType ON dbo.PostalAddressType.Id = dbo.PostalAddress.PostalAddressTypeId 
		AND (dbo.PostalAddressType.Name = 'Home Address') 
	INNER JOIN dbo.Country ON dbo.Country.Id = dbo.PostalAddress.CountryId 
	INNER JOIN dbo.PartyRole ON dbo.PartyRole.PartyId = dbo.Party.Id 
		AND (dbo.PartyRole.EndDate IS NULL)
	INNER JOIN dbo.RoleType ON dbo.RoleType.Id = dbo.PartyRole.RoleTypeId
		AND (dbo.RoleType.Name = 'Co-Borrower') ;
GO
/****** Object:  View [dbo].[ChequeViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ChequeViewList] AS

SELECT	Payment.Id as "HiddenPaymentId",
		payment.paymentreferencenumber as "ChequeNumber",
		organization.organizationname as "Bank",
		payment.transactiondate as "Date Received",
		dbo.concatname((SELECT     PartyId
						FROM         dbo.PartyRole
						WHERE     Id = dbo.Payment.ProcessedToPartyRoleId), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') as "Received From",
		payment.totalamount as "Amount",
		chequestatustype.name as "Status",
		cheque.checkdate as "Cheque Date"

FROM payment JOIN cheque ON payment.id = cheque.paymentid
	JOIN chequestatus ON chequestatus.checkid = cheque.id
	JOIN chequestatustype ON chequestatustype.id = chequestatus.CheckStatusTypeId
	JOIN PartyRole ON partyrole.Id = cheque.BankPartyRoleId
	JOIN Party ON Party.Id = PartyRole.PartyId
	JOIN Organization ON Organization.PartyId = Party.Id

WHERE ChequeStatus.IsActive = 1
;
GO
/****** Object:  View [dbo].[CoOwnerGuarantorViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CoOwnerGuarantorViewList] as

SELECT	RoleType.Name		as	"FinancialAccountRole",
		
			FinancialAccount.Id as "HiddenFinancialAccountID",
		
			dbo.concatname(partyrole.partyid, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') as	"GuarantorName",
		
			max(isnull(cast(Country.CountryTelephoneCode as nvarchar(max)) + ', ','')
				+ isnull(cast(TelecommunicationsNumber.AreaCode as nvarchar(max)) + ', ','')
				+ cast(TelecommunicationsNumber.PhoneNumber as nvarchar(max))) as "CellphoneNumber",
			
			max(isnull(cast(Country.CountryTelephoneCode as nvarchar(max)) + ', ','')
				+ isnull(cast(TelecommunicationsNumber.AreaCode as nvarchar(max)) + ', ','')
				+ cast(TelecommunicationsNumber.PhoneNumber as nvarchar(max))) as "Telephone Number",
		
			isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
				+ isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
				+ isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
				+ isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
				+ isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
				+ isnull(cast(country.name as nvarchar(max)) + ', ', '')
				+ cast(postaladdress.postalcode as nvarchar(max)) as "GuarantorAddress"
			
FROM	Party JOIN PartyRole ON Party.Id = PartyRole.PartyId
			JOIN PartyType ON Party.PartyTypeId = PartyType.Id
				AND PartyType.Name = 'Person'
			JOIN RoleType ON PartyRole.RoleTypeId = RoleType.Id
				AND RoleType.Name in ('Co-Owner', 'Guarantor')
			JOIN Address ON Party.Id = Address.PartyId
			JOIN PostalAddress ON Address.AddressId = PostalAddress.AddressId
			JOIN PostalAddressType ON PostalAddress.PostalAddressTypeId = PostalAddressType.Id
				AND PostalAddressType.Name = 'Home Address' 
			JOIN Country ON PostalAddress.CountryId = Country.Id
			JOIN TelecommunicationsNumber ON Address.AddressId = TelecommunicationsNumber.AddressId
			JOIN TelecommunicationsNumberType ON TelecommunicationsNumber.TypeId = TelecommunicationsNumber.TypeId
				AND TelecommunicationsNumberType.Name in('Personal Mobile Number','Home Phone Number')
			JOIN FinancialAccountRole ON PartyRole.Id = FinancialAccountRole.PartyRoleId
			JOIN FinancialAccount ON FinancialAccountRole.FinancialAccountId = FinancialAccount.Id 			
			
group by RoleType.Name, FinancialAccount.Id, 
			dbo.concatname(partyrole.partyid, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix'), 
			isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
				+ isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
				+ isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
				+ isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
				+ isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
				+ isnull(cast(country.name as nvarchar(max)) + ', ', '')
				+ cast(postaladdress.postalcode as nvarchar(max))
;
GO
/****** Object:  View [dbo].[BankViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[BankViewList] as
SELECT DISTINCT dbo.PartyRole.Id AS PartyRoleID, dbo.Organization.OrganizationName AS [Organization Name], dbo.Bank.Branch, 
dbo.Bank.Acronym,

dbo.BankStatusType.Name AS Status, 
                      ISNULL(CAST(dbo.PostalAddress.StreetAddress AS nvarchar(MAX)) + ', ', '') + 

ISNULL(CAST(dbo.PostalAddress.Barangay AS nvarchar(MAX)) + ', ', '') 
                      + ISNULL(CAST(dbo.PostalAddress.Municipality AS nvarchar(MAX)) + ', ', '') + 

ISNULL(CAST(dbo.PostalAddress.City AS nvarchar(MAX)) + ', ', '') 
                      + ISNULL(CAST(dbo.PostalAddress.Province AS nvarchar(MAX)) + ', ', '') + 

ISNULL(CAST(dbo.PostalAddress.State AS nvarchar(MAX)) + ', ', '') 
                      + ISNULL(CAST(dbo.Country.Name AS nvarchar(MAX)) + ', ', '') + CAST(dbo.PostalAddress.PostalCode AS 

nvarchar(MAX)) AS Address
FROM         dbo.PartyRole INNER JOIN
                      dbo.Organization ON dbo.PartyRole.PartyId = dbo.Organization.PartyId INNER JOIN
                      dbo.Bank ON dbo.PartyRole.Id = dbo.Bank.PartyRoleId INNER JOIN
                      dbo.Address ON dbo.Organization.PartyId = dbo.Address.PartyId INNER JOIN
                      dbo.PostalAddress ON dbo.Address.AddressId = dbo.PostalAddress.AddressId INNER JOIN
                      dbo.BankStatus ON dbo.PartyRole.Id = dbo.BankStatus.PartyRoleId INNER JOIN
                      dbo.BankStatusType ON dbo.BankStatus.BankStatusTypeId = dbo.BankStatusType.Id INNER JOIN
                      dbo.Country ON dbo.PostalAddress.CountryId = dbo.Country.Id INNER JOIN
                      dbo.PartyRoleType ON dbo.PartyRole.RoleTypeId = dbo.PartyRoleType.RoleTypeId INNER JOIN
                      dbo.RoleType ON dbo.PartyRoleType.RoleTypeId = dbo.RoleType.Id
WHERE     (dbo.PartyRole.EndDate IS NULL) AND (dbo.BankStatus.EndDate IS NULL) AND (dbo.Address.EndDate IS NULL)
GO
/****** Object:  View [dbo].[AllowedCustomersView]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: AllowedCustomersView                                   */
/*==============================================================*/
CREATE view [dbo].[AllowedCustomersView] as
SELECT	DISTINCT PartyRole.Id	as "PartyRoleId",
		Party.Id		as	"PartyId",
		dbo.concatname(partyrole.partyid, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix')
											as	"Owner",
		isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "Address"

FROM	Party JOIN PartyRole ON Party.Id = PartyRole.PartyId
		JOIN RoleType ON PartyRole.RoleTypeId = RoleType.Id
		JOIN PartyRelationship ON PartyRole.Id = PartyRelationship.FirstPartyRoleId
		JOIN PartyType ON Party.PartyTypeId = PartyType.Id
		JOIN Address ON Party.Id = Address.PartyId
		JOIN PostalAddress ON Address.AddressId = PostalAddress.AddressId
		JOIN PostalAddressType ON PostalAddress.PostalAddressTypeId = PostalAddressType.Id
		JOIN Country ON PostalAddress.CountryId = Country.Id
		
WHERE	PostalAddressType.Name = 'Home Address' 
		AND PostalAddress.IsPrimary = 1
		AND PartyType.Name = 'Person'
		AND RoleType.Name in ('Contact', 'Employee', 'Employer', 'Spouse', 'Guarantor', 'Co-borrower')
		AND PartyRelationship.EndDate is NULL
		AND Address.EndDate is NULL;
GO
/****** Object:  View [dbo].[AllowedMortgageeViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: AllowedMortgageeViewList                               */
/*==============================================================*/
CREATE view [dbo].[AllowedMortgageeViewList] as

SELECT DISTINCT party.id as "PartyId",
	dbo.concatname(partyrole.partyid, 'Last Name' , 'First Name','Middle Name','Name Suffix') as "Name",
	isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "Address",
	partytype.name as "PartyType"

FROM party 	JOIN address ON address.partyid = party.id
		JOIN postaladdress ON postaladdress.addressid = address.addressid
		JOIN postaladdresstype ON postaladdresstype.id = postaladdress.postaladdresstypeid
		JOIN country ON country.id = postaladdress.countryid
		JOIN partytype ON partytype.id = party.partytypeid	
		JOIN partyrole ON partyrole.partyid = party.id

WHERE postaladdress.isprimary = 1 AND postaladdresstype.name = 'Home Address'


UNION ALL

SELECT DISTINCT party.id as "PartyId",
	organization.organizationname as "Name",
	isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "Address",
	partytype.name as "PartyType"


FROM party 	JOIN organization ON party.id = organization.partyid
		JOIN address ON address.partyid = party.id
		JOIN postaladdress ON postaladdress.addressid = address.addressid
		JOIN postaladdresstype ON postaladdresstype.id = postaladdress.postaladdresstypeid
		JOIN country ON country.id = postaladdress.countryid
		JOIN partytype ON partytype.id = party.partytypeid

WHERE postaladdresstype.name = 'Business Address';
GO
/****** Object:  View [dbo].[AllowedGuarantorsViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: AllowedGuarantorsViewList                              */
/*==============================================================*/
CREATE view [dbo].[AllowedGuarantorsViewList] as
SELECT DISTINCT party.id as "PartyId",

	dbo.concatname(partyrole.partyid, 'Last Name' , 'First Name','Middle Name','Name Suffix') as "Name",		      

    
        isnull(cast(postaladdress.streetaddress as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.barangay as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.municipality as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.city as nvarchar(max)) + ', ','')
         + isnull(cast(postaladdress.province as nvarchar(max)) + ', ', '')
         + isnull(cast(country.name as nvarchar(max)) + ', ', '')
         + cast(postaladdress.postalcode as nvarchar(max)) as "Address"

FROM party 	JOIN address ON address.partyid = party.id
		JOIN postaladdress ON postaladdress.addressid = address.addressid
		JOIN postaladdresstype ON postaladdresstype.id = postaladdress.postaladdresstypeid
		JOIN country ON country.id = postaladdress.countryid
		JOIN partyrole ON partyrole.partyid = party.id
		JOIN RoleType ON PartyRole.RoleTypeId = RoleType.Id
		
WHERE postaladdress.isprimary = 1 
AND postaladdresstype.name = 'Home Address'
AND RoleType.Name in ('Contact', 'Employee', 'Employer', 'Spouse')
;
GO
/****** Object:  View [dbo].[ItemsDetailsView]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: ItemsDetailsView                                       */
/*==============================================================*/
create view [dbo].[ItemsDetailsView] as
--Items

SELECT DisbursementItem.Particular		as	"Particular",
		DisbursementItem.PerItemAmount	as	"Amount"

--Total Amount Disbursed:
		
FROM DisbursementItem
;
GO
/****** Object:  View [dbo].[GuarantorViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[GuarantorViewList] as
SELECT dbo.Party.Id AS PartyId, 
dbo.concatname(dbo.PartyRole.PartyId, 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS Name, 
						  ISNULL(CAST(dbo.PostalAddress.StreetAddress AS nvarchar(MAX)) + ', ', '') + ISNULL(CAST(dbo.PostalAddress.Barangay AS nvarchar(MAX)) + ', ', '') 
						  + ISNULL(CAST(dbo.PostalAddress.Municipality AS nvarchar(MAX)) + ', ', '') + ISNULL(CAST(dbo.PostalAddress.City AS nvarchar(MAX)) + ', ', '') 
						  + ISNULL(CAST(dbo.PostalAddress.Province AS nvarchar(MAX)) + ', ', '') + ISNULL(CAST(dbo.Country.Name AS nvarchar(MAX)) + ', ', '') 
						  + CAST(dbo.PostalAddress.PostalCode AS nvarchar(MAX)) AS Address
	FROM dbo.Party INNER JOIN dbo.Address ON dbo.Address.PartyId = dbo.Party.Id 
		AND (dbo.Address.EndDate IS NULL) 
	INNER JOIN dbo.AddressType ON dbo.AddressType.Id = dbo.Address.AddressTypeId 
		AND (dbo.AddressType.Name = 'Postal Address') 
	INNER JOIN dbo.PostalAddress ON dbo.PostalAddress.AddressId = dbo.Address.AddressId 
		AND (dbo.PostalAddress.IsPrimary = 1) 
	INNER JOIN dbo.PostalAddressType ON dbo.PostalAddressType.Id = dbo.PostalAddress.PostalAddressTypeId 
		AND (dbo.PostalAddressType.Name = 'Home Address') 
	INNER JOIN dbo.Country ON dbo.Country.Id = dbo.PostalAddress.CountryId 
	INNER JOIN dbo.PartyRole ON dbo.PartyRole.PartyId = dbo.Party.Id 
		AND (dbo.PartyRole.EndDate IS NULL)
	INNER JOIN dbo.RoleType ON dbo.RoleType.Id = dbo.PartyRole.RoleTypeId
		 AND (dbo.RoleType.Name = 'Guarantor')
GO
/****** Object:  View [dbo].[LoanViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================*/
/* View: LoanViewList                                           */
/*==============================================================*/
CREATE VIEW [dbo].[LoanViewList]
AS
SELECT DISTINCT LoanAccount.FinancialAccountId AS [LoanId], 
					  LoanAccount.LoanReleaseDate AS [LoanReleaseDate], 
					  dbo.concatname(partyrole.partyid, 'Last Name', 
                      'First Name', 'Middle Name', 'Name Suffix') AS [Name], isnull(cast(postaladdress.streetaddress AS nvarchar(max)) + ', ', '') 
                      + isnull(cast(postaladdress.barangay AS nvarchar(max)) + ', ', '') + isnull(cast(postaladdress.municipality AS nvarchar(max)) + ', ', '') 
                      + isnull(cast(postaladdress.city AS nvarchar(max)) + ', ', '') + isnull(cast(postaladdress.province AS nvarchar(max)) + ', ', '') 
                      + isnull(cast(country.name AS nvarchar(max)) + ', ', '') + cast(postaladdress.postalcode AS nvarchar(max)) AS [Address], 
                      FinancialProduct.Name AS [LoanProduct], 
                      LoanAccountStatusType.Name AS [Status],
                      AgreementItem.InterestComputationMode AS [InterestComputationMode]
FROM         LoanAccount JOIN
                      FinancialAccountProduct ON LoanAccount.FinancialAccountId = FinancialAccountProduct.FinancialAccountId JOIN
                      FinancialAccount ON LoanAccount.FinancialAccountId = FinancialAccount.Id JOIN
                      FinancialAccountType ON FinancialAccount.FinancialAccountTypeId = FinancialAccountType.Id JOIN
                      FinancialProduct ON FinancialAccountProduct.FinancialProductId = FinancialProduct.Id JOIN
                      ProductStatus ON FinancialProduct.Id = ProductStatus.FinancialProductId JOIN
                      ProductStatusType ON ProductStatus.StatusTypeId = ProductStatusType.Id JOIN
                      FinancialAccountRole ON FinancialAccount.Id = FinancialAccountRole.FinancialAccountId JOIN
                      PartyRole ON FinancialAccountRole.PartyRoleId = PartyRole.Id JOIN
                      RoleType ON PartyRole.RoleTypeId = RoleType.Id JOIN
                      Address ON PartyRole.PartyId = Address.PartyId JOIN
                      PostalAddress ON Address.AddressId = PostalAddress.AddressId JOIN
                      Country ON PostalAddress.CountryId = Country.Id JOIN
                      PostalAddressType on PostalAddress.PostalAddressTypeId = PostalAddressType.Id join
                      LoanAccountStatus ON LoanAccount.FinancialAccountId = LoanAccountStatus.FinancialAccountId JOIN
                      LoanAccountStatusType ON LoanAccountStatus.StatusTypeId = LoanAccountStatusType.Id JOIN
                      Agreement ON Agreement.Id = FinancialAccount.AgreementId JOIN
					  AgreementItem ON AgreementItem.AgreementId = Agreement.Id 
WHERE		RoleType.Name = 'Owner' AND FinancialAccountType.Name = 'Loan Account' 
			AND Address.EndDate IS NULL AND PostalAddressType.Name ='Home Address' 
			AND PostalAddress.IsPrimary = 1 AND Agreement.EndDate IS NULL
			AND LoanAccountStatus.IsActive = 1 AND AgreementItem.IsActive = 1
UNION ALL
SELECT DISTINCT LoanAccount.FinancialAccountId AS [LoanId], LoanAccount.LoanReleaseDate AS [LoanReleaseDate], Organization.OrganizationName AS [Name], 
                      isnull(cast(postaladdress.streetaddress AS nvarchar(max)) + ', ', '') + isnull(cast(postaladdress.barangay AS nvarchar(max)) + ', ', '') 
                      + isnull(cast(postaladdress.municipality AS nvarchar(max)) + ', ', '') + isnull(cast(postaladdress.city AS nvarchar(max)) + ', ', '') 
                      + isnull(cast(postaladdress.province AS nvarchar(max)) + ', ', '') + isnull(cast(country.name AS nvarchar(max)) + ', ', '') 
                      + cast(postaladdress.postalcode AS nvarchar(max)) AS [Address], 
                      FinancialProduct.Name AS [LoanProduct], 
                      LoanAccountStatusType.Name AS [Status],
                      AgreementItem.InterestComputationMode AS [InterestComputationMode]
FROM         LoanAccount JOIN
                      FinancialAccountProduct ON LoanAccount.FinancialAccountId = FinancialAccountProduct.FinancialAccountId JOIN
                      FinancialAccount ON LoanAccount.FinancialAccountId = FinancialAccount.Id JOIN
                      FinancialAccountType ON FinancialAccount.FinancialAccountTypeId = FinancialAccountType.Id JOIN
                      FinancialProduct ON FinancialAccountProduct.FinancialProductId = FinancialProduct.Id JOIN
                      ProductStatus ON FinancialProduct.Id = ProductStatus.FinancialProductId JOIN
                      ProductStatusType ON ProductStatus.StatusTypeId = ProductStatusType.Id JOIN
                      FinancialAccountRole ON FinancialAccount.Id = FinancialAccountRole.FinancialAccountId JOIN
                      PartyRole ON FinancialAccountRole.PartyRoleId = PartyRole.Id JOIN
                      RoleType ON PartyRole.RoleTypeId = RoleType.Id JOIN
                      Organization ON PartyRole.PartyId = Organization.PartyId JOIN
                      Address ON PartyRole.PartyId = Address.PartyId JOIN
                      PostalAddress ON Address.AddressId = PostalAddress.AddressId JOIN
                      PostalAddressType on PostalAddress.PostalAddressTypeId = PostalAddressType.Id join
                      Country ON PostalAddress.CountryId = Country.Id JOIN
                      LoanAccountStatus ON LoanAccount.FinancialAccountId = LoanAccountStatus.FinancialAccountId JOIN
                      LoanAccountStatusType ON LoanAccountStatus.StatusTypeId = LoanAccountStatusType.Id JOIN
                      Agreement ON Agreement.Id = FinancialAccount.AgreementId JOIN
					  AgreementItem ON AgreementItem.AgreementId = Agreement.Id 
WHERE     RoleType.Name = 'Owner' AND FinancialAccountType.Name = 'Loan Account' 
		  AND Address.EndDate IS NULL AND PostalAddressType.Name ='Business Address' 
		  AND PostalAddress.IsPrimary = 1 AND Agreement.EndDate IS NULL
		  AND LoanAccountStatus.IsActive = 1 AND AgreementItem.IsActive = 1;
GO
/****** Object:  View [dbo].[LoanDisbursementViewList]    Script Date: 02/22/2012 10:28:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[LoanDisbursementViewList]
AS

SELECT     dbo.Payment.TransactionDate AS DateDisbursed, dbo.Payment.Id AS PaymentId, 

dbo.concatname             ((SELECT     PartyId
                              FROM         dbo.PartyRole AS PartyRole_2
                              WHERE     (Id = dbo.Payment.ProcessedToPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS DisbursedTo, 
dbo.LoanDisbursementVcr.AgreementId AS LoanAgreementID, 
dbo.PaymentMethodType.Name AS PaymentMethod, 
dbo.Payment.PaymentReferenceNumber AS CheckNumber, 
dbo.Payment.TotalAmount AS AmountDisbursed, 
dbo.concatname             ((SELECT     PartyId
                              FROM         dbo.PartyRole
                              WHERE     (Id = dbo.Payment.ProcessedByPartyRoleId)), 'Last Name', 'First Name', 'Middle Name', 'Name Suffix') AS DisbursedBy, 
dbo.Disbursement.DisbursementTypeId
FROM         dbo.Payment INNER JOIN
                      dbo.Disbursement ON dbo.Payment.Id = dbo.Disbursement.PaymentId INNER JOIN
                      dbo.DisbursementType ON dbo.DisbursementType.Id = dbo.Disbursement.DisbursementTypeId INNER JOIN
                      dbo.PaymentMethodType ON dbo.Payment.PaymentMethodTypeId = dbo.PaymentMethodType.Id INNER JOIN
                      dbo.PartyRole ON dbo.Payment.ProcessedByPartyRoleId = dbo.PartyRole.Id INNER JOIN
                      dbo.PaymentApplication ON dbo.Payment.Id = dbo.PaymentApplication.PaymentId INNER JOIN
                      dbo.LoanDisbursementVcr ON dbo.PaymentApplication.LoanDisbursementVoucherId = dbo.LoanDisbursementVcr.Id
WHERE     (dbo.DisbursementType.Name = 'Loan Disbursement');
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Payment"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 256
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PaymentMethodType"
            Begin Extent = 
               Top = 6
               Left = 294
               Bottom = 95
               Right = 454
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PartyRole"
            Begin Extent = 
               Top = 96
               Left = 294
               Bottom = 215
               Right = 454
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PaymentApplication"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 245
               Right = 264
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LoanDisbursementVcr"
            Begin Extent = 
               Top = 216
               Left = 302
               Bottom = 335
               Right = 462
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Disbursement"
            Begin Extent = 
               Top = 6
               Left = 492
               Bottom = 110
               Right = 680
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DisbursementType"
            Begin Extent = 
               Top = 6
               Left = 718
               Bottom = 95
               Right = 878
     ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LoanDisbursementViewList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'       End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LoanDisbursementViewList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LoanDisbursementViewList'
GO
/****** Object:  ForeignKey [FK_UNIT_OF__REFERENCE_UNIT_OF_2]    Script Date: 02/22/2012 10:28:09 ******/
ALTER TABLE [dbo].[UnitOfMeasureType]  WITH CHECK ADD  CONSTRAINT [FK_UNIT_OF__REFERENCE_UNIT_OF_2] FOREIGN KEY([ParentUomTypeId])
REFERENCES [dbo].[UnitOfMeasureType] ([Id])
GO
ALTER TABLE [dbo].[UnitOfMeasureType] CHECK CONSTRAINT [FK_UNIT_OF__REFERENCE_UNIT_OF_2]
GO
/****** Object:  ForeignKey [FK_ROLETYPE_REFERENCE_ROLETYPE]    Script Date: 02/22/2012 10:28:09 ******/
ALTER TABLE [dbo].[RoleType]  WITH CHECK ADD  CONSTRAINT [FK_ROLETYPE_REFERENCE_ROLETYPE] FOREIGN KEY([ParentRoleTypeId])
REFERENCES [dbo].[RoleType] ([Id])
GO
ALTER TABLE [dbo].[RoleType] CHECK CONSTRAINT [FK_ROLETYPE_REFERENCE_ROLETYPE]
GO
/****** Object:  ForeignKey [FK_FINLACCT_REFERENCE_FINLACCT]    Script Date: 02/22/2012 10:28:09 ******/
ALTER TABLE [dbo].[FinlAcctTransType]  WITH CHECK ADD  CONSTRAINT [FK_FINLACCT_REFERENCE_FINLACCT] FOREIGN KEY([ParentFinancialAcctTransTypeId])
REFERENCES [dbo].[FinlAcctTransType] ([Id])
GO
ALTER TABLE [dbo].[FinlAcctTransType] CHECK CONSTRAINT [FK_FINLACCT_REFERENCE_FINLACCT]
GO
/****** Object:  ForeignKey [FK_PARTYROL_REFERENCE_ROLETYPE]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[PartyRoleType]  WITH CHECK ADD  CONSTRAINT [FK_PARTYROL_REFERENCE_ROLETYPE] FOREIGN KEY([RoleTypeId])
REFERENCES [dbo].[RoleType] ([Id])
GO
ALTER TABLE [dbo].[PartyRoleType] CHECK CONSTRAINT [FK_PARTYROL_REFERENCE_ROLETYPE]
GO
/****** Object:  ForeignKey [FK_PARTY_REFERENCE_PARTYTYP]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[Party]  WITH CHECK ADD  CONSTRAINT [FK_PARTY_REFERENCE_PARTYTYP] FOREIGN KEY([PartyTypeId])
REFERENCES [dbo].[PartyType] ([Id])
GO
ALTER TABLE [dbo].[Party] CHECK CONSTRAINT [FK_PARTY_REFERENCE_PARTYTYP]
GO
/****** Object:  ForeignKey [FK_ForeignExchangeAmount_Currency]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ForExDetail]  WITH CHECK ADD  CONSTRAINT [FK_ForeignExchangeAmount_Currency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[ForExDetail] CHECK CONSTRAINT [FK_ForeignExchangeAmount_Currency]
GO
/****** Object:  ForeignKey [FK_ForeignExchangeAmount_PaymentMethodType]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ForExDetail]  WITH CHECK ADD  CONSTRAINT [FK_ForeignExchangeAmount_PaymentMethodType] FOREIGN KEY([PaymentMethodTypeId])
REFERENCES [dbo].[PaymentMethodType] ([Id])
GO
ALTER TABLE [dbo].[ForExDetail] CHECK CONSTRAINT [FK_ForeignExchangeAmount_PaymentMethodType]
GO
/****** Object:  ForeignKey [FK_ForExDetail_ForExDetail]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ForExDetail]  WITH CHECK ADD  CONSTRAINT [FK_ForExDetail_ForExDetail] FOREIGN KEY([ParentForExDetailId])
REFERENCES [dbo].[ForExDetail] ([Id])
GO
ALTER TABLE [dbo].[ForExDetail] CHECK CONSTRAINT [FK_ForExDetail_ForExDetail]
GO
/****** Object:  ForeignKey [FK_APPLICAT_REFERENCE_APPLICAT2]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[Application]  WITH CHECK ADD  CONSTRAINT [FK_APPLICAT_REFERENCE_APPLICAT2] FOREIGN KEY([ApplicationType])
REFERENCES [dbo].[ApplicationType] ([Id])
GO
ALTER TABLE [dbo].[Application] CHECK CONSTRAINT [FK_APPLICAT_REFERENCE_APPLICAT2]
GO
/****** Object:  ForeignKey [FK_ClassificationType_DistrictType]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ClassificationType]  WITH CHECK ADD  CONSTRAINT [FK_ClassificationType_DistrictType] FOREIGN KEY([DistrictTypeId])
REFERENCES [dbo].[DistrictType] ([Id])
GO
ALTER TABLE [dbo].[ClassificationType] CHECK CONSTRAINT [FK_ClassificationType_DistrictType]
GO
/****** Object:  ForeignKey [FK_ExchangeRate_Currency]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ExchangeRate]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeRate_Currency] FOREIGN KEY([CurrencyFromId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[ExchangeRate] CHECK CONSTRAINT [FK_ExchangeRate_Currency]
GO
/****** Object:  ForeignKey [FK_ExchangeRate_Currency1]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ExchangeRate]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeRate_Currency1] FOREIGN KEY([CurrencyToId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[ExchangeRate] CHECK CONSTRAINT [FK_ExchangeRate_Currency1]
GO
/****** Object:  ForeignKey [FK_ExchangeRate_ExchangeRateType]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ExchangeRate]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeRate_ExchangeRateType] FOREIGN KEY([ExchangeRateTypeId])
REFERENCES [dbo].[ExchangeRateType] ([Id])
GO
ALTER TABLE [dbo].[ExchangeRate] CHECK CONSTRAINT [FK_ExchangeRate_ExchangeRateType]
GO
/****** Object:  ForeignKey [FK_DISBURSE_REFERENCE_DISBURSE2]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[DisbursementVcrStatTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_DISBURSE_REFERENCE_DISBURSE2] FOREIGN KEY([FromStatusTypeId])
REFERENCES [dbo].[DisbursementVcrStatusType] ([Id])
GO
ALTER TABLE [dbo].[DisbursementVcrStatTypeAssoc] CHECK CONSTRAINT [FK_DISBURSE_REFERENCE_DISBURSE2]
GO
/****** Object:  ForeignKey [FK_DISBURSE_REFERENCE_DISBURSE3]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[DisbursementVcrStatTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_DISBURSE_REFERENCE_DISBURSE3] FOREIGN KEY([ToStatusTypeId])
REFERENCES [dbo].[DisbursementVcrStatusType] ([Id])
GO
ALTER TABLE [dbo].[DisbursementVcrStatTypeAssoc] CHECK CONSTRAINT [FK_DISBURSE_REFERENCE_DISBURSE3]
GO
/****** Object:  ForeignKey [FK_RECEIPT__REFERENCE_RECEIPT_3]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ReceiptStatusTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_RECEIPT__REFERENCE_RECEIPT_3] FOREIGN KEY([FromStatusTypeId])
REFERENCES [dbo].[ReceiptStatusType] ([Id])
GO
ALTER TABLE [dbo].[ReceiptStatusTypeAssoc] CHECK CONSTRAINT [FK_RECEIPT__REFERENCE_RECEIPT_3]
GO
/****** Object:  ForeignKey [FK_RECEIPTS_REFERENCE_RECEIPTS]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ReceiptStatusTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_RECEIPTS_REFERENCE_RECEIPTS] FOREIGN KEY([ToStatusTypeId])
REFERENCES [dbo].[ReceiptStatusType] ([Id])
GO
ALTER TABLE [dbo].[ReceiptStatusTypeAssoc] CHECK CONSTRAINT [FK_RECEIPTS_REFERENCE_RECEIPTS]
GO
/****** Object:  ForeignKey [FK_PETTY_CA_REFERENCE_PETTY_CA5]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[PettyCashLoanAppStatusTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PETTY_CA_REFERENCE_PETTY_CA5] FOREIGN KEY([FromStatusTypeId])
REFERENCES [dbo].[PettyCashLoanAppStatusType] ([Id])
GO
ALTER TABLE [dbo].[PettyCashLoanAppStatusTypeAssoc] CHECK CONSTRAINT [FK_PETTY_CA_REFERENCE_PETTY_CA5]
GO
/****** Object:  ForeignKey [FK_PETTYCAS_REFERENCE_PETTYCAS]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[PettyCashLoanAppStatusTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PETTYCAS_REFERENCE_PETTYCAS] FOREIGN KEY([ToStatusTypeId])
REFERENCES [dbo].[PettyCashLoanAppStatusType] ([Id])
GO
ALTER TABLE [dbo].[PettyCashLoanAppStatusTypeAssoc] CHECK CONSTRAINT [FK_PETTYCAS_REFERENCE_PETTYCAS]
GO
/****** Object:  ForeignKey [FK_LOAN_ACC_REFERENCE_LOAN_ACC4]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[LoanAccountStatusTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_LOAN_ACC_REFERENCE_LOAN_ACC4] FOREIGN KEY([FromStatusTypeId])
REFERENCES [dbo].[LoanAccountStatusType] ([Id])
GO
ALTER TABLE [dbo].[LoanAccountStatusTypeAssoc] CHECK CONSTRAINT [FK_LOAN_ACC_REFERENCE_LOAN_ACC4]
GO
/****** Object:  ForeignKey [FK_LOANACCO_REFERENCE_LOANACCO]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[LoanAccountStatusTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_LOANACCO_REFERENCE_LOANACCO] FOREIGN KEY([ToStatusTypeId])
REFERENCES [dbo].[LoanAccountStatusType] ([Id])
GO
ALTER TABLE [dbo].[LoanAccountStatusTypeAssoc] CHECK CONSTRAINT [FK_LOANACCO_REFERENCE_LOANACCO]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_FINANCIA3]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductCategoryClassification]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_FINANCIA3] FOREIGN KEY([FinancialProductId])
REFERENCES [dbo].[FinancialProduct] ([Id])
GO
ALTER TABLE [dbo].[ProductCategoryClassification] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_FINANCIA3]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_PRODUCT_6]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductCategoryClassification]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_6] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategory] ([Id])
GO
ALTER TABLE [dbo].[ProductCategoryClassification] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_6]
GO
/****** Object:  ForeignKey [FK_UNITOFME_REFERENCE_UNITOFME]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[UnitOfMeasure]  WITH CHECK ADD  CONSTRAINT [FK_UNITOFME_REFERENCE_UNITOFME] FOREIGN KEY([UomTypeId])
REFERENCES [dbo].[UnitOfMeasureType] ([Id])
GO
ALTER TABLE [dbo].[UnitOfMeasure] CHECK CONSTRAINT [FK_UNITOFME_REFERENCE_UNITOFME]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_PRODUCT_2]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductCategoryRollup]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_2] FOREIGN KEY([ChildProductCategoryId])
REFERENCES [dbo].[ProductCategory] ([Id])
GO
ALTER TABLE [dbo].[ProductCategoryRollup] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_2]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_PRODUCT_3]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductCategoryRollup]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_3] FOREIGN KEY([ParentProductCategoryId])
REFERENCES [dbo].[ProductCategory] ([Id])
GO
ALTER TABLE [dbo].[ProductCategoryRollup] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_3]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_FINANCIA4]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductStatus]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_FINANCIA4] FOREIGN KEY([FinancialProductId])
REFERENCES [dbo].[FinancialProduct] ([Id])
GO
ALTER TABLE [dbo].[ProductStatus] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_FINANCIA4]
GO
/****** Object:  ForeignKey [FK_PRODUCTS_REFERENCE_PRODUCTS]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductStatus]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCTS_REFERENCE_PRODUCTS] FOREIGN KEY([StatusTypeId])
REFERENCES [dbo].[ProductStatusType] ([Id])
GO
ALTER TABLE [dbo].[ProductStatus] CHECK CONSTRAINT [FK_PRODUCTS_REFERENCE_PRODUCTS]
GO
/****** Object:  ForeignKey [FK_PRODUCTR_REFERENCE_FINANCIA]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductRequiredDocument]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCTR_REFERENCE_FINANCIA] FOREIGN KEY([FinancialProductId])
REFERENCES [dbo].[FinancialProduct] ([Id])
GO
ALTER TABLE [dbo].[ProductRequiredDocument] CHECK CONSTRAINT [FK_PRODUCTR_REFERENCE_FINANCIA]
GO
/****** Object:  ForeignKey [FK_PRODUCTR_REFERENCE_REQUIRED]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductRequiredDocument]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCTR_REFERENCE_REQUIRED] FOREIGN KEY([RequiredDocumentTypeId])
REFERENCES [dbo].[RequiredDocumentType] ([Id])
GO
ALTER TABLE [dbo].[ProductRequiredDocument] CHECK CONSTRAINT [FK_PRODUCTR_REFERENCE_REQUIRED]
GO
/****** Object:  ForeignKey [FK_RECEIPT__REFERENCE_RECEIPT_2]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ReceiptStatus]  WITH CHECK ADD  CONSTRAINT [FK_RECEIPT__REFERENCE_RECEIPT_2] FOREIGN KEY([ReceiptStatusTypeId])
REFERENCES [dbo].[ReceiptStatusType] ([Id])
GO
ALTER TABLE [dbo].[ReceiptStatus] CHECK CONSTRAINT [FK_RECEIPT__REFERENCE_RECEIPT_2]
GO
/****** Object:  ForeignKey [FK_ReceiptStatus_Receipt]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ReceiptStatus]  WITH CHECK ADD  CONSTRAINT [FK_ReceiptStatus_Receipt] FOREIGN KEY([ReceiptId])
REFERENCES [dbo].[Receipt] ([Id])
GO
ALTER TABLE [dbo].[ReceiptStatus] CHECK CONSTRAINT [FK_ReceiptStatus_Receipt]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_PRODUCT_5]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductFeature]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_5] FOREIGN KEY([ProductFeatCatId])
REFERENCES [dbo].[ProductFeatureCategory] ([Id])
GO
ALTER TABLE [dbo].[ProductFeature] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_5]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_PRODUCT_7]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductCatFeatApplicability]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_7] FOREIGN KEY([ProductFeatureId])
REFERENCES [dbo].[ProductFeature] ([Id])
GO
ALTER TABLE [dbo].[ProductCatFeatApplicability] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_7]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_PRODUCT_8]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductCatFeatApplicability]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_8] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategory] ([Id])
GO
ALTER TABLE [dbo].[ProductCatFeatApplicability] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_8]
GO
/****** Object:  ForeignKey [FK_TIME_UNI_REFERENCE_UNIT_OF_2]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[TimeUnitConversion]  WITH CHECK ADD  CONSTRAINT [FK_TIME_UNI_REFERENCE_UNIT_OF_2] FOREIGN KEY([SourceUomId])
REFERENCES [dbo].[UnitOfMeasure] ([Id])
GO
ALTER TABLE [dbo].[TimeUnitConversion] CHECK CONSTRAINT [FK_TIME_UNI_REFERENCE_UNIT_OF_2]
GO
/****** Object:  ForeignKey [FK_TIMEUNIT_REFERENCE_UNITOFME]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[TimeUnitConversion]  WITH CHECK ADD  CONSTRAINT [FK_TIMEUNIT_REFERENCE_UNITOFME] FOREIGN KEY([TargetUomId])
REFERENCES [dbo].[UnitOfMeasure] ([Id])
GO
ALTER TABLE [dbo].[TimeUnitConversion] CHECK CONSTRAINT [FK_TIMEUNIT_REFERENCE_UNITOFME]
GO
/****** Object:  ForeignKey [FK_SYSTEMSE_REFERENCE_SYSTEMSE]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[SystemSetting]  WITH CHECK ADD  CONSTRAINT [FK_SYSTEMSE_REFERENCE_SYSTEMSE] FOREIGN KEY([SystemSettingTypeId])
REFERENCES [dbo].[SystemSettingType] ([Id])
GO
ALTER TABLE [dbo].[SystemSetting] CHECK CONSTRAINT [FK_SYSTEMSE_REFERENCE_SYSTEMSE]
GO
/****** Object:  ForeignKey [FK_SYSTEMSE_REFERENCE_UNITOFME]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[SystemSetting]  WITH CHECK ADD  CONSTRAINT [FK_SYSTEMSE_REFERENCE_UNITOFME] FOREIGN KEY([UomId])
REFERENCES [dbo].[UnitOfMeasure] ([Id])
GO
ALTER TABLE [dbo].[SystemSetting] CHECK CONSTRAINT [FK_SYSTEMSE_REFERENCE_UNITOFME]
GO
/****** Object:  ForeignKey [FK_PETTYCAS_REFERENCE_APPLICAT]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[PettyCashLoanApplication]  WITH CHECK ADD  CONSTRAINT [FK_PETTYCAS_REFERENCE_APPLICAT] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[PettyCashLoanApplication] CHECK CONSTRAINT [FK_PETTYCAS_REFERENCE_APPLICAT]
GO
/****** Object:  ForeignKey [FK_PETTYCAS_REFERENCE_UNITOFME]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[PettyCashLoanApplication]  WITH CHECK ADD  CONSTRAINT [FK_PETTYCAS_REFERENCE_UNITOFME] FOREIGN KEY([UomId])
REFERENCES [dbo].[UnitOfMeasure] ([Id])
GO
ALTER TABLE [dbo].[PettyCashLoanApplication] CHECK CONSTRAINT [FK_PETTYCAS_REFERENCE_UNITOFME]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_FINANCIA2]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductFeatureApplicability]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_FINANCIA2] FOREIGN KEY([FinancialProductId])
REFERENCES [dbo].[FinancialProduct] ([Id])
GO
ALTER TABLE [dbo].[ProductFeatureApplicability] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_FINANCIA2]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_PRODUCT_4]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[ProductFeatureApplicability]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_4] FOREIGN KEY([ProductFeatureId])
REFERENCES [dbo].[ProductFeature] ([Id])
GO
ALTER TABLE [dbo].[ProductFeatureApplicability] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_4]
GO
/****** Object:  ForeignKey [FK_Denomination_ForeignExchangeAmount]    Script Date: 02/22/2012 10:28:10 ******/
ALTER TABLE [dbo].[Denomination]  WITH CHECK ADD  CONSTRAINT [FK_Denomination_ForeignExchangeAmount] FOREIGN KEY([ForExDetailId])
REFERENCES [dbo].[ForExDetail] ([Id])
GO
ALTER TABLE [dbo].[Denomination] CHECK CONSTRAINT [FK_Denomination_ForeignExchangeAmount]
GO
/****** Object:  ForeignKey [FK_AGREEMEN_REFERENCE_AGREEMEN3]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Agreement]  WITH CHECK ADD  CONSTRAINT [FK_AGREEMEN_REFERENCE_AGREEMEN3] FOREIGN KEY([ParentAgreementId])
REFERENCES [dbo].[Agreement] ([Id])
GO
ALTER TABLE [dbo].[Agreement] CHECK CONSTRAINT [FK_AGREEMEN_REFERENCE_AGREEMEN3]
GO
/****** Object:  ForeignKey [FK_AGREEMEN_REFERENCE_AGREEMEN4]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Agreement]  WITH CHECK ADD  CONSTRAINT [FK_AGREEMEN_REFERENCE_AGREEMEN4] FOREIGN KEY([AgreementTypeId])
REFERENCES [dbo].[AgreementType] ([Id])
GO
ALTER TABLE [dbo].[Agreement] CHECK CONSTRAINT [FK_AGREEMEN_REFERENCE_AGREEMEN4]
GO
/****** Object:  ForeignKey [FK_AGREEMEN_REFERENCE_APPLICAT]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Agreement]  WITH CHECK ADD  CONSTRAINT [FK_AGREEMEN_REFERENCE_APPLICAT] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[Agreement] CHECK CONSTRAINT [FK_AGREEMEN_REFERENCE_APPLICAT]
GO
/****** Object:  ForeignKey [FK_LOAN_APP_REFERENCE_UNIT_OF_2]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanApplication]  WITH CHECK ADD  CONSTRAINT [FK_LOAN_APP_REFERENCE_UNIT_OF_2] FOREIGN KEY([PaymentModeUomId])
REFERENCES [dbo].[UnitOfMeasure] ([Id])
GO
ALTER TABLE [dbo].[LoanApplication] CHECK CONSTRAINT [FK_LOAN_APP_REFERENCE_UNIT_OF_2]
GO
/****** Object:  ForeignKey [FK_LOANAPPL_REFERENCE_APPLICAT]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanApplication]  WITH CHECK ADD  CONSTRAINT [FK_LOANAPPL_REFERENCE_APPLICAT] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[LoanApplication] CHECK CONSTRAINT [FK_LOANAPPL_REFERENCE_APPLICAT]
GO
/****** Object:  ForeignKey [FK_LOANAPPL_REFERENCE_UNITOFME]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanApplication]  WITH CHECK ADD  CONSTRAINT [FK_LOANAPPL_REFERENCE_UNITOFME] FOREIGN KEY([LoanTermUomId])
REFERENCES [dbo].[UnitOfMeasure] ([Id])
GO
ALTER TABLE [dbo].[LoanApplication] CHECK CONSTRAINT [FK_LOANAPPL_REFERENCE_UNITOFME]
GO
/****** Object:  ForeignKey [FK_ORGANIZA_REFERENCE_ORGANIZA]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Organization]  WITH CHECK ADD  CONSTRAINT [FK_ORGANIZA_REFERENCE_ORGANIZA] FOREIGN KEY([OrganizationTypeId])
REFERENCES [dbo].[OrganizationType] ([Id])
GO
ALTER TABLE [dbo].[Organization] CHECK CONSTRAINT [FK_ORGANIZA_REFERENCE_ORGANIZA]
GO
/****** Object:  ForeignKey [FK_ORGANIZA_REFERENCE_PARTY]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Organization]  WITH CHECK ADD  CONSTRAINT [FK_ORGANIZA_REFERENCE_PARTY] FOREIGN KEY([PartyId])
REFERENCES [dbo].[Party] ([Id])
GO
ALTER TABLE [dbo].[Organization] CHECK CONSTRAINT [FK_ORGANIZA_REFERENCE_PARTY]
GO
/****** Object:  ForeignKey [FK_PARTYROL_REFERENCE_PARTY]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PartyRole]  WITH CHECK ADD  CONSTRAINT [FK_PARTYROL_REFERENCE_PARTY] FOREIGN KEY([PartyId])
REFERENCES [dbo].[Party] ([Id])
GO
ALTER TABLE [dbo].[PartyRole] CHECK CONSTRAINT [FK_PARTYROL_REFERENCE_PARTY]
GO
/****** Object:  ForeignKey [FK_PARTYROL_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PartyRole]  WITH CHECK ADD  CONSTRAINT [FK_PARTYROL_REFERENCE_PARTYROL] FOREIGN KEY([RoleTypeId])
REFERENCES [dbo].[PartyRoleType] ([RoleTypeId])
GO
ALTER TABLE [dbo].[PartyRole] CHECK CONSTRAINT [FK_PARTYROL_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_PERSON_REFERENCE_EDUCATTA]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [FK_PERSON_REFERENCE_EDUCATTA] FOREIGN KEY([EducAttainmentTypeId])
REFERENCES [dbo].[EducAttainmentType] ([Id])
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [FK_PERSON_REFERENCE_EDUCATTA]
GO
/****** Object:  ForeignKey [FK_PERSON_REFERENCE_GENDERTY]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [FK_PERSON_REFERENCE_GENDERTY] FOREIGN KEY([GenderTypeId])
REFERENCES [dbo].[GenderType] ([Id])
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [FK_PERSON_REFERENCE_GENDERTY]
GO
/****** Object:  ForeignKey [FK_PERSON_REFERENCE_NATIONAL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [FK_PERSON_REFERENCE_NATIONAL] FOREIGN KEY([NationalityTypeId])
REFERENCES [dbo].[NationalityType] ([Id])
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [FK_PERSON_REFERENCE_NATIONAL]
GO
/****** Object:  ForeignKey [FK_PERSON_REFERENCE_PARTY]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [FK_PERSON_REFERENCE_PARTY] FOREIGN KEY([PartyId])
REFERENCES [dbo].[Party] ([Id])
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [FK_PERSON_REFERENCE_PARTY]
GO
/****** Object:  ForeignKey [FK_PAYMENT_REFERENCE_PARTY_RO2]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENT_REFERENCE_PARTY_RO2] FOREIGN KEY([ProcessedByPartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_PAYMENT_REFERENCE_PARTY_RO2]
GO
/****** Object:  ForeignKey [FK_PAYMENT_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENT_REFERENCE_PARTYROL] FOREIGN KEY([ProcessedToPartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_PAYMENT_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_PAYMENT_REFERENCE_PAYMENT]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENT_REFERENCE_PAYMENT] FOREIGN KEY([ParentPaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_PAYMENT_REFERENCE_PAYMENT]
GO
/****** Object:  ForeignKey [FK_PAYMENT_REFERENCE_PAYMENTM]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENT_REFERENCE_PAYMENTM] FOREIGN KEY([PaymentMethodTypeId])
REFERENCES [dbo].[PaymentMethodType] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_PAYMENT_REFERENCE_PAYMENTM]
GO
/****** Object:  ForeignKey [FK_PAYMENT_REFERENCE_PAYMENTT]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENT_REFERENCE_PAYMENTT] FOREIGN KEY([PaymentTypeId])
REFERENCES [dbo].[PaymentType] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_PAYMENT_REFERENCE_PAYMENTT]
GO
/****** Object:  ForeignKey [FK_Payment_SpecificPaymentType]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_SpecificPaymentType] FOREIGN KEY([SpecificPaymentTypeId])
REFERENCES [dbo].[SpecificPaymentType] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_SpecificPaymentType]
GO
/****** Object:  ForeignKey [FK_LOANTERM_REFERENCE_PRODUCTF]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanTerm]  WITH CHECK ADD  CONSTRAINT [FK_LOANTERM_REFERENCE_PRODUCTF] FOREIGN KEY([ProductFeatApplicabilityId])
REFERENCES [dbo].[ProductFeatureApplicability] ([Id])
GO
ALTER TABLE [dbo].[LoanTerm] CHECK CONSTRAINT [FK_LOANTERM_REFERENCE_PRODUCTF]
GO
/****** Object:  ForeignKey [FK_LOANTERM_REFERENCE_UNITOFME]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanTerm]  WITH CHECK ADD  CONSTRAINT [FK_LOANTERM_REFERENCE_UNITOFME] FOREIGN KEY([UomId])
REFERENCES [dbo].[UnitOfMeasure] ([Id])
GO
ALTER TABLE [dbo].[LoanTerm] CHECK CONSTRAINT [FK_LOANTERM_REFERENCE_UNITOFME]
GO
/****** Object:  ForeignKey [FK_PARTY_RE_REFERENCE_PARTY_RO2]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PartyRelationship]  WITH CHECK ADD  CONSTRAINT [FK_PARTY_RE_REFERENCE_PARTY_RO2] FOREIGN KEY([FirstPartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[PartyRelationship] CHECK CONSTRAINT [FK_PARTY_RE_REFERENCE_PARTY_RO2]
GO
/****** Object:  ForeignKey [FK_PARTYREL_REFERENCE_PARTYREL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PartyRelationship]  WITH CHECK ADD  CONSTRAINT [FK_PARTYREL_REFERENCE_PARTYREL] FOREIGN KEY([PartyRelTypeId])
REFERENCES [dbo].[PartyRelType] ([Id])
GO
ALTER TABLE [dbo].[PartyRelationship] CHECK CONSTRAINT [FK_PARTYREL_REFERENCE_PARTYREL]
GO
/****** Object:  ForeignKey [FK_PARTYREL_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PartyRelationship]  WITH CHECK ADD  CONSTRAINT [FK_PARTYREL_REFERENCE_PARTYROL] FOREIGN KEY([SecondPartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[PartyRelationship] CHECK CONSTRAINT [FK_PARTYREL_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_LOANMODI_REFERENCE_LOANMODI4]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanModification]  WITH CHECK ADD  CONSTRAINT [FK_LOANMODI_REFERENCE_LOANMODI4] FOREIGN KEY([LoanModificationTypeId])
REFERENCES [dbo].[LoanModificationType] ([Id])
GO
ALTER TABLE [dbo].[LoanModification] CHECK CONSTRAINT [FK_LOANMODI_REFERENCE_LOANMODI4]
GO
/****** Object:  ForeignKey [FK_LOANMODI_REFERENCE_PARTYROL2]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanModification]  WITH CHECK ADD  CONSTRAINT [FK_LOANMODI_REFERENCE_PARTYROL2] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[LoanModification] CHECK CONSTRAINT [FK_LOANMODI_REFERENCE_PARTYROL2]
GO
/****** Object:  ForeignKey [FK_LOANAGRE_REFERENCE_AGREEMEN]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanAgreement]  WITH CHECK ADD  CONSTRAINT [FK_LOANAGRE_REFERENCE_AGREEMEN] FOREIGN KEY([AgreementId])
REFERENCES [dbo].[Agreement] ([Id])
GO
ALTER TABLE [dbo].[LoanAgreement] CHECK CONSTRAINT [FK_LOANAGRE_REFERENCE_AGREEMEN]
GO
/****** Object:  ForeignKey [FK_MARITALS_REFERENCE_MARITALS]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[MaritalStatus]  WITH CHECK ADD  CONSTRAINT [FK_MARITALS_REFERENCE_MARITALS] FOREIGN KEY([MaritalStatusTypeId])
REFERENCES [dbo].[MaritalStatusType] ([Id])
GO
ALTER TABLE [dbo].[MaritalStatus] CHECK CONSTRAINT [FK_MARITALS_REFERENCE_MARITALS]
GO
/****** Object:  ForeignKey [FK_MARITALS_REFERENCE_PERSON]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[MaritalStatus]  WITH CHECK ADD  CONSTRAINT [FK_MARITALS_REFERENCE_PERSON] FOREIGN KEY([PartyId])
REFERENCES [dbo].[Person] ([PartyId])
GO
ALTER TABLE [dbo].[MaritalStatus] CHECK CONSTRAINT [FK_MARITALS_REFERENCE_PERSON]
GO
/****** Object:  ForeignKey [FK_LOAN_APP_REFERENCE_LOAN_APP2]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanApplicationStatus]  WITH CHECK ADD  CONSTRAINT [FK_LOAN_APP_REFERENCE_LOAN_APP2] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[LoanApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[LoanApplicationStatus] CHECK CONSTRAINT [FK_LOAN_APP_REFERENCE_LOAN_APP2]
GO
/****** Object:  ForeignKey [FK_LOAN_APP_REFERENCE_LOAN_APP3]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanApplicationStatus]  WITH CHECK ADD  CONSTRAINT [FK_LOAN_APP_REFERENCE_LOAN_APP3] FOREIGN KEY([StatusTypeId])
REFERENCES [dbo].[LoanApplicationStatusType] ([Id])
GO
ALTER TABLE [dbo].[LoanApplicationStatus] CHECK CONSTRAINT [FK_LOAN_APP_REFERENCE_LOAN_APP3]
GO
/****** Object:  ForeignKey [FK_LOAN_APP_REFERENCE_LOAN_APP4]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanApplicationRole]  WITH CHECK ADD  CONSTRAINT [FK_LOAN_APP_REFERENCE_LOAN_APP4] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[LoanApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[LoanApplicationRole] CHECK CONSTRAINT [FK_LOAN_APP_REFERENCE_LOAN_APP4]
GO
/****** Object:  ForeignKey [FK_LOANAPPL_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanApplicationRole]  WITH CHECK ADD  CONSTRAINT [FK_LOANAPPL_REFERENCE_PARTYROL] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[LoanApplicationRole] CHECK CONSTRAINT [FK_LOANAPPL_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_LOANAPPL_REFERENCE_LOANAPPL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanApplicationFee]  WITH CHECK ADD  CONSTRAINT [FK_LOANAPPL_REFERENCE_LOANAPPL] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[LoanApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[LoanApplicationFee] CHECK CONSTRAINT [FK_LOANAPPL_REFERENCE_LOANAPPL]
GO
/****** Object:  ForeignKey [FK_LOANDISB_REFERENCE_AGREEMEN]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[LoanDisbursementVcr]  WITH CHECK ADD  CONSTRAINT [FK_LOANDISB_REFERENCE_AGREEMEN] FOREIGN KEY([AgreementId])
REFERENCES [dbo].[Agreement] ([Id])
GO
ALTER TABLE [dbo].[LoanDisbursementVcr] CHECK CONSTRAINT [FK_LOANDISB_REFERENCE_AGREEMEN]
GO
/****** Object:  ForeignKey [FK_HOMEOWNE_REFERENCE_HOMEOWNE]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[HomeOwnership]  WITH CHECK ADD  CONSTRAINT [FK_HOMEOWNE_REFERENCE_HOMEOWNE] FOREIGN KEY([HomeOwnershipTypeId])
REFERENCES [dbo].[HomeOwnershipType] ([Id])
GO
ALTER TABLE [dbo].[HomeOwnership] CHECK CONSTRAINT [FK_HOMEOWNE_REFERENCE_HOMEOWNE]
GO
/****** Object:  ForeignKey [FK_HOMEOWNE_REFERENCE_PERSON]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[HomeOwnership]  WITH CHECK ADD  CONSTRAINT [FK_HOMEOWNE_REFERENCE_PERSON] FOREIGN KEY([PartyId])
REFERENCES [dbo].[Person] ([PartyId])
GO
ALTER TABLE [dbo].[HomeOwnership] CHECK CONSTRAINT [FK_HOMEOWNE_REFERENCE_PERSON]
GO
/****** Object:  ForeignKey [FK_ForExCheque_ForeignExchangeAmount]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ForExCheque]  WITH CHECK ADD  CONSTRAINT [FK_ForExCheque_ForeignExchangeAmount] FOREIGN KEY([ForExDetailId])
REFERENCES [dbo].[ForExDetail] ([Id])
GO
ALTER TABLE [dbo].[ForExCheque] CHECK CONSTRAINT [FK_ForExCheque_ForeignExchangeAmount]
GO
/****** Object:  ForeignKey [FK_ForExCheque_PartyRole]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ForExCheque]  WITH CHECK ADD  CONSTRAINT [FK_ForExCheque_PartyRole] FOREIGN KEY([BankPartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[ForExCheque] CHECK CONSTRAINT [FK_ForExCheque_PartyRole]
GO
/****** Object:  ForeignKey [FK_APPLICAT_REFERENCE_APPLICAT]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ApplicationItem]  WITH CHECK ADD  CONSTRAINT [FK_APPLICAT_REFERENCE_APPLICAT] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[ApplicationItem] CHECK CONSTRAINT [FK_APPLICAT_REFERENCE_APPLICAT]
GO
/****** Object:  ForeignKey [FK_APPLICAT_REFERENCE_PRODUCTF]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ApplicationItem]  WITH CHECK ADD  CONSTRAINT [FK_APPLICAT_REFERENCE_PRODUCTF] FOREIGN KEY([ProdFeatApplicabilityId])
REFERENCES [dbo].[ProductFeatureApplicability] ([Id])
GO
ALTER TABLE [dbo].[ApplicationItem] CHECK CONSTRAINT [FK_APPLICAT_REFERENCE_PRODUCTF]
GO
/****** Object:  ForeignKey [FK_ASSET_REFERENCE_ASSETTYP]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Asset]  WITH CHECK ADD  CONSTRAINT [FK_ASSET_REFERENCE_ASSETTYP] FOREIGN KEY([AssetTypeId])
REFERENCES [dbo].[AssetType] ([Id])
GO
ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_ASSET_REFERENCE_ASSETTYP]
GO
/****** Object:  ForeignKey [FK_ASSET_REFERENCE_LOANAPPL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Asset]  WITH CHECK ADD  CONSTRAINT [FK_ASSET_REFERENCE_LOANAPPL] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[LoanApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_ASSET_REFERENCE_LOANAPPL]
GO
/****** Object:  ForeignKey [FK_BANK_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Bank]  WITH CHECK ADD  CONSTRAINT [FK_BANK_REFERENCE_PARTYROL] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[Bank] CHECK CONSTRAINT [FK_BANK_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_AGREEMEN_REFERENCE_AGREEMEN2]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[AgreementRole]  WITH CHECK ADD  CONSTRAINT [FK_AGREEMEN_REFERENCE_AGREEMEN2] FOREIGN KEY([AgreementId])
REFERENCES [dbo].[Agreement] ([Id])
GO
ALTER TABLE [dbo].[AgreementRole] CHECK CONSTRAINT [FK_AGREEMEN_REFERENCE_AGREEMEN2]
GO
/****** Object:  ForeignKey [FK_AGREEMEN_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[AgreementRole]  WITH CHECK ADD  CONSTRAINT [FK_AGREEMEN_REFERENCE_PARTYROL] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[AgreementRole] CHECK CONSTRAINT [FK_AGREEMEN_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_AGREEMEN_REFERENCE_AGREEMEN5]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[AgreementItem]  WITH CHECK ADD  CONSTRAINT [FK_AGREEMEN_REFERENCE_AGREEMEN5] FOREIGN KEY([AgreementId])
REFERENCES [dbo].[Agreement] ([Id])
GO
ALTER TABLE [dbo].[AgreementItem] CHECK CONSTRAINT [FK_AGREEMEN_REFERENCE_AGREEMEN5]
GO
/****** Object:  ForeignKey [FK_CashOnVault_Currency]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[CashOnVault]  WITH CHECK ADD  CONSTRAINT [FK_CashOnVault_Currency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[CashOnVault] CHECK CONSTRAINT [FK_CashOnVault_Currency]
GO
/****** Object:  ForeignKey [FK_CashOnVault_PartyRole]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[CashOnVault]  WITH CHECK ADD  CONSTRAINT [FK_CashOnVault_PartyRole] FOREIGN KEY([ClosedByPartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[CashOnVault] CHECK CONSTRAINT [FK_CashOnVault_PartyRole]
GO
/****** Object:  ForeignKey [FK_COMPROMI_REFERENCE_AGREEMEN]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[CompromiseAgreement]  WITH CHECK ADD  CONSTRAINT [FK_COMPROMI_REFERENCE_AGREEMEN] FOREIGN KEY([AgreementId])
REFERENCES [dbo].[Agreement] ([Id])
GO
ALTER TABLE [dbo].[CompromiseAgreement] CHECK CONSTRAINT [FK_COMPROMI_REFERENCE_AGREEMEN]
GO
/****** Object:  ForeignKey [FK_CUSTOMER_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_REFERENCE_PARTYROL] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_CUSTOMER_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_COVDetails_COVTransactionType]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[COVTransaction]  WITH CHECK ADD  CONSTRAINT [FK_COVDetails_COVTransactionType] FOREIGN KEY([COVTransTypeId])
REFERENCES [dbo].[COVTransactionType] ([Id])
GO
ALTER TABLE [dbo].[COVTransaction] CHECK CONSTRAINT [FK_COVDetails_COVTransactionType]
GO
/****** Object:  ForeignKey [FK_COVDetails_PartyRole]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[COVTransaction]  WITH CHECK ADD  CONSTRAINT [FK_COVDetails_PartyRole] FOREIGN KEY([ProcessedByPartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[COVTransaction] CHECK CONSTRAINT [FK_COVDetails_PartyRole]
GO
/****** Object:  ForeignKey [FK_COVTransaction_Currency]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[COVTransaction]  WITH CHECK ADD  CONSTRAINT [FK_COVTransaction_Currency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[COVTransaction] CHECK CONSTRAINT [FK_COVTransaction_Currency]
GO
/****** Object:  ForeignKey [FK_ForeignExchange_Currency]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ForeignExchange]  WITH CHECK ADD  CONSTRAINT [FK_ForeignExchange_Currency] FOREIGN KEY([OriginalCurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[ForeignExchange] CHECK CONSTRAINT [FK_ForeignExchange_Currency]
GO
/****** Object:  ForeignKey [FK_ForeignExchange_Currency1]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ForeignExchange]  WITH CHECK ADD  CONSTRAINT [FK_ForeignExchange_Currency1] FOREIGN KEY([ConvertedCurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[ForeignExchange] CHECK CONSTRAINT [FK_ForeignExchange_Currency1]
GO
/****** Object:  ForeignKey [FK_ForeignExchange_PartyRole]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ForeignExchange]  WITH CHECK ADD  CONSTRAINT [FK_ForeignExchange_PartyRole] FOREIGN KEY([ProcessedByPartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[ForeignExchange] CHECK CONSTRAINT [FK_ForeignExchange_PartyRole]
GO
/****** Object:  ForeignKey [FK_ForeignExchange_PartyRole1]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ForeignExchange]  WITH CHECK ADD  CONSTRAINT [FK_ForeignExchange_PartyRole1] FOREIGN KEY([ProcessedToPartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[ForeignExchange] CHECK CONSTRAINT [FK_ForeignExchange_PartyRole1]
GO
/****** Object:  ForeignKey [FK_FEE_REFERENCE_PRODUCTF]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Fee]  WITH CHECK ADD  CONSTRAINT [FK_FEE_REFERENCE_PRODUCTF] FOREIGN KEY([ProductFeatApplicabilityId])
REFERENCES [dbo].[ProductFeatureApplicability] ([Id])
GO
ALTER TABLE [dbo].[Fee] CHECK CONSTRAINT [FK_FEE_REFERENCE_PRODUCTF]
GO
/****** Object:  ForeignKey [FK_FINANCIA_REFERENCE_AGREEMEN]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[FinancialAccount]  WITH CHECK ADD  CONSTRAINT [FK_FINANCIA_REFERENCE_AGREEMEN] FOREIGN KEY([AgreementId])
REFERENCES [dbo].[Agreement] ([Id])
GO
ALTER TABLE [dbo].[FinancialAccount] CHECK CONSTRAINT [FK_FINANCIA_REFERENCE_AGREEMEN]
GO
/****** Object:  ForeignKey [FK_FINANCIA_REFERENCE_FINANCIA5]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[FinancialAccount]  WITH CHECK ADD  CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA5] FOREIGN KEY([FinancialAccountTypeId])
REFERENCES [dbo].[FinancialAccountType] ([Id])
GO
ALTER TABLE [dbo].[FinancialAccount] CHECK CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA5]
GO
/****** Object:  ForeignKey [FK_FINANCIA_REFERENCE_FINANCIA6]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[FinancialAccount]  WITH CHECK ADD  CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA6] FOREIGN KEY([ParentFinancialAccountId])
REFERENCES [dbo].[FinancialAccount] ([Id])
GO
ALTER TABLE [dbo].[FinancialAccount] CHECK CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA6]
GO
/****** Object:  ForeignKey [FK_EMPLOYEE_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_EMPLOYEE_REFERENCE_PARTYROL] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_EMPLOYEE_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_PETTY_CA_REFERENCE_PETTY_CA2]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PettyCashLoanItem]  WITH CHECK ADD  CONSTRAINT [FK_PETTY_CA_REFERENCE_PETTY_CA2] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[PettyCashLoanApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[PettyCashLoanItem] CHECK CONSTRAINT [FK_PETTY_CA_REFERENCE_PETTY_CA2]
GO
/****** Object:  ForeignKey [FK_PERSONNA_REFERENCE_PERSON]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PersonName]  WITH CHECK ADD  CONSTRAINT [FK_PERSONNA_REFERENCE_PERSON] FOREIGN KEY([PartyId])
REFERENCES [dbo].[Person] ([PartyId])
GO
ALTER TABLE [dbo].[PersonName] CHECK CONSTRAINT [FK_PERSONNA_REFERENCE_PERSON]
GO
/****** Object:  ForeignKey [FK_PERSONNA_REFERENCE_PERSONNA]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PersonName]  WITH CHECK ADD  CONSTRAINT [FK_PERSONNA_REFERENCE_PERSONNA] FOREIGN KEY([PersonNameTypeId])
REFERENCES [dbo].[PersonNameType] ([Id])
GO
ALTER TABLE [dbo].[PersonName] CHECK CONSTRAINT [FK_PERSONNA_REFERENCE_PERSONNA]
GO
/****** Object:  ForeignKey [FK_PERSONID_REFERENCE_IDENTIFI]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PersonIdentification]  WITH CHECK ADD  CONSTRAINT [FK_PERSONID_REFERENCE_IDENTIFI] FOREIGN KEY([IdentificationTypeId])
REFERENCES [dbo].[IdentificationType] ([Id])
GO
ALTER TABLE [dbo].[PersonIdentification] CHECK CONSTRAINT [FK_PERSONID_REFERENCE_IDENTIFI]
GO
/****** Object:  ForeignKey [FK_PERSONID_REFERENCE_PERSON]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PersonIdentification]  WITH CHECK ADD  CONSTRAINT [FK_PERSONID_REFERENCE_PERSON] FOREIGN KEY([PartyId])
REFERENCES [dbo].[Person] ([PartyId])
GO
ALTER TABLE [dbo].[PersonIdentification] CHECK CONSTRAINT [FK_PERSONID_REFERENCE_PERSON]
GO
/****** Object:  ForeignKey [FK_PETTY_CA_REFERENCE_PETTY_CA3]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PettyCashLoanApplicationStatus]  WITH CHECK ADD  CONSTRAINT [FK_PETTY_CA_REFERENCE_PETTY_CA3] FOREIGN KEY([StatusTypeId])
REFERENCES [dbo].[PettyCashLoanAppStatusType] ([Id])
GO
ALTER TABLE [dbo].[PettyCashLoanApplicationStatus] CHECK CONSTRAINT [FK_PETTY_CA_REFERENCE_PETTY_CA3]
GO
/****** Object:  ForeignKey [FK_PETTY_CA_REFERENCE_PETTY_CA4]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PettyCashLoanApplicationStatus]  WITH CHECK ADD  CONSTRAINT [FK_PETTY_CA_REFERENCE_PETTY_CA4] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[PettyCashLoanApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[PettyCashLoanApplicationStatus] CHECK CONSTRAINT [FK_PETTY_CA_REFERENCE_PETTY_CA4]
GO
/****** Object:  ForeignKey [FK_TAXPAYER_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Taxpayer]  WITH CHECK ADD  CONSTRAINT [FK_TAXPAYER_REFERENCE_PARTYROL] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[Taxpayer] CHECK CONSTRAINT [FK_TAXPAYER_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_PRODUCT_10]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ProductCategoryFeatureFunctionalApplicability]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_10] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategory] ([Id])
GO
ALTER TABLE [dbo].[ProductCategoryFeatureFunctionalApplicability] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_10]
GO
/****** Object:  ForeignKey [FK_PRODUCT__REFERENCE_PRODUCT_9]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ProductCategoryFeatureFunctionalApplicability]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_9] FOREIGN KEY([ProductCatFeatApplicabilityId])
REFERENCES [dbo].[ProductCatFeatApplicability] ([Id])
GO
ALTER TABLE [dbo].[ProductCategoryFeatureFunctionalApplicability] CHECK CONSTRAINT [FK_PRODUCT__REFERENCE_PRODUCT_9]
GO
/****** Object:  ForeignKey [FK_USERACCO_REFERENCE_PERSON]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[UserAccount]  WITH CHECK ADD  CONSTRAINT [FK_USERACCO_REFERENCE_PERSON] FOREIGN KEY([PartyId])
REFERENCES [dbo].[Person] ([PartyId])
GO
ALTER TABLE [dbo].[UserAccount] CHECK CONSTRAINT [FK_USERACCO_REFERENCE_PERSON]
GO
/****** Object:  ForeignKey [FK_UserAccount_UserAccountType]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[UserAccount]  WITH CHECK ADD  CONSTRAINT [FK_UserAccount_UserAccountType] FOREIGN KEY([UserAccountTypeId])
REFERENCES [dbo].[UserAccountType] ([Id])
GO
ALTER TABLE [dbo].[UserAccount] CHECK CONSTRAINT [FK_UserAccount_UserAccountType]
GO
/****** Object:  ForeignKey [FK_SUBMITTE_REFERENCE_LOANAPPL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[SubmittedDocument]  WITH CHECK ADD  CONSTRAINT [FK_SUBMITTE_REFERENCE_LOANAPPL] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[LoanApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[SubmittedDocument] CHECK CONSTRAINT [FK_SUBMITTE_REFERENCE_LOANAPPL]
GO
/****** Object:  ForeignKey [FK_SUBMITTE_REFERENCE_PRODUCTR]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[SubmittedDocument]  WITH CHECK ADD  CONSTRAINT [FK_SUBMITTE_REFERENCE_PRODUCTR] FOREIGN KEY([ProductRequiredDocumentId])
REFERENCES [dbo].[ProductRequiredDocument] ([Id])
GO
ALTER TABLE [dbo].[SubmittedDocument] CHECK CONSTRAINT [FK_SUBMITTE_REFERENCE_PRODUCTR]
GO
/****** Object:  ForeignKey [FK_SPECIMEN_REFERENCE_PERSON]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[SpecimenSignature]  WITH CHECK ADD  CONSTRAINT [FK_SPECIMEN_REFERENCE_PERSON] FOREIGN KEY([PartyId])
REFERENCES [dbo].[Person] ([PartyId])
GO
ALTER TABLE [dbo].[SpecimenSignature] CHECK CONSTRAINT [FK_SPECIMEN_REFERENCE_PERSON]
GO
/****** Object:  ForeignKey [FK_Vehicle_Asset1]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_Asset1] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([Id])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_Asset1]
GO
/****** Object:  ForeignKey [FK_Vehicle_VehicleType]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_VehicleType] FOREIGN KEY([VehicleTypeId])
REFERENCES [dbo].[VehicleType] ([Id])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_VehicleType]
GO
/****** Object:  ForeignKey [FK_ReceiptPaymentAssoc_Payment]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ReceiptPaymentAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ReceiptPaymentAssoc_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[ReceiptPaymentAssoc] CHECK CONSTRAINT [FK_ReceiptPaymentAssoc_Payment]
GO
/****** Object:  ForeignKey [FK_ReceiptPaymentAssoc_Receipt]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ReceiptPaymentAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ReceiptPaymentAssoc_Receipt] FOREIGN KEY([ReceiptId])
REFERENCES [dbo].[Receipt] ([Id])
GO
ALTER TABLE [dbo].[ReceiptPaymentAssoc] CHECK CONSTRAINT [FK_ReceiptPaymentAssoc_Receipt]
GO
/****** Object:  ForeignKey [FK_SUBMITTE_REFERENCE_SUBMITTE]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[SubmittedDocumentStatus]  WITH CHECK ADD  CONSTRAINT [FK_SUBMITTE_REFERENCE_SUBMITTE] FOREIGN KEY([SubmittedDocumentId])
REFERENCES [dbo].[SubmittedDocument] ([Id])
GO
ALTER TABLE [dbo].[SubmittedDocumentStatus] CHECK CONSTRAINT [FK_SUBMITTE_REFERENCE_SUBMITTE]
GO
/****** Object:  ForeignKey [FK_SUBMITTE_REFERENCE_SUBMITTE2]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[SubmittedDocumentStatus]  WITH CHECK ADD  CONSTRAINT [FK_SUBMITTE_REFERENCE_SUBMITTE2] FOREIGN KEY([StatusTypeId])
REFERENCES [dbo].[SubmittedDocumentStatusType] ([Id])
GO
ALTER TABLE [dbo].[SubmittedDocumentStatus] CHECK CONSTRAINT [FK_SUBMITTE_REFERENCE_SUBMITTE2]
GO
/****** Object:  ForeignKey [FK_USER_ACC_REFERENCE_USER_ACC2]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[UserAccountStatus]  WITH CHECK ADD  CONSTRAINT [FK_USER_ACC_REFERENCE_USER_ACC2] FOREIGN KEY([UserAccountId])
REFERENCES [dbo].[UserAccount] ([Id])
GO
ALTER TABLE [dbo].[UserAccountStatus] CHECK CONSTRAINT [FK_USER_ACC_REFERENCE_USER_ACC2]
GO
/****** Object:  ForeignKey [FK_USERACCO_REFERENCE_USERACCO]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[UserAccountStatus]  WITH CHECK ADD  CONSTRAINT [FK_USERACCO_REFERENCE_USERACCO] FOREIGN KEY([StatusTypeId])
REFERENCES [dbo].[UserAccountStatusType] ([Id])
GO
ALTER TABLE [dbo].[UserAccountStatus] CHECK CONSTRAINT [FK_USERACCO_REFERENCE_USERACCO]
GO
/****** Object:  ForeignKey [FK_PaymentCurrencyAssoc_Currency]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PaymentCurrencyAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PaymentCurrencyAssoc_Currency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[PaymentCurrencyAssoc] CHECK CONSTRAINT [FK_PaymentCurrencyAssoc_Currency]
GO
/****** Object:  ForeignKey [FK_PaymentCurrencyAssoc_Payment]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[PaymentCurrencyAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PaymentCurrencyAssoc_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[PaymentCurrencyAssoc] CHECK CONSTRAINT [FK_PaymentCurrencyAssoc_Payment]
GO
/****** Object:  ForeignKey [FK_FINACCTT_REFERENCE_FINANCIA]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[FinAcctTrans]  WITH CHECK ADD  CONSTRAINT [FK_FINACCTT_REFERENCE_FINANCIA] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[FinancialAccount] ([Id])
GO
ALTER TABLE [dbo].[FinAcctTrans] CHECK CONSTRAINT [FK_FINACCTT_REFERENCE_FINANCIA]
GO
/****** Object:  ForeignKey [FK_FINACCTT_REFERENCE_FINLACCT]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[FinAcctTrans]  WITH CHECK ADD  CONSTRAINT [FK_FINACCTT_REFERENCE_FINLACCT] FOREIGN KEY([FinancialAcctTransTypeId])
REFERENCES [dbo].[FinlAcctTransType] ([Id])
GO
ALTER TABLE [dbo].[FinAcctTrans] CHECK CONSTRAINT [FK_FINACCTT_REFERENCE_FINLACCT]
GO
/****** Object:  ForeignKey [FK_FINACCTT_REFERENCE_PAYMENT]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[FinAcctTrans]  WITH CHECK ADD  CONSTRAINT [FK_FINACCTT_REFERENCE_PAYMENT] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[FinAcctTrans] CHECK CONSTRAINT [FK_FINACCTT_REFERENCE_PAYMENT]
GO
/****** Object:  ForeignKey [FK_FeePayment_Payment]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[FeePayment]  WITH CHECK ADD  CONSTRAINT [FK_FeePayment_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[FeePayment] CHECK CONSTRAINT [FK_FeePayment_Payment]
GO
/****** Object:  ForeignKey [FK_EMPLOYME_REFERENCE_PARTYREL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Employment]  WITH CHECK ADD  CONSTRAINT [FK_EMPLOYME_REFERENCE_PARTYREL] FOREIGN KEY([PartyRelationshipId])
REFERENCES [dbo].[PartyRelationship] ([Id])
GO
ALTER TABLE [dbo].[Employment] CHECK CONSTRAINT [FK_EMPLOYME_REFERENCE_PARTYREL]
GO
/****** Object:  ForeignKey [FK_DOCUMENT_REFERENCE_SUBMITTE]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[DocumentPage]  WITH CHECK ADD  CONSTRAINT [FK_DOCUMENT_REFERENCE_SUBMITTE] FOREIGN KEY([SubmittedDocumentId])
REFERENCES [dbo].[SubmittedDocument] ([Id])
GO
ALTER TABLE [dbo].[DocumentPage] CHECK CONSTRAINT [FK_DOCUMENT_REFERENCE_SUBMITTE]
GO
/****** Object:  ForeignKey [FK_FINANCIA_REFERENCE_FINANCIA2]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[FinancialAccountRole]  WITH CHECK ADD  CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA2] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[FinancialAccount] ([Id])
GO
ALTER TABLE [dbo].[FinancialAccountRole] CHECK CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA2]
GO
/****** Object:  ForeignKey [FK_FINANCIA_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[FinancialAccountRole]  WITH CHECK ADD  CONSTRAINT [FK_FINANCIA_REFERENCE_PARTYROL] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[FinancialAccountRole] CHECK CONSTRAINT [FK_FINANCIA_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_FINANCIA_REFERENCE_FINANCIA3]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[FinancialAccountProduct]  WITH CHECK ADD  CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA3] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[FinancialAccount] ([Id])
GO
ALTER TABLE [dbo].[FinancialAccountProduct] CHECK CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA3]
GO
/****** Object:  ForeignKey [FK_FINANCIA_REFERENCE_FINANCIA4]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[FinancialAccountProduct]  WITH CHECK ADD  CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA4] FOREIGN KEY([FinancialProductId])
REFERENCES [dbo].[FinancialProduct] ([Id])
GO
ALTER TABLE [dbo].[FinancialAccountProduct] CHECK CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA4]
GO
/****** Object:  ForeignKey [FK_DISBURSE_REFERENCE_DISBURSE5]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Disbursement]  WITH CHECK ADD  CONSTRAINT [FK_DISBURSE_REFERENCE_DISBURSE5] FOREIGN KEY([DisbursementTypeId])
REFERENCES [dbo].[DisbursementType] ([Id])
GO
ALTER TABLE [dbo].[Disbursement] CHECK CONSTRAINT [FK_DISBURSE_REFERENCE_DISBURSE5]
GO
/****** Object:  ForeignKey [FK_DISBURSE_REFERENCE_PAYMENT]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Disbursement]  WITH CHECK ADD  CONSTRAINT [FK_DISBURSE_REFERENCE_PAYMENT] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[Disbursement] CHECK CONSTRAINT [FK_DISBURSE_REFERENCE_PAYMENT]
GO
/****** Object:  ForeignKey [FK_DISBURSE_REFERENCE_DISBURSE4]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[DisbursementVcrStatus]  WITH CHECK ADD  CONSTRAINT [FK_DISBURSE_REFERENCE_DISBURSE4] FOREIGN KEY([DisbursementVoucherStatTypId])
REFERENCES [dbo].[DisbursementVcrStatusType] ([Id])
GO
ALTER TABLE [dbo].[DisbursementVcrStatus] CHECK CONSTRAINT [FK_DISBURSE_REFERENCE_DISBURSE4]
GO
/****** Object:  ForeignKey [FK_DISBURSE_REFERENCE_LOANDISB]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[DisbursementVcrStatus]  WITH CHECK ADD  CONSTRAINT [FK_DISBURSE_REFERENCE_LOANDISB] FOREIGN KEY([LoanDisbursementVoucherId])
REFERENCES [dbo].[LoanDisbursementVcr] ([Id])
GO
ALTER TABLE [dbo].[DisbursementVcrStatus] CHECK CONSTRAINT [FK_DISBURSE_REFERENCE_LOANDISB]
GO
/****** Object:  ForeignKey [FK_CUSTOMER]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[CustomerStatus]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER] FOREIGN KEY([CustomerStatusTypeId])
REFERENCES [dbo].[CustomerStatusType] ([Id])
GO
ALTER TABLE [dbo].[CustomerStatus] CHECK CONSTRAINT [FK_CUSTOMER]
GO
/****** Object:  ForeignKey [FK_CUSTOMER_REFERENCE_CUSTOMER3]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[CustomerStatus]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_REFERENCE_CUSTOMER3] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[Customer] ([PartyRoleId])
GO
ALTER TABLE [dbo].[CustomerStatus] CHECK CONSTRAINT [FK_CUSTOMER_REFERENCE_CUSTOMER3]
GO
/****** Object:  ForeignKey [FK_CUSTOMER_REFERENCE_CUSTOMER2]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[CustomerSourceOfIncome]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_REFERENCE_CUSTOMER2] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[Customer] ([PartyRoleId])
GO
ALTER TABLE [dbo].[CustomerSourceOfIncome] CHECK CONSTRAINT [FK_CUSTOMER_REFERENCE_CUSTOMER2]
GO
/****** Object:  ForeignKey [FK_CUSTOMER_REFERENCE_SOURCEOF]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[CustomerSourceOfIncome]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_REFERENCE_SOURCEOF] FOREIGN KEY([SourceOfIncomeId])
REFERENCES [dbo].[SourceOfIncome] ([Id])
GO
ALTER TABLE [dbo].[CustomerSourceOfIncome] CHECK CONSTRAINT [FK_CUSTOMER_REFERENCE_SOURCEOF]
GO
/****** Object:  ForeignKey [FK_CUSTOMER_REFERENCE_CLASSIFI]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[CustomerClassification]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_REFERENCE_CLASSIFI] FOREIGN KEY([ClassificationTypeId])
REFERENCES [dbo].[ClassificationType] ([Id])
GO
ALTER TABLE [dbo].[CustomerClassification] CHECK CONSTRAINT [FK_CUSTOMER_REFERENCE_CLASSIFI]
GO
/****** Object:  ForeignKey [FK_CUSTOMER_REFERENCE_CUSTOMER]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[CustomerClassification]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_REFERENCE_CUSTOMER] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[Customer] ([PartyRoleId])
GO
ALTER TABLE [dbo].[CustomerClassification] CHECK CONSTRAINT [FK_CUSTOMER_REFERENCE_CUSTOMER]
GO
/****** Object:  ForeignKey [FK_CUSTOMER_REFERENCE_CUSTOME_]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[CustomerCategory]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_REFERENCE_CUSTOME_] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[Customer] ([PartyRoleId])
GO
ALTER TABLE [dbo].[CustomerCategory] CHECK CONSTRAINT [FK_CUSTOMER_REFERENCE_CUSTOME_]
GO
/****** Object:  ForeignKey [FK_CUSTOMER_REFERENCE_CUSTOMER1]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[CustomerCategory]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_REFERENCE_CUSTOMER1] FOREIGN KEY([CustomerCategoryType])
REFERENCES [dbo].[CustomerCategoryType] ([Id])
GO
ALTER TABLE [dbo].[CustomerCategory] CHECK CONSTRAINT [FK_CUSTOMER_REFERENCE_CUSTOMER1]
GO
/****** Object:  ForeignKey [FK_CTC_REFERENCE_TAXPAYER]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[Ctc]  WITH CHECK ADD  CONSTRAINT [FK_CTC_REFERENCE_TAXPAYER] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[Taxpayer] ([PartyRoleId])
GO
ALTER TABLE [dbo].[Ctc] CHECK CONSTRAINT [FK_CTC_REFERENCE_TAXPAYER]
GO
/****** Object:  ForeignKey [FK_ControlNumbers_FormType]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ControlNumbers]  WITH CHECK ADD  CONSTRAINT [FK_ControlNumbers_FormType] FOREIGN KEY([FormTypeId])
REFERENCES [dbo].[FormType] ([Id])
GO
ALTER TABLE [dbo].[ControlNumbers] CHECK CONSTRAINT [FK_ControlNumbers_FormType]
GO
/****** Object:  ForeignKey [FK_ControlNumbers_LoanApplication]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ControlNumbers]  WITH CHECK ADD  CONSTRAINT [FK_ControlNumbers_LoanApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[LoanApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[ControlNumbers] CHECK CONSTRAINT [FK_ControlNumbers_LoanApplication]
GO
/****** Object:  ForeignKey [FK_ControlNumbers_Payment]    Script Date: 02/22/2012 10:28:11 ******/
ALTER TABLE [dbo].[ControlNumbers]  WITH CHECK ADD  CONSTRAINT [FK_ControlNumbers_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[ControlNumbers] CHECK CONSTRAINT [FK_ControlNumbers_Payment]
GO
/****** Object:  ForeignKey [FK_BANKSTAT_REFERENCE_BANK]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[BankStatus]  WITH CHECK ADD  CONSTRAINT [FK_BANKSTAT_REFERENCE_BANK] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[Bank] ([PartyRoleId])
GO
ALTER TABLE [dbo].[BankStatus] CHECK CONSTRAINT [FK_BANKSTAT_REFERENCE_BANK]
GO
/****** Object:  ForeignKey [FK_BANKSTAT_REFERENCE_BANKSTAT]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[BankStatus]  WITH CHECK ADD  CONSTRAINT [FK_BANKSTAT_REFERENCE_BANKSTAT] FOREIGN KEY([BankStatusTypeId])
REFERENCES [dbo].[BankStatusType] ([Id])
GO
ALTER TABLE [dbo].[BankStatus] CHECK CONSTRAINT [FK_BANKSTAT_REFERENCE_BANKSTAT]
GO
/****** Object:  ForeignKey [FK_BANKACCO_REFERENCE_ASSET]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[BankAccount]  WITH CHECK ADD  CONSTRAINT [FK_BANKACCO_REFERENCE_ASSET] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([Id])
GO
ALTER TABLE [dbo].[BankAccount] CHECK CONSTRAINT [FK_BANKACCO_REFERENCE_ASSET]
GO
/****** Object:  ForeignKey [FK_BANKACCO_REFERENCE_BANKACCO]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[BankAccount]  WITH CHECK ADD  CONSTRAINT [FK_BANKACCO_REFERENCE_BANKACCO] FOREIGN KEY([BankAccountTypeId])
REFERENCES [dbo].[BankAccountType] ([Id])
GO
ALTER TABLE [dbo].[BankAccount] CHECK CONSTRAINT [FK_BANKACCO_REFERENCE_BANKACCO]
GO
/****** Object:  ForeignKey [FK_BANKACCO_REFERENCE_LOANAPPL]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[BankAccount]  WITH CHECK ADD  CONSTRAINT [FK_BANKACCO_REFERENCE_LOANAPPL] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[LoanApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[BankAccount] CHECK CONSTRAINT [FK_BANKACCO_REFERENCE_LOANAPPL]
GO
/****** Object:  ForeignKey [FK_BANKACCO_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[BankAccount]  WITH CHECK ADD  CONSTRAINT [FK_BANKACCO_REFERENCE_PARTYROL] FOREIGN KEY([BankPartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[BankAccount] CHECK CONSTRAINT [FK_BANKACCO_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_Cheque_Payment]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Cheque]  WITH CHECK ADD  CONSTRAINT [FK_Cheque_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[Cheque] CHECK CONSTRAINT [FK_Cheque_Payment]
GO
/****** Object:  ForeignKey [FK_CHEQUE_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Cheque]  WITH CHECK ADD  CONSTRAINT [FK_CHEQUE_REFERENCE_PARTYROL] FOREIGN KEY([BankPartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[Cheque] CHECK CONSTRAINT [FK_CHEQUE_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_ADDRESS_REFERENCE_ADDRESST]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_ADDRESS_REFERENCE_ADDRESST] FOREIGN KEY([AddressTypeId])
REFERENCES [dbo].[AddressType] ([Id])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_ADDRESS_REFERENCE_ADDRESST]
GO
/****** Object:  ForeignKey [FK_ADDRESS_REFERENCE_ASSET]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_ADDRESS_REFERENCE_ASSET] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([Id])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_ADDRESS_REFERENCE_ASSET]
GO
/****** Object:  ForeignKey [FK_ADDRESS_REFERENCE_PARTY]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_ADDRESS_REFERENCE_PARTY] FOREIGN KEY([PartyId])
REFERENCES [dbo].[Party] ([Id])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_ADDRESS_REFERENCE_PARTY]
GO
/****** Object:  ForeignKey [FK_ADDENDUM_REFERENCE_AGREEMEN]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Addendum]  WITH CHECK ADD  CONSTRAINT [FK_ADDENDUM_REFERENCE_AGREEMEN] FOREIGN KEY([AgreementId])
REFERENCES [dbo].[Agreement] ([Id])
GO
ALTER TABLE [dbo].[Addendum] CHECK CONSTRAINT [FK_ADDENDUM_REFERENCE_AGREEMEN]
GO
/****** Object:  ForeignKey [FK_ADDENDUM_REFERENCE_AGREEMEN2]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Addendum]  WITH CHECK ADD  CONSTRAINT [FK_ADDENDUM_REFERENCE_AGREEMEN2] FOREIGN KEY([AgreementItemId])
REFERENCES [dbo].[AgreementItem] ([Id])
GO
ALTER TABLE [dbo].[Addendum] CHECK CONSTRAINT [FK_ADDENDUM_REFERENCE_AGREEMEN2]
GO
/****** Object:  ForeignKey [FK_ASSETROL_REFERENCE_ASSET]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[AssetRole]  WITH CHECK ADD  CONSTRAINT [FK_ASSETROL_REFERENCE_ASSET] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([Id])
GO
ALTER TABLE [dbo].[AssetRole] CHECK CONSTRAINT [FK_ASSETROL_REFERENCE_ASSET]
GO
/****** Object:  ForeignKey [FK_ASSETROL_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[AssetRole]  WITH CHECK ADD  CONSTRAINT [FK_ASSETROL_REFERENCE_PARTYROL] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[AssetRole] CHECK CONSTRAINT [FK_ASSETROL_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_ASSETAPP_REFERENCE_ASSET]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[AssetAppraisal]  WITH CHECK ADD  CONSTRAINT [FK_ASSETAPP_REFERENCE_ASSET] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([Id])
GO
ALTER TABLE [dbo].[AssetAppraisal] CHECK CONSTRAINT [FK_ASSETAPP_REFERENCE_ASSET]
GO
/****** Object:  ForeignKey [FK_ASSETAPP_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[AssetAppraisal]  WITH CHECK ADD  CONSTRAINT [FK_ASSETAPP_REFERENCE_PARTYROL] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[AssetAppraisal] CHECK CONSTRAINT [FK_ASSETAPP_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_AMORTIZA_REFERENCE_AMORTIZA2]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[AmortizationSchedule]  WITH CHECK ADD  CONSTRAINT [FK_AMORTIZA_REFERENCE_AMORTIZA2] FOREIGN KEY([ParentAmortizationScheduleId])
REFERENCES [dbo].[AmortizationSchedule] ([Id])
GO
ALTER TABLE [dbo].[AmortizationSchedule] CHECK CONSTRAINT [FK_AMORTIZA_REFERENCE_AMORTIZA2]
GO
/****** Object:  ForeignKey [FK_AMORTIZA_REFERENCE_LOANAGRE]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[AmortizationSchedule]  WITH CHECK ADD  CONSTRAINT [FK_AMORTIZA_REFERENCE_LOANAGRE] FOREIGN KEY([AgreementId])
REFERENCES [dbo].[LoanAgreement] ([AgreementId])
GO
ALTER TABLE [dbo].[AmortizationSchedule] CHECK CONSTRAINT [FK_AMORTIZA_REFERENCE_LOANAGRE]
GO
/****** Object:  ForeignKey [FK_LOANACCO_REFERENCE_FINANCIA]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanAccount]  WITH CHECK ADD  CONSTRAINT [FK_LOANACCO_REFERENCE_FINANCIA] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[FinancialAccount] ([Id])
GO
ALTER TABLE [dbo].[LoanAccount] CHECK CONSTRAINT [FK_LOANACCO_REFERENCE_FINANCIA]
GO
/****** Object:  ForeignKey [FK_LoanAccount_InterestType]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanAccount]  WITH CHECK ADD  CONSTRAINT [FK_LoanAccount_InterestType] FOREIGN KEY([InterestTypeId])
REFERENCES [dbo].[InterestType] ([Id])
GO
ALTER TABLE [dbo].[LoanAccount] CHECK CONSTRAINT [FK_LoanAccount_InterestType]
GO
/****** Object:  ForeignKey [FK_LAND_REFERENCE_ASSET]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Land]  WITH CHECK ADD  CONSTRAINT [FK_LAND_REFERENCE_ASSET] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([Id])
GO
ALTER TABLE [dbo].[Land] CHECK CONSTRAINT [FK_LAND_REFERENCE_ASSET]
GO
/****** Object:  ForeignKey [FK_LAND_REFERENCE_LANDTYPE]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Land]  WITH CHECK ADD  CONSTRAINT [FK_LAND_REFERENCE_LANDTYPE] FOREIGN KEY([LandTypeId])
REFERENCES [dbo].[LandType] ([Id])
GO
ALTER TABLE [dbo].[Land] CHECK CONSTRAINT [FK_LAND_REFERENCE_LANDTYPE]
GO
/****** Object:  ForeignKey [FK_LAND_REFERENCE_UNITOFME]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Land]  WITH CHECK ADD  CONSTRAINT [FK_LAND_REFERENCE_UNITOFME] FOREIGN KEY([UomId])
REFERENCES [dbo].[UnitOfMeasure] ([Id])
GO
ALTER TABLE [dbo].[Land] CHECK CONSTRAINT [FK_LAND_REFERENCE_UNITOFME]
GO
/****** Object:  ForeignKey [FK_FormDetails_FormType]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FormDetails]  WITH CHECK ADD  CONSTRAINT [FK_FormDetails_FormType] FOREIGN KEY([FormTypeId])
REFERENCES [dbo].[FormType] ([Id])
GO
ALTER TABLE [dbo].[FormDetails] CHECK CONSTRAINT [FK_FormDetails_FormType]
GO
/****** Object:  ForeignKey [FK_FormDetails_LoanApplication]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FormDetails]  WITH CHECK ADD  CONSTRAINT [FK_FormDetails_LoanApplication] FOREIGN KEY([LoanAppId])
REFERENCES [dbo].[LoanApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[FormDetails] CHECK CONSTRAINT [FK_FormDetails_LoanApplication]
GO
/****** Object:  ForeignKey [FK_FormDetails_Payment]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FormDetails]  WITH CHECK ADD  CONSTRAINT [FK_FormDetails_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[FormDetails] CHECK CONSTRAINT [FK_FormDetails_Payment]
GO
/****** Object:  ForeignKey [FK_ForeignExchangeDetailAssoc_ForeignExchange]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ForeignExchangeDetailAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ForeignExchangeDetailAssoc_ForeignExchange] FOREIGN KEY([ForExId])
REFERENCES [dbo].[ForeignExchange] ([Id])
GO
ALTER TABLE [dbo].[ForeignExchangeDetailAssoc] CHECK CONSTRAINT [FK_ForeignExchangeDetailAssoc_ForeignExchange]
GO
/****** Object:  ForeignKey [FK_ForeignExchangeDetailAssoc_ForExDetail]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ForeignExchangeDetailAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ForeignExchangeDetailAssoc_ForExDetail] FOREIGN KEY([ForExDetailId])
REFERENCES [dbo].[ForExDetail] ([Id])
GO
ALTER TABLE [dbo].[ForeignExchangeDetailAssoc] CHECK CONSTRAINT [FK_ForeignExchangeDetailAssoc_ForExDetail]
GO
/****** Object:  ForeignKey [FK_LOANMODI_LOANMODIF_LOANMODI]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanModificationStatus]  WITH CHECK ADD  CONSTRAINT [FK_LOANMODI_LOANMODIF_LOANMODI] FOREIGN KEY([LoanModificationId])
REFERENCES [dbo].[LoanModification] ([Id])
GO
ALTER TABLE [dbo].[LoanModificationStatus] CHECK CONSTRAINT [FK_LOANMODI_LOANMODIF_LOANMODI]
GO
/****** Object:  ForeignKey [FK_LOANMODI_REFERENCE_LOANMODI2]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanModificationStatus]  WITH CHECK ADD  CONSTRAINT [FK_LOANMODI_REFERENCE_LOANMODI2] FOREIGN KEY([LoanModificationStatusTypeId])
REFERENCES [dbo].[LoanModificationStatusType] ([Id])
GO
ALTER TABLE [dbo].[LoanModificationStatus] CHECK CONSTRAINT [FK_LOANMODI_REFERENCE_LOANMODI2]
GO
/****** Object:  ForeignKey [FK_LOANMODI_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanModificationStatus]  WITH CHECK ADD  CONSTRAINT [FK_LOANMODI_REFERENCE_PARTYROL] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[LoanModificationStatus] CHECK CONSTRAINT [FK_LOANMODI_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_LOANMODI_REFERENCE_FINANCIA2]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanModificationPrevItems]  WITH CHECK ADD  CONSTRAINT [FK_LOANMODI_REFERENCE_FINANCIA2] FOREIGN KEY([OldFinancialAccountId])
REFERENCES [dbo].[FinancialAccount] ([Id])
GO
ALTER TABLE [dbo].[LoanModificationPrevItems] CHECK CONSTRAINT [FK_LOANMODI_REFERENCE_FINANCIA2]
GO
/****** Object:  ForeignKey [FK_LOANMODI_REFERENCE_LOANMODI3]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanModificationPrevItems]  WITH CHECK ADD  CONSTRAINT [FK_LOANMODI_REFERENCE_LOANMODI3] FOREIGN KEY([LoanModificationId])
REFERENCES [dbo].[LoanModification] ([Id])
GO
ALTER TABLE [dbo].[LoanModificationPrevItems] CHECK CONSTRAINT [FK_LOANMODI_REFERENCE_LOANMODI3]
GO
/****** Object:  ForeignKey [FK_Machine_Asset]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Machine]  WITH CHECK ADD  CONSTRAINT [FK_Machine_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([Id])
GO
ALTER TABLE [dbo].[Machine] CHECK CONSTRAINT [FK_Machine_Asset]
GO
/****** Object:  ForeignKey [FK_LoanPayment_Payment]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanPayment]  WITH CHECK ADD  CONSTRAINT [FK_LoanPayment_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[LoanPayment] CHECK CONSTRAINT [FK_LoanPayment_Payment]
GO
/****** Object:  ForeignKey [FK_POSTALAD_REFERENCE_ADDRESS]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[PostalAddress]  WITH CHECK ADD  CONSTRAINT [FK_POSTALAD_REFERENCE_ADDRESS] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[PostalAddress] CHECK CONSTRAINT [FK_POSTALAD_REFERENCE_ADDRESS]
GO
/****** Object:  ForeignKey [FK_POSTALAD_REFERENCE_COUNTRY]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[PostalAddress]  WITH CHECK ADD  CONSTRAINT [FK_POSTALAD_REFERENCE_COUNTRY] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[PostalAddress] CHECK CONSTRAINT [FK_POSTALAD_REFERENCE_COUNTRY]
GO
/****** Object:  ForeignKey [FK_POSTALAD_REFERENCE_POSTALAD]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[PostalAddress]  WITH CHECK ADD  CONSTRAINT [FK_POSTALAD_REFERENCE_POSTALAD] FOREIGN KEY([PostalAddressTypeId])
REFERENCES [dbo].[PostalAddressType] ([Id])
GO
ALTER TABLE [dbo].[PostalAddress] CHECK CONSTRAINT [FK_POSTALAD_REFERENCE_POSTALAD]
GO
/****** Object:  ForeignKey [FK_LOANSTAT_REFERENCE_LOANACCO]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanStatement]  WITH CHECK ADD  CONSTRAINT [FK_LOANSTAT_REFERENCE_LOANACCO] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[LoanAccount] ([FinancialAccountId])
GO
ALTER TABLE [dbo].[LoanStatement] CHECK CONSTRAINT [FK_LOANSTAT_REFERENCE_LOANACCO]
GO
/****** Object:  ForeignKey [FK_LOANSTAT_REFERENCE_LOANPAYM]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanStatement]  WITH CHECK ADD  CONSTRAINT [FK_LOANSTAT_REFERENCE_LOANPAYM] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[LoanPayment] ([PaymentId])
GO
ALTER TABLE [dbo].[LoanStatement] CHECK CONSTRAINT [FK_LOANSTAT_REFERENCE_LOANPAYM]
GO
/****** Object:  ForeignKey [FK_LOANREAV_REFERENCE_LOANACCO]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanReAvailment]  WITH CHECK ADD  CONSTRAINT [FK_LOANREAV_REFERENCE_LOANACCO] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[LoanAccount] ([FinancialAccountId])
GO
ALTER TABLE [dbo].[LoanReAvailment] CHECK CONSTRAINT [FK_LOANREAV_REFERENCE_LOANACCO]
GO
/****** Object:  ForeignKey [FK_LOANREAV_REFERENCE_LOANAPPL]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanReAvailment]  WITH CHECK ADD  CONSTRAINT [FK_LOANREAV_REFERENCE_LOANAPPL] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[LoanApplication] ([ApplicationId])
GO
ALTER TABLE [dbo].[LoanReAvailment] CHECK CONSTRAINT [FK_LOANREAV_REFERENCE_LOANAPPL]
GO
/****** Object:  ForeignKey [FK_LOANADJU_REFERENCE_ADJUSTME]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_LOANADJU_REFERENCE_ADJUSTME] FOREIGN KEY([AdjustmentTypeId])
REFERENCES [dbo].[AdjustmentType] ([Id])
GO
ALTER TABLE [dbo].[LoanAdjustment] CHECK CONSTRAINT [FK_LOANADJU_REFERENCE_ADJUSTME]
GO
/****** Object:  ForeignKey [FK_LOANADJU_REFERENCE_LOANACCO]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_LOANADJU_REFERENCE_LOANACCO] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[LoanAccount] ([FinancialAccountId])
GO
ALTER TABLE [dbo].[LoanAdjustment] CHECK CONSTRAINT [FK_LOANADJU_REFERENCE_LOANACCO]
GO
/****** Object:  ForeignKey [FK_LOANMODI_REFERENCE_FINANCIA]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanModificationNewItems]  WITH CHECK ADD  CONSTRAINT [FK_LOANMODI_REFERENCE_FINANCIA] FOREIGN KEY([NewFinancialAccountId])
REFERENCES [dbo].[FinancialAccount] ([Id])
GO
ALTER TABLE [dbo].[LoanModificationNewItems] CHECK CONSTRAINT [FK_LOANMODI_REFERENCE_FINANCIA]
GO
/****** Object:  ForeignKey [FK_LOANMODI_REFERENCE_LOANMODI]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanModificationNewItems]  WITH CHECK ADD  CONSTRAINT [FK_LOANMODI_REFERENCE_LOANMODI] FOREIGN KEY([LoanModificationPrevId])
REFERENCES [dbo].[LoanModificationPrevItems] ([Id])
GO
ALTER TABLE [dbo].[LoanModificationNewItems] CHECK CONSTRAINT [FK_LOANMODI_REFERENCE_LOANMODI]
GO
/****** Object:  ForeignKey [FK_LOANDISB_REFERENCE_DISBURSE2]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanDisbursement]  WITH CHECK ADD  CONSTRAINT [FK_LOANDISB_REFERENCE_DISBURSE2] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Disbursement] ([PaymentId])
GO
ALTER TABLE [dbo].[LoanDisbursement] CHECK CONSTRAINT [FK_LOANDISB_REFERENCE_DISBURSE2]
GO
/****** Object:  ForeignKey [FK_LoanDisbursement_LoanDisbursementType]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanDisbursement]  WITH CHECK ADD  CONSTRAINT [FK_LoanDisbursement_LoanDisbursementType] FOREIGN KEY([LoanDisbursementTypeId])
REFERENCES [dbo].[LoanDisbursementType] ([Id])
GO
ALTER TABLE [dbo].[LoanDisbursement] CHECK CONSTRAINT [FK_LoanDisbursement_LoanDisbursementType]
GO
/****** Object:  ForeignKey [FK_InterestItems_LoanAccount]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[InterestItems]  WITH CHECK ADD  CONSTRAINT [FK_InterestItems_LoanAccount] FOREIGN KEY([LoanId])
REFERENCES [dbo].[LoanAccount] ([FinancialAccountId])
GO
ALTER TABLE [dbo].[InterestItems] CHECK CONSTRAINT [FK_InterestItems_LoanAccount]
GO
/****** Object:  ForeignKey [FK_LOAN_ACC_REFERENCE_LOAN_ACC2]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanAccountStatus]  WITH CHECK ADD  CONSTRAINT [FK_LOAN_ACC_REFERENCE_LOAN_ACC2] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[LoanAccount] ([FinancialAccountId])
GO
ALTER TABLE [dbo].[LoanAccountStatus] CHECK CONSTRAINT [FK_LOAN_ACC_REFERENCE_LOAN_ACC2]
GO
/****** Object:  ForeignKey [FK_LOAN_ACC_REFERENCE_LOAN_ACC3]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[LoanAccountStatus]  WITH CHECK ADD  CONSTRAINT [FK_LOAN_ACC_REFERENCE_LOAN_ACC3] FOREIGN KEY([StatusTypeId])
REFERENCES [dbo].[LoanAccountStatusType] ([Id])
GO
ALTER TABLE [dbo].[LoanAccountStatus] CHECK CONSTRAINT [FK_LOAN_ACC_REFERENCE_LOAN_ACC3]
GO
/****** Object:  ForeignKey [FK_AMORTIZA_REFERENCE_AMORTIZA]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[AmortizationScheduleItem]  WITH CHECK ADD  CONSTRAINT [FK_AMORTIZA_REFERENCE_AMORTIZA] FOREIGN KEY([AmortizationScheduleId])
REFERENCES [dbo].[AmortizationSchedule] ([Id])
GO
ALTER TABLE [dbo].[AmortizationScheduleItem] CHECK CONSTRAINT [FK_AMORTIZA_REFERENCE_AMORTIZA]
GO
/****** Object:  ForeignKey [FK_CHEQUEST_REFERENCE_CHEQUE]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ChequeStatus]  WITH CHECK ADD  CONSTRAINT [FK_CHEQUEST_REFERENCE_CHEQUE] FOREIGN KEY([CheckId])
REFERENCES [dbo].[Cheque] ([Id])
GO
ALTER TABLE [dbo].[ChequeStatus] CHECK CONSTRAINT [FK_CHEQUEST_REFERENCE_CHEQUE]
GO
/****** Object:  ForeignKey [FK_CHEQUEST_REFERENCE_CHEQUEST]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ChequeStatus]  WITH CHECK ADD  CONSTRAINT [FK_CHEQUEST_REFERENCE_CHEQUEST] FOREIGN KEY([CheckStatusTypeId])
REFERENCES [dbo].[ChequeStatusType] ([Id])
GO
ALTER TABLE [dbo].[ChequeStatus] CHECK CONSTRAINT [FK_CHEQUEST_REFERENCE_CHEQUEST]
GO
/****** Object:  ForeignKey [FK_ChequeLoanAssoc_Cheque]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ChequeLoanAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ChequeLoanAssoc_Cheque] FOREIGN KEY([ChequeId])
REFERENCES [dbo].[Cheque] ([Id])
GO
ALTER TABLE [dbo].[ChequeLoanAssoc] CHECK CONSTRAINT [FK_ChequeLoanAssoc_Cheque]
GO
/****** Object:  ForeignKey [FK_ChequeLoanAssoc_LoanAccount]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ChequeLoanAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ChequeLoanAssoc_LoanAccount] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[LoanAccount] ([FinancialAccountId])
GO
ALTER TABLE [dbo].[ChequeLoanAssoc] CHECK CONSTRAINT [FK_ChequeLoanAssoc_LoanAccount]
GO
/****** Object:  ForeignKey [FK_ChequeApplicationAssoc_Application]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ChequeApplicationAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ChequeApplicationAssoc_Application] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[ChequeApplicationAssoc] CHECK CONSTRAINT [FK_ChequeApplicationAssoc_Application]
GO
/****** Object:  ForeignKey [FK_ChequeApplicationAssoc_Cheque]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ChequeApplicationAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ChequeApplicationAssoc_Cheque] FOREIGN KEY([ChequeId])
REFERENCES [dbo].[Cheque] ([Id])
GO
ALTER TABLE [dbo].[ChequeApplicationAssoc] CHECK CONSTRAINT [FK_ChequeApplicationAssoc_Cheque]
GO
/****** Object:  ForeignKey [FK_DISBURSE_REFERENCE_DISBURSE]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[DisbursementItem]  WITH CHECK ADD  CONSTRAINT [FK_DISBURSE_REFERENCE_DISBURSE] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Disbursement] ([PaymentId])
GO
ALTER TABLE [dbo].[DisbursementItem] CHECK CONSTRAINT [FK_DISBURSE_REFERENCE_DISBURSE]
GO
/****** Object:  ForeignKey [FK_DEMANDLE_REFERENCE_LOANACCO]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[DemandLetter]  WITH CHECK ADD  CONSTRAINT [FK_DEMANDLE_REFERENCE_LOANACCO] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[LoanAccount] ([FinancialAccountId])
GO
ALTER TABLE [dbo].[DemandLetter] CHECK CONSTRAINT [FK_DEMANDLE_REFERENCE_LOANACCO]
GO
/****** Object:  ForeignKey [FK_FINL_ACC_REFERENCE_FIN_ACCT2]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FinlAcctTransnStatus]  WITH CHECK ADD  CONSTRAINT [FK_FINL_ACC_REFERENCE_FIN_ACCT2] FOREIGN KEY([StatusTypeId])
REFERENCES [dbo].[FinAcctTransStatusType] ([Id])
GO
ALTER TABLE [dbo].[FinlAcctTransnStatus] CHECK CONSTRAINT [FK_FINL_ACC_REFERENCE_FIN_ACCT2]
GO
/****** Object:  ForeignKey [FK_FINLACCT_REFERENCE_FINACCTT]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FinlAcctTransnStatus]  WITH CHECK ADD  CONSTRAINT [FK_FINLACCT_REFERENCE_FINACCTT] FOREIGN KEY([FinancialAcctTransactionId])
REFERENCES [dbo].[FinAcctTrans] ([Id])
GO
ALTER TABLE [dbo].[FinlAcctTransnStatus] CHECK CONSTRAINT [FK_FINLACCT_REFERENCE_FINACCTT]
GO
/****** Object:  ForeignKey [FK_FINANCIA_REFERENCE_FINACCTT]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FinancialAcctNotification]  WITH CHECK ADD  CONSTRAINT [FK_FINANCIA_REFERENCE_FINACCTT] FOREIGN KEY([FinancialAcctTransactionId])
REFERENCES [dbo].[FinAcctTrans] ([Id])
GO
ALTER TABLE [dbo].[FinancialAcctNotification] CHECK CONSTRAINT [FK_FINANCIA_REFERENCE_FINACCTT]
GO
/****** Object:  ForeignKey [FK_FINANCIA_REFERENCE_FINANCIA]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FinancialAcctNotification]  WITH CHECK ADD  CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA] FOREIGN KEY([FinancialAcctNotificationId])
REFERENCES [dbo].[FinancialAcctNotificationTyp] ([Id])
GO
ALTER TABLE [dbo].[FinancialAcctNotification] CHECK CONSTRAINT [FK_FINANCIA_REFERENCE_FINANCIA]
GO
/****** Object:  ForeignKey [FK_FINACCTT_REFERENCE_FINACCTT]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FinAcctTransTask]  WITH CHECK ADD  CONSTRAINT [FK_FINACCTT_REFERENCE_FINACCTT] FOREIGN KEY([FinancialAcctTransactionId])
REFERENCES [dbo].[FinAcctTrans] ([Id])
GO
ALTER TABLE [dbo].[FinAcctTransTask] CHECK CONSTRAINT [FK_FINACCTT_REFERENCE_FINACCTT]
GO
/****** Object:  ForeignKey [FK_FINACCTT_REFERENCE_TRANSACT]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FinAcctTransTask]  WITH CHECK ADD  CONSTRAINT [FK_FINACCTT_REFERENCE_TRANSACT] FOREIGN KEY([TransactionTaskTypeId])
REFERENCES [dbo].[TransactionTaskType] ([Id])
GO
ALTER TABLE [dbo].[FinAcctTransTask] CHECK CONSTRAINT [FK_FINACCTT_REFERENCE_TRANSACT]
GO
/****** Object:  ForeignKey [FK_FINACCTT_REFERENCE_UNITOFME]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FinAcctTransTask]  WITH CHECK ADD  CONSTRAINT [FK_FINACCTT_REFERENCE_UNITOFME] FOREIGN KEY([UomId])
REFERENCES [dbo].[UnitOfMeasure] ([Id])
GO
ALTER TABLE [dbo].[FinAcctTransTask] CHECK CONSTRAINT [FK_FINACCTT_REFERENCE_UNITOFME]
GO
/****** Object:  ForeignKey [FK_ELECTRON_REFERENCE_ADDRESS]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ElectronicAddress]  WITH CHECK ADD  CONSTRAINT [FK_ELECTRON_REFERENCE_ADDRESS] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[ElectronicAddress] CHECK CONSTRAINT [FK_ELECTRON_REFERENCE_ADDRESS]
GO
/****** Object:  ForeignKey [FK_ELECTRON_REFERENCE_ELECTRON]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ElectronicAddress]  WITH CHECK ADD  CONSTRAINT [FK_ELECTRON_REFERENCE_ELECTRON] FOREIGN KEY([ElectronicAddressTypeId])
REFERENCES [dbo].[ElectronicAddressType] ([Id])
GO
ALTER TABLE [dbo].[ElectronicAddress] CHECK CONSTRAINT [FK_ELECTRON_REFERENCE_ELECTRON]
GO
/****** Object:  ForeignKey [FK_ENCASHME_REFERENCE_DISBURSE]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Encashment]  WITH CHECK ADD  CONSTRAINT [FK_ENCASHME_REFERENCE_DISBURSE] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Disbursement] ([PaymentId])
GO
ALTER TABLE [dbo].[Encashment] CHECK CONSTRAINT [FK_ENCASHME_REFERENCE_DISBURSE]
GO
/****** Object:  ForeignKey [FK_FIN_ACCT_REFERENCE_FIN_ACCT2]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FinAcctTransRel]  WITH CHECK ADD  CONSTRAINT [FK_FIN_ACCT_REFERENCE_FIN_ACCT2] FOREIGN KEY([FinancialAcctTransRelTypeId])
REFERENCES [dbo].[FinAcctTransRelType] ([Id])
GO
ALTER TABLE [dbo].[FinAcctTransRel] CHECK CONSTRAINT [FK_FIN_ACCT_REFERENCE_FIN_ACCT2]
GO
/****** Object:  ForeignKey [FK_FIN_ACCT_REFERENCE_FIN_ACCT3]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FinAcctTransRel]  WITH CHECK ADD  CONSTRAINT [FK_FIN_ACCT_REFERENCE_FIN_ACCT3] FOREIGN KEY([FromFinancialAcctTransactionId])
REFERENCES [dbo].[FinAcctTrans] ([Id])
GO
ALTER TABLE [dbo].[FinAcctTransRel] CHECK CONSTRAINT [FK_FIN_ACCT_REFERENCE_FIN_ACCT3]
GO
/****** Object:  ForeignKey [FK_FIN_ACCT_REFERENCE_FIN_ACCT4]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[FinAcctTransRel]  WITH CHECK ADD  CONSTRAINT [FK_FIN_ACCT_REFERENCE_FIN_ACCT4] FOREIGN KEY([ToFinancialAcctTransactionId])
REFERENCES [dbo].[FinAcctTrans] ([Id])
GO
ALTER TABLE [dbo].[FinAcctTransRel] CHECK CONSTRAINT [FK_FIN_ACCT_REFERENCE_FIN_ACCT4]
GO
/****** Object:  ForeignKey [FK_TELECOMM_REFERENCE_ADDRESS]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[TelecommunicationsNumber]  WITH CHECK ADD  CONSTRAINT [FK_TELECOMM_REFERENCE_ADDRESS] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[TelecommunicationsNumber] CHECK CONSTRAINT [FK_TELECOMM_REFERENCE_ADDRESS]
GO
/****** Object:  ForeignKey [FK_TELECOMM_REFERENCE_TELECOMM]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[TelecommunicationsNumber]  WITH CHECK ADD  CONSTRAINT [FK_TELECOMM_REFERENCE_TELECOMM] FOREIGN KEY([TypeId])
REFERENCES [dbo].[TelecommunicationsNumberType] ([Id])
GO
ALTER TABLE [dbo].[TelecommunicationsNumber] CHECK CONSTRAINT [FK_TELECOMM_REFERENCE_TELECOMM]
GO
/****** Object:  ForeignKey [FK_RECEIVAB_REFERENCE_LOANACCO]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Receivable]  WITH CHECK ADD  CONSTRAINT [FK_RECEIVAB_REFERENCE_LOANACCO] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[LoanAccount] ([FinancialAccountId])
GO
ALTER TABLE [dbo].[Receivable] CHECK CONSTRAINT [FK_RECEIVAB_REFERENCE_LOANACCO]
GO
/****** Object:  ForeignKey [FK_RECEIVAB_REFERENCE_RECEIVAB2]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Receivable]  WITH CHECK ADD  CONSTRAINT [FK_RECEIVAB_REFERENCE_RECEIVAB2] FOREIGN KEY([ReceivableTypeId])
REFERENCES [dbo].[ReceivableType] ([Id])
GO
ALTER TABLE [dbo].[Receivable] CHECK CONSTRAINT [FK_RECEIVAB_REFERENCE_RECEIVAB2]
GO
/****** Object:  ForeignKey [FK_Receivable_Disbursement]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[Receivable]  WITH CHECK ADD  CONSTRAINT [FK_Receivable_Disbursement] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Disbursement] ([PaymentId])
GO
ALTER TABLE [dbo].[Receivable] CHECK CONSTRAINT [FK_Receivable_Disbursement]
GO
/****** Object:  ForeignKey [FK_RECEIVAB_REFERENCE_RECEIVAB]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ReceivableStatus]  WITH CHECK ADD  CONSTRAINT [FK_RECEIVAB_REFERENCE_RECEIVAB] FOREIGN KEY([StatusTypeId])
REFERENCES [dbo].[ReceivableStatusType] ([Id])
GO
ALTER TABLE [dbo].[ReceivableStatus] CHECK CONSTRAINT [FK_RECEIVAB_REFERENCE_RECEIVAB]
GO
/****** Object:  ForeignKey [FK_RECEIVAB_REFERENCE_RECEIVAB4]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ReceivableStatus]  WITH CHECK ADD  CONSTRAINT [FK_RECEIVAB_REFERENCE_RECEIVAB4] FOREIGN KEY([ReceivableId])
REFERENCES [dbo].[Receivable] ([Id])
GO
ALTER TABLE [dbo].[ReceivableStatus] CHECK CONSTRAINT [FK_RECEIVAB_REFERENCE_RECEIVAB4]
GO
/****** Object:  ForeignKey [FK_RECEIVAB_REFERENCE_ADJUSTME]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ReceivableAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_RECEIVAB_REFERENCE_ADJUSTME] FOREIGN KEY([AdjustmentTypeId])
REFERENCES [dbo].[AdjustmentType] ([Id])
GO
ALTER TABLE [dbo].[ReceivableAdjustment] CHECK CONSTRAINT [FK_RECEIVAB_REFERENCE_ADJUSTME]
GO
/****** Object:  ForeignKey [FK_RECEIVAB_REFERENCE_PARTYROL]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ReceivableAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_RECEIVAB_REFERENCE_PARTYROL] FOREIGN KEY([PartyRoleId])
REFERENCES [dbo].[PartyRole] ([Id])
GO
ALTER TABLE [dbo].[ReceivableAdjustment] CHECK CONSTRAINT [FK_RECEIVAB_REFERENCE_PARTYROL]
GO
/****** Object:  ForeignKey [FK_RECEIVAB_REFERENCE_RECEIVAB3]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ReceivableAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_RECEIVAB_REFERENCE_RECEIVAB3] FOREIGN KEY([ReceivableId])
REFERENCES [dbo].[Receivable] ([Id])
GO
ALTER TABLE [dbo].[ReceivableAdjustment] CHECK CONSTRAINT [FK_RECEIVAB_REFERENCE_RECEIVAB3]
GO
/****** Object:  ForeignKey [FK_RELEASES_REFERENCE_LOANACCO]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ReleaseStatement]  WITH CHECK ADD  CONSTRAINT [FK_RELEASES_REFERENCE_LOANACCO] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[LoanAccount] ([FinancialAccountId])
GO
ALTER TABLE [dbo].[ReleaseStatement] CHECK CONSTRAINT [FK_RELEASES_REFERENCE_LOANACCO]
GO
/****** Object:  ForeignKey [FK_RELEASES_REFERENCE_LOANDISB]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[ReleaseStatement]  WITH CHECK ADD  CONSTRAINT [FK_RELEASES_REFERENCE_LOANDISB] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[LoanDisbursement] ([PaymentId])
GO
ALTER TABLE [dbo].[ReleaseStatement] CHECK CONSTRAINT [FK_RELEASES_REFERENCE_LOANDISB]
GO
/****** Object:  ForeignKey [FK_PAYMENTA_REFERENCE_LOANDISB]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[PaymentApplication]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENTA_REFERENCE_LOANDISB] FOREIGN KEY([LoanDisbursementVoucherId])
REFERENCES [dbo].[LoanDisbursementVcr] ([Id])
GO
ALTER TABLE [dbo].[PaymentApplication] CHECK CONSTRAINT [FK_PAYMENTA_REFERENCE_LOANDISB]
GO
/****** Object:  ForeignKey [FK_PAYMENTA_REFERENCE_PAYMENT]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[PaymentApplication]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENTA_REFERENCE_PAYMENT] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[PaymentApplication] CHECK CONSTRAINT [FK_PAYMENTA_REFERENCE_PAYMENT]
GO
/****** Object:  ForeignKey [FK_PAYMENTA_REFERENCE_RECEIVAB]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[PaymentApplication]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENTA_REFERENCE_RECEIVAB] FOREIGN KEY([ReceivableId])
REFERENCES [dbo].[Receivable] ([Id])
GO
ALTER TABLE [dbo].[PaymentApplication] CHECK CONSTRAINT [FK_PAYMENTA_REFERENCE_RECEIVAB]
GO
/****** Object:  ForeignKey [FK_PaymentApplication_LoanAccount]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[PaymentApplication]  WITH CHECK ADD  CONSTRAINT [FK_PaymentApplication_LoanAccount] FOREIGN KEY([FinancialAccountId])
REFERENCES [dbo].[LoanAccount] ([FinancialAccountId])
GO
ALTER TABLE [dbo].[PaymentApplication] CHECK CONSTRAINT [FK_PaymentApplication_LoanAccount]
GO
/****** Object:  ForeignKey [FK_DEMANDLE_REFERENCE_DEMANDLE]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[DemandLetterStatus]  WITH CHECK ADD  CONSTRAINT [FK_DEMANDLE_REFERENCE_DEMANDLE] FOREIGN KEY([DemandLetterId])
REFERENCES [dbo].[DemandLetter] ([Id])
GO
ALTER TABLE [dbo].[DemandLetterStatus] CHECK CONSTRAINT [FK_DEMANDLE_REFERENCE_DEMANDLE]
GO
/****** Object:  ForeignKey [FK_DEMANDLE_REFERENCE_DEMANDLES]    Script Date: 02/22/2012 10:28:13 ******/
ALTER TABLE [dbo].[DemandLetterStatus]  WITH CHECK ADD  CONSTRAINT [FK_DEMANDLE_REFERENCE_DEMANDLES] FOREIGN KEY([DemandLetterStatusTypeId])
REFERENCES [dbo].[DemandLetterStatusType] ([Id])
GO
ALTER TABLE [dbo].[DemandLetterStatus] CHECK CONSTRAINT [FK_DEMANDLE_REFERENCE_DEMANDLES]
GO
