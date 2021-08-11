using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Helper
{
    public static class Constants
    {

        #region SQL statements

        public static string GET_ALL_STORES_SQL = "SELECT ID AS 'StoreID', Name, StoreCode FROM Store";
        public static string GET_ALL_TABLES_SQL = "SELECT name FROM sys.Tables";
        public static string GET_ALL_REGISTERS_SQL = "SELECT r.ID AS 'TerminalID', r.Description AS 'TerminalDescription',r.StoreID,s.Name,s.StoreCode FROM Register r LEFT JOIN Store s ON s.ID = r.StoreID";
        public static string GET_ALL_CURRENCIES = "SELECT ID 'RMS_ID',ExchangeRate,[Description] 'Text',LEFT(Code,3) 'ID' FROM Currency";
        public static string GET_ALL_CUSTOMERS = @"select 
                                                    ID 'RMS_ID',
                                                    CAST(AccountNumber AS varchar(20)) as AccountNumber,
                                                    CAST((Title + ' ' +  Firstname + ' ' + Lastname) AS VarChar(200)) as Text,
                                                    CAST(Firstname AS varchar(50)) as FirstName, 
                                                    CAST(Lastname AS varchar(50)) as LastName,
                                                    CAST(Title AS varchar(8)) as Title,
                                                    CAST([address] AS varchar(50)) as Address,
                                                    CAST(City AS varchar(50)) as City,
                                                    CAST(State AS Varchar(50)) as State,
                                                    CAST(Country AS Varchar(50)) as Country,
                                                    CAST((Firstname + ' ' + Lastname) AS varchar(50)) as Contact,
                                                    CAST(Zip as varchar(10)) as ZIP,
                                                    CAST(taxNumber AS varchar(20)) as VatNum,
                                                    CAST(PhoneNumber AS Varchar(30)) as Telephone,
                                                    CAST(emailAddress AS varchar(80)) as Email
                                                    from Customer";

        public static string GET_ALL_VENDORS = @"SELECT
                                                supplier.ID as RMS_ID,
                                                SupplierName as Text,
                                                Address1 as Address1,
                                                Address2 as Address2,
                                                Zip as ZIP,
                                                City as City,
                                                State as State,
                                                Country as Country,
                                                PhoneNumber as Phone,
                                                EmailAddress as Email,
                                                FaxNumber as Fax,
                                                ContactName as DefaultContact,
                                                currencyid as RMS_CurrencyID,
                                                '' as TaxGroup,
                                                '2' as TaxCalculationMethod,
                                                '0' as DefaultDeliveryTime,
                                                '0' as DeliveryDaysType
                                                from Supplier supplier
                                                left join Currency on supplier.CurrencyID = Currency.ID";

        public static string GET_ALL_RETAILDEPARTMENTS = @"SELECT 
	                                                        id AS RMS_ID,
	                                                        Name AS Text
                                                        FROM Department";

        public static string GET_ALL_RETAILGROUPS = @"SELECT 
                                                        C.id AS RMS_ID,
                                                        C.Name AS Text,
                                                        C.DepartmentID AS RMS_Department_ID,
                                                        '' AS ItemSalesTaxGroupName,
                                                        '0' AS DefaultProfit,
                                                        '' AS POSPeriodicID,
                                                        Null AS DivisionMasterID
                                                        FROM Category C
                                                        left join Department D ON C.DepartmentID = D.ID";

        public static string GET_ALL_SALES_TAX = @"select ID AS 'RMS_ID',[Description] AS 'Text',Percentage,Code AS 'ReceiptDisplay',0.01 as TaxRoundOff, 0 AS TaxRoundOffType from Tax";

        public static string GET_ALL_GROUP_TAX = @"select ID AS 'RMS_ID',
                                                   [Description] AS 'Text',
	                                               Code AS 'ReceiptDisplay',
	                                               TaxID01 AS Tax1,
	                                               TaxID02 AS Tax2,
	                                               TaxID03 AS Tax3,
	                                               TaxID04 AS Tax4,
	                                               TaxID05 AS Tax5,
	                                               TaxID06 AS Tax6,
	                                               TaxID07 AS Tax7,
	                                               TaxID08 AS Tax8,
	                                               TaxID09 AS Tax9,
	                                               TaxID10 AS Tax10
                                            from ItemTax";

        public static string GET_ALL_MATRIX_ITEMS = @"
                                                        SELECT
                                                        IC.ID as RMS_ID,
                                                        CAST(IC.ItemLookupCode AS varchar(20)) as ItemNumber,	
                                                        CAST(IC.Description AS varchar(100)) as 'Text',	
                                                        '' as ExtendedDescription,	
                                                        '' as Status,	
                                                        S.ID AS RMS_CustomerID,
                                                        '' as VendorItemNo,	
                                                        '' as Barcode,	
                                                        CAST(IC.CategoryID AS varchar(10)) as RMS_RetailGroupID,	
                                                        CAST(IC.DepartmentID AS varchar(10)) RMS_DepartmentID,	
                                                        CONVERT(DECIMAL(38, 20),Cost) as PurchasePrice,	
                                                        CONVERT(DECIMAL(38, 20),price) as SalesPrice,
                                                        tax.ID as RMS_SalesTaxItemGroupID
                                                        from ItemClass IC	
                                                        left join ItemTax tax on IC.TaxID = tax.ID	
                                                        left join Supplier S on IC.SupplierID = s.ID";

        public static string GET_ALL_STANDARD_ITEMS = @"
                                                        SELECT
                                                        Item.ID as RMS_ID,
                                                        CAST(itemlookupcode AS varchar(20)) as ItemNumber,	
                                                        CAST(item.description AS varchar(100)) as 'Text',	
                                                        CAST (ExtendedDescription AS NVARCHAR(250))as ExtendedDescription,	
                                                        UnitOfMeasure as InventoryUnitOfMeasure,	
                                                        UnitOfMeasure as SalesUnitOfMeasure,	
                                                        '' as Status,	
                                                        S.ID AS RMS_CustomerID,
                                                        CASE   	
	                                                          WHEN Alias is Null THEN ''  
	                                                          WHEN Alias is not null THEN CAST(Alias AS nvarchar(30)) 
                                                           END as Barcode,	
                                                        CAST(CategoryID AS varchar(10)) as RMS_RetailGroupID,	
                                                        CAST(DepartmentID AS varchar(10)) as RMS_DepartmentID,	
                                                        Tax.ID as RMS_SalesTaxItemGroupID,
                                                        CONVERT(DECIMAL(38, 20),Cost) as PurchasePrice,	
                                                        CONVERT(DECIMAL(38, 20),price) as SalesPrice,
                                                        (SELECT TOP 1 TaxSystem FROM Configuration where StoreID = 0) AS TaxSystem	,
                                                        PictureName AS PictureName,
                                                        Item.PriceMustBeEntered as 'PriceMustBeEntered',
                                                        Item.ItemNotDiscountable as 'ItemNotDiscountable',
														Item.QuantityEntryNotAllowed as 'QuantityEntryNotAllowed',
														Item.ItemCannotBeSold as 'ItemCannotBeSold',
														Item.BlockSalesType as 'BlockSalesType',
														Item.BlockSalesAfterDate as 'BlockSalesAfterDate',
														Item.BlockSalesBeforeDate as 'BlockSalesBeforeDate',
	                                                    Item.TagAlongItem as 'TagAlongItem',
														Item.TagAlongQuantity as 'TagAlongQuantity',
														Item.ReorderPoint as 'ReorderPoint',
														Item.RestockLevel as 'RestockLevel'
                                                        from Item	
                                                        left join Tax on Item.TaxID = tax.ID	
                                                        left join Supplier S on Item.SupplierID = s.ID	
                                                        OUTER APPLY 	
	                                                        (Select Top 1 Alias, itemid from Alias Where Alias.ItemID = Item.id)Alias2
                                                        where item.id not in (select itemid from ItemClassComponent)";

        public static string GET_ALL_VARIANT_ITEMS = @"
                                                       Select 
                                                        ItemClassID AS RMS_MasterItemID,
                                                        Item.ID as RMS_ID,
                                                        CAST(itemclass.ItemLookupCode AS varchar(20)) as ItemNumber,	
                                                        CAST(item.itemlookupcode AS varchar(20)) as VariantCode,	
                                                        CAST(ItemClass.description AS varchar(100)) as 'Text',	
                                                        CAST(Item.ExtendedDescription AS varchar(100)) as 'ExtendedDescription',	
                                                        CAST(item.description AS varchar(100)) as 'VariantName',	
                                                        '' as ExtendedDescription,	
                                                        UnitOfMeasure as InventoryUnitOfMeasure,	
                                                        UnitOfMeasure as SalesUnitOfMeasure,	
                                                         CAST(Item.CategoryID AS varchar(10)) as RMS_RetailGroupID,	
                                                         CAST(Item.DepartmentID AS varchar(10)) RMS_DepartmentID,	
                                                        Tax.ID as RMS_SalesTaxItemGroupID,
                                                        '' as Status,	
                                                        S.ID AS RMS_CustomerID,
                                                        CASE   	
	                                                          WHEN Alias is Null THEN ''  
	                                                          WHEN Alias is not null THEN Alias  
                                                           END as Barcode,	
                                                        CONVERT(DECIMAL(38, 20), item.cost) as PurchasePrice,	
                                                        CONVERT(DECIMAL(38, 20), item.price) as SalesPrice ,
                                                        (SELECT TOP 1 TaxSystem FROM Configuration where StoreID = 0) AS TaxSystem,
                                                        PictureName AS PictureName,
                                                        Item.PriceMustBeEntered as 'PriceMustBeEntered',
                                                        Item.ItemNotDiscountable as 'ItemNotDiscountable',
														Item.QuantityEntryNotAllowed as 'QuantityEntryNotAllowed',
														Item.ItemCannotBeSold as 'ItemCannotBeSold',
														Item.BlockSalesType as 'BlockSalesType',
														Item.BlockSalesAfterDate as 'BlockSalesAfterDate',
														Item.BlockSalesBeforeDate as 'BlockSalesBeforeDate',
                                                        Item.TagAlongItem as 'TagAlongItem',
														Item.TagAlongQuantity as 'TagAlongQuantity',
                                                        Item.ReorderPoint as 'ReorderPoint',
														Item.RestockLevel as 'RestockLevel',
                                                        ItemClass.Title1 as Dimension1,
													    Detail1 AS DimensionAttribute1,
														ItemClass.Title2  as Dimension2,
                                                        Detail2 AS DimensionAttribute2,
														ItemClass.Title3  as Dimension3,
                                                        Detail3 AS DimensionAttribute3	
                                                        from ItemClass	
                                                        left join itemclasscomponent on itemclass.ID = ItemClassComponent.ItemClassID	
                                                        left join Supplier S on ItemClass.SupplierID = s.ID	
                                                        left join Item on ItemClassComponent.ItemID = Item.ID	
                                                        OUTER APPLY 	
	                                                        (Select Top 1 Alias, itemid from Alias Where Alias.ItemID = Item.id)Alias2
                                                        left join Tax on Item.TaxID = tax.ID	
                                                        where item.itemlookupcode is not null
                                                        order by ItemClassID";

        public static string GET_ALL_ITEM_SALE_PRICE = @"SELECT	
                                                        I.ID as RMS_ItemID,
                                                        CASE	
	                                                        WHEN IC.ItemLookupCode is null THEN ''
	                                                        ELSE
	                                                        CAST(I.Itemlookupcode AS nvarchar(20))
	                                                        END as RMS_VariantCode,
                                                        i.salestartdate as RMS_StartDate,	
                                                        i.saleenddate as RMS_EndDate,
                                                        CASE	
	                                                        WHEN Tax.Percentage = 20 THEN CAST((I.SalePrice/1.2) AS decimal(38,12))
	                                                        ELSE
	                                                        CAST(I.SalePrice AS decimal(38,12))
	                                                        END as Amount,
                                                        CAST(I.SalePrice AS decimal(38,12)) as AmountIncludingTax,	
                                                        CAST('0' AS decimal(38,12)) as LineDiscount,
                                                        CAST('1' AS decimal(38,12)) as QuantityAmount	
                                                        From item I	
                                                        left join itemclasscomponent ICC on I.id = ICC.itemid	
                                                        left join itemclass IC on ICC.ItemClassID = IC.id	
                                                        left join tax on I.taxid = Tax.id	
                                                        where 	
                                                        i.SaleStartDate >= getdate()-1 	
                                                        or i.SaleStartDate <= getdate() and i.SaleEndDate >= getdate()	
                                                        and i.SaleType = '1'	
                                                        ";

        public static string GET_ALL_PURCHASE_ORDERS = @"Select 
                                                        PO.ID as RMS_ID,
                                                        PO.StoreID as RMS_StoreID,
                                                        PO.PONumber as Description,	
                                                        PO.SupplierID as RMS_VendorID,
                                                        PO.RequiredDate as OrderDate
                                                        from[purchaseorder] PO
                                                        where  PO.Status in ('0','1') and PO.POType = '0' ";
        public static string GET_ALL_PURCHASE_ORDER_LINES = @"select 
                                                                PO.StoreID as RMS_StoreID,
                                                                PO.SupplierID as RMS_VendorID,
                                                                CAST(PO.ID AS nvarchar(20)) as RMS_PurchaseOrderHeaderID,	
                                                                ROW_NUMBER() OVER(PARTITION BY POE.PurchaseOrderId ORDER BY POE.PurchaseOrderId DESC) AS LineNumber,
                                                                I.ID as RMS_ItemID,	
                                                                I.TaxID as RMS_SalesTaxItemGroupID,
                                                                Unitofmeasure as UnitOfMeasure,	
                                                                CAST(POE.QuantityOrdered - POE.QuantityReceivedToDate AS decimal(38,12)) as Quantity,	
                                                                CAST(I.Cost AS decimal(38,12)) as UnitPrice,	
                                                                (I.Cost - POE.Price) as DiscountAmount,	
                                                                CAST(POE.Price AS decimal(38,12)) as Amount,	
                                                                POE.TaxRate as VATCode,
                                                                POE.Price*POE.TaxRate/100 as VATAmount,	
                                                                PO.RequiredDate as ExpectedReceiptDate	
                                                                From PurchaseOrder PO	
                                                                left join PurchaseOrderEntry POE on PO.ID = POE.PurchaseOrderID and PO.StoreID = POE.StoreID	
                                                                left join Item I on POE.Itemid = I.id	
																left join ItemClassComponent ICC on I.id = ICC.itemid	
                                                                Left join ItemClass IC on ICC.itemclassid = IC.id	
																left join	
                                                                (Select PurchaseOrderID,StoreID,Count(*) AS CountOfLines From PurchaseOrderEntry GROUP BY PurchaseOrderID,StoreID) CE	
                                                                on  PO.id = CE.PurchaseOrderID and PO.Status = '0' and PO.StoreID = CE.StoreID	
                                                                where  PO.Status in ('0','1') and ((IC.ItemLookupCode is not null and PO.POType = '0') or (I.ItemLookupCode is not null and PO.POType = '0'))";


        public static string GET_ALL_TRANSACTIONS = @"SELECT
                                                        CAST(T.TransactionNumber AS nvarchar(20)) as TransactionNumber,	
                                                        T.StoreID as RMS_StoreID,
                                                        Batch.RegisterID as RMS_TerminalID,
                                                        CAST(T.TransactionNumber AS nvarchar(20)) as RMS_ID,	
                                                        C.ID as RMS_CustomerID,
                                                        T.Time as RMSPostingDate,	
                                                        CE.CountOfLines AS NoOfLines,	
                                                        CAST(T.Total AS decimal(38,12)) as TransactionAmount,	
                                                        CAST(T.SalesTax AS decimal(38,12)) as VATAmount	
                                                        from [Transaction] T	
                                                        left join [Batch] on T.BatchNumber = [Batch].BatchNumber and T.StoreID = Batch.StoreID	
                                                        left join customer C on C.ID = T.CustomerID	
                                                        left join	
                                                        (Select TransactionNumber,StoreID,Count(*) AS CountOfLines From TransactionEntry GROUP BY TransactionNumber,StoreID) CE	
                                                        on  T.TransactionNumber = CE.TransactionNumber and CE.StoreID = T.StoreID	
                                                        where CE.CountOfLines is not null	";

        public static string GET_ALL_TRANSACTION_LINES = @"SELECT
                                                            CAST(T.TransactionNumber AS nvarchar(20)) as TransactionNumber,	
                                                            ROW_NUMBER() OVER(PARTITION BY TE.Transactionnumber ORDER BY TE.Transactionnumber DESC) AS LineNumber,	
                                                            T.StoreID as RMS_StoreID,
                                                            ROW_NUMBER() OVER(PARTITION BY TE.Transactionnumber ORDER BY TE.Transactionnumber DESC) AS LineNumber,	
                                                            I.ID as RMS_ItemID,
                                                            I.unitofmeasure as UnitOfMeasure,	
                                                            CAST(TE.Quantity AS decimal(38,12)) as Quantity,	
                                                            CAST(TE.Price AS decimal(38,12)) as UnitPrice,	
                                                            CAST((TE.FullPrice - TE.Price) AS decimal(38,12)) as LineDiscountAmount,	
                                                            CAST((TE.Price * TE.quantity) AS decimal(38,12)) as Amount,	
                                                            TE.SalesTax as VATCode,
                                                            CAST(TE.SalesTax AS decimal(38,12)) as VATAmount	
                                                            From [Transaction] T	
                                                            left join [TransactionEntry] TE on T.transactionnumber = TE.TransactionNumber and T.StoreID = TE.StoreID	
                                                            left join Item I on TE.Itemid = I.id	
                                                            left join [Batch] on T.BatchNumber = [Batch].BatchNumber and T.StoreID = Batch.StoreID	
                                                            left join ItemClassComponent ICC on I.id = ICC.itemid	
                                                            Left join ItemClass IC on ICC.itemclassid = IC.id	
                                                            left join	
                                                            (Select TransactionNumber,Storeid,Count(*) AS CountOfLines From TransactionEntry GROUP BY TransactionNumber,StoreID) CE	
                                                            on T.TransactionNumber = CE.TransactionNumber and CE.StoreID = T.StoreID	
                                                            order by T.transactionnumber	
                                                            ";

        public static string GET_ALL_TRANSACTION_PAYMENTS = @"SELECT
                                                                T.StoreID as RMS_StoreID,
                                                                Batch.RegisterID as RMS_TerminalID,
                                                                CAST(TNE.TransactionNumber AS nvarchar(20)) as TransactionNumber,	
                                                                ROW_NUMBER() OVER(PARTITION BY TNE.Transactionnumber ORDER BY TNE.Transactionnumber DESC) AS LineNumber,	
                                                                T.Time as RMS_PostingDate,	
                                                                CASE	
                                                                 WHEN TNE.Description = 'Cash' THEN 10
                                                                 WHEN TNE.Description = 'Credit Card' THEN 1	
                                                                 WHEN TNE.Description like 'Chip%' THEN 10
                                                                 WHEN TNE.Description like 'Credit Note%' THEN 5	
                                                                 WHEN TNE.Description like 'Loyalty%' THEN 9	
                                                                 WHEN TNE.Description like 'Voucher%' THEN 8	
                                                                 WHEN TNE.Description like 'Web%' THEN 10	
                                                                 WHEN TNE.Description like 'Cheque%' THEN 2
                                                                 ELSE	
                                                                 10	
                                                                END as PaymentMethodCode,
                                                                TNE.Description AS PaymentMethod,
                                                                CAST(TNE.Amount AS decimal(38,12)) as Amount,	
                                                                CAST(TNE.Amount AS decimal(38,12)) as AmountSystemCurrency,
                                                                '' as ReferenceTransactionNumber	
                                                                from TenderEntry TNE	
                                                                left join [Transaction] T on TNE.Transactionnumber = T.TransactionNumber and T.StoreID = TNE.StoreID	
                                                                left join [Batch] on T.BatchNumber = [Batch].BatchNumber and T.StoreID = Batch.StoreID	
                                                                where T.time is not null";


        public static string GET_ALL_USERS = @"select
                                                ID as 'RMS_ID',
                                                Number,
                                                Name as 'FirstName',                                                
                                                Name as 'Login',
                                                StoreID as 'RMS_StoreID'
                                                from [Cashier]
                                                where 
                                                Inactive = 0";

        public static string GET_ALL_VENDOR_ITEMS = @"select 
                                                        ID AS RMS_ID,
                                                        ItemID as RMS_ItemID,
                                                        SupplierID as RMS_VendorID,
                                                        ID AS VendorItemID,
                                                        CONVERT(DECIMAL(38, 20),Cost) as DefaultPurchasePrice
                                                        from SupplierList";

        public static string GET_SYSTEM_TAX = @"SELECT TOP 1 TaxSystem FROM Configuration where StoreID = 0";

        public static string GET_ALL_SYSTEM_TAX = @"SELECT  TaxSystem,StoreID FROM Configuration ";

        public static string GET_IMAGE_FOLDER_RELATIVE_PATH = @"DECLARE @datapath varchar(255)
                                                                if (select count(*) from configuration)>1
	                                                                EXEC master..xp_regread
	                                                                'HKEY_LOCAL_MACHINE',
	                                                                'SOFTWARE\WOW6432Node\Microsoft\Retail Management System\Headquarters\Manager\Path',
	                                                                'Pictures',
	                                                                @datapath OUTPUT
	                                                                if (@datapath is null)
		                                                                EXEC master..xp_regread
		                                                                'HKEY_LOCAL_MACHINE',
		                                                                'SOFTWARE\Microsoft\Retail Management System\Headquarters\Manager\Path',
		                                                                'Pictures',
		                                                                @datapath OUTPUT

                                                                if (select count(*) from configuration)<=1
	                                                                EXEC master..xp_regread
	                                                                'HKEY_LOCAL_MACHINE',
	                                                                'SOFTWARE\WOW6432Node\Microsoft\Retail Management System\Store Operations\Manager\Path',
	                                                                'Pictures',
	                                                                @datapath OUTPUT
	                                                                if (@datapath is null)
		                                                                EXEC master..xp_regread
		                                                                'HKEY_LOCAL_MACHINE',
		                                                                'SOFTWARE\Microsoft\Retail Management System\Store Operations\Manager\Path',
		                                                                'Pictures',
		                                                                @datapath OUTPUT

                                                                SELECT @datapath AS ImageRelativePath";

        #endregion

        public static List<string> RMS_TABLE_IDENTIFICATION_LIST = new List<string>() { "PurchaseOrder", "Store", "QuantityDiscount" };

        public static string EachUnitOfMeasure = "Each";
        public static string RMSMigrationFlag = "RMSMIGRATION";
    }
}