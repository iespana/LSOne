/*

	Incident No.	: 25670
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\July, August, September
	Date created	: 7.10.2013

	Description		: Create styles for dual display grid control. Font names are deliberately left blank, styles are only created but will not be used
	                  since there is no font info


   Changed By       : Florin Frentiu
   Date             : 31.10.2016
   Description      : Scripts of creating the system styles were moved in  ImportData/SystemData/001 - SystemStyles.sql
*/

--USE LSPOSNET

--IF NOT EXISTS (SELECT * FROM POSSTYLE WHERE NAME = 'DualDisplayTotal')
--BEGIN
--INSERT INTO POSSTYLE (ID, NAME, FONTNAME, FONTSIZE, FONTBOLD, FORECOLOR, BACKCOLOR, FONTITALIC, FONTCHARSET, DATAAREAID, BACKCOLOR2, GRADIENTMODE, SHAPE)
--     VALUES (2000010, 'DualDisplayTotal', '', 15, 1, -16777216, -1, 0, 0, 'LSR', -1, 0, 0)
--END
--GO
--IF NOT EXISTS (SELECT * FROM POSSTYLE WHERE NAME = 'DualDisplayLine')
--BEGIN
--INSERT INTO POSSTYLE (ID, NAME, FONTNAME, FONTSIZE, FONTBOLD, FORECOLOR, BACKCOLOR, FONTITALIC, FONTCHARSET, DATAAREAID, BACKCOLOR2, GRADIENTMODE, SHAPE)
--     VALUES (2000011, 'DualDisplayLine', '', 12, 1, -16777216, -1, 0, 0, 'LSR', -1, 0, 0)
--END
--GO
--IF NOT EXISTS (SELECT * FROM POSSTYLE WHERE NAME = 'DualDisplayLineSub')
--BEGIN
--INSERT INTO POSSTYLE (ID, NAME, FONTNAME, FONTSIZE, FONTBOLD, FORECOLOR, BACKCOLOR, FONTITALIC, FONTCHARSET, DATAAREAID, BACKCOLOR2, GRADIENTMODE, SHAPE)
--     VALUES (2000012, 'DualDisplayLineSub', '', 9, 1, -16777216, -1, 0, 0, 'LSR', -1, 0, 0)
--END