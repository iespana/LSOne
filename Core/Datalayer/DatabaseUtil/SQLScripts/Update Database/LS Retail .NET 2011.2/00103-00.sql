
/*

	Incident No.	: 11498
	Responsible		: Marý B. Steingrímsdóttir
	Sprint			: LS Retail .NET 2012/Frigg
	Date created	: 22.8.2011

	Description		: Adds a new text in translation table

	Logic scripts   : No stored procedures added or changed
	
-- 	Tables affected	: PosisLanguageText - data added
						
*/

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 2850)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '2850','Screen selected in visual profile #1 cannot be found. Default screen selected.',0, '8.2.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Screen selected in visual profile #1 cannot be found. Default screen selected.', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 2850
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'is-IS' and TEXTID = 2850)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('is-IS', '2850','Skjár sem er valinn í skjá prófíl #1 finnst ekki. Aðalskjár tölvunnar verður notaður í staðinn.',0, '8.2.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Screen selected in visual profile #1 cannot be found. Default screen selected.', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 2850
