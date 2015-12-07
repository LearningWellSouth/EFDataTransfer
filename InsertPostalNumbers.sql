USE [DATABASE_NAME]
GO
if exists (select * from sys.tables where name = '2013-09-18 Gatuadresser-postnummer')
	drop table [dbo].[2013-09-18 Gatuadresser-postnummer]
GO
/****** Object:  Table [dbo].[2013-09-18 Gatuadresser-postnummer]    Script Date: 2015-12-05 01:28:30 ******/
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
INSERT [dbo].[2013-09-18 Gatuadresser-postnummer] ([Column 0], [Column 1], [Column 2], [Column 3], [Column 4], [Column 5], [Column 6], [Column 7], [Column 8], [Column 9], [Column 10], [Column 11], [Column 12], [Column 13], [Column 14], [Column 15], [Column 16], [Column 17]) VALUES (N'AR', N'LEVERANSPUNKT', N'', N'', N'', N'', N'10005', N'STOCKHOLM', N'AB', N'01', N'STOCKHOLM', N'0188', N'NORRTÄLJE', N'018801', N'NORRTÄLJE-MALSTA', N'', N'02', N'100050514')
INSERT [dbo].[2013-09-18 Gatuadresser-postnummer] ([Column 0], [Column 1], [Column 2], [Column 3], [Column 4], [Column 5], [Column 6], [Column 7], [Column 8], [Column 9], [Column 10], [Column 11], [Column 12], [Column 13], [Column 14], [Column 15], [Column 16], [Column 17]) VALUES (N'AS', N'RIKSDAGEN', N'', N'', N'', N'', N'10012', N'STOCKHOLM', N'AB', N'01', N'STOCKHOLM', N'0180', N'STOCKHOLM', N'018001', N'STOCKHOLMS DOMKYRKOFÖRS.', N'', N'01', N'100120004')
INSERT [dbo].[2013-09-18 Gatuadresser-postnummer] ([Column 0], [Column 1], [Column 2], [Column 3], [Column 4], [Column 5], [Column 6], [Column 7], [Column 8], [Column 9], [Column 10], [Column 11], [Column 12], [Column 13], [Column 14], [Column 15], [Column 16], [Column 17]) VALUES (N'AO', N'BOX', N'     34001', N'', N'     34250', N'', N'10026', N'STOCKHOLM', N'NO', N'01', N'STOCKHOLM', N'0180', N'STOCKHOLM', N'018019', N'S:T GÖRAN', N'', N'01', N'100260008')
... [Actual data from postal codes file]

set identity_insert dbo.PostalCodeModels on;
insert into dbo.PostalCodeModels(
	PostalCodeType,PostalAddress,StreetNoLowest,
	GateLowest,StreetNoHighest,GateHighest,
	PostalCode,City,TypeOfPlacement,
	StateCode,[State],MunicipalityCode,
	Municipality,ParishCode,Parish,
	City2,Id,IsNotValid) 
select [Column 0],[Column 1],[Column 2]
	,[Column 3],[Column 4],[Column 5]
	,[Column 6],[Column 7],[Column 8]
	,[Column 9],[Column 10],[Column 11]
	,[Column 12],[Column 13],[Column 14]
	,[Column 15],[Column 17],1 
from dbo.[2013-09-18 Gatuadresser-postnummer]
drop table [dbo].[2013-09-18 Gatuadresser-postnummer]
