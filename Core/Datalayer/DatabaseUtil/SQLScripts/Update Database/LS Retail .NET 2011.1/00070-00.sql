
/*

	Incident No.	: 9448/9472
	Responsible		: Marý B Steingrímsdóttir
	Sprint			: LS Retail.Net 2011.1\Sprint 1
	Date created	: 01.04.2011

	Description		: Add translations texts

	Logic scripts   : No stored procedures added or changed
	
-- 	Tables affected	: PosisLanguageText - add textids
						
*/

USE LSPOSNET
GO

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 4685)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '4685','Station printing has been disabled. No items will be printed',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Station printing has been disabled. No items will be printed', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 4685
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 4685)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '4685','Prentun hefur verið gerð óvirk. Engar vörur verða prentaðar',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Prentun hefur verið gerð óvirk. Engar vörur verða prentaðar', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 4685

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 66019)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '66019','Print all',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Print all', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 66019
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 66019)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '66019','Prenta allt',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Prenta allt', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 66019

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 219)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '219','Connection to LS Store Server has been restored',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Connection to LS Store Server has been restored', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 219
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 219)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '219','LS Store Server tengingin er komin upp aftur',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'LS Store Server tengingin er komin upp aftur', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 219
GO