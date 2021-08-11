/*

	Incident No.	: ONE-5436
	Responsible		: Indri�i
	Sprint			: Turmeric 1.12 - 15.12
	Date created	: 7.12.2016

	Description		: Removing upper hierarchy columns from RetailItem table
	
	
	Tables affected	: RETAILITEM
						
*/

USE LSPOSNET
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RETAILITEM' AND COLUMN_NAME = 'RETAILDEPARTMENTMASTERID')
	ALTER TABLE RETAILITEM
	DROP COLUMN RETAILDEPARTMENTMASTERID
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RETAILITEM' AND COLUMN_NAME = 'RETAILDIVISIONMASTERID')
	ALTER TABLE RETAILITEM
	DROP COLUMN RETAILDIVISIONMASTERID
GO