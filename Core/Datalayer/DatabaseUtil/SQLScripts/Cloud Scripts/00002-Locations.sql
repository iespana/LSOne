
/*

	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Cumulus
	Date created	: 2014/11/18

	Description		: Auto generated from a database using the following script 
	/****** Script for SelectTopNRows command from SSMS  ******/

 
  SELECT 
  'IF NOT EXISTS(SELECT * FROM JscLocations WHERE ID = '''+CONVERT(VARCHAR(50),LOC.id)+''' ) 
  BEGIN 
  INSERT INTO JscLocations (
	ID,
	Name,
	EXDATAAREAID,
	ExCode,
	DatabaseDesign,
	LocationKind,
	DefaultOwner,
	DDHost,
	DDPort,
	DDNetMode,
	Enabled,
	Company,
	UserId,
	Password,
	DBServerIsUsed,
	DBServerHost,
	DBPathName,
	DBDriverType,
	DBConnectionString,
	SystemTag)
  VALUES(
'''+CONVERT(VARCHAR(50),LOC.id)+''',
'''+ Name+''',
'+CASE When EXDATAAREAID IS not null then '''' else '' END , EXDATAAREAID,CASE When EXDATAAREAID IS not null then '''' else '' END+',
'+CASE When ExCode IS not null then '''' else '' END ,  ExCode+CASE When ExCode IS not null then '''' else '' END+',
'+CASE When DatabaseDesign IS not null then '''' else '' END , DatabaseDesign,CASE When DatabaseDesign IS not null then '''' else '' END+',
'+ CONVERT(VARCHAR(50),LocationKind)+',
'+CASE When DefaultOwner IS not null then '''' else '' END ,DefaultOwner,CASE When DefaultOwner IS not null then '''' else '' END+',
'+CASE When DDHost IS not null then '''' else '' END , DDHost,CASE When DDHost IS not null then '''' else '' END+',
'+CASE When DDPort IS not null then '''' else '' END ,  DDPort,CASE When DDPort IS not null then '''' else '' END+',
'+ CONVERT(VARCHAR(50), DDNetMode)+',
'+ CONVERT(VARCHAR, Enabled)+',
'+CASE When Company IS not null then '''' else '' END ,  Company,CASE When Company IS not null then '''' else '' END+',
'+CASE When UserId IS not null then '''' else '' END ,  UserId,CASE When UserId IS not null then '''' else '' END+',
'+CASE When Password IS not null then '''' else '' END ,  Password,CASE When Password IS not null then '''' else '' END+',
'+CONVERT(VARCHAR, DBServerIsUsed)+',
'+CASE When DBServerHost IS not null then '''' else '' END ,  DBServerHost,CASE When DBServerHost IS not null then '''' else '' END+',
'+CASE When DBPathName IS not null then '''' else '' END ,  DBPathName,CASE When DBPathName IS not null then '''' else '' END+',
'+CASE When DBDriverType IS not null then '''' else '' END , DBDriverType,CASE When DBDriverType IS not null then '''' else '' END+',
'+CASE When DBConnectionString IS not null then '''' else '' END ,  DBConnectionString,CASE When DBConnectionString IS not null then '''' else '' END+',
'+CASE When SystemTag IS not null then '''' else '' END ,  SystemTag,CASE When SystemTag IS not null then '''' else '' END+')'+
'	
	END
ELSE
	Begin
		Update  JscLocations set
			Name='''+Name+''',
			EXDATAAREAID='''+EXDATAAREAID+''',
			ExCode='+CASE When EXDATAAREAID IS not null then '''' else '' END ,  EXDATAAREAID,CASE When EXDATAAREAID IS not null then '''' else '' END+',
			DatabaseDesign='+CASE When DatabaseDesign IS not null then '''' else '' END ,  DatabaseDesign, CASE When DatabaseDesign IS not null then '''' else '' END+',
			LocationKind='+ CONVERT(VARCHAR(50),LocationKind)+'
			DefaultOwner='+CASE When DefaultOwner IS not null then '''' else '' END ,   DefaultOwner,CASE When DefaultOwner IS not null then '''' else '' END+',
			DDHost='+CASE When DDHost IS not null then '''' else '' END ,  DDHost,CASE When DDHost IS not null then '''' else '' END+',
			DDPort='+CASE When DDPort IS not null then '''' else '' END ,  DDPort,CASE When DDPort IS not null then '''' else '' END+',
			DDNetMode='+ CONVERT(VARCHAR(50),DDNetMode)+'
			Enabled='+CONVERT(VARCHAR, Enabled)+',
			Company='+CASE When Company IS not null then '''' else '' END ,  Company,CASE When Company IS not null then '''' else '' END+',
			UserId='+CASE When UserId IS not null then '''' else '' END ,  UserId,CASE When UserId IS not null then '''' else '' END+',
			Password='+CASE When Password IS not null then '''' else '' END ,  Password,CASE When Password IS not null then '''' else '' END+',
			DBServerIsUsed='+CONVERT(VARCHAR, DBServerIsUsed)+',
			DBServerHost='+CASE When DBServerHost IS not null then '''' else '' END ,  DBServerHost,CASE When DBServerHost IS not null then '''' else '' END+',
			DBPathName='+CASE When DBPathName IS not null then '''' else '' END ,  DBPathName,CASE When DBPathName IS not null then '''' else '' END+',
			DBDriverType='+CASE When DBDriverType IS not null then '''' else '' END ,  DBDriverType, CASE When DBDriverType IS not null then '''' else '' END+',
			DBConnectionString='+CASE When DBConnectionString IS not null then '''' else '' END ,  DBConnectionString,CASE When DBConnectionString IS not null then '''' else '' END+',
			SystemTag='+CASE When SystemTag IS not null then '''' else '' END ,  SystemTag,CASE When SystemTag IS not null then '''' else '' END+'
		WHERE ID = '''+CONVERT(VARCHAR(50),LOC.id)+'''
	END'
  FROM (
  SELECT ID -- USE newid() AS id if you need new ones
       ,Name
      ,EXDATAAREAID
      ,ExCode
      ,DatabaseDesign
      ,LocationKind
      ,DefaultOwner
      ,DDHost
      ,DDPort
      ,DDNetMode
      ,Enabled
      ,Company
      ,UserId
      ,Password
      ,DBServerIsUsed
      ,DBServerHost
      ,DBPathName
      ,DBDriverType
      ,DBConnectionString
      ,SystemTag
  FROM JscLocations where name in ('local','cloud')) LOC

  Then using creative replacements to get the required results.
	
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/

Use LSPOSNET 

IF NOT EXISTS(SELECT * FROM JscLocations WHERE ID ='1AEF42A8-9E25-4234-A523-A5B0647F486A' ) 
  BEGIN 
  INSERT INTO JscLocations (
	ID,
	Name,
	EXDATAAREAID,
	ExCode,
	DatabaseDesign,
	LocationKind,
	DefaultOwner,
	DDHost,
	DDPort,
	DDNetMode,
	Enabled,
	Company,
	UserId,
	Password,
	DBServerIsUsed,
	DBServerHost,
	DBPathName,
	DBDriverType,
	DBConnectionString,
	SystemTag)
  VALUES(
	'1AEF42A8-9E25-4234-A523-A5B0647F486A',
	'Local',
	'LSR',
	'',
	'F971A608-D577-4A0A-82A6-97D4B8AEEFD4',
	1,
		NULL,
	'localhost',
	'',
	1,
	1,
	'',
	'',
	'',
	1,
	'',
	'',
		NULL,
	'Data Source=localhost;Initial Catalog=LSONEPOS;User=DataDir;Password=DataDir.2008;Network Library=dbmslpcn;|ms|',
	'')		
	END
ELSE
	Begin
		Update  JscLocations set
			Name='Local',
			EXDATAAREAID='LSR',
			ExCode='LSR',
			DatabaseDesign='F971A608-D577-4A0A-82A6-97D4B8AEEFD4',
			LocationKind=LocationKind,
			DefaultOwner=NULL,
			DDHost='localhost',
			DDPort='',
			DDNetMode=DDNetMode,
			Enabled=1,
			Company='',
			UserId='',
			Password='',
			DBServerIsUsed=1,
			DBServerHost='',
			DBPathName='',
			DBDriverType=NULL,
			DBConnectionString='Data Source=localhost;Initial Catalog=LSONEPOS;User=DataDir;Password=DataDir.2008;Network Library=dbmslpcn;|ms|',
			SystemTag=''
		WHERE ID ='1AEF42A8-9E25-4234-A523-A5B0647F486A'
	END
IF NOT EXISTS(SELECT * FROM JscLocations WHERE ID ='774E57C4-0CC6-422B-96D5-CFEC71D815EE' ) 
  BEGIN 
  INSERT INTO JscLocations (
	ID,
	Name,
	EXDATAAREAID,
	ExCode,
	DatabaseDesign,
	LocationKind,
	DefaultOwner,
	DDHost,
	DDPort,
	DDNetMode,
	Enabled,
	Company,
	UserId,
	Password,
	DBServerIsUsed,
	DBServerHost,
	DBPathName,
	DBDriverType,
	DBConnectionString,
	SystemTag)
  VALUES(
	'774E57C4-0CC6-422B-96D5-CFEC71D815EE',
	'Cloud',
	'LSR',
	'',
	'F971A608-D577-4A0A-82A6-97D4B8AEEFD4',
	1,
		NULL,
	'localhost',
	'',
	1,
	1,
	'',
	'',
	'',
	1,
	'',
	'',
		NULL,
	'Data Source=lsone.cloudapp.net;Initial Catalog=;User ID=;Password=;Network Library=dbmssocn;|ms|',
	'')		
	END
ELSE
	Begin
		Update  JscLocations set
			Name='Cloud',
			EXDATAAREAID='LSR',
			ExCode='LSR',
			DatabaseDesign='F971A608-D577-4A0A-82A6-97D4B8AEEFD4',
			LocationKind=LocationKind,
			DefaultOwner=NULL,
			DDHost='localhost',
			DDPort='',
			DDNetMode=DDNetMode,
			Enabled=1,
			Company='',
			UserId='',
			Password='',
			DBServerIsUsed=1,
			DBServerHost='',
			DBPathName='',
			DBDriverType=NULL,
			DBConnectionString='Data Source=lsone.cloudapp.net;Initial Catalog=Simplecloud;User ID=sa;Password=elvis.123;Network Library=dbmssocn;|ms|',
			SystemTag=''
		WHERE ID ='774E57C4-0CC6-422B-96D5-CFEC71D815EE'
	END