/*

	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS Retail .NET 2013\Mars
	Date created	: 26.11.2012

	Description		: Add new table for Kitchen Display Stations

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: KITCHENDISPLAYSTATION 
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KITCHENDISPLAYSTATION'))
	BEGIN
		CREATE TABLE [dbo].KITCHENDISPLAYSTATION (
		[ID] [nvarchar](20) NOT NULL,
		[NAME]  [nvarchar](100) NOT NULL,
		[SCREENNUMBER] int NOT NULL,
		[SCREENALIGNMENT] int NOT NULL,
		[KITCHENDISPLAYFUNCTIONALPROFILEID] [uniqueidentifier] NOT NULL,
		[KITCHENDISPLAYSTYLEPROFILEID] [uniqueidentifier] NOT NULL,
		[KITCHENDISPLAYVISUALPROFILEID] [uniqueidentifier] NOT NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL
		)

		Alter table KITCHENDISPLAYSTATION Add CONSTRAINT  PK_KITCHENDISPLAYSTATION Primary Key (ID,DATAAREAID)
	END
GO
