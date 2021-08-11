using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class NewKitchenPrinterDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewKitchenPrinterDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblPlaceHolderForErrorProvider = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEditHosts = new LSOne.Controls.ContextButton();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbPrinterName = new LSOne.Controls.DualDataComboBox();
            this.cmbPrintingStationHost = new LSOne.Controls.DualDataComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbVisualProfile = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.lblPlaceHolderForErrorProvider);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // lblPlaceHolderForErrorProvider
            // 
            resources.ApplyResources(this.lblPlaceHolderForErrorProvider, "lblPlaceHolderForErrorProvider");
            this.lblPlaceHolderForErrorProvider.Name = "lblPlaceHolderForErrorProvider";
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnEditHosts
            // 
            this.btnEditHosts.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditHosts, "btnEditHosts");
            this.btnEditHosts.Name = "btnEditHosts";
            this.btnEditHosts.Click += new System.EventHandler(this.btnEditHosts_Click);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cmbPrinterName
            // 
            this.cmbPrinterName.AddList = null;
            this.cmbPrinterName.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPrinterName, "cmbPrinterName");
            this.cmbPrinterName.EnableTextBox = true;
            this.cmbPrinterName.MaxLength = 32767;
            this.cmbPrinterName.Name = "cmbPrinterName";
            this.cmbPrinterName.OnlyDisplayID = false;
            this.cmbPrinterName.RemoveList = null;
            this.cmbPrinterName.RowHeight = ((short)(22));
            this.cmbPrinterName.SecondaryData = null;
            this.cmbPrinterName.SelectedData = null;
            this.cmbPrinterName.SelectedDataID = null;
            this.cmbPrinterName.SelectionList = null;
            this.cmbPrinterName.SkipIDColumn = true;
            this.cmbPrinterName.RequestData += new System.EventHandler(this.cmbPrinterName_RequestData);
            this.cmbPrinterName.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbPrinterName.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbPrintingStationHost
            // 
            this.cmbPrintingStationHost.AddList = null;
            this.cmbPrintingStationHost.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPrintingStationHost, "cmbPrintingStationHost");
            this.cmbPrintingStationHost.MaxLength = 32767;
            this.cmbPrintingStationHost.Name = "cmbPrintingStationHost";
            this.cmbPrintingStationHost.OnlyDisplayID = false;
            this.cmbPrintingStationHost.RemoveList = null;
            this.cmbPrintingStationHost.RowHeight = ((short)(22));
            this.cmbPrintingStationHost.SecondaryData = null;
            this.cmbPrintingStationHost.SelectedData = null;
            this.cmbPrintingStationHost.SelectedDataID = null;
            this.cmbPrintingStationHost.SelectionList = null;
            this.cmbPrintingStationHost.SkipIDColumn = true;
            this.cmbPrintingStationHost.RequestData += new System.EventHandler(this.cmbPrintingStationHost_RequestData);
            this.cmbPrintingStationHost.SelectedDataChanged += new System.EventHandler(this.cmbPrintingStationHost_SelectedDataChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbVisualProfile
            // 
            this.cmbVisualProfile.AddList = null;
            this.cmbVisualProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVisualProfile, "cmbVisualProfile");
            this.cmbVisualProfile.MaxLength = 32767;
            this.cmbVisualProfile.Name = "cmbVisualProfile";
            this.cmbVisualProfile.OnlyDisplayID = false;
            this.cmbVisualProfile.RemoveList = null;
            this.cmbVisualProfile.RowHeight = ((short)(22));
            this.cmbVisualProfile.SecondaryData = null;
            this.cmbVisualProfile.SelectedData = null;
            this.cmbVisualProfile.SelectedDataID = null;
            this.cmbVisualProfile.SelectionList = null;
            this.cmbVisualProfile.SkipIDColumn = true;
            this.cmbVisualProfile.RequestData += new System.EventHandler(this.cmbVisualProfile_RequestData);
            this.cmbVisualProfile.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // NewKitchenPrinterDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbVisualProfile);
            this.Controls.Add(this.cmbPrintingStationHost);
            this.Controls.Add(this.btnEditHosts);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbPrinterName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "NewKitchenPrinterDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbPrinterName, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.btnEditHosts, 0);
            this.Controls.SetChildIndex(this.cmbPrintingStationHost, 0);
            this.Controls.SetChildIndex(this.cmbVisualProfile, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private ContextButton btnEditHosts;
        private System.Windows.Forms.Label label9;
        private DualDataComboBox cmbPrinterName;
        private DualDataComboBox cmbPrintingStationHost;
        private System.Windows.Forms.Label lblPlaceHolderForErrorProvider;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbVisualProfile;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label2;
    }
}