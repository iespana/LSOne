/*

	Incident No.	: N/A
	Responsible		: Tobias Helmer
	Sprint			: Sprint 5
	Date created	: 23.02.2011

	Description		: Text id missing

	Logic scripts   : No stored procedures added or changed
	
-- 	Tables affected	: PosisLanguageText - data added
						
*/


-- LanguageID's for the frmManagerAccess dialog
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 1331)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '1331','Operator does not exist.',0, '7.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Operator does not exist.', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 1331
