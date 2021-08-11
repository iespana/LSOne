
/*

	Incident No.	: N/A
	Responsible		: Guðbjörn Einarsson
	Sprint			: 2011 - Sprint 5
	Date created	: 10.01.2011

	Description		: Change value in NUMBERSEQUENCETABLE.FORMAT 

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: NUMBERSEQUENCETABLE	-	FORMAT value changed because it was created wrongly for terminals
						
*/

USE LSPOSNET

GO

UPDATE dbo.NUMBERSEQUENCETABLE 
SET FORMAT = '####'
WHERE NUMBERSEQUENCE = 'Terminals'
AND   FORMAT = '####-'



