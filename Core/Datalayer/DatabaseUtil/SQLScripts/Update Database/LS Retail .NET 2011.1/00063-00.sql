/*

	Incident No.	: N/A
	Responsible		: Tobias Helmer
	Sprint			: 2011 - Store Controller 2.0.2 - Sprint 1
	Date created	: 28.03.2011

	Description		: table addition (TAXCOLLECTLIMIT) and table updates for U.S. tax implementation

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: TAXTABLE, TAXGROUPHEADING, TAXCOLLECTLIMIT, RBOSTORETABLE, RBOINVENTTABLE
						
*/

USE LSPOSNET

GO

--TAXTABLE

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXTABLE') AND NAME='TAXBASE')
BEGIN
	ALTER TABLE [dbo].[TAXTABLE] ADD TAXBASE int NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXTABLE') AND NAME='TAXCALCMETHOD')
BEGIN
	ALTER TABLE [dbo].[TAXTABLE] ADD TAXCALCMETHOD int NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXTABLE') AND NAME='TAXCURRENCYCODE')
BEGIN
	ALTER TABLE [dbo].[TAXTABLE] ADD TAXCURRENCYCODE nvarchar(3) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXTABLE') AND NAME='TAXLIMITBASE')
BEGIN
	ALTER TABLE [dbo].[TAXTABLE] ADD TAXLIMITBASE int NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXTABLE') AND NAME='TAXONTAX')
BEGIN
	ALTER TABLE [dbo].[TAXTABLE] ADD TAXONTAX nvarchar(20) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXTABLE') AND NAME='TAXUNIT')
BEGIN
	ALTER TABLE [dbo].[TAXTABLE] ADD TAXUNIT nvarchar(20) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXTABLE') AND NAME='TAXPERIOD')
BEGIN
	ALTER TABLE [dbo].[TAXTABLE] ADD TAXPERIOD nvarchar(20) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXTABLE') AND NAME='TAXACCOUNTGROUP')
BEGIN
	ALTER TABLE [dbo].[TAXTABLE] ADD TAXACCOUNTGROUP nvarchar(20) NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXTABLE') AND NAME='TAXINCLUDEINTAX')
BEGIN
	ALTER TABLE [dbo].[TAXTABLE] ADD TAXINCLUDEINTAX tinyint NULL;
	ALTER TABLE [dbo].[TAXTABLE] ADD  DEFAULT ((0)) FOR [TAXINCLUDEINTAX]
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXTABLE') AND NAME='TAXPACKAGINGTAX')
BEGIN
	ALTER TABLE [dbo].[TAXTABLE] ADD TAXPACKAGINGTAX tinyint NULL;
	ALTER TABLE [dbo].[TAXTABLE] ADD  DEFAULT ((0)) FOR [TAXPACKAGINGTAX]
END
GO

--TAXGROUPHEADING

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXGROUPHEADING') AND NAME='TAXGROUPROUNDING')
BEGIN
	ALTER TABLE [dbo].[TAXGROUPHEADING] ADD TAXGROUPROUNDING int NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXGROUPHEADING') AND NAME='TAXGROUPSETUP')
BEGIN
	ALTER TABLE [dbo].[TAXGROUPHEADING] ADD TAXGROUPSETUP int NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXGROUPHEADING') AND NAME='TAXREVERSEONCASHDISC')
BEGIN
	ALTER TABLE [dbo].[TAXGROUPHEADING] ADD TAXREVERSEONCASHDISC int NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXGROUPHEADING') AND NAME='TAXPRINTDETAIL')
BEGIN
	ALTER TABLE [dbo].[TAXGROUPHEADING] ADD TAXPRINTDETAIL int NULL;
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[TAXCOLLECTLIMIT]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[TAXCOLLECTLIMIT](
	[TAXCODE] [nvarchar](10) NOT NULL,
	[TAXMAX] [numeric](28, 12) NOT NULL,
	[TAXMIN] [numeric](28, 12) NOT NULL,
	[TAXTODATE] [datetime] NOT NULL,
	[TAXFROMDATE] [datetime] NOT NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[RECVERSION] [int] NOT NULL,
 CONSTRAINT [I_428TAXCODEIDX] PRIMARY KEY CLUSTERED 
(
	[DATAAREAID] ASC,
	[TAXCODE] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[TAXCOLLECTLIMIT] ADD  DEFAULT ('') FOR [TAXCODE]
ALTER TABLE [dbo].[TAXCOLLECTLIMIT] ADD  DEFAULT ((0)) FOR [TAXMAX]
ALTER TABLE [dbo].[TAXCOLLECTLIMIT] ADD  DEFAULT ((0)) FOR [TAXMIN]
ALTER TABLE [dbo].[TAXCOLLECTLIMIT] ADD  DEFAULT ('1900-01-01 00:00:00.000') FOR [TAXTODATE]
ALTER TABLE [dbo].[TAXCOLLECTLIMIT] ADD  DEFAULT ('1900-01-01 00:00:00.000') FOR [TAXFROMDATE]
ALTER TABLE [dbo].[TAXCOLLECTLIMIT] ADD  DEFAULT ('LSR') FOR [DATAAREAID]
ALTER TABLE [dbo].[TAXCOLLECTLIMIT] ADD  DEFAULT ((1)) FOR [RECVERSION]
END
GO

--RBOSTORETABLE

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOSTORETABLE') AND NAME='PRICEINCLUDESALESTAX')
BEGIN
	ALTER TABLE [dbo].[RBOSTORETABLE] ADD PRICEINCLUDESALESTAX int NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOSTORETABLE') AND NAME='TAXOVERRIDEGROUP')
BEGIN
	ALTER TABLE [dbo].[RBOSTORETABLE] ADD TAXOVERRIDEGROUP nvarchar(25) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOSTORETABLE') AND NAME='USEDESTINATIONBASEDTAX')
BEGIN
	ALTER TABLE [dbo].[RBOSTORETABLE] ADD USEDESTINATIONBASEDTAX int NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOSTORETABLE') AND NAME='USECUSTOMERBASEDTAX')
BEGIN
	ALTER TABLE [dbo].[RBOSTORETABLE] ADD USECUSTOMERBASEDTAX int NULL;
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOSTORETABLE') AND NAME='TAXIDENTIFICATIONNUMBER')
BEGIN
	ALTER TABLE [dbo].[RBOSTORETABLE] ADD TAXIDENTIFICATIONNUMBER nvarchar(25) NULL;
END
GO

--RBOINVENTTABLE
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINVENTTABLE') AND NAME='BUSINESSGROUP')
BEGIN
	ALTER TABLE [dbo].[RBOINVENTTABLE] ADD BUSINESSGROUP nvarchar(20);
END
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINVENTTABLE') AND NAME='DIVISIONGROUP')
BEGIN
	ALTER TABLE [dbo].[RBOINVENTTABLE] ADD DIVISIONGROUP nvarchar(20) NULL;
END
GO