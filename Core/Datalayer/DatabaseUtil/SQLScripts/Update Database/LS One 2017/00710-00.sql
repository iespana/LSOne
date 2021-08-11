/*

	Incident No.	: ONE-5319
	Responsible		: Frentiu Florin
	Sprint			: Sumac 18.11- 1.12
	Date created	: 25.11.2016

	Description		: Add RESERVED column to RETAILITEMSERIALNUMBERS -> When an item takes part of a transaction is marked as reserved so that other transactions cannot use it.
	
	
	Tables affected	: RETAILITEMSERIALNUMBERS
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RETAILITEMSERIALNUMBERS' AND COLUMN_NAME = 'RESERVED')
BEGIN
ALTER TABLE dbo.RETAILITEMSERIALNUMBERS ADD
	RESERVED bit NOT NULL CONSTRAINT DF_RETAILITEMSERIALNUMBERS_RESERVED DEFAULT 0
END

GO