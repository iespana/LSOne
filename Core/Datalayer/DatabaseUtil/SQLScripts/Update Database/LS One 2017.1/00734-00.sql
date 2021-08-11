/*

	Incident No.	: ONE-6400 Add an email to the user
	Responsible		: Adrian Chiorean
	Sprint			: Grundtal
	Date created	: 27.03.2017

	Description		: Add email field to USERS tables
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'Email' AND TABLE_NAME = 'USERS')
BEGIN
	ALTER TABLE USERS
	ADD EMAIL NVARCHAR(50) NULL
END
GO