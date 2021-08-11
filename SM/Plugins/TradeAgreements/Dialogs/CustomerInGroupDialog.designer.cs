using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    partial class CustomerInGroupDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerInGroupDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbGroup = new LSOne.Controls.DualDataComboBox();
            this.lblPriceGroup = new System.Windows.Forms.Label();
            this.btnAddGroup = new LSOne.Controls.ContextButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbGroup
            // 
            this.cmbGroup.AddList = null;
            this.cmbGroup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbGroup, "cmbGroup");
            this.cmbGroup.MaxLength = 32767;
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.NoChangeAllowed = false;
            this.cmbGroup.OnlyDisplayID = false;
            this.cmbGroup.RemoveList = null;
            this.cmbGroup.RowHeight = ((short)(22));
            this.cmbGroup.SecondaryData = null;
            this.cmbGroup.SelectedData = null;
            this.cmbGroup.SelectedDataID = null;
            this.cmbGroup.SelectionList = null;
            this.cmbGroup.SkipIDColumn = true;
            this.cmbGroup.RequestData += new System.EventHandler(this.cmbGroup_RequestData);
            this.cmbGroup.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbGroup.RequestClear += new System.EventHandler(this.RequestClear);
            // 
            // lblPriceGroup
            // 
            this.lblPriceGroup.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPriceGroup, "lblPriceGroup");
            this.lblPriceGroup.Name = "lblPriceGroup";
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnAddGroup.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddGroup, "btnAddGroup");
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // CustomerInGroupDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnAddGroup);
            this.Controls.Add(this.lblPriceGroup);
            this.Controls.Add(this.cmbGroup);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "CustomerInGroupDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbGroup, 0);
            this.Controls.SetChildIndex(this.lblPriceGroup, 0);
            this.Controls.SetChildIndex(this.btnAddGroup, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbGroup;
        private System.Windows.Forms.Label lblPriceGroup;
        private ContextButton btnAddGroup;
    }
}