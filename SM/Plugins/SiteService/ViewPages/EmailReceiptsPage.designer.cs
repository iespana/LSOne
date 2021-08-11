using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class EmailReceiptPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailReceiptPage));
            this.chkEmailReceipt = new System.Windows.Forms.CheckBox();
            this.lblEmailReceipts = new System.Windows.Forms.Label();
            this.lblOption = new System.Windows.Forms.Label();
            this.cmbEmailOption = new System.Windows.Forms.ComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lvShorthand = new LSOne.Controls.ListView();
            this.colShorthand = new LSOne.Controls.Columns.Column();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddEditShortHand = new LSOne.Controls.ContextButtons();
            this.cmbPrinterConfigurationID = new LSOne.Controls.DualDataComboBox();
            this.lblEmailPrinterID = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkEmailReceipt
            // 
            resources.ApplyResources(this.chkEmailReceipt, "chkEmailReceipt");
            this.chkEmailReceipt.Name = "chkEmailReceipt";
            this.chkEmailReceipt.UseVisualStyleBackColor = true;
            this.chkEmailReceipt.CheckedChanged += new System.EventHandler(this.chkEmailReceipt_CheckedChanged);
            // 
            // lblEmailReceipts
            // 
            resources.ApplyResources(this.lblEmailReceipts, "lblEmailReceipts");
            this.lblEmailReceipts.Name = "lblEmailReceipts";
            // 
            // lblOption
            // 
            resources.ApplyResources(this.lblOption, "lblOption");
            this.lblOption.Name = "lblOption";
            // 
            // cmbEmailOption
            // 
            this.cmbEmailOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmailOption.FormattingEnabled = true;
            resources.ApplyResources(this.cmbEmailOption, "cmbEmailOption");
            this.cmbEmailOption.Name = "cmbEmailOption";
            this.cmbEmailOption.SelectedIndexChanged += new System.EventHandler(this.cmbEmailOption_SelectedIndexChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lvShorthand
            // 
            this.lvShorthand.BuddyControl = null;
            this.lvShorthand.Columns.Add(this.colShorthand);
            this.lvShorthand.ContentBackColor = System.Drawing.Color.White;
            this.lvShorthand.DefaultRowHeight = ((short)(22));
            this.lvShorthand.DimSelectionWhenDisabled = true;
            this.lvShorthand.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvShorthand.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvShorthand.HeaderHeight = ((short)(25));
            resources.ApplyResources(this.lvShorthand, "lvShorthand");
            this.lvShorthand.Name = "lvShorthand";
            this.lvShorthand.OddRowColor = System.Drawing.Color.White;
            this.lvShorthand.RowLineColor = System.Drawing.Color.LightGray;
            this.lvShorthand.SecondarySortColumn = ((short)(-1));
            this.lvShorthand.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvShorthand.SortSetting = "0:1";
            this.lvShorthand.SelectionChanged += new System.EventHandler(this.lvShorthand_SelectionChanged);
            // 
            // colShorthand
            // 
            this.colShorthand.AutoSize = true;
            this.colShorthand.Clickable = false;
            this.colShorthand.DefaultStyle = null;
            resources.ApplyResources(this.colShorthand, "colShorthand");
            this.colShorthand.MaximumWidth = ((short)(0));
            this.colShorthand.MinimumWidth = ((short)(200));
            this.colShorthand.SecondarySortColumn = ((short)(-1));
            this.colShorthand.Tag = null;
            this.colShorthand.Width = ((short)(500));
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnAddEditShortHand);
            this.groupBox1.Controls.Add(this.lvShorthand);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnAddEditShortHand
            // 
            this.btnAddEditShortHand.AddButtonEnabled = true;
            this.btnAddEditShortHand.BackColor = System.Drawing.Color.Transparent;
            this.btnAddEditShortHand.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnAddEditShortHand.EditButtonEnabled = true;
            resources.ApplyResources(this.btnAddEditShortHand, "btnAddEditShortHand");
            this.btnAddEditShortHand.Name = "btnAddEditShortHand";
            this.btnAddEditShortHand.RemoveButtonEnabled = true;
            this.btnAddEditShortHand.EditButtonClicked += new System.EventHandler(this.btnAddEditShortHand_EditButtonClicked);
            this.btnAddEditShortHand.AddButtonClicked += new System.EventHandler(this.btnAddEditShortHand_AddButtonClicked);
            this.btnAddEditShortHand.RemoveButtonClicked += new System.EventHandler(this.btnAddEditShortHand_RemoveButtonClicked);
            // 
            // cmbPrinterConfigurationID
            // 
            this.cmbPrinterConfigurationID.AddList = null;
            this.cmbPrinterConfigurationID.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPrinterConfigurationID, "cmbPrinterConfigurationID");
            this.cmbPrinterConfigurationID.MaxLength = 32767;
            this.cmbPrinterConfigurationID.Name = "cmbPrinterConfigurationID";
            this.cmbPrinterConfigurationID.NoChangeAllowed = false;
            this.cmbPrinterConfigurationID.OnlyDisplayID = false;
            this.cmbPrinterConfigurationID.RemoveList = null;
            this.cmbPrinterConfigurationID.RowHeight = ((short)(22));
            this.cmbPrinterConfigurationID.SecondaryData = null;
            this.cmbPrinterConfigurationID.SelectedData = null;
            this.cmbPrinterConfigurationID.SelectedDataID = null;
            this.cmbPrinterConfigurationID.SelectionList = null;
            this.cmbPrinterConfigurationID.SkipIDColumn = true;
            this.cmbPrinterConfigurationID.RequestData += new System.EventHandler(this.cmbPrinterConfigurationID_RequestData);
            // 
            // lblEmailPrinterID
            // 
            resources.ApplyResources(this.lblEmailPrinterID, "lblEmailPrinterID");
            this.lblEmailPrinterID.Name = "lblEmailPrinterID";
            // 
            // EmailReceiptPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblEmailPrinterID);
            this.Controls.Add(this.cmbPrinterConfigurationID);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkEmailReceipt);
            this.Controls.Add(this.lblEmailReceipts);
            this.Controls.Add(this.lblOption);
            this.Controls.Add(this.cmbEmailOption);
            this.DoubleBuffered = true;
            this.Name = "EmailReceiptPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox chkEmailReceipt;
        private System.Windows.Forms.Label lblEmailReceipts;
        private System.Windows.Forms.Label lblOption;
        private System.Windows.Forms.ComboBox cmbEmailOption;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox groupBox1;
        private ContextButtons btnAddEditShortHand;
        private ListView lvShorthand;
        private Controls.Columns.Column colShorthand;
        private System.Windows.Forms.Label lblEmailPrinterID;
        private DualDataComboBox cmbPrinterConfigurationID;
    }
}
