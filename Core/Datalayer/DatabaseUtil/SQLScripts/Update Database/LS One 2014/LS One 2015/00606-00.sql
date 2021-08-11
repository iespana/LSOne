
/*

	Incident No.	: ONE-1325
	Responsible		: Indriði Ingi stefánsson
	Sprint			: Nacreus
	Date created	: 5.12.2014

	Description		: Add activated flag for Terminal
	
	
	Tables affected	: RBOTerminaltable
						
*/
USE LSPOSNET
GO
  
IF NOT EXISTS (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'RBOTERMINALTABLE' and COLUMN_NAME = 'LASTACTIVATEDDATE' )
BEGIN
	  ALTER TABLE RBOTERMINALTABLE	ADD LASTACTIVATEDDATE DATETIME
END	
GO



