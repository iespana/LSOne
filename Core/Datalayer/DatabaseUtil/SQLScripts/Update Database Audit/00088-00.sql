
/*

	Incident No.	: 
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 21.12.2013						
*/

USE LSPOSNET_Audit
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'NUMBERSEQUENCEVALUELog' AND COLUMN_NAME = 'NEXTREC')
BEGIN
  Alter Table NUMBERSEQUENCEVALUELog drop column NEXTREC
END
GO


