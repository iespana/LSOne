/*
	Incident No.	: ONE-9698
	Responsible		: Ovidiu Caba
	Sprint			: Betelgeuse
	Date created	: 05.03.2019

	Description		: Increase column ITEMID to 30 characters
*/

USE LSPOSNET

/*
	In order to determine which columns should be increased, I used the following script:

	select 'SELECT TABLE_NAME = ''' + TABLE_NAME + ''', COLUMN_NAME = ''' + COLUMN_NAME + ''', IS_NULLABLE = ''' + IS_NULLABLE + ''' UNION ', *
	from INFORMATION_SCHEMA.COLUMNS 
	where	COLUMN_NAME like '%item%' and 
			DATA_TYPE='nvarchar' and 
			COLUMN_NAME not like '%itemgroup%' and 
			COLUMN_NAME not like '%vendoritem%' and
			COLUMN_NAME not like '%itemdepartment%' and
			COLUMN_NAME not like '%RECEIPTITEMSLAYOUTXML%' and
			COLUMN_NAME not like '%TAXITEM%' and
			COLUMN_NAME not like '%ITEMGRIDID%' and
			COLUMN_NAME not like '%ITEMPOSTINGGROUP%' and
			COLUMN_NAME not like '%ITEMTENDER%' and
			COLUMN_NAME not like '%ITEMIDCOMPANY%' and
			COLUMN_NAME not like '%ITEMFAMILY%' and
			COLUMN_NAME not like '%ITEMNUMSEQ%' and
			COLUMN_NAME not like '%MULTIBLEITEMSYMBOL%' and
			COLUMN_NAME not like '%ITEMBUYERGROUPID%' and
			TABLE_NAME not like '%VINVENTSUM%' and	-- this is a view, not a table
			CHARACTER_MAXIMUM_LENGTH < 30
*/

DECLARE @table_name nvarchar(max), @column_name nvarchar(max), @is_nullable nvarchar(10);

DECLARE columns_cursor CURSOR  
    FOR
		SELECT TABLE_NAME = 'INVENTORYTRANSFERREQUESTLINE', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'COUPONITEMS', COLUMN_NAME = 'ITEMRELATION', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'ITEMREPLENISHMENTSETTING', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'INVENTDIMCOMBINATION', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'RBOINVENTTABLE', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'RBOINVENTLINKEDITEM', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'RBOINVENTLINKEDITEM', COLUMN_NAME = 'LINKEDITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'RBODISCOUNTOFFERLINE', COLUMN_NAME = 'ITEMRELATION', IS_NULLABLE = 'NULL' UNION 
		SELECT TABLE_NAME = 'RETAILITEM', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'VENDORITEMS', COLUMN_NAME = 'RETAILITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'PURCHASEORDERLINE', COLUMN_NAME = 'RETAILITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'ORGDATA_PRICEDISCTABLE', COLUMN_NAME = 'ITEMRELATION', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'ORGDATA_INVENTTABLEMODULE', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'RBOINVENTTRANSLATIONS', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'UNITCONVERT', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'POSISTENDERRESTRICTIONS', COLUMN_NAME = 'ITEMORGROUPID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'PRICEDISCTABLE', COLUMN_NAME = 'ITEMRELATION', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'INVENTJOURNALTRANS', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NULL' UNION 
		SELECT TABLE_NAME = 'INVENTTRANS', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'INVENTITEMBARCODE', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'RBOTRANSACTIONSALESTRANS', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NULL' UNION 
		SELECT TABLE_NAME = 'KITCHENDISPLAYLOG', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'RBOTRANSACTIONFUELTRANS', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NULL' UNION 
		SELECT TABLE_NAME = 'INVENTSUM', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'PURCHASEWORKSHEETLINE', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'RBOINVENTITEMIMAGE', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'SPECIALGROUPITEMS', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'RETAILITEMIMAGE', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'POSFUNCTIONALITYPROFILE', COLUMN_NAME = 'ITEMNOTONFILE', IS_NULLABLE = 'NULL' UNION 
		SELECT TABLE_NAME = 'RBOSPECIALGROUPITEMS', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'INVENTTABLEMODULE', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'INVENTTABLE', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'INVENTTABLE', COLUMN_NAME = 'ALTITEMID', IS_NULLABLE = 'NULL' UNION 
		SELECT TABLE_NAME = 'INVENTSERIAL', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'INVENTORYTRANSFERORDERLINE', COLUMN_NAME = 'ITEMID', IS_NULLABLE = 'NOT NULL' UNION 
		SELECT TABLE_NAME = 'RBOINFORMATIONSUBCODETABLE', COLUMN_NAME = 'TRIGGERCODE', IS_NULLABLE = 'NULL' UNION	-- this is needed to be able to add subcode with trigger item with ID whose length is 30 characters
		SELECT TABLE_NAME = 'RBOINFOCODETABLESPECIFIC', COLUMN_NAME = 'REFRELATION', IS_NULLABLE = 'NOT NULL' UNION	-- this is needed to be able to add infocodes to an item
		SELECT TABLE_NAME = 'RBOTRANSACTIONINFOCODETRANS', COLUMN_NAME = 'SOURCECODE', IS_NULLABLE = 'NULL'			-- this is needed to be able to post a transaction containing an item with an ID with more than 30 characters with infocode

OPEN columns_cursor

FETCH NEXT FROM columns_cursor
INTO @table_name, @column_name, @is_nullable

WHILE @@FETCH_STATUS = 0  
BEGIN

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table_name AND COLUMN_NAME = @column_name)
	BEGIN
		IF(COL_LENGTH(@table_name, @column_name) < 60) -- We want to update the column to be NVARCHAR(30) which has an equivalent length in bytes of 60
		BEGIN 
			EXEC('ALTER TABLE ' + @table_name + ' ALTER COLUMN ' + @column_name + ' NVARCHAR(30) ' + @is_nullable)
		END
	END

	FETCH NEXT FROM columns_cursor
	INTO @table_name, @column_name, @is_nullable

END

CLOSE columns_cursor;
DEALLOCATE columns_cursor;

GO