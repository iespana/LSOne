/*

	Incident No.	: 
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET 2013\Merkúr
	Date created	: 25.07.2012

	Description		: Add data to KMBUTTONOPERATIONS
	
	
	Tables affected	: KMBUTTONOPERATIONS = Data added
						
*/


Use LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[KMBUTTONOPERATIONS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[KMBUTTONOPERATIONS](
	[OPERATIONID] [int] NOT NULL,
	[OPERATIONNAME] [varchar](50) NULL,
	[DATAAREAID] [varchar](4) NOT NULL
	)

	Alter table [KMBUTTONOPERATIONS] Add CONSTRAINT  PK_KMBUTTONOPERATIONS Primary Key (OPERATIONID,DATAAREAID)

	INSERT INTO [dbo].[KMBUTTONOPERATIONS]
           ([OPERATIONID]
           ,[OPERATIONNAME]
           ,[DATAAREAID])
     VALUES
           (0, 'No operation', 'LSR'),
		   (1, 'Next', 'LSR'),
		   (2, 'Previous', 'LSR'),
		   (3, 'Select', 'LSR'),
		   (4, 'Deselect', 'LSR'),
		   (5, 'Bump', 'LSR'),
		   (6, 'Start', 'LSR')
END
GO

