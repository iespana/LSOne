namespace LSOne.ViewPlugins.Replenishment.Dialogs
{
    partial class InventoryTemplateCopyAreaDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryTemplateCopyAreaDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCopyFrom = new System.Windows.Forms.Label();
            this.cmbCopyFrom = new LSOne.Controls.DualDataComboBox();
            this.lvAreas = new LSOne.Controls.ListView();
            this.clmSelect = new LSOne.Controls.Columns.Column();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.lblAreas = new System.Windows.Forms.Label();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
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
            // lblCopyFrom
            // 
            resources.ApplyResources(this.lblCopyFrom, "lblCopyFrom");
            this.lblCopyFrom.Name = "lblCopyFrom";
            // 
            // cmbCopyFrom
            // 
            this.cmbCopyFrom.AddList = null;
            this.cmbCopyFrom.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCopyFrom, "cmbCopyFrom");
            this.cmbCopyFrom.MaxLength = 32767;
            this.cmbCopyFrom.Name = "cmbCopyFrom";
            this.cmbCopyFrom.NoChangeAllowed = false;
            this.cmbCopyFrom.OnlyDisplayID = false;
            this.cmbCopyFrom.RemoveList = null;
            this.cmbCopyFrom.RowHeight = ((short)(22));
            this.cmbCopyFrom.SecondaryData = null;
            this.cmbCopyFrom.SelectedData = null;
            this.cmbCopyFrom.SelectedDataID = null;
            this.cmbCopyFrom.SelectionList = null;
            this.cmbCopyFrom.SkipIDColumn = true;
            this.cmbCopyFrom.RequestData += new System.EventHandler(this.cmbCopyFrom_RequestData);
            this.cmbCopyFrom.SelectedDataChanged += new System.EventHandler(this.cmbCopyFrom_SelectedDataChanged);
            // 
            // lvAreas
            // 
            this.lvAreas.BuddyControl = null;
            this.lvAreas.Columns.Add(this.clmSelect);
            this.lvAreas.Columns.Add(this.clmDescription);
            this.lvAreas.ContentBackColor = System.Drawing.Color.White;
            this.lvAreas.DefaultRowHeight = ((short)(18));
            this.lvAreas.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvAreas.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lvAreas.HeaderHeight = ((short)(25));
            resources.ApplyResources(this.lvAreas, "lvAreas");
            this.lvAreas.Name = "lvAreas";
            this.lvAreas.OddRowColor = System.Drawing.Color.White;
            this.lvAreas.RowLineColor = System.Drawing.Color.LightGray;
            this.lvAreas.SecondarySortColumn = ((short)(-1));
            this.lvAreas.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvAreas.SortSetting = "0:1";
            this.lvAreas.CellAction += new LSOne.Controls.CellActionDelegate(this.lvAreas_CellAction);
            // 
            // clmSelect
            // 
            this.clmSelect.AutoSize = true;
            this.clmSelect.Clickable = false;
            this.clmSelect.DefaultStyle = null;
            resources.ApplyResources(this.clmSelect, "clmSelect");
            this.clmSelect.MaximumWidth = ((short)(50));
            this.clmSelect.MinimumWidth = ((short)(50));
            this.clmSelect.RelativeSize = 0;
            this.clmSelect.SecondarySortColumn = ((short)(-1));
            this.clmSelect.Tag = null;
            this.clmSelect.Width = ((short)(50));
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.Clickable = false;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.RelativeSize = 100;
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(50));
            // 
            // lblAreas
            // 
            resources.ApplyResources(this.lblAreas, "lblAreas");
            this.lblAreas.Name = "lblAreas";
            // 
            // btnSelectAll
            // 
            resources.ApplyResources(this.btnSelectAll, "btnSelectAll");
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectNone
            // 
            resources.ApplyResources(this.btnSelectNone, "btnSelectNone");
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // InventoryTemplateCopyAreaDialog
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.lblAreas);
            this.Controls.Add(this.lvAreas);
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.lblCopyFrom);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.HasHelp = true;
            this.Name = "InventoryTemplateCopyAreaDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblCopyFrom, 0);
            this.Controls.SetChildIndex(this.cmbCopyFrom, 0);
            this.Controls.SetChildIndex(this.lvAreas, 0);
            this.Controls.SetChildIndex(this.lblAreas, 0);
            this.Controls.SetChildIndex(this.btnSelectAll, 0);
            this.Controls.SetChildIndex(this.btnSelectNone, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCopyFrom;
        private Controls.DualDataComboBox cmbCopyFrom;
        private Controls.ListView lvAreas;
        private System.Windows.Forms.Label lblAreas;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private Controls.Columns.Column clmSelect;
        private Controls.Columns.Column clmDescription;
    }
}