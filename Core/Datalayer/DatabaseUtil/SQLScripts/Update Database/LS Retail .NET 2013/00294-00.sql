/*

	Incident No.	: x
	Responsible		: Sigfus Johannesson

						
*/
USE LSPOSNET

GO

IF NOT EXISTS(select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE' AND COLUMN_NAME = 'MAXIMUMGIFTCARDAMOUNT')
BEGIN
ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD MAXIMUMGIFTCARDAMOUNT DECIMAL(28,12)
END

GO