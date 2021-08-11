
/*
	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Date created	: 09.07.2014

	Description		: Store procedure added to delete columns that have a constraint tied to them	
*/

USE LSPOSNET

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spDeleteColumnAndConstaint')
   drop procedure spDeleteColumnAndConstaint
GO
   
CREATE PROCEDURE spDeleteColumnAndConstaint 
    @TABLE nvarchar(100), 
    @COLUMN nvarchar(100) 
AS 
	IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TABLE AND COLUMN_NAME = @COLUMN)
	BEGIN
		SET NOCOUNT ON;
		DECLARE @constraint NVARCHAR(100)
		SELECT TOP 1 @constraint = OBJECT_NAME([default_object_id]) FROM SYS.COLUMNS
		WHERE [object_id] = OBJECT_ID(@TABLE) AND [name] = @COLUMN;
		IF(@constraint IS NOT NULL)
		BEGIN
			EXEC('ALTER TABLE ' + @TABLE + ' DROP CONSTRAINT ' + @constraint)
		END

		EXEC('ALTER TABLE ' + @TABLE + ' DROP COLUMN ' + @COLUMN)
	END
GO