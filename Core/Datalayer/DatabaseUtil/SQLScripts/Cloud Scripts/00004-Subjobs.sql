/*

	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Cumulus
	Date created	: 2014/11/18

	Description		: Auto generated from a database using the following script 
	/****** Script for SelectTopNRows command from SSMS  ******/


SELECT 
  'GO  
  IF NOT EXISTS(SELECT * FROM JscSubJobs WHERE ID = '''+CONVERT(VARCHAR(50),SJ.id)+''' ) 
  BEGIN 
  INSERT INTO JscSubJobs (
	ID,
	Description,
	TableFrom,
	StoredProcName,
	TableNameTo,
	ReplicationMethod,
	WhatToDo,
	Enabled,
	IncludeFlowFields,
	ActionTable,
	ActionCounterInterval,
	MoveActions,
	NoDistributionFilter,
	RepCounterField,
	RepCounterInterval,
	UpdateRepCounter,
	UpdateRepCounterOnEmptyInt,
	MarkSentRecordsField)
  VALUES(
'''+CONVERT(VARCHAR(50),SJ.id)+''',
'''+Description+''',
'+ CASE When TableFrom IS not null then ''''+CONVERT(VARCHAR(50),TableFrom)+ '''' else 'NULL' END +',
' + CASE When StoredProcName IS not null then ''''+StoredProcName+'''' else 'NULL' END+',
' + CASE When TableNameTo IS not null then ''''+TableNameTo+'''' else 'NULL' END +',
' + CONVERT(VARCHAR(5), ReplicationMethod)+',
'+ CONVERT(VARCHAR(5), WhatToDo)+',
'+ CONVERT(VARCHAR(5), Enabled)+',
'+ CONVERT(VARCHAR(5), IncludeFlowFields)+',
'+ CASE When ActionTable IS not null then ''''+CONVERT(VARCHAR(50),ActionTable) + '''' else 'NULL' END +',
'+ CASE When ActionCounterInterval IS not null then CONVERT(VARCHAR(50), ActionCounterInterval) else 'NULL' END +',
'+ CONVERT(VARCHAR(5), MoveActions)+',
'+ CONVERT(VARCHAR(5), NoDistributionFilter)+',
'+ CASE When RepCounterField IS not null then ''''+CONVERT(VARCHAR(50), RepCounterField)+ '''' else 'NULL' END +', 
'+ CASE When RepCounterInterval IS not null then CONVERT(VARCHAR(50), RepCounterInterval) else 'NULL' END +', 
'+ CONVERT(VARCHAR(5), UpdateRepCounter)+', 
'+ CONVERT(VARCHAR(5), UpdateRepCounterOnEmptyInt)+', 
'+ CASE When MarkSentRecordsField IS not null then ''''+CONVERT(VARCHAR(50), MarkSentRecordsField)+ ''')' else 'NULL)' END +'

	
	END
ELSE
	Begin
		Update  JscSubJobs set
			Description='''+Description+''',
			TableFrom='''+ CASE When TableFrom IS not null then CONVERT(VARCHAR(50),TableFrom) else 'NULL' END +''',
			StoredProcName='+ CASE When StoredProcName IS not null then ''''+StoredProcName+'''' else 'NULL' END+',
			TableNameTo='+ CASE When TableNameTo IS not null then ''''+TableNameTo+'''' else 'NULL' END +',
			ReplicationMethod='+ CONVERT(VARCHAR(5), ReplicationMethod)+',
			WhatToDo='+CONVERT(VARCHAR(5), WhatToDo)+',
			Enabled='+ CONVERT(VARCHAR(5), Enabled)+',
			IncludeFlowFields='+ CONVERT(VARCHAR(5), IncludeFlowFields)+',
			ActionTable='+ CASE When ActionTable IS not null then ''''+CONVERT(VARCHAR(50),ActionTable)+ '''' else 'NULL' END +',
			ActionCounterInterval='+ CASE When ActionCounterInterval IS not null then CONVERT(VARCHAR(50), ActionCounterInterval) else 'NULL' END +',
			MoveActions='+ CONVERT(VARCHAR(5), MoveActions)+',
			NoDistributionFilter='+ CONVERT(VARCHAR(5), NoDistributionFilter)+',
			RepCounterField='+ CASE When RepCounterField IS not null then ''''+CONVERT(VARCHAR(50), RepCounterField)+ '''' else 'NULL' END +',
			RepCounterInterval='+ CASE When RepCounterInterval IS not null then CONVERT(VARCHAR(50), RepCounterInterval) else 'NULL' END +',
			UpdateRepCounter='+ CONVERT(VARCHAR(5), UpdateRepCounter)+',
			UpdateRepCounterOnEmptyInt='+ CONVERT(VARCHAR(5), UpdateRepCounterOnEmptyInt)+',
			MarkSentRecordsField='+ CASE When MarkSentRecordsField IS not null then ''''+CONVERT(VARCHAR(50), MarkSentRecordsField)+ '''' else 'NULL' END +'
		WHERE ID = '''+CONVERT(VARCHAR(50),SJ.id)+'''
	END'id



  FROM (
  SELECT  ID -- USE newid() AS id if you need new ones
      ,Description
      ,TableFrom
      ,StoredProcName
      ,TableNameTo
      ,ReplicationMethod
      ,WhatToDo
      ,Enabled
      ,IncludeFlowFields
      ,ActionTable
      ,ActionCounterInterval
      ,MoveActions
      ,NoDistributionFilter
      ,RepCounterField
      ,RepCounterInterval
      ,UpdateRepCounter
      ,UpdateRepCounterOnEmptyInt
      ,MarkSentRecordsField
  FROM JscSubJobs 
  --WHERE ID = 'D331DA96-DFA5-46E2-945C-253CCF83ABC8'
  ) SJ--	WHERE I D = 'CF5CB17C-85C9-43A9-B4B2-10A44B70B604'
	
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/

Use LSPOSNET 
GO  

GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5BACD73D-891B-4875-9B6F-00FF6B367175')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5BACD73D-891B-4875-9B6F-00FF6B367175', 'N-CUSTGROUPCATEGO  RY', 'D3598CA9-26F0-47C6-BADA-381C66457131', NULL, 'CUSTGROUPCATEGO  RY', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CUSTGROUPCATEGO  RY',
      TableFrom = 'D3598CA9-26F0-47C6-BADA-381C66457131',
      StoredProcName = NULL,
      TableNameTo = 'CUSTGROUPCATEGO  RY',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5BACD73D-891B-4875-9B6F-00FF6B367175'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1588EC17-FC8D-4E69-8CB5-01088C5D9CB0')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1588EC17-FC8D-4E69-8CB5-01088C5D9CB0', 'N-RBOBARCODEMASKTABLE', '2616CC4D-BEDB-4187-BDEA-1E19314AB38A', NULL, 'RBOBARCODEMASKTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOBARCODEMASKTABLE',
      TableFrom = '2616CC4D-BEDB-4187-BDEA-1E19314AB38A',
      StoredProcName = NULL,
      TableNameTo = 'RBOBARCODEMASKTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1588EC17-FC8D-4E69-8CB5-01088C5D9CB0'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AE7C6E7C-AB6E-44EE-83AD-01F8C92164E4')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AE7C6E7C-AB6E-44EE-83AD-01F8C92164E4', 'N-EXCHRATES', '6F6B35B8-C287-441A-9697-CC67EC20E7E0', NULL, 'EXCHRATES', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-EXCHRATES',
      TableFrom = '6F6B35B8-C287-441A-9697-CC67EC20E7E0',
      StoredProcName = NULL,
      TableNameTo = 'EXCHRATES',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AE7C6E7C-AB6E-44EE-83AD-01F8C92164E4'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D0FE9D36-FFE7-4ABE-814E-036AAFC80B3A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D0FE9D36-FFE7-4ABE-814E-036AAFC80B3A', 'A-POSISPARAMETERS', '1A551459-6318-49CF-9BF8-BC01761031A5', NULL, 'POSISPARAMETERS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISPARAMETERS',
      TableFrom = '1A551459-6318-49CF-9BF8-BC01761031A5',
      StoredProcName = NULL,
      TableNameTo = 'POSISPARAMETERS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D0FE9D36-FFE7-4ABE-814E-036AAFC80B3A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '77AF025C-B0F3-4267-B00B-037903A96DA5')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('77AF025C-B0F3-4267-B00B-037903A96DA5', 'A-POSMULTIBUYDISCOUNTLINE', 'EF26E8D0-75FC-429A-9FAB-BB038B2AF949', NULL, 'POSMULTIBUYDISCOUNTLINE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSMULTIBUYDISCOUNTLINE',
      TableFrom = 'EF26E8D0-75FC-429A-9FAB-BB038B2AF949',
      StoredProcName = NULL,
      TableNameTo = 'POSMULTIBUYDISCOUNTLINE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '77AF025C-B0F3-4267-B00B-037903A96DA5'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3DBAEF0B-B8D0-4107-A6A5-03896D1E973C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3DBAEF0B-B8D0-4107-A6A5-03896D1E973C', 'A-POSUSERPROFILE', '4DFD502D-2A40-4759-8766-D7BCBEA9CEBD', NULL, 'POSUSERPROFILE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSUSERPROFILE',
      TableFrom = '4DFD502D-2A40-4759-8766-D7BCBEA9CEBD',
      StoredProcName = NULL,
      TableNameTo = 'POSUSERPROFILE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3DBAEF0B-B8D0-4107-A6A5-03896D1E973C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1518BC42-38FC-4A40-A7D1-0392A3E9360A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1518BC42-38FC-4A40-A7D1-0392A3E9360A', 'A-UNIT', 'B8DF2290-C1D3-4EC4-ABAF-F2C508F43268', NULL, 'UNIT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-UNIT',
      TableFrom = 'B8DF2290-C1D3-4EC4-ABAF-F2C508F43268',
      StoredProcName = NULL,
      TableNameTo = 'UNIT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1518BC42-38FC-4A40-A7D1-0392A3E9360A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AB79E9C1-86F7-438D-8ED3-03F1A0686C50')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AB79E9C1-86F7-438D-8ED3-03F1A0686C50', 'A-POSISOILTAX', '8F9EE93C-597C-48E8-8E39-24CE8C432A31', NULL, 'POSISOILTAX', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISOILTAX',
      TableFrom = '8F9EE93C-597C-48E8-8E39-24CE8C432A31',
      StoredProcName = NULL,
      TableNameTo = 'POSISOILTAX',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AB79E9C1-86F7-438D-8ED3-03F1A0686C50'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'FF3F6F6E-4974-45DA-9A9C-04CC6C15AF53')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('FF3F6F6E-4974-45DA-9A9C-04CC6C15AF53', 'N-RBOSPECIALGROUP', '69E136DC-6B9A-4F45-B3FC-9EA6130605D8', NULL, 'RBOSPECIALGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSPECIALGROUP',
      TableFrom = '69E136DC-6B9A-4F45-B3FC-9EA6130605D8',
      StoredProcName = NULL,
      TableNameTo = 'RBOSPECIALGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'FF3F6F6E-4974-45DA-9A9C-04CC6C15AF53'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6A6E938E-1ABA-4A17-AB66-052E8113EF6D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6A6E938E-1ABA-4A17-AB66-052E8113EF6D', 'N-PRICEDISCTABLE', 'AA30FBEC-9AD3-45F3-B1E7-FB91059C0FBB', NULL, 'PRICEDISCTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-PRICEDISCTABLE',
      TableFrom = 'AA30FBEC-9AD3-45F3-B1E7-FB91059C0FBB',
      StoredProcName = NULL,
      TableNameTo = 'PRICEDISCTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6A6E938E-1ABA-4A17-AB66-052E8113EF6D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2BBB35CF-D03D-417B-8637-06236D31D295')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2BBB35CF-D03D-417B-8637-06236D31D295', 'A-GOODSRECEIVINGLINE', '0830F8E2-659F-48C3-927B-41982B442506', NULL, 'GOODSRECEIVINGLINE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-GOODSRECEIVINGLINE',
      TableFrom = '0830F8E2-659F-48C3-927B-41982B442506',
      StoredProcName = NULL,
      TableNameTo = 'GOODSRECEIVINGLINE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2BBB35CF-D03D-417B-8637-06236D31D295'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '56834ADB-5901-4D92-B64B-0746FCD80BA4')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('56834ADB-5901-4D92-B64B-0746FCD80BA4', 'A-CUSTGROUPCATEGORY', '9B66DCAC-B01A-4484-84A3-F94CF2DDD672', NULL, 'CUSTGROUPCATEGORY', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CUSTGROUPCATEGORY',
      TableFrom = '9B66DCAC-B01A-4484-84A3-F94CF2DDD672',
      StoredProcName = NULL,
      TableNameTo = 'CUSTGROUPCATEGORY',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '56834ADB-5901-4D92-B64B-0746FCD80BA4'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'BBEA9858-F3B3-42A2-B50A-0747829B6775')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('BBEA9858-F3B3-42A2-B50A-0747829B6775', 'A-REPORTS', '8826F144-4B13-40D4-8481-BAF6C6894C7E', NULL, 'REPORTS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-REPORTS',
      TableFrom = '8826F144-4B13-40D4-8481-BAF6C6894C7E',
      StoredProcName = NULL,
      TableNameTo = 'REPORTS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'BBEA9858-F3B3-42A2-B50A-0747829B6775'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4BAEB8C7-C31D-4EAB-9047-0891B13FC83C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4BAEB8C7-C31D-4EAB-9047-0891B13FC83C', 'A-REGION', '8CBB00EB-9A95-4010-AE00-679A287BFA09', NULL, 'REGION', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-REGION',
      TableFrom = '8CBB00EB-9A95-4010-AE00-679A287BFA09',
      StoredProcName = NULL,
      TableNameTo = 'REGION',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4BAEB8C7-C31D-4EAB-9047-0891B13FC83C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '12416114-448A-4CA1-8894-098A9C8C3DF5')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('12416114-448A-4CA1-8894-098A9C8C3DF5', 'A-PERIODICDISCOUNT', 'CFBB6B18-ABEA-4E00-8244-1CA673A8863A', NULL, 'PERIODICDISCOUNT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-PERIODICDISCOUNT',
      TableFrom = 'CFBB6B18-ABEA-4E00-8244-1CA673A8863A',
      StoredProcName = NULL,
      TableNameTo = 'PERIODICDISCOUNT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '12416114-448A-4CA1-8894-098A9C8C3DF5'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A6A1806C-B54C-48B4-AB18-09C8C07EC30B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A6A1806C-B54C-48B4-AB18-09C8C07EC30B', 'A-VENDTABLE', '01B318E1-3A18-4A0E-A51F-A07C9450EFAC', NULL, 'VENDTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-VENDTABLE',
      TableFrom = '01B318E1-3A18-4A0E-A51F-A07C9450EFAC',
      StoredProcName = NULL,
      TableNameTo = 'VENDTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A6A1806C-B54C-48B4-AB18-09C8C07EC30B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'C593AF6F-77F4-4C3B-82EB-0A6022F94D2E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('C593AF6F-77F4-4C3B-82EB-0A6022F94D2E', 'N-POSISLANGUAGETEXT', '4C746A04-7629-4022-9DF7-DED041A71D37', NULL, 'POSISLANGUAGETEXT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISLANGUAGETEXT',
      TableFrom = '4C746A04-7629-4022-9DF7-DED041A71D37',
      StoredProcName = NULL,
      TableNameTo = 'POSISLANGUAGETEXT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'C593AF6F-77F4-4C3B-82EB-0A6022F94D2E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'DF8A4FB4-C4A7-4E2F-AB5A-0B41FCE03ACE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('DF8A4FB4-C4A7-4E2F-AB5A-0B41FCE03ACE', 'A-PRICEGROUP', 'E36F44D8-C163-4269-8E00-2D3879BC30F2', NULL, 'PRICEGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-PRICEGROUP',
      TableFrom = 'E36F44D8-C163-4269-8E00-2D3879BC30F2',
      StoredProcName = NULL,
      TableNameTo = 'PRICEGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'DF8A4FB4-C4A7-4E2F-AB5A-0B41FCE03ACE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '28C5FA77-267A-422D-8F52-0C18601A3E0E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('28C5FA77-267A-422D-8F52-0C18601A3E0E', 'A-RBOINFORMATIONSUBCODETABLE', '0BFFBC04-4BB1-49D8-8DE6-416AB28297A6', NULL, 'RBOINFORMATIONSUBCODETABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOINFORMATIONSUBCODETABLE',
      TableFrom = '0BFFBC04-4BB1-49D8-8DE6-416AB28297A6',
      StoredProcName = NULL,
      TableNameTo = 'RBOINFORMATIONSUBCODETABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '28C5FA77-267A-422D-8F52-0C18601A3E0E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A59C66B7-9776-4E1B-9657-0D0D5E301ECC')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A59C66B7-9776-4E1B-9657-0D0D5E301ECC', 'N-TAXREFUNDRANGE', '23EE7EA6-8AE0-4519-8155-94DA15037A96', NULL, 'TAXREFUNDRANGE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TAXREFUNDRANGE',
      TableFrom = '23EE7EA6-8AE0-4519-8155-94DA15037A96',
      StoredProcName = NULL,
      TableNameTo = 'TAXREFUNDRANGE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A59C66B7-9776-4E1B-9657-0D0D5E301ECC'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5D0C76F5-E97A-4343-9ABE-0DF97980E28C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5D0C76F5-E97A-4343-9ABE-0DF97980E28C', 'A-EMAILSETTINGS', '288BB85B-F461-4F19-AF5F-6D9AF6CDD240', NULL, 'EMAILSETTINGS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-EMAILSETTINGS',
      TableFrom = '288BB85B-F461-4F19-AF5F-6D9AF6CDD240',
      StoredProcName = NULL,
      TableNameTo = 'EMAILSETTINGS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5D0C76F5-E97A-4343-9ABE-0DF97980E28C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'DDC165BA-53DC-44A5-A252-0F35B2C2C2F0')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('DDC165BA-53DC-44A5-A252-0F35B2C2C2F0', 'N-RETAILITEMIMAGE', 'F797F1D8-98A0-44A0-A451-EFC75AB46F21', NULL, 'RETAILITEMIMAGE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RETAILITEMIMAGE',
      TableFrom = 'F797F1D8-98A0-44A0-A451-EFC75AB46F21',
      StoredProcName = NULL,
      TableNameTo = 'RETAILITEMIMAGE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'DDC165BA-53DC-44A5-A252-0F35B2C2C2F0'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '64E53934-D239-4A19-809D-0F7AD4F7E35E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('64E53934-D239-4A19-809D-0F7AD4F7E35E', 'N-RBOTRANSACTIONSAFETENDERTRANS', 'B1FF1A98-5CF6-4CFC-9B61-EA0C4D9DAE5E', NULL, 'RBOTRANSACTIONSAFETENDERTRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, 'A14603D5-99A5-4CC6-B17F-2E181BB5F4D9', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONSAFETENDERTRANS',
      TableFrom = 'B1FF1A98-5CF6-4CFC-9B61-EA0C4D9DAE5E',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONSAFETENDERTRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = 'A14603D5-99A5-4CC6-B17F-2E181BB5F4D9',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '64E53934-D239-4A19-809D-0F7AD4F7E35E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'EDE9AA0A-57EC-462C-819F-100449FAA928')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('EDE9AA0A-57EC-462C-819F-100449FAA928', 'A-RBOSTORETABLE', '4EEC0E6F-F21C-4B3D-A2B1-DFC2C0C2A0BE', NULL, 'RBOSTORETABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTORETABLE',
      TableFrom = '4EEC0E6F-F21C-4B3D-A2B1-DFC2C0C2A0BE',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTORETABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'EDE9AA0A-57EC-462C-819F-100449FAA928'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '86DC65CA-726C-494A-AB89-1053F297E800')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('86DC65CA-726C-494A-AB89-1053F297E800', 'P-spMAINT_Set', NULL, 'spMAINT_Set', NULL, 2, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'P-spMAINT_Set',
      TableFrom = NULL,
      StoredProcName = 'spMAINT_Set',
      TableNameTo = NULL,
      ReplicationMethod = 2,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '86DC65CA-726C-494A-AB89-1053F297E800'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'CF5CB17C-85C9-43A9-B4B2-10A44B70B604')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('CF5CB17C-85C9-43A9-B4B2-10A44B70B604', 'N-STATIONPRINTINGROUTES', 'F5CCBC69-5B9A-4630-8B1E-6C50E49F6EED', NULL, 'STATIONPRINTINGROUTES', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-STATIONPRINTINGROUTES',
      TableFrom = 'F5CCBC69-5B9A-4630-8B1E-6C50E49F6EED',
      StoredProcName = NULL,
      TableNameTo = 'STATIONPRINTINGROUTES',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'CF5CB17C-85C9-43A9-B4B2-10A44B70B604'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '912A7041-1727-447D-8387-10A657047342')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('912A7041-1727-447D-8387-10A657047342', 'A-POSISFUELLINGPOINTSOUNDS', '6B524099-44A9-44E2-978D-F658952C65ED', NULL, 'POSISFUELLINGPOINTSOUNDS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISFUELLINGPOINTSOUNDS',
      TableFrom = '6B524099-44A9-44E2-978D-F658952C65ED',
      StoredProcName = NULL,
      TableNameTo = 'POSISFUELLINGPOINTSOUNDS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '912A7041-1727-447D-8387-10A657047342'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '45DF92C8-409D-4EE8-AEB5-119BF30CC9F4')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('45DF92C8-409D-4EE8-AEB5-119BF30CC9F4', 'N-RBOTRANSACTIONTENDERDECLA20165', '731387D9-6894-477D-876B-910E51E61E7F', NULL, 'RBOTRANSACTIONTENDERDECLA20165', 0, 1, 1, 0, NULL, NULL, 0, 0, '6F3BA125-B974-4474-A452-D7034ED4BBDD', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONTENDERDECLA20165',
      TableFrom = '731387D9-6894-477D-876B-910E51E61E7F',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONTENDERDECLA20165',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '6F3BA125-B974-4474-A452-D7034ED4BBDD',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '45DF92C8-409D-4EE8-AEB5-119BF30CC9F4'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A19C9B50-EE0C-47E2-9527-11B99C888E5E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A19C9B50-EE0C-47E2-9527-11B99C888E5E', 'A-RBOINFOCODETABLESPECIFIC', '6628A2F4-1387-4EE4-AF51-4BBF4B78F33D', NULL, 'RBOINFOCODETABLESPECIFIC', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOINFOCODETABLESPECIFIC',
      TableFrom = '6628A2F4-1387-4EE4-AF51-4BBF4B78F33D',
      StoredProcName = NULL,
      TableNameTo = 'RBOINFOCODETABLESPECIFIC',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A19C9B50-EE0C-47E2-9527-11B99C888E5E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '7EC20E15-478C-4D18-8DAF-125365E097CE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('7EC20E15-478C-4D18-8DAF-125365E097CE', 'N-RBOLOYALTYPOINTSTABLE', '3D36B809-5A68-4041-960D-F72F9D4B1355', NULL, 'RBOLOYALTYPOINTSTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOLOYALTYPOINTSTABLE',
      TableFrom = '3D36B809-5A68-4041-960D-F72F9D4B1355',
      StoredProcName = NULL,
      TableNameTo = 'RBOLOYALTYPOINTSTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '7EC20E15-478C-4D18-8DAF-125365E097CE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '26EFDA98-F3DB-4268-8D13-1255EFE07803')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('26EFDA98-F3DB-4268-8D13-1255EFE07803', 'N-POSTRANSACTIONSERVICEPROFILESHORTHAND', 'D15442D2-3BD5-434E-A5FB-505BA2DD5E65', NULL, 'POSTRANSACTIONSERVICEPROFILESHORTHAND', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSTRANSACTIONSERVICEPROFILESHORTHAND',
      TableFrom = 'D15442D2-3BD5-434E-A5FB-505BA2DD5E65',
      StoredProcName = NULL,
      TableNameTo = 'POSTRANSACTIONSERVICEPROFILESHORTHAND',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '26EFDA98-F3DB-4268-8D13-1255EFE07803'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '77F2E476-CC89-4E1D-B2BA-13EB16DF1260')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('77F2E476-CC89-4E1D-B2BA-13EB16DF1260', 'N-EMAILSETTINGS', '288BB85B-F461-4F19-AF5F-6D9AF6CDD240', NULL, 'EMAILSETTINGS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-EMAILSETTINGS',
      TableFrom = '288BB85B-F461-4F19-AF5F-6D9AF6CDD240',
      StoredProcName = NULL,
      TableNameTo = 'EMAILSETTINGS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '77F2E476-CC89-4E1D-B2BA-13EB16DF1260'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'EB3B8F12-9B3E-41CF-9D37-141BE69024EE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('EB3B8F12-9B3E-41CF-9D37-141BE69024EE', 'N-RBOTRANSACTIONTAXTRANS', '1C44CC81-278C-4EB4-9B64-2075D6E2A012', NULL, 'RBOTRANSACTIONTAXTRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, '5F4FC252-8E2F-4E79-B1F0-F7FF4568339E', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONTAXTRANS',
      TableFrom = '1C44CC81-278C-4EB4-9B64-2075D6E2A012',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONTAXTRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '5F4FC252-8E2F-4E79-B1F0-F7FF4568339E',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'EB3B8F12-9B3E-41CF-9D37-141BE69024EE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '86FAC7BC-9C9E-40B5-9FF3-150A7D961259')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('86FAC7BC-9C9E-40B5-9FF3-150A7D961259', 'N-RBOTERMINALTABLE', 'B2D4CDE3-3F64-4C40-9FF5-505A052CDF7D', NULL, 'RBOTERMINALTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTERMINALTABLE',
      TableFrom = 'B2D4CDE3-3F64-4C40-9FF5-505A052CDF7D',
      StoredProcName = NULL,
      TableNameTo = 'RBOTERMINALTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '86FAC7BC-9C9E-40B5-9FF3-150A7D961259'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4294CF52-FE00-45E1-B599-15ED58721836')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4294CF52-FE00-45E1-B599-15ED58721836', 'A-RBOSTATEMENTLINE', 'D76FDF76-4254-4E29-A44A-51F2855F5BD5', NULL, 'RBOSTATEMENTLINE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTATEMENTLINE',
      TableFrom = 'D76FDF76-4254-4E29-A44A-51F2855F5BD5',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTATEMENTLINE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4294CF52-FE00-45E1-B599-15ED58721836'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2404FD0E-6FC8-4EF3-8AFE-16A4F9380BF1')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2404FD0E-6FC8-4EF3-8AFE-16A4F9380BF1', 'N-POSMULTIBUYDISCOUNTLINE', 'EF26E8D0-75FC-429A-9FAB-BB038B2AF949', NULL, 'POSMULTIBUYDISCOUNTLINE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSMULTIBUYDISCOUNTLINE',
      TableFrom = 'EF26E8D0-75FC-429A-9FAB-BB038B2AF949',
      StoredProcName = NULL,
      TableNameTo = 'POSMULTIBUYDISCOUNTLINE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2404FD0E-6FC8-4EF3-8AFE-16A4F9380BF1'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '37152B4D-C90D-49DF-ADA9-16C5CF715E03')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('37152B4D-C90D-49DF-ADA9-16C5CF715E03', 'A-DECIMALSETTINGS', '82D3B816-58FC-4D02-9376-00EFC71630A1', NULL, 'DECIMALSETTINGS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-DECIMALSETTINGS',
      TableFrom = '82D3B816-58FC-4D02-9376-00EFC71630A1',
      StoredProcName = NULL,
      TableNameTo = 'DECIMALSETTINGS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '37152B4D-C90D-49DF-ADA9-16C5CF715E03'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AD2F955C-6F8D-430C-911F-17BB6DF71321')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AD2F955C-6F8D-430C-911F-17BB6DF71321', 'A-INVENTDIM', '4627C23C-5BFE-41B3-9F31-D91C24BF1FB9', NULL, 'INVENTDIM', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-INVENTDIM',
      TableFrom = '4627C23C-5BFE-41B3-9F31-D91C24BF1FB9',
      StoredProcName = NULL,
      TableNameTo = 'INVENTDIM',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AD2F955C-6F8D-430C-911F-17BB6DF71321'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D2D6BCDE-1676-40AE-8640-181F83362EEA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D2D6BCDE-1676-40AE-8640-181F83362EEA', 'A-RBOCOLORGROUPTABLE', '6301F83F-DE63-4E10-83E7-A32F38D89700', NULL, 'RBOCOLORGROUPTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOCOLORGROUPTABLE',
      TableFrom = '6301F83F-DE63-4E10-83E7-A32F38D89700',
      StoredProcName = NULL,
      TableNameTo = 'RBOCOLORGROUPTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D2D6BCDE-1676-40AE-8640-181F83362EEA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '9353B420-64F0-43B5-89DA-19DB5AC13190')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('9353B420-64F0-43B5-89DA-19DB5AC13190', 'A-RBOSTORETENDERTYPETABLE', '85DE7B50-929A-4DDA-9643-3886DA71DEAF', NULL, 'RBOSTORETENDERTYPETABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTORETENDERTYPETABLE',
      TableFrom = '85DE7B50-929A-4DDA-9643-3886DA71DEAF',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTORETENDERTYPETABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '9353B420-64F0-43B5-89DA-19DB5AC13190'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '709C1191-E176-4640-859B-1A37993ECF5E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('709C1191-E176-4640-859B-1A37993ECF5E', 'A-POSISCARDTOTENDERMAPPING', 'F17F8BF3-5528-44D5-A1C4-CD0BE26D50AF', NULL, 'POSISCARDTOTENDERMAPPING', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISCARDTOTENDERMAPPING',
      TableFrom = 'F17F8BF3-5528-44D5-A1C4-CD0BE26D50AF',
      StoredProcName = NULL,
      TableNameTo = 'POSISCARDTOTENDERMAPPING',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '709C1191-E176-4640-859B-1A37993ECF5E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '67B52E50-291B-48C4-9604-1B8FBEC3F7E9')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('67B52E50-291B-48C4-9604-1B8FBEC3F7E9', 'A-POSTRANSACTIONSERVICEPROFILESHORTHAND', 'D15442D2-3BD5-434E-A5FB-505BA2DD5E65', NULL, 'POSTRANSACTIONSERVICEPROFILESHORTHAND', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSTRANSACTIONSERVICEPROFILESHORTHAND',
      TableFrom = 'D15442D2-3BD5-434E-A5FB-505BA2DD5E65',
      StoredProcName = NULL,
      TableNameTo = 'POSTRANSACTIONSERVICEPROFILESHORTHAND',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '67B52E50-291B-48C4-9604-1B8FBEC3F7E9'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D605BF5D-2572-4824-8081-1BE674DADB08')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D605BF5D-2572-4824-8081-1BE674DADB08', 'A-RBOINVENTITEMDEPARTMENT', '35AA442C-AE96-4516-A975-0565F17D7B1D', NULL, 'RBOINVENTITEMDEPARTMENT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOINVENTITEMDEPARTMENT',
      TableFrom = '35AA442C-AE96-4516-A975-0565F17D7B1D',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTITEMDEPARTMENT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D605BF5D-2572-4824-8081-1BE674DADB08'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6AACBD60-4E7D-497F-BC8D-1C6A3B8225D1')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6AACBD60-4E7D-497F-BC8D-1C6A3B8225D1', 'N-POSISHOSPITALITYOPERATIONS', 'CA9F007E-F334-4389-8AA1-E41844A8E117', NULL, 'POSISHOSPITALITYOPERATIONS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISHOSPITALITYOPERATIONS',
      TableFrom = 'CA9F007E-F334-4389-8AA1-E41844A8E117',
      StoredProcName = NULL,
      TableNameTo = 'POSISHOSPITALITYOPERATIONS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6AACBD60-4E7D-497F-BC8D-1C6A3B8225D1'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '556CF353-57E8-4E40-A4D3-1C8D1F9AFD3D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('556CF353-57E8-4E40-A4D3-1C8D1F9AFD3D', 'A-DIMENSIONTEMPLATE', '5FB9E1EF-FF24-4788-8576-477920259DE8', NULL, 'DIMENSIONTEMPLATE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-DIMENSIONTEMPLATE',
      TableFrom = '5FB9E1EF-FF24-4788-8576-477920259DE8',
      StoredProcName = NULL,
      TableNameTo = 'DIMENSIONTEMPLATE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '556CF353-57E8-4E40-A4D3-1C8D1F9AFD3D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3B055513-1EBA-4150-ADC5-1CBA96E550AC')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3B055513-1EBA-4150-ADC5-1CBA96E550AC', 'A-RBOCOLORS', '141724F1-F897-47B8-9800-98261BDF0E76', NULL, 'RBOCOLORS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOCOLORS',
      TableFrom = '141724F1-F897-47B8-9800-98261BDF0E76',
      StoredProcName = NULL,
      TableNameTo = 'RBOCOLORS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3B055513-1EBA-4150-ADC5-1CBA96E550AC'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '788CCACC-386B-4BAD-9B1E-1D144E213D11')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('788CCACC-386B-4BAD-9B1E-1D144E213D11', 'A-RESTAURANTDININGTABLEDESIGN', 'ED23AF38-3687-4740-A322-37ABBFDA4981', NULL, 'RESTAURANTDININGTABLEDESIGN', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RESTAURANTDININGTABLEDESIGN',
      TableFrom = 'ED23AF38-3687-4740-A322-37ABBFDA4981',
      StoredProcName = NULL,
      TableNameTo = 'RESTAURANTDININGTABLEDESIGN',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '788CCACC-386B-4BAD-9B1E-1D144E213D11'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '566A0222-07A9-4D95-92E5-1DF13CF64861')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('566A0222-07A9-4D95-92E5-1DF13CF64861', 'N-STATIONPRINTINGROUTES', 'F5CCBC69-5B9A-4630-8B1E-6C50E49F6EED', NULL, 'STATIONPRINTINGROUTES', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-STATIONPRINTINGROUTES',
      TableFrom = 'F5CCBC69-5B9A-4630-8B1E-6C50E49F6EED',
      StoredProcName = NULL,
      TableNameTo = 'STATIONPRINTINGROUTES',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '566A0222-07A9-4D95-92E5-1DF13CF64861'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'FB804C2D-DC22-4380-95A4-1E0D516CA5BB')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('FB804C2D-DC22-4380-95A4-1E0D516CA5BB', 'A-RBOBARCODEMASKSEGMENT', '4F0A2FC6-553D-47D6-8A89-4C95BFA66419', NULL, 'RBOBARCODEMASKSEGMENT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOBARCODEMASKSEGMENT',
      TableFrom = '4F0A2FC6-553D-47D6-8A89-4C95BFA66419',
      StoredProcName = NULL,
      TableNameTo = 'RBOBARCODEMASKSEGMENT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'FB804C2D-DC22-4380-95A4-1E0D516CA5BB'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '36259A54-8835-49EC-8521-20B8DB74E084')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('36259A54-8835-49EC-8521-20B8DB74E084', 'A-TAXREFUND', '56C98F6C-997F-4E13-A769-6A84C78530D7', NULL, 'TAXREFUND', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TAXREFUND',
      TableFrom = '56C98F6C-997F-4E13-A769-6A84C78530D7',
      StoredProcName = NULL,
      TableNameTo = 'TAXREFUND',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '36259A54-8835-49EC-8521-20B8DB74E084'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '205FF34B-8411-438A-8AF1-21058C266A4C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('205FF34B-8411-438A-8AF1-21058C266A4C', 'N-VENDTABLE', '01B318E1-3A18-4A0E-A51F-A07C9450EFAC', NULL, 'VENDTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-VENDTABLE',
      TableFrom = '01B318E1-3A18-4A0E-A51F-A07C9450EFAC',
      StoredProcName = NULL,
      TableNameTo = 'VENDTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '205FF34B-8411-438A-8AF1-21058C266A4C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '561DB78B-22AA-4352-9530-216A7CEED768')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('561DB78B-22AA-4352-9530-216A7CEED768', 'N-USERSETTINGS', '14C47C00-1A17-48B3-A731-34D3DE1B694A', NULL, 'USERSETTINGS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-USERSETTINGS',
      TableFrom = '14C47C00-1A17-48B3-A731-34D3DE1B694A',
      StoredProcName = NULL,
      TableNameTo = 'USERSETTINGS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '561DB78B-22AA-4352-9530-216A7CEED768'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3506F9D5-0425-4FBF-A5B0-21B163171C3C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3506F9D5-0425-4FBF-A5B0-21B163171C3C', 'N-POSPARAMETERSETUP', '199C3808-1249-45D9-8484-66AE863BE916', NULL, 'POSPARAMETERSETUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSPARAMETERSETUP',
      TableFrom = '199C3808-1249-45D9-8484-66AE863BE916',
      StoredProcName = NULL,
      TableNameTo = 'POSPARAMETERSETUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3506F9D5-0425-4FBF-A5B0-21B163171C3C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '8FE39EC9-F5AC-4055-9D6C-21D7B0829945')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('8FE39EC9-F5AC-4055-9D6C-21D7B0829945', 'N-REGION', '8CBB00EB-9A95-4010-AE00-679A287BFA09', NULL, 'REGION', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-REGION',
      TableFrom = '8CBB00EB-9A95-4010-AE00-679A287BFA09',
      StoredProcName = NULL,
      TableNameTo = 'REGION',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '8FE39EC9-F5AC-4055-9D6C-21D7B0829945'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '343E136F-6219-4DC0-8BEA-21EBA0A116A4')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('343E136F-6219-4DC0-8BEA-21EBA0A116A4', 'A-TAXTABLE', 'DB1D8C68-C922-427A-8764-B14AA9502963', NULL, 'TAXTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TAXTABLE',
      TableFrom = 'DB1D8C68-C922-427A-8764-B14AA9502963',
      StoredProcName = NULL,
      TableNameTo = 'TAXTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '343E136F-6219-4DC0-8BEA-21EBA0A116A4'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '9F913D3A-D154-4E61-BF73-225C0E0DDD54')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('9F913D3A-D154-4E61-BF73-225C0E0DDD54', 'N-RBODISCOUNTOFFERTABLE', 'CCFFD680-11E3-4880-BE8C-9502E8DF689B', NULL, 'RBODISCOUNTOFFERTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBODISCOUNTOFFERTABLE',
      TableFrom = 'CCFFD680-11E3-4880-BE8C-9502E8DF689B',
      StoredProcName = NULL,
      TableNameTo = 'RBODISCOUNTOFFERTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '9F913D3A-D154-4E61-BF73-225C0E0DDD54'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AED9E687-459C-4460-B3BB-22CD7D8BF92F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AED9E687-459C-4460-B3BB-22CD7D8BF92F', 'A-RETAILITEMDIMENSIONATTRIBUTE', '02701AB4-0CD3-447E-9E46-E0CB747C016D', NULL, 'RETAILITEMDIMENSIONATTRIBUTE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RETAILITEMDIMENSIONATTRIBUTE',
      TableFrom = '02701AB4-0CD3-447E-9E46-E0CB747C016D',
      StoredProcName = NULL,
      TableNameTo = 'RETAILITEMDIMENSIONATTRIBUTE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AED9E687-459C-4460-B3BB-22CD7D8BF92F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '541F262D-3995-4EAF-BEDF-23A19AB34DB7')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('541F262D-3995-4EAF-BEDF-23A19AB34DB7', 'A-INVENTTRANSREASON', '66C2A2D8-89F8-4933-A3BC-642BD0FC5E00', NULL, 'INVENTTRANSREASON', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-INVENTTRANSREASON',
      TableFrom = '66C2A2D8-89F8-4933-A3BC-642BD0FC5E00',
      StoredProcName = NULL,
      TableNameTo = 'INVENTTRANSREASON',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '541F262D-3995-4EAF-BEDF-23A19AB34DB7'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F39D0A11-D70E-4A9A-9B0A-23D8C2DD25C1')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F39D0A11-D70E-4A9A-9B0A-23D8C2DD25C1', 'N-JscJobTriggers', '6B80EED7-23B7-4306-A92B-785B15700494', NULL, 'JscJobTriggers', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscJobTriggers',
      TableFrom = '6B80EED7-23B7-4306-A92B-785B15700494',
      StoredProcName = NULL,
      TableNameTo = 'JscJobTriggers',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F39D0A11-D70E-4A9A-9B0A-23D8C2DD25C1'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '7D4B1A15-EF6D-45E7-9ED6-23DE3B0EC765')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('7D4B1A15-EF6D-45E7-9ED6-23DE3B0EC765', 'N-RBOSTYLEGROUPTRANS', '20180F86-1C46-484F-8C55-56D530662197', NULL, 'RBOSTYLEGROUPTRANS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSTYLEGROUPTRANS',
      TableFrom = '20180F86-1C46-484F-8C55-56D530662197',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTYLEGROUPTRANS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '7D4B1A15-EF6D-45E7-9ED6-23DE3B0EC765'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '56CA6490-9C42-4E24-B944-24159BDAEAF4')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('56CA6490-9C42-4E24-B944-24159BDAEAF4', 'N-PRICEPARAMETERS', '1A9BE9CE-3053-486D-A74B-6597FFE8A6C5', NULL, 'PRICEPARAMETERS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-PRICEPARAMETERS',
      TableFrom = '1A9BE9CE-3053-486D-A74B-6597FFE8A6C5',
      StoredProcName = NULL,
      TableNameTo = 'PRICEPARAMETERS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '56CA6490-9C42-4E24-B944-24159BDAEAF4'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '57E60851-091B-495F-91A0-245387355491')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('57E60851-091B-495F-91A0-245387355491', 'A-RETAILITEM', '1631024E-D4BB-4F4A-8FFC-830636E3AB9B', NULL, 'RETAILITEM', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RETAILITEM',
      TableFrom = '1631024E-D4BB-4F4A-8FFC-830636E3AB9B',
      StoredProcName = NULL,
      TableNameTo = 'RETAILITEM',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '57E60851-091B-495F-91A0-245387355491'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5F460DB6-21C3-4288-AB8D-2494505161D7')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5F460DB6-21C3-4288-AB8D-2494505161D7', 'A-TAXCOLLECTLIMIT', 'F5A6179B-DE3D-44CB-8F5F-3EEEDD505E20', NULL, 'TAXCOLLECTLIMIT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TAXCOLLECTLIMIT',
      TableFrom = 'F5A6179B-DE3D-44CB-8F5F-3EEEDD505E20',
      StoredProcName = NULL,
      TableNameTo = 'TAXCOLLECTLIMIT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5F460DB6-21C3-4288-AB8D-2494505161D7'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5400F32B-839C-43F0-B2A5-24A6D295156A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5400F32B-839C-43F0-B2A5-24A6D295156A', 'N-TAXITEMGROUPHEADING', '59794233-3CAF-4FE5-9919-A4E2DE3AE5A3', NULL, 'TAXITEMGROUPHEADING', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TAXITEMGROUPHEADING',
      TableFrom = '59794233-3CAF-4FE5-9919-A4E2DE3AE5A3',
      StoredProcName = NULL,
      TableNameTo = 'TAXITEMGROUPHEADING',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5400F32B-839C-43F0-B2A5-24A6D295156A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D331DA96-DFA5-46E2-945C-253CCF83ABC8')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D331DA96-DFA5-46E2-945C-253CCF83ABC8', 'P-spSECURITY_DeleteAuditLogs', NULL, 'spSECURITY_DeleteAuditLogs_1_0', NULL, 2, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'P-spSECURITY_DeleteAuditLogs',
      TableFrom = 'NULL',
      StoredProcName = 'spSECURITY_DeleteAuditLogs_1_0',
      TableNameTo = NULL,
      ReplicationMethod = 2,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D331DA96-DFA5-46E2-945C-253CCF83ABC8'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '349B4BE1-83AD-4250-898E-259C82AAE910')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('349B4BE1-83AD-4250-898E-259C82AAE910', 'A-POSISTENDERRESTRICTIONS', '95368CC9-7A60-4270-B995-545F9326B161', NULL, 'POSISTENDERRESTRICTIONS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISTENDERRESTRICTIONS',
      TableFrom = '95368CC9-7A60-4270-B995-545F9326B161',
      StoredProcName = NULL,
      TableNameTo = 'POSISTENDERRESTRICTIONS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '349B4BE1-83AD-4250-898E-259C82AAE910'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B15AA2AE-BB82-4647-9C0E-26CE1D24F072')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B15AA2AE-BB82-4647-9C0E-26CE1D24F072', 'A-POSVISUALPROFILE', 'DB644ED5-2024-4A71-BED9-A70324BFD320', NULL, 'POSVISUALPROFILE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSVISUALPROFILE',
      TableFrom = 'DB644ED5-2024-4A71-BED9-A70324BFD320',
      StoredProcName = NULL,
      TableNameTo = 'POSVISUALPROFILE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B15AA2AE-BB82-4647-9C0E-26CE1D24F072'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'EB21E68A-4376-4998-90BB-274118AFE33B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('EB21E68A-4376-4998-90BB-274118AFE33B', 'N-JscDatabaseDesigns', 'B541A10D-0B9F-4F5F-BCD7-2D4B6E774F59', NULL, 'JscDatabaseDesigns', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscDatabaseDesigns',
      TableFrom = 'B541A10D-0B9F-4F5F-BCD7-2D4B6E774F59',
      StoredProcName = NULL,
      TableNameTo = 'JscDatabaseDesigns',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'EB21E68A-4376-4998-90BB-274118AFE33B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '7DF06ED6-4A62-4420-8D63-28046E08CA9F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('7DF06ED6-4A62-4420-8D63-28046E08CA9F', 'N-RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE', 'CFE7F467-1977-43AB-907A-9E427FCA5318', NULL, 'RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE',
      TableFrom = 'CFE7F467-1977-43AB-907A-9E427FCA5318',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '7DF06ED6-4A62-4420-8D63-28046E08CA9F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '321EB189-26B6-4A52-A469-28367EB6EE05')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('321EB189-26B6-4A52-A469-28367EB6EE05', 'A-INVENTITEMGROUP', '9CDF18E3-B83F-45FB-9B1D-046BA979FBF4', NULL, 'INVENTITEMGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-INVENTITEMGROUP',
      TableFrom = '9CDF18E3-B83F-45FB-9B1D-046BA979FBF4',
      StoredProcName = NULL,
      TableNameTo = 'INVENTITEMGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '321EB189-26B6-4A52-A469-28367EB6EE05'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E1A00BE5-B97A-49FC-89F7-284E1CE15CEB')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E1A00BE5-B97A-49FC-89F7-284E1CE15CEB', 'N-INVENTDIM', '4627C23C-5BFE-41B3-9F31-D91C24BF1FB9', NULL, 'INVENTDIM', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-INVENTDIM',
      TableFrom = '4627C23C-5BFE-41B3-9F31-D91C24BF1FB9',
      StoredProcName = NULL,
      TableNameTo = 'INVENTDIM',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E1A00BE5-B97A-49FC-89F7-284E1CE15CEB'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'EB3BCB83-2000-4B03-A3B4-28686AE19E6B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('EB3BCB83-2000-4B03-A3B4-28686AE19E6B', 'N-REPORTCONTEXTS', '8AB2CDB4-6674-4BA9-A221-D606BD56CD65', NULL, 'REPORTCONTEXTS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-REPORTCONTEXTS',
      TableFrom = '8AB2CDB4-6674-4BA9-A221-D606BD56CD65',
      StoredProcName = NULL,
      TableNameTo = 'REPORTCONTEXTS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'EB3BCB83-2000-4B03-A3B4-28686AE19E6B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '935F0539-A8C7-485D-9F2D-2887CEBDCFE3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('935F0539-A8C7-485D-9F2D-2887CEBDCFE3', 'P-spSECURITY_SignAction', NULL, 'spSECURITY_SignAction_1_0', NULL, 2, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'P-spSECURITY_SignAction',
      TableFrom = 'NULL',
      StoredProcName = 'spSECURITY_SignAction_1_0',
      TableNameTo = NULL,
      ReplicationMethod = 2,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '935F0539-A8C7-485D-9F2D-2887CEBDCFE3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '0C4ABF76-C05C-4352-8D3A-297C67D1AD0A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('0C4ABF76-C05C-4352-8D3A-297C67D1AD0A', 'A-RBOINVENTITEMIMAGE', '5FBD809B-4D27-4EC9-B103-8EDA05D2D8B4', NULL, 'RBOINVENTITEMIMAGE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOINVENTITEMIMAGE',
      TableFrom = '5FBD809B-4D27-4EC9-B103-8EDA05D2D8B4',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTITEMIMAGE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '0C4ABF76-C05C-4352-8D3A-297C67D1AD0A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '80C53B7F-EB95-40D5-81B7-29DF20B216A0')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('80C53B7F-EB95-40D5-81B7-29DF20B216A0', 'N-RBOLOYALTYMSRCARDTABLE', '522F0A0E-E78F-420A-8899-CDB9320C29FC', NULL, 'RBOLOYALTYMSRCARDTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOLOYALTYMSRCARDTABLE',
      TableFrom = '522F0A0E-E78F-420A-8899-CDB9320C29FC',
      StoredProcName = NULL,
      TableNameTo = 'RBOLOYALTYMSRCARDTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '80C53B7F-EB95-40D5-81B7-29DF20B216A0'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '0E4C9BEB-A462-4646-B03E-2A113E66C840')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('0E4C9BEB-A462-4646-B03E-2A113E66C840', 'A-RBODISCOUNTOFFERTABLE', 'CCFFD680-11E3-4880-BE8C-9502E8DF689B', NULL, 'RBODISCOUNTOFFERTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBODISCOUNTOFFERTABLE',
      TableFrom = 'CCFFD680-11E3-4880-BE8C-9502E8DF689B',
      StoredProcName = NULL,
      TableNameTo = 'RBODISCOUNTOFFERTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '0E4C9BEB-A462-4646-B03E-2A113E66C840'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E1833C34-4590-4F0F-893C-2A7F4FD4BDE9')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E1833C34-4590-4F0F-893C-2A7F4FD4BDE9', 'N-POSFUNCTIONALITYPROFILE', 'CFD3BA98-CBB9-4347-866A-9CCA64E78B9F', NULL, 'POSFUNCTIONALITYPROFILE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSFUNCTIONALITYPROFILE',
      TableFrom = 'CFD3BA98-CBB9-4347-866A-9CCA64E78B9F',
      StoredProcName = NULL,
      TableNameTo = 'POSFUNCTIONALITYPROFILE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E1833C34-4590-4F0F-893C-2A7F4FD4BDE9'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '529AEAA5-8A81-46C1-8BE2-2A9196F08F08')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('529AEAA5-8A81-46C1-8BE2-2A9196F08F08', 'A-RBOLOYALTYSCHEMESTABLE', '959FC286-EA28-4FB5-AC27-986529848992', NULL, 'RBOLOYALTYSCHEMESTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOLOYALTYSCHEMESTABLE',
      TableFrom = '959FC286-EA28-4FB5-AC27-986529848992',
      StoredProcName = NULL,
      TableNameTo = 'RBOLOYALTYSCHEMESTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '529AEAA5-8A81-46C1-8BE2-2A9196F08F08'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '0E5A2DFF-CC1A-4A1B-8635-2B01EECF0B0E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('0E5A2DFF-CC1A-4A1B-8635-2B01EECF0B0E', 'N-RBOINVENTTABLE', 'C738E16F-B6BF-4FBE-AA48-B35F249CC745', NULL, 'RBOINVENTTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOINVENTTABLE',
      TableFrom = 'C738E16F-B6BF-4FBE-AA48-B35F249CC745',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '0E5A2DFF-CC1A-4A1B-8635-2B01EECF0B0E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'FBD9A299-6DA5-4788-869F-2B1C1952656F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('FBD9A299-6DA5-4788-869F-2B1C1952656F', 'A-PRICEDISCTABLE', 'AA30FBEC-9AD3-45F3-B1E7-FB91059C0FBB', NULL, 'PRICEDISCTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-PRICEDISCTABLE',
      TableFrom = 'AA30FBEC-9AD3-45F3-B1E7-FB91059C0FBB',
      StoredProcName = NULL,
      TableNameTo = 'PRICEDISCTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'FBD9A299-6DA5-4788-869F-2B1C1952656F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '9371DF11-7F4E-476D-B8EB-2BA568C7C20F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('9371DF11-7F4E-476D-B8EB-2BA568C7C20F', 'A-DISCOUNTPARAMETERS', 'CA136130-C9A1-4BD6-8F77-C34DF9DAF720', NULL, 'DISCOUNTPARAMETERS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-DISCOUNTPARAMETERS',
      TableFrom = 'CA136130-C9A1-4BD6-8F77-C34DF9DAF720',
      StoredProcName = NULL,
      TableNameTo = 'DISCOUNTPARAMETERS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '9371DF11-7F4E-476D-B8EB-2BA568C7C20F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'CFA1ECA7-DB5B-4891-8F8A-2CD3A33E8F3E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('CFA1ECA7-DB5B-4891-8F8A-2CD3A33E8F3E', 'A-TAXGROUPDATA', '7D117979-6EB1-4163-A931-BB1B1A0B2FCB', NULL, 'TAXGROUPDATA', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TAXGROUPDATA',
      TableFrom = '7D117979-6EB1-4163-A931-BB1B1A0B2FCB',
      StoredProcName = NULL,
      TableNameTo = 'TAXGROUPDATA',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'CFA1ECA7-DB5B-4891-8F8A-2CD3A33E8F3E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4972A3C1-B1F8-4598-A05C-2D33B4FDBD90')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4972A3C1-B1F8-4598-A05C-2D33B4FDBD90', 'N-POSISTILLLAYOUT', 'E477F2C4-0ABF-4708-A961-3839F9D19775', NULL, 'POSISTILLLAYOUT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISTILLLAYOUT',
      TableFrom = 'E477F2C4-0ABF-4708-A961-3839F9D19775',
      StoredProcName = NULL,
      TableNameTo = 'POSISTILLLAYOUT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4972A3C1-B1F8-4598-A05C-2D33B4FDBD90'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AF7A2421-FCCB-40E7-BB57-2DD4460135A8')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AF7A2421-FCCB-40E7-BB57-2DD4460135A8', 'N-POSMMLINEGROUPS', '7E8BC8F3-AAE2-4B43-8C08-0C3109BB2A7C', NULL, 'POSMMLINEGROUPS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSMMLINEGROUPS',
      TableFrom = '7E8BC8F3-AAE2-4B43-8C08-0C3109BB2A7C',
      StoredProcName = NULL,
      TableNameTo = 'POSMMLINEGROUPS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AF7A2421-FCCB-40E7-BB57-2DD4460135A8'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'DA6691C9-A192-4EEA-81E5-2EC254424CB7')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('DA6691C9-A192-4EEA-81E5-2EC254424CB7', 'N-JscLinkedFilters', '66D5382F-F888-494E-A5FB-5B1A468BCB8D', NULL, 'JscLinkedFilters', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscLinkedFilters',
      TableFrom = '66D5382F-F888-494E-A5FB-5B1A468BCB8D',
      StoredProcName = NULL,
      TableNameTo = 'JscLinkedFilters',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'DA6691C9-A192-4EEA-81E5-2EC254424CB7'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3CBAB75F-9477-4127-91AB-2F85D2C10401')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3CBAB75F-9477-4127-91AB-2F85D2C10401', 'A-UNITCONVERT', 'D50077DB-AD6B-4BD4-ACD1-72060FFC2FE4', NULL, 'UNITCONVERT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-UNITCONVERT',
      TableFrom = 'D50077DB-AD6B-4BD4-ACD1-72060FFC2FE4',
      StoredProcName = NULL,
      TableNameTo = 'UNITCONVERT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3CBAB75F-9477-4127-91AB-2F85D2C10401'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'BFE5835B-26A6-4988-9DDB-2FC2B6D789C3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('BFE5835B-26A6-4988-9DDB-2FC2B6D789C3', 'A-RBOTENDERTYPECARDNUMBERS', 'B493024F-5EC3-48FE-9340-BD4E3F288359', NULL, 'RBOTENDERTYPECARDNUMBERS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOTENDERTYPECARDNUMBERS',
      TableFrom = 'B493024F-5EC3-48FE-9340-BD4E3F288359',
      StoredProcName = NULL,
      TableNameTo = 'RBOTENDERTYPECARDNUMBERS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'BFE5835B-26A6-4988-9DDB-2FC2B6D789C3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '418117FC-716C-4237-A7C4-2FDE471E9C40')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('418117FC-716C-4237-A7C4-2FDE471E9C40', 'N-BARCODESETUP', 'AE77A066-DA2E-47C2-B7B0-CA153574D47B', NULL, 'BARCODESETUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-BARCODESETUP',
      TableFrom = 'AE77A066-DA2E-47C2-B7B0-CA153574D47B',
      StoredProcName = NULL,
      TableNameTo = 'BARCODESETUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '418117FC-716C-4237-A7C4-2FDE471E9C40'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '36879E97-C22E-4B20-A9B9-2FE365822B8C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('36879E97-C22E-4B20-A9B9-2FE365822B8C', 'N-TAXREFUND', '56C98F6C-997F-4E13-A769-6A84C78530D7', NULL, 'TAXREFUND', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TAXREFUND',
      TableFrom = '56C98F6C-997F-4E13-A769-6A84C78530D7',
      StoredProcName = NULL,
      TableNameTo = 'TAXREFUND',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '36879E97-C22E-4B20-A9B9-2FE365822B8C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '8CB3C72F-30CC-43D0-A805-302831CF4E82')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('8CB3C72F-30CC-43D0-A805-302831CF4E82', 'N-PERIODICDISCOUNTLINE', '8F51974D-8E6A-400A-BFBC-30750B14B4DD', NULL, 'PERIODICDISCOUNTLINE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-PERIODICDISCOUNTLINE',
      TableFrom = '8F51974D-8E6A-400A-BFBC-30750B14B4DD',
      StoredProcName = NULL,
      TableNameTo = 'PERIODICDISCOUNTLINE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '8CB3C72F-30CC-43D0-A805-302831CF4E82'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'FB4CC6F1-AD9F-4410-B0AD-327D014672ED')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('FB4CC6F1-AD9F-4410-B0AD-327D014672ED', 'N-RBOBARCODEMASKCHARACTER', 'AFA0495C-BF6D-4C9E-81D5-4AA709DCDEE6', NULL, 'RBOBARCODEMASKCHARACTER', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOBARCODEMASKCHARACTER',
      TableFrom = 'AFA0495C-BF6D-4C9E-81D5-4AA709DCDEE6',
      StoredProcName = NULL,
      TableNameTo = 'RBOBARCODEMASKCHARACTER',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'FB4CC6F1-AD9F-4410-B0AD-327D014672ED'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'CA9B95D4-5B3A-47C4-AB0D-3397F5C8F19F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('CA9B95D4-5B3A-47C4-AB0D-3397F5C8F19F', 'A-EXCHRATES', '6F6B35B8-C287-441A-9697-CC67EC20E7E0', NULL, 'EXCHRATES', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-EXCHRATES',
      TableFrom = '6F6B35B8-C287-441A-9697-CC67EC20E7E0',
      StoredProcName = NULL,
      TableNameTo = 'EXCHRATES',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'CA9B95D4-5B3A-47C4-AB0D-3397F5C8F19F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4F750548-9920-460F-8E40-34CC650F6EF2')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4F750548-9920-460F-8E40-34CC650F6EF2', 'N-DININGTABLELAYOUT', '27EACD3A-879D-42CD-8EFB-FC7152F2079D', NULL, 'DININGTABLELAYOUT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-DININGTABLELAYOUT',
      TableFrom = '27EACD3A-879D-42CD-8EFB-FC7152F2079D',
      StoredProcName = NULL,
      TableNameTo = 'DININGTABLELAYOUT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4F750548-9920-460F-8E40-34CC650F6EF2'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '9D3F9E37-BC63-405E-B368-36A6227DBA86')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('9D3F9E37-BC63-405E-B368-36A6227DBA86', 'N-TOURIST', '90FB5F07-04F3-47A3-B79D-B1E2242532DD', NULL, 'TOURIST', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TOURIST',
      TableFrom = '90FB5F07-04F3-47A3-B79D-B1E2242532DD',
      StoredProcName = NULL,
      TableNameTo = 'TOURIST',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '9D3F9E37-BC63-405E-B368-36A6227DBA86'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '678D0A60-FF12-4AA8-A138-37174543630D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('678D0A60-FF12-4AA8-A138-37174543630D', 'N-RBOTRANSACTIONINCOMEEXPEN20158', 'F328E317-FD8E-4B95-A85E-DE835504C9F7', NULL, 'RBOTRANSACTIONINCOMEEXPEN20158', 0, 1, 1, 0, NULL, NULL, 0, 0, '4A77181E-519F-469D-87E3-9A2AC651C64D', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONINCOMEEXPEN20158',
      TableFrom = 'F328E317-FD8E-4B95-A85E-DE835504C9F7',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONINCOMEEXPEN20158',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '4A77181E-519F-469D-87E3-9A2AC651C64D',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '678D0A60-FF12-4AA8-A138-37174543630D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '487C9014-E6E1-45A4-B12C-374DA81FA21B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('487C9014-E6E1-45A4-B12C-374DA81FA21B', 'N-RBOTRANSACTIONRECEIPTS', 'AFE1AF88-33DA-4D46-95C8-D9D6387E6A12', NULL, 'RBOTRANSACTIONRECEIPTS', 0, 1, 1, 0, NULL, NULL, 0, 0, '18D2D6D5-AED3-4EA8-9535-61ABAE6DF9AE', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONRECEIPTS',
      TableFrom = 'AFE1AF88-33DA-4D46-95C8-D9D6387E6A12',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONRECEIPTS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '18D2D6D5-AED3-4EA8-9535-61ABAE6DF9AE',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '487C9014-E6E1-45A4-B12C-374DA81FA21B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E27FC118-2D95-427E-9706-375FD037E773')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E27FC118-2D95-427E-9706-375FD037E773', 'A-RBOSTAFFTABLE', '32B1AF89-D8E1-489A-AEBE-309DBDF558EA', NULL, 'RBOSTAFFTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTAFFTABLE',
      TableFrom = '32B1AF89-D8E1-489A-AEBE-309DBDF558EA',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTAFFTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E27FC118-2D95-427E-9706-375FD037E773'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '862D679A-9782-45EE-82BB-37E6BB8B5F6E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('862D679A-9782-45EE-82BB-37E6BB8B5F6E', 'A-RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE', 'CFE7F467-1977-43AB-907A-9E427FCA5318', NULL, 'RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE',
      TableFrom = 'CFE7F467-1977-43AB-907A-9E427FCA5318',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '862D679A-9782-45EE-82BB-37E6BB8B5F6E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '46F3440F-D3D2-4485-B399-38A54DA63BC0')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('46F3440F-D3D2-4485-B399-38A54DA63BC0', 'N-REPORTS', '8826F144-4B13-40D4-8481-BAF6C6894C7E', NULL, 'REPORTS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-REPORTS',
      TableFrom = '8826F144-4B13-40D4-8481-BAF6C6894C7E',
      StoredProcName = NULL,
      TableNameTo = 'REPORTS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '46F3440F-D3D2-4485-B399-38A54DA63BC0'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '683CFB23-7908-492B-8992-39210A04EB44')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('683CFB23-7908-492B-8992-39210A04EB44', 'A-POSPERIODICDISCOUNTLINE', 'C1AFC1D1-0BB9-4FA9-AEDD-DAE91D565FBE', NULL, 'POSPERIODICDISCOUNTLINE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSPERIODICDISCOUNTLINE',
      TableFrom = 'C1AFC1D1-0BB9-4FA9-AEDD-DAE91D565FBE',
      StoredProcName = NULL,
      TableNameTo = 'POSPERIODICDISCOUNTLINE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '683CFB23-7908-492B-8992-39210A04EB44'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '19C8BF42-81A9-46F9-9A92-3B167292468C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('19C8BF42-81A9-46F9-9A92-3B167292468C', 'A-RETAILITEMDIMENSION', '10400D7D-7F9D-4FFD-B9ED-E10D2C873BE7', NULL, 'RETAILITEMDIMENSION', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RETAILITEMDIMENSION',
      TableFrom = '10400D7D-7F9D-4FFD-B9ED-E10D2C873BE7',
      StoredProcName = NULL,
      TableNameTo = 'RETAILITEMDIMENSION',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '19C8BF42-81A9-46F9-9A92-3B167292468C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '192524DE-C85F-4C75-A792-3B20C7822CA8')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('192524DE-C85F-4C75-A792-3B20C7822CA8', 'N-RBOTENDERTYPECARDNUMBERS', 'B493024F-5EC3-48FE-9340-BD4E3F288359', NULL, 'RBOTENDERTYPECARDNUMBERS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTENDERTYPECARDNUMBERS',
      TableFrom = 'B493024F-5EC3-48FE-9340-BD4E3F288359',
      StoredProcName = NULL,
      TableNameTo = 'RBOTENDERTYPECARDNUMBERS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '192524DE-C85F-4C75-A792-3B20C7822CA8'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AEF3618B-16DF-4233-AC55-3BB96C818176')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AEF3618B-16DF-4233-AC55-3BB96C818176', 'A-SALESTYPE', '42EDA5FC-7350-4DE2-9D08-E4D670F668C8', NULL, 'SALESTYPE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-SALESTYPE',
      TableFrom = '42EDA5FC-7350-4DE2-9D08-E4D670F668C8',
      StoredProcName = NULL,
      TableNameTo = 'SALESTYPE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AEF3618B-16DF-4233-AC55-3BB96C818176'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3607E2CF-AAC4-4275-857D-3C1475E1C41A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3607E2CF-AAC4-4275-857D-3C1475E1C41A', 'N-RBOSTYLES', '916D8659-13A4-4E70-9B74-2A1048E1B625', NULL, 'RBOSTYLES', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSTYLES',
      TableFrom = '916D8659-13A4-4E70-9B74-2A1048E1B625',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTYLES',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3607E2CF-AAC4-4275-857D-3C1475E1C41A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '199DAE00-245C-4EBF-BA1F-3C1A2B628AC3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('199DAE00-245C-4EBF-BA1F-3C1A2B628AC3', 'N-RBOPARAMETERS', 'AD45C2CE-6E1E-42F8-9E01-BB7EE2DB600E', NULL, 'RBOPARAMETERS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOPARAMETERS',
      TableFrom = 'AD45C2CE-6E1E-42F8-9E01-BB7EE2DB600E',
      StoredProcName = NULL,
      TableNameTo = 'RBOPARAMETERS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '199DAE00-245C-4EBF-BA1F-3C1A2B628AC3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'CF9A443F-56ED-42C7-8720-3D5CE0EA58A8')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('CF9A443F-56ED-42C7-8720-3D5CE0EA58A8', 'N-POSMENULINE', '9A6E8836-65CB-4773-B220-24BA0EA0D32E', NULL, 'POSMENULINE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSMENULINE',
      TableFrom = '9A6E8836-65CB-4773-B220-24BA0EA0D32E',
      StoredProcName = NULL,
      TableNameTo = 'POSMENULINE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'CF9A443F-56ED-42C7-8720-3D5CE0EA58A8'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F3434EB6-3090-4AA4-90F6-3D80A6D031B1')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F3434EB6-3090-4AA4-90F6-3D80A6D031B1', 'A-POSMENULINE', '9A6E8836-65CB-4773-B220-24BA0EA0D32E', NULL, 'POSMENULINE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSMENULINE',
      TableFrom = '9A6E8836-65CB-4773-B220-24BA0EA0D32E',
      StoredProcName = NULL,
      TableNameTo = 'POSMENULINE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F3434EB6-3090-4AA4-90F6-3D80A6D031B1'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1CFF9F58-F31F-4FDE-8894-3E34A1794927')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1CFF9F58-F31F-4FDE-8894-3E34A1794927', 'N-RBOMSRCARDTABLE', 'B280F68B-B9C8-46C6-AF05-9F913A23BE69', NULL, 'RBOMSRCARDTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOMSRCARDTABLE',
      TableFrom = 'B280F68B-B9C8-46C6-AF05-9F913A23BE69',
      StoredProcName = NULL,
      TableNameTo = 'RBOMSRCARDTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1CFF9F58-F31F-4FDE-8894-3E34A1794927'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'BCFFFDEC-AD90-49E4-89CE-3E58C459A2FB')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('BCFFFDEC-AD90-49E4-89CE-3E58C459A2FB', 'N-RBOINVENTITEMIMAGE', '5FBD809B-4D27-4EC9-B103-8EDA05D2D8B4', NULL, 'RBOINVENTITEMIMAGE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOINVENTITEMIMAGE',
      TableFrom = '5FBD809B-4D27-4EC9-B103-8EDA05D2D8B4',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTITEMIMAGE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'BCFFFDEC-AD90-49E4-89CE-3E58C459A2FB'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '49ADE0D3-46B7-4A78-B2FA-3F23BA1D1E3D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('49ADE0D3-46B7-4A78-B2FA-3F23BA1D1E3D', 'A-RBOTERMINALGROUPCONNECTION', 'F1158E4F-7F8C-499D-92AF-11484F32C8FD', NULL, 'RBOTERMINALGROUPCONNECTION', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOTERMINALGROUPCONNECTION',
      TableFrom = 'F1158E4F-7F8C-499D-92AF-11484F32C8FD',
      StoredProcName = NULL,
      TableNameTo = 'RBOTERMINALGROUPCONNECTION',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '49ADE0D3-46B7-4A78-B2FA-3F23BA1D1E3D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E5421182-6E02-45F0-96B0-3F248EAD1E76')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E5421182-6E02-45F0-96B0-3F248EAD1E76', 'N-RBOCOLORGROUPTABLE', '6301F83F-DE63-4E10-83E7-A32F38D89700', NULL, 'RBOCOLORGROUPTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOCOLORGROUPTABLE',
      TableFrom = '6301F83F-DE63-4E10-83E7-A32F38D89700',
      StoredProcName = NULL,
      TableNameTo = 'RBOCOLORGROUPTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E5421182-6E02-45F0-96B0-3F248EAD1E76'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '91BA5045-B886-4420-9B06-3F294D026106')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('91BA5045-B886-4420-9B06-3F294D026106', 'A-RBOTERMINALGROUP', '374430E6-BADB-4513-913D-56A6AD8114C6', NULL, 'RBOTERMINALGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOTERMINALGROUP',
      TableFrom = '374430E6-BADB-4513-913D-56A6AD8114C6',
      StoredProcName = NULL,
      TableNameTo = 'RBOTERMINALGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '91BA5045-B886-4420-9B06-3F294D026106'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '37852764-F454-4C75-A3BF-4002BE4666B5')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('37852764-F454-4C75-A3BF-4002BE4666B5', 'N-RBOINVENTLINKEDITEM', '55290174-49C8-4E0E-AAC3-2DBEE4D89964', NULL, 'RBOINVENTLINKEDITEM', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOINVENTLINKEDITEM',
      TableFrom = '55290174-49C8-4E0E-AAC3-2DBEE4D89964',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTLINKEDITEM',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '37852764-F454-4C75-A3BF-4002BE4666B5'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '8337972F-98D1-4F53-9729-409FE53FC2BE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('8337972F-98D1-4F53-9729-409FE53FC2BE', 'N-REPORTLOCALIZATION', '3AB1417A-4965-43CF-AF54-97872724D04F', NULL, 'REPORTLOCALIZATION', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-REPORTLOCALIZATION',
      TableFrom = '3AB1417A-4965-43CF-AF54-97872724D04F',
      StoredProcName = NULL,
      TableNameTo = 'REPORTLOCALIZATION',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '8337972F-98D1-4F53-9729-409FE53FC2BE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F955FF49-34D6-4692-9BE5-40B582A65CA2')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F955FF49-34D6-4692-9BE5-40B582A65CA2', 'A-POSMENUHEADER', 'A55747FD-DDB4-4E43-A0E1-88FFC96EEF52', NULL, 'POSMENUHEADER', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSMENUHEADER',
      TableFrom = 'A55747FD-DDB4-4E43-A0E1-88FFC96EEF52',
      StoredProcName = NULL,
      TableNameTo = 'POSMENUHEADER',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F955FF49-34D6-4692-9BE5-40B582A65CA2'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6DADEC9D-6F13-488C-B19E-411CBA6AFFFE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6DADEC9D-6F13-488C-B19E-411CBA6AFFFE', 'N-RESTAURANTSTATION', 'F3BE753E-D7CD-40C4-A1E5-34C1FDFA8D54', NULL, 'RESTAURANTSTATION', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RESTAURANTSTATION',
      TableFrom = 'F3BE753E-D7CD-40C4-A1E5-34C1FDFA8D54',
      StoredProcName = NULL,
      TableNameTo = 'RESTAURANTSTATION',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6DADEC9D-6F13-488C-B19E-411CBA6AFFFE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '42D6795B-8E8B-4B33-929B-41C39A8A1844')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('42D6795B-8E8B-4B33-929B-41C39A8A1844', 'A-POSPERIODICDISCOUNT', 'C96D3A6F-B7A8-48FC-B801-D5FEB1F0402B', NULL, 'POSPERIODICDISCOUNT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSPERIODICDISCOUNT',
      TableFrom = 'C96D3A6F-B7A8-48FC-B801-D5FEB1F0402B',
      StoredProcName = NULL,
      TableNameTo = 'POSPERIODICDISCOUNT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '42D6795B-8E8B-4B33-929B-41C39A8A1844'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B4F9097D-2277-4BD1-8986-434FAF427149')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B4F9097D-2277-4BD1-8986-434FAF427149', 'A-RBOMSRCARDTABLE', 'B280F68B-B9C8-46C6-AF05-9F913A23BE69', NULL, 'RBOMSRCARDTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOMSRCARDTABLE',
      TableFrom = 'B280F68B-B9C8-46C6-AF05-9F913A23BE69',
      StoredProcName = NULL,
      TableNameTo = 'RBOMSRCARDTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B4F9097D-2277-4BD1-8986-434FAF427149'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '8C4D7EE6-C850-4D16-AD9C-43560499AC2E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('8C4D7EE6-C850-4D16-AD9C-43560499AC2E', 'N-POSDISCVALIDATIONPERIOD', '0C4F6C1A-49CD-45E1-801A-A8817569A318', NULL, 'POSDISCVALIDATIONPERIOD', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSDISCVALIDATIONPERIOD',
      TableFrom = '0C4F6C1A-49CD-45E1-801A-A8817569A318',
      StoredProcName = NULL,
      TableNameTo = 'POSDISCVALIDATIONPERIOD',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '8C4D7EE6-C850-4D16-AD9C-43560499AC2E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4A94D18F-C05B-4BCC-BABA-4580A02EB0EB')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4A94D18F-C05B-4BCC-BABA-4580A02EB0EB', 'N-RBOLOYALTYSCHEMESTABLE', '959FC286-EA28-4FB5-AC27-986529848992', NULL, 'RBOLOYALTYSCHEMESTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOLOYALTYSCHEMESTABLE',
      TableFrom = '959FC286-EA28-4FB5-AC27-986529848992',
      StoredProcName = NULL,
      TableNameTo = 'RBOLOYALTYSCHEMESTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4A94D18F-C05B-4BCC-BABA-4580A02EB0EB'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '423A73EA-56F0-4F35-A41C-45C5EE27632E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('423A73EA-56F0-4F35-A41C-45C5EE27632E', 'A-PRICEDISCGROUP', '3AFC31F2-E0FB-4ABE-9068-F8B9584DAAC2', NULL, 'PRICEDISCGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-PRICEDISCGROUP',
      TableFrom = '3AFC31F2-E0FB-4ABE-9068-F8B9584DAAC2',
      StoredProcName = NULL,
      TableNameTo = 'PRICEDISCGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '423A73EA-56F0-4F35-A41C-45C5EE27632E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5D149818-F966-403C-AB49-462675B60E0A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5D149818-F966-403C-AB49-462675B60E0A', 'N-RBODISCOUNTOFFERLINE', 'FA14B78B-CF5D-456C-93C5-3227E380C545', NULL, 'RBODISCOUNTOFFERLINE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBODISCOUNTOFFERLINE',
      TableFrom = 'FA14B78B-CF5D-456C-93C5-3227E380C545',
      StoredProcName = NULL,
      TableNameTo = 'RBODISCOUNTOFFERLINE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5D149818-F966-403C-AB49-462675B60E0A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D3E5FD5F-4492-479D-8535-4679DF9284F0')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D3E5FD5F-4492-479D-8535-4679DF9284F0', 'N-PRICEDISCGROUP', '3AFC31F2-E0FB-4ABE-9068-F8B9584DAAC2', NULL, 'PRICEDISCGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-PRICEDISCGROUP',
      TableFrom = '3AFC31F2-E0FB-4ABE-9068-F8B9584DAAC2',
      StoredProcName = NULL,
      TableNameTo = 'PRICEDISCGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D3E5FD5F-4492-479D-8535-4679DF9284F0'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D477693F-11FD-44A8-B312-4694098725DB')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D477693F-11FD-44A8-B312-4694098725DB', 'A-RBOLOYALTYPOINTSTABLE', '3D36B809-5A68-4041-960D-F72F9D4B1355', NULL, 'RBOLOYALTYPOINTSTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOLOYALTYPOINTSTABLE',
      TableFrom = '3D36B809-5A68-4041-960D-F72F9D4B1355',
      StoredProcName = NULL,
      TableNameTo = 'RBOLOYALTYPOINTSTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D477693F-11FD-44A8-B312-4694098725DB'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2A820ECA-0296-4853-ABA1-469DA1E919C9')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2A820ECA-0296-4853-ABA1-469DA1E919C9', 'N-RBOINVENTITEMRETAILDIVISION', '5FC36955-2CCD-431E-B6A4-B7A69A2DE25F', NULL, 'RBOINVENTITEMRETAILDIVISION', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOINVENTITEMRETAILDIVISION',
      TableFrom = '5FC36955-2CCD-431E-B6A4-B7A69A2DE25F',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTITEMRETAILDIVISION',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2A820ECA-0296-4853-ABA1-469DA1E919C9'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AB8A8872-9779-41EF-965E-46F36B67D3A8')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AB8A8872-9779-41EF-965E-46F36B67D3A8', 'N-RESTAURANTMENUTYPE', 'B7F8D694-45CD-48B4-B542-14EF3D5249FC', NULL, 'RESTAURANTMENUTYPE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RESTAURANTMENUTYPE',
      TableFrom = 'B7F8D694-45CD-48B4-B542-14EF3D5249FC',
      StoredProcName = NULL,
      TableNameTo = 'RESTAURANTMENUTYPE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AB8A8872-9779-41EF-965E-46F36B67D3A8'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'FB3E18FC-670F-4166-89C9-47808113A9ED')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('FB3E18FC-670F-4166-89C9-47808113A9ED', 'A-CUSTOMERORDERSETTINGS', '88F30286-DAFF-4B6D-893D-EAC327D6CD22', NULL, 'CUSTOMERORDERSETTINGS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CUSTOMERORDERSETTINGS',
      TableFrom = '88F30286-DAFF-4B6D-893D-EAC327D6CD22',
      StoredProcName = NULL,
      TableNameTo = 'CUSTOMERORDERSETTINGS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'FB3E18FC-670F-4166-89C9-47808113A9ED'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '7E7EF9F6-57D3-4AE0-BB23-4805C21A101A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('7E7EF9F6-57D3-4AE0-BB23-4805C21A101A', 'A-RBOINCOMEEXPENSEACCOUNTTABLE', 'E9B12AA1-5E5C-4211-97EF-ABB1209AD54D', NULL, 'RBOINCOMEEXPENSEACCOUNTTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOINCOMEEXPENSEACCOUNTTABLE',
      TableFrom = 'E9B12AA1-5E5C-4211-97EF-ABB1209AD54D',
      StoredProcName = NULL,
      TableNameTo = 'RBOINCOMEEXPENSEACCOUNTTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '7E7EF9F6-57D3-4AE0-BB23-4805C21A101A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '292500DA-EFA7-4C49-9B4D-481567F30DBA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('292500DA-EFA7-4C49-9B4D-481567F30DBA', 'N-CUSTGROUPCATEGORY', 'D3598CA9-26F0-47C6-BADA-381C66457131', NULL, 'CUSTGROUPCATEGORY', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CUSTGROUPCATEGORY',
      TableFrom = 'D3598CA9-26F0-47C6-BADA-381C66457131',
      StoredProcName = NULL,
      TableNameTo = 'CUSTGROUPCATEGORY',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '292500DA-EFA7-4C49-9B4D-481567F30DBA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6A69D8AA-DCB8-42C6-9761-4899A07DFEA3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6A69D8AA-DCB8-42C6-9761-4899A07DFEA3', 'A-TAXONITEM', 'A327A470-15BC-4D42-9B88-4C9287CF6594', NULL, 'TAXONITEM', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TAXONITEM',
      TableFrom = 'A327A470-15BC-4D42-9B88-4C9287CF6594',
      StoredProcName = NULL,
      TableNameTo = 'TAXONITEM',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6A69D8AA-DCB8-42C6-9761-4899A07DFEA3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '53DC949C-34B7-4C11-94E6-492BF66998B2')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('53DC949C-34B7-4C11-94E6-492BF66998B2', 'N-RBOTRANSACTIONFUELTRANS', '6D5D0B78-A3C2-402C-962F-F1668704BC84', NULL, 'RBOTRANSACTIONFUELTRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, '8E8CF65C-145F-4B86-9C0B-8EA3FF744F3C', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONFUELTRANS',
      TableFrom = '6D5D0B78-A3C2-402C-962F-F1668704BC84',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONFUELTRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '8E8CF65C-145F-4B86-9C0B-8EA3FF744F3C',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '53DC949C-34B7-4C11-94E6-492BF66998B2'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1B69DD7A-878C-4D7D-8317-49E43FDC5984')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1B69DD7A-878C-4D7D-8317-49E43FDC5984', 'A-CONTACTTABLE', 'C0CEA21E-0F77-4964-8506-0E2CD0D8A977', NULL, 'CONTACTTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CONTACTTABLE',
      TableFrom = 'C0CEA21E-0F77-4964-8506-0E2CD0D8A977',
      StoredProcName = NULL,
      TableNameTo = 'CONTACTTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1B69DD7A-878C-4D7D-8317-49E43FDC5984'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '664C4173-F09E-4B45-957D-4D98D38C8663')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('664C4173-F09E-4B45-957D-4D98D38C8663', 'A-RBOSTYLES', '916D8659-13A4-4E70-9B74-2A1048E1B625', NULL, 'RBOSTYLES', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTYLES',
      TableFrom = '916D8659-13A4-4E70-9B74-2A1048E1B625',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTYLES',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '664C4173-F09E-4B45-957D-4D98D38C8663'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'DFA9B30E-C837-4591-85C5-4F700E0C4AA4')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('DFA9B30E-C837-4591-85C5-4F700E0C4AA4', 'A-POSISHOSPITALITYTRANSTABLE', 'C94BFD1E-0A21-4720-85AE-63F9427AF22B', NULL, 'POSISHOSPITALITYTRANSTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISHOSPITALITYTRANSTABLE',
      TableFrom = 'C94BFD1E-0A21-4720-85AE-63F9427AF22B',
      StoredProcName = NULL,
      TableNameTo = 'POSISHOSPITALITYTRANSTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'DFA9B30E-C837-4591-85C5-4F700E0C4AA4'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B1F2E9A8-2840-4C59-83A7-4F76CA45BF86')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B1F2E9A8-2840-4C59-83A7-4F76CA45BF86', 'N-RBOTRANSACTIONREPRINTTRANS', '89B26156-7358-447E-AD75-BF4692601AAE', NULL, 'RBOTRANSACTIONREPRINTTRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, '5622444C-49D7-4CA2-AE01-EC72D6BDF454', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONREPRINTTRANS',
      TableFrom = '89B26156-7358-447E-AD75-BF4692601AAE',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONREPRINTTRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '5622444C-49D7-4CA2-AE01-EC72D6BDF454',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B1F2E9A8-2840-4C59-83A7-4F76CA45BF86'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F2781BFB-2574-46AE-9F62-4FF67A4EB6B3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F2781BFB-2574-46AE-9F62-4FF67A4EB6B3', 'N-POSMENUHEADER', 'A55747FD-DDB4-4E43-A0E1-88FFC96EEF52', NULL, 'POSMENUHEADER', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSMENUHEADER',
      TableFrom = 'A55747FD-DDB4-4E43-A0E1-88FFC96EEF52',
      StoredProcName = NULL,
      TableNameTo = 'POSMENUHEADER',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F2781BFB-2574-46AE-9F62-4FF67A4EB6B3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '01B05814-6910-4EE6-94B4-5070602F15F0')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('01B05814-6910-4EE6-94B4-5070602F15F0', 'N-POSFORMPROFILELINES', 'CAD4BB5C-4E76-4D3E-88BC-D70C660883A6', NULL, 'POSFORMPROFILELINES', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSFORMPROFILELINES',
      TableFrom = 'CAD4BB5C-4E76-4D3E-88BC-D70C660883A6',
      StoredProcName = NULL,
      TableNameTo = 'POSFORMPROFILELINES',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '01B05814-6910-4EE6-94B4-5070602F15F0'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3DDBAFCF-0B96-4077-8F14-52FDA91EE286')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3DDBAFCF-0B96-4077-8F14-52FDA91EE286', 'A-RBOCUSTTABLE', '80A5DAFF-BB53-4C82-BB0A-E57EF3A13780', NULL, 'RBOCUSTTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOCUSTTABLE',
      TableFrom = '80A5DAFF-BB53-4C82-BB0A-E57EF3A13780',
      StoredProcName = NULL,
      TableNameTo = 'RBOCUSTTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3DDBAFCF-0B96-4077-8F14-52FDA91EE286'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AD5B945A-7CD4-48E9-AAD7-5353105C6A81')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AD5B945A-7CD4-48E9-AAD7-5353105C6A81', 'N-RBOTRANSACTIONFISCALTRANS', '592186F0-C7A3-4E32-A94B-78F329F4AFC2', NULL, 'RBOTRANSACTIONFISCALTRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, 'D09B114A-E927-4E2F-B128-341AE14808BB', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONFISCALTRANS',
      TableFrom = '592186F0-C7A3-4E32-A94B-78F329F4AFC2',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONFISCALTRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = 'D09B114A-E927-4E2F-B128-341AE14808BB',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AD5B945A-7CD4-48E9-AAD7-5353105C6A81'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '73C42849-EF1B-45BF-B74A-536D6CA64AC8')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('73C42849-EF1B-45BF-B74A-536D6CA64AC8', 'N-DININGTABLELAYOUTSCREEN', '817DC0B2-5D34-4A75-BADA-FA8D76C4937E', NULL, 'DININGTABLELAYOUTSCREEN', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-DININGTABLELAYOUTSCREEN',
      TableFrom = '817DC0B2-5D34-4A75-BADA-FA8D76C4937E',
      StoredProcName = NULL,
      TableNameTo = 'DININGTABLELAYOUTSCREEN',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '73C42849-EF1B-45BF-B74A-536D6CA64AC8'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3F071BB8-82C8-4505-BFE5-547CE9C10D4E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3F071BB8-82C8-4505-BFE5-547CE9C10D4E', 'A-SPECIALGROUP', 'D6C6AC7C-6A0D-400E-85DD-8420240A3717', NULL, 'SPECIALGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-SPECIALGROUP',
      TableFrom = 'D6C6AC7C-6A0D-400E-85DD-8420240A3717',
      StoredProcName = NULL,
      TableNameTo = 'SPECIALGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3F071BB8-82C8-4505-BFE5-547CE9C10D4E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F1DD3161-C78F-42A6-9D7F-547F512C73BC')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F1DD3161-C78F-42A6-9D7F-547F512C73BC', 'A-RESTAURANTMENUTYPE', 'B7F8D694-45CD-48B4-B542-14EF3D5249FC', NULL, 'RESTAURANTMENUTYPE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RESTAURANTMENUTYPE',
      TableFrom = 'B7F8D694-45CD-48B4-B542-14EF3D5249FC',
      StoredProcName = NULL,
      TableNameTo = 'RESTAURANTMENUTYPE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F1DD3161-C78F-42A6-9D7F-547F512C73BC'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6725198E-1B93-4A4B-B1D9-56423C5A8291')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6725198E-1B93-4A4B-B1D9-56423C5A8291', 'A-IMAGES', '9A5109F1-713F-485C-9F8D-FF60D9B4780C', NULL, 'IMAGES', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-IMAGES',
      TableFrom = '9A5109F1-713F-485C-9F8D-FF60D9B4780C',
      StoredProcName = NULL,
      TableNameTo = 'IMAGES',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6725198E-1B93-4A4B-B1D9-56423C5A8291'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '64708016-7161-4193-A61D-56CB05CEB4D3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('64708016-7161-4193-A61D-56CB05CEB4D3', 'A-RBOBARCODEMASKTABLE', '2616CC4D-BEDB-4187-BDEA-1E19314AB38A', NULL, 'RBOBARCODEMASKTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOBARCODEMASKTABLE',
      TableFrom = '2616CC4D-BEDB-4187-BDEA-1E19314AB38A',
      StoredProcName = NULL,
      TableNameTo = 'RBOBARCODEMASKTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '64708016-7161-4193-A61D-56CB05CEB4D3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D1AA3C64-DE8A-416A-A4E3-586FD413ACA2')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D1AA3C64-DE8A-416A-A4E3-586FD413ACA2', 'N-RBOTRANSACTIONEFTINFOTRANS', '6D7D5825-4AC5-4345-8589-74AFA5897179', NULL, 'RBOTRANSACTIONEFTINFOTRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, 'FC8520EB-55D6-455F-8E0F-795B097F595B', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONEFTINFOTRANS',
      TableFrom = '6D7D5825-4AC5-4345-8589-74AFA5897179',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONEFTINFOTRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = 'FC8520EB-55D6-455F-8E0F-795B097F595B',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D1AA3C64-DE8A-416A-A4E3-586FD413ACA2'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2A37D5CB-2567-4291-9B86-58D4F013D9B5')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2A37D5CB-2567-4291-9B86-58D4F013D9B5', 'N-JscTableMap', '9448B28C-831B-4A51-888A-20054B6FA1A7', NULL, 'JscTableMap', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscTableMap',
      TableFrom = '9448B28C-831B-4A51-888A-20054B6FA1A7',
      StoredProcName = NULL,
      TableNameTo = 'JscTableMap',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2A37D5CB-2567-4291-9B86-58D4F013D9B5'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B9C3B5CC-3693-4E2E-BB21-59821373DD21')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B9C3B5CC-3693-4E2E-BB21-59821373DD21', 'N-RBOTENDERTYPECARDTABLE', '374B165E-18FF-4D12-8109-32A3240B37AC', NULL, 'RBOTENDERTYPECARDTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTENDERTYPECARDTABLE',
      TableFrom = '374B165E-18FF-4D12-8109-32A3240B37AC',
      StoredProcName = NULL,
      TableNameTo = 'RBOTENDERTYPECARDTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B9C3B5CC-3693-4E2E-BB21-59821373DD21'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F427FB2B-2CA3-4192-A609-5992FAAA4712')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F427FB2B-2CA3-4192-A609-5992FAAA4712', 'N-SPECIALGROUPITEMS', 'AA5CD90D-7B42-4F1A-B921-6E8C4561276E', NULL, 'SPECIALGROUPITEMS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-SPECIALGROUPITEMS',
      TableFrom = 'AA5CD90D-7B42-4F1A-B921-6E8C4561276E',
      StoredProcName = NULL,
      TableNameTo = 'SPECIALGROUPITEMS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F427FB2B-2CA3-4192-A609-5992FAAA4712'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5FF5E5B7-22C7-415E-9EC5-5A12560F1945')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5FF5E5B7-22C7-415E-9EC5-5A12560F1945', 'N-RBOTRANSACTIONOPERATIONAUDIT', '67497997-9DC6-4519-B75B-86BE025C0BB9', NULL, 'RBOTRANSACTIONOPERATIONAUDIT', 0, 1, 1, 0, NULL, NULL, 0, 0, '491E9AB8-D360-497A-BBA3-CF62EE5764BC', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONOPERATIONAUDIT',
      TableFrom = '67497997-9DC6-4519-B75B-86BE025C0BB9',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONOPERATIONAUDIT',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '491E9AB8-D360-497A-BBA3-CF62EE5764BC',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5FF5E5B7-22C7-415E-9EC5-5A12560F1945'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '9EA526DC-2192-4E91-9BB0-5AB35C6838A0')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('9EA526DC-2192-4E91-9BB0-5AB35C6838A0', 'A-DININGTABLELAYOUTSCREEN', '817DC0B2-5D34-4A75-BADA-FA8D76C4937E', NULL, 'DININGTABLELAYOUTSCREEN', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-DININGTABLELAYOUTSCREEN',
      TableFrom = '817DC0B2-5D34-4A75-BADA-FA8D76C4937E',
      StoredProcName = NULL,
      TableNameTo = 'DININGTABLELAYOUTSCREEN',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '9EA526DC-2192-4E91-9BB0-5AB35C6838A0'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '69D42846-60E4-468A-A251-5AD871D1D682')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('69D42846-60E4-468A-A251-5AD871D1D682', 'A-RBOINVENTITEMRETAILGROUP', '9F02D050-30B4-480C-BFE0-E81AD1C724BA', NULL, 'RBOINVENTITEMRETAILGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOINVENTITEMRETAILGROUP',
      TableFrom = '9F02D050-30B4-480C-BFE0-E81AD1C724BA',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTITEMRETAILGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '69D42846-60E4-468A-A251-5AD871D1D682'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'FBE9E768-75F7-418C-A536-5AFC153D9C88')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('FBE9E768-75F7-418C-A536-5AFC153D9C88', 'N-POSISOILTAX', '8F9EE93C-597C-48E8-8E39-24CE8C432A31', NULL, 'POSISOILTAX', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISOILTAX',
      TableFrom = '8F9EE93C-597C-48E8-8E39-24CE8C432A31',
      StoredProcName = NULL,
      TableNameTo = 'POSISOILTAX',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'FBE9E768-75F7-418C-A536-5AFC153D9C88'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A10D3A48-0DA6-4297-9B81-5BD0D89315E3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A10D3A48-0DA6-4297-9B81-5BD0D89315E3', 'A-INVENTTABLE', '15F0B991-2F18-47D7-8B47-E6FE78E819E4', NULL, 'INVENTTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-INVENTTABLE',
      TableFrom = '15F0B991-2F18-47D7-8B47-E6FE78E819E4',
      StoredProcName = NULL,
      TableNameTo = 'INVENTTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A10D3A48-0DA6-4297-9B81-5BD0D89315E3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6E116ACE-26D9-436E-B872-5C5E6CB8658E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6E116ACE-26D9-436E-B872-5C5E6CB8658E', 'N-POSTRANSACTIONSERVICEPROFILE', '57787D24-DB50-46DD-9D8B-BFA714E2DC42', NULL, 'POSTRANSACTIONSERVICEPROFILE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSTRANSACTIONSERVICEPROFILE',
      TableFrom = '57787D24-DB50-46DD-9D8B-BFA714E2DC42',
      StoredProcName = NULL,
      TableNameTo = 'POSTRANSACTIONSERVICEPROFILE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6E116ACE-26D9-436E-B872-5C5E6CB8658E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '8BED45DB-C8AC-47B3-B1A5-5D61492E255B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('8BED45DB-C8AC-47B3-B1A5-5D61492E255B', 'N-INVENTTABLEMODULE', '1788D6AA-91C5-47EE-8DB5-8DE6B00C3C57', NULL, 'INVENTTABLEMODULE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-INVENTTABLEMODULE',
      TableFrom = '1788D6AA-91C5-47EE-8DB5-8DE6B00C3C57',
      StoredProcName = NULL,
      TableNameTo = 'INVENTTABLEMODULE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '8BED45DB-C8AC-47B3-B1A5-5D61492E255B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D30D7FDF-C8E2-46F9-9B90-5D67588B2950')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D30D7FDF-C8E2-46F9-9B90-5D67588B2950', 'A-TAXITEMGROUPHEADING', '59794233-3CAF-4FE5-9919-A4E2DE3AE5A3', NULL, 'TAXITEMGROUPHEADING', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TAXITEMGROUPHEADING',
      TableFrom = '59794233-3CAF-4FE5-9919-A4E2DE3AE5A3',
      StoredProcName = NULL,
      TableNameTo = 'TAXITEMGROUPHEADING',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D30D7FDF-C8E2-46F9-9B90-5D67588B2950'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D0B1EA84-B49B-4BC1-93E6-5E94441532AA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D0B1EA84-B49B-4BC1-93E6-5E94441532AA', 'N-JscSubJobs', 'C737F5D6-7AA3-41C1-BA99-8271FE6C4D67', NULL, 'JscSubJobs', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscSubJobs',
      TableFrom = 'C737F5D6-7AA3-41C1-BA99-8271FE6C4D67',
      StoredProcName = NULL,
      TableNameTo = 'JscSubJobs',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D0B1EA84-B49B-4BC1-93E6-5E94441532AA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'FE76BC67-C3C6-4ED3-A97A-5E9F93031BC9')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('FE76BC67-C3C6-4ED3-A97A-5E9F93031BC9', 'N-POSISTENDERRESTRICTIONS', '95368CC9-7A60-4270-B995-545F9326B161', NULL, 'POSISTENDERRESTRICTIONS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISTENDERRESTRICTIONS',
      TableFrom = '95368CC9-7A60-4270-B995-545F9326B161',
      StoredProcName = NULL,
      TableNameTo = 'POSISTENDERRESTRICTIONS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'FE76BC67-C3C6-4ED3-A97A-5E9F93031BC9'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '782EA679-33A3-4CC8-9A59-608B8F12A1DE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('782EA679-33A3-4CC8-9A59-608B8F12A1DE', 'N-POSCOLOR', '428DD277-1E97-4FBA-9B08-1D5ACE222DA2', NULL, 'POSCOLOR', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSCOLOR',
      TableFrom = '428DD277-1E97-4FBA-9B08-1D5ACE222DA2',
      StoredProcName = NULL,
      TableNameTo = 'POSCOLOR',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '782EA679-33A3-4CC8-9A59-608B8F12A1DE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '896F2541-2337-4874-B85E-60FD3F075EE9')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('896F2541-2337-4874-B85E-60FD3F075EE9', 'A-RBOPARAMETERS', 'AD45C2CE-6E1E-42F8-9E01-BB7EE2DB600E', NULL, 'RBOPARAMETERS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOPARAMETERS',
      TableFrom = 'AD45C2CE-6E1E-42F8-9E01-BB7EE2DB600E',
      StoredProcName = NULL,
      TableNameTo = 'RBOPARAMETERS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '896F2541-2337-4874-B85E-60FD3F075EE9'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4594E0BD-DA74-448C-AFED-611E73B79A0B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4594E0BD-DA74-448C-AFED-611E73B79A0B', 'A-RBODISCOUNTOFFERLINE', 'FA14B78B-CF5D-456C-93C5-3227E380C545', NULL, 'RBODISCOUNTOFFERLINE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBODISCOUNTOFFERLINE',
      TableFrom = 'FA14B78B-CF5D-456C-93C5-3227E380C545',
      StoredProcName = NULL,
      TableNameTo = 'RBODISCOUNTOFFERLINE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4594E0BD-DA74-448C-AFED-611E73B79A0B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'EA279FE4-6815-4757-9CE1-62C44CC73348')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('EA279FE4-6815-4757-9CE1-62C44CC73348', 'A-POSISRFIDTABLE', 'AB7CC7E2-1BFE-46E4-858A-3AA47B6588F6', NULL, 'POSISRFIDTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISRFIDTABLE',
      TableFrom = 'AB7CC7E2-1BFE-46E4-858A-3AA47B6588F6',
      StoredProcName = NULL,
      TableNameTo = 'POSISRFIDTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'EA279FE4-6815-4757-9CE1-62C44CC73348'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A173A8F1-C3AC-4E95-98EC-63B1B1887EF7')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A173A8F1-C3AC-4E95-98EC-63B1B1887EF7', 'A-CUSTOMERADDRESSTYPE', 'AB4F3B96-1688-4D31-A19B-5F95896C3E80', NULL, 'CUSTOMERADDRESSTYPE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CUSTOMERADDRESSTYPE',
      TableFrom = 'AB4F3B96-1688-4D31-A19B-5F95896C3E80',
      StoredProcName = NULL,
      TableNameTo = 'CUSTOMERADDRESSTYPE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A173A8F1-C3AC-4E95-98EC-63B1B1887EF7'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'C484FAD3-2F8E-4B6B-8356-63F53DB245D2')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('C484FAD3-2F8E-4B6B-8356-63F53DB245D2', 'A-INVENTDIMGROUP', '96DAA48F-692C-4AAC-88FA-C30FC59A5F94', NULL, 'INVENTDIMGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-INVENTDIMGROUP',
      TableFrom = '96DAA48F-692C-4AAC-88FA-C30FC59A5F94',
      StoredProcName = NULL,
      TableNameTo = 'INVENTDIMGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'C484FAD3-2F8E-4B6B-8356-63F53DB245D2'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '37A72F76-F2AA-4FA0-8D89-6534F302C045')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('37A72F76-F2AA-4FA0-8D89-6534F302C045', 'A-POSISBLANKOPERATIONS', '834C2D13-62FB-48CB-868D-727F413C0079', NULL, 'POSISBLANKOPERATIONS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISBLANKOPERATIONS',
      TableFrom = '834C2D13-62FB-48CB-868D-727F413C0079',
      StoredProcName = NULL,
      TableNameTo = 'POSISBLANKOPERATIONS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '37A72F76-F2AA-4FA0-8D89-6534F302C045'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '75198E15-AA95-4FF1-BA99-66333CC55181')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('75198E15-AA95-4FF1-BA99-66333CC55181', 'A-RESTAURANTDININGTABLE', '7A8EB4B4-8940-4200-8BD3-DACE5B5226EC', NULL, 'RESTAURANTDININGTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RESTAURANTDININGTABLE',
      TableFrom = '7A8EB4B4-8940-4200-8BD3-DACE5B5226EC',
      StoredProcName = NULL,
      TableNameTo = 'RESTAURANTDININGTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '75198E15-AA95-4FF1-BA99-66333CC55181'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E8C06281-384D-4F2A-8E50-663FC7EC5494')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E8C06281-384D-4F2A-8E50-663FC7EC5494', 'N-POSISHOSPITALITYTRANSTABLE', 'C94BFD1E-0A21-4720-85AE-63F9427AF22B', NULL, 'POSISHOSPITALITYTRANSTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISHOSPITALITYTRANSTABLE',
      TableFrom = 'C94BFD1E-0A21-4720-85AE-63F9427AF22B',
      StoredProcName = NULL,
      TableNameTo = 'POSISHOSPITALITYTRANSTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E8C06281-384D-4F2A-8E50-663FC7EC5494'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '67DEA1A2-51CC-47A7-B6CF-669B57A8E03A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('67DEA1A2-51CC-47A7-B6CF-669B57A8E03A', 'A-TAXDATA', '172F00E5-B542-4B23-A3FF-65D3473F5A25', NULL, 'TAXDATA', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TAXDATA',
      TableFrom = '172F00E5-B542-4B23-A3FF-65D3473F5A25',
      StoredProcName = NULL,
      TableNameTo = 'TAXDATA',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '67DEA1A2-51CC-47A7-B6CF-669B57A8E03A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6D4181EA-707A-4936-8531-66DD67586469')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6D4181EA-707A-4936-8531-66DD67586469', 'N-INVENTITEMGROUP', '9CDF18E3-B83F-45FB-9B1D-046BA979FBF4', NULL, 'INVENTITEMGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-INVENTITEMGROUP',
      TableFrom = '9CDF18E3-B83F-45FB-9B1D-046BA979FBF4',
      StoredProcName = NULL,
      TableNameTo = 'INVENTITEMGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6D4181EA-707A-4936-8531-66DD67586469'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3CF2377C-09C0-449E-A53A-6866987CA935')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3CF2377C-09C0-449E-A53A-6866987CA935', 'A-RBOSTYLEGROUPTABLE', 'E33E0E5A-C11E-421E-8038-E5412BC107BC', NULL, 'RBOSTYLEGROUPTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTYLEGROUPTABLE',
      TableFrom = 'E33E0E5A-C11E-421E-8038-E5412BC107BC',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTYLEGROUPTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3CF2377C-09C0-449E-A53A-6866987CA935'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '9B794DC8-75BD-4D31-94CF-696DCEA94D88')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('9B794DC8-75BD-4D31-94CF-696DCEA94D88', 'N-POSLOOKUP', '7970B1A5-5BE5-4EB9-809A-1FC0807E9C0B', NULL, 'POSLOOKUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSLOOKUP',
      TableFrom = '7970B1A5-5BE5-4EB9-809A-1FC0807E9C0B',
      StoredProcName = NULL,
      TableNameTo = 'POSLOOKUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '9B794DC8-75BD-4D31-94CF-696DCEA94D88'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E2F25D3E-892A-4580-98F4-698FC91D57F9')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E2F25D3E-892A-4580-98F4-698FC91D57F9', 'N-DIMENSIONATTRIBUTE', '249AB47A-DBEC-49BE-88B6-9945F641EFD1', NULL, 'DIMENSIONATTRIBUTE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-DIMENSIONATTRIBUTE',
      TableFrom = '249AB47A-DBEC-49BE-88B6-9945F641EFD1',
      StoredProcName = NULL,
      TableNameTo = 'DIMENSIONATTRIBUTE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E2F25D3E-892A-4580-98F4-698FC91D57F9'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '94C5EE18-439A-4EEA-8696-69EAB0D7D002')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('94C5EE18-439A-4EEA-8696-69EAB0D7D002', 'N-COMPANYINFO', 'FBDA1538-2C94-4BB8-8A87-635CE2A6540E', NULL, 'COMPANYINFO', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-COMPANYINFO',
      TableFrom = 'FBDA1538-2C94-4BB8-8A87-635CE2A6540E',
      StoredProcName = NULL,
      TableNameTo = 'COMPANYINFO',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '94C5EE18-439A-4EEA-8696-69EAB0D7D002'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '9B1A4DF8-95E1-4DAD-A6FF-6A693BB4CE2A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('9B1A4DF8-95E1-4DAD-A6FF-6A693BB4CE2A', 'N-RBOTERMINALGROUP', '374430E6-BADB-4513-913D-56A6AD8114C6', NULL, 'RBOTERMINALGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTERMINALGROUP',
      TableFrom = '374430E6-BADB-4513-913D-56A6AD8114C6',
      StoredProcName = NULL,
      TableNameTo = 'RBOTERMINALGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '9B1A4DF8-95E1-4DAD-A6FF-6A693BB4CE2A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F38FD301-5CA6-4E24-8F16-6B5CFED13998')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F38FD301-5CA6-4E24-8F16-6B5CFED13998', 'N-JscSchedulerSubLog', '7459657B-B777-4289-849F-8F5DFE559D0B', NULL, 'JscSchedulerSubLog', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscSchedulerSubLog',
      TableFrom = '7459657B-B777-4289-849F-8F5DFE559D0B',
      StoredProcName = NULL,
      TableNameTo = 'JscSchedulerSubLog',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F38FD301-5CA6-4E24-8F16-6B5CFED13998'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '40124AA7-70CD-436B-911C-6BC99440F6A3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('40124AA7-70CD-436B-911C-6BC99440F6A3', 'N-RBOSTAFFTABLE', '32B1AF89-D8E1-489A-AEBE-309DBDF558EA', NULL, 'RBOSTAFFTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSTAFFTABLE',
      TableFrom = '32B1AF89-D8E1-489A-AEBE-309DBDF558EA',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTAFFTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '40124AA7-70CD-436B-911C-6BC99440F6A3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6271A330-64AF-4739-8ABF-6BDA6FCBF602')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6271A330-64AF-4739-8ABF-6BDA6FCBF602', 'A-STATIONPRINTINGROUTES', 'F5CCBC69-5B9A-4630-8B1E-6C50E49F6EED', NULL, 'STATIONPRINTINGROUTES', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-STATIONPRINTINGROUTES',
      TableFrom = 'F5CCBC69-5B9A-4630-8B1E-6C50E49F6EED',
      StoredProcName = NULL,
      TableNameTo = 'STATIONPRINTINGROUTES',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6271A330-64AF-4739-8ABF-6BDA6FCBF602'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2DF0009C-261B-43B4-9309-6CB28BE521B1')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2DF0009C-261B-43B4-9309-6CB28BE521B1', 'A-POSISFORMLAYOUT', '1A66E8EB-BFC5-43C8-BCB1-B6198C2BC6F3', NULL, 'POSISFORMLAYOUT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISFORMLAYOUT',
      TableFrom = '1A66E8EB-BFC5-43C8-BCB1-B6198C2BC6F3',
      StoredProcName = NULL,
      TableNameTo = 'POSISFORMLAYOUT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2DF0009C-261B-43B4-9309-6CB28BE521B1'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'BD92B7EF-6DD6-4005-954E-6CC847E6934A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('BD92B7EF-6DD6-4005-954E-6CC847E6934A', 'A-PURCHASEORDERS', '8FFAC02F-3B53-429F-92D3-074D417C8E90', NULL, 'PURCHASEORDERS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-PURCHASEORDERS',
      TableFrom = '8FFAC02F-3B53-429F-92D3-074D417C8E90',
      StoredProcName = NULL,
      TableNameTo = 'PURCHASEORDERS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'BD92B7EF-6DD6-4005-954E-6CC847E6934A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'BBEC9E5E-3702-407C-98F5-6DBEAE95BF48')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('BBEC9E5E-3702-407C-98F5-6DBEAE95BF48', 'N-ITEMREPLENISHMENTSETTING', '2A68DBE7-9AB0-4003-B4FF-6BAF10F39E75', NULL, 'ITEMREPLENISHMENTSETTING', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-ITEMREPLENISHMENTSETTING',
      TableFrom = '2A68DBE7-9AB0-4003-B4FF-6BAF10F39E75',
      StoredProcName = NULL,
      TableNameTo = 'ITEMREPLENISHMENTSETTING',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'BBEC9E5E-3702-407C-98F5-6DBEAE95BF48'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'C7F1FC8F-5770-4E60-9E7B-6EB06DB3E936')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('C7F1FC8F-5770-4E60-9E7B-6EB06DB3E936', 'N-JscLocationMembers', '91BE6E70-212A-45E0-98E0-F72F24600BAE', NULL, 'JscLocationMembers', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscLocationMembers',
      TableFrom = '91BE6E70-212A-45E0-98E0-F72F24600BAE',
      StoredProcName = NULL,
      TableNameTo = 'JscLocationMembers',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'C7F1FC8F-5770-4E60-9E7B-6EB06DB3E936'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '56CF7FDA-7AD7-43EB-B2AB-6F02D99151CA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('56CF7FDA-7AD7-43EB-B2AB-6F02D99151CA', 'N-POSPERIODICDISCOUNTLINE', 'C1AFC1D1-0BB9-4FA9-AEDD-DAE91D565FBE', NULL, 'POSPERIODICDISCOUNTLINE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSPERIODICDISCOUNTLINE',
      TableFrom = 'C1AFC1D1-0BB9-4FA9-AEDD-DAE91D565FBE',
      StoredProcName = NULL,
      TableNameTo = 'POSPERIODICDISCOUNTLINE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '56CF7FDA-7AD7-43EB-B2AB-6F02D99151CA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5D020FAF-986E-4F4E-A341-6FC1AFE49C78')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5D020FAF-986E-4F4E-A341-6FC1AFE49C78', 'N-DIMENSIONTEMPLATE', '5FB9E1EF-FF24-4788-8576-477920259DE8', NULL, 'DIMENSIONTEMPLATE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-DIMENSIONTEMPLATE',
      TableFrom = '5FB9E1EF-FF24-4788-8576-477920259DE8',
      StoredProcName = NULL,
      TableNameTo = 'DIMENSIONTEMPLATE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5D020FAF-986E-4F4E-A341-6FC1AFE49C78'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '977B7276-530A-4563-AD3B-6FEE21CE92C2')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('977B7276-530A-4563-AD3B-6FEE21CE92C2', 'N-HOSPITALITYSETUP', 'D1C50885-5F8B-4807-B533-6C18D2F1E0F7', NULL, 'HOSPITALITYSETUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-HOSPITALITYSETUP',
      TableFrom = 'D1C50885-5F8B-4807-B533-6C18D2F1E0F7',
      StoredProcName = NULL,
      TableNameTo = 'HOSPITALITYSETUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '977B7276-530A-4563-AD3B-6FEE21CE92C2'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '0FA2C1A4-FF38-480B-9EAC-701AB0FB9F0F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('0FA2C1A4-FF38-480B-9EAC-701AB0FB9F0F', 'N-JscFieldMap', '0BF7F3F8-7110-4A14-85CE-084F01BBF7BF', NULL, 'JscFieldMap', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscFieldMap',
      TableFrom = '0BF7F3F8-7110-4A14-85CE-084F01BBF7BF',
      StoredProcName = NULL,
      TableNameTo = 'JscFieldMap',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '0FA2C1A4-FF38-480B-9EAC-701AB0FB9F0F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '315E4C34-15BF-420E-9861-70434A11D3F2')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('315E4C34-15BF-420E-9861-70434A11D3F2', 'N-RETAILITEMDIMENSION', '10400D7D-7F9D-4FFD-B9ED-E10D2C873BE7', NULL, 'RETAILITEMDIMENSION', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RETAILITEMDIMENSION',
      TableFrom = '10400D7D-7F9D-4FFD-B9ED-E10D2C873BE7',
      StoredProcName = NULL,
      TableNameTo = 'RETAILITEMDIMENSION',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '315E4C34-15BF-420E-9861-70434A11D3F2'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '8D975765-CE94-4D91-940A-708C44483B96')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('8D975765-CE94-4D91-940A-708C44483B96', 'T-RBOTRANSACTIONTABLE', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', NULL, 'RBOTRANSACTIONTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'T-RBOTRANSACTIONTABLE',
      TableFrom = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '8D975765-CE94-4D91-940A-708C44483B96'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E58DB7D5-0F2B-4FCC-BE61-73460813406E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E58DB7D5-0F2B-4FCC-BE61-73460813406E', 'A-DININGTABLELAYOUT', '27EACD3A-879D-42CD-8EFB-FC7152F2079D', NULL, 'DININGTABLELAYOUT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-DININGTABLELAYOUT',
      TableFrom = '27EACD3A-879D-42CD-8EFB-FC7152F2079D',
      StoredProcName = NULL,
      TableNameTo = 'DININGTABLELAYOUT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E58DB7D5-0F2B-4FCC-BE61-73460813406E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E0A63760-88CE-491E-A0C0-739D2DC41D51')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E0A63760-88CE-491E-A0C0-739D2DC41D51', 'A-POSDISCVALIDATIONPERIOD', '0C4F6C1A-49CD-45E1-801A-A8817569A318', NULL, 'POSDISCVALIDATIONPERIOD', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSDISCVALIDATIONPERIOD',
      TableFrom = '0C4F6C1A-49CD-45E1-801A-A8817569A318',
      StoredProcName = NULL,
      TableNameTo = 'POSDISCVALIDATIONPERIOD',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E0A63760-88CE-491E-A0C0-739D2DC41D51'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'CAA7AF18-A867-46BC-BCF5-7442E0AACB35')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('CAA7AF18-A867-46BC-BCF5-7442E0AACB35', 'N-DECIMALSETTINGS', '82D3B816-58FC-4D02-9376-00EFC71630A1', NULL, 'DECIMALSETTINGS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-DECIMALSETTINGS',
      TableFrom = '82D3B816-58FC-4D02-9376-00EFC71630A1',
      StoredProcName = NULL,
      TableNameTo = 'DECIMALSETTINGS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'CAA7AF18-A867-46BC-BCF5-7442E0AACB35'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6F32B2B7-850C-450A-92EA-74559F2622AD')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6F32B2B7-850C-450A-92EA-74559F2622AD', 'N-POSHARDWAREPROFILE', '0DDC28AC-EB78-434F-A2C6-23849476E5E5', NULL, 'POSHARDWAREPROFILE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSHARDWAREPROFILE',
      TableFrom = '0DDC28AC-EB78-434F-A2C6-23849476E5E5',
      StoredProcName = NULL,
      TableNameTo = 'POSHARDWAREPROFILE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6F32B2B7-850C-450A-92EA-74559F2622AD'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6A9F7F28-3F73-4A7F-85A9-74B70794472F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6A9F7F28-3F73-4A7F-85A9-74B70794472F', 'N-RBOTRANSACTIONTABLE', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', NULL, 'RBOTRANSACTIONTABLE', 0, 1, 1, 0, NULL, NULL, 0, 0, '70D1AD90-E0E2-4291-8D0A-12CB6CC35810', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONTABLE',
      TableFrom = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONTABLE',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '70D1AD90-E0E2-4291-8D0A-12CB6CC35810',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6A9F7F28-3F73-4A7F-85A9-74B70794472F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A3D487BA-07CD-40CF-AE44-74CAD016BED1')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A3D487BA-07CD-40CF-AE44-74CAD016BED1', 'N-POSFORMPROFILE', '8ADBD0C8-D29C-47B9-BACE-2340E803D917', NULL, 'POSFORMPROFILE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSFORMPROFILE',
      TableFrom = '8ADBD0C8-D29C-47B9-BACE-2340E803D917',
      StoredProcName = NULL,
      TableNameTo = 'POSFORMPROFILE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A3D487BA-07CD-40CF-AE44-74CAD016BED1'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6AC67F54-F5AF-4C68-9696-74D78FD61447')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6AC67F54-F5AF-4C68-9696-74D78FD61447', 'A-POSFUNCTIONALITYPROFILE', 'CFD3BA98-CBB9-4347-866A-9CCA64E78B9F', NULL, 'POSFUNCTIONALITYPROFILE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSFUNCTIONALITYPROFILE',
      TableFrom = 'CFD3BA98-CBB9-4347-866A-9CCA64E78B9F',
      StoredProcName = NULL,
      TableNameTo = 'POSFUNCTIONALITYPROFILE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6AC67F54-F5AF-4C68-9696-74D78FD61447'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4E0E61BA-5CD2-4AAB-B976-7537CA04D94A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4E0E61BA-5CD2-4AAB-B976-7537CA04D94A', 'A-INVENTDIMSETUP', '1D146271-277F-4A17-8822-0B3D962747FB', NULL, 'INVENTDIMSETUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-INVENTDIMSETUP',
      TableFrom = '1D146271-277F-4A17-8822-0B3D962747FB',
      StoredProcName = NULL,
      TableNameTo = 'INVENTDIMSETUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4E0E61BA-5CD2-4AAB-B976-7537CA04D94A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'C4B98552-604A-4B9A-A8EA-7569E9D20A21')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('C4B98552-604A-4B9A-A8EA-7569E9D20A21', 'A-RBOINVENTTABLE', 'C738E16F-B6BF-4FBE-AA48-B35F249CC745', NULL, 'RBOINVENTTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOINVENTTABLE',
      TableFrom = 'C738E16F-B6BF-4FBE-AA48-B35F249CC745',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'C4B98552-604A-4B9A-A8EA-7569E9D20A21'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A7A0EEA3-A04E-45EF-BE8E-75A94F10BEB1')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A7A0EEA3-A04E-45EF-BE8E-75A94F10BEB1', 'A-RBOINVENTLINKEDITEM', '55290174-49C8-4E0E-AAC3-2DBEE4D89964', NULL, 'RBOINVENTLINKEDITEM', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOINVENTLINKEDITEM',
      TableFrom = '55290174-49C8-4E0E-AAC3-2DBEE4D89964',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTLINKEDITEM',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A7A0EEA3-A04E-45EF-BE8E-75A94F10BEB1'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3D700EEB-46D6-4882-9427-763A4AB28CAB')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3D700EEB-46D6-4882-9427-763A4AB28CAB', 'A-REMOTEHOSTS', 'CB4A25D0-BC36-4F82-9A53-33D1AE962C2A', NULL, 'REMOTEHOSTS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-REMOTEHOSTS',
      TableFrom = 'CB4A25D0-BC36-4F82-9A53-33D1AE962C2A',
      StoredProcName = NULL,
      TableNameTo = 'REMOTEHOSTS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3D700EEB-46D6-4882-9427-763A4AB28CAB'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '235E75DA-4B69-4344-A8C0-76BE5DD64195')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('235E75DA-4B69-4344-A8C0-76BE5DD64195', 'A-HOSPITALITYTYPE', '62B7861D-B777-41B6-9E6C-E54A80EA8B50', NULL, 'HOSPITALITYTYPE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-HOSPITALITYTYPE',
      TableFrom = '62B7861D-B777-41B6-9E6C-E54A80EA8B50',
      StoredProcName = NULL,
      TableNameTo = 'HOSPITALITYTYPE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '235E75DA-4B69-4344-A8C0-76BE5DD64195'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '7795A214-0949-425B-B6A0-76C38AEE5A61')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('7795A214-0949-425B-B6A0-76C38AEE5A61', 'N-JscLinkedTables', '4B1FA49B-1FE0-4D76-A126-F39A3E0C1DF5', NULL, 'JscLinkedTables', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscLinkedTables',
      TableFrom = '4B1FA49B-1FE0-4D76-A126-F39A3E0C1DF5',
      StoredProcName = NULL,
      TableNameTo = 'JscLinkedTables',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '7795A214-0949-425B-B6A0-76C38AEE5A61'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '29029075-216D-45C1-A43C-783BAB9945BD')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('29029075-216D-45C1-A43C-783BAB9945BD', 'N-RBOINVENTITEMRETAILGROUP', '9F02D050-30B4-480C-BFE0-E81AD1C724BA', NULL, 'RBOINVENTITEMRETAILGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOINVENTITEMRETAILGROUP',
      TableFrom = '9F02D050-30B4-480C-BFE0-E81AD1C724BA',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTITEMRETAILGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '29029075-216D-45C1-A43C-783BAB9945BD'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '22C6F5E4-86E6-4BDF-9B3D-79157F6CD086')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('22C6F5E4-86E6-4BDF-9B3D-79157F6CD086', 'A-DIMENSIONATTRIBUTE', '249AB47A-DBEC-49BE-88B6-9945F641EFD1', NULL, 'DIMENSIONATTRIBUTE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-DIMENSIONATTRIBUTE',
      TableFrom = '249AB47A-DBEC-49BE-88B6-9945F641EFD1',
      StoredProcName = NULL,
      TableNameTo = 'DIMENSIONATTRIBUTE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '22C6F5E4-86E6-4BDF-9B3D-79157F6CD086'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '62079EEA-5BFF-4DA4-A0C6-793E9939EA19')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('62079EEA-5BFF-4DA4-A0C6-793E9939EA19', 'N-RBOTENDERTYPETABLE', '3B5A324D-190E-449D-A8CE-69A714B019E1', NULL, 'RBOTENDERTYPETABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTENDERTYPETABLE',
      TableFrom = '3B5A324D-190E-449D-A8CE-69A714B019E1',
      StoredProcName = NULL,
      TableNameTo = 'RBOTENDERTYPETABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '62079EEA-5BFF-4DA4-A0C6-793E9939EA19'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'EBA84383-DD17-432F-9368-79EAACDC379C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('EBA84383-DD17-432F-9368-79EAACDC379C', 'N-RETAILITEMDIMENSIONATTRIBUTE', '02701AB4-0CD3-447E-9E46-E0CB747C016D', NULL, 'RETAILITEMDIMENSIONATTRIBUTE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RETAILITEMDIMENSIONATTRIBUTE',
      TableFrom = '02701AB4-0CD3-447E-9E46-E0CB747C016D',
      StoredProcName = NULL,
      TableNameTo = 'RETAILITEMDIMENSIONATTRIBUTE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'EBA84383-DD17-432F-9368-79EAACDC379C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'C1FF78E7-EEE0-43AA-8C84-7A82798F8D38')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('C1FF78E7-EEE0-43AA-8C84-7A82798F8D38', 'A-RBOSIZES', 'FB8E335C-9494-4910-BA6F-8DD904D6535A', NULL, 'RBOSIZES', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSIZES',
      TableFrom = 'FB8E335C-9494-4910-BA6F-8DD904D6535A',
      StoredProcName = NULL,
      TableNameTo = 'RBOSIZES',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'C1FF78E7-EEE0-43AA-8C84-7A82798F8D38'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1CE3F364-696A-4710-9BFA-7B752775FCFC')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1CE3F364-696A-4710-9BFA-7B752775FCFC', 'A-POSPARAMETERSETUP', '199C3808-1249-45D9-8484-66AE863BE916', NULL, 'POSPARAMETERSETUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSPARAMETERSETUP',
      TableFrom = '199C3808-1249-45D9-8484-66AE863BE916',
      StoredProcName = NULL,
      TableNameTo = 'POSPARAMETERSETUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1CE3F364-696A-4710-9BFA-7B752775FCFC'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'CA81E267-78BF-4A7E-B572-7B845B599573')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('CA81E267-78BF-4A7E-B572-7B845B599573', 'N-RESTAURANTDININGTABLE', '7A8EB4B4-8940-4200-8BD3-DACE5B5226EC', NULL, 'RESTAURANTDININGTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RESTAURANTDININGTABLE',
      TableFrom = '7A8EB4B4-8940-4200-8BD3-DACE5B5226EC',
      StoredProcName = NULL,
      TableNameTo = 'RESTAURANTDININGTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'CA81E267-78BF-4A7E-B572-7B845B599573'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E05EE372-B026-4316-A7D5-7D1AB43F6987')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E05EE372-B026-4316-A7D5-7D1AB43F6987', 'N-POSISIMAGES', '53A834AC-629D-4712-AF04-40CA632E3E52', NULL, 'POSISIMAGES', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISIMAGES',
      TableFrom = '53A834AC-629D-4712-AF04-40CA632E3E52',
      StoredProcName = NULL,
      TableNameTo = 'POSISIMAGES',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E05EE372-B026-4316-A7D5-7D1AB43F6987'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '82B58257-E2A7-488E-B60A-7DEF22A6BA5E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('82B58257-E2A7-488E-B60A-7DEF22A6BA5E', 'N-JscJobSubjobs', '21BF260C-CC16-4155-ADF5-7547308BC589', NULL, 'JscJobSubjobs', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscJobSubjobs',
      TableFrom = '21BF260C-CC16-4155-ADF5-7547308BC589',
      StoredProcName = NULL,
      TableNameTo = 'JscJobSubjobs',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '82B58257-E2A7-488E-B60A-7DEF22A6BA5E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '775C539F-D747-47DA-A5B2-7E10799770C6')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('775C539F-D747-47DA-A5B2-7E10799770C6', 'A-RBOINVENTITEMRETAILDIVISION', '5FC36955-2CCD-431E-B6A4-B7A69A2DE25F', NULL, 'RBOINVENTITEMRETAILDIVISION', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOINVENTITEMRETAILDIVISION',
      TableFrom = '5FC36955-2CCD-431E-B6A4-B7A69A2DE25F',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTITEMRETAILDIVISION',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '775C539F-D747-47DA-A5B2-7E10799770C6'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4367C817-6963-4889-9187-7EA9FFDFAA82')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4367C817-6963-4889-9187-7EA9FFDFAA82', 'A-VENDORITEMS', '04EDE76E-ED61-46F9-9B2B-3295619CF021', NULL, 'VENDORITEMS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-VENDORITEMS',
      TableFrom = '04EDE76E-ED61-46F9-9B2B-3295619CF021',
      StoredProcName = NULL,
      TableNameTo = 'VENDORITEMS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4367C817-6963-4889-9187-7EA9FFDFAA82'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '8C575439-1A79-4D9D-A9FB-7EACD4D9F5EC')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('8C575439-1A79-4D9D-A9FB-7EACD4D9F5EC', 'CleanLog', 'D0F2BEF0-382F-4F9A-B0F9-61587CADCF8D', NULL, 'POSISLOG', 0, 3, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'CleanLog',
      TableFrom = 'D0F2BEF0-382F-4F9A-B0F9-61587CADCF8D',
      StoredProcName = NULL,
      TableNameTo = 'POSISLOG',
      ReplicationMethod = 0,
      WhatToDo = 3,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '8C575439-1A79-4D9D-A9FB-7EACD4D9F5EC'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A01C8E32-3623-43AD-91B6-7F3880C5A754')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A01C8E32-3623-43AD-91B6-7F3880C5A754', 'N-TAXTABLE', 'DB1D8C68-C922-427A-8764-B14AA9502963', NULL, 'TAXTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TAXTABLE',
      TableFrom = 'DB1D8C68-C922-427A-8764-B14AA9502963',
      StoredProcName = NULL,
      TableNameTo = 'TAXTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A01C8E32-3623-43AD-91B6-7F3880C5A754'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '825E8319-B8C5-4EC5-9DD6-7FDB16AC9FC7')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('825E8319-B8C5-4EC5-9DD6-7FDB16AC9FC7', 'N-POSISHOSPITALITYDININGTABLES', '616B8634-7EA5-44F6-945A-A8B4EE02E969', NULL, 'POSISHOSPITALITYDININGTABLES', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISHOSPITALITYDININGTABLES',
      TableFrom = '616B8634-7EA5-44F6-945A-A8B4EE02E969',
      StoredProcName = NULL,
      TableNameTo = 'POSISHOSPITALITYDININGTABLES',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '825E8319-B8C5-4EC5-9DD6-7FDB16AC9FC7'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '7E2AFAE2-F750-4B0C-A30D-80562CF19B59')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('7E2AFAE2-F750-4B0C-A30D-80562CF19B59', 'N-PAYMENTLIMITATIONS', '20842E4F-F1DF-42BC-8B0B-AD5C0B50CE6B', NULL, 'PAYMENTLIMITATIONS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-PAYMENTLIMITATIONS',
      TableFrom = '20842E4F-F1DF-42BC-8B0B-AD5C0B50CE6B',
      StoredProcName = NULL,
      TableNameTo = 'PAYMENTLIMITATIONS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '7E2AFAE2-F750-4B0C-A30D-80562CF19B59'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A279F498-BBC7-44B5-8E67-805A367E6D66')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A279F498-BBC7-44B5-8E67-805A367E6D66', 'N-RBOBARCODEMASKSEGMENT', '4F0A2FC6-553D-47D6-8A89-4C95BFA66419', NULL, 'RBOBARCODEMASKSEGMENT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOBARCODEMASKSEGMENT',
      TableFrom = '4F0A2FC6-553D-47D6-8A89-4C95BFA66419',
      StoredProcName = NULL,
      TableNameTo = 'RBOBARCODEMASKSEGMENT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A279F498-BBC7-44B5-8E67-805A367E6D66'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '572C2064-4D30-4CDD-954E-812FF80F849D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('572C2064-4D30-4CDD-954E-812FF80F849D', 'N-INVENTSERIAL', 'C3DFB38F-1DB0-40E3-AA6E-BF0684DD8FB5', NULL, 'INVENTSERIAL', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-INVENTSERIAL',
      TableFrom = 'C3DFB38F-1DB0-40E3-AA6E-BF0684DD8FB5',
      StoredProcName = NULL,
      TableNameTo = 'INVENTSERIAL',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '572C2064-4D30-4CDD-954E-812FF80F849D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'FBDA732C-8A1C-43A5-8357-813667E1C4FE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('FBDA732C-8A1C-43A5-8357-813667E1C4FE', 'A-POSISSUSPENSIONADDINFO', 'EBD44150-2572-416F-83CE-B108F6435684', NULL, 'POSISSUSPENSIONADDINFO', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISSUSPENSIONADDINFO',
      TableFrom = 'EBD44150-2572-416F-83CE-B108F6435684',
      StoredProcName = NULL,
      TableNameTo = 'POSISSUSPENSIONADDINFO',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'FBDA732C-8A1C-43A5-8357-813667E1C4FE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '9666C1E9-798A-4578-89EA-8140A2049ED7')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('9666C1E9-798A-4578-89EA-8140A2049ED7', 'A-POSISLANGUAGETEXT', '4C746A04-7629-4022-9DF7-DED041A71D37', NULL, 'POSISLANGUAGETEXT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISLANGUAGETEXT',
      TableFrom = '4C746A04-7629-4022-9DF7-DED041A71D37',
      StoredProcName = NULL,
      TableNameTo = 'POSISLANGUAGETEXT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '9666C1E9-798A-4578-89EA-8140A2049ED7'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B8E93ECD-4117-4AA9-BA0A-82E242240BE6')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B8E93ECD-4117-4AA9-BA0A-82E242240BE6', 'N-RBOTRANSACTIONPAYMENTTRANS', '26829746-CD0C-4F2B-BA3A-42D4CC36C006', NULL, 'RBOTRANSACTIONPAYMENTTRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, 'AF4590F1-8838-4A7C-94A4-E3EFA68C6D5E', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONPAYMENTTRANS',
      TableFrom = '26829746-CD0C-4F2B-BA3A-42D4CC36C006',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONPAYMENTTRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = 'AF4590F1-8838-4A7C-94A4-E3EFA68C6D5E',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B8E93ECD-4117-4AA9-BA0A-82E242240BE6'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B1B4FEBF-AFFA-4836-84DD-83589052EB7D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B1B4FEBF-AFFA-4836-84DD-83589052EB7D', 'A-POSHARDWAREPROFILE', '0DDC28AC-EB78-434F-A2C6-23849476E5E5', NULL, 'POSHARDWAREPROFILE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSHARDWAREPROFILE',
      TableFrom = '0DDC28AC-EB78-434F-A2C6-23849476E5E5',
      StoredProcName = NULL,
      TableNameTo = 'POSHARDWAREPROFILE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B1B4FEBF-AFFA-4836-84DD-83589052EB7D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'C0AF553C-FF12-413D-9061-842278EFFF0A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('C0AF553C-FF12-413D-9061-842278EFFF0A', 'N-RBOCUSTTABLE', '80A5DAFF-BB53-4C82-BB0A-E57EF3A13780', NULL, 'RBOCUSTTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOCUSTTABLE',
      TableFrom = '80A5DAFF-BB53-4C82-BB0A-E57EF3A13780',
      StoredProcName = NULL,
      TableNameTo = 'RBOCUSTTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'C0AF553C-FF12-413D-9061-842278EFFF0A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F2AF88A7-3353-4759-932F-856E957F0CFB')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F2AF88A7-3353-4759-932F-856E957F0CFB', 'A-STATIONPRINTINGHOSTS', 'B278F0E7-B96B-405A-9FCA-6D8F98E6A4D1', NULL, 'STATIONPRINTINGHOSTS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-STATIONPRINTINGHOSTS',
      TableFrom = 'B278F0E7-B96B-405A-9FCA-6D8F98E6A4D1',
      StoredProcName = NULL,
      TableNameTo = 'STATIONPRINTINGHOSTS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F2AF88A7-3353-4759-932F-856E957F0CFB'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'BB650AD6-86F1-49E2-9437-8727E29C1230')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('BB650AD6-86F1-49E2-9437-8727E29C1230', 'N-USERLOGINTOKENS', '96824EE4-2C3E-4F2A-A68F-5169214D9573', NULL, 'USERLOGINTOKENS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-USERLOGINTOKENS',
      TableFrom = '96824EE4-2C3E-4F2A-A68F-5169214D9573',
      StoredProcName = NULL,
      TableNameTo = 'USERLOGINTOKENS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'BB650AD6-86F1-49E2-9437-8727E29C1230'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '7FFE742F-3069-490B-9C85-8764AA1AD8E2')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('7FFE742F-3069-490B-9C85-8764AA1AD8E2', 'A-RETAILDEPARTMENT', '2342EFAC-7AC2-4257-8772-C5C7C30165F2', NULL, 'RETAILDEPARTMENT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RETAILDEPARTMENT',
      TableFrom = '2342EFAC-7AC2-4257-8772-C5C7C30165F2',
      StoredProcName = NULL,
      TableNameTo = 'RETAILDEPARTMENT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '7FFE742F-3069-490B-9C85-8764AA1AD8E2'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '56C952DA-DC97-40F0-9930-8875C0E92E4D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('56C952DA-DC97-40F0-9930-8875C0E92E4D', 'N-JscJobs', 'E7B8CEEE-FE51-4B8C-8DF8-449DF5ECC9C2', NULL, 'JscJobs', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscJobs',
      TableFrom = 'E7B8CEEE-FE51-4B8C-8DF8-449DF5ECC9C2',
      StoredProcName = NULL,
      TableNameTo = 'JscJobs',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '56C952DA-DC97-40F0-9930-8875C0E92E4D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2874FA28-1CF1-4819-8197-88B432E976CC')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2874FA28-1CF1-4819-8197-88B432E976CC', 'N-RBOTRANSACTIONLOYALTYPOINTTRANS', '9637A056-A63B-4260-81E2-44E703914C47', NULL, 'RBOTRANSACTIONLOYALTYPOINTTRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, 'BEBAF425-DA61-4922-B99C-717C04A83124', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONLOYALTYPOINTTRANS',
      TableFrom = '9637A056-A63B-4260-81E2-44E703914C47',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONLOYALTYPOINTTRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = 'BEBAF425-DA61-4922-B99C-717C04A83124',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2874FA28-1CF1-4819-8197-88B432E976CC'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '586F138B-55B2-4592-B857-8AF1F6DFA738')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('586F138B-55B2-4592-B857-8AF1F6DFA738', 'A-CUSTTABLE', 'FE8F1B25-11F8-48A2-96E8-AF65A2B8BC5C', NULL, 'CUSTTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CUSTTABLE',
      TableFrom = 'FE8F1B25-11F8-48A2-96E8-AF65A2B8BC5C',
      StoredProcName = NULL,
      TableNameTo = 'CUSTTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '586F138B-55B2-4592-B857-8AF1F6DFA738'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '8C72D43F-E488-42AA-AFBE-8B4C0E3C5A5C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('8C72D43F-E488-42AA-AFBE-8B4C0E3C5A5C', 'N-INVENTTABLE', '15F0B991-2F18-47D7-8B47-E6FE78E819E4', NULL, 'INVENTTABLE', 0, -1, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-INVENTTABLE',
      TableFrom = '15F0B991-2F18-47D7-8B47-E6FE78E819E4',
      StoredProcName = NULL,
      TableNameTo = 'INVENTTABLE',
      ReplicationMethod = 0,
      WhatToDo = -1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '8C72D43F-E488-42AA-AFBE-8B4C0E3C5A5C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F824EBB9-BEBE-4684-B4FD-8B7047B386E1')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F824EBB9-BEBE-4684-B4FD-8B7047B386E1', 'N-TAXFREECONFIG', '56E9B0B4-C118-40AC-A491-46D5C5A19C40', NULL, 'TAXFREECONFIG', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TAXFREECONFIG',
      TableFrom = '56E9B0B4-C118-40AC-A491-46D5C5A19C40',
      StoredProcName = NULL,
      TableNameTo = 'TAXFREECONFIG',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F824EBB9-BEBE-4684-B4FD-8B7047B386E1'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '7682086B-F5AD-4C1E-A089-8C37EADF4C13')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('7682086B-F5AD-4C1E-A089-8C37EADF4C13', 'N-TAXGROUPDATA', '7D117979-6EB1-4163-A931-BB1B1A0B2FCB', NULL, 'TAXGROUPDATA', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TAXGROUPDATA',
      TableFrom = '7D117979-6EB1-4163-A931-BB1B1A0B2FCB',
      StoredProcName = NULL,
      TableNameTo = 'TAXGROUPDATA',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '7682086B-F5AD-4C1E-A089-8C37EADF4C13'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4D1E8C85-9F05-4174-8C98-8C45115D8150')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4D1E8C85-9F05-4174-8C98-8C45115D8150', 'A-RBOTENDERTYPECARDTABLE', '374B165E-18FF-4D12-8109-32A3240B37AC', NULL, 'RBOTENDERTYPECARDTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOTENDERTYPECARDTABLE',
      TableFrom = '374B165E-18FF-4D12-8109-32A3240B37AC',
      StoredProcName = NULL,
      TableNameTo = 'RBOTENDERTYPECARDTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4D1E8C85-9F05-4174-8C98-8C45115D8150'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E11276B7-0C89-4B1D-9F31-8CCFEE7BEB15')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E11276B7-0C89-4B1D-9F31-8CCFEE7BEB15', 'N-USERSINGROUP', '66CBDD42-0D72-4AC2-B54F-0B43BD1B1097', NULL, 'USERSINGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-USERSINGROUP',
      TableFrom = '66CBDD42-0D72-4AC2-B54F-0B43BD1B1097',
      StoredProcName = NULL,
      TableNameTo = 'USERSINGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E11276B7-0C89-4B1D-9F31-8CCFEE7BEB15'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '7527223E-2DCE-47C1-BE14-8CF86019DDF6')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('7527223E-2DCE-47C1-BE14-8CF86019DDF6', 'A-BARCODESETUP', 'AE77A066-DA2E-47C2-B7B0-CA153574D47B', NULL, 'BARCODESETUP', 1, -1, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-BARCODESETUP',
      TableFrom = 'AE77A066-DA2E-47C2-B7B0-CA153574D47B',
      StoredProcName = NULL,
      TableNameTo = 'BARCODESETUP',
      ReplicationMethod = 1,
      WhatToDo = -1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '7527223E-2DCE-47C1-BE14-8CF86019DDF6'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '221CCC8C-61E1-4600-A971-8D4E05F2A11D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('221CCC8C-61E1-4600-A971-8D4E05F2A11D', 'A-INVENTDIMCOMBINATION', '55AE0BF5-DA34-4262-9078-B9062EACEB1B', NULL, 'INVENTDIMCOMBINATION', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-INVENTDIMCOMBINATION',
      TableFrom = '55AE0BF5-DA34-4262-9078-B9062EACEB1B',
      StoredProcName = NULL,
      TableNameTo = 'INVENTDIMCOMBINATION',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '221CCC8C-61E1-4600-A971-8D4E05F2A11D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'ED195159-EE3B-47BA-BF02-8DFDAAD4860D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('ED195159-EE3B-47BA-BF02-8DFDAAD4860D', 'N-POSISCARDTOTENDERMAPPING', 'F17F8BF3-5528-44D5-A1C4-CD0BE26D50AF', NULL, 'POSISCARDTOTENDERMAPPING', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISCARDTOTENDERMAPPING',
      TableFrom = 'F17F8BF3-5528-44D5-A1C4-CD0BE26D50AF',
      StoredProcName = NULL,
      TableNameTo = 'POSISCARDTOTENDERMAPPING',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'ED195159-EE3B-47BA-BF02-8DFDAAD4860D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6DE904B6-33F4-4222-9E09-8E56B2B7ED08')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6DE904B6-33F4-4222-9E09-8E56B2B7ED08', 'N-RETAILDEPARTMENT', '2342EFAC-7AC2-4257-8772-C5C7C30165F2', NULL, 'RETAILDEPARTMENT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RETAILDEPARTMENT',
      TableFrom = '2342EFAC-7AC2-4257-8772-C5C7C30165F2',
      StoredProcName = NULL,
      TableNameTo = 'RETAILDEPARTMENT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6DE904B6-33F4-4222-9E09-8E56B2B7ED08'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'ACC941A2-4246-4428-8FE6-8E6EE850CC1B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('ACC941A2-4246-4428-8FE6-8E6EE850CC1B', 'A-CUSTOMER', '41896D98-1EFB-490A-92AD-E3063144EBF0', NULL, 'CUSTOMER', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CUSTOMER',
      TableFrom = '41896D98-1EFB-490A-92AD-E3063144EBF0',
      StoredProcName = NULL,
      TableNameTo = 'CUSTOMER',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'ACC941A2-4246-4428-8FE6-8E6EE850CC1B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D7284BDE-C81F-440D-A7FC-8EF8C81A5B47')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D7284BDE-C81F-440D-A7FC-8EF8C81A5B47', 'A-POSSTYLE', '68BDB08D-1F2C-495A-A5CD-42EFDC0E918C', NULL, 'POSSTYLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSSTYLE',
      TableFrom = '68BDB08D-1F2C-495A-A5CD-42EFDC0E918C',
      StoredProcName = NULL,
      TableNameTo = 'POSSTYLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D7284BDE-C81F-440D-A7FC-8EF8C81A5B47'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5DA7EAF0-B67C-4856-9783-8FA1B73D4948')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5DA7EAF0-B67C-4856-9783-8FA1B73D4948', 'N-VENDORITEMS', '04EDE76E-ED61-46F9-9B2B-3295619CF021', NULL, 'VENDORITEMS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-VENDORITEMS',
      TableFrom = '04EDE76E-ED61-46F9-9B2B-3295619CF021',
      StoredProcName = NULL,
      TableNameTo = 'VENDORITEMS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5DA7EAF0-B67C-4856-9783-8FA1B73D4948'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '289977F8-17DF-42E4-99E2-901D39521DD6')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('289977F8-17DF-42E4-99E2-901D39521DD6', 'N-RBOTRANSACTIONDISCOUNTTRANS', 'CBD1E096-6F8E-4A56-A065-84E185335FE8', NULL, 'RBOTRANSACTIONDISCOUNTTRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, '99C3F415-6BB7-4A1D-9DD0-306929128F87', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONDISCOUNTTRANS',
      TableFrom = 'CBD1E096-6F8E-4A56-A065-84E185335FE8',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONDISCOUNTTRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '99C3F415-6BB7-4A1D-9DD0-306929128F87',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '289977F8-17DF-42E4-99E2-901D39521DD6'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1B52A4AC-10E6-4A35-B5E1-911C1D79D887')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1B52A4AC-10E6-4A35-B5E1-911C1D79D887', 'A-RBOBARCODEMASKCHARACTER', 'AFA0495C-BF6D-4C9E-81D5-4AA709DCDEE6', NULL, 'RBOBARCODEMASKCHARACTER', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOBARCODEMASKCHARACTER',
      TableFrom = 'AFA0495C-BF6D-4C9E-81D5-4AA709DCDEE6',
      StoredProcName = NULL,
      TableNameTo = 'RBOBARCODEMASKCHARACTER',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1B52A4AC-10E6-4A35-B5E1-911C1D79D887'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '63F3E266-81BC-4B9A-8E74-914D5714E642')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('63F3E266-81BC-4B9A-8E74-914D5714E642', 'N-POSISFUELLINGPOINTSOUNDS', '6B524099-44A9-44E2-978D-F658952C65ED', NULL, 'POSISFUELLINGPOINTSOUNDS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISFUELLINGPOINTSOUNDS',
      TableFrom = '6B524099-44A9-44E2-978D-F658952C65ED',
      StoredProcName = NULL,
      TableNameTo = 'POSISFUELLINGPOINTSOUNDS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '63F3E266-81BC-4B9A-8E74-914D5714E642'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B672D073-6785-4D36-BF0C-91C210321B6F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B672D073-6785-4D36-BF0C-91C210321B6F', 'N-CUSTOMERORDERSADDITIONALCONFIG', 'B1983C3F-1C06-4BAB-B6B9-D4E3F0F2E745', NULL, 'CUSTOMERORDERSADDITIONALCONFIG', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CUSTOMERORDERSADDITIONALCONFIG',
      TableFrom = 'B1983C3F-1C06-4BAB-B6B9-D4E3F0F2E745',
      StoredProcName = NULL,
      TableNameTo = 'CUSTOMERORDERSADDITIONALCONFIG',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B672D073-6785-4D36-BF0C-91C210321B6F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '74E2BD58-F65B-4B14-9500-928481D27000')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('74E2BD58-F65B-4B14-9500-928481D27000', 'A-POSFORMTYPE', '972D4141-2A8A-448C-B3A3-DCABD10DF657', NULL, 'POSFORMTYPE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSFORMTYPE',
      TableFrom = '972D4141-2A8A-448C-B3A3-DCABD10DF657',
      StoredProcName = NULL,
      TableNameTo = 'POSFORMTYPE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '74E2BD58-F65B-4B14-9500-928481D27000'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '0ED1D38F-670A-422E-B2C8-946F9E66EE1F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('0ED1D38F-670A-422E-B2C8-946F9E66EE1F', 'A-RETAILDIVISION', '5F26F8B9-278E-4D04-85A2-88855536CFB1', NULL, 'RETAILDIVISION', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RETAILDIVISION',
      TableFrom = '5F26F8B9-278E-4D04-85A2-88855536CFB1',
      StoredProcName = NULL,
      TableNameTo = 'RETAILDIVISION',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '0ED1D38F-670A-422E-B2C8-946F9E66EE1F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '90DA69BD-47D8-4E3F-81BD-958FBD84AB7C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('90DA69BD-47D8-4E3F-81BD-958FBD84AB7C', 'N-RBOTRANSACTIONFISCALLOG', '1B31C396-F9B4-4D44-8570-3A96D319D09A', NULL, 'RBOTRANSACTIONFISCALLOG', 0, 1, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONFISCALLOG',
      TableFrom = '1B31C396-F9B4-4D44-8570-3A96D319D09A',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONFISCALLOG',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '90DA69BD-47D8-4E3F-81BD-958FBD84AB7C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B64C835D-D23D-4226-93A8-95A0989C3C9E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B64C835D-D23D-4226-93A8-95A0989C3C9E', 'A-PAYMENTLIMITATIONS', '20842E4F-F1DF-42BC-8B0B-AD5C0B50CE6B', NULL, 'PAYMENTLIMITATIONS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-PAYMENTLIMITATIONS',
      TableFrom = '20842E4F-F1DF-42BC-8B0B-AD5C0B50CE6B',
      StoredProcName = NULL,
      TableNameTo = 'PAYMENTLIMITATIONS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B64C835D-D23D-4226-93A8-95A0989C3C9E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E4B6130D-D29C-480A-8A3B-95DB08804063')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E4B6130D-D29C-480A-8A3B-95DB08804063', 'A-REPORTLOCALIZATION', '3AB1417A-4965-43CF-AF54-97872724D04F', NULL, 'REPORTLOCALIZATION', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-REPORTLOCALIZATION',
      TableFrom = '3AB1417A-4965-43CF-AF54-97872724D04F',
      StoredProcName = NULL,
      TableNameTo = 'REPORTLOCALIZATION',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E4B6130D-D29C-480A-8A3B-95DB08804063'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '017CB8D2-F32D-4855-BA47-965546165050')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('017CB8D2-F32D-4855-BA47-965546165050', 'N-RBOSTORETENDERTYPETABLE', '85DE7B50-929A-4DDA-9643-3886DA71DEAF', NULL, 'RBOSTORETENDERTYPETABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSTORETENDERTYPETABLE',
      TableFrom = '85DE7B50-929A-4DDA-9643-3886DA71DEAF',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTORETENDERTYPETABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '017CB8D2-F32D-4855-BA47-965546165050'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4827F8D2-94B1-4AC3-A9F1-96677DECB0C9')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4827F8D2-94B1-4AC3-A9F1-96677DECB0C9', 'A-RBOSPECIALGROUPITEMS', 'EBDFE573-75A0-46C8-A9B4-AEF284A319EB', NULL, 'RBOSPECIALGROUPITEMS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSPECIALGROUPITEMS',
      TableFrom = 'EBDFE573-75A0-46C8-A9B4-AEF284A319EB',
      StoredProcName = NULL,
      TableNameTo = 'RBOSPECIALGROUPITEMS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4827F8D2-94B1-4AC3-A9F1-96677DECB0C9'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D830DFCA-1DDC-45BA-A451-96F19CD38038')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D830DFCA-1DDC-45BA-A451-96F19CD38038', 'N-SIGNEDACTIONS', '93E643EE-503A-4D22-B4EC-C0E8828C9A9B', NULL, 'SIGNEDACTIONS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-SIGNEDACTIONS',
      TableFrom = '93E643EE-503A-4D22-B4EC-C0E8828C9A9B',
      StoredProcName = NULL,
      TableNameTo = 'SIGNEDACTIONS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D830DFCA-1DDC-45BA-A451-96F19CD38038'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'EA39E4F0-71CE-47F5-84DA-97A76495398B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('EA39E4F0-71CE-47F5-84DA-97A76495398B', 'A-RBOTERMINALTABLE', 'B2D4CDE3-3F64-4C40-9FF5-505A052CDF7D', NULL, 'RBOTERMINALTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOTERMINALTABLE',
      TableFrom = 'B2D4CDE3-3F64-4C40-9FF5-505A052CDF7D',
      StoredProcName = NULL,
      TableNameTo = 'RBOTERMINALTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'EA39E4F0-71CE-47F5-84DA-97A76495398B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '52CD9061-A47F-49F3-AFDB-9843B19F5B9B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('52CD9061-A47F-49F3-AFDB-9843B19F5B9B', 'N-POSISLICENSE', '3E9FB0BC-2A6B-4B32-ACBA-C404288ABA13', NULL, 'POSISLICENSE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISLICENSE',
      TableFrom = '3E9FB0BC-2A6B-4B32-ACBA-C404288ABA13',
      StoredProcName = NULL,
      TableNameTo = 'POSISLICENSE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '52CD9061-A47F-49F3-AFDB-9843B19F5B9B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B78C8E26-AAC0-4273-9B8F-98758DA2B0CD')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B78C8E26-AAC0-4273-9B8F-98758DA2B0CD', 'A-PURCHASEORDERMISCCHARGES', '46795F49-2D45-450B-9768-9C8688FB4EFD', NULL, 'PURCHASEORDERMISCCHARGES', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-PURCHASEORDERMISCCHARGES',
      TableFrom = '46795F49-2D45-450B-9768-9C8688FB4EFD',
      StoredProcName = NULL,
      TableNameTo = 'PURCHASEORDERMISCCHARGES',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B78C8E26-AAC0-4273-9B8F-98758DA2B0CD'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '740DA1DA-9FD3-4F59-B5FB-9941C3A70F10')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('740DA1DA-9FD3-4F59-B5FB-9941C3A70F10', 'A-POSISLICENSE', '3E9FB0BC-2A6B-4B32-ACBA-C404288ABA13', NULL, 'POSISLICENSE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISLICENSE',
      TableFrom = '3E9FB0BC-2A6B-4B32-ACBA-C404288ABA13',
      StoredProcName = NULL,
      TableNameTo = 'POSISLICENSE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '740DA1DA-9FD3-4F59-B5FB-9941C3A70F10'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2A286FA1-1488-452D-866E-9BFBD6013DC9')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2A286FA1-1488-452D-866E-9BFBD6013DC9', 'N-TAXCOLLECTLIMIT', 'F5A6179B-DE3D-44CB-8F5F-3EEEDD505E20', NULL, 'TAXCOLLECTLIMIT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TAXCOLLECTLIMIT',
      TableFrom = 'F5A6179B-DE3D-44CB-8F5F-3EEEDD505E20',
      StoredProcName = NULL,
      TableNameTo = 'TAXCOLLECTLIMIT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2A286FA1-1488-452D-866E-9BFBD6013DC9'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E1896884-D3E4-4F47-8FAE-9C067BC5E414')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E1896884-D3E4-4F47-8FAE-9C067BC5E414', 'N-RBOTERMINALGROUPCONNECTION', 'F1158E4F-7F8C-499D-92AF-11484F32C8FD', NULL, 'RBOTERMINALGROUPCONNECTION', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTERMINALGROUPCONNECTION',
      TableFrom = 'F1158E4F-7F8C-499D-92AF-11484F32C8FD',
      StoredProcName = NULL,
      TableNameTo = 'RBOTERMINALGROUPCONNECTION',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E1896884-D3E4-4F47-8FAE-9C067BC5E414'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5F73940C-9A38-4DD2-A2B7-9D028FAAE57C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5F73940C-9A38-4DD2-A2B7-9D028FAAE57C', 'N-RBOSIZEGROUPTABLE', 'A36163FF-F083-420E-9286-B7F520ADDB6B', NULL, 'RBOSIZEGROUPTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSIZEGROUPTABLE',
      TableFrom = 'A36163FF-F083-420E-9286-B7F520ADDB6B',
      StoredProcName = NULL,
      TableNameTo = 'RBOSIZEGROUPTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5F73940C-9A38-4DD2-A2B7-9D028FAAE57C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D691CA9B-B55A-4905-B16F-9E13EFA6C45C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D691CA9B-B55A-4905-B16F-9E13EFA6C45C', 'N-JscSubJobFromTableFilters', 'E3EC6289-83EA-461F-B2BA-FBDDB3A01DC7', NULL, 'JscSubJobFromTableFilters', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscSubJobFromTableFilters',
      TableFrom = 'E3EC6289-83EA-461F-B2BA-FBDDB3A01DC7',
      StoredProcName = NULL,
      TableNameTo = 'JscSubJobFromTableFilters',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D691CA9B-B55A-4905-B16F-9E13EFA6C45C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '89E05C58-B9A4-44A4-BCBB-9E41B0087064')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('89E05C58-B9A4-44A4-BCBB-9E41B0087064', 'N-INVENTDIMGROUP', '96DAA48F-692C-4AAC-88FA-C30FC59A5F94', NULL, 'INVENTDIMGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-INVENTDIMGROUP',
      TableFrom = '96DAA48F-692C-4AAC-88FA-C30FC59A5F94',
      StoredProcName = NULL,
      TableNameTo = 'INVENTDIMGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '89E05C58-B9A4-44A4-BCBB-9E41B0087064'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6E153A08-B807-4920-8F3B-9EA89F2CF77A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6E153A08-B807-4920-8F3B-9EA89F2CF77A', 'A-POSFORMPROFILELINES', 'CAD4BB5C-4E76-4D3E-88BC-D70C660883A6', NULL, 'POSFORMPROFILELINES', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSFORMPROFILELINES',
      TableFrom = 'CAD4BB5C-4E76-4D3E-88BC-D70C660883A6',
      StoredProcName = NULL,
      TableNameTo = 'POSFORMPROFILELINES',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6E153A08-B807-4920-8F3B-9EA89F2CF77A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3930C4AF-67E1-4E9C-B609-A117D27D78B4')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3930C4AF-67E1-4E9C-B609-A117D27D78B4', 'A-RBOSTAFFPERMISSIONGROUP', '8E565A79-2F0F-4FBE-BE5E-C1141A53551A', NULL, 'RBOSTAFFPERMISSIONGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTAFFPERMISSIONGROUP',
      TableFrom = '8E565A79-2F0F-4FBE-BE5E-C1141A53551A',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTAFFPERMISSIONGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3930C4AF-67E1-4E9C-B609-A117D27D78B4'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AADB36FC-49B2-475B-8513-A2F372D2D3EE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AADB36FC-49B2-475B-8513-A2F372D2D3EE', 'A-RBOSTYLEGROUPTRANS', '20180F86-1C46-484F-8C55-56D530662197', NULL, 'RBOSTYLEGROUPTRANS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTYLEGROUPTRANS',
      TableFrom = '20180F86-1C46-484F-8C55-56D530662197',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTYLEGROUPTRANS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AADB36FC-49B2-475B-8513-A2F372D2D3EE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B1C38020-92EF-41FA-ADFB-A346B96B6662')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B1C38020-92EF-41FA-ADFB-A346B96B6662', 'N-RBOSPECIALGROUPITEMS', 'EBDFE573-75A0-46C8-A9B4-AEF284A319EB', NULL, 'RBOSPECIALGROUPITEMS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSPECIALGROUPITEMS',
      TableFrom = 'EBDFE573-75A0-46C8-A9B4-AEF284A319EB',
      StoredProcName = NULL,
      TableNameTo = 'RBOSPECIALGROUPITEMS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B1C38020-92EF-41FA-ADFB-A346B96B6662'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2AF36F7D-8925-4944-9819-A3A8972DEF8A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2AF36F7D-8925-4944-9819-A3A8972DEF8A', 'N-INVENTTRANSREASON', '66C2A2D8-89F8-4933-A3BC-642BD0FC5E00', NULL, 'INVENTTRANSREASON', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-INVENTTRANSREASON',
      TableFrom = '66C2A2D8-89F8-4933-A3BC-642BD0FC5E00',
      StoredProcName = NULL,
      TableNameTo = 'INVENTTRANSREASON',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2AF36F7D-8925-4944-9819-A3A8972DEF8A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '474D30A4-667D-455B-BECE-A4AF1359F61E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('474D30A4-667D-455B-BECE-A4AF1359F61E', 'A-TOURIST', '90FB5F07-04F3-47A3-B79D-B1E2242532DD', NULL, 'TOURIST', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TOURIST',
      TableFrom = '90FB5F07-04F3-47A3-B79D-B1E2242532DD',
      StoredProcName = NULL,
      TableNameTo = 'TOURIST',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '474D30A4-667D-455B-BECE-A4AF1359F61E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '55D0AAE1-5498-474F-863C-A4C31655ADA3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('55D0AAE1-5498-474F-863C-A4C31655ADA3', 'N-RBOINCOMEEXPENSEACCOUNTTABLE', 'E9B12AA1-5E5C-4211-97EF-ABB1209AD54D', NULL, 'RBOINCOMEEXPENSEACCOUNTTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOINCOMEEXPENSEACCOUNTTABLE',
      TableFrom = 'E9B12AA1-5E5C-4211-97EF-ABB1209AD54D',
      StoredProcName = NULL,
      TableNameTo = 'RBOINCOMEEXPENSEACCOUNTTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '55D0AAE1-5498-474F-863C-A4C31655ADA3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '29F31529-9871-4C07-B99F-A509DC37BB91')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('29F31529-9871-4C07-B99F-A509DC37BB91', 'A-RETAILGROUP', '03415632-18FD-4634-A976-6EDAA4959E85', NULL, 'RETAILGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RETAILGROUP',
      TableFrom = '03415632-18FD-4634-A976-6EDAA4959E85',
      StoredProcName = NULL,
      TableNameTo = 'RETAILGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '29F31529-9871-4C07-B99F-A509DC37BB91'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'BFE14915-04A1-4EAF-B79D-A5962F3722CE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('BFE14915-04A1-4EAF-B79D-A5962F3722CE', 'A-POSMMLINEGROUPS', '7E8BC8F3-AAE2-4B43-8C08-0C3109BB2A7C', NULL, 'POSMMLINEGROUPS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSMMLINEGROUPS',
      TableFrom = '7E8BC8F3-AAE2-4B43-8C08-0C3109BB2A7C',
      StoredProcName = NULL,
      TableNameTo = 'POSMMLINEGROUPS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'BFE14915-04A1-4EAF-B79D-A5962F3722CE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'BD3441FD-6F16-4273-B2AF-A5CDD81FC13A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('BD3441FD-6F16-4273-B2AF-A5CDD81FC13A', 'A-RBOCOLORGROUPTRANS', '0520B57F-09E9-4E5F-86CE-CA1749AB5043', NULL, 'RBOCOLORGROUPTRANS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOCOLORGROUPTRANS',
      TableFrom = '0520B57F-09E9-4E5F-86CE-CA1749AB5043',
      StoredProcName = NULL,
      TableNameTo = 'RBOCOLORGROUPTRANS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'BD3441FD-6F16-4273-B2AF-A5CDD81FC13A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B3BF5D41-C6E9-4C1B-8511-A744F1B8F77C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B3BF5D41-C6E9-4C1B-8511-A744F1B8F77C', 'N-RBOINFORMATIONSUBCODETABLE', '0BFFBC04-4BB1-49D8-8DE6-416AB28297A6', NULL, 'RBOINFORMATIONSUBCODETABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOINFORMATIONSUBCODETABLE',
      TableFrom = '0BFFBC04-4BB1-49D8-8DE6-416AB28297A6',
      StoredProcName = NULL,
      TableNameTo = 'RBOINFORMATIONSUBCODETABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B3BF5D41-C6E9-4C1B-8511-A744F1B8F77C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4D42DECD-62E4-4F63-B34A-A8498482118A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4D42DECD-62E4-4F63-B34A-A8498482118A', 'N-RBOLABELTEMPLATES', '3C9A8D68-9F09-4DFA-9381-A365B4038D25', NULL, 'RBOLABELTEMPLATES', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOLABELTEMPLATES',
      TableFrom = '3C9A8D68-9F09-4DFA-9381-A365B4038D25',
      StoredProcName = NULL,
      TableNameTo = 'RBOLABELTEMPLATES',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4D42DECD-62E4-4F63-B34A-A8498482118A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '57F23157-7A02-4489-B9BA-A855981CFEFC')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('57F23157-7A02-4489-B9BA-A855981CFEFC', 'A-CUSTOMERORDERSADDITIONALCONFIG', 'B1983C3F-1C06-4BAB-B6B9-D4E3F0F2E745', NULL, 'CUSTOMERORDERSADDITIONALCONFIG', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CUSTOMERORDERSADDITIONALCONFIG',
      TableFrom = 'B1983C3F-1C06-4BAB-B6B9-D4E3F0F2E745',
      StoredProcName = NULL,
      TableNameTo = 'CUSTOMERORDERSADDITIONALCONFIG',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '57F23157-7A02-4489-B9BA-A855981CFEFC'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6A48F980-C470-4E0F-8618-A912340A99B5')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6A48F980-C470-4E0F-8618-A912340A99B5', 'N-TAXONITEM', 'A327A470-15BC-4D42-9B88-4C9287CF6594', NULL, 'TAXONITEM', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TAXONITEM',
      TableFrom = 'A327A470-15BC-4D42-9B88-4C9287CF6594',
      StoredProcName = NULL,
      TableNameTo = 'TAXONITEM',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6A48F980-C470-4E0F-8618-A912340A99B5'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F22BD8CD-2522-49E5-B9DE-A921D96586CA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F22BD8CD-2522-49E5-B9DE-A921D96586CA', 'N-RBOINFOCODETABLESPECIFIC', '6628A2F4-1387-4EE4-AF51-4BBF4B78F33D', NULL, 'RBOINFOCODETABLESPECIFIC', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOINFOCODETABLESPECIFIC',
      TableFrom = '6628A2F4-1387-4EE4-AF51-4BBF4B78F33D',
      StoredProcName = NULL,
      TableNameTo = 'RBOINFOCODETABLESPECIFIC',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F22BD8CD-2522-49E5-B9DE-A921D96586CA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2AD40B2B-CB8E-449F-98FD-AA8B6B885A98')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2AD40B2B-CB8E-449F-98FD-AA8B6B885A98', 'N-RBOSTORETENDERTYPECARDTABLE', 'C521B76F-F56A-4EE2-BAF9-3F1BEE702617', NULL, 'RBOSTORETENDERTYPECARDTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSTORETENDERTYPECARDTABLE',
      TableFrom = 'C521B76F-F56A-4EE2-BAF9-3F1BEE702617',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTORETENDERTYPECARDTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2AD40B2B-CB8E-449F-98FD-AA8B6B885A98'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '616837DF-247E-4EA5-85B3-AABEAA99127C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('616837DF-247E-4EA5-85B3-AABEAA99127C', 'A-CUSTLOYALTYPARAMETERS', 'FF31504B-75B3-4681-A861-D65E07B380B6', NULL, 'CUSTLOYALTYPARAMETERS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CUSTLOYALTYPARAMETERS',
      TableFrom = 'FF31504B-75B3-4681-A861-D65E07B380B6',
      StoredProcName = NULL,
      TableNameTo = 'CUSTLOYALTYPARAMETERS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '616837DF-247E-4EA5-85B3-AABEAA99127C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5A9A1132-7DF8-4491-8168-AB3587EA4805')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5A9A1132-7DF8-4491-8168-AB3587EA4805', 'A-PERMISSIONGROUP', '07E8A0B1-FE0E-4E46-B291-F110FB9E911C', NULL, 'PERMISSIONGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-PERMISSIONGROUP',
      TableFrom = '07E8A0B1-FE0E-4E46-B291-F110FB9E911C',
      StoredProcName = NULL,
      TableNameTo = 'PERMISSIONGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5A9A1132-7DF8-4491-8168-AB3587EA4805'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '19F900F0-A681-4738-B7F3-ABA127229795')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('19F900F0-A681-4738-B7F3-ABA127229795', 'A-POSTRANSACTIONSERVICEPROFILE', '57787D24-DB50-46DD-9D8B-BFA714E2DC42', NULL, 'POSTRANSACTIONSERVICEPROFILE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSTRANSACTIONSERVICEPROFILE',
      TableFrom = '57787D24-DB50-46DD-9D8B-BFA714E2DC42',
      StoredProcName = NULL,
      TableNameTo = 'POSTRANSACTIONSERVICEPROFILE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '19F900F0-A681-4738-B7F3-ABA127229795'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '77A4FA4A-5CEF-418A-A699-ABA9ADE42780')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('77A4FA4A-5CEF-418A-A699-ABA9ADE42780', 'N-CUSTOMERORDERSETTINGS', '88F30286-DAFF-4B6D-893D-EAC327D6CD22', NULL, 'CUSTOMERORDERSETTINGS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CUSTOMERORDERSETTINGS',
      TableFrom = '88F30286-DAFF-4B6D-893D-EAC327D6CD22',
      StoredProcName = NULL,
      TableNameTo = 'CUSTOMERORDERSETTINGS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '77A4FA4A-5CEF-418A-A699-ABA9ADE42780'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1B24CD91-FF49-484D-9F15-AC26F8849579')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1B24CD91-FF49-484D-9F15-AC26F8849579', 'N-REPORTENUMS', '16751733-246D-40D2-936B-0A95A01FF9CD', NULL, 'REPORTENUMS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-REPORTENUMS',
      TableFrom = '16751733-246D-40D2-936B-0A95A01FF9CD',
      StoredProcName = NULL,
      TableNameTo = 'REPORTENUMS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1B24CD91-FF49-484D-9F15-AC26F8849579'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F5209839-C247-404F-8159-AC3C0877FE66')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F5209839-C247-404F-8159-AC3C0877FE66', 'A-GOODSRECEIVING', 'F7BCAA17-2DB4-4596-B58E-BD6BA919D181', NULL, 'GOODSRECEIVING', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-GOODSRECEIVING',
      TableFrom = 'F7BCAA17-2DB4-4596-B58E-BD6BA919D181',
      StoredProcName = NULL,
      TableNameTo = 'GOODSRECEIVING',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F5209839-C247-404F-8159-AC3C0877FE66'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A86B7DDF-EC9D-430C-9C3D-AC8FCDD1C771')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A86B7DDF-EC9D-430C-9C3D-AC8FCDD1C771', 'N-CUSTLOYALTYPARAMETERS', 'FF31504B-75B3-4681-A861-D65E07B380B6', NULL, 'CUSTLOYALTYPARAMETERS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CUSTLOYALTYPARAMETERS',
      TableFrom = 'FF31504B-75B3-4681-A861-D65E07B380B6',
      StoredProcName = NULL,
      TableNameTo = 'CUSTLOYALTYPARAMETERS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A86B7DDF-EC9D-430C-9C3D-AC8FCDD1C771'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D38D060D-CD96-4FD6-B546-AD6570E5305F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D38D060D-CD96-4FD6-B546-AD6570E5305F', 'A-RBOSPECIALGROUP', 'F94BC205-32CF-44AA-A452-B517FB34D865', NULL, 'RBOSPECIALGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSPECIALGROUP',
      TableFrom = 'F94BC205-32CF-44AA-A452-B517FB34D865',
      StoredProcName = NULL,
      TableNameTo = 'RBOSPECIALGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D38D060D-CD96-4FD6-B546-AD6570E5305F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '8FA2799D-856B-4AD6-8A78-ADA81B66AD46')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('8FA2799D-856B-4AD6-8A78-ADA81B66AD46', 'N-RBOTRANSACTIONORDERINVOICETRANS', 'D8F14F9A-172B-4A41-91C1-CC5B379F3D12', NULL, 'RBOTRANSACTIONORDERINVOICETRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, '2AD7E016-3040-4AAD-96BA-32B13F1507B5', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONORDERINVOICETRANS',
      TableFrom = 'D8F14F9A-172B-4A41-91C1-CC5B379F3D12',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONORDERINVOICETRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '2AD7E016-3040-4AAD-96BA-32B13F1507B5',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '8FA2799D-856B-4AD6-8A78-ADA81B66AD46'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '39A63E83-590E-41DD-A941-ADF5216FDC9F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('39A63E83-590E-41DD-A941-ADF5216FDC9F', 'A-TAXREFUNDTRANSACTION', '1AC02749-3F29-4904-BA4D-AF86EB6EA5D4', NULL, 'TAXREFUNDTRANSACTION', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TAXREFUNDTRANSACTION',
      TableFrom = '1AC02749-3F29-4904-BA4D-AF86EB6EA5D4',
      StoredProcName = NULL,
      TableNameTo = 'TAXREFUNDTRANSACTION',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '39A63E83-590E-41DD-A941-ADF5216FDC9F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5B7E4ABB-0D24-4BD4-82B6-AEF0E0D26AB0')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5B7E4ABB-0D24-4BD4-82B6-AEF0E0D26AB0', 'N-RBOSIZEGROUPTRANS', '6C0B6344-9591-4C2A-A2FE-B22983CEF177', NULL, 'RBOSIZEGROUPTRANS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSIZEGROUPTRANS',
      TableFrom = '6C0B6344-9591-4C2A-A2FE-B22983CEF177',
      StoredProcName = NULL,
      TableNameTo = 'RBOSIZEGROUPTRANS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5B7E4ABB-0D24-4BD4-82B6-AEF0E0D26AB0'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D413D452-6BC8-4ECC-825B-AFBBF289ED6A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D413D452-6BC8-4ECC-825B-AFBBF289ED6A', 'N-JscFieldDesigns', '43EC749C-E2FB-4174-809A-A38048CC8D84', NULL, 'JscFieldDesigns', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscFieldDesigns',
      TableFrom = '43EC749C-E2FB-4174-809A-A38048CC8D84',
      StoredProcName = NULL,
      TableNameTo = 'JscFieldDesigns',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D413D452-6BC8-4ECC-825B-AFBBF289ED6A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'BA8362EB-8CDE-4AAD-92A7-B11D90C24022')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('BA8362EB-8CDE-4AAD-92A7-B11D90C24022', 'A-POSISIMAGES', '53A834AC-629D-4712-AF04-40CA632E3E52', NULL, 'POSISIMAGES', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISIMAGES',
      TableFrom = '53A834AC-629D-4712-AF04-40CA632E3E52',
      StoredProcName = NULL,
      TableNameTo = 'POSISIMAGES',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'BA8362EB-8CDE-4AAD-92A7-B11D90C24022'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D058ACC8-3620-464F-8FB5-B132A330F080')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D058ACC8-3620-464F-8FB5-B132A330F080', 'A-CUSTGROUP', 'F1421057-F2BE-400C-8F6F-24D3930E99B5', NULL, 'CUSTGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CUSTGROUP',
      TableFrom = 'F1421057-F2BE-400C-8F6F-24D3930E99B5',
      StoredProcName = NULL,
      TableNameTo = 'CUSTGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D058ACC8-3620-464F-8FB5-B132A330F080'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F85B8978-66F5-4667-AA4F-B17B1C016605')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F85B8978-66F5-4667-AA4F-B17B1C016605', 'A-INVENTTRANS', '7580D1F1-2D86-4D70-8FED-3CCB4A7778A0', NULL, 'INVENTTRANS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-INVENTTRANS',
      TableFrom = '7580D1F1-2D86-4D70-8FED-3CCB4A7778A0',
      StoredProcName = NULL,
      TableNameTo = 'INVENTTRANS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F85B8978-66F5-4667-AA4F-B17B1C016605'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'C5F35B7A-D11F-4F16-9F87-B1EBA302C690')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('C5F35B7A-D11F-4F16-9F87-B1EBA302C690', 'N-STATIONPRINTINGHOSTS', 'B278F0E7-B96B-405A-9FCA-6D8F98E6A4D1', NULL, 'STATIONPRINTINGHOSTS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-STATIONPRINTINGHOSTS',
      TableFrom = 'B278F0E7-B96B-405A-9FCA-6D8F98E6A4D1',
      StoredProcName = NULL,
      TableNameTo = 'STATIONPRINTINGHOSTS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'C5F35B7A-D11F-4F16-9F87-B1EBA302C690'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'F9512572-59F3-4181-B310-B202FE5B802D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('F9512572-59F3-4181-B310-B202FE5B802D', 'N-RBOINVENTITEMDEPARTMENT', '35AA442C-AE96-4516-A975-0565F17D7B1D', NULL, 'RBOINVENTITEMDEPARTMENT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOINVENTITEMDEPARTMENT',
      TableFrom = '35AA442C-AE96-4516-A975-0565F17D7B1D',
      StoredProcName = NULL,
      TableNameTo = 'RBOINVENTITEMDEPARTMENT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'F9512572-59F3-4181-B310-B202FE5B802D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B40122FE-B903-4469-8D9C-B29E0A70C36A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B40122FE-B903-4469-8D9C-B29E0A70C36A', 'A-INVENTSERIAL', 'C3DFB38F-1DB0-40E3-AA6E-BF0684DD8FB5', NULL, 'INVENTSERIAL', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-INVENTSERIAL',
      TableFrom = 'C3DFB38F-1DB0-40E3-AA6E-BF0684DD8FB5',
      StoredProcName = NULL,
      TableNameTo = 'INVENTSERIAL',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B40122FE-B903-4469-8D9C-B29E0A70C36A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1C6F9C0F-8588-4D29-BD2D-B3B0634FB2E6')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1C6F9C0F-8588-4D29-BD2D-B3B0634FB2E6', 'N-DISCOUNTPARAMETERS', 'CA136130-C9A1-4BD6-8F77-C34DF9DAF720', NULL, 'DISCOUNTPARAMETERS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-DISCOUNTPARAMETERS',
      TableFrom = 'CA136130-C9A1-4BD6-8F77-C34DF9DAF720',
      StoredProcName = NULL,
      TableNameTo = 'DISCOUNTPARAMETERS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1C6F9C0F-8588-4D29-BD2D-B3B0634FB2E6'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3705E4F2-7DFC-4C71-9951-B693B09A5278')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3705E4F2-7DFC-4C71-9951-B693B09A5278', 'N-POSISFORMLAYOUT', '1A66E8EB-BFC5-43C8-BCB1-B6198C2BC6F3', NULL, 'POSISFORMLAYOUT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISFORMLAYOUT',
      TableFrom = '1A66E8EB-BFC5-43C8-BCB1-B6198C2BC6F3',
      StoredProcName = NULL,
      TableNameTo = 'POSISFORMLAYOUT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3705E4F2-7DFC-4C71-9951-B693B09A5278'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'C964F451-5968-44F9-A52D-B6AF24A68FEA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('C964F451-5968-44F9-A52D-B6AF24A68FEA', 'A-POSISTILLLAYOUT', 'E477F2C4-0ABF-4708-A961-3839F9D19775', NULL, 'POSISTILLLAYOUT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISTILLLAYOUT',
      TableFrom = 'E477F2C4-0ABF-4708-A961-3839F9D19775',
      StoredProcName = NULL,
      TableNameTo = 'POSISTILLLAYOUT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'C964F451-5968-44F9-A52D-B6AF24A68FEA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '45238758-42AF-4057-9110-B75CC05DB18D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('45238758-42AF-4057-9110-B75CC05DB18D', 'N-PRICEGROUP', 'E36F44D8-C163-4269-8E00-2D3879BC30F2', NULL, 'PRICEGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-PRICEGROUP',
      TableFrom = 'E36F44D8-C163-4269-8E00-2D3879BC30F2',
      StoredProcName = NULL,
      TableNameTo = 'PRICEGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '45238758-42AF-4057-9110-B75CC05DB18D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'DFC877CF-7492-4C49-BED3-B89BE1E3C981')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('DFC877CF-7492-4C49-BED3-B89BE1E3C981', 'N-INVENTDIMCOMBINATION', '55AE0BF5-DA34-4262-9078-B9062EACEB1B', NULL, 'INVENTDIMCOMBINATION', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-INVENTDIMCOMBINATION',
      TableFrom = '55AE0BF5-DA34-4262-9078-B9062EACEB1B',
      StoredProcName = NULL,
      TableNameTo = 'INVENTDIMCOMBINATION',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'DFC877CF-7492-4C49-BED3-B89BE1E3C981'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AAD4740D-C230-46C4-B592-B9A1C9C4CEC9')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AAD4740D-C230-46C4-B592-B9A1C9C4CEC9', 'N-JscTableDesigns', 'B1FEEB58-DB6A-4B20-9206-2C7831E14AC2', NULL, 'JscTableDesigns', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscTableDesigns',
      TableFrom = 'B1FEEB58-DB6A-4B20-9206-2C7831E14AC2',
      StoredProcName = NULL,
      TableNameTo = 'JscTableDesigns',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AAD4740D-C230-46C4-B592-B9A1C9C4CEC9'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B1A3EB39-9BB4-4DB3-9796-B9DC8EB3B2D3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B1A3EB39-9BB4-4DB3-9796-B9DC8EB3B2D3', 'A-CUSTINGROUP', '764D6199-9561-4A6A-91F1-8D5C133B2B79', NULL, 'CUSTINGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CUSTINGROUP',
      TableFrom = '764D6199-9561-4A6A-91F1-8D5C133B2B79',
      StoredProcName = NULL,
      TableNameTo = 'CUSTINGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B1A3EB39-9BB4-4DB3-9796-B9DC8EB3B2D3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5B6F7875-245F-44E2-BB96-BA39E70D0AC1')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5B6F7875-245F-44E2-BB96-BA39E70D0AC1', 'A-PERIODICDISCOUNTLINE', '8F51974D-8E6A-400A-BFBC-30750B14B4DD', NULL, 'PERIODICDISCOUNTLINE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-PERIODICDISCOUNTLINE',
      TableFrom = '8F51974D-8E6A-400A-BFBC-30750B14B4DD',
      StoredProcName = NULL,
      TableNameTo = 'PERIODICDISCOUNTLINE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5B6F7875-245F-44E2-BB96-BA39E70D0AC1'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '838A5F90-D4E2-4EEF-9600-BB95879D523B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('838A5F90-D4E2-4EEF-9600-BB95879D523B', 'N-RBOTRANSACTIONDININGTABLE', '0DF328A4-54EF-4BF8-8A9F-6772FA3155F9', NULL, 'RBOTRANSACTIONDININGTABLE', 0, 1, 1, 0, NULL, NULL, 0, 0, '0862F4F8-D26D-42AA-9836-E03CE81D94C2', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONDININGTABLE',
      TableFrom = '0DF328A4-54EF-4BF8-8A9F-6772FA3155F9',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONDININGTABLE',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '0862F4F8-D26D-42AA-9836-E03CE81D94C2',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '838A5F90-D4E2-4EEF-9600-BB95879D523B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '00261D28-13D5-4E91-AD7A-BBBC160223AB')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('00261D28-13D5-4E91-AD7A-BBBC160223AB', 'A-RBOLOYALTYMSRCARDTABLE', '522F0A0E-E78F-420A-8899-CDB9320C29FC', NULL, 'RBOLOYALTYMSRCARDTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOLOYALTYMSRCARDTABLE',
      TableFrom = '522F0A0E-E78F-420A-8899-CDB9320C29FC',
      StoredProcName = NULL,
      TableNameTo = 'RBOLOYALTYMSRCARDTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '00261D28-13D5-4E91-AD7A-BBBC160223AB'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '11D60887-BC3B-4835-B380-BC9DF504F731')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('11D60887-BC3B-4835-B380-BC9DF504F731', 'N-POSUSERPROFILE', '4DFD502D-2A40-4759-8766-D7BCBEA9CEBD', NULL, 'POSUSERPROFILE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSUSERPROFILE',
      TableFrom = '4DFD502D-2A40-4759-8766-D7BCBEA9CEBD',
      StoredProcName = NULL,
      TableNameTo = 'POSUSERPROFILE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '11D60887-BC3B-4835-B380-BC9DF504F731'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3052BFF9-E9B1-4553-9ED3-BD22B848D6CF')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3052BFF9-E9B1-4553-9ED3-BD22B848D6CF', 'N-POSSTYLE', '68BDB08D-1F2C-495A-A5CD-42EFDC0E918C', NULL, 'POSSTYLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSSTYLE',
      TableFrom = '68BDB08D-1F2C-495A-A5CD-42EFDC0E918C',
      StoredProcName = NULL,
      TableNameTo = 'POSSTYLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3052BFF9-E9B1-4553-9ED3-BD22B848D6CF'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '33025348-2997-4B13-B395-BD76F6A28683')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('33025348-2997-4B13-B395-BD76F6A28683', 'N-UNITCONVERT', 'D50077DB-AD6B-4BD4-ACD1-72060FFC2FE4', NULL, 'UNITCONVERT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-UNITCONVERT',
      TableFrom = 'D50077DB-AD6B-4BD4-ACD1-72060FFC2FE4',
      StoredProcName = NULL,
      TableNameTo = 'UNITCONVERT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '33025348-2997-4B13-B395-BD76F6A28683'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4B918291-E28D-43D9-9FF3-BDABA370580B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4B918291-E28D-43D9-9FF3-BDABA370580B', 'A-RBOSTORECASHDECLARATIONTABLE', 'C8272D2C-C978-4109-87F0-382BE63962FA', NULL, 'RBOSTORECASHDECLARATIONTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTORECASHDECLARATIONTABLE',
      TableFrom = 'C8272D2C-C978-4109-87F0-382BE63962FA',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTORECASHDECLARATIONTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4B918291-E28D-43D9-9FF3-BDABA370580B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D6DA05D4-3D39-48A6-BC9B-C0059254A368')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D6DA05D4-3D39-48A6-BC9B-C0059254A368', 'A-RBOLOCATIONPRICEGROUP', '21B9F3F4-2451-4C68-9D8E-AB90305BE6CE', NULL, 'RBOLOCATIONPRICEGROUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOLOCATIONPRICEGROUP',
      TableFrom = '21B9F3F4-2451-4C68-9D8E-AB90305BE6CE',
      StoredProcName = NULL,
      TableNameTo = 'RBOLOCATIONPRICEGROUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D6DA05D4-3D39-48A6-BC9B-C0059254A368'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'DA430C31-C8D2-40FE-A190-C01D0C21EC03')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('DA430C31-C8D2-40FE-A190-C01D0C21EC03', 'A-POSISPERMISSIONS', 'E64488D7-FB03-4254-9E0F-5FCF8949C300', NULL, 'POSISPERMISSIONS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISPERMISSIONS',
      TableFrom = 'E64488D7-FB03-4254-9E0F-5FCF8949C300',
      StoredProcName = NULL,
      TableNameTo = 'POSISPERMISSIONS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'DA430C31-C8D2-40FE-A190-C01D0C21EC03'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E0501BB3-5C2B-450F-82C2-C20560A9ABFE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E0501BB3-5C2B-450F-82C2-C20560A9ABFE', 'N-CUSTINGROUP', '764D6199-9561-4A6A-91F1-8D5C133B2B79', NULL, 'CUSTINGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CUSTINGROUP',
      TableFrom = '764D6199-9561-4A6A-91F1-8D5C133B2B79',
      StoredProcName = NULL,
      TableNameTo = 'CUSTINGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E0501BB3-5C2B-450F-82C2-C20560A9ABFE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A17D1B6B-D898-4E4B-8917-C2FA2112C15C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A17D1B6B-D898-4E4B-8917-C2FA2112C15C', 'A-RBOEFTMAPPING', 'E8BDB0D6-2CF9-4C2C-84E4-ECA5CD0F8CB1', NULL, 'RBOEFTMAPPING', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOEFTMAPPING',
      TableFrom = 'E8BDB0D6-2CF9-4C2C-84E4-ECA5CD0F8CB1',
      StoredProcName = NULL,
      TableNameTo = 'RBOEFTMAPPING',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A17D1B6B-D898-4E4B-8917-C2FA2112C15C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '32EC11D6-F7FE-4874-BD8C-C348E019BA9F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('32EC11D6-F7FE-4874-BD8C-C348E019BA9F', 'N-RBOINFOCODETABLE', '505121A5-98DE-42B2-B1E5-E542F9CD7C2E', NULL, 'RBOINFOCODETABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOINFOCODETABLE',
      TableFrom = '505121A5-98DE-42B2-B1E5-E542F9CD7C2E',
      StoredProcName = NULL,
      TableNameTo = 'RBOINFOCODETABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '32EC11D6-F7FE-4874-BD8C-C348E019BA9F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AAB68B04-7274-4FD8-85FA-C4D10E0FBC28')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AAB68B04-7274-4FD8-85FA-C4D10E0FBC28', 'N-RETAILITEM', '1631024E-D4BB-4F4A-8FFC-830636E3AB9B', NULL, 'RETAILITEM', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RETAILITEM',
      TableFrom = '1631024E-D4BB-4F4A-8FFC-830636E3AB9B',
      StoredProcName = NULL,
      TableNameTo = 'RETAILITEM',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AAB68B04-7274-4FD8-85FA-C4D10E0FBC28'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '77D93CD2-A343-4009-8185-C4DF4485D9E8')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('77D93CD2-A343-4009-8185-C4DF4485D9E8', 'A-RBOSTORETENDERTYPECARDTABLE', 'C521B76F-F56A-4EE2-BAF9-3F1BEE702617', NULL, 'RBOSTORETENDERTYPECARDTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTORETENDERTYPECARDTABLE',
      TableFrom = 'C521B76F-F56A-4EE2-BAF9-3F1BEE702617',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTORETENDERTYPECARDTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '77D93CD2-A343-4009-8185-C4DF4485D9E8'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1BC2F37D-BBE4-4D46-9710-C56BC5BE6D6F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1BC2F37D-BBE4-4D46-9710-C56BC5BE6D6F', 'N-REMOTEHOSTS', 'CB4A25D0-BC36-4F82-9A53-33D1AE962C2A', NULL, 'REMOTEHOSTS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-REMOTEHOSTS',
      TableFrom = 'CB4A25D0-BC36-4F82-9A53-33D1AE962C2A',
      StoredProcName = NULL,
      TableNameTo = 'REMOTEHOSTS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1BC2F37D-BBE4-4D46-9710-C56BC5BE6D6F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '710838F0-489E-4940-90C6-C579BAB594DB')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('710838F0-489E-4940-90C6-C579BAB594DB', 'N-RBOTRANSACTIONSALESTRANS', '095F0A72-60C8-4B56-84F4-E40DDE021C84', NULL, 'RBOTRANSACTIONSALESTRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, '2CC137CD-2BE0-43E8-BEB0-388D4AE72E45', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONSALESTRANS',
      TableFrom = '095F0A72-60C8-4B56-84F4-E40DDE021C84',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONSALESTRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '2CC137CD-2BE0-43E8-BEB0-388D4AE72E45',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '710838F0-489E-4940-90C6-C579BAB594DB'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'BCFF43F3-F72E-4D9A-B84E-C70E82381417')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('BCFF43F3-F72E-4D9A-B84E-C70E82381417', 'A-HOSPITALITYSETUP', 'D1C50885-5F8B-4807-B533-6C18D2F1E0F7', NULL, 'HOSPITALITYSETUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-HOSPITALITYSETUP',
      TableFrom = 'D1C50885-5F8B-4807-B533-6C18D2F1E0F7',
      StoredProcName = NULL,
      TableNameTo = 'HOSPITALITYSETUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'BCFF43F3-F72E-4D9A-B84E-C70E82381417'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AE874DD6-DDFC-4810-A63B-C752136852A5')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AE874DD6-DDFC-4810-A63B-C752136852A5', 'N-JscDriverTypes', '7F146D1F-BA58-4047-BC34-031AE790DD3D', NULL, 'JscDriverTypes', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscDriverTypes',
      TableFrom = '7F146D1F-BA58-4047-BC34-031AE790DD3D',
      StoredProcName = NULL,
      TableNameTo = 'JscDriverTypes',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AE874DD6-DDFC-4810-A63B-C752136852A5'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'AE100771-F5AD-40CD-A176-C8C95FC49A78')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('AE100771-F5AD-40CD-A176-C8C95FC49A78', 'N-CUSTOMER', '41896D98-1EFB-490A-92AD-E3063144EBF0', NULL, 'CUSTOMER', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CUSTOMER',
      TableFrom = '41896D98-1EFB-490A-92AD-E3063144EBF0',
      StoredProcName = NULL,
      TableNameTo = 'CUSTOMER',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'AE100771-F5AD-40CD-A176-C8C95FC49A78'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2397E775-D4B5-4DC4-9511-C8EC5DCD3A4B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2397E775-D4B5-4DC4-9511-C8EC5DCD3A4B', 'N-RBOSTORECASHDECLARATIONTABLE', 'C8272D2C-C978-4109-87F0-382BE63962FA', NULL, 'RBOSTORECASHDECLARATIONTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSTORECASHDECLARATIONTABLE',
      TableFrom = 'C8272D2C-C978-4109-87F0-382BE63962FA',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTORECASHDECLARATIONTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2397E775-D4B5-4DC4-9511-C8EC5DCD3A4B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1D99C26F-81AC-4140-8DAE-C9192AB40BDC')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1D99C26F-81AC-4140-8DAE-C9192AB40BDC', 'A-PURCHASEORDERLINE', '5573D9E7-556D-49E6-AB3E-13B2A6C56EAC', NULL, 'PURCHASEORDERLINE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-PURCHASEORDERLINE',
      TableFrom = '5573D9E7-556D-49E6-AB3E-13B2A6C56EAC',
      StoredProcName = NULL,
      TableNameTo = 'PURCHASEORDERLINE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1D99C26F-81AC-4140-8DAE-C9192AB40BDC'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D36F222A-DCAA-46DB-BBC9-C9723B63CD42')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D36F222A-DCAA-46DB-BBC9-C9723B63CD42', 'N-RBOUNIT', '04DD7908-1003-4F5E-AFA9-50FC1F99D21C', NULL, 'RBOUNIT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOUNIT',
      TableFrom = '04DD7908-1003-4F5E-AFA9-50FC1F99D21C',
      StoredProcName = NULL,
      TableNameTo = 'RBOUNIT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D36F222A-DCAA-46DB-BBC9-C9723B63CD42'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '65A2976E-42B7-428C-8B80-C997F071C7F4')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('65A2976E-42B7-428C-8B80-C997F071C7F4', 'N-STATIONSELECTION', '53FA83EB-8D93-4542-8AB6-CEC8ECC9238C', NULL, 'STATIONSELECTION', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-STATIONSELECTION',
      TableFrom = '53FA83EB-8D93-4542-8AB6-CEC8ECC9238C',
      StoredProcName = NULL,
      TableNameTo = 'STATIONSELECTION',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '65A2976E-42B7-428C-8B80-C997F071C7F4'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '423455C3-2E4E-4AE3-A520-C9985CA2A3CC')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('423455C3-2E4E-4AE3-A520-C9985CA2A3CC', 'A-POSFORMPROFILE', '8ADBD0C8-D29C-47B9-BACE-2340E803D917', NULL, 'POSFORMPROFILE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSFORMPROFILE',
      TableFrom = '8ADBD0C8-D29C-47B9-BACE-2340E803D917',
      StoredProcName = NULL,
      TableNameTo = 'POSFORMPROFILE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '423455C3-2E4E-4AE3-A520-C9985CA2A3CC'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'FEAB4ECB-3B0F-4B6F-9496-CA7270DB0841')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('FEAB4ECB-3B0F-4B6F-9496-CA7270DB0841', 'N-RBOTRANSACTIONBANKEDTENDE20338', '816964CC-F153-4F95-A488-DFD31522C090', NULL, 'RBOTRANSACTIONBANKEDTENDE20338', 0, 1, 1, 0, NULL, NULL, 0, 0, '149B1379-76D7-4142-A774-755D26DAF091', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONBANKEDTENDE20338',
      TableFrom = '816964CC-F153-4F95-A488-DFD31522C090',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONBANKEDTENDE20338',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '149B1379-76D7-4142-A774-755D26DAF091',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'FEAB4ECB-3B0F-4B6F-9496-CA7270DB0841'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2B0BEAA3-1DDA-48C9-BD1F-CB40A71B0D55')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2B0BEAA3-1DDA-48C9-BD1F-CB40A71B0D55', 'CleanActions', 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 'REPLICATIONACTIONS', 0, 3, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'CleanActions',
      TableFrom = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      StoredProcName = NULL,
      TableNameTo = 'REPLICATIONACTIONS',
      ReplicationMethod = 0,
      WhatToDo = 3,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2B0BEAA3-1DDA-48C9-BD1F-CB40A71B0D55'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '998D50E9-5E43-4499-A404-CC01679096B7')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('998D50E9-5E43-4499-A404-CC01679096B7', 'A-RBOLABELTEMPLATES', '3C9A8D68-9F09-4DFA-9381-A365B4038D25', NULL, 'RBOLABELTEMPLATES', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOLABELTEMPLATES',
      TableFrom = '3C9A8D68-9F09-4DFA-9381-A365B4038D25',
      StoredProcName = NULL,
      TableNameTo = 'RBOLABELTEMPLATES',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '998D50E9-5E43-4499-A404-CC01679096B7'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '63457772-D8BC-4D33-A190-CC69D8610E43')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('63457772-D8BC-4D33-A190-CC69D8610E43', 'N-POSISLICENSE', '3E9FB0BC-2A6B-4B32-ACBA-C404288ABA13', NULL, 'POSISLICENSE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISLICENSE',
      TableFrom = '3E9FB0BC-2A6B-4B32-ACBA-C404288ABA13',
      StoredProcName = NULL,
      TableNameTo = 'POSISLICENSE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '63457772-D8BC-4D33-A190-CC69D8610E43'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '98BB57DD-B484-4A9C-A143-CCA525B45DE2')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('98BB57DD-B484-4A9C-A143-CCA525B45DE2', 'N-RETAILDIVISION', '5F26F8B9-278E-4D04-85A2-88855536CFB1', NULL, 'RETAILDIVISION', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RETAILDIVISION',
      TableFrom = '5F26F8B9-278E-4D04-85A2-88855536CFB1',
      StoredProcName = NULL,
      TableNameTo = 'RETAILDIVISION',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '98BB57DD-B484-4A9C-A143-CCA525B45DE2'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6FA26856-3CCA-441B-A686-CDEF335598CA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6FA26856-3CCA-441B-A686-CDEF335598CA', 'N-JscLocations', '3F2F2938-488D-42C6-81B3-CC9FF837018F', NULL, 'JscLocations', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-JscLocations',
      TableFrom = '3F2F2938-488D-42C6-81B3-CC9FF837018F',
      StoredProcName = NULL,
      TableNameTo = 'JscLocations',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6FA26856-3CCA-441B-A686-CDEF335598CA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D445BFCC-BB9B-4D04-9BDC-CE056555DE53')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D445BFCC-BB9B-4D04-9BDC-CE056555DE53', 'A-RBOSTATEMENTTABLE', '217F6930-3AA1-4B05-A861-857214554168', NULL, 'RBOSTATEMENTTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSTATEMENTTABLE',
      TableFrom = '217F6930-3AA1-4B05-A861-857214554168',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTATEMENTTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D445BFCC-BB9B-4D04-9BDC-CE056555DE53'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'C1AE3B73-AB56-4EF5-9712-CF0D6109AC7D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('C1AE3B73-AB56-4EF5-9712-CF0D6109AC7D', 'N-SALESPARAMETERS', '2D9ED6E2-070B-4E7C-9C72-F9FE158D4862', NULL, 'SALESPARAMETERS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-SALESPARAMETERS',
      TableFrom = '2D9ED6E2-070B-4E7C-9C72-F9FE158D4862',
      StoredProcName = NULL,
      TableNameTo = 'SALESPARAMETERS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'C1AE3B73-AB56-4EF5-9712-CF0D6109AC7D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'EC389218-5A5F-421B-96FC-CF7BCB099BFB')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('EC389218-5A5F-421B-96FC-CF7BCB099BFB', 'A-TAXFREECONFIG', '56E9B0B4-C118-40AC-A491-46D5C5A19C40', NULL, 'TAXFREECONFIG', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TAXFREECONFIG',
      TableFrom = '56E9B0B4-C118-40AC-A491-46D5C5A19C40',
      StoredProcName = NULL,
      TableNameTo = 'TAXFREECONFIG',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'EC389218-5A5F-421B-96FC-CF7BCB099BFB'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '961B9A87-64AA-4E78-81D4-D03996E00C87')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('961B9A87-64AA-4E78-81D4-D03996E00C87', 'N-RBOLOCATIONPRICEGROUP', '21B9F3F4-2451-4C68-9D8E-AB90305BE6CE', NULL, 'RBOLOCATIONPRICEGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOLOCATIONPRICEGROUP',
      TableFrom = '21B9F3F4-2451-4C68-9D8E-AB90305BE6CE',
      StoredProcName = NULL,
      TableNameTo = 'RBOLOCATIONPRICEGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '961B9A87-64AA-4E78-81D4-D03996E00C87'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '9940E5D8-3F0F-4EC0-8F22-D0AECF58B1C8')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('9940E5D8-3F0F-4EC0-8F22-D0AECF58B1C8', 'A-CURRENCY', '95F91322-7091-4653-AEB2-5DD09E9425EF', NULL, 'CURRENCY', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CURRENCY',
      TableFrom = '95F91322-7091-4653-AEB2-5DD09E9425EF',
      StoredProcName = NULL,
      TableNameTo = 'CURRENCY',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '9940E5D8-3F0F-4EC0-8F22-D0AECF58B1C8'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '11D92F31-9E02-4944-B2D2-D11407C2B988')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('11D92F31-9E02-4944-B2D2-D11407C2B988', 'N-CUSTTABLE', 'FE8F1B25-11F8-48A2-96E8-AF65A2B8BC5C', NULL, 'CUSTTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CUSTTABLE',
      TableFrom = 'FE8F1B25-11F8-48A2-96E8-AF65A2B8BC5C',
      StoredProcName = NULL,
      TableNameTo = 'CUSTTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '11D92F31-9E02-4944-B2D2-D11407C2B988'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B2C627D4-C1A8-4195-B4DE-D180DBF7661E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B2C627D4-C1A8-4195-B4DE-D180DBF7661E', 'N-RBOEFTMAPPING', 'E8BDB0D6-2CF9-4C2C-84E4-ECA5CD0F8CB1', NULL, 'RBOEFTMAPPING', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOEFTMAPPING',
      TableFrom = 'E8BDB0D6-2CF9-4C2C-84E4-ECA5CD0F8CB1',
      StoredProcName = NULL,
      TableNameTo = 'RBOEFTMAPPING',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B2C627D4-C1A8-4195-B4DE-D180DBF7661E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '7269023A-3371-4152-8669-D26B9F869244')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('7269023A-3371-4152-8669-D26B9F869244', 'N-RETAILGROUP', '03415632-18FD-4634-A976-6EDAA4959E85', NULL, 'RETAILGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RETAILGROUP',
      TableFrom = '03415632-18FD-4634-A976-6EDAA4959E85',
      StoredProcName = NULL,
      TableNameTo = 'RETAILGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '7269023A-3371-4152-8669-D26B9F869244'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'DA401256-863A-4C1F-8774-D2AC5ECBE895')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('DA401256-863A-4C1F-8774-D2AC5ECBE895', 'A-REPORTCONTEXTS', '8AB2CDB4-6674-4BA9-A221-D606BD56CD65', NULL, 'REPORTCONTEXTS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-REPORTCONTEXTS',
      TableFrom = '8AB2CDB4-6674-4BA9-A221-D606BD56CD65',
      StoredProcName = NULL,
      TableNameTo = 'REPORTCONTEXTS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'DA401256-863A-4C1F-8774-D2AC5ECBE895'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B20C7783-F814-4D18-B8BA-D4ACC00857C3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B20C7783-F814-4D18-B8BA-D4ACC00857C3', 'A-RESTAURANTSTATION', 'F3BE753E-D7CD-40C4-A1E5-34C1FDFA8D54', NULL, 'RESTAURANTSTATION', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RESTAURANTSTATION',
      TableFrom = 'F3BE753E-D7CD-40C4-A1E5-34C1FDFA8D54',
      StoredProcName = NULL,
      TableNameTo = 'RESTAURANTSTATION',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B20C7783-F814-4D18-B8BA-D4ACC00857C3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1E2D6675-E739-4127-8E7D-D5222CBD770B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1E2D6675-E739-4127-8E7D-D5222CBD770B', 'A-SPECIALGROUPITEMS', 'AA5CD90D-7B42-4F1A-B921-6E8C4561276E', NULL, 'SPECIALGROUPITEMS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-SPECIALGROUPITEMS',
      TableFrom = 'AA5CD90D-7B42-4F1A-B921-6E8C4561276E',
      StoredProcName = NULL,
      TableNameTo = 'SPECIALGROUPITEMS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1E2D6675-E739-4127-8E7D-D5222CBD770B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '05A6BCE3-7193-4AD2-ACDC-D53685D8423D')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('05A6BCE3-7193-4AD2-ACDC-D53685D8423D', 'A-INVENTITEMBARCODE', '320954D5-5436-49AE-AE36-B44FE59CA099', NULL, 'INVENTITEMBARCODE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-INVENTITEMBARCODE',
      TableFrom = '320954D5-5436-49AE-AE36-B44FE59CA099',
      StoredProcName = NULL,
      TableNameTo = 'INVENTITEMBARCODE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '05A6BCE3-7193-4AD2-ACDC-D53685D8423D'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '8A5C92C5-E4B9-42ED-BEAC-D57545EAD58E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('8A5C92C5-E4B9-42ED-BEAC-D57545EAD58E', 'N-SALESTYPE', '42EDA5FC-7350-4DE2-9D08-E4D670F668C8', NULL, 'SALESTYPE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-SALESTYPE',
      TableFrom = '42EDA5FC-7350-4DE2-9D08-E4D670F668C8',
      StoredProcName = NULL,
      TableNameTo = 'SALESTYPE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '8A5C92C5-E4B9-42ED-BEAC-D57545EAD58E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6F7153A2-85F0-48F9-BE1B-D58A1262868B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6F7153A2-85F0-48F9-BE1B-D58A1262868B', 'A-STATIONSELECTION', '53FA83EB-8D93-4542-8AB6-CEC8ECC9238C', NULL, 'STATIONSELECTION', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-STATIONSELECTION',
      TableFrom = '53FA83EB-8D93-4542-8AB6-CEC8ECC9238C',
      StoredProcName = NULL,
      TableNameTo = 'STATIONSELECTION',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6F7153A2-85F0-48F9-BE1B-D58A1262868B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6F1C0CBE-80FB-46DA-A115-D710A1372B74')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6F1C0CBE-80FB-46DA-A115-D710A1372B74', 'N-POSISPARAMETERS', '1A551459-6318-49CF-9BF8-BC01761031A5', NULL, 'POSISPARAMETERS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISPARAMETERS',
      TableFrom = '1A551459-6318-49CF-9BF8-BC01761031A5',
      StoredProcName = NULL,
      TableNameTo = 'POSISPARAMETERS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6F1C0CBE-80FB-46DA-A115-D710A1372B74'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'D9C3E374-DB01-4EAA-B34A-D85CFE68CE67')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('D9C3E374-DB01-4EAA-B34A-D85CFE68CE67', 'N-USERPERMISSION', 'FA5C7F0E-D9B7-4817-BCB4-53CFDED0A9B8', NULL, 'USERPERMISSION', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-USERPERMISSION',
      TableFrom = 'FA5C7F0E-D9B7-4817-BCB4-53CFDED0A9B8',
      StoredProcName = NULL,
      TableNameTo = 'USERPERMISSION',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'D9C3E374-DB01-4EAA-B34A-D85CFE68CE67'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1501549C-ECB3-495A-BD0E-D8957BDA4315')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1501549C-ECB3-495A-BD0E-D8957BDA4315', 'N-POSISSUSPENSIONTYPE', '1BD28BC0-3F41-47F8-BFD0-48FBC96551AF', NULL, 'POSISSUSPENSIONTYPE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISSUSPENSIONTYPE',
      TableFrom = '1BD28BC0-3F41-47F8-BFD0-48FBC96551AF',
      StoredProcName = NULL,
      TableNameTo = 'POSISSUSPENSIONTYPE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1501549C-ECB3-495A-BD0E-D8957BDA4315'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '99112887-9936-4908-BF7A-D988278B6EB4')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('99112887-9936-4908-BF7A-D988278B6EB4', 'A-PRICEPARAMETERS', '1A9BE9CE-3053-486D-A74B-6597FFE8A6C5', NULL, 'PRICEPARAMETERS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-PRICEPARAMETERS',
      TableFrom = '1A9BE9CE-3053-486D-A74B-6597FFE8A6C5',
      StoredProcName = NULL,
      TableNameTo = 'PRICEPARAMETERS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '99112887-9936-4908-BF7A-D988278B6EB4'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '05F28621-9A39-46CE-98D9-DA65F3B38E4B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('05F28621-9A39-46CE-98D9-DA65F3B38E4B', 'N-POSISSUSPENSIONADDINFO', 'EBD44150-2572-416F-83CE-B108F6435684', NULL, 'POSISSUSPENSIONADDINFO', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISSUSPENSIONADDINFO',
      TableFrom = 'EBD44150-2572-416F-83CE-B108F6435684',
      StoredProcName = NULL,
      TableNameTo = 'POSISSUSPENSIONADDINFO',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '05F28621-9A39-46CE-98D9-DA65F3B38E4B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '9F5C1F3B-C650-4AFE-B81D-DB66E708BF85')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('9F5C1F3B-C650-4AFE-B81D-DB66E708BF85', 'A-RBOSIZEGROUPTRANS', '6C0B6344-9591-4C2A-A2FE-B22983CEF177', NULL, 'RBOSIZEGROUPTRANS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSIZEGROUPTRANS',
      TableFrom = '6C0B6344-9591-4C2A-A2FE-B22983CEF177',
      StoredProcName = NULL,
      TableNameTo = 'RBOSIZEGROUPTRANS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '9F5C1F3B-C650-4AFE-B81D-DB66E708BF85'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '02315286-65F8-489D-8AAB-DBAD48481414')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('02315286-65F8-489D-8AAB-DBAD48481414', 'N-CUSTGROUP', 'F1421057-F2BE-400C-8F6F-24D3930E99B5', NULL, 'CUSTGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CUSTGROUP',
      TableFrom = 'F1421057-F2BE-400C-8F6F-24D3930E99B5',
      StoredProcName = NULL,
      TableNameTo = 'CUSTGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '02315286-65F8-489D-8AAB-DBAD48481414'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6C720627-9D81-4B10-8DA2-DC39D19AB510')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6C720627-9D81-4B10-8DA2-DC39D19AB510', 'N-INVENTITEMBARCODE', '320954D5-5436-49AE-AE36-B44FE59CA099', NULL, 'INVENTITEMBARCODE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-INVENTITEMBARCODE',
      TableFrom = '320954D5-5436-49AE-AE36-B44FE59CA099',
      StoredProcName = NULL,
      TableNameTo = 'INVENTITEMBARCODE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6C720627-9D81-4B10-8DA2-DC39D19AB510'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'EE50A504-DB0E-4FC2-854E-DC617825CB18')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('EE50A504-DB0E-4FC2-854E-DC617825CB18', 'N-UNIT', 'B8DF2290-C1D3-4EC4-ABAF-F2C508F43268', NULL, 'UNIT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-UNIT',
      TableFrom = 'B8DF2290-C1D3-4EC4-ABAF-F2C508F43268',
      StoredProcName = NULL,
      TableNameTo = 'UNIT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'EE50A504-DB0E-4FC2-854E-DC617825CB18'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'BCAC9444-ACF9-4550-8FEA-DCAA9FB53A67')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('BCAC9444-ACF9-4550-8FEA-DCAA9FB53A67', 'N-RBOSIZES', 'FB8E335C-9494-4910-BA6F-8DD904D6535A', NULL, 'RBOSIZES', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSIZES',
      TableFrom = 'FB8E335C-9494-4910-BA6F-8DD904D6535A',
      StoredProcName = NULL,
      TableNameTo = 'RBOSIZES',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'BCAC9444-ACF9-4550-8FEA-DCAA9FB53A67'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '0DA5D643-2E52-4CB2-A8A3-DD9289B36232')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('0DA5D643-2E52-4CB2-A8A3-DD9289B36232', 'A-RBOTENDERTYPETABLE', '3B5A324D-190E-449D-A8CE-69A714B019E1', NULL, 'RBOTENDERTYPETABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOTENDERTYPETABLE',
      TableFrom = '3B5A324D-190E-449D-A8CE-69A714B019E1',
      StoredProcName = NULL,
      TableNameTo = 'RBOTENDERTYPETABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '0DA5D643-2E52-4CB2-A8A3-DD9289B36232'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '84207527-B930-42B3-8E59-DF4CA667BA0A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('84207527-B930-42B3-8E59-DF4CA667BA0A', 'N-RESTAURANTDININGTABLEDESIGN', 'ED23AF38-3687-4740-A322-37ABBFDA4981', NULL, 'RESTAURANTDININGTABLEDESIGN', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RESTAURANTDININGTABLEDESIGN',
      TableFrom = 'ED23AF38-3687-4740-A322-37ABBFDA4981',
      StoredProcName = NULL,
      TableNameTo = 'RESTAURANTDININGTABLEDESIGN',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '84207527-B930-42B3-8E59-DF4CA667BA0A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6B59D203-0D03-4CBD-83EA-DFEFF77320BE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6B59D203-0D03-4CBD-83EA-DFEFF77320BE', 'A-POSISOPERATIONS', '11AE76BE-27F8-4916-BC99-D3A9DDED5283', NULL, 'POSISOPERATIONS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISOPERATIONS',
      TableFrom = '11AE76BE-27F8-4916-BC99-D3A9DDED5283',
      StoredProcName = NULL,
      TableNameTo = 'POSISOPERATIONS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6B59D203-0D03-4CBD-83EA-DFEFF77320BE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B7AB23D9-797D-4472-9AF1-DFF6ADD6A8BA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B7AB23D9-797D-4472-9AF1-DFF6ADD6A8BA', 'N-CUSTOMERADDRESSTYPE', 'AB4F3B96-1688-4D31-A19B-5F95896C3E80', NULL, 'CUSTOMERADDRESSTYPE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CUSTOMERADDRESSTYPE',
      TableFrom = 'AB4F3B96-1688-4D31-A19B-5F95896C3E80',
      StoredProcName = NULL,
      TableNameTo = 'CUSTOMERADDRESSTYPE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B7AB23D9-797D-4472-9AF1-DFF6ADD6A8BA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '42CF3A90-4178-4528-BA23-E01A03C76C4B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('42CF3A90-4178-4528-BA23-E01A03C76C4B', 'N-POSISRFIDTABLE', 'AB7CC7E2-1BFE-46E4-858A-3AA47B6588F6', NULL, 'POSISRFIDTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISRFIDTABLE',
      TableFrom = 'AB7CC7E2-1BFE-46E4-858A-3AA47B6588F6',
      StoredProcName = NULL,
      TableNameTo = 'POSISRFIDTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '42CF3A90-4178-4528-BA23-E01A03C76C4B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '490A9267-0678-4AB3-85F2-E03E6636AD7C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('490A9267-0678-4AB3-85F2-E03E6636AD7C', 'A-CUSTGROUPCATEGO  RY', 'D3598CA9-26F0-47C6-BADA-381C66457131', NULL, 'CUSTGROUPCATEGORY', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CUSTGROUPCATEGO  RY',
      TableFrom = 'D3598CA9-26F0-47C6-BADA-381C66457131',
      StoredProcName = NULL,
      TableNameTo = 'CUSTGROUPCATEGORY',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '490A9267-0678-4AB3-85F2-E03E6636AD7C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '427ADC84-6BD9-49FF-A1AF-E0DCCEAAB268')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('427ADC84-6BD9-49FF-A1AF-E0DCCEAAB268', 'N-POSISPERMISSIONS', 'E64488D7-FB03-4254-9E0F-5FCF8949C300', NULL, 'POSISPERMISSIONS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISPERMISSIONS',
      TableFrom = 'E64488D7-FB03-4254-9E0F-5FCF8949C300',
      StoredProcName = NULL,
      TableNameTo = 'POSISPERMISSIONS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '427ADC84-6BD9-49FF-A1AF-E0DCCEAAB268'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E356AA31-A8B7-49F0-B747-E1570A8833BD')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E356AA31-A8B7-49F0-B747-E1570A8833BD', 'A-REPORTENUMS', '16751733-246D-40D2-936B-0A95A01FF9CD', NULL, 'REPORTENUMS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-REPORTENUMS',
      TableFrom = '16751733-246D-40D2-936B-0A95A01FF9CD',
      StoredProcName = NULL,
      TableNameTo = 'REPORTENUMS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E356AA31-A8B7-49F0-B747-E1570A8833BD'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E6F42617-8821-4412-9B5B-E1BD45F9BBC9')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E6F42617-8821-4412-9B5B-E1BD45F9BBC9', 'N-POSISBLANKOPERATIONS', '834C2D13-62FB-48CB-868D-727F413C0079', NULL, 'POSISBLANKOPERATIONS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISBLANKOPERATIONS',
      TableFrom = '834C2D13-62FB-48CB-868D-727F413C0079',
      StoredProcName = NULL,
      TableNameTo = 'POSISBLANKOPERATIONS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E6F42617-8821-4412-9B5B-E1BD45F9BBC9'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3231B64B-93AF-44B6-B42E-E242E2300859')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3231B64B-93AF-44B6-B42E-E242E2300859', 'N-POSFORMTYPE', '972D4141-2A8A-448C-B3A3-DCABD10DF657', NULL, 'POSFORMTYPE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSFORMTYPE',
      TableFrom = '972D4141-2A8A-448C-B3A3-DCABD10DF657',
      StoredProcName = NULL,
      TableNameTo = 'POSFORMTYPE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3231B64B-93AF-44B6-B42E-E242E2300859'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'ED7D1BDD-C3F3-4C9D-8A26-E244486D66C4')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('ED7D1BDD-C3F3-4C9D-8A26-E244486D66C4', 'N-TAXDATA', '172F00E5-B542-4B23-A3FF-65D3473F5A25', NULL, 'TAXDATA', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TAXDATA',
      TableFrom = '172F00E5-B542-4B23-A3FF-65D3473F5A25',
      StoredProcName = NULL,
      TableNameTo = 'TAXDATA',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'ED7D1BDD-C3F3-4C9D-8A26-E244486D66C4'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '30F89A2E-5CB6-4579-B599-E251FDC4DADF')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('30F89A2E-5CB6-4579-B599-E251FDC4DADF', 'A-POSISHOSPITALITYDININGTABLES', '616B8634-7EA5-44F6-945A-A8B4EE02E969', NULL, 'POSISHOSPITALITYDININGTABLES', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISHOSPITALITYDININGTABLES',
      TableFrom = '616B8634-7EA5-44F6-945A-A8B4EE02E969',
      StoredProcName = NULL,
      TableNameTo = 'POSISHOSPITALITYDININGTABLES',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '30F89A2E-5CB6-4579-B599-E251FDC4DADF'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '13274EA3-1B24-45D9-9830-E364EE19FB18')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('13274EA3-1B24-45D9-9830-E364EE19FB18', 'N-USERGROUPS', 'E6EB8914-00A4-4C14-BECF-1501288AC5B9', NULL, 'USERGROUPS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-USERGROUPS',
      TableFrom = 'E6EB8914-00A4-4C14-BECF-1501288AC5B9',
      StoredProcName = NULL,
      TableNameTo = 'USERGROUPS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '13274EA3-1B24-45D9-9830-E364EE19FB18'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '70837D79-A2CB-4137-8518-E464EFEA1DC5')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('70837D79-A2CB-4137-8518-E464EFEA1DC5', 'N-CURRENCY', '95F91322-7091-4653-AEB2-5DD09E9425EF', NULL, 'CURRENCY', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CURRENCY',
      TableFrom = '95F91322-7091-4653-AEB2-5DD09E9425EF',
      StoredProcName = NULL,
      TableNameTo = 'CURRENCY',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '70837D79-A2CB-4137-8518-E464EFEA1DC5'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'CE83D549-58FB-4C54-9F20-E5D262CBF32B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('CE83D549-58FB-4C54-9F20-E5D262CBF32B', 'P-spSECURITY_', NULL, 'spSECURITY_', NULL, 2, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'P-spSECURITY_',
      TableFrom = 'NULL',
      StoredProcName = 'spSECURITY_',
      TableNameTo = NULL,
      ReplicationMethod = 2,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'CE83D549-58FB-4C54-9F20-E5D262CBF32B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3C559FF4-3585-4CF6-9223-E64718503DEA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3C559FF4-3585-4CF6-9223-E64718503DEA', 'N-PERIODICDISCOUNT', 'CFBB6B18-ABEA-4E00-8244-1CA673A8863A', NULL, 'PERIODICDISCOUNT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-PERIODICDISCOUNT',
      TableFrom = 'CFBB6B18-ABEA-4E00-8244-1CA673A8863A',
      StoredProcName = NULL,
      TableNameTo = 'PERIODICDISCOUNT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3C559FF4-3585-4CF6-9223-E64718503DEA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '71F814FF-77A2-4FA7-B30B-E653A4E42472')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('71F814FF-77A2-4FA7-B30B-E653A4E42472', 'A-POSISSUSPENSIONTYPE', '1BD28BC0-3F41-47F8-BFD0-48FBC96551AF', NULL, 'POSISSUSPENSIONTYPE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISSUSPENSIONTYPE',
      TableFrom = '1BD28BC0-3F41-47F8-BFD0-48FBC96551AF',
      StoredProcName = NULL,
      TableNameTo = 'POSISSUSPENSIONTYPE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '71F814FF-77A2-4FA7-B30B-E653A4E42472'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '5FAE9152-EE60-4D14-9928-E6DB6A015AC5')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('5FAE9152-EE60-4D14-9928-E6DB6A015AC5', 'A-CUSTOMERADDRESS', '8A93FED0-F223-4ED1-BCFA-02C8AB9F275D', NULL, 'CUSTOMERADDRESS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-CUSTOMERADDRESS',
      TableFrom = '8A93FED0-F223-4ED1-BCFA-02C8AB9F275D',
      StoredProcName = NULL,
      TableNameTo = 'CUSTOMERADDRESS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '5FAE9152-EE60-4D14-9928-E6DB6A015AC5'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '106C31E0-ACE4-4B24-A8C6-E7008C8C58DB')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('106C31E0-ACE4-4B24-A8C6-E7008C8C58DB', 'N-HOSPITALITYTYPE', '62B7861D-B777-41B6-9E6C-E54A80EA8B50', NULL, 'HOSPITALITYTYPE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-HOSPITALITYTYPE',
      TableFrom = '62B7861D-B777-41B6-9E6C-E54A80EA8B50',
      StoredProcName = NULL,
      TableNameTo = 'HOSPITALITYTYPE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '106C31E0-ACE4-4B24-A8C6-E7008C8C58DB'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'FB7D0D37-6211-4B48-984B-E70271754786')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('FB7D0D37-6211-4B48-984B-E70271754786', 'A-COMPANYINFO', 'FBDA1538-2C94-4BB8-8A87-635CE2A6540E', NULL, 'COMPANYINFO', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-COMPANYINFO',
      TableFrom = 'FBDA1538-2C94-4BB8-8A87-635CE2A6540E',
      StoredProcName = NULL,
      TableNameTo = 'COMPANYINFO',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'FB7D0D37-6211-4B48-984B-E70271754786'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '1EE24908-C640-448A-AA0C-E7950D652640')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('1EE24908-C640-448A-AA0C-E7950D652640', 'A-TAXREFUNDRANGE', '23EE7EA6-8AE0-4519-8155-94DA15037A96', NULL, 'TAXREFUNDRANGE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TAXREFUNDRANGE',
      TableFrom = '23EE7EA6-8AE0-4519-8155-94DA15037A96',
      StoredProcName = NULL,
      TableNameTo = 'TAXREFUNDRANGE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '1EE24908-C640-448A-AA0C-E7950D652640'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'CEE287D2-A695-498D-801E-E86D628BAED0')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('CEE287D2-A695-498D-801E-E86D628BAED0', 'A-USERLOGINTOKENS', '96824EE4-2C3E-4F2A-A68F-5169214D9573', NULL, 'USERLOGINTOKENS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-USERLOGINTOKENS',
      TableFrom = '96824EE4-2C3E-4F2A-A68F-5169214D9573',
      StoredProcName = NULL,
      TableNameTo = 'USERLOGINTOKENS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'CEE287D2-A695-498D-801E-E86D628BAED0'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '11CE6CFA-FDCA-4B0D-88FD-E8C7424308D3')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('11CE6CFA-FDCA-4B0D-88FD-E8C7424308D3', 'N-USERGROUPPERMISSION', '1B91DA57-F0C4-48C1-82C9-8AB293C0E08C', NULL, 'USERGROUPPERMISSIONS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-USERGROUPPERMISSION',
      TableFrom = '1B91DA57-F0C4-48C1-82C9-8AB293C0E08C',
      StoredProcName = NULL,
      TableNameTo = 'USERGROUPPERMISSIONS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '11CE6CFA-FDCA-4B0D-88FD-E8C7424308D3'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B12989A0-71E5-4D5F-804F-E8F5B071748A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B12989A0-71E5-4D5F-804F-E8F5B071748A', 'A-RBOINFOCODETABLE', '505121A5-98DE-42B2-B1E5-E542F9CD7C2E', NULL, 'RBOINFOCODETABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOINFOCODETABLE',
      TableFrom = '505121A5-98DE-42B2-B1E5-E542F9CD7C2E',
      StoredProcName = NULL,
      TableNameTo = 'RBOINFOCODETABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B12989A0-71E5-4D5F-804F-E8F5B071748A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B33EF012-2789-43A5-B1C7-EA80A6EBA6BF')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B33EF012-2789-43A5-B1C7-EA80A6EBA6BF', 'N-PERMISSIONGROUP', '07E8A0B1-FE0E-4E46-B291-F110FB9E911C', NULL, 'PERMISSIONGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-PERMISSIONGROUP',
      TableFrom = '07E8A0B1-FE0E-4E46-B291-F110FB9E911C',
      StoredProcName = NULL,
      TableNameTo = 'PERMISSIONGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B33EF012-2789-43A5-B1C7-EA80A6EBA6BF'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'B3C2DB8D-ABF1-4457-8F94-EAA5A945D921')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('B3C2DB8D-ABF1-4457-8F94-EAA5A945D921', 'A-POSISHOSPITALITYOPERATIONS', 'CA9F007E-F334-4389-8AA1-E41844A8E117', NULL, 'POSISHOSPITALITYOPERATIONS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSISHOSPITALITYOPERATIONS',
      TableFrom = 'CA9F007E-F334-4389-8AA1-E41844A8E117',
      StoredProcName = NULL,
      TableNameTo = 'POSISHOSPITALITYOPERATIONS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'B3C2DB8D-ABF1-4457-8F94-EAA5A945D921'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '21CD71BD-BC27-43E1-A16A-EB40A7196077')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('21CD71BD-BC27-43E1-A16A-EB40A7196077', 'N-RBOCOLORS', '141724F1-F897-47B8-9800-98261BDF0E76', NULL, 'RBOCOLORS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOCOLORS',
      TableFrom = '141724F1-F897-47B8-9800-98261BDF0E76',
      StoredProcName = NULL,
      TableNameTo = 'RBOCOLORS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '21CD71BD-BC27-43E1-A16A-EB40A7196077'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2C2E8057-7D65-401A-A3F2-ECB28911652B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2C2E8057-7D65-401A-A3F2-ECB28911652B', 'N-RBOTRANSACTIONINFOCODETRANS', 'C4DF66F1-BA4D-4B30-B1B7-9F0F88D780D8', NULL, 'RBOTRANSACTIONINFOCODETRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, 'A0A5DF82-3EB9-4ECA-BBB3-C5FB4FD603EE', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONINFOCODETRANS',
      TableFrom = 'C4DF66F1-BA4D-4B30-B1B7-9F0F88D780D8',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONINFOCODETRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = 'A0A5DF82-3EB9-4ECA-BBB3-C5FB4FD603EE',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2C2E8057-7D65-401A-A3F2-ECB28911652B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6BE14C5E-E24F-47AC-B471-EE2E82432344')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6BE14C5E-E24F-47AC-B471-EE2E82432344', 'N-RBOCOLORGROUPTRANS', '0520B57F-09E9-4E5F-86CE-CA1749AB5043', NULL, 'RBOCOLORGROUPTRANS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOCOLORGROUPTRANS',
      TableFrom = '0520B57F-09E9-4E5F-86CE-CA1749AB5043',
      StoredProcName = NULL,
      TableNameTo = 'RBOCOLORGROUPTRANS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6BE14C5E-E24F-47AC-B471-EE2E82432344'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '995F3671-A9EC-461D-93C2-EEE51A901A98')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('995F3671-A9EC-461D-93C2-EEE51A901A98', 'N-RBOSTYLEGROUPTABLE', 'E33E0E5A-C11E-421E-8038-E5412BC107BC', NULL, 'RBOSTYLEGROUPTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSTYLEGROUPTABLE',
      TableFrom = 'E33E0E5A-C11E-421E-8038-E5412BC107BC',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTYLEGROUPTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '995F3671-A9EC-461D-93C2-EEE51A901A98'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '972727B4-3E0C-4E6E-9D45-EFB70BF30EA2')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('972727B4-3E0C-4E6E-9D45-EFB70BF30EA2', 'A-TAXGROUPHEADING', '73489874-BC34-4D8D-B1EF-38C85FF3F2AB', NULL, 'TAXGROUPHEADING', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-TAXGROUPHEADING',
      TableFrom = '73489874-BC34-4D8D-B1EF-38C85FF3F2AB',
      StoredProcName = NULL,
      TableNameTo = 'TAXGROUPHEADING',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '972727B4-3E0C-4E6E-9D45-EFB70BF30EA2'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '9860AE58-4E69-4853-A3BE-F0DA587C951A')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('9860AE58-4E69-4853-A3BE-F0DA587C951A', 'N-TAXREFUNDTRANSACTION', '1AC02749-3F29-4904-BA4D-AF86EB6EA5D4', NULL, 'TAXREFUNDTRANSACTION', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TAXREFUNDTRANSACTION',
      TableFrom = '1AC02749-3F29-4904-BA4D-AF86EB6EA5D4',
      StoredProcName = NULL,
      TableNameTo = 'TAXREFUNDTRANSACTION',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '9860AE58-4E69-4853-A3BE-F0DA587C951A'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'C43C6E69-7442-496F-98CB-F0F9F454E5A1')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('C43C6E69-7442-496F-98CB-F0F9F454E5A1', 'N-RBOTRANSACTIONLOGTRANS', '51C72CEF-794B-458C-A370-A91C924C506B', NULL, 'RBOTRANSACTIONLOGTRANS', 0, 1, 1, 0, NULL, NULL, 0, 0, '2D9CAB5C-5BFB-4FB3-95A7-CDE4D78D99DA', 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOTRANSACTIONLOGTRANS',
      TableFrom = '51C72CEF-794B-458C-A370-A91C924C506B',
      StoredProcName = NULL,
      TableNameTo = 'RBOTRANSACTIONLOGTRANS',
      ReplicationMethod = 0,
      WhatToDo = 1,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = '2D9CAB5C-5BFB-4FB3-95A7-CDE4D78D99DA',
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'C43C6E69-7442-496F-98CB-F0F9F454E5A1'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '23AE57A4-4BDC-4490-A199-F12875A6B9DA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('23AE57A4-4BDC-4490-A199-F12875A6B9DA', 'N-POSISOPERATIONS', '11AE76BE-27F8-4916-BC99-D3A9DDED5283', NULL, 'POSISOPERATIONS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSISOPERATIONS',
      TableFrom = '11AE76BE-27F8-4916-BC99-D3A9DDED5283',
      StoredProcName = NULL,
      TableNameTo = 'POSISOPERATIONS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '23AE57A4-4BDC-4490-A199-F12875A6B9DA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'A5E9FD14-AB0D-4190-8CA5-F1CDE6E2925B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('A5E9FD14-AB0D-4190-8CA5-F1CDE6E2925B', 'N-SYSTEMSETTINGS', '636C6AB5-166D-4C9E-8F78-B7669C90115E', NULL, 'SYSTEMSETTINGS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-SYSTEMSETTINGS',
      TableFrom = '636C6AB5-166D-4C9E-8F78-B7669C90115E',
      StoredProcName = NULL,
      TableNameTo = 'SYSTEMSETTINGS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'A5E9FD14-AB0D-4190-8CA5-F1CDE6E2925B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '52C1AA65-532D-405A-8A33-F2A8E0DDEB8C')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('52C1AA65-532D-405A-8A33-F2A8E0DDEB8C', 'N-TAXGROUPHEADING', '73489874-BC34-4D8D-B1EF-38C85FF3F2AB', NULL, 'TAXGROUPHEADING', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-TAXGROUPHEADING',
      TableFrom = '73489874-BC34-4D8D-B1EF-38C85FF3F2AB',
      StoredProcName = NULL,
      TableNameTo = 'TAXGROUPHEADING',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '52C1AA65-532D-405A-8A33-F2A8E0DDEB8C'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'E21130E3-8128-4289-A9A8-F2C675673F1E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('E21130E3-8128-4289-A9A8-F2C675673F1E', 'N-CONTACTTABLE', 'C0CEA21E-0F77-4964-8506-0E2CD0D8A977', NULL, 'CONTACTTABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CONTACTTABLE',
      TableFrom = 'C0CEA21E-0F77-4964-8506-0E2CD0D8A977',
      StoredProcName = NULL,
      TableNameTo = 'CONTACTTABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'E21130E3-8128-4289-A9A8-F2C675673F1E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3FE7C444-97D1-42BC-8FC5-F342DE7F45B9')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3FE7C444-97D1-42BC-8FC5-F342DE7F45B9', 'N-POSVISUALPROFILE', 'DB644ED5-2024-4A71-BED9-A70324BFD320', NULL, 'POSVISUALPROFILE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSVISUALPROFILE',
      TableFrom = 'DB644ED5-2024-4A71-BED9-A70324BFD320',
      StoredProcName = NULL,
      TableNameTo = 'POSVISUALPROFILE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3FE7C444-97D1-42BC-8FC5-F342DE7F45B9'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '6698D862-5AE0-49D1-9BCE-F360BC3F134B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('6698D862-5AE0-49D1-9BCE-F360BC3F134B', 'N-IMAGES', '9A5109F1-713F-485C-9F8D-FF60D9B4780C', NULL, 'IMAGES', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-IMAGES',
      TableFrom = '9A5109F1-713F-485C-9F8D-FF60D9B4780C',
      StoredProcName = NULL,
      TableNameTo = 'IMAGES',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '6698D862-5AE0-49D1-9BCE-F360BC3F134B'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '2091937B-9734-4B0A-8796-F4702D3D0D9F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('2091937B-9734-4B0A-8796-F4702D3D0D9F', 'A-INVENTTABLEMODULE', '1788D6AA-91C5-47EE-8DB5-8DE6B00C3C57', NULL, 'INVENTTABLEMODULE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-INVENTTABLEMODULE',
      TableFrom = '1788D6AA-91C5-47EE-8DB5-8DE6B00C3C57',
      StoredProcName = NULL,
      TableNameTo = 'INVENTTABLEMODULE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '2091937B-9734-4B0A-8796-F4702D3D0D9F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '699842AF-BDDC-4821-84C4-F4853CB4FAEA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('699842AF-BDDC-4821-84C4-F4853CB4FAEA', 'N-CUSTOMERADDRESS', '8A93FED0-F223-4ED1-BCFA-02C8AB9F275D', NULL, 'CUSTOMERADDRESS', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-CUSTOMERADDRESS',
      TableFrom = '8A93FED0-F223-4ED1-BCFA-02C8AB9F275D',
      StoredProcName = NULL,
      TableNameTo = 'CUSTOMERADDRESS',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '699842AF-BDDC-4821-84C4-F4853CB4FAEA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '67123259-B1BA-4DDE-A77E-F5C9D3382F14')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('67123259-B1BA-4DDE-A77E-F5C9D3382F14', 'N-INVENTDIMSETUP', '1D146271-277F-4A17-8822-0B3D962747FB', NULL, 'INVENTDIMSETUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-INVENTDIMSETUP',
      TableFrom = '1D146271-277F-4A17-8822-0B3D962747FB',
      StoredProcName = NULL,
      TableNameTo = 'INVENTDIMSETUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '67123259-B1BA-4DDE-A77E-F5C9D3382F14'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '4886ED14-434A-4D95-8E14-F7A5DD42EF4F')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('4886ED14-434A-4D95-8E14-F7A5DD42EF4F', 'A-POSLOOKUP', '7970B1A5-5BE5-4EB9-809A-1FC0807E9C0B', NULL, 'POSLOOKUP', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSLOOKUP',
      TableFrom = '7970B1A5-5BE5-4EB9-809A-1FC0807E9C0B',
      StoredProcName = NULL,
      TableNameTo = 'POSLOOKUP',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '4886ED14-434A-4D95-8E14-F7A5DD42EF4F'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '97560299-9FA5-4EB8-AC8C-F7F52CCEF03E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('97560299-9FA5-4EB8-AC8C-F7F52CCEF03E', 'N-RBOSTORETABLE', '4EEC0E6F-F21C-4B3D-A2B1-DFC2C0C2A0BE', NULL, 'RBOSTORETABLE', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSTORETABLE',
      TableFrom = '4EEC0E6F-F21C-4B3D-A2B1-DFC2C0C2A0BE',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTORETABLE',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '97560299-9FA5-4EB8-AC8C-F7F52CCEF03E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '3167B65F-6918-490B-95B7-F86D009617DA')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('3167B65F-6918-490B-95B7-F86D009617DA', 'N-RBOSTAFFPERMISSIONGROUP', '8E565A79-2F0F-4FBE-BE5E-C1141A53551A', NULL, 'RBOSTAFFPERMISSIONGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-RBOSTAFFPERMISSIONGROUP',
      TableFrom = '8E565A79-2F0F-4FBE-BE5E-C1141A53551A',
      StoredProcName = NULL,
      TableNameTo = 'RBOSTAFFPERMISSIONGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '3167B65F-6918-490B-95B7-F86D009617DA'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '342C6076-30A2-40D9-8F53-F9D2B51E86E6')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('342C6076-30A2-40D9-8F53-F9D2B51E86E6', 'A-RBOUNIT', '04DD7908-1003-4F5E-AFA9-50FC1F99D21C', NULL, 'RBOUNIT', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOUNIT',
      TableFrom = '04DD7908-1003-4F5E-AFA9-50FC1F99D21C',
      StoredProcName = NULL,
      TableNameTo = 'RBOUNIT',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '342C6076-30A2-40D9-8F53-F9D2B51E86E6'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = 'C15A005F-BBDE-464F-B014-FAE896D47500')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('C15A005F-BBDE-464F-B014-FAE896D47500', 'N-SPECIALGROUP', 'D6C6AC7C-6A0D-400E-85DD-8420240A3717', NULL, 'SPECIALGROUP', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-SPECIALGROUP',
      TableFrom = 'D6C6AC7C-6A0D-400E-85DD-8420240A3717',
      StoredProcName = NULL,
      TableNameTo = 'SPECIALGROUP',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = 'C15A005F-BBDE-464F-B014-FAE896D47500'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '98839016-B21D-4E4C-8ECA-FC2599637884')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('98839016-B21D-4E4C-8ECA-FC2599637884', 'N-POSPERIODICDISCOUNT', 'C96D3A6F-B7A8-48FC-B801-D5FEB1F0402B', NULL, 'POSPERIODICDISCOUNT', 0, 4, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-POSPERIODICDISCOUNT',
      TableFrom = 'C96D3A6F-B7A8-48FC-B801-D5FEB1F0402B',
      StoredProcName = NULL,
      TableNameTo = 'POSPERIODICDISCOUNT',
      ReplicationMethod = 0,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '98839016-B21D-4E4C-8ECA-FC2599637884'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '02B503FD-F352-4042-8B30-FC8C46BEDB4E')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('02B503FD-F352-4042-8B30-FC8C46BEDB4E', 'A-RBOSIZEGROUPTABLE', 'A36163FF-F083-420E-9286-B7F520ADDB6B', NULL, 'RBOSIZEGROUPTABLE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RBOSIZEGROUPTABLE',
      TableFrom = 'A36163FF-F083-420E-9286-B7F520ADDB6B',
      StoredProcName = NULL,
      TableNameTo = 'RBOSIZEGROUPTABLE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '02B503FD-F352-4042-8B30-FC8C46BEDB4E'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '85EDA60D-942E-41EC-84DF-FD9AF67A72FE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('85EDA60D-942E-41EC-84DF-FD9AF67A72FE', 'A-POSCOLOR', '428DD277-1E97-4FBA-9B08-1D5ACE222DA2', NULL, 'POSCOLOR', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-POSCOLOR',
      TableFrom = '428DD277-1E97-4FBA-9B08-1D5ACE222DA2',
      StoredProcName = NULL,
      TableNameTo = 'POSCOLOR',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '85EDA60D-942E-41EC-84DF-FD9AF67A72FE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '86A3615B-6696-4D84-8736-FE46A5FF6FFE')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('86A3615B-6696-4D84-8736-FE46A5FF6FFE', 'N-USERS', '6CC19068-7CAC-483C-8AB7-AA353E92F812', NULL, 'USERS', 0, 5, 1, 0, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'N-USERS',
      TableFrom = '6CC19068-7CAC-483C-8AB7-AA353E92F812',
      StoredProcName = NULL,
      TableNameTo = 'USERS',
      ReplicationMethod = 0,
      WhatToDo = 5,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = NULL,
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '86A3615B-6696-4D84-8736-FE46A5FF6FFE'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '81D559E8-0C16-4710-9E49-FF3D01A85851')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('81D559E8-0C16-4710-9E49-FF3D01A85851', 'A-RETAILITEMIMAGE', 'F797F1D8-98A0-44A0-A451-EFC75AB46F21', NULL, 'RETAILITEMIMAGE', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, 0, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-RETAILITEMIMAGE',
      TableFrom = 'F797F1D8-98A0-44A0-A451-EFC75AB46F21',
      StoredProcName = NULL,
      TableNameTo = 'RETAILITEMIMAGE',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = 0,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '81D559E8-0C16-4710-9E49-FF3D01A85851'
END
GO
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobs
  WHERE ID = '54E60C30-0D4E-40E1-9CF2-FF689C13C10B')
BEGIN
  INSERT INTO JscSubJobs (ID, Description, TableFrom, StoredProcName, TableNameTo, ReplicationMethod, WhatToDo, Enabled, IncludeFlowFields, ActionTable, ActionCounterInterval, MoveActions, NoDistributionFilter, RepCounterField, RepCounterInterval, UpdateRepCounter, UpdateRepCounterOnEmptyInt, MarkSentRecordsField)
    VALUES ('54E60C30-0D4E-40E1-9CF2-FF689C13C10B', 'A-SALESPARAMETERS', '2D9ED6E2-070B-4E7C-9C72-F9FE158D4862', NULL, 'SALESPARAMETERS', 1, 4, 1, 0, 'E13F1E30-EEDA-4483-8055-1643600464BF', NULL, 0, 0, NULL, NULL, 0, 0, NULL)
END
ELSE
BEGIN
  UPDATE JscSubJobs
  SET Description = 'A-SALESPARAMETERS',
      TableFrom = '2D9ED6E2-070B-4E7C-9C72-F9FE158D4862',
      StoredProcName = NULL,
      TableNameTo = 'SALESPARAMETERS',
      ReplicationMethod = 1,
      WhatToDo = 4,
      Enabled = 1,
      IncludeFlowFields = 0,
      ActionTable = 'E13F1E30-EEDA-4483-8055-1643600464BF',
      ActionCounterInterval = NULL,
      MoveActions = 0,
      NoDistributionFilter = 0,
      RepCounterField = NULL,
      RepCounterInterval = NULL,
      UpdateRepCounter = 0,
      UpdateRepCounterOnEmptyInt = 0,
      MarkSentRecordsField = NULL
  WHERE ID = '54E60C30-0D4E-40E1-9CF2-FF689C13C10B'
END