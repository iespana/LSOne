using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    partial class CustomerCardDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerCardDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbCardNumber = new LSOne.Controls.MSRTextBox(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCustomers = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.errorCardNumberLength = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorCardNumberLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
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
            // tbCardNumber
            // 
            this.tbCardNumber.EndCharacter = null;
            this.tbCardNumber.LastTrack = null;
            resources.ApplyResources(this.tbCardNumber, "tbCardNumber");
            this.tbCardNumber.ManualEntryOfTrack = true;
            this.tbCardNumber.Name = "tbCardNumber";
            this.tbCardNumber.NumericOnly = false;
            this.tbCardNumber.Seperator = null;
            this.tbCardNumber.StartCharacter = null;
            this.tbCardNumber.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.tbCardNumber.TextChanged += new System.EventHandler(this.CheckEnabled);
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
            // cmbCustomers
            // 
            this.cmbCustomers.AddList = null;
            this.cmbCustomers.EnableTextBox = true;
            resources.ApplyResources(this.cmbCustomers, "cmbCustomers");
            this.cmbCustomers.MaxLength = 32767;
            this.cmbCustomers.Name = "cmbCustomers";
            this.cmbCustomers.RemoveList = null;
            this.cmbCustomers.RowHeight = ((short)(22));
            this.cmbCustomers.SecondaryData = null;
            this.cmbCustomers.SelectedData = null;
            this.cmbCustomers.SelectionList = null;
            this.cmbCustomers.ShowDropDownOnTyping = true;
            this.cmbCustomers.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbCustomer_DropDown);
            this.cmbCustomers.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // errorCardNumberLength
            // 
            this.errorCardNumberLength.ContainerControl = this;
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // CustomerCardDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbCustomers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbCardNumber);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "CustomerCardDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.tbCardNumber, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbCustomers, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorCardNumberLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private MSRTextBox tbCardNumber;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label1;
        private DropDownFormComboBox cmbCustomers;
        private System.Windows.Forms.ErrorProvider errorCardNumberLength;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.Label label2;
    }
}