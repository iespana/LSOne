
/*

	Incident No.	: 21100
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Neptune
	Date created	: 6.2.2013

	Description		: Change operations to not be user operations anymore

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PosisOperations
						
*/

USE LSPOSNET

UPDATE POSISOPERATIONS
SET USEROPERATION = 0
WHERE OPERATIONID in (913, 127, 128, 515, 516)
AND DATAAREAID = 'LSR'
GO


IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 215)
BEGIN
  INSERT INTO POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
  VALUES (215, 'Authorize card',	NULL,	NULL,	1,	1,'LSR',2)
END
GO




