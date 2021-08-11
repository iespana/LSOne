/*

	Incident No.	: 13599
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET 2012/Mímir
	Date created	: 02.12.2011

	Description		: Add address format field to SuspendedTransactions answer table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSISSUSPENDTRANSADDINFO - ADDRESSFORMAT field added
						
*/

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISSUSPENDTRANSADDINFO' AND COLUMN_NAME='ADDRESSFORMAT')
Begin
	Alter table POSISSUSPENDTRANSADDINFO 
	Add ADDRESSFORMAT int NULL
END
GO
