
/*

	Incident No.	: 11714
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .NET 2012/Tyr
	Date created	: 6.9.2011

	Description		: Add text strings to language table

	Logic scripts   : No stored procedures added or changed
	
-- 	Tables affected	: PosisLanguageText - data added
						
*/

USE LSPOSNET
GO

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 4665)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '4665','No menu types available for this restaurant',0, '8.2.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'No menu types available for this restaurant', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 4665
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 4665)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '4665','Engar gerðir hafa verið settar upp fyrir þennan veitingastað',0, '8.2.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Engar gerðir hafa verið settar upp fyrir þennan veitingastað', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 4665

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 4644)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '4644','No menu types available for this restaurant',0, '8.2.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'No menu types available for this restaurant', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 4644
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 4644)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '4644','Engar gerðir hafa verið settar upp fyrir þennan veitingastað',0, '8.2.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Engar gerðir hafa verið settar upp fyrir þennan veitingastað', DateUpdated = GetDate() where LANGUAGEID = 'is-IS' AND TEXTID = 4644
