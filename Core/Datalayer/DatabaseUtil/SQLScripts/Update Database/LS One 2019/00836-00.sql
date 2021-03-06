/*
	Incident No.	: ONE-8662, LSTS-2666
	Responsible		: Hörður Kristjánsson
	Sprint			: Huraga
	Date created	: 23.03.2018

	Description		: Adding new columns to REPLICATIONACTIONS and scheduler tables to support better pre-filtering of jobs and 
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'REPLICATIONACTIONS' AND COLUMN_NAME = 'FILTERCODE')
BEGIN
	ALTER TABLE REPLICATIONACTIONS
	ADD FILTERCODE NVARCHAR(20) NULL
END
GO

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = N'INDEX_REPLICATIONACTIONS_FILTERCODE')
BEGIN
	CREATE NONCLUSTERED INDEX INDEX_REPLICATIONACTIONS_FILTERCODE
    ON REPLICATIONACTIONS (FILTERCODE)
    WHERE FILTERCODE IS NOT NULL
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'JscSubJobs' and COLUMN_NAME = 'FilterCodeFilter')
BEGIN
	ALTER TABLE JscSubJobs
	ADD FilterCodeFilter NVARCHAR(MAX)
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'JscJobs' and COLUMN_NAME = 'CompressionMode')
BEGIN
	ALTER TABLE JscJobs
	ADD CompressionMode INT DEFAULT 0
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'JscJobs' and COLUMN_NAME = 'IsolationLevel')
BEGIN
	ALTER TABLE JscJobs
	ADD IsolationLevel INT DEFAULT 0
END