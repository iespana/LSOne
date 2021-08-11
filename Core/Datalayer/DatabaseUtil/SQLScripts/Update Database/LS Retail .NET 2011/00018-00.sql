/*

	Incident No.	: N/A
	Responsible		: Tobias Helmer
	Sprint			: 
	Date created	: 17.11.2010
	
	Description		: Increasing column sizes the tables

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:  INVENTTRANS, INVENTTRANSREASON, INVENTJOURNALTABLE
						
*/

Use LSPOSNET


--this one increases the column sizes; we are dealing with an indexed column here (OFFERID), so we drop and recreate the index.
	--  HG - This table was recreated in 00030-00
	--BEGIN
	--	DECLARE @length int
	--	set @length = 
	--	(SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
	--	WHERE TABLE_NAME = 'INVENTTRANS' AND COLUMN_NAME = 'REASONCODE')
	--
	--	if (@length < 20)
	--	BEGIN
	--		BEGIN 				
	--			IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[INVENTTRANS]') AND name = N'IX_INVENTTRANS_OFFERID')
	--			DROP INDEX [IX_INVENTTRANS_OFFERID] ON [dbo].[INVENTTRANS] WITH ( ONLINE = OFF )
	--		END
	--		BEGIN
	--			ALTER TABLE INVENTTRANS ALTER COLUMN VARIANTID [nvarchar](20) NOT NULL
	--			ALTER TABLE INVENTTRANS ALTER COLUMN STOREID [nvarchar](20) NOT NULL	
	--			ALTER TABLE INVENTTRANS ALTER COLUMN OFFERID [nvarchar](20) NOT NULL	
	--			ALTER TABLE INVENTTRANS ALTER COLUMN REASONCODE [nvarchar](20) NOT NULL
	--		END	
	--		BEGIN
	--			CREATE NONCLUSTERED INDEX [IX_INVENTTRANS_OFFERID] ON [dbo].[INVENTTRANS] 
	--			([OFFERID] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	--		END		
	--	END
	--END

	--this one increases the column size; since it is part of the PK, we have to drop the PK first
	BEGIN
		DECLARE @length2 int
		set @length2 = 
		(SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
		WHERE TABLE_NAME = 'INVENTTRANSREASON' AND COLUMN_NAME = 'REASONID')

		if (@length2 < 20)
		BEGIN
		declare @name varchar(255)
		declare @SQL varchar(255)
		declare @SQL2 varchar(255)

			BEGIN				
				set @name = (select name FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[INVENTTRANSREASON]') AND is_primary_key = 1)				
				set @SQL = 'ALTER TABLE dbo.INVENTTRANSREASON DROP CONSTRAINT ' + @name
				exec(@SQL)		
			END
			BEGIN
				ALTER TABLE INVENTTRANSREASON ALTER COLUMN REASONID [nvarchar](20) NOT NULL
			END
			BEGIN
				set @SQL2 = 'ALTER TABLE dbo.INVENTTRANSREASON ADD CONSTRAINT ' + @name + ' PRIMARY KEY CLUSTERED ([REASONID],[DATAAREAID]) ON [PRIMARY] '
				exec(@SQL2)
			END
		END
	END

GO

--this one increases the column size; since it is part of the PK, we have to drop the PK first
	BEGIN
		DECLARE @length3 int
		set @length3 = 
		(SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
		WHERE TABLE_NAME = 'INVENTJOURNALTABLE' AND COLUMN_NAME = 'JOURNALID')

		if (@length3 < 20)
		BEGIN
		declare @name2 varchar(255)
		declare @SQL3 varchar(255)
		declare @SQL4 varchar(255)

			BEGIN				
				set @name2 = (select name FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[INVENTJOURNALTABLE]') AND is_primary_key = 1)				
				set @SQL3 = 'ALTER TABLE dbo.INVENTJOURNALTABLE DROP CONSTRAINT ' + @name2
				exec(@SQL3)		
			END
			BEGIN
				ALTER TABLE INVENTJOURNALTABLE ALTER COLUMN JOURNALID [nvarchar](20) NOT NULL
			END
			BEGIN
				set @SQL4 = 'ALTER TABLE dbo.INVENTJOURNALTABLE ADD CONSTRAINT ' + @name2 + ' PRIMARY KEY CLUSTERED ([DATAAREAID] ASC,[JOURNALID] ASC) ON [PRIMARY] '
				exec(@SQL4)
			END
		END
	END

GO


	IF NOT EXISTS(SELECT * FROM PERMISSIONGROUP WHERE [GUID] = '{32246200-f0bb-11df-98cf-0800200c9a66}')
	Insert into PERMISSIONGROUP (GUID,Name,DATAAREAID) 
	values ('{32246200-f0bb-11df-98cf-0800200c9a66}','Inventory','LSR');

GO