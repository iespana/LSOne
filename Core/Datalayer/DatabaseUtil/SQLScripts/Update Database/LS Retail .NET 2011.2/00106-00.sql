/*

	Incident No.	: 
	Responsible		: Tobias Helmer
	Sprint			: 
	Date created	: 31.08.2011

	Description		: Text label add

	Logic scripts   : No stored procedures added or changed
	
-- 	Tables affected	: POSISLANGUAGETEXT					  
						
*/


USE LSPOSNET
GO

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 7038)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '7038','Currency report',0, '8.2.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Currency report', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 7038

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 4542)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '4542','Operation will not continue since the POS is in return state',0, '8.2.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Operation will not continue since the POS is in return state', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 4542