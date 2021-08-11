
/*
-- Use this document to create new Site Manager permissions. Remove the outer comments to get the document working.

-- Do not change this section
-- =================================================================================================================================================================
Use LSPOSNET 

declare @dataAreaID nvarchar(10)
set @dataAreaID = 'LSR'
-- ==================================================================================================================================================================


-- Permission line. YOU NEED TO CHANGE THIS LINE. See the sample line below and extra explanations as well.
exec spSECURITY_AddPermission_1_0 @dataAreaID,'Localization GUID','English permission description','GROUP GUID','SortString','GUID that you need in code',0

-- SAMPLE LINE. 
--                                                Localization GUID                      Description      GROUP GUID                             Sort     Guid that you need in code
-- exec spSECURITY_AddPermission_1_0 @dataAreaID,'322180d0-cdfd-11de-8a39-0800200c9a66','New permission','1ddeef60-d9ae-11de-8a39-0800200c9a66','SEC020','CAA3E23D-C7B6-4E3C-A607-3175862EFC64',0

-- Creates a 'New permission' in group 'General'. You would then have to localize it which is done in '00007-Partner SC Permission Localize.sql'
-- and then use the last GUID in the Store Controller code like this : PluginEntry.DataModel.HasPermission("CAA3E23D-C7B6-4E3C-A607-3175862EFC64")

/*
Explanation:
Localization GUID : If you want to translate the description of your permission, you will need this GUID. See '00007-Partner SC Permission Localize.sql' for details
English permission description : Description of permission that shows up in the Store Controller. You can translate this text. 
								 See '00007-Partner SC Permission Localize.sql' for details
GROUP GUID : The GUID of the permission group you want your permisson to belong to. You simply copy a GUID from GROUP GUIDS below depending on which group you want
GUID that you need in code : To use the permission in the Store Controller you use this GUID. The call 'PluginEntry.DataModel.HasPermission("GUID")' will 
						     return a boolean telling wether the user has the permission or not.
*/

/*
GROUP GUIDS
General :						'1ddeef60-d9ae-11de-8a39-0800200c9a66'
User and security management :	'0ca8e620-e997-11da-8ad9-0800200c9a66'
System Administration :			'808ed9f0-e997-11da-8ad9-0800200c9a66'
Profiles :						'df4afbd0-b35c-11de-8a39-0800200c9a66'
Item Master :					'78535d50-04e7-11df-8a39-0800200c9a66'
Hospitality :					'a54c7dc0-a922-11df-94e2-0800200c9a66'
Inventory :						'32246200-f0bb-11df-98cf-0800200c9a66'
Employee and Customer cards :	'2c562010-f660-11e0-be50-0800200c9a66'
*/

*/																				