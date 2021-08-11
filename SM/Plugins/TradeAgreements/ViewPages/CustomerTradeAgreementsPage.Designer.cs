using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    partial class CustomerTradeAgreementsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerTradeAgreementsPage));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnViewCustomerGroup = new System.Windows.Forms.Button();
            this.btnViewItem = new System.Windows.Forms.Button();
            this.btnViewItemGroup = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbMultilineDiscount = new LSOne.Controls.DualDataComboBox();
            this.cmbLineDiscount = new LSOne.Controls.DualDataComboBox();
            this.cmbTotalDiscountGroup = new LSOne.Controls.DualDataComboBox();
            this.lvValues = new LSOne.Controls.ListView();
            this.column0 = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.column9 = new LSOne.Controls.Columns.Column();
            this.column10 = new LSOne.Controls.Columns.Column();
            this.column11 = new LSOne.Controls.Columns.Column();
            this.column12 = new LSOne.Controls.Columns.Column();
            this.btnEditMultilineDiscount = new LSOne.Controls.ContextButton();
            this.btnEditLineDiscount = new LSOne.Controls.ContextButton();
            this.btnEditTotalDiscountGroup = new LSOne.Controls.ContextButton();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnViewCustomerGroup, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnViewItem, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnViewItemGroup, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // btnViewCustomerGroup
            // 
            resources.ApplyResources(this.btnViewCustomerGroup, "btnViewCustomerGroup");
            this.btnViewCustomerGroup.Name = "btnViewCustomerGroup";
            this.btnViewCustomerGroup.UseVisualStyleBackColor = true;
            this.btnViewCustomerGroup.Click += new System.EventHandler(this.btnViewCustomerGroup_Click);
            // 
            // btnViewItem
            // 
            resources.ApplyResources(this.btnViewItem, "btnViewItem");
            this.btnViewItem.Name = "btnViewItem";
            this.btnViewItem.UseVisualStyleBackColor = true;
            this.btnViewItem.Click += new System.EventHandler(this.btnViewItem_Click);
            // 
            // btnViewItemGroup
            // 
            resources.ApplyResources(this.btnViewItemGroup, "btnViewItemGroup");
            this.btnViewItemGroup.Name = "btnViewItemGroup";
            this.btnViewItemGroup.UseVisualStyleBackColor = true;
            this.btnViewItemGroup.Click += new System.EventHandler(this.btnViewItemGroup_Click);
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cmbMultilineDiscount
            // 
            this.cmbMultilineDiscount.AddList = null;
            this.cmbMultilineDiscount.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbMultilineDiscount, "cmbMultilineDiscount");
            this.cmbMultilineDiscount.MaxLength = 32767;
            this.cmbMultilineDiscount.Name = "cmbMultilineDiscount";
            this.cmbMultilineDiscount.NoChangeAllowed = false;
            this.cmbMultilineDiscount.OnlyDisplayID = false;
            this.cmbMultilineDiscount.RemoveList = null;
            this.cmbMultilineDiscount.RowHeight = ((short)(22));
            this.cmbMultilineDiscount.SecondaryData = null;
            this.cmbMultilineDiscount.SelectedData = null;
            this.cmbMultilineDiscount.SelectedDataID = null;
            this.cmbMultilineDiscount.SelectionList = null;
            this.cmbMultilineDiscount.SkipIDColumn = true;
            this.cmbMultilineDiscount.RequestData += new System.EventHandler(this.cmbMultilineDiscount_RequestData);
            this.cmbMultilineDiscount.SelectedDataChanged += new System.EventHandler(this.cmb_SelectedDataChanged);
            // 
            // cmbLineDiscount
            // 
            this.cmbLineDiscount.AddList = null;
            this.cmbLineDiscount.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbLineDiscount, "cmbLineDiscount");
            this.cmbLineDiscount.MaxLength = 32767;
            this.cmbLineDiscount.Name = "cmbLineDiscount";
            this.cmbLineDiscount.NoChangeAllowed = false;
            this.cmbLineDiscount.OnlyDisplayID = false;
            this.cmbLineDiscount.RemoveList = null;
            this.cmbLineDiscount.RowHeight = ((short)(22));
            this.cmbLineDiscount.SecondaryData = null;
            this.cmbLineDiscount.SelectedData = null;
            this.cmbLineDiscount.SelectedDataID = null;
            this.cmbLineDiscount.SelectionList = null;
            this.cmbLineDiscount.SkipIDColumn = true;
            this.cmbLineDiscount.RequestData += new System.EventHandler(this.cmbLineDiscount_RequestData);
            this.cmbLineDiscount.SelectedDataChanged += new System.EventHandler(this.cmb_SelectedDataChanged);
            // 
            // cmbTotalDiscountGroup
            // 
            this.cmbTotalDiscountGroup.AddList = null;
            this.cmbTotalDiscountGroup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTotalDiscountGroup, "cmbTotalDiscountGroup");
            this.cmbTotalDiscountGroup.MaxLength = 32767;
            this.cmbTotalDiscountGroup.Name = "cmbTotalDiscountGroup";
            this.cmbTotalDiscountGroup.NoChangeAllowed = false;
            this.cmbTotalDiscountGroup.OnlyDisplayID = false;
            this.cmbTotalDiscountGroup.RemoveList = null;
            this.cmbTotalDiscountGroup.RowHeight = ((short)(22));
            this.cmbTotalDiscountGroup.SecondaryData = null;
            this.cmbTotalDiscountGroup.SelectedData = null;
            this.cmbTotalDiscountGroup.SelectedDataID = null;
            this.cmbTotalDiscountGroup.SelectionList = null;
            this.cmbTotalDiscountGroup.SkipIDColumn = true;
            this.cmbTotalDiscountGroup.RequestData += new System.EventHandler(this.cmbTotalDiscountGroup_RequestData);
            this.cmbTotalDiscountGroup.SelectedDataChanged += new System.EventHandler(this.cmb_SelectedDataChanged);
            // 
            // lvValues
            // 
            resources.ApplyResources(this.lvValues, "lvValues");
            this.lvValues.BuddyControl = null;
            this.lvValues.Columns.Add(this.column0);
            this.lvValues.Columns.Add(this.column1);
            this.lvValues.Columns.Add(this.column3);
            this.lvValues.Columns.Add(this.column4);
            this.lvValues.Columns.Add(this.column5);
            this.lvValues.Columns.Add(this.column6);
            this.lvValues.Columns.Add(this.column7);
            this.lvValues.Columns.Add(this.column8);
            this.lvValues.Columns.Add(this.column9);
            this.lvValues.Columns.Add(this.column10);
            this.lvValues.Columns.Add(this.column11);
            this.lvValues.Columns.Add(this.column12);
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
            this.lvValues.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvValues.SortSetting = "0:1";
            this.lvValues.SelectionChanged += new System.EventHandler(this.lvValues_SelectionChanged);
            this.lvValues.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvValues_RowDoubleClick);
            // 
            // column0
            // 
            this.column0.AutoSize = true;
            this.column0.DefaultStyle = null;
            resources.ApplyResources(this.column0, "column0");
            this.column0.InternalSort = true;
            this.column0.MaximumWidth = ((short)(0));
            this.column0.MinimumWidth = ((short)(10));
            this.column0.Tag = null;
            this.column0.Width = ((short)(50));
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
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
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            this.column5.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.column5, "column5");
            this.column5.InternalSort = true;
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.NoTextWhenSmall = true;
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
            this.column8.Tag = null;
            this.column8.Width = ((short)(50));
            // 
            // column9
            // 
            this.column9.AutoSize = true;
            this.column9.DefaultStyle = null;
            resources.ApplyResources(this.column9, "column9");
            this.column9.InternalSort = true;
            this.column9.MaximumWidth = ((short)(0));
            this.column9.MinimumWidth = ((short)(10));
            this.column9.Tag = null;
            this.column9.Width = ((short)(50));
            // 
            // column10
            // 
            this.column10.AutoSize = true;
            this.column10.DefaultStyle = null;
            resources.ApplyResources(this.column10, "column10");
            this.column10.InternalSort = true;
            this.column10.MaximumWidth = ((short)(0));
            this.column10.MinimumWidth = ((short)(10));
            this.column10.Tag = null;
            this.column10.Width = ((short)(50));
            // 
            // column11
            // 
            this.column11.AutoSize = true;
            this.column11.DefaultStyle = null;
            resources.ApplyResources(this.column11, "column11");
            this.column11.InternalSort = true;
            this.column11.MaximumWidth = ((short)(0));
            this.column11.MinimumWidth = ((short)(10));
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
            this.column12.Tag = null;
            this.column12.Width = ((short)(50));
            // 
            // btnEditMultilineDiscount
            // 
            this.btnEditMultilineDiscount.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditMultilineDiscount, "btnEditMultilineDiscount");
            this.btnEditMultilineDiscount.Name = "btnEditMultilineDiscount";
            this.btnEditMultilineDiscount.Click += new System.EventHandler(this.btnEditMultilineDiscount_Click);
            // 
            // btnEditLineDiscount
            // 
            this.btnEditLineDiscount.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditLineDiscount, "btnEditLineDiscount");
            this.btnEditLineDiscount.Name = "btnEditLineDiscount";
            this.btnEditLineDiscount.Click += new System.EventHandler(this.btnEditLineDiscount_Click);
            // 
            // btnEditTotalDiscountGroup
            // 
            this.btnEditTotalDiscountGroup.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditTotalDiscountGroup, "btnEditTotalDiscountGroup");
            this.btnEditTotalDiscountGroup.Name = "btnEditTotalDiscountGroup";
            this.btnEditTotalDiscountGroup.Click += new System.EventHandler(this.btnEditTotalDiscountGroup_Click);
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
            // CustomerTradeAgreementsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvValues);
            this.Controls.Add(this.btnEditMultilineDiscount);
            this.Controls.Add(this.btnEditLineDiscount);
            this.Controls.Add(this.btnEditTotalDiscountGroup);
            this.Controls.Add(this.cmbMultilineDiscount);
            this.Controls.Add(this.cmbLineDiscount);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cmbTotalDiscountGroup);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnsContextButtons);
            this.Name = "CustomerTradeAgreementsPage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnViewCustomerGroup;
        private System.Windows.Forms.Button btnViewItemGroup;
        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.Button btnViewItem;
        private ContextButton btnEditMultilineDiscount;
        private ContextButton btnEditLineDiscount;
        private ContextButton btnEditTotalDiscountGroup;
        private DualDataComboBox cmbMultilineDiscount;
        private DualDataComboBox cmbLineDiscount;
        private System.Windows.Forms.Label label12;
        private DualDataComboBox cmbTotalDiscountGroup;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private ListView lvValues;
        private Controls.Columns.Column column0;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
        private Controls.Columns.Column column6;
        private Controls.Columns.Column column7;
        private Controls.Columns.Column column8;
        private Controls.Columns.Column column9;
        private Controls.Columns.Column column10;
        private Controls.Columns.Column column11;
        private Controls.Columns.Column column12;
    }
}
