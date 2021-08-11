
/*

	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Cumulus
	Date created	: 2014/11/18

	Description		: Auto generated from a database using the following script 


   SELECT 
  'IF NOT EXISTS(SELECT * FROM JscJobs WHERE ID = '''+CONVERT(VARCHAR(50),jobs.id)+''' ) 
  BEGIN 
  INSERT INTO JscJobs (
	ID,
	Description,
	JobType,
	Source,
	Destination,
	ErrorHandling,
	Enabled,
	SubjobJob,
	PLUGINPATH,
	PLUGINARGUMENTS,
	JobTypeCode,
	UseCurrentLocation)
  VALUES(

'''+CONVERT(VARCHAR(50),jobs.id)+''',
''' + Description+''',
'''+CONVERT(VARCHAR(50), JobType)+''',
'+ CASE When Source IS not null then  ''''+ CONVERT(VARCHAR(50),Source)+'''' else 'null' END+',
'+ CASE When Destination IS not null then ''''+ CONVERT(VARCHAR(50),Destination)+'''' else 'null' END+',
'+CONVERT(VARCHAR(50), ErrorHandling)+',
'+CONVERT(VARCHAR(50), Enabled)+',
'+ CASE When SubjobJob IS not null then ''''+ CONVERT(VARCHAR(50),SubjobJob)+'''' else 'null' END+',
'+ CASE When PLUGINPATH IS not null then ''''+ CONVERT(VARCHAR(50),PLUGINPATH)+'''' else 'null' END+',
'+ CASE When PLUGINARGUMENTS IS not null then ''''+ CONVERT(VARCHAR(50),PLUGINARGUMENTS)+'''' else 'null' END+',
'+ CASE When JobTypeCode IS not null then ''''+ CONVERT(VARCHAR(50),JobTypeCode)+'''' else 'null' END+',
'+CONVERT(VARCHAR(50), UseCurrentLocation)+')
	END
ELSE
	Begin
		Update  JscJobs set
			Description ='''+CONVERT(VARCHAR(50), Description)+''',
			JobType ='+CONVERT(VARCHAR(50), JobType)+',			
			Source='+ CASE When Source IS not null then  ''''+ CONVERT(VARCHAR(50),Source)+'''' else 'null' END+',
			Destination='+ CASE When Destination IS not null then ''''+ CONVERT(VARCHAR(50),Destination)+'''' else 'null' END+',
			ErrorHandling='+CONVERT(VARCHAR(50), ErrorHandling)+',
			Enabled='+CONVERT(VARCHAR(50), Enabled)+',
			SubjobJob='+ CASE When SubjobJob IS not null then ''''+ CONVERT(VARCHAR(50),SubjobJob)+'''' else 'null' END+',
			PLUGINPATH='+ CASE When PLUGINPATH IS not null then ''''+ CONVERT(VARCHAR(50),PLUGINPATH)+'''' else 'null' END+',
			PLUGINARGUMENTS='+ CASE When PLUGINARGUMENTS IS not null then ''''+ CONVERT(VARCHAR(50),PLUGINARGUMENTS)+'''' else 'null' END+',
			JobTypeCode='+ CASE When JobTypeCode IS not null then ''''+ CONVERT(VARCHAR(50),JobTypeCode)+'''' else 'null' END+',
			UseCurrentLocation='+CONVERT(VARCHAR(50), UseCurrentLocation)+'

			WHERE ID = '''+CONVERT(VARCHAR(50), jobs.id)+'''
	END'
  FROM (
  SELECT  ID -- USE 
  --newid() AS id --if you need new ones
        ,Description
      ,JobType
      ,Source
      ,Destination
      ,ErrorHandling
      ,Enabled
      ,SubjobJob
      ,PLUGINPATH
      ,PLUGINARGUMENTS
      ,JobTypeCode
      ,UseCurrentLocation
  FROM JscJobs where enabled = 1) jobs 




	
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/

Use LSPOSNET 
GO

IF NOT EXISTS (SELECT
    *
  FROM JscJobs
  WHERE ID = '51A5A057-0243-4771-9223-0B98BB9F54A8')
BEGIN
  INSERT INTO JscJobs (ID, Description, JobType, Source, Destination, ErrorHandling, Enabled, SubjobJob, PLUGINPATH, PLUGINARGUMENTS, JobTypeCode, UseCurrentLocation)
    VALUES ('51A5A057-0243-4771-9223-0B98BB9F54A8', 'Synchronize', '1', '774E57C4-0CC6-422B-96D5-CFEC71D815EE', '1AEF42A8-9E25-4234-A523-A5B0647F486A', 0, 1, NULL, '', '', NULL, 0)
END
ELSE
BEGIN
  UPDATE JscJobs
  SET Description = 'Synchronize',
      JobType = 1,
      Source = '774E57C4-0CC6-422B-96D5-CFEC71D815EE',
      Destination = '1AEF42A8-9E25-4234-A523-A5B0647F486A',
      ErrorHandling = 0,
      Enabled = 1,
      SubjobJob = NULL,
      PLUGINPATH = '',
      PLUGINARGUMENTS = '',
      JobTypeCode = NULL,
      UseCurrentLocation = 0
  WHERE ID = '51A5A057-0243-4771-9223-0B98BB9F54A8'
END
IF NOT EXISTS (SELECT
    *
  FROM JscJobs
  WHERE ID = 'ADB54B83-32DB-40AD-9E18-0D1659674B54')
BEGIN
  INSERT INTO JscJobs (ID, Description, JobType, Source, Destination, ErrorHandling, Enabled, SubjobJob, PLUGINPATH, PLUGINARGUMENTS, JobTypeCode, UseCurrentLocation)
    VALUES ('ADB54B83-32DB-40AD-9E18-0D1659674B54', 'Replication data', '1', '774E57C4-0CC6-422B-96D5-CFEC71D815EE', '1AEF42A8-9E25-4234-A523-A5B0647F486A', 0, 1, NULL, '', '', NULL, 0)
END
ELSE
BEGIN
  UPDATE JscJobs
  SET Description = 'Replication data',
      JobType = 1,
      Source = '774E57C4-0CC6-422B-96D5-CFEC71D815EE',
      Destination = '1AEF42A8-9E25-4234-A523-A5B0647F486A',
      ErrorHandling = 0,
      Enabled = 1,
      SubjobJob = NULL,
      PLUGINPATH = '',
      PLUGINARGUMENTS = '',
      JobTypeCode = NULL,
      UseCurrentLocation = 0
  WHERE ID = 'ADB54B83-32DB-40AD-9E18-0D1659674B54'
END
IF NOT EXISTS (SELECT
    *
  FROM JscJobs
  WHERE ID = '2648035C-C996-4E35-9C10-20E2660CDA4B')
BEGIN
  INSERT INTO JscJobs (ID, Description, JobType, Source, Destination, ErrorHandling, Enabled, SubjobJob, PLUGINPATH, PLUGINARGUMENTS, JobTypeCode, UseCurrentLocation)
    VALUES ('2648035C-C996-4E35-9C10-20E2660CDA4B', 'Transactions from POS', '1', '1AEF42A8-9E25-4234-A523-A5B0647F486A', '774E57C4-0CC6-422B-96D5-CFEC71D815EE', 0, 1, NULL, '', '', NULL, 0)
END
ELSE
BEGIN
  UPDATE JscJobs
  SET Description = 'Transactions from POS',
      JobType = 1,
      Source = '1AEF42A8-9E25-4234-A523-A5B0647F486A',
      Destination = '774E57C4-0CC6-422B-96D5-CFEC71D815EE',
      ErrorHandling = 0,
      Enabled = 1,
      SubjobJob = NULL,
      PLUGINPATH = '',
      PLUGINARGUMENTS = '',
      JobTypeCode = NULL,
      UseCurrentLocation = 0
  WHERE ID = '2648035C-C996-4E35-9C10-20E2660CDA4B'
END
IF NOT EXISTS (SELECT
    *
  FROM JscJobs
  WHERE ID = 'E76244B1-3DD7-46CC-9A0B-752ECD050815')
BEGIN
  INSERT INTO JscJobs (ID, Description, JobType, Source, Destination, ErrorHandling, Enabled, SubjobJob, PLUGINPATH, PLUGINARGUMENTS, JobTypeCode, UseCurrentLocation)
    VALUES ('E76244B1-3DD7-46CC-9A0B-752ECD050815', 'Clean actions', '1', '1AEF42A8-9E25-4234-A523-A5B0647F486A', '774E57C4-0CC6-422B-96D5-CFEC71D815EE', 0, 1, NULL, '', '', NULL, 0)
END
ELSE
BEGIN
  UPDATE JscJobs
  SET Description = 'Clean actions',
      JobType = 1,
      Source = '1AEF42A8-9E25-4234-A523-A5B0647F486A',
      Destination = '774E57C4-0CC6-422B-96D5-CFEC71D815EE',
      ErrorHandling = 0,
      Enabled = 1,
      SubjobJob = NULL,
      PLUGINPATH = '',
      PLUGINARGUMENTS = '',
      JobTypeCode = NULL,
      UseCurrentLocation = 0
  WHERE ID = 'E76244B1-3DD7-46CC-9A0B-752ECD050815'
END
IF NOT EXISTS (SELECT
    *
  FROM JscJobs
  WHERE ID = '095752B2-15F1-420D-8D67-951907988D63')
BEGIN
  INSERT INTO JscJobs (ID, Description, JobType, Source, Destination, ErrorHandling, Enabled, SubjobJob, PLUGINPATH, PLUGINARGUMENTS, JobTypeCode, UseCurrentLocation)
    VALUES ('095752B2-15F1-420D-8D67-951907988D63', 'Preload POS', '1', '774E57C4-0CC6-422B-96D5-CFEC71D815EE', '1AEF42A8-9E25-4234-A523-A5B0647F486A', 0, 1, NULL, '', '', NULL, 0)
END
ELSE
BEGIN
  UPDATE JscJobs
  SET Description = 'Preload POS',
      JobType = 1,
      Source = '774E57C4-0CC6-422B-96D5-CFEC71D815EE',
      Destination = '1AEF42A8-9E25-4234-A523-A5B0647F486A',
      ErrorHandling = 0,
      Enabled = 1,
      SubjobJob = NULL,
      PLUGINPATH = '',
      PLUGINARGUMENTS = '',
      JobTypeCode = NULL,
      UseCurrentLocation = 0
  WHERE ID = '095752B2-15F1-420D-8D67-951907988D63'
END
IF NOT EXISTS (SELECT
    *
  FROM JscJobs
  WHERE ID = 'EFAF10E4-A7AB-4047-8172-9AF26839F489')
BEGIN
  INSERT INTO JscJobs (ID, Description, JobType, Source, Destination, ErrorHandling, Enabled, SubjobJob, PLUGINPATH, PLUGINARGUMENTS, JobTypeCode, UseCurrentLocation)
    VALUES ('EFAF10E4-A7AB-4047-8172-9AF26839F489', 'Layout from POS', '1', '1AEF42A8-9E25-4234-A523-A5B0647F486A', '774E57C4-0CC6-422B-96D5-CFEC71D815EE', 0, 1, NULL, '', '', NULL, 0)
END
ELSE
BEGIN
  UPDATE JscJobs
  SET Description = 'Layout from POS',
      JobType = 1,
      Source = '1AEF42A8-9E25-4234-A523-A5B0647F486A',
      Destination = '774E57C4-0CC6-422B-96D5-CFEC71D815EE',
      ErrorHandling = 0,
      Enabled = 1,
      SubjobJob = NULL,
      PLUGINPATH = '',
      PLUGINARGUMENTS = '',
      JobTypeCode = NULL,
      UseCurrentLocation = 0
  WHERE ID = 'EFAF10E4-A7AB-4047-8172-9AF26839F489'
END
IF NOT EXISTS (SELECT
    *
  FROM JscJobs
  WHERE ID = 'A6AC9D48-DC95-4EEA-9864-9D273EDC20D7')
BEGIN
  INSERT INTO JscJobs (ID, Description, JobType, Source, Destination, ErrorHandling, Enabled, SubjobJob, PLUGINPATH, PLUGINARGUMENTS, JobTypeCode, UseCurrentLocation)
    VALUES ('A6AC9D48-DC95-4EEA-9864-9D273EDC20D7', 'My time plan export', '7', NULL, NULL, 0, 1, NULL, 'LSOne.ViewPlugins.MyTimePlan.dll', '', NULL, 0)
END
ELSE
BEGIN
  UPDATE JscJobs
  SET Description = 'My time plan export',
      JobType = 7,
      Source = NULL,
      Destination = NULL,
      ErrorHandling = 0,
      Enabled = 1,
      SubjobJob = NULL,
      PLUGINPATH = 'LSOne.ViewPlugins.MyTimePlan.dll',
      PLUGINARGUMENTS = '',
      JobTypeCode = NULL,
      UseCurrentLocation = 0
  WHERE ID = 'A6AC9D48-DC95-4EEA-9864-9D273EDC20D7'
END
IF NOT EXISTS (SELECT
    *
  FROM JscJobs
  WHERE ID = 'A386604E-74CF-4BB2-B0C6-E8D4D0AAF1CB')
BEGIN
  INSERT INTO JscJobs (ID, Description, JobType, Source, Destination, ErrorHandling, Enabled, SubjobJob, PLUGINPATH, PLUGINARGUMENTS, JobTypeCode, UseCurrentLocation)
    VALUES ('A386604E-74CF-4BB2-B0C6-E8D4D0AAF1CB', 'Pull from POS', '1', '1AEF42A8-9E25-4234-A523-A5B0647F486A', '774E57C4-0CC6-422B-96D5-CFEC71D815EE', 0, 1, NULL, '', '', NULL, 0)
END
ELSE
BEGIN
  UPDATE JscJobs
  SET Description = 'Pull from POS',
      JobType = 1,
      Source = '1AEF42A8-9E25-4234-A523-A5B0647F486A',
      Destination = '774E57C4-0CC6-422B-96D5-CFEC71D815EE',
      ErrorHandling = 0,
      Enabled = 1,
      SubjobJob = NULL,
      PLUGINPATH = '',
      PLUGINARGUMENTS = '',
      JobTypeCode = NULL,
      UseCurrentLocation = 0
  WHERE ID = 'A386604E-74CF-4BB2-B0C6-E8D4D0AAF1CB'
END