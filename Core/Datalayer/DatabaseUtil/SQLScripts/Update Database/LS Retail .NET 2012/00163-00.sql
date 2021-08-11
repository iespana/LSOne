﻿
/*

	Incident No.	: 13783
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2012\Embla
	Date created	: 22.02.2012

	Description		: Alterning fields in table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBODISCOUNTOFFERLINE - Id field added 
						
*/

--USE LSPOSNET

GO

--Drop Constraint

IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE  CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'RBODISCOUNTOFFERLINE' AND CONSTRAINT_NAME = 'I_20101OFFERLINEIDX')
Begin
	Alter Table RBODISCOUNTOFFERLINE
	DROP CONSTRAINT I_20101OFFERLINEIDX

End
GO

IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE  CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'RBODISCOUNTOFFERLINE' AND CONSTRAINT_NAME = 'DF__RBODISCOU__LINEN__1EDD9165')
Begin
	Alter Table RBODISCOUNTOFFERLINE
	DROP CONSTRAINT DF__RBODISCOU__LINEN__1EDD9165

End
GO

----remove linenum column

--IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='RBODISCOUNTOFFERLINE' AND COLUMN_NAME='LINENUM')
--Begin
	
--	Alter Table RBODISCOUNTOFFERLINE
--	DROP CONSTRAINT DF__RBODISCOU__LINEN__1EDD9165
--	Alter Table RBODISCOUNTOFFERLINE
--	DROP COLUMN LINENUM
--End
--GO




--Add ID column
IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='RBODISCOUNTOFFERLINE' AND COLUMN_NAME='ID')
Begin
	ALTER TABLE RBODISCOUNTOFFERLINE 
	Add ID uniqueidentifier NOT NULL default NEWID()	
	CREATE INDEX RBODISCOUNTOFFERLINE_ID ON RBODISCOUNTOFFERLINE (ID)
End
GO



--Set primary key

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE  CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'RBODISCOUNTOFFERLINE' AND CONSTRAINT_NAME = 'PK_RBODISCOUNTOFFERLINE')
BEGIN

ALTER TABLE dbo.RBODISCOUNTOFFERLINE ADD CONSTRAINT
		PK_RBODISCOUNTOFFERLINE PRIMARY KEY CLUSTERED 
		(
		ID,
		DATAAREAID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END











