﻿/*
	Incident No.	: ONE-5555
	Responsible		: Adrian Chiorean
	Sprint			: Färgrik
	Date created	: 23.05.2017

	Description		: Add Open Drawer form type and receipt
*/

USE LSPOSNET

IF NOT EXISTS (SELECT ID FROM POSFORMTYPE WHERE ID = 'F01461F2-3CFB-4652-9483-9F784653E4BD')
BEGIN
	INSERT INTO POSFORMTYPE (ID, DESCRIPTION, SYSTEMTYPE, DATAAREAID)
	VALUES ('F01461F2-3CFB-4652-9483-9F784653E4BD', 'Open drawer', 37, 'LSR')
END

IF NOT EXISTS (SELECT ID FROM POSISFORMLAYOUT WHERE ID = 'SYS100028')
BEGIN
	INSERT INTO POSISFORMLAYOUT (ID, DATAAREAID, DEFAULTFORMWIDTH, DESCRIPTION, FOOTERXML, FORMTYPEID, HEADERXML, LINECOUNTPRPAGE, LINESXML, PRINTASSLIP, PRINTBEHAVIOUR, PROMPTQUESTION, PROMPTTEXT, TITLE, UPPERCASE, USEWINDOWSPRINTER, WINDOWSPRINTERNAME, ISSYSTEMLAYOUT) VALUES
	('SYS100028','LSR','56','Open drawer','3C6865616465723E0D0A20203C6C696E65206E723D223030222049443D2243524C4622202F3E0D0A3C2F6865616465723E000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000','F01461F2-3CFB-4652-9483-9F784653E4BD','3C6865616465723E0D0A20203C6C696E65206E723D223030222049443D2243524C4622202F3E0D0A20203C6C696E65206E723D223031222049443D2243524C4622202F3E0D0A20203C6C696E65206E723D223032222049443D224E6F6E5F5661726961626C6522204E756D6265726F66436F6C756D6E733D223536223E0D0A202020203C63686172706F73206E723D22303222207661726961626C653D22222076616C75653D224F70656E206472617765722072656365697074222066696C6C3D222022207072656669783D2222206C656E6774683D223535222076616C69676E3D2263656E7472652220466F6E745374796C653D22362220436F6E646974696F6E616C4964656E7469666965723D2222202F3E0D0A20203C2F6C696E653E0D0A20203C6C696E65206E723D223033222049443D2243524C4622202F3E0D0A20203C6C696E65206E723D223034222049443D224E6F6E5F5661726961626C6522204E756D6265726F66436F6C756D6E733D223536223E0D0A202020203C63686172706F73206E723D22313022207661726961626C653D22222076616C75653D224F70657261746F722049443A222066696C6C3D222022207072656669783D2222206C656E6774683D223135222076616C69676E3D226C6566742220466F6E745374796C653D22302220436F6E646974696F6E616C4964656E7469666965723D2222202F3E0D0A202020203C63686172706F73206E723D22323522207661726961626C653D224F70657261746F72206E616D65206F6E2072656365697074222076616C75653D22222066696C6C3D222022207072656669783D2222206C656E6774683D223332222076616C69676E3D226C6566742220466F6E745374796C653D22302220436F6E646974696F6E616C4964656E7469666965723D2222202F3E0D0A20203C2F6C696E653E0D0A20203C6C696E65206E723D223035222049443D224E6F6E5F5661726961626C6522204E756D6265726F66436F6C756D6E733D223536223E0D0A202020203C63686172706F73206E723D22313022207661726961626C653D22222076616C75653D2253746F72653A222066696C6C3D222022207072656669783D2222206C656E6774683D223135222076616C69676E3D226C6566742220466F6E745374796C653D22302220436F6E646974696F6E616C4964656E7469666965723D2222202F3E0D0A202020203C63686172706F73206E723D22323522207661726961626C653D2253746F7265206E616D65222076616C75653D22222066696C6C3D222022207072656669783D2222206C656E6774683D223332222076616C69676E3D226C6566742220466F6E745374796C653D22302220436F6E646974696F6E616C4964656E7469666965723D2222202F3E0D0A20203C2F6C696E653E0D0A20203C6C696E65206E723D223036222049443D2243524C4622202F3E0D0A20203C6C696E65206E723D223037222049443D224E6F6E5F5661726961626C6522204E756D6265726F66436F6C756D6E733D223536223E0D0A202020203C63686172706F73206E723D22313022207661726961626C653D22222076616C75653D22446174653A222066696C6C3D222022207072656669783D2222206C656E6774683D223135222076616C69676E3D226C6566742220466F6E745374796C653D22302220436F6E646974696F6E616C4964656E7469666965723D2222202F3E0D0A202020203C63686172706F73206E723D22323522207661726961626C653D2244617465222076616C75653D22222066696C6C3D222022207072656669783D2222206C656E6774683D223130222076616C69676E3D2272696768742220466F6E745374796C653D22302220436F6E646974696F6E616C4964656E7469666965723D2222202F3E0D0A20203C2F6C696E653E0D0A20203C6C696E65206E723D223038222049443D224E6F6E5F5661726961626C6522204E756D6265726F66436F6C756D6E733D223536223E0D0A202020203C63686172706F73206E723D22313022207661726961626C653D22222076616C75653D2254696D653A222066696C6C3D222022207072656669783D2222206C656E6774683D223135222076616C69676E3D226C6566742220466F6E745374796C653D22302220436F6E646974696F6E616C4964656E7469666965723D2222202F3E0D0A202020203C63686172706F73206E723D22323522207661726961626C653D2254696D65323448222076616C75653D22222066696C6C3D222022207072656669783D2222206C656E6774683D223130222076616C69676E3D2272696768742220466F6E745374796C653D22302220436F6E646974696F6E616C4964656E7469666965723D2222202F3E0D0A20203C2F6C696E653E0D0A20203C6C696E65206E723D223039222049443D2243524C4622202F3E0D0A20203C6C696E65206E723D223130222049443D224E6F6E5F5661726961626C6522204E756D6265726F66436F6C756D6E733D223536223E0D0A202020203C63686172706F73206E723D22303222207661726961626C653D22222076616C75653D22222066696C6C3D222D22207072656669783D2222206C656E6774683D223535222076616C69676E3D226C6566742220466F6E745374796C653D22302220436F6E646974696F6E616C4964656E7469666965723D2222202F3E0D0A20203C2F6C696E653E0D0A20203C6C696E65206E723D223131222049443D2243524C4622202F3E0D0A3C2F6865616465723E000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000','0','3C6865616465723E0D0A20203C6C696E65206E723D223030222049443D2243524C4622202F3E0D0A3C2F6865616465723E000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000','0','0','0',NULL,'Open drawer','0','0',NULL,1);
END

IF (0 <> (SELECT COUNT(*) FROM [dbo].[POSFORMPROFILELINES] WHERE [PROFILEID] = '0C7D790C-096C-4ED5-94B5-6B9814C87A46'))
IF NOT EXISTS (SELECT 1 FROM POSFORMPROFILELINES WHERE PROFILEID = '0C7D790C-096C-4ED5-94B5-6B9814C87A46' AND FORMLAYOUTID = 'SYS100028' AND FORMTYPEID = 'F01461F2-3CFB-4652-9483-9F784653E4BD')
BEGIN
	INSERT INTO [POSFORMPROFILELINES] (PROFILEID, FORMTYPEID, FORMLAYOUTID, DATAAREAID, DESCRIPTION, ISSYSTEMPROFILELINE)
	VALUES ('0C7D790C-096C-4ED5-94B5-6B9814C87A46', 'F01461F2-3CFB-4652-9483-9F784653E4BD', 'SYS100028', 'LSR', '', 1)
END
GO