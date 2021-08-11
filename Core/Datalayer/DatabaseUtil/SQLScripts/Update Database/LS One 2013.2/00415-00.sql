/*

	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 17.9.2013

	Description		:  Add new fields to the Store
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTORETABLE' AND COLUMN_NAME = 'RECEIPTPROFILEID')
Begin
  Alter Table RBOSTORETABLE ADD RECEIPTPROFILEID uniqueidentifier
  Alter Table RBOSTORETABLE ADD RECEIPTLOGO varbinary(max)
  Alter Table RBOSTORETABLE ADD RECEIPTLOGOWIDTH int
End
