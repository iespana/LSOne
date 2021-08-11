/*
	Incident No.	: ONE-7632
	Responsible		: Simona Avornicesei
	Sprint			: Bellatrix
	Date created	: 17.09.2019

	Description		: Stock counting worksheet additions
*/
USE LSPOSNET

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spDB_SetRoutineDescription_1_0]') AND 
OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[spDB_SetRoutineDescription_1_0]
GO

CREATE PROCEDURE [dbo].[spDB_SetRoutineDescription_1_0]
(
    @RoutineName SYSNAME, 
    @MS_DescriptionValue NVARCHAR(400)
) 
AS
BEGIN
    /*
     -------------------------------------------------------------------------------------------------------------------
     Description  : Routine to add or update description of a stored procedure / function / sequence / table / table 
                    type / view
     Changes      : [17.09.2019][ONE-7632] Added
     -------------------------------------------------------------------------------------------------------------------
    */
    SET NOCOUNT ON

    DECLARE @MS_Description NVARCHAR(400) = NULL;
    DECLARE @objType NVARCHAR(20) = NULL;
    
    SELECT @objType = CASE [type]
        WHEN 'FN' THEN 'FUNCTION'   
        WHEN 'P' THEN 'PROCEDURE'
        WHEN 'SO' THEN 'SEQUENCE'
        WHEN 'U' THEN 'TABLE'
        WHEN 'TT' THEN 'TABLE_TYPE'
        WHEN 'V' THEN 'VIEW'
    END
    FROM sys.objects 
    WHERE [name] = @RoutineName

    SET @MS_Description = (SELECT CAST(Value AS NVARCHAR(400)) AS [MS_Description]
       FROM sys.extended_properties AS ep
       WHERE ep.major_id = OBJECT_ID(@RoutineName)
            AND ep.name = N'MS_Description' AND ep.minor_id = 0);

    IF @MS_Description IS NULL
        BEGIN
            EXEC sys.sp_addextendedproperty
             @name  = N'MS_Description',
             @value = @MS_DescriptionValue,
             @level0type = N'SCHEMA',
             @level0name = N'dbo',
             @level1type = @objType,
             @level1name = @RoutineName;
        END
    ELSE
        BEGIN
            EXEC sys.sp_updateextendedproperty
             @name  = N'MS_Description',
             @value = @MS_DescriptionValue,
             @level0type = N'SCHEMA',
             @level0name = N'dbo',
             @level1type = @objType,
             @level1name = @RoutineName;
    END  
END
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spDB_SetFieldDescription_1_0]') AND
        OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[spDB_SetFieldDescription_1_0]
GO

CREATE PROCEDURE [dbo].[spDB_SetFieldDescription_1_0]
(
    @TableName SYSNAME,
    @FieldName SYSNAME,
    @MS_DescriptionValue NVARCHAR(400)
)
AS
BEGIN
    /*
     -------------------------------------------------------------------------------------------------------------------
     Description  : Routine to add or update description of a table column
     Changes      : [17.09.2019][ONE-7632]Description parameter size was increased to 400
     -------------------------------------------------------------------------------------------------------------------
    */
    SET NOCOUNT ON

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
        OBJECTPROPERTY(c.object_id, 'IsMsShipped') = 0
        AND OBJECT_NAME(c.object_id) = @TableName
        AND c.name = @FieldName
    
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
END
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[spDB_SetTableDescription_1_0]') AND
        OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[spDB_SetTableDescription_1_0]
GO

CREATE PROCEDURE [dbo].[spDB_SetTableDescription_1_0]
(
    @TableName SYSNAME,
    @MS_DescriptionValue NVARCHAR(400)
)
AS
BEGIN
    /*
     -------------------------------------------------------------------------------------------------------------------
     Description  : Routine to add or update description of a table column
     Changes      : [17.09.2019][ONE-7632] Description parameter size was increased to 400
     -------------------------------------------------------------------------------------------------------------------
    */
    set nocount on

    DECLARE @MS_Description NVARCHAR(200) = NULL;

    SELECT @MS_Description = CAST(Value AS NVARCHAR(200))
        FROM sys.extended_properties AS ep
        WHERE ep.major_id = OBJECT_ID(@TableName)
            AND ep.name = N'MS_Description' 
            AND ep.minor_id = 0;

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
END
GO