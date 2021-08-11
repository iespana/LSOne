﻿
/*

	Incident No.	: 9327/9191/8647
	Responsible		: Marý B Steingrímsdóttir
	Sprint			: LS Retail.Net 2011.1\Sprint 1
	Date created	: 30.03.2011

	Description		: Add translations texts

	Logic scripts   : No stored procedures added or changed
	
-- 	Tables affected	: PosisLanguageText - add textids
						
*/

USE LSPOSNET
GO

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 4642)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '4642','Select a menu type',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Select a menu type for the item', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 4642
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 4642)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '4642','Veldu gerð fyrir vöruna',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Veldu gerð fyrir vöruna sem er valin', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 4642
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 4643)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '4643','Available Menu Types',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Available Menu Types', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 4643
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 4643)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '4643','Gerðir sem hægt er að velja',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Gerðir sem hægt er að velja', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 4643
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 4663)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '4663','Change the menu type',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Change the menu type', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 4663
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 4663)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '4663','Breyttu gerðinni fyrir vöruna',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Breyttu gerðinni fyrir vöruna', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 4663
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 4664)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '4664','Available Menu Types',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Available Menu Types', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 4664
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 4664)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '4664','Gerðir sem hægt er að velja',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Gerðir sem hægt er að velja', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 4664
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 66017)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '66017','Select a menu type for printing',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Select a menu type for printing', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 66017
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 66017)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '66017','Veldu gerð til að prenta út',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Veldu gerð til að prenta út', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 66017
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 66018)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '66018','Available Menu Types',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Available Menu Types', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 66018
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 66018)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '66018','Gerðir sem hægt er að velja',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Gerðir sem hægt er að velja', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 66018
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 215)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '215','The same table cannot be selected as from and to table',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'The same table cannot be selected as from and to table', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 215
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 215)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '215','Ekki er hægt að velja sama borðið til þegar það er verið að færa á milli borða',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Ekki er hægt að velja sama borðið til þegar það er verið að færa á milli borða', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 215

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 216)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '216','There are no items to transfer on this table',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'There are no items to transfer on this table', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 216
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 216)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '216','Það eru engar vörur á þessu borði sem er hægt að færa',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Það eru engar vörur á þessu borði sem er hægt að færa', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 216
GO

