/*

	Incident No.	: 26164
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\July, August, September
	Date created	: 16.10.2013

	Description		: Added column to POSSTYLE
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSSTYLE' AND COLUMN_NAME = 'SYSTEMSTYLE')
Begin
  Alter Table POSSTYLE add SYSTEMSTYLE bit 
End

GO

update POSSTYLE SET SYSTEMSTYLE=1 where SYSTEMSTYLE is NULL and NAME in (
	'SystemNumPadButtonStyle',
	'SystemNumPadDigitButtonStyle',
	'DualDisplayTotal',
	'DualDisplayLine',
	'DualDisplayLineSub')