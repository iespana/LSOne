
/*

	Incident No.	: N/A
	Responsible		:Björn Eiríksson
	Sprint			: N/A
	Date created	: 26.04.2011

	Description		: Add support for database documentation


	
	Tables affected	: NONE
						
*/

/*
Example uses:

To set field and table descriptions:
exec spDB_SetTableDescription_1_0 'RBOSTYLES','Table that contains style setup information'
exec spDB_SetFieldDescription_1_0 'RBOSTYLES','STYLE','The ID of the style record'
exec spDB_SetFieldDescription_1_0 'RBOSTYLES','NAME','Description for the style record'

To pull out a report:
exec spDB_GetTableDescription_1_0 'RBOSTYLES'
exec spDB_GetTableFieldDescriptions_1_0 'RBOSTYLES'

*/

USE LSPOSNET
GO

-----------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spDB_SetTableDescription_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spDB_SetTableDescription_1_0]
GO

create procedure dbo.spDB_SetTableDescription_1_0
(@TableName SYSNAME,@MS_DescriptionValue NVARCHAR(200)) 
as

set nocount on

-- Routine to add or update an extended property on a table
DECLARE @MS_Description NVARCHAR(200) = NULL;

SET @MS_Description = (SELECT CAST(Value AS NVARCHAR(200)) AS [MS_Description]
FROM sys.extended_properties AS ep
WHERE ep.major_id = OBJECT_ID(@TableName)
AND ep.name = N'MS_Description' AND ep.minor_id = 0); 

IF @MS_Description IS NULL
    BEGIN
        EXEC sys.sp_addextendedproperty
         @name  = N'MS_Description',
         @value = @MS_DescriptionValue,
         @level0type = N'SCHEMA',
         @level0name = N'dbo',
         @level1type = N'TABLE',
         @level1name = @TableName;
    END
ELSE
    BEGIN
        EXEC sys.sp_updateextendedproperty
         @name  = N'MS_Description',
         @value = @MS_DescriptionValue,
         @level0type = N'SCHEMA',
         @level0name = N'dbo',
         @level1type = N'TABLE',
         @level1name = @TableName;
    END
 
    
GO
-----------------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spDB_GetTableDescription_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spDB_GetTableDescription_1_0]
GO

create procedure dbo.spDB_GetTableDescription_1_0
(@TableName SYSNAME) 
as

set nocount on

-- Get extended properties for all tables in one schema
SELECT objtype, objname, name, value
FROM fn_listextendedproperty (NULL, 'schema', 'dbo', N'table', default, NULL, NULL)
where objname = @TableName

GO
-----------------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spDB_SetFieldDescription_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spDB_SetFieldDescription_1_0]
GO

create procedure dbo.spDB_SetFieldDescription_1_0
(@TableName SYSNAME,@FieldName SYSNAME,@MS_DescriptionValue NVARCHAR(200)) 
as

set nocount on

-- Routine to add or update an extended property on a table
DECLARE @MS_Description NVARCHAR(200) = NULL;

SELECT @MS_Description = CAST(ex.value AS NVARCHAR(200))
FROM  
    sys.columns c  
LEFT OUTER JOIN  
    sys.extended_properties ex  
ON  
    ex.major_id = c.object_id 
    AND ex.minor_id = c.column_id  
    AND ex.name = 'MS_Description'  
WHERE  
    OBJECTPROPERTY(c.object_id, 'IsMsShipped')=0  
    AND OBJECT_NAME(c.object_id) = @TableName 
	and c.name = @FieldName

IF @MS_Description IS NULL
    BEGIN
        EXEC sys.sp_addextendedproperty
         @name  = N'MS_Description',
         @value = @MS_DescriptionValue,
         @level0type = N'SCHEMA',
         @level0name = N'dbo',
         @level1type = N'TABLE',
         @level1name = @TableName,
         @level2type = N'COLUMN',
         @level2name = @FieldName;
    END
ELSE
    BEGIN
        EXEC sys.sp_updateextendedproperty
         @name  = N'MS_Description',
         @value = @MS_DescriptionValue,
         @level0type = N'SCHEMA',
         @level0name = N'dbo',
         @level1type = N'TABLE',
         @level1name = @TableName,
         @level2type = N'COLUMN',
         @level2name = @FieldName;
    END
 
    
GO
-----------------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spDB_GetTableFieldDescriptions_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spDB_GetTableFieldDescriptions_1_0]
GO

create procedure dbo.spDB_GetTableFieldDescriptions_1_0
(@TableName SYSNAME) 
as

set nocount on

SELECT  
    [Table Name] = OBJECT_NAME(c.object_id), 
    [Column Name] = c.name, 
    [Description] = ex.value  
FROM  
    sys.columns c  
LEFT OUTER JOIN  
    sys.extended_properties ex  
ON  
    ex.major_id = c.object_id 
    AND ex.minor_id = c.column_id  
    AND ex.name = 'MS_Description'  
WHERE  
    OBJECTPROPERTY(c.object_id, 'IsMsShipped')=0  
    AND OBJECT_NAME(c.object_id) = @TableName
ORDER  
    BY OBJECT_NAME(c.object_id), c.column_id

GO
-----------------------------------------------------------------------------------------------