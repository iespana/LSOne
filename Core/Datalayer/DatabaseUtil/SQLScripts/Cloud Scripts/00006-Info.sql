
/*

	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Cumulus
	Date created	: 2014/11/18

	Description		: Auto generated from a database using the following script 
 SELECT 
  'IF NOT EXISTS(SELECT * FROM JscInfos WHERE ID = ''',info.id,''' ) 
  BEGIN 
  INSERT INTO JscInfos (
	ID,
	Name,
	Xml)
  VALUES(
''', info.id,''',
''',Name,''',
', CASE When Xml IS not null then '''' else '' END ,  Xml,CASE When Xml IS not null then '''' else '' END,')',
'	
	END
ELSE
	Begin
		Update  JscInfos set
			Name=''',Name,''',
			Xml=',CASE When Xml IS not null then '''' else '' END ,  Xml,CASE When Xml IS not null then '''' else '' END,'
		WHERE ID = ''',info.id,'''
	END'
  FROM (
  SELECT  ID -- USE newid() AS id if you need new ones
      ,Name
      ,Xml
  FROM JscInfos) info

  Then using creative replacements to get the required results.
	
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/

Use LSPOSNET 

IF NOT EXISTS(SELECT * FROM JscInfos WHERE ID = 	'2C07453E-8E0A-4C50-BF25-EEC033DB83C6' ) 
  BEGIN 
  INSERT INTO JscInfos (
	ID,
	Name,
	Xml)
  VALUES(
	'2C07453E-8E0A-4C50-BF25-EEC033DB83C6',
	'SchedulerSettings',
		'<SchedulerSettings xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><ServerSettings><Host>localhost</Host><NetMode>TCP</NetMode></ServerSettings></SchedulerSettings>		')		
	END
ELSE
	Begin
		Update  JscInfos set
			Name=	'SchedulerSettings',
			Xml=		'<SchedulerSettings xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><ServerSettings><Host>localhost</Host><NetMode>TCP</NetMode></ServerSettings></SchedulerSettings>		'
		WHERE ID = 	'2C07453E-8E0A-4C50-BF25-EEC033DB83C6'
	END