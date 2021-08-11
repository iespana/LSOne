/*

	Incident No.	: 21811
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Pluto
	Date created	: 16.03.2013

	Description		: Changed operation name in POSISOPERATIONS from 'Pay Giftcard' to 'Pay Gift Card'
	
	
	Tables affected	: POSISOPERATIONS
						
*/
USE LSPOSNET

GO

 IF EXISTS(SELECT * FROM POSISOPERATIONS where OPERATIONNAME = 'Pay Giftcard' and OPERATIONID = '214')
  BEGIN
		UPDATE POSISOPERATIONS SET OPERATIONNAME = 'Pay Gift Card'
		WHERE OPERATIONNAME = 'Pay Giftcard' and OPERATIONID = '214'
  END

GO