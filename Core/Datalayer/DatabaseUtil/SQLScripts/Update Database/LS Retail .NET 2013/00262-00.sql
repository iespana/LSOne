/*

	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS Retail .NET 2013\Mars
	Date created	: 26.11.2012

	Description		: Change column in KMTIMESTYLE table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: KMTIMESTYLE
						
*/

USE LSPOSNET

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMTIMESTYLE')  AND NAME='KDSPROFILEID' )
	Begin
		Alter table KMTIMESTYLE
		Drop column KDSPROFILEID

		Alter table KMTIMESTYLE
		Add KDSTYLEPROFILEID uniqueidentifier
	End
GO
