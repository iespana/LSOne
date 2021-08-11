/*

	Incident No.	: 25793
	Responsible		: Sigfus Johannesson
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 02.10.2013

	Description		: Added VERSION to POSISLICENSE
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSISLICENSE' AND COLUMN_NAME = 'VERSION')
Begin
  Alter Table POSISLICENSE add [VERSION] nvarchar(10) 
End

GO