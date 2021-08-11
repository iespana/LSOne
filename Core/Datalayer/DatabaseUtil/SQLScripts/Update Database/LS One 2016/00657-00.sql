﻿/*
	Incident No.	: ONE-3195
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Jerusalem
	Date created	: 04.01.2016

	Description		: Added Customer orders operation
	
						
*/
USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 614)
BEGIN
  INSERT INTO POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
  VALUES (614, 'Customer order',	NULL,	NULL,	1,	1,'LSR',0)
END
GO

IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 615)
BEGIN
  INSERT INTO POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
  VALUES (615, 'Quote',	NULL,	NULL,	1,	0,'LSR',0)
END
GO


IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 616)
BEGIN
  INSERT INTO POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
  VALUES (616, 'Recall customer orders',	NULL,	NULL,	1,	1,'LSR',0)
END
GO

IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 617)
BEGIN
  INSERT INTO POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
  VALUES (617, 'Recall quotes',	NULL,	NULL,	1,	0,'LSR',0)
END
GO