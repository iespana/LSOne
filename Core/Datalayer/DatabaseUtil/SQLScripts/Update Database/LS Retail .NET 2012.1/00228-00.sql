/*

	Incident No.	: xxx
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 17.08.2012

	Description		: Add Recall value to KMBUTTONOPERATIONS
	
	
	Tables affected	: KMINTERFACEPROFILE
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT OPERATIONID FROM KMBUTTONOPERATIONS WHERE OPERATIONID=7)
BEGIN
	Insert into KMBUTTONOPERATIONS (OPERATIONID, OPERATIONNAME, DATAAREAID) values (7, 'Recall', 'LSR')
END
GO
