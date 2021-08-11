/*

	Incident No.	: XXX
	Responsible		: Sigfus Johannesson
	Sprint			: LS Retail .NET 2013\Jupiter
	Date created	: 10.12.2012

	Description		: Disable print x per day
	
	
	Tables affected	: POSISOPERATIONS
						
*/
USE LSPOSNET
GO
-- Update the Print x per day operation to no longer being an user operation
UPDATE POSISOPERATIONS
SET USEROPERATION = 0
WHERE OPERATIONID = 903
GO

