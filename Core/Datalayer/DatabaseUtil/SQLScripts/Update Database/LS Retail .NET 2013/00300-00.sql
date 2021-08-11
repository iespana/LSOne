/*

	Incident No.	: 21811
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Pluto
	Date created	: 16.03.2013

	Description		: Changed operation name in POSISOPERATIONS from 'Issue Gift Certificate' to 'Issue Gift Card'
	
	Tables affected	: POSISOPERATIONS
						
*/
USE LSPOSNET

GO

 IF EXISTS(SELECT * FROM POSISOPERATIONS where OPERATIONNAME = N'Issue Gift Certificate' and OPERATIONID = '512')
  BEGIN
		UPDATE POSISOPERATIONS SET OPERATIONNAME = N'Issue Gift Card'
		WHERE OPERATIONNAME = N'Issue Gift Certificate' and OPERATIONID = '512'
  END

GO