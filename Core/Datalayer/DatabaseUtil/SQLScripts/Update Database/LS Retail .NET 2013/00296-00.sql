

--   Changed By       : Florin Frentiu
--   Date             : 31.10.2016
--   Description      : Scripts of creating the system styles were moved in  ImportData/SystemData/001 - SystemStyles.sql


--USE LSPOSNET

--GO

--IF NOT EXISTS (SELECT * FROM POSSTYLE WHERE NAME = 'SystemNumPadButtonStyle')
--BEGIN
--INSERT INTO POSSTYLE (ID, NAME, FONTNAME, FONTSIZE, FONTBOLD, FORECOLOR, BACKCOLOR, FONTITALIC, FONTCHARSET, DATAAREAID, BACKCOLOR2, GRADIENTMODE, SHAPE)
--     VALUES (2000000, 'SystemNumPadButtonStyle', 'Microsoft Sans Serif', 24, 0, -16777216, -1, 0, 0, 'LSR', -655366, 3, 0)
--END
--GO

--IF NOT EXISTS (SELECT * FROM POSSTYLE WHERE NAME = 'SystemNumPadDigitButtonStyle')
--BEGIN
--INSERT INTO POSSTYLE (ID, NAME, FONTNAME, FONTSIZE, FONTBOLD, FORECOLOR, BACKCOLOR, FONTITALIC, FONTCHARSET, DATAAREAID, BACKCOLOR2, GRADIENTMODE, SHAPE)
--     VALUES (2000001, 'SystemNumPadDigitButtonStyle', 'Microsoft Sans Serif', 24, 0, -16777216, -1, 0, 0, 'LSR', -655366, 3, 0)
--END

--GO