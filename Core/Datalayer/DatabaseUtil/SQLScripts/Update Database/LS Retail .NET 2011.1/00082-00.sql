/*

	Incident No.	: 
	Responsible		: Tobias Helmer
	Sprint			: LS Retail .NET 2011.1\Sprint 2
	Date created	: 28.04.2011

	Description		: Update TaxCollectLimit table has now a new combined primary key

	Logic scripts   : 
	
	Tables affected	: TAXCOLLECTLIMIT
						
*/

USE LSPOSNET

declare @name varchar(255)
declare @SQL varchar(255)

set @name = ( select name from sysobjects where xtype = 'PK'
and parent_obj = (object_id('TAXCOLLECTLIMIT')))

set @SQL = 'ALTER TABLE TAXCOLLECTLIMIT DROP CONSTRAINT ' + @name
exec(@SQL)

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXCOLLECTLIMIT') AND NAME='LINENUM')
ALTER TABLE TAXCOLLECTLIMIT ADD
	LINENUM int NOT NULL CONSTRAINT DF_TAXCOLLECTLIMIT_LINENUM DEFAULT 1
GO

ALTER TABLE TAXCOLLECTLIMIT ADD CONSTRAINT
	I_428TAXCODEIDX PRIMARY KEY CLUSTERED 
	(
	TAXCODE,
	DATAAREAID,
	LINENUM
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO