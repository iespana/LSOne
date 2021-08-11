
/*

	Incident No.	: 13312
	Responsible		: Óskar Bjarnason	
	Sprint			: LS Retail .NET 2012/Idunn
	Date created	: 09.11.2011

	Description		: Change Pay Cheque to Pay Check

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSISOPERATION - Change field
					  
					  
					  
					  	
*/								

USE LSPOSNET

GO

IF EXISTS (SELECT * FROM dbo.POSISOPERATIONS WHERE  OPERATIONID = '204')
BEGIN
	UPDATE POSISOPERATIONS
	SET OPERATIONNAME = 'Pay Check' 
	where OPERATIONID = '204' 

END

GO



