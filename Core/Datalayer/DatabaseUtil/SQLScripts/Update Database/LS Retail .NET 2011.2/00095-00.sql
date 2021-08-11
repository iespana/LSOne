﻿/*

	Incident No.	: 9327/9191/8647
	Responsible		: Tobias Helmer
	Sprint			: LS Retail.Net 2012\Sprint 1
	Date created	: 23.06.2011

	Description		: Change translations text

	Logic scripts   : No stored procedures added or changed
	
-- 	Tables affected	: PosisLanguageText
						
*/

USE LSPOSNET
GO

-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 3491)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '3491','Are you sure you want to perform a tender declaration?',0, '8.1.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Are you sure you want to perform a tender declaration?', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 3491
GO
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 3925)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '3925','Are you sure you want to perform a bank drop?',0, '8.1.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Are you sure you want to perform a bank drop?', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 3925
GO
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 3904)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '3904','Are you sure you want to reverse a bank drop?',0, '8.1.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Are you sure you want to reverse a bank drop?', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 3904
GO
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 3905)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '3905','Are you sure you want to reverse a safe drop?',0, '8.1.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Are you sure you want to reverse a safe drop?', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 3905
GO
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 3906)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '3906','Safe drop reversion',0, '8.1.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Safe drop reversion', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 3906
GO
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 3907)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '3907','Bank drop reversion',0, '8.1.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Bank drop reversion', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 3907
GO
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 3908)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '3908','Bank drop reversion',0, '8.1.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Please conclude the current transaction before reversing a Safe Drop operation.', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 3908
GO
-- IF NOT EXISTS (SELECT * FROM POSISLANGUAGETEXT WHERE LANGUAGEID = 'en-US' and TEXTID = 3909)
-- INSERT INTO POSISLANGUAGETEXT (LANGUAGEID, TEXTID, TEXT, ERRORTEXT, FIRSTINVERSION) VALUES ('en-US', '3909','Bank drop reversion',0, '8.1.0.0')
-- UPDATE POSISLANGUAGETEXT SET TEXT = 'Please conclude the current transaction before reversing a Bank Drop operation.', DateUpdated = GetDate() where LANGUAGEID = 'en-US' AND TEXTID = 3909
GO


IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 1213 and [DATAAREAID] = 'LSR')
INSERT POSISOPERATIONS(OPERATIONID,OPERATIONNAME,CHECKUSERACCESS,USEROPERATION,DATAAREAID) VALUES('1213','Safe Drop Reversal','1','1','LSR')
GO
IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 1214 and [DATAAREAID] = 'LSR')
INSERT POSISOPERATIONS(OPERATIONID,OPERATIONNAME,CHECKUSERACCESS,USEROPERATION,DATAAREAID) VALUES('1214','Bank Drop Reversal','1','1','LSR')
GO
