
USE LSPOSNET
GO
/****** Script for SelectTopNRows command from SSMS  ******/
/* SELECT 
  'IF NOT EXISTS(SELECT * FROM JscSubJobFromTableFilters WHERE ID = '''+CONVERT(VARCHAR(50),design.id)+''' ) 
  BEGIN 
  INSERT INTO JscSubJobFromTableFilters (
	Id,
	SubJob,
	Field,
	FilterType,
	Value1,
	Value2,
	ApplyFilter)
  VALUES(
'''+CONVERT(VARCHAR(50),design.id)+''',
'''+CONVERT(VARCHAR(50),SubJob)+''',
'''+CONVERT(VARCHAR(50), Field)+''',
'+CONVERT(VARCHAR(50), FilterType)+',
'+ CASE When Value1 IS not null then ''''+ CONVERT(VARCHAR(50),Value1)+'''' else 'NULL' END +',
'+ CASE When Value2 IS not null then ''''+ CONVERT(VARCHAR(50),Value2)+'''' else 'NULL' END +',
'''+CONVERT(VARCHAR(50), ApplyFilter)+''')

	END
ELSE
	Begin
		Update  JscSubJobFromTableFilters set			
		    SubJob='''+CONVERT(VARCHAR(50), SubJob)+''',
		    Field='''+CONVERT(VARCHAR(50), Field)+''',
		    FilterType='+CONVERT(VARCHAR(50), FilterType)+',
		    Value1='+ CASE When Value1 IS not null then ''''+CONVERT(VARCHAR(50),Value1) +'''' else 'NULL' END +',
			Value2='+ CASE When Value2 IS not null then ''''+CONVERT(VARCHAR(50),Value2)+'''' else 'NULL' END +',
		    ApplyFilter='''+CONVERT(VARCHAR(50), ApplyFilter)+'''
		WHERE ID = '''+CONVERT(VARCHAR(50),design.id)+'''
	END'
  FROM (
    
  SELECT 
	Id,
	SubJob,
	Field,
	FilterType,
	Value1,
	Value2,
	ApplyFilter
  FROM JscSubJobFromTableFilters--where ID = 'F8238AF6-1019-4179-A1BC-0213037D4AAA'
  ) design
  */
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobFromTableFilters
  WHERE ID = 'C880D036-E248-4E31-B5DD-181FD21F0311')
BEGIN
  INSERT INTO JscSubJobFromTableFilters (Id, SubJob, Field, FilterType, Value1, Value2, ApplyFilter)
    VALUES ('C880D036-E248-4E31-B5DD-181FD21F0311', '8C575439-1A79-4D9D-A9FB-7EACD4D9F5EC', '5E67EF97-F0A8-49F4-8517-033C56061A14', 1, 'D-60', '', '3')
END
ELSE
BEGIN
  UPDATE JscSubJobFromTableFilters
  SET SubJob = '8C575439-1A79-4D9D-A9FB-7EACD4D9F5EC',
      Field = '5E67EF97-F0A8-49F4-8517-033C56061A14',
      FilterType = 1,
      Value1 = 'D-60',
      Value2 = '',
      ApplyFilter = '3'
  WHERE ID = 'C880D036-E248-4E31-B5DD-181FD21F0311'
END
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobFromTableFilters
  WHERE ID = '02A849B3-840D-4A45-9AD3-9E20B87DE333')
BEGIN
  INSERT INTO JscSubJobFromTableFilters (Id, SubJob, Field, FilterType, Value1, Value2, ApplyFilter)
    VALUES ('02A849B3-840D-4A45-9AD3-9E20B87DE333', '2B0BEAA3-1DDA-48C9-BD1F-CB40A71B0D55', '11830199-43B2-4251-B0C7-E845BFBA5944', 1, 'D-60', '', '3')
END
ELSE
BEGIN
  UPDATE JscSubJobFromTableFilters
  SET SubJob = '2B0BEAA3-1DDA-48C9-BD1F-CB40A71B0D55',
      Field = '11830199-43B2-4251-B0C7-E845BFBA5944',
      FilterType = 1,
      Value1 = 'D-60',
      Value2 = '',
      ApplyFilter = '3'
  WHERE ID = '02A849B3-840D-4A45-9AD3-9E20B87DE333'
END
IF NOT EXISTS (SELECT
    *
  FROM JscSubJobFromTableFilters
  WHERE ID = '2B7DC202-6E7E-4761-986D-BA312C884676')
BEGIN
  INSERT INTO JscSubJobFromTableFilters (Id, SubJob, Field, FilterType, Value1, Value2, ApplyFilter)
    VALUES ('2B7DC202-6E7E-4761-986D-BA312C884676', '8D975765-CE94-4D91-940A-708C44483B96', '46E8CA91-237D-4CA6-B76F-5B4D01081380', 2, '', '', '0')
END
ELSE
BEGIN
  UPDATE JscSubJobFromTableFilters
  SET SubJob = '8D975765-CE94-4D91-940A-708C44483B96',
      Field = '46E8CA91-237D-4CA6-B76F-5B4D01081380',
      FilterType = 2,
      Value1 = '',
      Value2 = '',
      ApplyFilter = '0'
  WHERE ID = '2B7DC202-6E7E-4761-986D-BA312C884676'
END