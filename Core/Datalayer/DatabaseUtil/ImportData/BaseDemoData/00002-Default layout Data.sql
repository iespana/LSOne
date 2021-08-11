
/*

	Incident No.	: N/A
	Responsible		: Marý B. Steingrímsdóttir
	Sprint			: N/A
	Date created	: 16.4.2011

	Description		: Creates a default tillayout for the LS POS to use if no other is found

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PosisTillLayout - data added
					  PosisButtonGrid - data added
					  PosisButtonGridButtons- data added
						
*/

use LSPOSNET

DECLARE @STORECOUNT INT
DECLARE @TERMINALCOUNT INT
DECLARE @STAFFCOUNT INT
DECLARE @PRICECOUNT INT

SELECT @STORECOUNT = COUNT(*) FROM RBOSTORETABLE
SELECT @TERMINALCOUNT = COUNT(*) FROM RBOTERMINALTABLE
SELECT @STAFFCOUNT = COUNT(*) FROM RBOSTAFFTABLE
SELECT @PRICECOUNT = COUNT(*) FROM PRICEPARAMETERS

IF (@PRICECOUNT = 0)
BEGIN
	INSERT [dbo].[PRICEPARAMETERS] ([SALESPRICEACCOUNTITEM], [SALESLINEACCOUNTITEM], [SALESLINEACCOUNTGROUP], [SALESLINEACCOUNTALL], [SALESMULTILNACCOUNTGROUP], [SALESMULTILNACCOUNTALL], [SALESENDACCOUNTALL], [SALESPRICEGROUPITEM], [SALESLINEGROUPITEM], [SALESLINEGROUPGROUP], [SALESLINEGROUPALL], [SALESMULTILNGROUPGROUP], [SALESMULTILNGROUPALL], [SALESENDGROUPALL], [SALESPRICEALLITEM], [SALESLINEALLITEM], [SALESLINEALLGROUP], [SALESLINEALLALL], [SALESMULTILNALLGROUP], [SALESMULTILNALLALL], [SALESENDALLALL], [PURCHPRICEACCOUNTITEM], [PURCHLINEACCOUNTITEM], [PURCHLINEACCOUNTGROUP], [PURCHLINEACCOUNTALL], [PURCHMULTILNACCOUNTGROUP], [PURCHMULTILNACCOUNTALL], [PURCHENDACCOUNTALL], [PURCHPRICEGROUPITEM], [PURCHLINEGROUPITEM], [PURCHLINEGROUPGROUP], [PURCHLINEGROUPALL], [PURCHMULTILNGROUPGROUP], [PURCHMULTILNGROUPALL], [PURCHENDGROUPALL], [PURCHPRICEALLITEM], [PURCHLINEALLITEM], [PURCHLINEALLGROUP], [PURCHLINEALLALL], [PURCHMULTILNALLGROUP], [PURCHMULTILNALLALL], [PURCHENDALLALL], [KEY_], [MODIFIEDDATE], [MODIFIEDTIME], [MODIFIEDBY], [DATAAREAID]) VALUES (1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, N'LSR')
END

IF (@STORECOUNT = 0)
BEGIN
	IF NOT EXISTS (SELECT TOP 1 STOREID FROM RBOSTORETABLE WHERE STOREID = 'S0001' AND DATAAREAID = 'LSR')
	INSERT [dbo].[RBOSTORETABLE] ([STOREID], [NAME], [ADDRESS], [STREET], [ZIPCODE], [CITY], [COUNTY], [STATE], [COUNTRY], [PHONE], [MANAGERID], [OPENFROM], [OPENTO], [DIMENSION], [DIMENSION2_], [DIMENSION3_], [INVENTLOCATION], [STATEMENTMETHOD], [ONESTATEMENTPERDAY], [CURRENCY], [CLOSINGMETHOD], [ROUNDINGACCOUNT], [MAXIMUMPOSTINGDIFFERENCE], [MAXROUNDINGAMOUNT], [MAXSHIFTDIFFERENCEAMOUNT], [MAXTRANSACTIONDIFFERENCEAMOUNT], [STATEMENTNUMSEQ], [FUNCTIONALITYPROFILE], [CREATELABELSFORZEROPRICE], [TERMINALNUMSEQ], [STAFFNUMSEQ], [ITEMNUMSEQ], [DISCOUNTOFFERNUMSEQ], [MIXANDMATCHNUMSEQ], [MULTIBUYDISCOUNTNUMSEQ], [INVENTORYLOOKUP], [REMOVEADDTENDER], [TENDERDECLARATIONCALCULATION], [MAXIMUMTEXTLENGTHONRECEIPT], [NUMBEROFTOPORBOTTOMLINES], [ITEMIDONRECEIPT], [SERVICECHARGEPCT], [INCOMEEXEPENSEACCOUNT], [SERVICECHARGEPROMPT], [STATEMENTVOUCHERNUMSEQ], [TAXGROUP], [REPLICATIONCOUNTER], [ROUNDINGTAXACCOUNT], [MAXROUNDINGTAXAMOUNT], [CULTURENAME], [LAYOUTID], [MODIFIEDDATE], [MODIFIEDTIME], [MODIFIEDBY], [DATAAREAID], [SQLSERVERNAME], [DATABASENAME], [USERNAME], [PASSWORD], [DEFAULTCUSTACCOUNT], [USEDEFAULTCUSTACCOUNT], [WINDOWSAUTHENTICATION], [HIDETRAININGMODE]) VALUES (N'S0001', N'Demo store', N'', N'The Ring 123', N'35135', N'Liege', N'', N'', N'BE', N'', N'', 0, 0, N'', N'', N'', N'', 1, 0, N'EUR', 0, N'', CAST(0.000000000000 AS Numeric(28, 12)), CAST(0.000000000000 AS Numeric(28, 12)), CAST(0.000000000000 AS Numeric(28, 12)), CAST(0.000000000000 AS Numeric(28, 12)), N'', N'', 0, N'', N'', N'', N'', N'', N'', 0, N'', 0, 0, 0, 0, CAST(0.000000000000 AS Numeric(28, 12)), N'', N'', N'', N'', 0, N'', CAST(0.000000000000 AS Numeric(28, 12)), N'en-US', N'', CAST(0x0000000000000000 AS DateTime), 0, N'?', N'LSR', N'', N'', N'', N'', N'', 0, 0, 0)
END
IF (@TERMINALCOUNT = 0)
BEGIN	
	IF NOT EXISTS (SELECT TOP 1 TERMINALID FROM RBOTERMINALTABLE WHERE TERMINALID = '0001' AND DATAAREAID = 'LSR')
	INSERT [dbo].[RBOTERMINALTABLE] ([TERMINALID], [NAME], [STOREID], [LOCATION], [STATEMENTMETHOD], [TERMINALSTATEMENT], [NOTACTIVE], [CLOSINGSTATUS], [DISPLAYTERMINALCLOSED], [DISPLAYLINKEDITEM], [MANAGERKEYONRETURN], [SLIPIFRETURN], [OPENDRAWERATLILO], [ONLYTOTALINSUSPENDEDTRANS20015], [EXITAFTEREACHTRANSACTION], [AUTOLOGOFFTIMEOUT], [RETURNINTRANSACTION], [ITEMIDONRECEIPT], [EFTSTOREID], [EFTTERMINALID], [MAXRECEIPTTEXTLENGTH], [NUMBEROFTOPBOTTOMLINES], [RECEIPTSETUPLOCATION], [MAXDISPLAYTEXTLENGTH], [CUSTOMERDISPLAYTEXT1], [CUSTOMERDISPLAYTEXT2], [HARDWAREPROFILE], [VISUALPROFILE], [PRINTVATREFUNDCHECKS], [RECEIPTPRINTINGDEFAULTOFF], [RECEIPTBARCODE], [LASTZREPORTID], [LAYOUTID], [UPDATESERVICEPORT], [IPADDRESS], [MODIFIEDDATE], [MODIFIEDTIME], [MODIFIEDBY], [DATAAREAID], [FUNCTIONALITYPROFILE], [RECEIPTIDNUMBERSEQUENCE], [STANDALONE], [TRANSACTIONIDNUMBERSEQUENCE], [TRANSACTIONSERVICEPROFILE], [DatabaseName], [DatabaseServer], [DatabaseUserName], [DatabasePassword], [SALESTYPEFILTER]) VALUES (N'0001', N'DEMO POS', N'S0001', N'', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'', N'', 0, 0, 0, 0, N'Thank you very much', N'Please come again', N'', N'', 0, 0, 0, N'', N'', 0, N'', CAST(0x0000000000000000 AS DateTime), 0, N'?', N'LSR', NULL, NULL, 0, N'', N'', N'', N'', N'', N'', NULL)
END
IF (@STAFFCOUNT = 0)
BEGIN 
	IF NOT EXISTS (SELECT STAFFID FROM RBOSTAFFTABLE WHERE STAFFID = '101' AND DATAAREAID = 'LSR')
	INSERT [dbo].[RBOSTAFFTABLE] ([STAFFID], [NAME], [STOREID], [PASSWORD], [CHANGEPASSWORD], [ALLOWTRANSACTIONVOIDING], [MANAGERPRIVILEGES], [ALLOWXREPORTPRINTING], [ALLOWTENDERDECLARATION], [ALLOWFLOATINGDECLARATION], [PRICEOVERRIDE], [MAXDISCOUNTPCT], [ALLOWCHANGENOVOID], [ALLOWTRANSACTIONSUSPENSION], [ALLOWOPENDRAWERONLY], [FIRSTNAME], [LASTNAME], [EMPLOYMENTTYPE], [ADDRESS], [STREET], [ZIPCODE], [CITY], [COUNTY], [STATE], [COUNTRY], [DEL_PHONELOCAL], [PHONEWORK], [BLOCKED], [BLOCKINGDATE], [LEFTHANDED], [PERMISSIONGROUPID], [ZREPORTID], [NAMEONRECEIPT], [PAYROLLNUMBER], [CONTINUEONTSERRORS], [PHONEHOME], [MAXTOTALDISCOUNTPCT], [LAYOUTID], [MODIFIEDDATE], [MODIFIEDTIME], [MODIFIEDBY], [DATAAREAID], [VISUALPROFILE], [OPERATORCULTURE], [MAXLINEDISCOUNTAMOUNT], [MAXTOTALDISCOUNTAMOUNT], [MAXLINERETURNAMOUNT], [MAXTOTALRETURNAMOUNT]) VALUES (N'101', N'Holly Flynn', N'S0001', N'', 0, 1, 1, 1, 1, 1, 0, CAST(10.550000000000 AS Numeric(28, 12)), 1, 0, 0, N'', N'', 0, N'', N'', N'', N'', N'', N'', N'', N'', N'', 0, CAST(0x0000000000000000 AS DateTime), 0, N'', N'', N'', N'', 0, N'', CAST(10.550000000000 AS Numeric(28, 12)), N'', CAST(0x0000000000000000 AS DateTime), 0, N'?', N'LSR', N'', N'en-US', CAST(0.550000000000 AS Numeric(28, 12)), CAST(0.550000000000 AS Numeric(28, 12)), CAST(5.550000000000 AS Numeric(28, 12)), CAST(0.550000000000 AS Numeric(28, 12)))
END	
GO

declare @btnId int
select @btnId = COALESCE(MAX(ID), 0) + 1 from POSISBUTTONGRIDBUTTONS

--If no buttons exist for button grid DEF1 - then they are added
IF NOT EXISTS (SELECT TOP 1 ID FROM POSISBUTTONGRIDBUTTONS WHERE BUTTONGRIDID = 'DEF1' AND DATAAREAID = 'LSR')
BEGIN
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 1, 1, 1, 1, NULL, N'', NULL, N'', -1, NULL, NULL, N'DEF1', N'LSR', NULL)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 1, 4, 2, 1, 108, N'', -1, N'Item Search', 4, 22, 0, N'DEF1', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 2, 1, 1, 1, NULL, N'', NULL, N'', -1, NULL, NULL, N'DEF1', N'LSR', NULL)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 3, 1, 1, 1, NULL, N'', NULL, N'', -1, NULL, NULL, N'DEF1', N'LSR', NULL)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 4, 1, 1, 1, NULL, N'', NULL, N'', -1, NULL, NULL, N'DEF1', N'LSR', NULL)
	set @btnId = @btnId + 1
END

--If no buttons exist for button grid DEF2 - then they are added
IF NOT EXISTS (SELECT TOP 1 ID FROM POSISBUTTONGRIDBUTTONS WHERE BUTTONGRIDID = 'DEF2' AND DATAAREAID = 'LSR')
BEGIN
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 1, 1, 1, 1, 912, N'', -1, N'Design Mode Enable', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 3, 1, 3, 1, 101, N'', -1, N'Price Check', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 1, 1, 4, 1, 105, N'', -1, N'Set Qty', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 1, 1, 3, 1, 103, N'', -1, N'Item Comment', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 1, 1, 2, 1, 104, N'', -1, N'Price Override', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 2, 1, 3, 1, 115, N'', -1, N'Show Journal', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 3, 1, 1, 1, 301, N'', -1, N'Line Discount %', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 3, 1, 2, 1, 504, N'', -1, N'Recall Transaction', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 2, 1, 2, 1, 503, N'', -1, N'Suspend Transaction', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 4, 1, 3, 1, 703, N'', -1, N'Lock Terminal', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 4, 1, 1, 1, 109, N'', -1, N'Return Item', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 4, 1, 2, 1, 1052, N'', -1, N'Tender Declaration', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 2, 1, 1, 1, 303, N'', -1, N'Total Discount %', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 2, 1, 4, 1, 102, N'', -1, N'Void Item', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 3, 1, 4, 1, 500, N'', -1, N'Void Transaction', 0, 14, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 4, 1, 4, 1, 701, N'', -1, N'Log Off', 5, 16, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
END

--Make sure that the LogOff operation is on the DEF2 button grid
IF NOT EXISTS (SELECT TOP 1 ID FROM POSISBUTTONGRIDBUTTONS WHERE BUTTONGRIDID = 'DEF2' AND ACTION = 701 AND DATAAREAID = 'LSR')
BEGIN
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 4, 1, 4, 1, 701, N'', -1, N'Log Off', 5, 16, 0, N'DEF2', N'LSR', 2)
	set @btnId = @btnId + 1
END


--If no buttons exist for button grid DEF3 - then they are added
IF NOT EXISTS (SELECT TOP 1 ID FROM POSISBUTTONGRIDBUTTONS WHERE BUTTONGRIDID = 'DEF3' AND DATAAREAID = 'LSR')
BEGIN
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 1, 1, 1, 1, NULL, N'', NULL, N'', -1, NULL, NULL, N'DEF3', N'LSR', NULL)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 1, 1, 2, 1, NULL, N'', NULL, N'', -1, NULL, NULL, N'DEF3', N'LSR', NULL)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 1, 1, 3, 1, NULL, N'', NULL, N'', -1, NULL, NULL, N'DEF3', N'LSR', NULL)
	set @btnId = @btnId + 1
	INSERT [dbo].[POSISBUTTONGRIDBUTTONS] ([ID], [COL], [COLSPAN], [ROWNUMBER], [ROWSPAN], [ACTION], [ACTIONPROPERTY], [PICTUREID], [DISPLAYTEXT], [COLOUR], [FONTSIZE], [FONTSTYLE], [BUTTONGRIDID], [DATAAREAID], [IMAGEALIGNMENT]) VALUES (@btnId, 1, 1, 4, 1, NULL, N'', NULL, N'', -1, NULL, NULL, N'DEF3', N'LSR', NULL)
END
GO

--Create the button grids if they don't already exist
IF NOT EXISTS (SELECT BUTTONGRIDID FROM POSISBUTTONGRID WHERE BUTTONGRIDID = 'DEF1' AND DATAAREAID = 'LSR')
INSERT [dbo].[POSISBUTTONGRID] ([BUTTONGRIDID], [NAME], [SPACEBETWEENBUTTONS], [FONT], [KEYBOARDUSED], [DATAAREAID], [DEFAULTCOLOR], [DEFAULTFONTSIZE], [DEFAULTFONTSTYLE]) VALUES (N'DEF1', N'Default PLU', 0, N'', N'', N'LSR', 4, 16, 0)
IF NOT EXISTS (SELECT BUTTONGRIDID FROM POSISBUTTONGRID WHERE BUTTONGRIDID = 'DEF2' AND DATAAREAID = 'LSR')
INSERT [dbo].[POSISBUTTONGRID] ([BUTTONGRIDID], [NAME], [SPACEBETWEENBUTTONS], [FONT], [KEYBOARDUSED], [DATAAREAID], [DEFAULTCOLOR], [DEFAULTFONTSIZE], [DEFAULTFONTSTYLE]) VALUES (N'DEF2', N'Default Operations', 0, N'', N'', N'LSR', 5, 14, 0)
IF NOT EXISTS (SELECT BUTTONGRIDID FROM POSISBUTTONGRID WHERE BUTTONGRIDID = 'DEF3' AND DATAAREAID = 'LSR')
INSERT [dbo].[POSISBUTTONGRID] ([BUTTONGRIDID], [NAME], [SPACEBETWEENBUTTONS], [FONT], [KEYBOARDUSED], [DATAAREAID], [DEFAULTCOLOR], [DEFAULTFONTSIZE], [DEFAULTFONTSTYLE]) VALUES (N'DEF3', N'Default Payment', 0, N'', N'', N'LSR', 1, 16, 0)
GO

--If the DEFAULT layout doesn't already exist create it
IF NOT EXISTS (SELECT LAYOUTID FROM POSISTILLLAYOUT WHERE LAYOUTID = 'DEFAULT' AND DATAAREAID = 'LSR')
INSERT [dbo].[POSISTILLLAYOUT] ([LAYOUTID], [NAME], [WIDTH], [HEIGHT], [BUTTONGRID1], [BUTTONGRID2], [BUTTONGRID3], [BUTTONGRID4], [BUTTONGRID5], [RECEIPTID], [TOTALID], [CUSTOMERLAYOUTID], [DATAAREAID], [LOGOPICTUREID], [IMG_CUSTOMERLAYOUTXML], [IMG_RECEIPTITEMSLAYOUTXML], [IMG_RECEIPTPAYMENTLAYOUTXML], [IMG_TOTALSLAYOUTXML], [IMG_LAYOUTXML], [CUSTOMERLAYOUTXML], [RECEIPTITEMSLAYOUTXML], [RECEIPTPAYMENTLAYOUTXML], [TOTALSLAYOUTXML], [LAYOUTXML], [IMG_CASHCHANGERLAYOUTXML], [CASHCHANGERLAYOUTXML]) VALUES (N'DEFAULT', N'Default', 800, 600, N'DEF2', N'DEF1', N'DEF3', N'', N'', NULL, NULL, NULL, N'LSR', -1, NULL, NULL, NULL, NULL, NULL, N'ï»¿<XtraSerializer version="1.0" application="LayoutControl">
  <property name="#LayoutVersion" />
  <property name="Items" iskey="true" value="9">
    <property name="Item1" isnull="true" iskey="true">
      <property name="ExpandButtonVisible">false</property>
      <property name="Size">@3,Width=376@3,Height=177</property>
      <property name="Padding">0, 0, 0, 0</property>
      <property name="TextVisible">true</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="ExpandOnDoubleClick">false</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Expanded">true</property>
      <property name="ShowTabPageCloseButton">false</property>
      <property name="DefaultLayoutType">Vertical</property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="Name">Root</property>
      <property name="AppearanceTabPage" isnull="true" iskey="true">
        <property name="HeaderHotTracked" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="PageClient" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderDisabled" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="Header" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderActive" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
      </property>
      <property name="TextLocation">Top</property>
      <property name="ParentName" />
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="AppearanceGroup" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="CaptionImageLocation">Default</property>
      <property name="CaptionImageVisible">true</property>
      <property name="ExpandButtonLocation">Default</property>
      <property name="CaptionImageIndex">-1</property>
      <property name="TextToControlDistance">0</property>
      <property name="OptionsItemText" isnull="true" iskey="true">
        <property name="TextAlignMode">UseParentOptions</property>
        <property name="TextToControlDistance">3</property>
      </property>
      <property name="TabbedGroupParentName" />
      <property name="TypeName">LayoutGroup</property>
      <property name="GroupBordersVisible">true</property>
      <property name="EnableIndentsWithoutBorders">Default</property>
      <property name="AllowDrawBackground">true</property>
      <property name="CustomizationFormText">Customer</property>
      <property name="Text">Customer</property>
      <property name="Visibility">Always</property>
    </property>
    <property name="Item2" isnull="true" iskey="true">
      <property name="ExpandButtonVisible">false</property>
      <property name="Size">@3,Width=199@3,Height=155</property>
      <property name="Padding">0, 0, 0, 0</property>
      <property name="TextVisible">true</property>
      <property name="Spacing">2, 2, 2, 2</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="ExpandOnDoubleClick">false</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Expanded">true</property>
      <property name="ShowTabPageCloseButton">false</property>
      <property name="DefaultLayoutType">Vertical</property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="Name">layGrCustomer</property>
      <property name="AppearanceTabPage" isnull="true" iskey="true">
        <property name="HeaderHotTracked" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="PageClient" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderDisabled" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="Header" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderActive" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
      </property>
      <property name="TextLocation">Left</property>
      <property name="ParentName">Root</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="AppearanceGroup" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="CaptionImageLocation">Default</property>
      <property name="CaptionImageVisible">true</property>
      <property name="ExpandButtonLocation">Default</property>
      <property name="CaptionImageIndex">-1</property>
      <property name="TextToControlDistance">0</property>
      <property name="OptionsItemText" isnull="true" iskey="true">
        <property name="TextAlignMode">UseParentOptions</property>
        <property name="TextToControlDistance">3</property>
      </property>
      <property name="TabbedGroupParentName" />
      <property name="TypeName">LayoutGroup</property>
      <property name="GroupBordersVisible">true</property>
      <property name="EnableIndentsWithoutBorders">Default</property>
      <property name="AllowDrawBackground">true</property>
      <property name="CustomizationFormText">Customer</property>
      <property name="Text">Invoice</property>
      <property name="Visibility">Always</property>
    </property>
    <property name="Item3" isnull="true" iskey="true">
      <property name="ParentName">layGrCustomer</property>
      <property name="CustomizationFormText">Invoice Address</property>
      <property name="Text">Address</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=30@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=36</property>
      <property name="Size">@3,Width=173@3,Height=113</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layInvoiceAddress</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblInvAddress</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item4" isnull="true" iskey="true">
      <property name="ParentName">layGrCustomer</property>
      <property name="CustomizationFormText">InvoiceName</property>
      <property name="Text">Name</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=30@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="Size">@3,Width=173@2,Height=36</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layInvoiceName</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblInvName</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item5" isnull="true" iskey="true">
      <property name="ExpandButtonVisible">false</property>
      <property name="Size">@3,Width=175@3,Height=155</property>
      <property name="Padding">0, 0, 0, 0</property>
      <property name="TextVisible">true</property>
      <property name="Spacing">2, 2, 2, 2</property>
      <property name="Location">@3,X=199@1,Y=0</property>
      <property name="ExpandOnDoubleClick">false</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Expanded">true</property>
      <property name="ShowTabPageCloseButton">false</property>
      <property name="DefaultLayoutType">Vertical</property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="Name">layGrShippingAddr</property>
      <property name="AppearanceTabPage" isnull="true" iskey="true">
        <property name="HeaderHotTracked" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="PageClient" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderDisabled" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="Header" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderActive" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
      </property>
      <property name="TextLocation">Left</property>
      <property name="ParentName">Root</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="AppearanceGroup" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="CaptionImageLocation">Default</property>
      <property name="CaptionImageVisible">true</property>
      <property name="ExpandButtonLocation">Default</property>
      <property name="CaptionImageIndex">-1</property>
      <property name="TextToControlDistance">0</property>
      <property name="OptionsItemText" isnull="true" iskey="true">
        <property name="TextAlignMode">UseParentOptions</property>
        <property name="TextToControlDistance">3</property>
      </property>
      <property name="TabbedGroupParentName" />
      <property name="TypeName">LayoutGroup</property>
      <property name="GroupBordersVisible">true</property>
      <property name="EnableIndentsWithoutBorders">Default</property>
      <property name="AllowDrawBackground">true</property>
      <property name="CustomizationFormText">Shipping Address</property>
      <property name="Text">Shipping address</property>
      <property name="Visibility">Always</property>
    </property>
    <property name="Item6" isnull="true" iskey="true">
      <property name="ParentName">layGrShippingAddr</property>
      <property name="CustomizationFormText">Address</property>
      <property name="Text">Address</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=30@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=36</property>
      <property name="Size">@3,Width=149@3,Height=113</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layAddress</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblAddress</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item7" isnull="true" iskey="true">
      <property name="ParentName">layGrShippingAddr</property>
      <property name="CustomizationFormText">Name</property>
      <property name="Text">Name</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=30@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="Size">@3,Width=149@2,Height=36</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layName</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblName</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item8" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Customer Id</property>
      <property name="Text">Customer Id</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=109@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@2,X=30@2,Y=30</property>
      <property name="Size">@3,Width=109@2,Height=96</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layCustomerId</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblCustomerId</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@2,Width=74@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">5</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item9" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">InvoiceAccount</property>
      <property name="Text">Invoice account</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=109@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@2,X=52@2,Y=30</property>
      <property name="Size">@3,Width=109@2,Height=96</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layInvoiceAccount</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblInvAccount</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@2,Width=74@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">5</property>
      <property name="TextLocation">Left</property>
    </property>
  </property>
  <property name="LookAndFeel" isnull="true" iskey="true">
    <property name="UseDefaultLookAndFeel">true</property>
    <property name="UseWindowsXPTheme">false</property>
    <property name="SkinName">Money Twins</property>
    <property name="Style">Skin</property>
  </property>
</XtraSerializer>', N'ï»¿<XtraSerializer version="1.0" application="View">
  <property name="#LayoutVersion" />
  <property name="ActiveFilterEnabled">true</property>
  <property name="Columns" iskey="true" value="20">
    <property name="Item1" isnull="true" iskey="true">
      <property name="Name">gcVat</property>
    </property>
    <property name="Item2" isnull="true" iskey="true">
      <property name="VisibleIndex">0</property>
      <property name="Visible">true</property>
      <property name="Width">247</property>
      <property name="Name">gcItemName</property>
    </property>
    <property name="Item3" isnull="true" iskey="true">
      <property name="VisibleIndex">1</property>
      <property name="Visible">true</property>
      <property name="Width">71</property>
      <property name="Name">gcQty</property>
    </property>
    <property name="Item4" isnull="true" iskey="true">
      <property name="VisibleIndex">2</property>
      <property name="Visible">true</property>
      <property name="Width">98</property>
      <property name="Name">gcTotalPrice</property>
    </property>
    <property name="Item5" isnull="true" iskey="true">
      <property name="Width">72</property>
      <property name="Name">gcTotalPriceWithoutTax</property>
    </property>
    <property name="Item6" isnull="true" iskey="true">
      <property name="Name">gcVoided</property>
    </property>
    <property name="Item7" isnull="true" iskey="true">
      <property name="Name">gcTaxRatePct</property>
    </property>
    <property name="Item8" isnull="true" iskey="true">
      <property name="Name">gcTaxAmount</property>
    </property>
    <property name="Item9" isnull="true" iskey="true">
      <property name="Name">gcBarcodeId</property>
    </property>
    <property name="Item10" isnull="true" iskey="true">
      <property name="Name">gcItemId</property>
    </property>
    <property name="Item11" isnull="true" iskey="true">
      <property name="Name">gcTaxCode</property>
    </property>
    <property name="Item12" isnull="true" iskey="true">
      <property name="Name">gcLineId</property>
    </property>
    <property name="Item13" isnull="true" iskey="true">
      <property name="Name">gcStandardRetailPrice</property>
    </property>
    <property name="Item14" isnull="true" iskey="true">
      <property name="Name">gcOriginalPrice</property>
    </property>
    <property name="Item15" isnull="true" iskey="true">
      <property name="Name">gcColorName</property>
    </property>
    <property name="Item16" isnull="true" iskey="true">
      <property name="Name">gcSizeName</property>
    </property>
    <property name="Item17" isnull="true" iskey="true">
      <property name="Name">gcStyleName</property>
    </property>
    <property name="Item18" isnull="true" iskey="true">
      <property name="Name">gcOffer</property>
    </property>
    <property name="Item19" isnull="true" iskey="true">
      <property name="Name">gcLinkedItem</property>
    </property>
    <property name="Item20" isnull="true" iskey="true">
      <property name="Name">gcCorporateCard</property>
    </property>
  </property>
  <property name="OptionsView" isnull="true" iskey="true">
    <property name="ShowHorzLines">false</property>
    <property name="ShowIndicator">false</property>
    <property name="ShowGroupPanel">false</property>
    <property name="ShowPreview">true</property>
    <property name="ShowFilterPanelMode">Never</property>
    <property name="AutoCalcPreviewLineCount">true</property>
  </property>
  <property name="ActiveFilterString" />
  <property name="GroupSummarySortInfoState" />
</XtraSerializer>', N'ï»¿<XtraSerializer version="1.0" application="View">
  <property name="#LayoutVersion" />
  <property name="ActiveFilterEnabled">true</property>
  <property name="Columns" iskey="true" value="4">
    <property name="Item1" isnull="true" iskey="true">
      <property name="VisibleIndex">0</property>
      <property name="Visible">true</property>
      <property name="Width">100</property>
      <property name="Name">gcPayment</property>
    </property>
    <property name="Item2" isnull="true" iskey="true">
      <property name="VisibleIndex">1</property>
      <property name="Visible">true</property>
      <property name="Width">214</property>
      <property name="Name">gcExtraInfo</property>
    </property>
    <property name="Item3" isnull="true" iskey="true">
      <property name="VisibleIndex">2</property>
      <property name="Visible">true</property>
      <property name="Width">92</property>
      <property name="Name">gcTotal</property>
    </property>
    <property name="Item4" isnull="true" iskey="true">
      <property name="Name">gcVoidPayment</property>
    </property>
  </property>
  <property name="OptionsView" isnull="true" iskey="true">
    <property name="ShowHorzLines">false</property>
    <property name="ShowIndicator">false</property>
    <property name="ShowGroupPanel">false</property>
    <property name="ShowPreview">true</property>
    <property name="AutoCalcPreviewLineCount">true</property>
  </property>
  <property name="ActiveFilterString" />
  <property name="GroupSummarySortInfoState" />
</XtraSerializer>', N'ï»¿<XtraSerializer version="1.0" application="LayoutControl">
  <property name="#LayoutVersion" />
  <property name="Items" iskey="true" value="17">
    <property name="Item1" isnull="true" iskey="true">
      <property name="ExpandButtonVisible">false</property>
      <property name="Size">@3,Width=353@3,Height=164</property>
      <property name="Padding">0, 0, 0, 0</property>
      <property name="TextVisible">false</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="ExpandOnDoubleClick">false</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Expanded">true</property>
      <property name="ShowTabPageCloseButton">false</property>
      <property name="DefaultLayoutType">Vertical</property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="Name">Root</property>
      <property name="AppearanceTabPage" isnull="true" iskey="true">
        <property name="HeaderHotTracked" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="PageClient" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderDisabled" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="Header" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderActive" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
      </property>
      <property name="TextLocation">Top</property>
      <property name="ParentName" />
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="AppearanceGroup" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="CaptionImageLocation">Default</property>
      <property name="CaptionImageVisible">true</property>
      <property name="ExpandButtonLocation">Default</property>
      <property name="CaptionImageIndex">-1</property>
      <property name="TextToControlDistance">0</property>
      <property name="OptionsItemText" isnull="true" iskey="true">
        <property name="TextAlignMode">UseParentOptions</property>
        <property name="TextToControlDistance">3</property>
      </property>
      <property name="TabbedGroupParentName" />
      <property name="TypeName">LayoutGroup</property>
      <property name="GroupBordersVisible">true</property>
      <property name="EnableIndentsWithoutBorders">Default</property>
      <property name="AllowDrawBackground">true</property>
      <property name="CustomizationFormText">Totals</property>
      <property name="Text">Totals</property>
      <property name="Visibility">Always</property>
    </property>
    <property name="Item2" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Tax</property>
      <property name="Text">Tax</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=106@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@3,X=176@2,Y=44</property>
      <property name="Size">@3,Width=175@2,Height=35</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCITax</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblTax</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@2,Width=73@2,Height=13</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">3</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item3" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">No of item lines</property>
      <property name="Text">No of item lines</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=106@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=44</property>
      <property name="Size">@3,Width=176@2,Height=35</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCINoOfItemLines</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblNoOfItemLines</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@2,Width=73@2,Height=13</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">3</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item4" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Total discount</property>
      <property name="Text">Total discount</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=106@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="Size">@3,Width=176@2,Height=44</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCITotalDiscount</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblTotalDiscount</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@2,Width=73@2,Height=13</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">3</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item5" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Subtotal</property>
      <property name="Text">Subtotal</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=106@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@3,X=176@1,Y=0</property>
      <property name="Size">@3,Width=175@2,Height=44</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCINetAmount</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblNetAmount</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@2,Width=73@2,Height=13</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">3</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item6" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Payment</property>
      <property name="Text">Payment</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=106@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=79</property>
      <property name="Size">@3,Width=351@2,Height=34</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCIPayment</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblPayment</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@2,Width=73@2,Height=13</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">3</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item7" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Balance</property>
      <property name="Text">Balance</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=106@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@3,Y=113</property>
      <property name="Size">@3,Width=351@2,Height=49</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCIBalance</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblBalance</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@2,Width=73@2,Height=13</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">3</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item8" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Amount</property>
      <property name="Text">Amount</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=194@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=90</property>
      <property name="Size">@3,Width=334@2,Height=30</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCIGrossAmount</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblGrossAmount</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@3,Width=159@2,Height=20</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">5</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item9" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Subtotal with tax</property>
      <property name="Text">Subtotal with tax</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=194@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=90</property>
      <property name="Size">@3,Width=334@2,Height=30</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCINetAmountWithTax</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblNetAmountWithTax</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@3,Width=159@2,Height=20</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">5</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item10" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Rounded</property>
      <property name="Text">Rounded</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=30@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@3,Y=240</property>
      <property name="Size">@3,Width=334@2,Height=30</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCIRounded</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblRounded</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@2,Height=20</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item11" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">No of items</property>
      <property name="Text">No of items</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=30@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@3,Y=240</property>
      <property name="Size">@3,Width=334@2,Height=30</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCINoOfItems</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblNoOfItems</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@2,Height=20</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item12" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Sum of discounts</property>
      <property name="Text">Sum of discounts</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=30@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@3,Y=240</property>
      <property name="Size">@3,Width=334@2,Height=30</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCISumOfDiscounts</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblSumOfDiscounts</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@2,Height=20</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item13" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Discount</property>
      <property name="Text">Discount</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=153@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=60</property>
      <property name="Size">@3,Width=334@2,Height=30</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCISumOfDiscountsWithTax</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblSumOfDiscountsWithTax</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@3,Width=118@2,Height=20</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">5</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item14" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Amount with tax</property>
      <property name="Text">Amount with tax</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=153@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=60</property>
      <property name="Size">@3,Width=334@2,Height=30</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCIGrossAmountWithTax</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblGrossAmountWithTax</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@3,Width=118@2,Height=20</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">5</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item15" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Line discount with tax</property>
      <property name="Text">Line discount with tax</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=153@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=90</property>
      <property name="Size">@3,Width=334@2,Height=30</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCILineDiscountWithTax</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblLineDiscountWithTax</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@3,Width=118@2,Height=20</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">5</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item16" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Total discount with tax</property>
      <property name="Text">Total Discount with tax</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=153@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@3,Y=120</property>
      <property name="Size">@3,Width=334@2,Height=30</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCITotalDiscountWithTax</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblTotalDiscountWithTax</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@3,Width=118@2,Height=20</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">5</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item17" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Line discount</property>
      <property name="Text">Line discount</property>
      <property name="TextVisible">true</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=153@2,Height=30</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=90</property>
      <property name="Size">@3,Width=351@2,Height=54</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">5, 5, 5, 5</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutCILineDiscount</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblLineDiscount</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@3,Width=118@2,Height=20</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">5</property>
      <property name="TextLocation">Left</property>
    </property>
  </property>
  <property name="LookAndFeel" isnull="true" iskey="true">
    <property name="UseDefaultLookAndFeel">true</property>
    <property name="UseWindowsXPTheme">false</property>
    <property name="SkinName">Money Twins</property>
    <property name="Style">Skin</property>
  </property>
</XtraSerializer>', N'ï»¿<XtraSerializer version="1.0" application="LayoutControl">
  <property name="#LayoutVersion" />
  <property name="Items" iskey="true" value="16">
    <property name="Item1" isnull="true" iskey="true">
      <property name="ExpandButtonVisible">false</property>
      <property name="Size">@4,Width=1680@3,Height=770</property>
      <property name="Padding">9, 9, 9, 9</property>
      <property name="TextVisible">false</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="ExpandOnDoubleClick">false</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Expanded">true</property>
      <property name="ShowTabPageCloseButton">false</property>
      <property name="DefaultLayoutType">Vertical</property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="Name">Root</property>
      <property name="AppearanceTabPage" isnull="true" iskey="true">
        <property name="HeaderHotTracked" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="PageClient" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderDisabled" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="Header" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderActive" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
      </property>
      <property name="TextLocation">Top</property>
      <property name="ParentName" />
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="AppearanceGroup" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="CaptionImageLocation">Default</property>
      <property name="CaptionImageVisible">true</property>
      <property name="ExpandButtonLocation">Default</property>
      <property name="CaptionImageIndex">-1</property>
      <property name="TextToControlDistance">0</property>
      <property name="OptionsItemText" isnull="true" iskey="true">
        <property name="TextAlignMode">UseParentOptions</property>
        <property name="TextToControlDistance">3</property>
      </property>
      <property name="TabbedGroupParentName" />
      <property name="TypeName">LayoutGroup</property>
      <property name="GroupBordersVisible">true</property>
      <property name="EnableIndentsWithoutBorders">Default</property>
      <property name="AllowDrawBackground">true</property>
      <property name="CustomizationFormText">layoutControlGroup1</property>
      <property name="Text">Root</property>
      <property name="Visibility">Always</property>
    </property>
    <property name="Item2" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Receipt</property>
      <property name="Text">Receipt</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=50</property>
      <property name="Size">@3,Width=483@3,Height=700</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemReceipt</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">receipt</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item3" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Message</property>
      <property name="Text">Message</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="Size">@3,Width=483@2,Height=50</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemMessage</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">messages</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item4" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Numpad</property>
      <property name="Text">Numpad</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@3,X=483@3,Y=165</property>
      <property name="Size">@3,Width=445@3,Height=402</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemNumPad</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">numPad</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item5" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Customer</property>
      <property name="Text">Customer</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@3,X=483@1,Y=0</property>
      <property name="Size">@3,Width=445@3,Height=165</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemCustomer</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">customer</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item6" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">ButtonGrid1</property>
      <property name="Text">ButtonGrid1</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@3,X=928@3,Y=375</property>
      <property name="Size">@3,Width=456@3,Height=375</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemButtonGrid1</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">buttonGrid1</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item7" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">ButtonGrid2</property>
      <property name="Text">ButtonGrid2</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@3,X=928@1,Y=0</property>
      <property name="Size">@3,Width=732@3,Height=375</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemButtonGrid2</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">buttonGrid2</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item8" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Totals</property>
      <property name="Text">Totals</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@3,X=483@3,Y=567</property>
      <property name="Size">@3,Width=445@3,Height=183</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemTotals</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">totals</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item9" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">ButtonGrid3</property>
      <property name="Text">ButtonGrid3</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@4,X=1384@3,Y=375</property>
      <property name="Size">@3,Width=276@3,Height=375</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemButtonGrid3</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">buttonGrid3</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item10" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Forecourt Control</property>
      <property name="Text">Forecourt Control</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="Size">@3,Width=727@3,Height=551</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemForecourtControl</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">forecourtControl</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item11" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">ButtonGrid5</property>
      <property name="Text">ButtonGrid5</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@3,X=333@1,Y=0</property>
      <property name="Size">@3,Width=504@3,Height=131</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemButtonGrid5</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">buttonGrid5</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item12" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">ButtonGrid4</property>
      <property name="Text">ButtonGrid4</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@3,X=333@1,Y=0</property>
      <property name="Size">@3,Width=504@3,Height=252</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemButtonGrid4</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">buttonGrid4</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item13" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Logo</property>
      <property name="Text">Logo</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=50@2,Height=50</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="Size">@3,Width=756@3,Height=578</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemLogo</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">logo</property>
      <property name="SizeConstraintsType">Custom</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item14" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Keyboard InputControl</property>
      <property name="Text">Keyboard InputControl</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=104@2,Height=24</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="Size">@3,Width=756@3,Height=578</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layoutControlItem1</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">keyboardInputControl</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item15" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Dynakeyboard</property>
      <property name="Text">Dynakeyboard</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=104@2,Height=24</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="Size">@3,Width=756@3,Height=578</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemKeyboardButtonControl</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">keyboardButtonControl</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item16" isnull="true" iskey="true">
      <property name="ParentName">Customization</property>
      <property name="CustomizationFormText">Cash changer</property>
      <property name="Text">Cash changer</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=104@2,Height=24</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="Size">@3,Width=758@3,Height=580</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">itemCashChanger</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">cashChanger</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
  </property>
  <property name="LookAndFeel" isnull="true" iskey="true">
    <property name="UseDefaultLookAndFeel">true</property>
    <property name="UseWindowsXPTheme">false</property>
    <property name="SkinName">Money Twins</property>
    <property name="Style">Skin</property>
  </property>
</XtraSerializer>', NULL, N'ï»¿<XtraSerializer version="1.0" application="LayoutControl">
  <property name="#LayoutVersion" />
  <property name="Items" iskey="true" value="8">
    <property name="Item1" isnull="true" iskey="true">
      <property name="ExpandButtonVisible">false</property>
      <property name="Size">@3,Width=499@3,Height=348</property>
      <property name="Padding">9, 9, 9, 9</property>
      <property name="TextVisible">false</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="ExpandOnDoubleClick">false</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Expanded">true</property>
      <property name="ShowTabPageCloseButton">false</property>
      <property name="DefaultLayoutType">Vertical</property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="Name">Root</property>
      <property name="AppearanceTabPage" isnull="true" iskey="true">
        <property name="HeaderHotTracked" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="PageClient" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderDisabled" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="Header" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
        <property name="HeaderActive" isnull="true" iskey="true">
          <property name="Font">Tahoma, 8.25pt</property>
          <property name="BackColor2" />
          <property name="BackColor" />
          <property name="ForeColor" />
          <property name="BorderColor" />
          <property name="GradientMode">Horizontal</property>
        </property>
      </property>
      <property name="TextLocation">Top</property>
      <property name="ParentName" />
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="AppearanceGroup" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="CaptionImageLocation">Default</property>
      <property name="CaptionImageVisible">true</property>
      <property name="ExpandButtonLocation">Default</property>
      <property name="CaptionImageIndex">-1</property>
      <property name="TextToControlDistance">0</property>
      <property name="OptionsItemText" isnull="true" iskey="true">
        <property name="TextAlignMode">UseParentOptions</property>
        <property name="TextToControlDistance">3</property>
      </property>
      <property name="TabbedGroupParentName" />
      <property name="TypeName">LayoutGroup</property>
      <property name="GroupBordersVisible">true</property>
      <property name="EnableIndentsWithoutBorders">Default</property>
      <property name="AllowDrawBackground">true</property>
      <property name="CustomizationFormText">Cash changer layout</property>
      <property name="Text">Cash changer layout</property>
      <property name="Visibility">Always</property>
    </property>
    <property name="Item2" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Free text</property>
      <property name="Text">layFreeText</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=24@2,Height=24</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@1,Y=0</property>
      <property name="Size">@3,Width=479@2,Height=39</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layFreeText</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblText</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item3" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Amount inserted</property>
      <property name="Text">layAmountInserted</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=24@2,Height=24</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=39</property>
      <property name="Size">@3,Width=479@2,Height=56</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layAmountInserted</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblAmountInserted</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item4" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Change text</property>
      <property name="Text">layChangeText</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=24@2,Height=24</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@2,Y=95</property>
      <property name="Size">@3,Width=322@2,Height=39</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layChangeText</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblChange</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item5" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Change amount</property>
      <property name="Text">layChangeAmount</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@2,Width=24@2,Height=24</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@3,X=322@2,Y=95</property>
      <property name="Size">@3,Width=157@2,Height=39</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layChangeAmount</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">lblChangeAmount</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item6" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Insert amount button</property>
      <property name="Text">layInsertButton</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=104@2,Height=24</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@3,Y=134</property>
      <property name="Size">@3,Width=479@2,Height=75</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layInsertButton</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">btnRegisterAmount</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item7" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">Operation buttons</property>
      <property name="Text">layFunctionButtons</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=104@2,Height=24</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@3,X=204@3,Y=209</property>
      <property name="Size">@3,Width=275@3,Height=119</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layFunctionButtons</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">btnFunctionButtons</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
    <property name="Item8" isnull="true" iskey="true">
      <property name="ParentName">Root</property>
      <property name="CustomizationFormText">layStatusImage</property>
      <property name="Text">layStatusImage</property>
      <property name="TextVisible">false</property>
      <property name="ImageToTextDistance">5</property>
      <property name="MaxSize">@1,Width=0@1,Height=0</property>
      <property name="MinSize">@3,Width=104@2,Height=24</property>
      <property name="ShowInCustomizationForm">true</property>
      <property name="Location">@1,X=0@3,Y=209</property>
      <property name="Size">@3,Width=204@3,Height=119</property>
      <property name="AppearanceItemCaption" isnull="true" iskey="true">
        <property name="Font">Tahoma, 8.25pt</property>
        <property name="BackColor2" />
        <property name="BackColor" />
        <property name="ForeColor" />
        <property name="BorderColor" />
        <property name="GradientMode">Horizontal</property>
      </property>
      <property name="OptionsCustomization" isnull="true" iskey="true">
        <property name="AllowDrop">Default</property>
        <property name="AllowDrag">Default</property>
      </property>
      <property name="OptionsToolTip" isnull="true" iskey="true">
        <property name="IconToolTipTitle" />
        <property name="IconToolTipIconType">None</property>
        <property name="EnableIconToolTip">true</property>
        <property name="IconToolTip" />
        <property name="ToolTip" />
        <property name="ToolTipTitle" />
        <property name="ToolTipIconType">None</property>
      </property>
      <property name="ImageAlignment">MiddleLeft</property>
      <property name="Padding">2, 2, 2, 2</property>
      <property name="TextAlignMode">UseParentOptions</property>
      <property name="Spacing">0, 0, 0, 0</property>
      <property name="AllowHtmlStringInCaption">false</property>
      <property name="Name">layStatusImage</property>
      <property name="TypeName">LayoutControlItem</property>
      <property name="ControlName">panel1</property>
      <property name="SizeConstraintsType">Default</property>
      <property name="TextSize">@1,Width=0@1,Height=0</property>
      <property name="Visibility">Always</property>
      <property name="ImageIndex">-1</property>
      <property name="Image" isnull="true" />
      <property name="TextToControlDistance">0</property>
      <property name="TextLocation">Left</property>
    </property>
  </property>
  <property name="LookAndFeel" isnull="true" iskey="true">
    <property name="UseDefaultLookAndFeel">true</property>
    <property name="UseWindowsXPTheme">false</property>
    <property name="SkinName">Caramel</property>
    <property name="Style">Skin</property>
  </property>
</XtraSerializer>')
GO

