
/*

	Incident No.	: 
	Responsible		: Kristján Ásvaldsson
	Sprint			: 2014
	Date created	: 19.06.2014

	Description		: Created table for tax free configuration
	
						
*/
USE LSPOSNET

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TAXFREECONFIG')
BEGIN
	CREATE TABLE [dbo].[TAXFREECONFIG](
		[KEY_] [int] NOT NULL,	
		[DATAAREAID] [nvarchar](4) NOT NULL,	
		[NAME] [nvarchar](60) NOT NULL,
        [ADDRESS] [nvarchar](250) NOT NULL,
        [POSTCITY] [nvarchar](100) NOT NULL,
        [COUNTRY] [nvarchar](20) NOT NULL,
        [PHONE] [nvarchar](20) NOT NULL,
        [WEB] [nvarchar](250) NULL,
        [VATNUMBER] [nvarchar](20) NOT NULL,
        [PRINTOUTTYPE] [int] NOT NULL,
        [PROMPTCUSTFORINFO] [tinyint] NOT NULL,
        [PROMPTFORPASSPORT] [tinyint] NOT NULL,
        [PROMPTFORFLIGHT] [tinyint] NOT NULL,
        [PROMPTFORREPORT] [tinyint] NOT NULL,
        [PROMPTFORTOURIST] [tinyint] NOT NULL,
        [LINEWIDTH] [int] NOT NULL,
        [DEFAULTPADDING] [int] NOT NULL,
        [MINTAXREFUNDLIMIT] [numeric](28, 12) NULL,
        [TAXNUMBER] [nvarchar](5) NOT NULL,
        [COUNTRYCODE] [nvarchar](2) NOT NULL,
        [FORMTYPE] [nvarchar](1) NOT NULL,
		CONSTRAINT [PK_TAXFREECONFIG] PRIMARY KEY CLUSTERED 
		(
			[DATAAREAID] ASC,
			[KEY_] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]
END
GO