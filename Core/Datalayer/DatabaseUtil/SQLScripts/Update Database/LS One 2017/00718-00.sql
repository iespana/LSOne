/*

	Incident No.	: ONE-4639
	Responsible		: Mar� Bj�rk Steingr�msd�ttir
	Sprint			: Turmeric 1.12 - 15.12
	Date created	: 12.12.2016

	Description		: Adding new configurations site service profile and a new column to receipt saving
	
	
	Tables affected	: RBOTRANSACTIONRECEIPTS, POSTRANSACTIONSERVICEPROFILESHORTHAND
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONRECEIPTS' AND COLUMN_NAME = 'FORMWIDTH')
BEGIN
	ALTER TABLE dbo.RBOTRANSACTIONRECEIPTS ADD FORMWIDTH INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('POSTRANSACTIONSERVICEPROFILESHORTHAND') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[POSTRANSACTIONSERVICEPROFILESHORTHAND](
		[MASTERID] [uniqueidentifier] NOT NULL,
		[PROFILEID] [nvarchar](20) NOT NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
		[SHORTHANDTYPE] [tinyint] NOT NULL,
		[SHORTHAND] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_POSTRANSACTIONSERVICEPROFILESHORTHAND] PRIMARY KEY CLUSTERED 
	(
		[MASTERID] ASC,
		[DATAAREAID] ASC		
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO