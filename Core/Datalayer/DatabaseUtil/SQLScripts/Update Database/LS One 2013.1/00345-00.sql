USE LSPOSNET
GO
  
 IF EXISTS(SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = '1501' and USEROPERATION = '1')
 BEGIN
	UPDATE POSISOPERATIONS SET USEROPERATION = '0' where OPERATIONID = '1501'
 END

 IF EXISTS(SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = '1305' and USEROPERATION = '1')
 BEGIN
	UPDATE POSISOPERATIONS SET USEROPERATION = '0' where OPERATIONID = '1305'
 END

GO




