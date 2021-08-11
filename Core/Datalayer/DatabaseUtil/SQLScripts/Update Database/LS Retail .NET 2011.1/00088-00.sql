
/*

	Incident No.	: 9955
	Responsible		: Marý B. Steingrímsdóttir
	Sprint			: N/A
	Date created	: 23.05.2011

	Description		: Add new translation strings

	Logic scripts   : No stored procedures added or changed
	
-- 	Tables affected	: PosisLanguageText - data added
						
*/

USE LSPOSNET
GO

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 3214)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '3214','Critical error found during startup. Logon not allowed',0, '8.1.0.0000')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Critical error found during startup. Logon not allowed', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 3214
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 3214)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '3214','Alvarleg villa fannst við ræsingu. Innskráning er ekki leyfð',0, '8.1.0.0000')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Alvarleg villa fannst við ræsingu. Innskráning er ekki leyfð', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 3214
GO
