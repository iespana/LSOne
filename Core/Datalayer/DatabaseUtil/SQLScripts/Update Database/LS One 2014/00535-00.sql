
/*

	Incident No.	: n/a
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: N/A
	Date created	: 17.6.2014

	Description		: [Short description]	
	
						
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TAXREFUNDRANGE' AND COLUMN_NAME = 'TAXVALUE')
BEGIN
	ALTER TABLE TAXREFUNDRANGE ADD TAXVALUE NUMERIC(28, 12)
END




