
/*

	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Cumulus
	Date created	: 2014/11/18

	Description		: Auto generated from a database using the following script 
	/****** Script for SelectTopNRows command from SSMS  ******/
 
 SELECT 
  'IF NOT EXISTS(SELECT * FROM JscDatabaseDesigns WHERE ID = ''',design.id,''' ) 
  BEGIN 
  INSERT INTO JscDatabaseDesigns (
	ID,
	Description,
	CodePage,
	Enabled)
  VALUES(
''', design.id,''',
''',Description,''',
', CodePage,',
' , Enabled,')',
'	
	END
ELSE
	Begin
		Update  JscDatabaseDesigns set
			Description=''',Description,''',
			CodePage=',CodePage,',
			Enabled=',Enabled,'
		WHERE ID = ''',design.id,'''
	END'
  FROM (
  SELECT  ID -- USE newid() AS id if you need new ones
      ,Description
      ,CodePage
      ,Enabled
  FROM JscDatabaseDesigns) design

  Then using creative replacements to get the required results.
	
						
*/


/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/

Use LSPOSNET 

IF NOT EXISTS (SELECT
    *
  FROM JscDatabaseDesigns
  WHERE ID = 'F971A608-D577-4A0A-82A6-97D4B8AEEFD4')
BEGIN
  INSERT INTO JscDatabaseDesigns (ID, Description, CodePage, Enabled)
    VALUES ('F971A608-D577-4A0A-82A6-97D4B8AEEFD4', 'SiteManager', NULL, 1)
END
ELSE
BEGIN
  UPDATE JscDatabaseDesigns
  SET Description = 'SiteManager',
      CodePage = NULL,
      Enabled = 1
  WHERE ID = 'F971A608-D577-4A0A-82A6-97D4B8AEEFD4'
END