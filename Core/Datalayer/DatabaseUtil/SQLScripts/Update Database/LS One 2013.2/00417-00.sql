/*

	Incident No.	: 25553
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 19.9.2013

	Description		: Added CHANGINGBUMPEDORDERPOSSIBLE to KITCHENDISPLAYTRANSACTIONPROFILE
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'KITCHENDISPLAYTRANSACTIONPROFILE' AND COLUMN_NAME = 'CHANGINGBUMPEDORDERPOSSIBLE')
Begin
  Alter Table KITCHENDISPLAYTRANSACTIONPROFILE add CHANGINGBUMPEDORDERPOSSIBLE bit not null default 0
End
