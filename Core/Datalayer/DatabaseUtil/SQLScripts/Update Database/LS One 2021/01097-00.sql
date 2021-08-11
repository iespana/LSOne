/*
	Incident No.	: ONE-13015
	Responsible		: Adrian Chiorean
	Sprint			: Ruble
	Date created	: 07.01.2021

	Description		: Add AlwaysExecute column to JscSubJobs
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'AlwaysExecute' AND TABLE_NAME = 'JscSubJobs')
    BEGIN
        ALTER TABLE JscSubJobs ADD AlwaysExecute BIT NOT NULL DEFAULT 0
        EXECUTE spDB_SetFieldDescription_1_0 'JscSubJobs', 'AlwaysExecute', 'True if a job should ignore the replication actions table and execute always.'
    END
GO