
/*

	Incident No.	: N/A
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: N/A
	Date created	: 22.12.2010

	Description		: Add a new field in RboParameters for a default dimension value - used for Trade Agreement calculations

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RboParameters - field added
						
*/

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOPARAMETERS') AND NAME='DEFAULTDIMENSION')
BEGIN
	ALTER TABLE [dbo].[RBOPARAMETERS] ADD DEFAULTDIMENSION NVARCHAR(20) NULL
END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RBOPARAMETERS_DEFAULTDIMENSION]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[RBOPARAMETERS] DROP CONSTRAINT [DF_RBOPARAMETERS_DEFAULTDIMENSION]
END

ALTER TABLE [dbo].[RBOPARAMETERS] ADD  CONSTRAINT [DF_RBOPARAMETERS_DEFAULTDIMENSION]  DEFAULT (('')) FOR [DEFAULTDIMENSION]
GO



