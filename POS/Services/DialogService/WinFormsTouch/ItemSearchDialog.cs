using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Progress;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.Controls.Themes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.POS.Processes.Common;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses.Price;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.ListViewExtensions;
using LSOne.Services.Properties;
using LSOne.Services.SupportClasses;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using ListView = LSOne.Controls.ListView;

namespace LSOne.Services.WinFormsTouch
{
    public partial class ItemSearchDialog : TouchBaseForm
    {
        private static Guid ViewModeSettingID = new Guid("ad6b03fc-7670-4293-a13b-ddd7a93cd9ab");

        private enum Buttons
        {
            PageUp,
            ArrowUp,
            ArrowDown,
            PageDown,
            Clear,
            Select,
            ShowPrice,
            Close,
            ToggleView
        }
        
        private ItemSearchViewModeEnum viewMode;
        private int maxNumberOfEntriesPerQuery;
        private bool taxIncludedInPrice;
        private bool rememberViewMode;
        private Setting viewModeSetting;
        private bool showPrice;
        private bool showVariant;
        private bool showingPrice;
        private bool showingVariant;
        private bool reachedTheEnd;
        private Currency storeCurrency;
        private DecimalLimit decimalLlimit;
        private SimpleRetailItem selectedItem;
        private LoadingIndicator progress;
        private ColumnCollection listColumnCollection;
        private ColumnCollection imageColumnCollection;
        private int lastEntry;
        private int loadedRows;
        private string searchParameters;
        private List<SimpleRetailItem> items;
        private Cell previouslySelectedCell;
        private ImageCellDictionaryManager imageCellManager;
        private Task imageLoadingTask;
        private Timer invalidateTimer;
        private Image tickMarkImage;
        private int prevTopRow;
        private int prevBottomRow;
        private IPosTransaction currentTransaction;
        private OperationInfo operationInfo;
        private ItemSearchFilter itemSearchFilter;
        private bool includeCannotBeSold;

        public ItemSearchDialog()
        {
            InitializeComponent();

            listColumnCollection = lvItems.Columns;
            imageColumnCollection = new ColumnCollection();

            CreateImageColumnCollection();

            touchKeyboard1.BuddyControl = tbSearch;
            lvItems.ApplyTheme(new LSOneTouchTheme());
            items = new List<SimpleRetailItem>();
            imageCellManager = new ImageCellDictionaryManager(DLLEntry.DataModel);

            invalidateTimer = new Timer();
            invalidateTimer.Interval = 200;
            invalidateTimer.Tick += InvalidateTimerOnTick;

            prevTopRow = -1;
            prevBottomRow = -1;

            tickMarkImage = Resources.TickCircle24;
            itemSearchFilter = new ItemSearchFilter();

            showVariant = false;
        }

        public ItemSearchDialog(int maxNumberOfEntriesPerQuery)
            : this()
        {
            this.maxNumberOfEntriesPerQuery = maxNumberOfEntriesPerQuery;

            taxIncludedInPrice = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).TaxIncludedInPrice;

            //Set the size of the form the same as the main form
            this.Width = DLLEntry.Settings.MainFormInfo.MainWindowWidth;
            this.Height = DLLEntry.Settings.MainFormInfo.MainWindowHeight;

            panel.AddButton("", Buttons.PageUp, Conversion.ToStr((int)Buttons.PageUp), image: Resources.Doublearrowupthin_32px);
            panel.AddButton("", Buttons.ArrowUp, Conversion.ToStr((int)Buttons.ArrowUp), image: Resources.Arrowupthin_32px);
            panel.AddButton("", Buttons.ArrowDown, Conversion.ToStr((int)Buttons.ArrowDown), image: Resources.Arrowdownthin_32px);
            panel.AddButton("", Buttons.PageDown, Conversion.ToStr((int)Buttons.PageDown), image: Resources.Doublearrowdownthin_32px);

            panel.AddButton(Resources.Clear, Buttons.Clear, Conversion.ToStr((int)Buttons.Clear), dock: DockEnum.DockEnd);
            panel.AddButton(Resources.ShowPrice, Buttons.ShowPrice, Conversion.ToStr((int)Buttons.ShowPrice), dock: DockEnum.DockEnd);

            if (DLLEntry.Settings.FunctionalityProfile.AllowImageViewInItemLookup)
            {
                panel.AddButton(Resources.Images, Buttons.ToggleView, Conversion.ToStr((int)Buttons.ToggleView), TouchButtonType.Action, DockEnum.DockEnd);
            }

            panel.AddButton(Resources.Select, Buttons.Select, Conversion.ToStr((int)Buttons.Select), TouchButtonType.OK, DockEnum.DockEnd);
            panel.AddButton(Resources.Close, Buttons.Close, Conversion.ToStr((int)Buttons.Close), TouchButtonType.Cancel, DockEnum.DockEnd);

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), false);
            POSFormsManager.SetAppearanceOfForm(this);

            if (DLLEntry.Settings.FunctionalityProfile.AllowImageViewInItemLookup)
            {
                ToggleViewMode();
            }
            else
            {
                viewMode = ItemSearchViewModeEnum.List;
            }

            lvItems.Columns[0].Tag = SortEnum.ID;
            lvItems.Columns[1].Tag = SortEnum.Description;
            lvItems.Columns[2].Tag = SortEnum.RetailGroup;

            tbSearch.Focus();
        }

        public ItemSearchDialog(int maxNumberOfEntriesPerQuery, ItemSearchViewModeEnum viewMode, RecordIdentifier retailGroupID, IPosTransaction currentTransaction, OperationInfo operationInfo = null)            
            : this(maxNumberOfEntriesPerQuery)
        {
            rememberViewMode = viewMode == ItemSearchViewModeEnum.Default;
            this.viewMode = rememberViewMode ? this.viewMode : viewMode;
            this.currentTransaction = currentTransaction;
            this.operationInfo = operationInfo;

            PopulateItemSearchFilter(RetailItemSearchEnum.RetailGroup, retailGroupID);
        }

        public ItemSearchDialog(int maxNumberOfEntriesPerQuery, ItemSearchViewModeEnum viewMode, RetailItemSearchEnum searchFilterType, RecordIdentifier searchFilterID, IPosTransaction currentTransaction, OperationInfo operationInfo = null, bool includeCannotBeSold = false)
            : this(maxNumberOfEntriesPerQuery)
        {
            rememberViewMode = viewMode == ItemSearchViewModeEnum.Default;
            this.viewMode = rememberViewMode ? this.viewMode : viewMode;
            this.currentTransaction = currentTransaction;
            this.operationInfo = operationInfo;
            this.includeCannotBeSold = includeCannotBeSold;

            PopulateItemSearchFilter(searchFilterType, searchFilterID);
        }

        internal Currency StoreCurrency
        {
            get
            {
                if (storeCurrency == null)
                {
                    storeCurrency = Providers.CurrencyData.Get(DLLEntry.DataModel,
                        ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.Currency,
                        CacheType.CacheTypeApplicationLifeTime);
                }
                return storeCurrency;
            }
        }

        internal DecimalLimit DecimalLimit
        {
            get
            {
                if (decimalLlimit == null)
                {
                    decimalLlimit = new DecimalLimit(StoreCurrency.RoundOffSales.DigitsBeforeFirstSignificantDigit());
                }

                return decimalLlimit;
            }
        }

        public SimpleRetailItem SelectedItem
        {
            get { return selectedItem ?? (selectedItem = new SimpleRetailItem()); }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Top = DLLEntry.Settings.MainFormInfo.MainWindowTop;
            Left = DLLEntry.Settings.MainFormInfo.MainWindowLeft;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            searchParameters = "";

            invalidateTimer.Start();

            LoadData();
            lvItems.SetSortColumn(1, true);
            tbSearch.Focus();

            if (!rememberViewMode)
            {
                // Simply apply the current viewMode
                ApplyColumnCollections();
            }
            else
            {
                // Check to see if we are remembering last used view mode
                if (DLLEntry.Settings.FunctionalityProfile.RememberListImageSelection)
                {
                    viewModeSetting = DLLEntry.DataModel.Settings.GetSetting(
                        DLLEntry.DataModel,
                        ViewModeSettingID,
                        SettingType.UISetting, ItemSearchViewModeEnum.List.ToString());

                    if (viewModeSetting.Value == ItemSearchViewModeEnum.Images.ToString())
                    {
                        ToggleViewMode();
                    }
                }
            }

            if(!showPrice && DLLEntry.Settings.FunctionalityProfile.ShowPricesByDefault)
            {
                GetPriceHandler();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (viewMode == ItemSearchViewModeEnum.Images)
            {
                ClearVisibleImageCells();
            }

            if (DLLEntry.Settings.FunctionalityProfile.AllowImageViewInItemLookup &&
                DLLEntry.Settings.FunctionalityProfile.RememberListImageSelection)
            {
                string newViewMode = viewMode.ToString();

                if (rememberViewMode && newViewMode != viewModeSetting.Value)
                {
                    viewModeSetting.UserSettingExists = true;
                    viewModeSetting.Value = newViewMode;

                    DLLEntry.DataModel.Settings.SaveSetting(DLLEntry.DataModel, ViewModeSettingID,
                        SettingsLevel.User,
                        viewModeSetting);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            try
            {
                if (touchKeyboard1 != null)
                {
                    if (Width > 1400)
                    {
                        touchKeyboard1.Location = new Point((Width - 1400) / 2, touchKeyboard1.Location.Y);
                        touchKeyboard1.Width = 1400;
                    }
                    else
                    {
                        touchKeyboard1.Location = new Point(11, touchKeyboard1.Location.Y);
                        touchKeyboard1.Width = Width - 22;
                    }
                }
            }
            catch
            {
                // We suppress this form sizing exeption
            }
        }
        
        private void InvalidateTimerOnTick(object sender, EventArgs eventArgs)
        {
            invalidateTimer.Stop();

            if (viewMode == ItemSearchViewModeEnum.Images)
            {
                if (imageCellManager.Invalidated)
                {
                    lvItems.InvalidateContent();
                    imageCellManager.Invalidated = false;
                }

                if (imageLoadingTask == null || imageLoadingTask.Status == TaskStatus.RanToCompletion)
                {
                    imageLoadingTask = Task.Run(() => imageCellManager.LoadImageRequests());
                }
            }

            invalidateTimer.Start();
        }

        private void ClearVisibleImageCells()
        {
            for (int i = lvItems.FirstRowOnScreen; i <= lvItems.FirstRowOnScreen + lvItems.RowCountOnScreen; i++)
            {
                Row row = lvItems.Row(i);

                if (row != null)
                {
                    for (uint j = 0; j < row.CellCount; j++)
                    {
                        Cell cell = row[j];

                        if (cell != null)
                        {
                            imageCellManager.ClearImageFromCell((ItemImageCell)cell);
                        }
                    }
                }
            }
        }

        private void ClearNonVisibleImageCells()
        {
            int rowStart = 0;
            int rowStop = 0;

            // Calculate the range of rows to clear
            if (lvItems.FirstRowOnScreen > 0 && lvItems.FirstRowOnScreen > prevTopRow)
            {
                // The user scrolled down
                rowStart = prevTopRow;
                rowStop = lvItems.FirstRowOnScreen - 1;
            }
            else if (lvItems.FirstRowOnScreen > 0 && lvItems.FirstRowOnScreen < prevTopRow)
            {
                // The user scrolled up so we clear from the bottom
                rowStart = lvItems.FirstRowOnScreen + lvItems.RowCountOnScreen + 1;
                rowStop = prevBottomRow;
            }

            for (int i = rowStart; i <= rowStop; i++)
            {
                Row row = lvItems.Row(i);

                if (row != null)
                {
                    for (uint j = 0; j < row.CellCount; j++)
                    {
                        Cell cell = row[j];

                        if (cell != null)
                        {
                            imageCellManager.ClearImageFromCell((ItemImageCell) cell);
                        }
                    }
                }
            }
        }

        private void LoadDataIntoListView(List<SimpleRetailItem> list)
        {
            if (viewMode == ItemSearchViewModeEnum.List)
            {
                LoadListData(list);
                lvItems.Columns[0].Sizable = true;
                lvItems.Columns[0].RelativeSize = 0;
                if (!showVariant)
				{
                    lvItems.Columns[2].RelativeSize = 4;
                } 
                else
				{
                    lvItems.Columns[3].RelativeSize = 4;
                }
                lvItems.AutoSizeColumns(true);
            }
            else if (viewMode == ItemSearchViewModeEnum.Images)
            {
                LoadImageData(list);
            }
        }

        private void LoadData()
        {
            if (progress == null)
            {
                EventHandler handler = (sender, args) =>
                {
                    LoadFromDataBase();
                };
                progress = LoadingIndicator.ShowOnParent(this, handler);
            }
        }

        private void LoadFromDataBase()
        {
            try
            {
                SortEnum currentSort = viewMode == ItemSearchViewModeEnum.Images ? SortEnum.Description : (SortEnum) lvItems.SortColumn.Tag + (lvItems.SortedAscending ? 0 : 100);

                List<SimpleRetailItem> list = Providers.RetailItemData.AdvancedSearch(DLLEntry.DataModel,
                                                                                  lastEntry + 1,
                                                                                  lastEntry + maxNumberOfEntriesPerQuery,
                                                                                  currentSort,
                                                                                  out int rows,
                                                                                  searchParameters,
                                                                                  false,
                                                                                  includeHeaders: true,
                                                                                  includeVariants: searchParameters != "",
                                                                                  retailGroupID: itemSearchFilter.RetailGroupID,
                                                                                  retailDepartmentID: itemSearchFilter.RetailDepartmentID,
                                                                                  taxGroupID: itemSearchFilter.TaxGroupID,
                                                                                  variantGroupID: itemSearchFilter.VariantGroupID,
                                                                                  vendorID: itemSearchFilter.VendorID,
                                                                                  barCode: itemSearchFilter.BarCode,
                                                                                  specialGroup: itemSearchFilter.SpecialGroupID,
                                                                                  itemTypeFilter: itemSearchFilter.ItemType,
                                                                                  includeCannotBeSold: includeCannotBeSold);

                string culture = DLLEntry.Settings.CompanyInfo.LanguageCode != DLLEntry.Settings.CultureName && DLLEntry.Settings.CompanyInfo.LanguageCode != "" ? DLLEntry.Settings.CultureName : "";
                if (culture != "")
                {
                    List<ItemTranslation> translations = Providers.ItemTranslationData.GetListByCultureName(DLLEntry.DataModel, culture, CacheType.CacheTypeApplicationLifeTime);

                    if (translations != null)
                    {
                        foreach (SimpleRetailItem item in list)
                        {
                            ItemTranslation translation = translations.Find(x => x.ItemID == item.ID);

                            if (translation != null)
                            {
                                item.Text = translation.Description;
                            }
                        }
                    }
                }

                items.AddRange(list);

                LoadDataIntoListView(list);
                loadedRows = list.Count;
                lastEntry += list.Count;
                reachedTheEnd = loadedRows < maxNumberOfEntriesPerQuery;
            }
            catch (Exception ex)
            {
                HideProgress();
                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowExceptionMessage(ex);
                loadedRows = 0;
            }
            finally
            {
                HideProgress();
            }
        }

        private void HideProgress()
        {
            if (progress != null)
            {
                Controls.Remove(progress);
                progress.Dispose();
                progress = null;
            }
        }

        private void LoadListData(List<SimpleRetailItem> list)
        {
            lvItems.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            lvItems.HeaderHeight = 30;

            if(list.Exists(x => x.VariantName != ""))
			{
                showVariant = true;
            }
            else
			{
                showVariant = false;
			}

            if (showVariant && !showingVariant)
			{
                AddVariantColumn();
            }
            else if (!showVariant && showingVariant)
            {
                RemoveVariantColumn();
            }

            if (showPrice && !showingPrice)
            {
                AddPriceColumn();
            }
            else if (!showPrice && showingPrice)
            {
                RemovePriceColumn();
            }
            Currency info = null;
            DecimalLimit limit = null;

            if (showPrice)
            {
                info = StoreCurrency;
                limit = DecimalLimit;
            }

            foreach (var current in list)
            {
                var row = new Row();
                row.AddText((string)current.ID);
                row.AddText(current.Text);

                if (showVariant)
				{
                    row.AddText(current.VariantName);
                }

                row.AddText(DLLEntry.GetGroupName(current.RetailGroupMasterID));

                if (showPrice)
                {
                    var priceInfo = GetPrice(current.ID);
                    if (priceInfo.Price != null)
                    {
                        row.AddText(GetPriceText(info, limit, (decimal)priceInfo.Price));
                    }
                }
                row.Tag = current;
                lvItems.AddRow(row);
            }
        }

        private string GetPriceText(Currency currency, DecimalLimit limit, decimal price)
        {
            return (currency.CurrencyPrefix +
                    " " +
                    price.FormatWithLimits(limit) +
                    " " +
                    currency.CurrencySuffix).Trim();
        }

        private void AddVariantColumn()
		{
            var var = new Column(Resources.Variant)
            {
                RelativeSize = 3,
                DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Left,
                Clickable = false
            };
            lvItems.Columns.Insert(2, var);
            showingVariant = true;
        }
        private void RemoveVariantColumn()
        {
            lvItems.Columns.RemoveAt(2);
            showingVariant = false;
        }
        private void AddPriceColumn()
        {
            var col = new Column(Resources.Price)
            {
                RelativeSize = 4,
                DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right,
                Clickable = false
            };
            lvItems.Columns.Add(col);

            showingPrice = true;
        }

        private void RemovePriceColumn()
        {
            if(showingVariant)
                lvItems.Columns.RemoveAt(4);
            else
                lvItems.Columns.RemoveAt(3);

            showingPrice = false;
        }

        private TradeAgreementPriceInfo GetPrice(RecordIdentifier itemID)
        {
            return Interfaces.Services.PriceService(DLLEntry.DataModel).GetPrice(
                DLLEntry.DataModel,
                itemID,
                RecordIdentifier.Empty,
                RecordIdentifier.Empty,
                DLLEntry.DataModel.CurrentStoreID,
                ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.Currency,
                RecordIdentifier.Empty,
                RecordIdentifier.Empty,
                taxIncludedInPrice,
                1,
                CacheType.CacheTypeTransactionLifeTime);
        }

        private void LoadImageData(List<SimpleRetailItem> list)
        {
            int columns = lvItems.Columns.Count;
            short rowHeight = (short) (lvItems.Width/columns);
            lvItems.SelectionStyle = ListView.SelectionStyleEnum.Hidden;
            lvItems.HeaderHeight = 1;
            lvItems.RowLines = true;            

            bool addingToLastRow = lvItems.RowCount > 0 && lvItems.Row(lvItems.RowCount - 1).CellCount < 8;
            int rowCellCounter = addingToLastRow ? lvItems.Row(lvItems.RowCount - 1).PopulatedCellCount : 0;
            Row row = addingToLastRow ? lvItems.Row(lvItems.RowCount - 1) : new Row();

            bool triggerCellAction = false;
            int triggerRowNumber = -1;
            int triggerRowColumn = -1;
            Cell triggerCell = null;

            for (int i = 0; i < list.Count; i++)
            {
                row.Height = rowHeight;

                SimpleRetailItem item = list[i];

                ItemImageCell cell = new ItemImageCell(null, item.Text, item.VariantName, item, imageCellManager, tickMarkImage);
                var priceInfo = GetPrice(item.ID);
                if (showPrice && priceInfo.Price != null)
                {
                    cell.PriceText = GetPriceText(StoreCurrency, DecimalLimit, (decimal)priceInfo.Price);
                }

                if (addingToLastRow)
                {
                    row[(uint) rowCellCounter] = cell;
                }
                else
                {
                    row.AddCell(cell);
                }
                
                if (previouslySelectedCell != null && ((ItemImageCell)previouslySelectedCell).Focused && ((ItemImageCell)previouslySelectedCell).RetailItem.ID == item.ID)
                {

                    triggerCellAction = true;
                    triggerRowNumber = lvItems.RowCount;
                    triggerRowColumn = i;
                    triggerCell = cell;
                }

                if ((rowCellCounter + 1) % 8 == 0 || i + 1 == list.Count)
                {
                    if (!addingToLastRow)
                    {
                        lvItems.AddRow(row);
                    }
                    else
                    {
                        // We've finished adding cells to the last row and so we can stop
                        addingToLastRow = false;
                    }
                    
                    row = new Row();
                }

                rowCellCounter++;
            }

            lvItems.ApplyRelativeColumnSize();

            if (triggerCellAction)
            {
                ((ItemImageCell) triggerCell).Focused = true;
                lvItems_CellAction(this, new CellEventArgs(triggerRowColumn, triggerRowNumber, triggerCell));
            }  
        }

        private void ToggleViewMode()
        {
            if (viewMode == ItemSearchViewModeEnum.List)
            {
                viewMode = ItemSearchViewModeEnum.Images;
                panel.SetButtonCaption(Conversion.ToStr((int)Buttons.ToggleView), Resources.List);
            }
            else
            {
                viewMode = ItemSearchViewModeEnum.List;
                panel.SetButtonCaption(Conversion.ToStr((int)Buttons.ToggleView), Resources.Images);
            } 
            
            ApplyColumnCollections();
            LoadDataIntoListView(items);
        }

        private void ApplyColumnCollections()
        {
            // Rows must be cleared otherwise we cannot switch out the column collections
            lvItems.ClearRows();

            // Here we switch out the column sets and reload the rows. This is cheap since we should already have the data we need from the database
            if (viewMode == ItemSearchViewModeEnum.List)
            {
                lvItems.SetColumnCollection(listColumnCollection);
            }
            else
            {
                lvItems.SetColumnCollection(imageColumnCollection);
            }

            lvItems.ApplyRelativeColumnSize();
        }

        private void CreateImageColumnCollection()
        {
            for (int i = 0; i < 8; i++)
            {
                imageColumnCollection.Add(new Column {Clickable = false, RelativeSize = 12});
            }
        }

        private void ScrollPageUp()
        {
            if (lvItems.RowCount > 0)
            {
                lvItems.MovePageUp();
                tbSearch.Focus();
            }
        }

        private void ScrollPageDown()
        {
            if (lvItems.RowCount > 0)
            {
                lvItems.MovePageDown();
                tbSearch.Focus();
            }
        }

        private void MoveSelectionUp()
        {
            if (lvItems.Selection.FirstSelectedRow > 0)
            {
                lvItems.MoveSelectionUp();
            }

            tbSearch.Focus();
        }

        private void MoveSelectionDown()
        {
            lvItems.MoveSelectionDown();
            tbSearch.Focus();
        }

        private void Clear()
        {
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), false);

            if (viewMode == ItemSearchViewModeEnum.Images)
            {
                ClearNonVisibleImageCells();
                ClearVisibleImageCells();
            }

            tbSearch.Text = "";
            searchParameters = "";
            lastEntry = 0;
            items.Clear();
            lvItems.ClearRows();
            LoadData();                
            tbSearch.Focus();
        }        

        private void panel_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((Buttons)args.Tag)
            {
                case Buttons.PageUp: { ScrollPageUp(); break; }
                case Buttons.ArrowUp: { MoveSelectionUp(); break; }
                case Buttons.ArrowDown: { MoveSelectionDown(); break; }
                case Buttons.PageDown: { ScrollPageDown(); break; }
                case Buttons.Clear: { Clear(); break; }
                case Buttons.Select: { SelectItem(); break; }
                case Buttons.ShowPrice: { GetPriceHandler(); break; }
                case Buttons.Close: { Close(); break; }
                case Buttons.ToggleView: { ToggleViewMode(); break; }
            }
        }

        private void GetPriceHandler()
        {
            try
            {
                if (showPrice)
                {
                    showPrice = imageCellManager.ShowPrice = false;
                    panel.SetButtonCaption(Conversion.ToStr((int) Buttons.ShowPrice), Resources.ShowPrice);

                    if (viewMode == ItemSearchViewModeEnum.List)
                    {
                        RemovePriceColumn();
                        lvItems.ApplyRelativeColumnSize();                        
                    }
                    else
                    {
                        lvItems.Invalidate();
                    }
                }
                else
                {
                    showPrice = imageCellManager.ShowPrice = true;
                    lvItems.InvalidateContent();
                    panel.SetButtonCaption(Conversion.ToStr((int) Buttons.ShowPrice), Resources.HidePrice);

                    if (viewMode == ItemSearchViewModeEnum.List)
                    {
                        LoadPricesForList();
                        lvItems.ApplyRelativeColumnSize();                        
                    }
                    else
                    {
                        LoadPricesForImages();
                    }
                }
            }
            catch (Exception ex)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "ItemSearchDialog.GetPriceHandler", ex);
            }
            finally
            {                
                HideProgress();                
            }
        }

        private void LoadPricesForImages()
        {
            if (!showPrice)
            {
                return;
            }

            var info = StoreCurrency;
            var limit = DecimalLimit;

            if (progress == null)
            {
                EventHandler handler = (sender, args) =>
                {
                    foreach (Row row in lvItems.Rows)
                    {
                        for(int i = 0; i < row.CellCount; i++)
                        {
                            ItemImageCell cell = (ItemImageCell) row[(uint)i];
                            if (cell != null)
                            {
                                TradeAgreementPriceInfo priceInfo = GetPrice(cell.RetailItem.ID);

                                if (priceInfo.Price != null)
                                {
                                    cell.PriceText = (info.CurrencyPrefix + " " +
                                     DecimalExtensions.FormatWithLimits((decimal)priceInfo.Price, limit) +
                                     " " + info.CurrencySuffix).Trim();
                                }
                            }
                        }
                    }

                    lvItems.Invalidate();
                };
                progress = LoadingIndicator.ShowOnParent(this, handler);
            }
        }

        private void LoadPricesForList()
        {
            if (!showPrice)
            {
                return;
            }

            if (lvItems.Columns.Count == 3 && !showVariant || lvItems.Columns.Count == 4 && showVariant)
            {
                AddPriceColumn();
            }

            var info = StoreCurrency;
            var limit = DecimalLimit;

            if (progress == null)
            {
                EventHandler handler = (sender, args) =>
                {
                    foreach (var current in lvItems.Rows)
                    {
                        if (current.CellCount == 3 && !showVariant || current.CellCount == 4 && showVariant)
                        {
                            var priceInfo = GetPrice(current[0].Text);
                            if (priceInfo.Price != null)
                            {
                                current.AddText(
                                    (info.CurrencyPrefix +
                                     " " +
                                     ((decimal)priceInfo.Price).FormatWithLimits(limit) +
                                     " " +
                                     info.CurrencySuffix).Trim());
                            }
                        }
                    }

                    lvItems.Invalidate();
                };
                progress = LoadingIndicator.ShowOnParent(this, handler);
            }
        }

        private void lvItems_VerticalScrollValueChanged(object sender, EventArgs e)
        {
            if (viewMode == ItemSearchViewModeEnum.Images)
            {
                if (prevTopRow == -1)
                {
                    prevTopRow = 0;
                }

                ClearNonVisibleImageCells();

                prevTopRow = lvItems.FirstRowOnScreen;
                prevBottomRow = lvItems.FirstRowOnScreen + lvItems.RowCountOnScreen;
            }

            if ((lvItems.FirstRowOnScreen + lvItems.RowCountOnScreen) >= (lvItems.RowCount - 1))
            {
                GetMoreItems();
            }
        }

        private void GetMoreItems()
        {
            if (reachedTheEnd)
            {
                return;
            }
            LoadData();
        }

        private void SelectItem()
        {
            if (viewMode == ItemSearchViewModeEnum.Images && previouslySelectedCell == null)
            {
                tbSearch.Focus();
                return;
            }

            if (lvItems.RowCount > 0)
            {
                IRoundingService rounding = Interfaces.Services.RoundingService(DLLEntry.DataModel);
                string maxLineReturnAmount = rounding.RoundForDisplay(DLLEntry.DataModel, ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.MaxLineReturnAmount, false, false, DLLEntry.Settings.Store.Currency);

                if (viewMode == ItemSearchViewModeEnum.List)
                {
                    if (lvItems.Selection.Count > 0)
                    {
                        Row row = lvItems.Row(lvItems.Selection.FirstSelectedRow);
                        selectedItem = (SimpleRetailItem) row.Tag;

                        if (selectedItem.SalesPriceIncludingTax <= 0)
                        {
                            RetailItem item = Providers.RetailItemData.Get(DLLEntry.DataModel, selectedItem.ID);
                            if (item.GradeID != "")
                            {
                                Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.ActionNotAllowedForFuelItems, MessageBoxButtons.OK, MessageDialogType.Generic);
                                return;
                            }
                        }

                        bool operationValid = currentTransaction.OperationStack.Count == 0 ? false 
                            : currentTransaction.OperationStack[currentTransaction.OperationStack.Count - 1] == POSOperations.ItemSearch;
                        if (((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.MaxLineReturnAmount > 0
                            && ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.MaxLineReturnAmount < selectedItem.SalesPriceIncludingTax
                            && operationValid
                            && operationInfo.ReturnItems)
                        {
                            Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.PriceHigherThanMaximumLineReturnAmount +" \n \n "+ string.Format(Resources.MaximumLineReturnAmountIs, maxLineReturnAmount), 
                                                                                              MessageBoxButtons.OK, 
                                                                                              MessageDialogType.Generic);
                            return;
                        }
                    }
                    else
                    {
                        tbSearch.Focus();
                        return;
                    }
                }
                else
                {                                        
                    selectedItem = ((ItemImageCell) previouslySelectedCell).RetailItem;

                    bool operationValid = currentTransaction.OperationStack.Count == 0 ? false
                        : currentTransaction.OperationStack[currentTransaction.OperationStack.Count - 1] == POSOperations.ItemSearch;
                    if (((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.MaxLineReturnAmount > 0
                        && ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.MaxLineReturnAmount < selectedItem.SalesPriceIncludingTax
                        && operationValid
                        && operationInfo.ReturnItems)
                    {
                        Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.PriceHigherThanMaximumLineReturnAmount + " \n \n " + string.Format(Resources.MaximumLineReturnAmountIs, maxLineReturnAmount), 
                                                                                          MessageBoxButtons.OK, 
                                                                                          MessageDialogType.Generic);
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }

            tbSearch.Focus();
        }
        
        private void touchKeyboard1_EnterPressed(object sender, EventArgs e)
        {
            if (tbSearch.Text.Trim() == "" || tbSearch.Text.Trim() == searchParameters)
            {
                SelectItem();
            }
            else
            {
                searchParameters = tbSearch.Text.Trim();

                panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), searchParameters != "");

                if (viewMode == ItemSearchViewModeEnum.Images)
                {
                    ClearNonVisibleImageCells();
                    ClearVisibleImageCells();
                }

                lastEntry = 0;
                lvItems.ClearRows();
                items.Clear();
                LoadData();
            }
        }

        private void touchKeyboard1_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardCode != "")
            {
                args.CultureName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardCode;
                args.LayoutName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.KeyboardCode;
                args.LayoutName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.KeyboardLayoutName;
            }
        }

        private void ItemSearchDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if (tbSearch.Text.Trim() == "" || tbSearch.Text.Trim() == searchParameters)
                {
                    SelectItem();
                }
                else
                {
                    tbSearch_KeyDown(sender, e);
                }
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                ScrollPageUp();
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                ScrollPageDown();
            }
            else if (e.KeyCode == Keys.Up)
            {
                MoveSelectionUp();
            }
            else if (e.KeyCode == Keys.Left)
            {
                MoveSelectionUp();
            }
            else if (e.KeyCode == Keys.Down)
            {
                MoveSelectionDown();                
            }
            else if (e.KeyCode == Keys.Right)
            {
                MoveSelectionDown();
            }
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;

                // Needed so that the SuppressKeyPress and Handled aren't affected by any handling in the event
                this.BeginInvoke(new EventHandler(touchKeyboard1_EnterPressed), this, EventArgs.Empty);
            }
        }

        private void lvItems_CellAction(object sender, LSOne.Controls.EventArguments.CellEventArgs args)
        {
            if (viewMode == ItemSearchViewModeEnum.Images)
            {
                if (previouslySelectedCell == null)
                {
                    previouslySelectedCell = args.Cell;
                }

                if (previouslySelectedCell != null && args.Cell != previouslySelectedCell)
                {
                    ((ItemImageCell) previouslySelectedCell).Focused = false;
                    previouslySelectedCell = args.Cell;
                    lvItems.InvalidateContent();
                }
            }
        }

        private void lvItems_RowDoubleClick(object sender, RowEventArgs args)
        {
            SelectItem();
        }

        private void lvItems_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (args.ColumnNumber == 4 && showVariant || args.ColumnNumber == 3 && !showVariant) return;//price column

            lvItems.SetSortColumn(args.Column, lvItems.SortColumn == args.Column ? !lvItems.SortedAscending : true);

            Clear();
            LoadData();
            LoadPricesForList();
        }

        private void PopulateItemSearchFilter(RetailItemSearchEnum searchFilterType, RecordIdentifier searchFilterID)
        {
            // Checks if the current searchFilterID is a GUID or not
            bool IsNotMasterID()
            {
                return searchFilterID != null && searchFilterID != RecordIdentifier.Empty && searchFilterID.DBType != SqlDbType.UniqueIdentifier;
            }
            
            // Gets the master id from searchFilterID with the given GetMasterID method
            RecordIdentifier GetMasterID(Func<IConnectionManager, RecordIdentifier, Guid> GetMasterIDDelegate)
            {
                RecordIdentifier resultMasterID = null;

                if (searchFilterID != null && searchFilterID != RecordIdentifier.Empty && searchFilterID.DBType != SqlDbType.UniqueIdentifier)
                {
                    RecordIdentifier masterID = GetMasterIDDelegate(DLLEntry.DataModel, searchFilterID);
                    if (masterID != null)
                    {
                        resultMasterID = masterID;
                    }
                }

                // We don't want RecordIdentifier.Empty since it will cause an error when passed down to the item search methods. They expect null
                // values for filters that cannot be applied.
                if(resultMasterID != null && resultMasterID == RecordIdentifier.Empty)
                {
                    return null;
                }

                return resultMasterID;
            }

            switch (searchFilterType)
            {
                case RetailItemSearchEnum.RetailGroup:
                    itemSearchFilter.RetailGroupID = searchFilterID;

                    if(IsNotMasterID())
                    {   
                        itemSearchFilter.RetailGroupID = GetMasterID(Providers.RetailGroupData.GetMasterID);                        
                    }

                    //This needs to be passed as null, otherwise no records are retrieved
                    if(RecordIdentifier.IsEmptyOrNull(itemSearchFilter.RetailGroupID))
                    {
                        itemSearchFilter.RetailGroupID = null;
                    }

                    break;
                case RetailItemSearchEnum.RetailDepartment:
                    itemSearchFilter.RetailDepartmentID = searchFilterID;

                    if (IsNotMasterID())
                    {
                        itemSearchFilter.RetailDepartmentID = GetMasterID(Providers.RetailDepartmentData.GetMasterID);
                    }

                    break;
                case RetailItemSearchEnum.TaxGroup:
                    itemSearchFilter.TaxGroupID = searchFilterID;
                    break;
                case RetailItemSearchEnum.VariantGroup:
                    itemSearchFilter.VariantGroupID = searchFilterID;
                    break;
                case RetailItemSearchEnum.Vendor:
                    itemSearchFilter.VendorID = searchFilterID;
                    break;
                case RetailItemSearchEnum.BarCode:
                    itemSearchFilter.BarCode = (string)searchFilterID;
                    break;
                case RetailItemSearchEnum.SpecialGroup:
                    itemSearchFilter.SpecialGroupID = searchFilterID;
                    break;
                case RetailItemSearchEnum.ItemType:
                    itemSearchFilter.ItemType = (ItemTypeEnum)((int)searchFilterID);
                    break;
            }
        }
    }
}