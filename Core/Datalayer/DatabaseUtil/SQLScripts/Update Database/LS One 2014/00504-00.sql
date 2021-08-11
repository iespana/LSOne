/*
	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 23.01.2014

	Description		: Changed lookup type of PayCard to StorePaymentMethods
	
						
*/
USE LSPOSNET
GO

update POSISOPERATIONS set LOOKUPTYPE = 2 where OPERATIONID = 201 -- Pay Card

GO