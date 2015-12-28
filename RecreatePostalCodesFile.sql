/****** Object:  Table [dbo].[2013-09-18 Gatuadresser-postnummer]    Script Date: 2015-12-05 01:28:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

if not exists (select * from @TARGET_DATABASE.sys.tables where name = '2013-09-18 Gatuadresser-postnummer')
	CREATE TABLE @TARGET_DATABASE.[dbo].[2013-09-18 Gatuadresser-postnummer](
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

if('@TARGET_DATABASE' != 'eriks_test_db')
	insert into @TARGET_DATABASE.dbo.[2013-09-18 Gatuadresser-postnummer] 
	select * from eriks_test_db.dbo.[2013-09-18 Gatuadresser-postnummer]
go


set identity_insert @TARGET_DATABASE.dbo.PostalCodeModels on
insert into @TARGET_DATABASE.dbo.PostalCodeModels(
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
	,[Column 15],[Column 17],0 
from @TARGET_DATABASE.dbo.[2013-09-18 Gatuadresser-postnummer]

set identity_insert @TARGET_DATABASE.dbo.PostalCodeModels off