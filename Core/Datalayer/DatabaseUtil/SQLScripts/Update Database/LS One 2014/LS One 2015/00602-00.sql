
/*
	Incident No.	: ONE-954
	Responsible		: Hörður Kristjánsson
	Sprint			: LS One 2015 - Blika
	Date created	: 23.10.2014

	Description		: Adding a parameter to SetQty operation
*/
USE LSPOSNET
GO

-- NumericInput	
update POSISOPERATIONS set LOOKUPTYPE = 11 where OPERATIONID = 105 -- Set qty

GO