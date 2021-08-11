/*

	Incident No.	:
	Responsible		: Olga Rún Kristjánsdóttir
	Sprint			: LS Retail .NET 2013\Merkúr
	Date created	: 23.07.2012

	Description		: add fields to PosReceiptProfileLines
	
	
	Tables affected	: POSRECEIPTPROFILELINES - altered table
						
*/


Use LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSFORMPROFILELINES') AND NAME='DESCRIPTION')
BEGIN
	ALTER TABLE dbo.POSFORMPROFILELINES ADD [DESCRIPTION] nvarchar(60) NULL
END
GO

