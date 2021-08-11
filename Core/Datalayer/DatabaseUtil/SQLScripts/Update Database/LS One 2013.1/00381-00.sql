
/*

	Incident No.	: 23707
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 2.7.2013

	Description		: Adds the operation Loyalty card information to the database
	
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 132)
BEGIN
  INSERT INTO POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
  VALUES (132, 'Loyalty card information',	NULL,	NULL,	1,	1,'LSR',0)
END
GO




