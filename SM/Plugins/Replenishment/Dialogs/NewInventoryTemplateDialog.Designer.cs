using LSOne.Controls;

namespace LSOne.ViewPlugins.Replenishment.Dialogs
{
    partial class NewInventoryTemplateDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewInventoryTemplateDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.cmbCopyExisting = new LSOne.Controls.DualDataComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkAllStores = new System.Windows.Forms.CheckBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbTemplateType = new System.Windows.Forms.ComboBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
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
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            this.cmbStore.EnableTextBox = true;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.NoChangeAllowed = false;
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.ShowDropDownOnTyping = true;
            this.cmbStore.SkipIDColumn = true;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            this.cmbStore.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbStore_DropDown);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbStore.RequestClear += new System.EventHandler(this.cmbStore_RequestClear);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbCopyExisting
            // 
            this.cmbCopyExisting.AddList = null;
            this.cmbCopyExisting.AllowKeyboardSelection = false;
            this.cmbCopyExisting.EnableTextBox = true;
            resources.ApplyResources(this.cmbCopyExisting, "cmbCopyExisting");
            this.cmbCopyExisting.MaxLength = 32767;
            this.cmbCopyExisting.Name = "cmbCopyExisting";
            this.cmbCopyExisting.NoChangeAllowed = false;
            this.cmbCopyExisting.OnlyDisplayID = false;
            this.cmbCopyExisting.RemoveList = null;
            this.cmbCopyExisting.RowHeight = ((short)(22));
            this.cmbCopyExisting.SecondaryData = null;
            this.cmbCopyExisting.SelectedData = null;
            this.cmbCopyExisting.SelectedDataID = null;
            this.cmbCopyExisting.SelectionList = null;
            this.cmbCopyExisting.ShowDropDownOnTyping = true;
            this.cmbCopyExisting.SkipIDColumn = true;
            this.cmbCopyExisting.RequestData += new System.EventHandler(this.cmbCopyExisting_RequestData);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // chkAllStores
            // 
            resources.ApplyResources(this.chkAllStores, "chkAllStores");
            this.chkAllStores.Name = "chkAllStores";
            this.chkAllStores.UseVisualStyleBackColor = true;
            this.chkAllStores.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lblType
            // 
            this.lblType.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.Name = "lblType";
            // 
            // cmbTemplateType
            // 
            this.cmbTemplateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTemplateType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbTemplateType, "cmbTemplateType");
            this.cmbTemplateType.Name = "cmbTemplateType";
            // 
            // NewInventoryTemplateDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbTemplateType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.chkAllStores);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbCopyExisting);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "NewInventoryTemplateDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbStore, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cmbCopyExisting, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.chkAllStores, 0);
            this.Controls.SetChildIndex(this.lblType, 0);
            this.Controls.SetChildIndex(this.cmbTemplateType, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label2;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label4;
        private DualDataComboBox cmbCopyExisting;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkAllStores;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbTemplateType;
        private System.Windows.Forms.Label lblType;
    }
}