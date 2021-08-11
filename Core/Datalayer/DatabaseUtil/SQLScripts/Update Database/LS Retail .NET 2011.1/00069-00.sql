
/*

	Incident No.	: 9192
	Responsible		: Marý B Steingrímsdóttir
	Sprint			: LS Retail.Net 2011.1\Sprint 1
	Date created	: 31.03.2011

	Description		: Add translations texts

	Logic scripts   : No stored procedures added or changed
	
-- 	Tables affected	: PosisLanguageText - add textids
						
*/

USE LSPOSNET
GO

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 218)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '218','Table cannot be transferred between hospitality types.',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Table cannot be transferred between hospitality types.', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 218
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 218)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '218','Ekki er hægt að færa borð á milli veitingahúsa',0, '8.0.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Ekki er hægt að færa borð á milli veitingahúsa', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 218
GO
