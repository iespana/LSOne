/*

	Incident No.	: 25670
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2014 - Stratus
	Date created	: 06.01.13

	Description		: Create styles for pos total amount control. Font names are deliberately left blank, styles are only created but will not be used
	                  since there is no font info


    Changed By       : Florin Frentiu
    Date             : 31.10.2016
    Description      : Scripts of creating the system styles were moved in  ImportData/SystemData/001 - SystemStyles.sql
*/

--USE LSPOSNET

--IF NOT EXISTS (SELECT * FROM POSSTYLE WHERE NAME = 'PosTotalsLabels')
--BEGIN
--INSERT INTO POSSTYLE (ID, NAME, FONTNAME, FONTSIZE, FONTBOLD, FORECOLOR, BACKCOLOR, FONTITALIC, FONTCHARSET, DATAAREAID, BACKCOLOR2, GRADIENTMODE, SHAPE, SYSTEMSTYLE)
--     VALUES (2000013, 'PosTotalsLabels', '', 9, 0, -16777216, -1, 0, 0, 'LSR', -1, 0, 0, 1)
--END
--GO
--IF NOT EXISTS (SELECT * FROM POSSTYLE WHERE NAME = 'PosTotalsValues')
--BEGIN
--INSERT INTO POSSTYLE (ID, NAME, FONTNAME, FONTSIZE, FONTBOLD, FORECOLOR, BACKCOLOR, FONTITALIC, FONTCHARSET, DATAAREAID, BACKCOLOR2, GRADIENTMODE, SHAPE, SYSTEMSTYLE)
--     VALUES (2000014, 'PosTotalsValues', '', 9, 0, -16777216, -1, 0, 0, 'LSR', -1, 0, 0, 1)
--END
--GO
--IF NOT EXISTS (SELECT * FROM POSSTYLE WHERE NAME = 'PosTotalsBalanceValue')
--BEGIN
--INSERT INTO POSSTYLE (ID, NAME, FONTNAME, FONTSIZE, FONTBOLD, FORECOLOR, BACKCOLOR, FONTITALIC, FONTCHARSET, DATAAREAID, BACKCOLOR2, GRADIENTMODE, SHAPE, SYSTEMSTYLE)
--     VALUES (2000015, 'PosTotalsBalanceValue', '', 16, 1, -16777216, -1, 0, 0, 'LSR', -1, 0, 0, 1)
--END