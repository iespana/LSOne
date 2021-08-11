/*

	Incident No.	: 
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\July, August, September
	Date created	: 11.10.2013

	Description		: Removed image search (was there only temporarily) and modified item search parameters
*/

USE LSPOSNET

DELETE FROM POSISOPERATIONS WHERE OPERATIONNAME='Image search by retail group' AND OPERATIONID=134
GO

IF EXISTS (SELECT 'x' FROM POSISOPERATIONS WHERE OPERATIONID=108 AND LOOKUPTYPE=0)
Begin
	UPDATE POSISOPERATIONS SET LOOKUPTYPE=14 WHERE OPERATIONID=108
End
 