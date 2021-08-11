/* 
        Incident No.: N/A 
        Responsible : Höður Sigurdór Heiðarsson
        Sprint		: LS One 2013.1\July, August, September
        Date created: 17.10.13 
        Description	: Increase the capacity of NAME/DESCRIPTION columns of POSSTYLE, POSMENUHEADER and POSISTILLLAYOUT tables
*/ 
USE LSPOSNET 
GO 
IF EXISTS (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'POSSTYLE' and COLUMN_NAME = 'NAME')
BEGIN
	alter table POSSTYLE alter column NAME nvarchar(100)
END
GO

IF EXISTS (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'POSMENUHEADER' and COLUMN_NAME = 'DESCRIPTION')
BEGIN
	alter table POSMENUHEADER alter column DESCRIPTION nvarchar(100)
END
GO

IF EXISTS (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'POSISTILLLAYOUT' and COLUMN_NAME = 'NAME')
BEGIN
	alter table POSISTILLLAYOUT alter column NAME nvarchar(100)
END
GO