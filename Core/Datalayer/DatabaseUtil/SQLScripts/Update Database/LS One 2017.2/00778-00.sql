 /*

	Incident No.	: ONE-3251
	Responsible		: Anca Roibu
	Sprint			: Stunsig 21.6-5.7
	Date created	: 26.06.17

	Description		: Start of day set to be a non-user operation
	
	
	Tables affected	: POSISOPERATIONS
						
*/

USE LSPOSNET
GO

IF EXISTS (SELECT 1 FROM POSISOPERATIONS WHERE OPERATIONID = 1610)
BEGIN
  UPDATE POSISOPERATIONS
  SET USEROPERATION = 0
  WHERE OPERATIONID = 1610
END
GO
