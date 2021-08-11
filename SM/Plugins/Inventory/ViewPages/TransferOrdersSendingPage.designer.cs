using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class TransferOrdersSendingPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferOrdersSendingPage));
            this.btnsItems = new LSOne.Controls.ContextButtons();
            this.lvItems = new LSOne.Controls.ListView();
            this.column15 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column12 = new LSOne.Controls.Columns.Column();
            this.column13 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.lblItemHeader = new System.Windows.Forms.Label();
            this.btnsTransfers = new LSOne.Controls.ContextButtons();
            this.btnSend = new System.Windows.Forms.Button();
            this.lvTransfers = new LSOne.Controls.ListView();
            this.column14 = new LSOne.Controls.Columns.Column();
            this.column10 = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column9 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column11 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.groupPanelNoSelection = new LSOne.Controls.GroupPanel();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnViewTransferRequest = new System.Windows.Forms.Button();
            this.groupPanelNoSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnsItems
            // 
            this.btnsItems.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsItems, "btnsItems");
            this.btnsItems.BackColor = System.Drawing.Color.Transparent;
            this.btnsItems.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsItems.EditButtonEnabled = false;
            this.btnsItems.Name = "btnsItems";
            this.btnsItems.RemoveButtonEnabled = true;
            this.btnsItems.EditButtonClicked += new System.EventHandler(this.btnsItems_EditButtonClicked);
            this.btnsItems.AddButtonClicked += new System.EventHandler(this.btnsItems_AddButtonClicked);
            this.btnsItems.RemoveButtonClicked += new System.EventHandler(this.btnsItems_RemoveButtonClicked);
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.column15);
            this.lvItems.Columns.Add(this.column5);
            this.lvItems.Columns.Add(this.column6);
            this.lvItems.Columns.Add(this.column7);
            this.lvItems.Columns.Add(this.column12);
            this.lvItems.Columns.Add(this.column13);
            this.lvItems.Columns.Add(this.column8);
            this.lvItems.ContentBackColor = System.Drawing.Color.White;
            this.lvItems.DefaultRowHeight = ((short)(22));
            this.lvItems.DimSelectionWhenDisabled = true;
            this.lvItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItems.HeaderHeight = ((short)(25));
            this.lvItems.Name = "lvItems";
            this.lvItems.OddRowColor = System.Drawing.Color.White;
            this.lvItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItems.SecondarySortColumn = ((short)(-1));
            this.lvItems.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItems.SortSetting = "1:1";
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectionChanged);
            this.lvItems.CellAction += new LSOne.Controls.CellActionDelegate(this.lvItems_CellAction);
            this.lvItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItems_RowDoubleClick);
            // 
            // column15
            // 
            this.column15.AutoSize = true;
            this.column15.DefaultStyle = null;
            resources.ApplyResources(this.column15, "column15");
            this.column15.InternalSort = true;
            this.column15.MaximumWidth = ((short)(0));
            this.column15.MinimumWidth = ((short)(10));
            this.column15.Tag = null;
            this.column15.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.InternalSort = true;
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.Tag = null;
            this.column5.Width = ((short)(100));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.Clickable = false;
            this.column6.DefaultStyle = null;
            this.column6.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.column6, "column6");
            this.column6.InternalSort = true;
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.NoTextWhenSmall = true;
            this.column6.Tag = null;
            this.column6.Width = ((short)(100));
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
            this.column7.Width = ((short)(100));
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
            // column13
            // 
            this.column13.AutoSize = true;
            this.column13.DefaultStyle = null;
            resources.ApplyResources(this.column13, "column13");
            this.column13.InternalSort = true;
            this.column13.MaximumWidth = ((short)(0));
            this.column13.MinimumWidth = ((short)(10));
            this.column13.Tag = null;
            this.column13.Width = ((short)(50));
            // 
            // column8
            // 
            this.column8.Clickable = false;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.Tag = null;
            this.column8.Width = ((short)(50));
            // 
            // lblItemHeader
            // 
            resources.ApplyResources(this.lblItemHeader, "lblItemHeader");
            this.lblItemHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblItemHeader.Name = "lblItemHeader";
            // 
            // btnsTransfers
            // 
            this.btnsTransfers.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsTransfers, "btnsTransfers");
            this.btnsTransfers.BackColor = System.Drawing.Color.Transparent;
            this.btnsTransfers.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsTransfers.EditButtonEnabled = false;
            this.btnsTransfers.Name = "btnsTransfers";
            this.btnsTransfers.RemoveButtonEnabled = true;
            this.btnsTransfers.EditButtonClicked += new System.EventHandler(this.btnsTransfers_EditButtonClicked);
            this.btnsTransfers.AddButtonClicked += new System.EventHandler(this.btnsTransfers_AddButtonClicked);
            this.btnsTransfers.RemoveButtonClicked += new System.EventHandler(this.btnsTransfers_RemoveButtonClicked);
            // 
            // btnSend
            // 
            resources.ApplyResources(this.btnSend, "btnSend");
            this.btnSend.Name = "btnSend";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lvTransfers
            // 
            resources.ApplyResources(this.lvTransfers, "lvTransfers");
            this.lvTransfers.BuddyControl = null;
            this.lvTransfers.Columns.Add(this.column14);
            this.lvTransfers.Columns.Add(this.column10);
            this.lvTransfers.Columns.Add(this.column1);
            this.lvTransfers.Columns.Add(this.column2);
            this.lvTransfers.Columns.Add(this.column9);
            this.lvTransfers.Columns.Add(this.column3);
            this.lvTransfers.Columns.Add(this.column11);
            this.lvTransfers.Columns.Add(this.column4);
            this.lvTransfers.ContentBackColor = System.Drawing.Color.White;
            this.lvTransfers.DefaultRowHeight = ((short)(22));
            this.lvTransfers.DimSelectionWhenDisabled = true;
            this.lvTransfers.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvTransfers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvTransfers.HeaderHeight = ((short)(25));
            this.lvTransfers.Name = "lvTransfers";
            this.lvTransfers.OddRowColor = System.Drawing.Color.White;
            this.lvTransfers.RowLineColor = System.Drawing.Color.LightGray;
            this.lvTransfers.SecondarySortColumn = ((short)(-1));
            this.lvTransfers.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvTransfers.SortSetting = "0:1";
            this.lvTransfers.SelectionChanged += new System.EventHandler(this.lvTransfers_SelectionChanged);
            this.lvTransfers.CellAction += new LSOne.Controls.CellActionDelegate(this.lvTransfers_CellAction);
            this.lvTransfers.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvTransfers_RowDoubleClick);
            // 
            // column14
            // 
            this.column14.AutoSize = true;
            this.column14.Clickable = false;
            this.column14.DefaultStyle = null;
            resources.ApplyResources(this.column14, "column14");
            this.column14.MaximumWidth = ((short)(0));
            this.column14.MinimumWidth = ((short)(5));
            this.column14.Tag = null;
            this.column14.Width = ((short)(30));
            // 
            // column10
            // 
            this.column10.AutoSize = true;
            this.column10.DefaultStyle = null;
            resources.ApplyResources(this.column10, "column10");
            this.column10.MaximumWidth = ((short)(0));
            this.column10.MinimumWidth = ((short)(10));
            this.column10.Tag = null;
            this.column10.Width = ((short)(100));
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.Tag = null;
            this.column1.Width = ((short)(100));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.Tag = null;
            this.column2.Width = ((short)(100));
            // 
            // column9
            // 
            this.column9.AutoSize = true;
            this.column9.DefaultStyle = null;
            resources.ApplyResources(this.column9, "column9");
            this.column9.MaximumWidth = ((short)(0));
            this.column9.MinimumWidth = ((short)(10));
            this.column9.Tag = null;
            this.column9.Width = ((short)(100));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.Tag = null;
            this.column3.Width = ((short)(100));
            // 
            // column11
            // 
            this.column11.AutoSize = true;
            this.column11.DefaultStyle = null;
            resources.ApplyResources(this.column11, "column11");
            this.column11.MaximumWidth = ((short)(0));
            this.column11.MinimumWidth = ((short)(10));
            this.column11.Tag = null;
            this.column11.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.Clickable = false;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // groupPanelNoSelection
            // 
            resources.ApplyResources(this.groupPanelNoSelection, "groupPanelNoSelection");
            this.groupPanelNoSelection.Controls.Add(this.lvItems);
            this.groupPanelNoSelection.Controls.Add(this.lblItemHeader);
            this.groupPanelNoSelection.Controls.Add(this.btnsItems);
            this.groupPanelNoSelection.Controls.Add(this.lblNoSelection);
            this.groupPanelNoSelection.Name = "groupPanelNoSelection";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.btnSend);
            this.flowLayoutPanel1.Controls.Add(this.btnViewTransferRequest);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // btnViewTransferRequest
            // 
            resources.ApplyResources(this.btnViewTransferRequest, "btnViewTransferRequest");
            this.btnViewTransferRequest.Name = "btnViewTransferRequest";
            this.btnViewTransferRequest.UseVisualStyleBackColor = true;
            this.btnViewTransferRequest.Click += new System.EventHandler(this.btnViewTransferRequest_Click);
            // 
            // TransferOrdersSendingPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.btnsTransfers);
            this.Controls.Add(this.lvTransfers);
            this.Controls.Add(this.groupPanelNoSelection);
            this.DoubleBuffered = true;
            this.Name = "TransferOrdersSendingPage";
            this.groupPanelNoSelection.ResumeLayout(false);
            this.groupPanelNoSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsItems;
        private ListView lvItems;
        private System.Windows.Forms.Label lblItemHeader;
        private ContextButtons btnsTransfers;
        private System.Windows.Forms.Button btnSend;
        private ListView lvTransfers;
        private GroupPanel groupPanelNoSelection;
        private System.Windows.Forms.Label lblNoSelection;
        private Column column1;
        private Column column2;
        private Column column3;
        private Column column4;
        private Column column5;
        private Column column6;
        private Column column7;
        private Column column9;
        private Column column10;
        private Column column11;
        private Column column12;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Column column13;
        private Column column14;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnViewTransferRequest;
        private Column column8;
        private Column column15;
    }
}
