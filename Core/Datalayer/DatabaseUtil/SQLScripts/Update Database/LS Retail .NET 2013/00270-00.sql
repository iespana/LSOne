﻿/*

	Incident No.	: XXX
	Responsible		: Sigfus Johannesson
	Sprint			: LS Retail .NET 2013\Jupiter
	Date created	: 12.12.2012

	Description		: Permission alterations
	
	
	Tables affected	: PERMISSIONS
						
*/
USE LSPOSNET
GO

  --Delete POS users
DELETE FROM PERMISSIONS WHERE GUID = '43479C00-CDFD-11DE-8A39-0800200C9A66' 
DELETE FROM PERMISSIONSLOCALIZATION WHERE PermissionGUID = '43479C00-CDFD-11DE-8A39-0800200C9A66'

  --Edit POS users
DELETE FROM PERMISSIONS WHERE GUID = '3ED26790-CDFD-11DE-8A39-0800200C9A66' 
DELETE FROM PERMISSIONSLOCALIZATION WHERE PermissionGUID = '3ED26790-CDFD-11DE-8A39-0800200C9A66'

  --Create new POS users
DELETE FROM PERMISSIONS WHERE GUID = '322180D0-CDFD-11DE-8A39-0800200C9A66' 
DELETE FROM PERMISSIONSLOCALIZATION WHERE PermissionGUID = '322180D0-CDFD-11DE-8A39-0800200C9A66'

