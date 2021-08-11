using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Views
{
    partial class DiscountOffersView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiscountOffersView));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.btnLoadAll = new System.Windows.Forms.Button();
            this.lblLinesCount = new System.Windows.Forms.Label();
            this.btnEditAll = new System.Windows.Forms.Button();
            this.lvValues = new LSOne.Controls.ListView();
            this.column21 = new LSOne.Controls.Columns.Column();
            this.column22 = new LSOne.Controls.Columns.Column();
            this.column23 = new LSOne.Controls.Columns.Column();
            this.colVariantInfo = new LSOne.Controls.Columns.Column();
            this.column24 = new LSOne.Controls.Columns.Column();
            this.column25 = new LSOne.Controls.Columns.Column();
            this.column26 = new LSOne.Controls.Columns.Column();
            this.column27 = new LSOne.Controls.Columns.Column();
            this.column28 = new LSOne.Controls.Columns.Column();
            this.btnsContextButtonsItems = new LSOne.Controls.ContextButtons();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnActivation = new System.Windows.Forms.Button();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lblOfferCount = new System.Windows.Forms.Label();
            this.lvOffers = new LSOne.Controls.ListView();
            this.column11 = new LSOne.Controls.Columns.Column();
            this.column12 = new LSOne.Controls.Columns.Column();
            this.column13 = new LSOne.Controls.Columns.Column();
            this.column14 = new LSOne.Controls.Columns.Column();
            this.column15 = new LSOne.Controls.Columns.Column();
            this.column16 = new LSOne.Controls.Columns.Column();
            this.column17 = new LSOne.Controls.Columns.Column();
            this.column18 = new LSOne.Controls.Columns.Column();
            this.column19 = new LSOne.Controls.Columns.Column();
            this.column20 = new LSOne.Controls.Columns.Column();
            this.column29 = new LSOne.Controls.Columns.Column();
            this.column10 = new LSOne.Controls.Columns.Column();
            this.column9 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column30 = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblOfferCount);
            this.pnlBottom.Controls.Add(this.lvOffers);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.btnActivation);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.btnLoadAll);
            this.groupPanel1.Controls.Add(this.lblLinesCount);
            this.groupPanel1.Controls.Add(this.btnEditAll);
            this.groupPanel1.Controls.Add(this.lvValues);
            this.groupPanel1.Controls.Add(this.btnsContextButtonsItems);
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // btnLoadAll
            // 
            resources.ApplyResources(this.btnLoadAll, "btnLoadAll");
            this.btnLoadAll.Name = "btnLoadAll";
            this.btnLoadAll.UseVisualStyleBackColor = true;
            this.btnLoadAll.Click += new System.EventHandler(this.btnLoadAll_Click);
            // 
            // lblLinesCount
            // 
            resources.ApplyResources(this.lblLinesCount, "lblLinesCount");
            this.lblLinesCount.BackColor = System.Drawing.Color.Transparent;
            this.lblLinesCount.Name = "lblLinesCount";
            // 
            // btnEditAll
            // 
            resources.ApplyResources(this.btnEditAll, "btnEditAll");
            this.btnEditAll.Name = "btnEditAll";
            this.btnEditAll.UseVisualStyleBackColor = true;
            this.btnEditAll.Click += new System.EventHandler(this.MultiEdit);
            // 
            // lvValues
            // 
            resources.ApplyResources(this.lvValues, "lvValues");
            this.lvValues.AutoSelectOnFocus = true;
            this.lvValues.BuddyControl = null;
            this.lvValues.Columns.Add(this.column21);
            this.lvValues.Columns.Add(this.column22);
            this.lvValues.Columns.Add(this.column23);
            this.lvValues.Columns.Add(this.colVariantInfo);
            this.lvValues.Columns.Add(this.column24);
            this.lvValues.Columns.Add(this.column25);
            this.lvValues.Columns.Add(this.column26);
            this.lvValues.Columns.Add(this.column27);
            this.lvValues.Columns.Add(this.column28);
            this.lvValues.ContentBackColor = System.Drawing.Color.White;
            this.lvValues.DefaultRowHeight = ((short)(22));
            this.lvValues.DimSelectionWhenDisabled = true;
            this.lvValues.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvValues.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvValues.HeaderHeight = ((short)(25));
            this.lvValues.HorizontalScrollbar = true;
            this.lvValues.Name = "lvValues";
            this.lvValues.OddRowColor = System.Drawing.Color.White;
            this.lvValues.RowLineColor = System.Drawing.Color.LightGray;
            this.lvValues.SecondarySortColumn = ((short)(-1));
            this.lvValues.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvValues.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvValues.SortSetting = "0:1";
            this.lvValues.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvValues_HeaderClicked);
            this.lvValues.SelectionChanged += new System.EventHandler(this.lvValues_SelectionChanged);
            this.lvValues.CellAction += new LSOne.Controls.CellActionDelegate(this.lvValues_CellAction);
            this.lvValues.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvValues_RowDoubleClick);
            // 
            // column21
            // 
            this.column21.AutoSize = true;
            this.column21.DefaultStyle = null;
            resources.ApplyResources(this.column21, "column21");
            this.column21.MaximumWidth = ((short)(0));
            this.column21.MinimumWidth = ((short)(10));
            this.column21.RelativeSize = 0;
            this.column21.SecondarySortColumn = ((short)(-1));
            this.column21.Tag = null;
            this.column21.Width = ((short)(50));
            // 
            // column22
            // 
            this.column22.AutoSize = true;
            this.column22.DefaultStyle = null;
            resources.ApplyResources(this.column22, "column22");
            this.column22.MaximumWidth = ((short)(0));
            this.column22.MinimumWidth = ((short)(10));
            this.column22.RelativeSize = 0;
            this.column22.SecondarySortColumn = ((short)(-1));
            this.column22.Tag = null;
            this.column22.Width = ((short)(50));
            // 
            // column23
            // 
            this.column23.AutoSize = true;
            this.column23.DefaultStyle = null;
            resources.ApplyResources(this.column23, "column23");
            this.column23.MaximumWidth = ((short)(0));
            this.column23.MinimumWidth = ((short)(10));
            this.column23.RelativeSize = 0;
            this.column23.SecondarySortColumn = ((short)(-1));
            this.column23.Tag = null;
            this.column23.Width = ((short)(50));
            // 
            // colVariantInfo
            // 
            this.colVariantInfo.AutoSize = true;
            this.colVariantInfo.DefaultStyle = null;
            this.colVariantInfo.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.colVariantInfo, "colVariantInfo");
            this.colVariantInfo.MaximumWidth = ((short)(0));
            this.colVariantInfo.MinimumWidth = ((short)(10));
            this.colVariantInfo.NoTextWhenSmall = true;
            this.colVariantInfo.SecondarySortColumn = ((short)(-1));
            this.colVariantInfo.Sizable = false;
            this.colVariantInfo.Tag = null;
            this.colVariantInfo.Width = ((short)(50));
            // 
            // column24
            // 
            this.column24.AutoSize = true;
            this.column24.Clickable = false;
            this.column24.DefaultStyle = null;
            resources.ApplyResources(this.column24, "column24");
            this.column24.MaximumWidth = ((short)(0));
            this.column24.MinimumWidth = ((short)(10));
            this.column24.RelativeSize = 0;
            this.column24.SecondarySortColumn = ((short)(-1));
            this.column24.Tag = null;
            this.column24.Width = ((short)(50));
            // 
            // column25
            // 
            this.column25.AutoSize = true;
            this.column25.Clickable = false;
            this.column25.DefaultStyle = null;
            resources.ApplyResources(this.column25, "column25");
            this.column25.MaximumWidth = ((short)(0));
            this.column25.MinimumWidth = ((short)(10));
            this.column25.RelativeSize = 0;
            this.column25.SecondarySortColumn = ((short)(-1));
            this.column25.Tag = null;
            this.column25.Width = ((short)(50));
            // 
            // column26
            // 
            this.column26.AutoSize = true;
            this.column26.Clickable = false;
            this.column26.DefaultStyle = null;
            resources.ApplyResources(this.column26, "column26");
            this.column26.MaximumWidth = ((short)(0));
            this.column26.MinimumWidth = ((short)(10));
            this.column26.RelativeSize = 0;
            this.column26.SecondarySortColumn = ((short)(-1));
            this.column26.Tag = null;
            this.column26.Width = ((short)(50));
            // 
            // column27
            // 
            this.column27.AutoSize = true;
            this.column27.Clickable = false;
            this.column27.DefaultStyle = null;
            resources.ApplyResources(this.column27, "column27");
            this.column27.MaximumWidth = ((short)(0));
            this.column27.MinimumWidth = ((short)(10));
            this.column27.RelativeSize = 0;
            this.column27.SecondarySortColumn = ((short)(-1));
            this.column27.Tag = null;
            this.column27.Width = ((short)(50));
            // 
            // column28
            // 
            this.column28.AutoSize = true;
            this.column28.Clickable = false;
            this.column28.DefaultStyle = null;
            resources.ApplyResources(this.column28, "column28");
            this.column28.MaximumWidth = ((short)(0));
            this.column28.MinimumWidth = ((short)(22));
            this.column28.RelativeSize = 0;
            this.column28.SecondarySortColumn = ((short)(-1));
            this.column28.Tag = null;
            this.column28.Width = ((short)(22));
            // 
            // btnsContextButtonsItems
            // 
            this.btnsContextButtonsItems.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtonsItems, "btnsContextButtonsItems");
            this.btnsContextButtonsItems.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtonsItems.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtonsItems.EditButtonEnabled = false;
            this.btnsContextButtonsItems.Name = "btnsContextButtonsItems";
            this.btnsContextButtonsItems.RemoveButtonEnabled = false;
            this.btnsContextButtonsItems.EditButtonClicked += new System.EventHandler(this.btnEditValue_Click);
            this.btnsContextButtonsItems.AddButtonClicked += new System.EventHandler(this.btnAddValue_Click);
            this.btnsContextButtonsItems.RemoveButtonClicked += new System.EventHandler(this.btnRemoveValue_Click);
            // 
            // lblGroupHeader
            // 
            resources.ApplyResources(this.lblGroupHeader, "lblGroupHeader");
            this.lblGroupHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupHeader.Name = "lblGroupHeader";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // btnActivation
            // 
            resources.ApplyResources(this.btnActivation, "btnActivation");
            this.btnActivation.Name = "btnActivation";
            this.btnActivation.UseVisualStyleBackColor = true;
            this.btnActivation.Click += new System.EventHandler(this.btnActivation_Click);
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = true;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = true;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // lblOfferCount
            // 
            resources.ApplyResources(this.lblOfferCount, "lblOfferCount");
            this.lblOfferCount.BackColor = System.Drawing.Color.Transparent;
            this.lblOfferCount.Name = "lblOfferCount";
            // 
            // lvOffers
            // 
            resources.ApplyResources(this.lvOffers, "lvOffers");
            this.lvOffers.BuddyControl = null;
            this.lvOffers.Columns.Add(this.column11);
            this.lvOffers.Columns.Add(this.column12);
            this.lvOffers.Columns.Add(this.column13);
            this.lvOffers.Columns.Add(this.column14);
            this.lvOffers.Columns.Add(this.column15);
            this.lvOffers.Columns.Add(this.column16);
            this.lvOffers.Columns.Add(this.column17);
            this.lvOffers.Columns.Add(this.column18);
            this.lvOffers.Columns.Add(this.column19);
            this.lvOffers.Columns.Add(this.column20);
            this.lvOffers.Columns.Add(this.column29);
            this.lvOffers.Columns.Add(this.column30);
            this.lvOffers.ContentBackColor = System.Drawing.Color.White;
            this.lvOffers.DefaultRowHeight = ((short)(22));
            this.lvOffers.DimSelectionWhenDisabled = true;
            this.lvOffers.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvOffers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvOffers.HeaderHeight = ((short)(25));
            this.lvOffers.HorizontalScrollbar = true;
            this.lvOffers.Name = "lvOffers";
            this.lvOffers.OddRowColor = System.Drawing.Color.White;
            this.lvOffers.RowLineColor = System.Drawing.Color.LightGray;
            this.lvOffers.SecondarySortColumn = ((short)(-1));
            this.lvOffers.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvOffers.SortSetting = "0:1";
            this.lvOffers.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvOffers_HeaderClicked);
            this.lvOffers.SelectionChanged += new System.EventHandler(this.lvOffers_SelectionChanged);
            this.lvOffers.CellAction += new LSOne.Controls.CellActionDelegate(this.lvOffers_CellAction);
            this.lvOffers.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvOffers_RowDoubleClick);
            // 
            // column11
            // 
            this.column11.AutoSize = true;
            this.column11.DefaultStyle = null;
            this.column11.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.column11, "column11");
            this.column11.MaximumWidth = ((short)(0));
            this.column11.MinimumWidth = ((short)(10));
            this.column11.SecondarySortColumn = ((short)(-1));
            this.column11.Sizable = false;
            this.column11.Tag = null;
            this.column11.Width = ((short)(50));
            // 
            // column12
            // 
            this.column12.AutoSize = true;
            this.column12.DefaultStyle = null;
            resources.ApplyResources(this.column12, "column12");
            this.column12.MaximumWidth = ((short)(0));
            this.column12.MinimumWidth = ((short)(10));
            this.column12.SecondarySortColumn = ((short)(-1));
            this.column12.Tag = null;
            this.column12.Width = ((short)(50));
            // 
            // column13
            // 
            this.column13.AutoSize = true;
            this.column13.DefaultStyle = null;
            resources.ApplyResources(this.column13, "column13");
            this.column13.MaximumWidth = ((short)(0));
            this.column13.MinimumWidth = ((short)(10));
            this.column13.SecondarySortColumn = ((short)(-1));
            this.column13.Tag = null;
            this.column13.Width = ((short)(50));
            // 
            // column14
            // 
            this.column14.AutoSize = true;
            this.column14.DefaultStyle = null;
            resources.ApplyResources(this.column14, "column14");
            this.column14.MaximumWidth = ((short)(0));
            this.column14.MinimumWidth = ((short)(10));
            this.column14.SecondarySortColumn = ((short)(-1));
            this.column14.Tag = null;
            this.column14.Width = ((short)(50));
            // 
            // column15
            // 
            this.column15.AutoSize = true;
            this.column15.DefaultStyle = null;
            resources.ApplyResources(this.column15, "column15");
            this.column15.MaximumWidth = ((short)(0));
            this.column15.MinimumWidth = ((short)(10));
            this.column15.SecondarySortColumn = ((short)(-1));
            this.column15.Tag = null;
            this.column15.Width = ((short)(50));
            // 
            // column16
            // 
            this.column16.AutoSize = true;
            this.column16.DefaultStyle = null;
            resources.ApplyResources(this.column16, "column16");
            this.column16.MaximumWidth = ((short)(0));
            this.column16.MinimumWidth = ((short)(10));
            this.column16.SecondarySortColumn = ((short)(-1));
            this.column16.Tag = null;
            this.column16.Width = ((short)(50));
            // 
            // column17
            // 
            this.column17.AutoSize = true;
            this.column17.DefaultStyle = null;
            resources.ApplyResources(this.column17, "column17");
            this.column17.MaximumWidth = ((short)(0));
            this.column17.MinimumWidth = ((short)(10));
            this.column17.SecondarySortColumn = ((short)(-1));
            this.column17.Tag = null;
            this.column17.Width = ((short)(50));
            // 
            // column18
            // 
            this.column18.AutoSize = true;
            this.column18.DefaultStyle = null;
            resources.ApplyResources(this.column18, "column18");
            this.column18.MaximumWidth = ((short)(0));
            this.column18.MinimumWidth = ((short)(10));
            this.column18.SecondarySortColumn = ((short)(-1));
            this.column18.Tag = null;
            this.column18.Width = ((short)(50));
            // 
            // column19
            // 
            this.column19.AutoSize = true;
            this.column19.Clickable = false;
            this.column19.DefaultStyle = null;
            resources.ApplyResources(this.column19, "column19");
            this.column19.MaximumWidth = ((short)(0));
            this.column19.MinimumWidth = ((short)(10));
            this.column19.SecondarySortColumn = ((short)(-1));
            this.column19.Tag = null;
            this.column19.Width = ((short)(50));
            // 
            // column20
            // 
            this.column20.AutoSize = true;
            this.column20.Clickable = false;
            this.column20.DefaultStyle = null;
            resources.ApplyResources(this.column20, "column20");
            this.column20.MaximumWidth = ((short)(0));
            this.column20.MinimumWidth = ((short)(10));
            this.column20.SecondarySortColumn = ((short)(-1));
            this.column20.Tag = null;
            this.column20.Width = ((short)(50));
            // 
            // column29
            // 
            this.column29.AutoSize = true;
            this.column29.Clickable = false;
            this.column29.DefaultStyle = null;
            resources.ApplyResources(this.column29, "column29");
            this.column29.MaximumWidth = ((short)(0));
            this.column29.MinimumWidth = ((short)(10));
            this.column29.SecondarySortColumn = ((short)(-1));
            this.column29.Sizable = false;
            this.column29.Tag = null;
            this.column29.Width = ((short)(50));
            // 
            // column10
            // 
            this.column10.AutoSize = true;
            this.column10.Clickable = false;
            this.column10.DefaultStyle = null;
            resources.ApplyResources(this.column10, "column10");
            this.column10.MaximumWidth = ((short)(0));
            this.column10.MinimumWidth = ((short)(10));
            this.column10.SecondarySortColumn = ((short)(-1));
            this.column10.Tag = null;
            this.column10.Width = ((short)(50));
            // 
            // column9
            // 
            this.column9.AutoSize = true;
            this.column9.Clickable = false;
            this.column9.DefaultStyle = null;
            resources.ApplyResources(this.column9, "column9");
            this.column9.MaximumWidth = ((short)(0));
            this.column9.MinimumWidth = ((short)(10));
            this.column9.SecondarySortColumn = ((short)(-1));
            this.column9.Tag = null;
            this.column9.Width = ((short)(50));
            // 
            // column8
            // 
            this.column8.AutoSize = true;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.SecondarySortColumn = ((short)(-1));
            this.column8.Tag = null;
            this.column8.Width = ((short)(50));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.SecondarySortColumn = ((short)(-1));
            this.column7.Tag = null;
            this.column7.Width = ((short)(50));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column30
            // 
            this.column30.AutoSize = true;
            this.column30.Clickable = false;
            this.column30.DefaultStyle = null;
            resources.ApplyResources(this.column30, "column30");
            this.column30.MaximumWidth = ((short)(0));
            this.column30.MinimumWidth = ((short)(10));
            this.column30.NoTextWhenSmall = true;
            this.column30.SecondarySortColumn = ((short)(-1));
            this.column30.Tag = null;
            this.column30.Width = ((short)(50));
            // 
            // DiscountOffersView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "DiscountOffersView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblGroupHeader;
        private System.Windows.Forms.Button btnActivation;
        private ContextButtons btnsContextButtons;
        private ContextButtons btnsContextButtonsItems;
        private System.Windows.Forms.Label lblNoSelection;
        private ListView lvOffers;
        private Column column10;
        private Column column9;
        private Column column8;
        private Column column7;
        private Column column6;
        private Column column5;
        private Column column4;
        private Column column3;
        private Column column2;
        private Column column1;
        private ListView lvValues;
        private Column column21;
        private Column column22;
        private Column column23;
        private Column column24;
        private Column column25;
        private Column column26;
        private Column column27;
        private Column column11;
        private Column column12;
        private Column column13;
        private Column column14;
        private Column column15;
        private Column column16;
        private Column column17;
        private Column column18;
        private Column column19;
        private Column column20;
        private Column column28;
        private System.Windows.Forms.Button btnEditAll;
        private Column column29;
        private Column colVariantInfo;
        private System.Windows.Forms.Label lblOfferCount;
        private System.Windows.Forms.Label lblLinesCount;
        private System.Windows.Forms.Button btnLoadAll;
        private Column column30;
    }
}
