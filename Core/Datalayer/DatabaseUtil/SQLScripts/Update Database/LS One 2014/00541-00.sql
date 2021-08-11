
/*
	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Date created	: 17.07.2014

	Description		: Remove unused columns from inventory adjustments and stock counting tables
*/

USE LSPOSNET

EXEC spDeleteColumnAndConstaint @TABLE='INVENTJOURNALTABLE', @COLUMN='EMPLID'
EXEC spDeleteColumnAndConstaint @TABLE='INVENTJOURNALTABLE', @COLUMN='POSTEDUSERID'

GO