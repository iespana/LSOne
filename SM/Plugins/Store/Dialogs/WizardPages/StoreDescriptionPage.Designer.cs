using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.Dialogs.WizardPages
{
    partial class StoreDescriptionPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreDescriptionPage));
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddCurrency = new ContextButton();
            this.cmbCurrency = new DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.addressField = new AddressControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label1.Name = "label1";
            // 
            // btnAddCurrency
            // 
            this.btnAddCurrency.BackColor = System.Drawing.Color.Transparent;
            this.btnAddCurrency.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddCurrency, "btnAddCurrency");
            this.btnAddCurrency.MaximumSize = new System.Drawing.Size(23, 23);
            this.btnAddCurrency.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnAddCurrency.Name = "btnAddCurrency";
            // 
            // cmbCurrency
            // 
            resources.ApplyResources(this.cmbCurrency, "cmbCurrency");
            this.cmbCurrency.MaxLength = 32767;
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.SelectedData = null;
            this.cmbCurrency.SkipIDColumn = true;
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // addressField
            // 
            this.addressField.BackColor = System.Drawing.Color.Transparent;
            this.addressField.DataModel = null;
            resources.ApplyResources(this.addressField, "addressField");
            this.addressField.MinimumSize = new System.Drawing.Size(478, 133);
            this.addressField.Name = "addressField";
            // 
            // StoreDescriptionPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.addressField);
            this.Controls.Add(this.btnAddCurrency);
            this.Controls.Add(this.cmbCurrency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "StoreDescriptionPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private ContextButton btnAddCurrency;
        private DualDataComboBox cmbCurrency;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private AddressControl addressField;
    }
}
