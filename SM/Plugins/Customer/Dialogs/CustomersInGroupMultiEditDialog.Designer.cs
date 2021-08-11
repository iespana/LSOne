namespace LSOne.ViewPlugins.Customer.Dialogs
{
    partial class CustomersInGroupMultiEditDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomersInGroupMultiEditDialog));
            this.lblEditPreview = new System.Windows.Forms.Label();
            this.lvlEditPreview = new LSOne.Controls.ListView();
            this.cmbCustomers = new LSOne.Controls.DualDataComboBox();
            this.lblRelation = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.colCustomer = new LSOne.Controls.Columns.Column();
            this.colChange = new LSOne.Controls.Columns.Column();
            this.colIcon = new LSOne.Controls.Columns.Column();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEditPreview
            // 
            resources.ApplyResources(this.lblEditPreview, "lblEditPreview");
            this.lblEditPreview.BackColor = System.Drawing.Color.Transparent;
            this.lblEditPreview.Name = "lblEditPreview";
            // 
            // lvlEditPreview
            // 
            this.lvlEditPreview.BuddyControl = null;
            this.lvlEditPreview.Columns.Add(this.colCustomer);
            this.lvlEditPreview.Columns.Add(this.colChange);
            this.lvlEditPreview.Columns.Add(this.colIcon);
            this.lvlEditPreview.ContentBackColor = System.Drawing.Color.White;
            this.lvlEditPreview.DefaultRowHeight = ((short)(22));
            this.lvlEditPreview.DimSelectionWhenDisabled = true;
            this.lvlEditPreview.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvlEditPreview.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvlEditPreview.HeaderHeight = ((short)(25));
            resources.ApplyResources(this.lvlEditPreview, "lvlEditPreview");
            this.lvlEditPreview.Name = "lvlEditPreview";
            this.lvlEditPreview.OddRowColor = System.Drawing.Color.White;
            this.lvlEditPreview.RowLineColor = System.Drawing.Color.LightGray;
            this.lvlEditPreview.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvlEditPreview.SortSetting = "-1:1";
            this.lvlEditPreview.CellAction += new LSOne.Controls.CellActionDelegate(this.lvlEditPreview_CellAction);
            // 
            // cmbCustomers
            // 
            this.cmbCustomers.AddList = null;
            this.cmbCustomers.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCustomers, "cmbCustomers");
            this.cmbCustomers.MaxLength = 32767;
            this.cmbCustomers.Name = "cmbCustomers";
            this.cmbCustomers.OnlyDisplayID = false;
            this.cmbCustomers.RemoveList = null;
            this.cmbCustomers.RowHeight = ((short)(22));
            this.cmbCustomers.SelectedData = null;
            this.cmbCustomers.SelectedDataID = null;
            this.cmbCustomers.SelectionList = null;
            this.cmbCustomers.SkipIDColumn = false;
            this.cmbCustomers.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbCustomers_DropDown);
            this.cmbCustomers.SelectedDataChanged += new System.EventHandler(this.cmbCustomers_SelectedDataChanged);
            // 
            // lblRelation
            // 
            resources.ApplyResources(this.lblRelation, "lblRelation");
            this.lblRelation.Name = "lblRelation";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // colCustomer
            // 
            this.colCustomer.AutoSize = true;
            this.colCustomer.Clickable = false;
            this.colCustomer.DefaultStyle = null;
            resources.ApplyResources(this.colCustomer, "colCustomer");
            this.colCustomer.MaximumWidth = ((short)(0));
            this.colCustomer.MinimumWidth = ((short)(200));
            this.colCustomer.RelativeSize = 0;
            this.colCustomer.Tag = null;
            this.colCustomer.Width = ((short)(200));
            // 
            // colChange
            // 
            this.colChange.AutoSize = true;
            this.colChange.Clickable = false;
            this.colChange.DefaultStyle = null;
            resources.ApplyResources(this.colChange, "colChange");
            this.colChange.MaximumWidth = ((short)(0));
            this.colChange.MinimumWidth = ((short)(10));
            this.colChange.RelativeSize = 0;
            this.colChange.Tag = null;
            this.colChange.Width = ((short)(50));
            // 
            // colIcon
            // 
            this.colIcon.Clickable = false;
            this.colIcon.DefaultStyle = null;
            resources.ApplyResources(this.colIcon, "colIcon");
            this.colIcon.MaximumWidth = ((short)(10));
            this.colIcon.MinimumWidth = ((short)(10));
            this.colIcon.RelativeSize = 10;
            this.colIcon.Tag = null;
            this.colIcon.Width = ((short)(50));
            // 
            // CustomersInGroupMultiEditDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblEditPreview);
            this.Controls.Add(this.lvlEditPreview);
            this.Controls.Add(this.cmbCustomers);
            this.Controls.Add(this.lblRelation);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "CustomersInGroupMultiEditDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblRelation, 0);
            this.Controls.SetChildIndex(this.cmbCustomers, 0);
            this.Controls.SetChildIndex(this.lvlEditPreview, 0);
            this.Controls.SetChildIndex(this.lblEditPreview, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEditPreview;
        private Controls.ListView lvlEditPreview;
        private Controls.DualDataComboBox cmbCustomers;
        private System.Windows.Forms.Label lblRelation;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.Columns.Column colCustomer;
        private Controls.Columns.Column colChange;
        private Controls.Columns.Column colIcon;
    }
}