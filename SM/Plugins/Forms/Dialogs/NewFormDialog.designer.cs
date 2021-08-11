using LSOne.Controls;

namespace LSOne.ViewPlugins.Forms.Dialogs
{
    partial class NewFormDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewFormDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCopyFrom = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblType = new System.Windows.Forms.Label();
            this.cmbFormType = new LSOne.Controls.DualDataComboBox();
            this.cmbPrintBehavior = new System.Windows.Forms.ComboBox();
            this.chkIsSlip = new System.Windows.Forms.CheckBox();
            this.tbPromptText = new System.Windows.Forms.TextBox();
            this.cmbCopyFrom = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ntbFormWidth = new LSOne.Controls.NumericTextBox();
            this.lblFormWidth = new System.Windows.Forms.Label();
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
            // lblCopyFrom
            // 
            resources.ApplyResources(this.lblCopyFrom, "lblCopyFrom");
            this.lblCopyFrom.Name = "lblCopyFrom";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblType
            // 
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.Name = "lblType";
            // 
            // cmbFormType
            // 
            this.cmbFormType.AddList = null;
            this.cmbFormType.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFormType, "cmbFormType");
            this.cmbFormType.MaxLength = 32767;
            this.cmbFormType.Name = "cmbFormType";
            this.cmbFormType.OnlyDisplayID = false;
            this.cmbFormType.RemoveList = null;
            this.cmbFormType.RowHeight = ((short)(22));
            this.cmbFormType.SecondaryData = null;
            this.cmbFormType.SelectedData = null;
            this.cmbFormType.SelectedDataID = null;
            this.cmbFormType.SelectionList = null;
            this.cmbFormType.SkipIDColumn = true;
            this.cmbFormType.RequestData += new System.EventHandler(this.cmbFormType_RequestData);
            this.cmbFormType.SelectedDataChanged += new System.EventHandler(this.cmbFormType_SelectedDataChanged);
            // 
            // cmbPrintBehavior
            // 
            this.cmbPrintBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrintBehavior.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPrintBehavior, "cmbPrintBehavior");
            this.cmbPrintBehavior.Name = "cmbPrintBehavior";
            this.cmbPrintBehavior.SelectedIndexChanged += new System.EventHandler(this.cmbPrintBehavior_SelectedIndexChanged);
            // 
            // chkIsSlip
            // 
            resources.ApplyResources(this.chkIsSlip, "chkIsSlip");
            this.chkIsSlip.Name = "chkIsSlip";
            this.chkIsSlip.UseVisualStyleBackColor = true;
            // 
            // tbPromptText
            // 
            resources.ApplyResources(this.tbPromptText, "tbPromptText");
            this.tbPromptText.Name = "tbPromptText";
            // 
            // cmbCopyFrom
            // 
            this.cmbCopyFrom.AddList = null;
            this.cmbCopyFrom.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCopyFrom, "cmbCopyFrom");
            this.cmbCopyFrom.MaxLength = 32767;
            this.cmbCopyFrom.Name = "cmbCopyFrom";
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
            this.cmbCopyFrom.RequestClear += new System.EventHandler(this.cmbCopyFrom_RequestClear);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ntbFormWidth
            // 
            this.ntbFormWidth.AllowDecimal = false;
            this.ntbFormWidth.AllowNegative = false;
            this.ntbFormWidth.CultureInfo = null;
            this.ntbFormWidth.DecimalLetters = 2;
            this.ntbFormWidth.HasMinValue = false;
            resources.ApplyResources(this.ntbFormWidth, "ntbFormWidth");
            this.ntbFormWidth.MaxValue = 0D;
            this.ntbFormWidth.MinValue = 0D;
            this.ntbFormWidth.Name = "ntbFormWidth";
            this.ntbFormWidth.Value = 0D;
            // 
            // lblFormWidth
            // 
            resources.ApplyResources(this.lblFormWidth, "lblFormWidth");
            this.lblFormWidth.Name = "lblFormWidth";
            // 
            // NewFormDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbFormType);
            this.Controls.Add(this.ntbFormWidth);
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.cmbPrintBehavior);
            this.Controls.Add(this.chkIsSlip);
            this.Controls.Add(this.tbPromptText);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblFormWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblCopyFrom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "NewFormDialog";
            this.Controls.SetChildIndex(this.lblCopyFrom, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.lblFormWidth, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.lblType, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.tbPromptText, 0);
            this.Controls.SetChildIndex(this.chkIsSlip, 0);
            this.Controls.SetChildIndex(this.cmbPrintBehavior, 0);
            this.Controls.SetChildIndex(this.cmbCopyFrom, 0);
            this.Controls.SetChildIndex(this.ntbFormWidth, 0);
            this.Controls.SetChildIndex(this.cmbFormType, 0);
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
        private System.Windows.Forms.Label lblCopyFrom;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblType;
        private DualDataComboBox cmbFormType;
        private System.Windows.Forms.ComboBox cmbPrintBehavior;
        private System.Windows.Forms.CheckBox chkIsSlip;
        private System.Windows.Forms.TextBox tbPromptText;
        private DualDataComboBox cmbCopyFrom;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private NumericTextBox ntbFormWidth;
        private System.Windows.Forms.Label lblFormWidth;
    }
}