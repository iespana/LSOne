namespace LSOne.ViewPlugins.Store.Dialogs
{
    partial class AddStoresToRegionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddStoresToRegionDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbStores = new LSOne.Controls.DualDataComboBox();
            this.lblStores = new System.Windows.Forms.Label();
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
            // cmbStores
            // 
            this.cmbStores.AddList = null;
            this.cmbStores.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStores, "cmbStores");
            this.cmbStores.MaxLength = 32767;
            this.cmbStores.Name = "cmbStores";
            this.cmbStores.NoChangeAllowed = false;
            this.cmbStores.OnlyDisplayID = false;
            this.cmbStores.RemoveList = null;
            this.cmbStores.RowHeight = ((short)(22));
            this.cmbStores.SecondaryData = null;
            this.cmbStores.SelectedData = null;
            this.cmbStores.SelectedDataID = null;
            this.cmbStores.SelectionList = null;
            this.cmbStores.SkipIDColumn = true;
            this.cmbStores.RequestData += new System.EventHandler(this.cmbStores_RequestData);
            this.cmbStores.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbStores_DropDown);
            this.cmbStores.SelectedDataChanged += new System.EventHandler(this.cmbStores_SelectedDataChanged);
            // 
            // lblStores
            // 
            resources.ApplyResources(this.lblStores, "lblStores");
            this.lblStores.Name = "lblStores";
            // 
            // AddStoresToRegionDialog
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbStores);
            this.Controls.Add(this.lblStores);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "AddStoresToRegionDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblStores, 0);
            this.Controls.SetChildIndex(this.cmbStores, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.DualDataComboBox cmbStores;
        private System.Windows.Forms.Label lblStores;
    }
}