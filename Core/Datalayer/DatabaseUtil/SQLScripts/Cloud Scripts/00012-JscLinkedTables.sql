
USE LSPOSNET
GO
/****** Script for SelectTopNRows command from SSMS  ******/
/* SELECT 
  'IF NOT EXISTS(SELECT * FROM JscLinkedTables WHERE ID = '''+CONVERT(VARCHAR(50),design.id)+''' ) 
  BEGIN 
  INSERT INTO JscLinkedTables (
	 Id,
	FromTable,
	ToTable)
  VALUES(
'''+CONVERT(VARCHAR(50),design.id)+''',
'''+CONVERT(VARCHAR(50),FromTable)+''',
'''+CONVERT(VARCHAR(50), ToTable)+''')

	END
ELSE
	Begin
		Update  JscLinkedTables set			
		    FromTable='''+CONVERT(VARCHAR(50), FromTable)+''',
		    ToTable='''+CONVERT(VARCHAR(50), ToTable)+'''
		WHERE ID = '''+CONVERT(VARCHAR(50),design.id)+'''
	END'
  FROM (
 
  SELECT 
	Id,
	FromTable,
	ToTable
  FROM JscLinkedTables --where ID = 'F8238AF6-1019-4179-A1BC-0213037D4AAA'
  ) design
  */
  IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = 'BDC362F6-663A-4B42-AF3C-1D6A6B11B427')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('BDC362F6-663A-4B42-AF3C-1D6A6B11B427', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', 'AFE1AF88-33DA-4D46-95C8-D9D6387E6A12')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = 'AFE1AF88-33DA-4D46-95C8-D9D6387E6A12'
  WHERE ID = 'BDC362F6-663A-4B42-AF3C-1D6A6B11B427'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '5D18A6A0-1776-422B-9E37-22693C399944')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('5D18A6A0-1776-422B-9E37-22693C399944', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '67497997-9DC6-4519-B75B-86BE025C0BB9')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '67497997-9DC6-4519-B75B-86BE025C0BB9'
  WHERE ID = '5D18A6A0-1776-422B-9E37-22693C399944'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '506ADD77-2220-4C01-B0DB-23B0FA8899A1')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('506ADD77-2220-4C01-B0DB-23B0FA8899A1', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', 'D8F14F9A-172B-4A41-91C1-CC5B379F3D12')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = 'D8F14F9A-172B-4A41-91C1-CC5B379F3D12'
  WHERE ID = '506ADD77-2220-4C01-B0DB-23B0FA8899A1'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '51FA9D3E-53F5-4FC1-AA97-27D5E68E8618')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('51FA9D3E-53F5-4FC1-AA97-27D5E68E8618', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '1C44CC81-278C-4EB4-9B64-2075D6E2A012')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '1C44CC81-278C-4EB4-9B64-2075D6E2A012'
  WHERE ID = '51FA9D3E-53F5-4FC1-AA97-27D5E68E8618'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '00D0C840-8A55-4A3E-A7B9-37D50B6BFB5D')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('00D0C840-8A55-4A3E-A7B9-37D50B6BFB5D', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '89B26156-7358-447E-AD75-BF4692601AAE')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '89B26156-7358-447E-AD75-BF4692601AAE'
  WHERE ID = '00D0C840-8A55-4A3E-A7B9-37D50B6BFB5D'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = 'E1C90782-71A4-406E-8CA2-446C03B600A0')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('E1C90782-71A4-406E-8CA2-446C03B600A0', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '51C72CEF-794B-458C-A370-A91C924C506B')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '51C72CEF-794B-458C-A370-A91C924C506B'
  WHERE ID = 'E1C90782-71A4-406E-8CA2-446C03B600A0'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '1A5C3D32-2AD1-47E5-84C0-54270164D142')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('1A5C3D32-2AD1-47E5-84C0-54270164D142', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', 'F328E317-FD8E-4B95-A85E-DE835504C9F7')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = 'F328E317-FD8E-4B95-A85E-DE835504C9F7'
  WHERE ID = '1A5C3D32-2AD1-47E5-84C0-54270164D142'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '825FB8BE-5DF9-46D3-AE80-5808656910FE')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('825FB8BE-5DF9-46D3-AE80-5808656910FE', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '095F0A72-60C8-4B56-84F4-E40DDE021C84')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '095F0A72-60C8-4B56-84F4-E40DDE021C84'
  WHERE ID = '825FB8BE-5DF9-46D3-AE80-5808656910FE'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '1888FDAD-2911-4C98-BA14-62C34761B352')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('1888FDAD-2911-4C98-BA14-62C34761B352', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '0DF328A4-54EF-4BF8-8A9F-6772FA3155F9')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '0DF328A4-54EF-4BF8-8A9F-6772FA3155F9'
  WHERE ID = '1888FDAD-2911-4C98-BA14-62C34761B352'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '11105332-C7D8-4CB1-AEE0-635FFEC80EBF')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('11105332-C7D8-4CB1-AEE0-635FFEC80EBF', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '6D7D5825-4AC5-4345-8589-74AFA5897179')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '6D7D5825-4AC5-4345-8589-74AFA5897179'
  WHERE ID = '11105332-C7D8-4CB1-AEE0-635FFEC80EBF'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '9785318F-F6D8-4540-9A9F-76816286FBDE')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('9785318F-F6D8-4540-9A9F-76816286FBDE', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', 'CBD1E096-6F8E-4A56-A065-84E185335FE8')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = 'CBD1E096-6F8E-4A56-A065-84E185335FE8'
  WHERE ID = '9785318F-F6D8-4540-9A9F-76816286FBDE'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '43E7634C-15BB-4B52-98BA-7A0927EEA8AD')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('43E7634C-15BB-4B52-98BA-7A0927EEA8AD', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '1B31C396-F9B4-4D44-8570-3A96D319D09A')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '1B31C396-F9B4-4D44-8570-3A96D319D09A'
  WHERE ID = '43E7634C-15BB-4B52-98BA-7A0927EEA8AD'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = 'E4292096-B03F-4A02-B9F1-7E7E85C14A2E')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('E4292096-B03F-4A02-B9F1-7E7E85C14A2E', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '816964CC-F153-4F95-A488-DFD31522C090')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '816964CC-F153-4F95-A488-DFD31522C090'
  WHERE ID = 'E4292096-B03F-4A02-B9F1-7E7E85C14A2E'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '97305698-2C6D-419B-80CC-922E33225BD4')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('97305698-2C6D-419B-80CC-922E33225BD4', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '26829746-CD0C-4F2B-BA3A-42D4CC36C006')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '26829746-CD0C-4F2B-BA3A-42D4CC36C006'
  WHERE ID = '97305698-2C6D-419B-80CC-922E33225BD4'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '00292E48-19F6-495D-8B42-A43CF8F7C74E')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('00292E48-19F6-495D-8B42-A43CF8F7C74E', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '9637A056-A63B-4260-81E2-44E703914C47')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '9637A056-A63B-4260-81E2-44E703914C47'
  WHERE ID = '00292E48-19F6-495D-8B42-A43CF8F7C74E'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = 'C67DF5A6-C423-42D3-8979-A6AF803D5F9E')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('C67DF5A6-C423-42D3-8979-A6AF803D5F9E', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '6D5D0B78-A3C2-402C-962F-F1668704BC84')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '6D5D0B78-A3C2-402C-962F-F1668704BC84'
  WHERE ID = 'C67DF5A6-C423-42D3-8979-A6AF803D5F9E'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = 'D3A494B6-45FB-4C9A-AEBD-A979DC92B103')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('D3A494B6-45FB-4C9A-AEBD-A979DC92B103', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', '731387D9-6894-477D-876B-910E51E61E7F')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = '731387D9-6894-477D-876B-910E51E61E7F'
  WHERE ID = 'D3A494B6-45FB-4C9A-AEBD-A979DC92B103'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '3F2DEA08-ABD5-4836-996E-DCF7D207DE6F')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('3F2DEA08-ABD5-4836-996E-DCF7D207DE6F', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', 'C4DF66F1-BA4D-4B30-B1B7-9F0F88D780D8')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = 'C4DF66F1-BA4D-4B30-B1B7-9F0F88D780D8'
  WHERE ID = '3F2DEA08-ABD5-4836-996E-DCF7D207DE6F'
END
IF NOT EXISTS (SELECT
    *
  FROM JscLinkedTables
  WHERE ID = '89AFB649-4FC8-4D89-8965-F9F0DF308213')
BEGIN
  INSERT INTO JscLinkedTables (Id, FromTable, ToTable)
    VALUES ('89AFB649-4FC8-4D89-8965-F9F0DF308213', '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8', 'B1FF1A98-5CF6-4CFC-9B61-EA0C4D9DAE5E')
END
ELSE
BEGIN
  UPDATE JscLinkedTables
  SET FromTable = '41AFDEE9-1EF4-4CD4-8E0D-8A61B4B339F8',
      ToTable = 'B1FF1A98-5CF6-4CFC-9B61-EA0C4D9DAE5E'
  WHERE ID = '89AFB649-4FC8-4D89-8965-F9F0DF308213'
END