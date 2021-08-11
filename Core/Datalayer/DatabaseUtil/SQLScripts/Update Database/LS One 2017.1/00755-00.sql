﻿/*
	Incident No.	: ONE-6694
	Responsible		: Helgi Rúnar Gunanrsson
	Sprint			: Pax
	Date created	: 29.05.2017

	Description		: Change the replication job so that the cloud location doesn't break.
*/

USE LSPOSNET

IF EXISTS(SELECT 1 FROM JSCLOCATIONS WHERE NAME = 'CLOUD')
BEGIN
IF NOT EXISTS(SELECT * FROM JSCJOBSUBJOBS WHERE ID = '4EE6250B-2E2A-4A94-A57C-E9A123D54043' ) 
  BEGIN 
  INSERT INTO JSCJOBSUBJOBS (
	ID,
	JOB,
	SUBJOB,
	SEQUENCE,
	ENABLED)
  VALUES
  (
'4EE6250B-2E2A-4A94-A57C-E9A123D54043',
'060FACEB-8B76-4EAD-AC20-8434A2158CEB',
'6FA26856-3CCA-441B-A686-CDEF335598CA',
135,
1)
	END
ELSE
	BEGIN
		UPDATE  JSCJOBSUBJOBS SET
			JOB = '060FACEB-8B76-4EAD-AC20-8434A2158CEB',
			SUBJOB='6FA26856-3CCA-441B-A686-CDEF335598CA',
			SEQUENCE=135,
			ENABLED=1

			WHERE ID = '4EE6250B-2E2A-4A94-A57C-E9A123D54043'
	END

IF NOT EXISTS(SELECT * FROM JSCJOBSUBJOBS WHERE ID = '0EAD5BBD-7D4C-40AE-8BF2-F20475844BE3' ) 
  BEGIN 
  INSERT INTO JSCJOBSUBJOBS (
	ID,
	JOB,
	SUBJOB,
	SEQUENCE,
	ENABLED)
  VALUES
  (
'0EAD5BBD-7D4C-40AE-8BF2-F20475844BE3',
'ADB54B83-32DB-40AD-9E18-0D1659674B54',
'6FA26856-3CCA-441B-A686-CDEF335598CA',
24,
0)
	END
ELSE
	BEGIN
		UPDATE  JSCJOBSUBJOBS SET
			JOB = 'ADB54B83-32DB-40AD-9E18-0D1659674B54',
			SUBJOB='6FA26856-3CCA-441B-A686-CDEF335598CA',
			SEQUENCE=24,
			ENABLED=0

			WHERE ID = '0EAD5BBD-7D4C-40AE-8BF2-F20475844BE3'
	END
END
GO