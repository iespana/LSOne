
/*

	Incident No.	: 9328
	Responsible		: Marý B. Steingrímsdóttir
	Sprint			: LS Retail.Net 2011.1\Sprint 1\POS Team
	Date created	: 29.03.2011

	Description		: New field added to table PosisHospitalityDiningTables and new texts added

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PosisHospitalityDiningTables - new field added
-- 					  PosisLanguageText - new text id's added
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSISHOSPITALITYDININGTABLES') AND NAME='TERMINALID')
ALTER TABLE dbo.POSISHOSPITALITYDININGTABLES ADD TERMINALID nvarchar(20) NULL
GO

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 214)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '214','The selected table is locked by another terminal',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'The selected table is locked by another terminal', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 214
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 214)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '214','Valið borð hefur verið læst af öðrum kassa',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Valið borð hefur verið læst af öðrum kassa', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 214
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 217)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '217','Locked',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Locked', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 217
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 217)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '217','Læst',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Læst', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 217

GO