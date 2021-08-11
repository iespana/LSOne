using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Views
{
    partial class MultiBuyView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiBuyView));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.btnEditAll = new System.Windows.Forms.Button();
            this.lvValues = new LSOne.Controls.ListView();
            this.column11 = new LSOne.Controls.Columns.Column();
            this.column12 = new LSOne.Controls.Columns.Column();
            this.column13 = new LSOne.Controls.Columns.Column();
            this.clmVariant = new LSOne.Controls.Columns.Column();
            this.column14 = new LSOne.Controls.Columns.Column();
            this.btnsContextButtonsItems = new LSOne.Controls.ContextButtons();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnActivation = new System.Windows.Forms.Button();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvOffers = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.column9 = new LSOne.Controls.Columns.Column();
            this.column10 = new LSOne.Controls.Columns.Column();
            this.column15 = new LSOne.Controls.Columns.Column();
            this.column16 = new LSOne.Controls.Columns.Column();
            this.clmUnit = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvOffers);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.btnActivation);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.btnEditAll);
            this.groupPanel1.Controls.Add(this.lvValues);
            this.groupPanel1.Controls.Add(this.btnsContextButtonsItems);
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
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
            this.lvValues.BorderColor = System.Drawing.Color.DarkGray;
            this.lvValues.BuddyControl = null;
            this.lvValues.Columns.Add(this.column11);
            this.lvValues.Columns.Add(this.column12);
            this.lvValues.Columns.Add(this.column13);
            this.lvValues.Columns.Add(this.clmVariant);
            this.lvValues.Columns.Add(this.clmUnit);
            this.lvValues.Columns.Add(this.column14);
            this.lvValues.ContentBackColor = System.Drawing.Color.White;
            this.lvValues.DefaultRowHeight = ((short)(22));
            this.lvValues.DimSelectionWhenDisabled = true;
            this.lvValues.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvValues.HeaderBackColor = System.Drawing.Color.White;
            this.lvValues.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvValues.HeaderHeight = ((short)(25));
            this.lvValues.HorizontalScrollbar = true;
            this.lvValues.Name = "lvValues";
            this.lvValues.OddRowColor = System.Drawing.Color.White;
            this.lvValues.RowLineColor = System.Drawing.Color.LightGray;
            this.lvValues.SecondarySortColumn = ((short)(-1));
            this.lvValues.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvValues.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvValues.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvValues.SortSetting = "0:1";
            this.lvValues.VerticalScrollbarValue = 0;
            this.lvValues.VerticalScrollbarYOffset = 0;
            this.lvValues.SelectionChanged += new System.EventHandler(this.lvValues_SelectionChanged);
            this.lvValues.CellAction += new LSOne.Controls.CellActionDelegate(this.lvValues_CellAction);
            this.lvValues.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvValues_RowDoubleClick);
            // 
            // column11
            // 
            this.column11.AutoSize = true;
            this.column11.DefaultStyle = null;
            resources.ApplyResources(this.column11, "column11");
            this.column11.InternalSort = true;
            this.column11.MaximumWidth = ((short)(0));
            this.column11.MinimumWidth = ((short)(10));
            this.column11.RelativeSize = 0;
            this.column11.SecondarySortColumn = ((short)(-1));
            this.column11.Tag = null;
            this.column11.Width = ((short)(50));
            // 
            // column12
            // 
            this.column12.AutoSize = true;
            this.column12.DefaultStyle = null;
            resources.ApplyResources(this.column12, "column12");
            this.column12.InternalSort = true;
            this.column12.MaximumWidth = ((short)(0));
            this.column12.MinimumWidth = ((short)(10));
            this.column12.RelativeSize = 0;
            this.column12.SecondarySortColumn = ((short)(-1));
            this.column12.Tag = null;
            this.column12.Width = ((short)(50));
            // 
            // column13
            // 
            this.column13.AutoSize = true;
            this.column13.DefaultStyle = null;
            resources.ApplyResources(this.column13, "column13");
            this.column13.InternalSort = true;
            this.column13.MaximumWidth = ((short)(0));
            this.column13.MinimumWidth = ((short)(10));
            this.column13.RelativeSize = 0;
            this.column13.SecondarySortColumn = ((short)(-1));
            this.column13.Tag = null;
            this.column13.Width = ((short)(50));
            // 
            // clmVariant
            // 
            this.clmVariant.AutoSize = true;
            this.clmVariant.DefaultStyle = null;
            this.clmVariant.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.clmVariant, "clmVariant");
            this.clmVariant.InternalSort = true;
            this.clmVariant.MaximumWidth = ((short)(0));
            this.clmVariant.MinimumWidth = ((short)(10));
            this.clmVariant.NoTextWhenSmall = true;
            this.clmVariant.RelativeSize = 0;
            this.clmVariant.SecondarySortColumn = ((short)(-1));
            this.clmVariant.Tag = null;
            this.clmVariant.Width = ((short)(50));
            // 
            // column14
            // 
            this.column14.AutoSize = true;
            this.column14.Clickable = false;
            this.column14.DefaultStyle = null;
            resources.ApplyResources(this.column14, "column14");
            this.column14.MaximumWidth = ((short)(0));
            this.column14.MinimumWidth = ((short)(10));
            this.column14.SecondarySortColumn = ((short)(-1));
            this.column14.Tag = null;
            this.column14.Width = ((short)(50));
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
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvOffers
            // 
            resources.ApplyResources(this.lvOffers, "lvOffers");
            this.lvOffers.BorderColor = System.Drawing.Color.DarkGray;
            this.lvOffers.BuddyControl = null;
            this.lvOffers.Columns.Add(this.column1);
            this.lvOffers.Columns.Add(this.column2);
            this.lvOffers.Columns.Add(this.column3);
            this.lvOffers.Columns.Add(this.column4);
            this.lvOffers.Columns.Add(this.column5);
            this.lvOffers.Columns.Add(this.column6);
            this.lvOffers.Columns.Add(this.column7);
            this.lvOffers.Columns.Add(this.column8);
            this.lvOffers.Columns.Add(this.column9);
            this.lvOffers.Columns.Add(this.column10);
            this.lvOffers.Columns.Add(this.column15);
            this.lvOffers.Columns.Add(this.column16);
            this.lvOffers.ContentBackColor = System.Drawing.Color.White;
            this.lvOffers.DefaultRowHeight = ((short)(22));
            this.lvOffers.DimSelectionWhenDisabled = true;
            this.lvOffers.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvOffers.HeaderBackColor = System.Drawing.Color.White;
            this.lvOffers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvOffers.HeaderHeight = ((short)(25));
            this.lvOffers.HorizontalScrollbar = true;
            this.lvOffers.Name = "lvOffers";
            this.lvOffers.OddRowColor = System.Drawing.Color.White;
            this.lvOffers.RowLineColor = System.Drawing.Color.LightGray;
            this.lvOffers.SecondarySortColumn = ((short)(-1));
            this.lvOffers.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvOffers.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvOffers.SortSetting = "0:1";
            this.lvOffers.VerticalScrollbarValue = 0;
            this.lvOffers.VerticalScrollbarYOffset = 0;
            this.lvOffers.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvOffers_HeaderClicked);
            this.lvOffers.SelectionChanged += new System.EventHandler(this.lvOffers_SelectionChanged);
            this.lvOffers.CellAction += new LSOne.Controls.CellActionDelegate(this.lvOffers_CellAction);
            this.lvOffers.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvOffers_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.InternalSort = true;
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.InternalSort = true;
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.InternalSort = true;
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.InternalSort = true;
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.InternalSort = true;
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.SecondarySortColumn = ((short)(-1));
            this.column7.Tag = null;
            this.column7.Width = ((short)(50));
            // 
            // column8
            // 
            this.column8.AutoSize = true;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.InternalSort = true;
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.SecondarySortColumn = ((short)(-1));
            this.column8.Tag = null;
            this.column8.Width = ((short)(50));
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
            // column15
            // 
            this.column15.AutoSize = true;
            this.column15.Clickable = false;
            this.column15.DefaultStyle = null;
            resources.ApplyResources(this.column15, "column15");
            this.column15.MaximumWidth = ((short)(0));
            this.column15.MinimumWidth = ((short)(10));
            this.column15.SecondarySortColumn = ((short)(-1));
            this.column15.Sizable = false;
            this.column15.Tag = null;
            this.column15.Width = ((short)(50));
            // 
            // column16
            // 
            this.column16.AutoSize = true;
            this.column16.Clickable = false;
            this.column16.DefaultStyle = null;
            this.column16.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.column16, "column16");
            this.column16.MaximumWidth = ((short)(0));
            this.column16.MinimumWidth = ((short)(10));
            this.column16.NoTextWhenSmall = true;
            this.column16.SecondarySortColumn = ((short)(-1));
            this.column16.Sizable = false;
            this.column16.Tag = null;
            this.column16.Width = ((short)(50));
            // 
            // clmUnit
            // 
            this.clmUnit.AutoSize = true;
            this.clmUnit.DefaultStyle = null;
            this.clmUnit.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.clmUnit, "clmUnit");
            this.clmUnit.InternalSort = true;
            this.clmUnit.MaximumWidth = ((short)(0));
            this.clmUnit.MinimumWidth = ((short)(5));
            this.clmUnit.NoTextWhenSmall = true;
            this.clmUnit.RelativeSize = 0;
            this.clmUnit.SecondarySortColumn = ((short)(-1));
            this.clmUnit.Tag = null;
            this.clmUnit.Width = ((short)(50));
            // 
            // MultiBuyView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MultiBuyView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblGroupHeader;
        private System.Windows.Forms.Label lblNoSelection;
        private System.Windows.Forms.Button btnActivation;
        private ContextButtons btnsContextButtons;
        private ContextButtons btnsContextButtonsItems;
        private ListView lvOffers;
        private Column column1;
        private Column column2;
        private Column column3;
        private Column column4;
        private Column column5;
        private Column column6;
        private Column column7;
        private Column column8;
        private Column column9;
        private Column column10;
        private ListView lvValues;
        private Column column11;
        private Column column12;
        private Column column13;
        private Column column14;
        private System.Windows.Forms.Button btnEditAll;
        private Column column15;
        private Column clmVariant;
        private Column column16;
        private Column clmUnit;
    }
}
