﻿
/*

	Incident No.	: N/A
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: N/A
	Date created	: 10.12.2010

	Description		: Field Comment in RboTransactionSalesTrans needs to be expanded

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RboTransactionSalesTrans	- column altered
						
*/

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RBOTRANSACTIONSALESTRANS_COMMENT]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[RBOTRANSACTIONSALESTRANS] DROP CONSTRAINT [DF_RBOTRANSACTIONSALESTRANS_COMMENT]
END

ALTER TABLE [dbo].[RBOTRANSACTIONSALESTRANS] ALTER COLUMN COMMENT NVARCHAR(MAX)

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RBOTRANSACTIONSALESTRANS_COMMENT]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[RBOTRANSACTIONSALESTRANS] ADD  CONSTRAINT [DF_RBOTRANSACTIONSALESTRANS_COMMENT]  DEFAULT ('') FOR [COMMENT]
END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__RBOTRANSA__ORIGI__6C5CFE90]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[RBOTRANSACTIONSALESTRANS] DROP CONSTRAINT [DF__RBOTRANSA__ORIGI__6C5CFE90]
END

ALTER TABLE [dbo].[RBOTRANSACTIONSALESTRANS] ALTER COLUMN ORIGINALOFLINKEDITEMLIST NUMERIC(28,12)

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__RBOTRANSA__ORIGI__6C5CFE90]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[RBOTRANSACTIONSALESTRANS] ADD  CONSTRAINT [DF__RBOTRANSA__ORIGI__6C5CFE90]  DEFAULT ((0)) FOR [ORIGINALOFLINKEDITEMLIST]
END
GO



