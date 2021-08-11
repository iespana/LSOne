/*
	
	Responsible		: <Partner name>	
	Date created	: <Date created>

	Description		: <Which styles have been overwritten/changed>
*/

USE LSPOSNET

/* ************************************************************************************** 
 In this script any and all changes that are needed to the system styles should be done and maintained.
 As an example:
	All system styles that should display negatvie numbers should have a blue font (instead of red) 
	The font for the Dual display styles should have font size 16
	..and etc.

This script will never be updated, added to or used by the LS One product team 
so this script can be moved between development pack versions without any merging

SAMPLE CODE:

IF NOT EXISTS (SELECT * FROM POSSTYLE WHERE ID = 123456)
BEGIN
INSERT INTO POSSTYLE (ID, NAME, FONTNAME, FONTSIZE, FONTBOLD, FORECOLOR, BACKCOLOR, FONTITALIC, FONTCHARSET, DATAAREAID, BACKCOLOR2, GRADIENTMODE, SHAPE, SYSTEMSTYLE, STYLETYPE)
     VALUES (123456, 'My style', '', 15, 1, -16777216, -1, 0, 0, 'LSR', -1, 0, 0, 0, 0)
END

**************************************************************************************  */
