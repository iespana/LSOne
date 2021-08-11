
/*
	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2014
	Date created	: 10.03.2014

	Description		: Added a new form type
						
*/
USE LSPOSNET
GO

IF (0 = (SELECT count(*) FROM [dbo].[POSFORMTYPE] WHERE [DATAAREAID] = 'LSR' AND [ID] = '364f2b61-03f1-49ab-8105-d6fceae75a94'))
BEGIN
	INSERT INTO [dbo].[POSFORMTYPE] (ID, DESCRIPTION, SYSTEMTYPE, DATAAREAID) values ('364f2b61-03f1-49ab-8105-d6fceae75a94','Suspended transaction header',29,'LSR')
END

GO
