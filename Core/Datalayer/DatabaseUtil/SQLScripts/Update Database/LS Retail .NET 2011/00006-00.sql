/*

	Responsible		: [Tobias]
	Date created	: [09. Nov. 2010]

	Description		: [Adds a DateTime field to the table InventJournalTable]

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: [List of all tables affected in this script with a short description of what was done]
					  Example:
						InventJournalTable			- fields added, default values added to fields
						
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[INVENTJOURNALTABLE]') AND TYPE IN ('U'))
BEGIN
	IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('[DBO].[INVENTJOURNALTABLE]') AND NAME='CREATEDDATETIME')
	BEGIN
		ALTER TABLE dbo.INVENTJOURNALTABLE ADD CREATEDDATETIME datetime NULL
	END
END
GO