using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class ItemInventoryPage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemInventoryPage));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxInventoryOnHand = new System.Windows.Forms.GroupBox();
            this.lblRegion = new System.Windows.Forms.Label();
            this.cmbRegion = new LSOne.Controls.DualDataComboBox();
            this.lvItemInventory = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.colReserved = new LSOne.Controls.Columns.Column();
            this.colParked = new LSOne.Controls.Columns.Column();
            this.lblNoConnection = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnEditPurchaseUnits = new LSOne.Controls.ContextButton();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPurchaseUnit = new LSOne.Controls.DualDataComboBox();
            this.btnEditInventoryUnits = new LSOne.Controls.ContextButton();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbInventoryUnit = new LSOne.Controls.DualDataComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.clmInfo = new LSOne.Controls.Columns.Column();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxInventoryOnHand.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.groupBoxInventoryOnHand, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // groupBoxInventoryOnHand
            // 
            resources.ApplyResources(this.groupBoxInventoryOnHand, "groupBoxInventoryOnHand");
            this.tableLayoutPanel1.SetColumnSpan(this.groupBoxInventoryOnHand, 2);
            this.groupBoxInventoryOnHand.Controls.Add(this.lblRegion);
            this.groupBoxInventoryOnHand.Controls.Add(this.cmbRegion);
            this.groupBoxInventoryOnHand.Controls.Add(this.lvItemInventory);
            this.groupBoxInventoryOnHand.Controls.Add(this.lblNoConnection);
            this.groupBoxInventoryOnHand.Name = "groupBoxInventoryOnHand";
            this.groupBoxInventoryOnHand.TabStop = false;
            // 
            // lblRegion
            // 
            this.lblRegion.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRegion, "lblRegion");
            this.lblRegion.Name = "lblRegion";
            // 
            // cmbRegion
            // 
            this.cmbRegion.AddList = null;
            this.cmbRegion.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRegion, "cmbRegion");
            this.cmbRegion.MaxLength = 32767;
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.NoChangeAllowed = false;
            this.cmbRegion.OnlyDisplayID = false;
            this.cmbRegion.RemoveList = null;
            this.cmbRegion.RowHeight = ((short)(22));
            this.cmbRegion.SecondaryData = null;
            this.cmbRegion.SelectedData = null;
            this.cmbRegion.SelectedDataID = null;
            this.cmbRegion.SelectionList = null;
            this.cmbRegion.SkipIDColumn = true;
            this.cmbRegion.RequestData += new System.EventHandler(this.cmbRegion_RequestData);
            this.cmbRegion.SelectedDataChanged += new System.EventHandler(this.cmbRegion_SelectedDataChanged);
            // 
            // lvItemInventory
            // 
            resources.ApplyResources(this.lvItemInventory, "lvItemInventory");
            this.lvItemInventory.BorderColor = System.Drawing.Color.DarkGray;
            this.lvItemInventory.BuddyControl = null;
            this.lvItemInventory.Columns.Add(this.column1);
            this.lvItemInventory.Columns.Add(this.column2);
            this.lvItemInventory.Columns.Add(this.column3);
            this.lvItemInventory.Columns.Add(this.colReserved);
            this.lvItemInventory.Columns.Add(this.colParked);
            this.lvItemInventory.Columns.Add(this.clmInfo);
            this.lvItemInventory.ContentBackColor = System.Drawing.Color.White;
            this.lvItemInventory.DefaultRowHeight = ((short)(22));
            this.lvItemInventory.DimSelectionWhenDisabled = true;
            this.lvItemInventory.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItemInventory.HeaderBackColor = System.Drawing.Color.White;
            this.lvItemInventory.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItemInventory.HeaderHeight = ((short)(25));
            this.lvItemInventory.Name = "lvItemInventory";
            this.lvItemInventory.OddRowColor = System.Drawing.Color.White;
            this.lvItemInventory.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItemInventory.SecondarySortColumn = ((short)(-1));
            this.lvItemInventory.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvItemInventory.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItemInventory.SortSetting = "0:1";
            this.lvItemInventory.VerticalScrollbarValue = 0;
            this.lvItemInventory.VerticalScrollbarYOffset = 0;
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
            this.column3.Clickable = false;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // colReserved
            // 
            this.colReserved.AutoSize = true;
            this.colReserved.Clickable = false;
            this.colReserved.DefaultStyle = null;
            resources.ApplyResources(this.colReserved, "colReserved");
            this.colReserved.InternalSort = true;
            this.colReserved.MaximumWidth = ((short)(0));
            this.colReserved.MinimumWidth = ((short)(10));
            this.colReserved.SecondarySortColumn = ((short)(-1));
            this.colReserved.Tag = null;
            this.colReserved.Width = ((short)(50));
            // 
            // colParked
            // 
            this.colParked.AutoSize = true;
            this.colParked.Clickable = false;
            this.colParked.DefaultStyle = null;
            resources.ApplyResources(this.colParked, "colParked");
            this.colParked.InternalSort = true;
            this.colParked.MaximumWidth = ((short)(0));
            this.colParked.MinimumWidth = ((short)(10));
            this.colParked.SecondarySortColumn = ((short)(-1));
            this.colParked.Tag = null;
            this.colParked.Width = ((short)(50));
            // 
            // lblNoConnection
            // 
            resources.ApplyResources(this.lblNoConnection, "lblNoConnection");
            this.lblNoConnection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoConnection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoConnection.Name = "lblNoConnection";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox3, 2);
            this.groupBox3.Controls.Add(this.btnEditPurchaseUnits);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cmbPurchaseUnit);
            this.groupBox3.Controls.Add(this.btnEditInventoryUnits);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.cmbInventoryUnit);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // btnEditPurchaseUnits
            // 
            this.btnEditPurchaseUnits.BackColor = System.Drawing.Color.Transparent;
            this.btnEditPurchaseUnits.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditPurchaseUnits, "btnEditPurchaseUnits");
            this.btnEditPurchaseUnits.Name = "btnEditPurchaseUnits";
            this.btnEditPurchaseUnits.Click += new System.EventHandler(this.btnEditUnits_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbPurchaseUnit
            // 
            this.cmbPurchaseUnit.AddList = null;
            this.cmbPurchaseUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPurchaseUnit, "cmbPurchaseUnit");
            this.cmbPurchaseUnit.MaxLength = 32767;
            this.cmbPurchaseUnit.Name = "cmbPurchaseUnit";
            this.cmbPurchaseUnit.NoChangeAllowed = false;
            this.cmbPurchaseUnit.OnlyDisplayID = false;
            this.cmbPurchaseUnit.RemoveList = null;
            this.cmbPurchaseUnit.RowHeight = ((short)(22));
            this.cmbPurchaseUnit.SecondaryData = null;
            this.cmbPurchaseUnit.SelectedData = null;
            this.cmbPurchaseUnit.SelectedDataID = null;
            this.cmbPurchaseUnit.SelectionList = null;
            this.cmbPurchaseUnit.SkipIDColumn = true;
            // 
            // btnEditInventoryUnits
            // 
            this.btnEditInventoryUnits.BackColor = System.Drawing.Color.Transparent;
            this.btnEditInventoryUnits.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditInventoryUnits, "btnEditInventoryUnits");
            this.btnEditInventoryUnits.Name = "btnEditInventoryUnits";
            this.btnEditInventoryUnits.Click += new System.EventHandler(this.btnEditUnits_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbInventoryUnit
            // 
            this.cmbInventoryUnit.AddList = null;
            this.cmbInventoryUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbInventoryUnit, "cmbInventoryUnit");
            this.cmbInventoryUnit.MaxLength = 32767;
            this.cmbInventoryUnit.Name = "cmbInventoryUnit";
            this.cmbInventoryUnit.NoChangeAllowed = false;
            this.cmbInventoryUnit.OnlyDisplayID = false;
            this.cmbInventoryUnit.RemoveList = null;
            this.cmbInventoryUnit.RowHeight = ((short)(22));
            this.cmbInventoryUnit.SecondaryData = null;
            this.cmbInventoryUnit.SelectedData = null;
            this.cmbInventoryUnit.SelectedDataID = null;
            this.cmbInventoryUnit.SelectionList = null;
            this.cmbInventoryUnit.SkipIDColumn = true;
            this.cmbInventoryUnit.RequestData += new System.EventHandler(this.cmbInventoryUnit_RequestData);
            this.cmbInventoryUnit.SelectedDataChanged += new System.EventHandler(this.cmbInventoryUnit_SelectedDataChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // clmInfo
            // 
            this.clmInfo.AutoSize = true;
            this.clmInfo.Clickable = false;
            this.clmInfo.DefaultStyle = null;
            resources.ApplyResources(this.clmInfo, "clmInfo");
            this.clmInfo.MaximumWidth = ((short)(0));
            this.clmInfo.MinimumWidth = ((short)(10));
            this.clmInfo.SecondarySortColumn = ((short)(-1));
            this.clmInfo.Tag = null;
            this.clmInfo.Width = ((short)(50));
            // 
            // ItemInventoryPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "ItemInventoryPage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBoxInventoryOnHand.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox groupBoxInventoryOnHand;
        private System.Windows.Forms.GroupBox groupBox3;
        private ContextButton btnEditInventoryUnits;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbInventoryUnit;
        private ContextButton btnEditPurchaseUnits;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbPurchaseUnit;
        private ListView lvItemInventory;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column colReserved;
        private System.Windows.Forms.Label lblNoConnection;
        private Controls.Columns.Column colParked;
        private System.Windows.Forms.Label lblRegion;
        private DualDataComboBox cmbRegion;
        private Controls.Columns.Column clmInfo;
    }
}
