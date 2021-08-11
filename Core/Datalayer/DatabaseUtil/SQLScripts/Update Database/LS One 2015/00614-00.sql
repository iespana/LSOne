/*
	Incident No.	: ONE-1984
	Responsible		: Hörður Kristjánsson
	Sprint			: LS One 2015
	Date created	: 27.04.2015

	Description		: Changed lookuptype of print z
						
*/
USE LSPOSNET
GO

IF EXISTS (SELECT 'x' FROM POSISOPERATIONS WHERE OPERATIONID=905 AND LOOKUPTYPE=0)
Begin
	UPDATE POSISOPERATIONS SET LOOKUPTYPE=19 WHERE OPERATIONID=905
End

GO