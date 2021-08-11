
/*

	Incident No.	: N/A
	Responsible		: Björn Eiríksson
	Sprint			: 2012 - Sprint Baldur
	Date created	: 30.06.2011

	Description		: Added IsServerUser flag to the user table

	Logic scripts   : Added spSECURITY_Login_1_1
	
	Tables affected	: RBOGIFTCARDTABLE, RBOGIFTCARDTRANSACTIONS
						
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('USERS') AND NAME='IsServerUser')
BEGIN
	ALTER TABLE dbo.USERS ADD IsServerUser bit NOT NULL CONSTRAINT DF_USERS_IsServerUser DEFAULT 0
END

GO




