/*

	Incident No.	: PBI #10195 / SPT #11073
	Responsible		: Tobias Helmer
	Sprint			: LS Retail.Net 2012 Mjolnir\DotNet Stream\Baldur 1 June 23-aug 3\Team Æsir
	Date created	: 07.07.2011

	Description		: Adding a boolean that determines whether a blank operation has been created on the POS terminal.			

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSFUNCTIONALITYPROFILE					  
						
*/


USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSISBLANKOPERATIONS') AND NAME='OPERATIONCREATEDONPOS')
BEGIN
	ALTER TABLE [dbo].[POSISBLANKOPERATIONS] ADD OPERATIONCREATEDONPOS tinyint NULL;
	ALTER TABLE [dbo].[POSISBLANKOPERATIONS] ADD  CONSTRAINT [DF_POSISBLANKOPERATIONS_OPERATIONCREATEDONPOS]  DEFAULT ((0)) FOR [OPERATIONCREATEDONPOS]
END

GO