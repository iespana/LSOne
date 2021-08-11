/*

	Incident No.	: [TFS incident no]
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012\Loki
	Date created	: 09.5.2012

	Description		: Updating the RECEIPTID and transactionid number sequence

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: NUMBERSEQUENCETABLE - data modified
						
*/

USE LSPOSNET

IF EXISTS(SELECT * FROM dbo.NUMBERSEQUENCETABLE WHERE NUMBERSEQUENCE = 'RECEIPTID' AND EMBEDSTOREID = 1 )
BEGIN
	update NUMBERSEQUENCETABLE set EMBEDSTOREID = 0 where NUMBERSEQUENCE = 'RECEIPTID'
END
GO

IF EXISTS(SELECT * FROM dbo.NUMBERSEQUENCETABLE WHERE NUMBERSEQUENCE = 'TRANSACTIONID' AND EMBEDSTOREID = 1 )
BEGIN
	update NUMBERSEQUENCETABLE set EMBEDSTOREID = 0 where NUMBERSEQUENCE = 'TRANSACTIONID'
END
GO

----If no data is in RboStaffTable then at least one row needs to be added
--IF NOT EXISTS (SELECT STAFFID FROM RBOSTAFFTABLE WHERE DATAAREAID = 'LSR')
--BEGIN
--	INSERT [dbo].[RBOSTAFFTABLE] ([STAFFID], [NAME], [PASSWORD], [MANAGERPRIVILEGES])
--	VALUES (N'1001', N'Default POS user', N'1001', 1)
--END