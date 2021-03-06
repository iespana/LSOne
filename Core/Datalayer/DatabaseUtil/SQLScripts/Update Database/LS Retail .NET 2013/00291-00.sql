/*

	Incident No.	: 20823
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2013\Neptune
	Date created	: 12.02.2013

	Description		: Added columns
	
	
	Tables affected	: POSPERIODICDISCOUNT
						
*/
USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'POSPERIODICDISCOUNT' and COLUMN_NAME = 'ACCOUNTCODE')
BEGIN
	 ALTER TABLE POSPERIODICDISCOUNT Add ACCOUNTCODE int NULL 
END	

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'POSPERIODICDISCOUNT' and COLUMN_NAME = 'ACCOUNTRELATION')
BEGIN
	 ALTER TABLE POSPERIODICDISCOUNT Add ACCOUNTRELATION nvarchar(20) NULL 
END	
GO