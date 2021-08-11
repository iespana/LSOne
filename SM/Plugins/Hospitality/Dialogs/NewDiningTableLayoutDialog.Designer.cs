using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    partial class NewDiningTableLayoutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDiningTableLayoutDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ntbDiningTableColumns = new NumericTextBox();
            this.ntbDiningTableRows = new NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbCopyFrom = new DualDataComboBox();
            this.tbHospitalityTypeDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
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
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ntbDiningTableColumns
            // 
            this.ntbDiningTableColumns.AllowDecimal = false;
            this.ntbDiningTableColumns.AllowNegative = false;
            this.ntbDiningTableColumns.DecimalLetters = 2;
            this.ntbDiningTableColumns.HasMinValue = false;
            resources.ApplyResources(this.ntbDiningTableColumns, "ntbDiningTableColumns");
            this.ntbDiningTableColumns.MaxValue = 0D;
            this.ntbDiningTableColumns.MinValue = 0D;
            this.ntbDiningTableColumns.Name = "ntbDiningTableColumns";
            this.ntbDiningTableColumns.Value = 0D;
            // 
            // ntbDiningTableRows
            // 
            this.ntbDiningTableRows.AllowDecimal = false;
            this.ntbDiningTableRows.AllowNegative = false;
            this.ntbDiningTableRows.DecimalLetters = 2;
            this.ntbDiningTableRows.HasMinValue = false;
            resources.ApplyResources(this.ntbDiningTableRows, "ntbDiningTableRows");
            this.ntbDiningTableRows.MaxValue = 0D;
            this.ntbDiningTableRows.MinValue = 0D;
            this.ntbDiningTableRows.Name = "ntbDiningTableRows";
            this.ntbDiningTableRows.Value = 0D;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cmbCopyFrom
            // 
            resources.ApplyResources(this.cmbCopyFrom, "cmbCopyFrom");
            this.cmbCopyFrom.MaxLength = 32767;
            this.cmbCopyFrom.Name = "cmbCopyFrom";
            this.cmbCopyFrom.SelectedData = null;
            this.cmbCopyFrom.SkipIDColumn = true;
            this.cmbCopyFrom.RequestData += new System.EventHandler(this.cmbCopyFrom_RequestData);
            this.cmbCopyFrom.SelectedDataChanged += new System.EventHandler(this.cmbCopyFrom_SelectedDataChanged);
            // 
            // tbHospitalityTypeDescription
            // 
            resources.ApplyResources(this.tbHospitalityTypeDescription, "tbHospitalityTypeDescription");
            this.tbHospitalityTypeDescription.Name = "tbHospitalityTypeDescription";
            this.tbHospitalityTypeDescription.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // NewDiningTableLayoutDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbHospitalityTypeDescription);
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ntbDiningTableColumns);
            this.Controls.Add(this.ntbDiningTableRows);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "NewDiningTableLayoutDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.ntbDiningTableRows, 0);
            this.Controls.SetChildIndex(this.ntbDiningTableColumns, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.cmbCopyFrom, 0);
            this.Controls.SetChildIndex(this.tbHospitalityTypeDescription, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private NumericTextBox ntbDiningTableColumns;
        private NumericTextBox ntbDiningTableRows;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private DualDataComboBox cmbCopyFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbHospitalityTypeDescription;
    }
}