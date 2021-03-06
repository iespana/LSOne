/*
	Incident No.	: ONE-9113
	Responsible		: Adrian Chiorean
	Sprint			: Denobulans
	Date created	: 05.10.2018

	Description		: Increase LINENUM column size to 30 in table INVENTJOURNALTRANS
*/

USE LSPOSNET

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTJOURNALTRANS' AND COLUMN_NAME = 'LINENUM')
BEGIN
	IF(COL_LENGTH('INVENTJOURNALTRANS', 'LINENUM') < 120) -- We want to update the column to be NVARCHAR(60) which has an equivalent length in bytes of 120
	BEGIN 

	DECLARE @PKNAME NVARCHAR(40);
	SET @PKNAME = (select TOP 1 object_name(object_id) AS PKName from sys.key_constraints where object_name(parent_object_id) = 'INVENTJOURNALTRANS');
	DECLARE @COMMAND NVARCHAR(MAX);
	SET @COMMAND = 'ALTER TABLE [dbo].[INVENTJOURNALTRANS] DROP CONSTRAINT ' + @PKNAME + ' WITH ( ONLINE = OFF )';
	
	EXECUTE(@COMMAND)

	ALTER TABLE [INVENTJOURNALTRANS] ALTER COLUMN [LINENUM] NVARCHAR(60) NOT NULL

	ALTER TABLE [dbo].[INVENTJOURNALTRANS] ADD PRIMARY KEY CLUSTERED 
	(
		[JOURNALID] ASC,
		[LINENUM] ASC,
		[DATAAREAID] ASC																																		 --SET IGNORE DUPLICATE KEY TO TRUE
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, IGNORE_DUP_KEY = ON) ON [PRIMARY]
	END
END
GO