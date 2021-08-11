
/*

	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 22.6.2013

	Description		: Remove unused table RETAILGROUPREPLENISHMENT
	
						
*/

USE LSPOSNET
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RETAILGROUPREPLENISHMENT')
BEGIN
	Drop table RETAILGROUPREPLENISHMENT
END
GO
