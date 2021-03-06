/* 
        Incident No.: N/A 
        Responsible : Sigfus Johannesson
        Sprint		: LS One 2013.1\July, August, September
        Date created: 23.10.13 
        Description	: Add columns to POSFUNCTIONALITYPROFILE
*/ 
USE LSPOSNET 
GO 

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'CLEARSETTING')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILE ADD CLEARSETTING INT
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'CLEARGRACEPERIOD')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILE ADD CLEARGRACEPERIOD INT
END
