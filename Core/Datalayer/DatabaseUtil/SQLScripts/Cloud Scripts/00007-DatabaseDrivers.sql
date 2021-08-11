
/*

	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Cumulus
	Date created	: 2014/11/18

	Description		: Auto generated from a database using the following script 

/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 Id
      ,Name
      ,DatabaseServerType
      ,DatabaseParams
      ,ConnectionStringFormat
      ,EnabledFields
  FROM CloudSample.dbo.JscDriverTypes

  SELECT 
  'IF NOT EXISTS(SELECT * FROM JscDriverTypes WHERE ID = ''',driver.id,''' ) 
  BEGIN 
  INSERT INTO JscDriverTypes (
	ID,
	Name,
	DatabaseServerType,
	DatabaseParams,
	ConnectionStringFormat,
	EnabledFields)
  VALUES(
''', driver.id,''',
''',Name,''',
', DatabaseServerType,',
', CASE When DatabaseParams IS not null then '''' else '' END ,  DatabaseParams,CASE When DatabaseParams IS not null then '''' else '' END,',
', CASE When ConnectionStringFormat IS not null then '''' else '' END ,  ConnectionStringFormat,CASE When ConnectionStringFormat IS not null then '''' else '' END,',
', CASE When EnabledFields IS not null then '''' else '' END ,  EnabledFields,CASE When EnabledFields IS not null then '''' else '' END,')',
'	
	END
ELSE
	Begin
		Update  JscDriverTypes set
			Name=''',Name,''',
			DatabaseServerType=',DatabaseServerType,',
			DatabaseParams=',CASE When DatabaseParams IS not null then '''' else '' END ,  DatabaseParams,CASE When DatabaseParams IS not null then '''' else '' END,',
			ConnectionStringFormat=',CASE When ConnectionStringFormat IS not null then '''' else '' END ,  ConnectionStringFormat,CASE When ConnectionStringFormat IS not null then '''' else '' END,',
			EnabledFields=', CASE When EnabledFields IS not null then '''' else '' END ,  EnabledFields,CASE When EnabledFields IS not null then '''' else '' END,'
		WHERE ID = ''',driver.id,'''
	END'
  FROM (
  SELECT  ID -- USE newid() AS id if you need new ones
        ,Name
      ,DatabaseServerType
      ,DatabaseParams
      ,ConnectionStringFormat
      ,EnabledFields
  FROM JscDriverTypes) driver

  Then using creative replacements to get the required results.
	
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/

Use LSPOSNET 

IF NOT EXISTS(SELECT * FROM JscDriverTypes WHERE ID = '2C844CFF-886D-414C-8D1F-081E291A07C0' ) 
  BEGIN 
  INSERT INTO JscDriverTypes (
	ID,
	Name,
	DatabaseServerType,
	DatabaseParams,
	ConnectionStringFormat,
	EnabledFields)
  VALUES(
'2C844CFF-886D-414C-8D1F-081E291A07C0',
'OLE DB',
	6	,
		NULL		,
'Provider=SQLOLEDB;Data Source={DBServerHost};Initial Catalog={DBPathName};User ID={UserId};Password={Password};|{DatabaseServerType}|{DatabaseParams}',
'UserId,Password,DBServerHost,DBPathName')		
	END
ELSE
	Begin
		Update  JscDriverTypes set
			Name='OLE DB',
			DatabaseServerType=	6	,
			DatabaseParams=		NULL		,
			ConnectionStringFormat='Provider=SQLOLEDB;Data Source={DBServerHost};Initial Catalog={DBPathName};User ID={UserId};Password={Password};|{DatabaseServerType}|{DatabaseParams}',
			EnabledFields='UserId,Password,DBServerHost,DBPathName'
		WHERE ID = '2C844CFF-886D-414C-8D1F-081E291A07C0'
	END
IF NOT EXISTS(SELECT * FROM JscDriverTypes WHERE ID = 'F3B1CD7C-1E8D-452E-B0F6-53E240E6C176' ) 
  BEGIN 
  INSERT INTO JscDriverTypes (
	ID,
	Name,
	DatabaseServerType,
	DatabaseParams,
	ConnectionStringFormat,
	EnabledFields)
  VALUES(
'F3B1CD7C-1E8D-452E-B0F6-53E240E6C176',
'NAV Native 6.01',
	0	,
'ndbcn@601',
'id={Code};company={Company};server={DBServerHost};nt=tcp;user={UserId};passwd={Password};|{DatabaseServerType}|{DatabaseParams}',
'Company,UserId,Password,DBServerHost,DBPathName,NetType')		
	END
ELSE
	Begin
		Update  JscDriverTypes set
			Name='NAV Native 6.01',
			DatabaseServerType=	0	,
			DatabaseParams='ndbcn@601',
			ConnectionStringFormat='id={Code};company={Company};server={DBServerHost};nt=tcp;user={UserId};passwd={Password};|{DatabaseServerType}|{DatabaseParams}',
			EnabledFields='Company,UserId,Password,DBServerHost,DBPathName,NetType'
		WHERE ID = 'F3B1CD7C-1E8D-452E-B0F6-53E240E6C176'
	END
IF NOT EXISTS(SELECT * FROM JscDriverTypes WHERE ID = '67BC214E-293A-4EA7-99F7-76F612172B07' ) 
  BEGIN 
  INSERT INTO JscDriverTypes (
	ID,
	Name,
	DatabaseServerType,
	DatabaseParams,
	ConnectionStringFormat,
	EnabledFields)
  VALUES(
'67BC214E-293A-4EA7-99F7-76F612172B07',
'OLE DB (Windows authentication)',
	6	,
		NULL		,
'Provider=SQLOLEDB;Data Source={DBServerHost};Initial Catalog={DBPathName};Integrated Security=SSPI;|{DatabaseServerType}|{DatabaseParams}',
'DBServerHost,DBPathName')		
	END
ELSE
	Begin
		Update  JscDriverTypes set
			Name='OLE DB (Windows authentication)',
			DatabaseServerType=	6	,
			DatabaseParams=		NULL		,
			ConnectionStringFormat='Provider=SQLOLEDB;Data Source={DBServerHost};Initial Catalog={DBPathName};Integrated Security=SSPI;|{DatabaseServerType}|{DatabaseParams}',
			EnabledFields='DBServerHost,DBPathName'
		WHERE ID = '67BC214E-293A-4EA7-99F7-76F612172B07'
	END
IF NOT EXISTS(SELECT * FROM JscDriverTypes WHERE ID = '869E6CE1-600B-4A8A-A2A8-79473CD49BB2' ) 
  BEGIN 
  INSERT INTO JscDriverTypes (
	ID,
	Name,
	DatabaseServerType,
	DatabaseParams,
	ConnectionStringFormat,
	EnabledFields)
  VALUES(
'869E6CE1-600B-4A8A-A2A8-79473CD49BB2',
'MS SQL (Windows authentication)',
	2	,
		NULL		,
'Data Source={DBServerHost};Initial Catalog={DBPathName};Integrated Security=SSPI;Network Library={ConnectionType};|{DatabaseServerType}|{DatabaseParams}',
'DBServerHost,DBPathName,ConnectionType')		
	END
ELSE
	Begin
		Update  JscDriverTypes set
			Name='MS SQL (Windows authentication)',
			DatabaseServerType=	2	,
			DatabaseParams=		NULL		,
			ConnectionStringFormat='Data Source={DBServerHost};Initial Catalog={DBPathName};Integrated Security=SSPI;Network Library={ConnectionType};|{DatabaseServerType}|{DatabaseParams}',
			EnabledFields='DBServerHost,DBPathName,ConnectionType'
		WHERE ID = '869E6CE1-600B-4A8A-A2A8-79473CD49BB2'
	END
IF NOT EXISTS(SELECT * FROM JscDriverTypes WHERE ID = 'C3EBC991-357E-4799-AB3A-8F1691BB0BD1' ) 
  BEGIN 
  INSERT INTO JscDriverTypes (
	ID,
	Name,
	DatabaseServerType,
	DatabaseParams,
	ConnectionStringFormat,
	EnabledFields)
  VALUES(
'C3EBC991-357E-4799-AB3A-8F1691BB0BD1',
'MS SQL',
	2	,
		NULL		,
'Data Source={DBServerHost};Initial Catalog={DBPathName};User ID={UserId};Password={Password};Network Library={ConnectionType};|{DatabaseServerType}|{DatabaseParams}',
'UserId,Password,DBServerHost,DBPathName,ConnectionType')		
	END
ELSE
	Begin
		Update  JscDriverTypes set
			Name='MS SQL',
			DatabaseServerType=	2	,
			DatabaseParams=		NULL		,
			ConnectionStringFormat='Data Source={DBServerHost};Initial Catalog={DBPathName};User ID={UserId};Password={Password};Network Library={ConnectionType};|{DatabaseServerType}|{DatabaseParams}',
			EnabledFields='UserId,Password,DBServerHost,DBPathName,ConnectionType'
		WHERE ID = 'C3EBC991-357E-4799-AB3A-8F1691BB0BD1'
	END
IF NOT EXISTS(SELECT * FROM JscDriverTypes WHERE ID = '97290F9C-6C5B-41EF-A5B8-C35690E831CF' ) 
  BEGIN 
  INSERT INTO JscDriverTypes (
	ID,
	Name,
	DatabaseServerType,
	DatabaseParams,
	ConnectionStringFormat,
	EnabledFields)
  VALUES(
'97290F9C-6C5B-41EF-A5B8-C35690E831CF',
'NAV SQL 6.01',
	0	,
'ndbcs@601',
'id={Code};company={Company};server={DBServerHost};dbname={DBPathName};nt=tcp;user={UserId};passwd={Password};|{DatabaseServerType}|{DatabaseParams}',
'Company,UserId,Password,DBServerHost,DBPathName,NetType')		
	END
ELSE
	Begin
		Update  JscDriverTypes set
			Name='NAV SQL 6.01',
			DatabaseServerType=	0	,
			DatabaseParams='ndbcs@601',
			ConnectionStringFormat='id={Code};company={Company};server={DBServerHost};dbname={DBPathName};nt=tcp;user={UserId};passwd={Password};|{DatabaseServerType}|{DatabaseParams}',
			EnabledFields='Company,UserId,Password,DBServerHost,DBPathName,NetType'
		WHERE ID = '97290F9C-6C5B-41EF-A5B8-C35690E831CF'
	END