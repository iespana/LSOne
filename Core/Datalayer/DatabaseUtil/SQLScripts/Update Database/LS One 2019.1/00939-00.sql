/*
	Incident No.	: ONE-10095
	Responsible		: Helgi Rúnar Gunnarsson
	Sprint			: Vega
	Date created	: 16.09.2019

	Description		: Add columns LIMITATIONDISPLAYTYPE and DISPLAYLIMITATIONSTOTALSINPOS to POSFUNCTIONALITYPROFILE
*/
USE LSPOSNET

-- ONE-10699: when the user updates the existing DBs, the default value should be 'Excluded from limitation'; 
-- but when the user creates a new functionality profile, the default value should be 'Included in limitation'
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'LIMITATIONDISPLAYTYPE')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILE ADD LIMITATIONDISPLAYTYPE INT
	EXECUTE spDB_SetFieldDescription_1_0 'POSFUNCTIONALITYPROFILE', 'LIMITATIONDISPLAYTYPE', 'How items with 
	limitation should be displayed in the POS (0 - show items included in limitation, 1 - show items excluded from 
	limitation, 2 - don''t show any limitation info)';
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'DISPLAYLIMITATIONSTOTALSINPOS')
    BEGIN
        ALTER TABLE POSFUNCTIONALITYPROFILE ADD DISPLAYLIMITATIONSTOTALSINPOS TINYINT NOT NULL DEFAULT 0 WITH VALUES
        EXECUTE spDB_SetFieldDescription_1_0 'POSFUNCTIONALITYPROFILE', 'DISPLAYLIMITATIONSTOTALSINPOS', 'If true the limitation totals are displayed in the POS';
    END
GO

-- 1 = 'Excluded from limitation'
-- 0 = 'Included in limitation'
-- 2 = 'Do not display'
UPDATE POSFUNCTIONALITYPROFILE SET LIMITATIONDISPLAYTYPE = 1 WHERE LIMITATIONDISPLAYTYPE IS NULL
GO

-- delete any existing DEFAULT constraint on LIMITATIONDISPLAYTYPE column and add the new, named one (LIMITATIONDISPLAYTYPE_DEFAULT)
IF OBJECT_ID('dbo.[LIMITATIONDISPLAYTYPE_DEFAULT]', 'D') IS NULL 
BEGIN
    DECLARE @defaultConstraint sysname
    
    SELECT @defaultConstraint = [name] 
    FROM sys.default_constraints AS D
    WHERE D.parent_object_id = OBJECT_ID('POSFUNCTIONALITYPROFILE')
        AND D.parent_column_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID('POSFUNCTIONALITYPROFILE') AND name = 'LIMITATIONDISPLAYTYPE')
        AND D.type = 'D'
    
    IF(@defaultConstraint IS NOT NULL)
    BEGIN
        DECLARE @sql nvarchar(255)
        SET @sql = 'ALTER TABLE POSFUNCTIONALITYPROFILE DROP CONSTRAINT ' + @defaultConstraint
        EXECUTE sp_executesql @sql;
    END
    
    ALTER TABLE POSFUNCTIONALITYPROFILE ADD CONSTRAINT LIMITATIONDISPLAYTYPE_DEFAULT DEFAULT 0 FOR LIMITATIONDISPLAYTYPE
END 
GO