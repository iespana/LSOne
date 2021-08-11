
/*

	Incident No.	: 
	Responsible		: Olga Rún Kristjánsdóttir
	Sprint			: LS Retail .NET Milky Way/Sprint 2 Mercury
	Date created	: 25.07.2012
	
	Description		: Change type of column "id" in posisformlayout

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : POSISFORMLAYOUTLog - Changing column in table
						
*/


IF((SELECT [DATA_TYPE] from [INFORMATION_SCHEMA].[COLUMNS] WHERE [TABLE_NAME] = 'POSISFORMLAYOUTLog' AND [COLUMN_NAME] = 'ID')= 'INT')
BEGIN
ALTER TABLE dbo.POSISFORMLAYOUTLog ALTER column ID nvarchar(20) not null
END
GO


