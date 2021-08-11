using LSOne.Controls;

namespace LSOne.ViewPlugins.TouchButtons.Dialogs
{
    partial class NewButtonDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewButtonDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbOperation = new LSOne.Controls.DualDataComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblParameter2 = new System.Windows.Forms.Label();
            this.cmbParameter2 = new LSOne.Controls.DualDataComboBox();
            this.lblParameter1 = new System.Windows.Forms.Label();
            this.cmbParameter = new LSOne.Controls.DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ntbNoOfLinesToCreate = new LSOne.Controls.NumericTextBox();
            this.chkCreateMultipleLines = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCopyFrom = new LSOne.Controls.DualDataComboBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            // cmbOperation
            // 
            this.cmbOperation.AddList = null;
            this.cmbOperation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbOperation, "cmbOperation");
            this.cmbOperation.MaxLength = 32767;
            this.cmbOperation.Name = "cmbOperation";
            this.cmbOperation.NoChangeAllowed = false;
            this.cmbOperation.OnlyDisplayID = false;
            this.cmbOperation.RemoveList = null;
            this.cmbOperation.RowHeight = ((short)(22));
            this.cmbOperation.SecondaryData = null;
            this.cmbOperation.SelectedData = null;
            this.cmbOperation.SelectedDataID = null;
            this.cmbOperation.SelectionList = null;
            this.cmbOperation.SkipIDColumn = true;
            this.cmbOperation.RequestData += new System.EventHandler(this.cmbOperation_RequestData);
            this.cmbOperation.SelectedDataChanged += new System.EventHandler(this.cmbOperation_SelectedDataChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblParameter2);
            this.groupBox1.Controls.Add(this.cmbParameter2);
            this.groupBox1.Controls.Add(this.lblParameter1);
            this.groupBox1.Controls.Add(this.cmbParameter);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lblParameter2
            // 
            this.lblParameter2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblParameter2, "lblParameter2");
            this.lblParameter2.Name = "lblParameter2";
            // 
            // cmbParameter2
            // 
            this.cmbParameter2.AddList = null;
            this.cmbParameter2.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbParameter2, "cmbParameter2");
            this.cmbParameter2.MaxLength = 32767;
            this.cmbParameter2.Name = "cmbParameter2";
            this.cmbParameter2.NoChangeAllowed = false;
            this.cmbParameter2.OnlyDisplayID = false;
            this.cmbParameter2.RemoveList = null;
            this.cmbParameter2.RowHeight = ((short)(22));
            this.cmbParameter2.SecondaryData = null;
            this.cmbParameter2.SelectedData = null;
            this.cmbParameter2.SelectedDataID = null;
            this.cmbParameter2.SelectionList = null;
            this.cmbParameter2.SkipIDColumn = true;
            this.cmbParameter2.RequestData += new System.EventHandler(this.cmbParameter2_RequestData);
            this.cmbParameter2.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbParameter2_FormatData);
            this.cmbParameter2.SelectedDataChanged += new System.EventHandler(this.cmbParameter2_SelectedDataChanged);
            this.cmbParameter2.RequestClear += new System.EventHandler(this.cmbParameter_RequestClear);
            // 
            // lblParameter1
            // 
            this.lblParameter1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblParameter1, "lblParameter1");
            this.lblParameter1.Name = "lblParameter1";
            // 
            // cmbParameter
            // 
            this.cmbParameter.AddList = null;
            this.cmbParameter.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbParameter, "cmbParameter");
            this.cmbParameter.EnableTextBox = true;
            this.cmbParameter.MaxLength = 32767;
            this.cmbParameter.Name = "cmbParameter";
            this.cmbParameter.NoChangeAllowed = false;
            this.cmbParameter.OnlyDisplayID = false;
            this.cmbParameter.RemoveList = null;
            this.cmbParameter.RowHeight = ((short)(22));
            this.cmbParameter.SecondaryData = null;
            this.cmbParameter.SelectedData = null;
            this.cmbParameter.SelectedDataID = null;
            this.cmbParameter.SelectionList = null;
            this.cmbParameter.ShowDropDownOnTyping = true;
            this.cmbParameter.SkipIDColumn = true;
            this.cmbParameter.RequestData += new System.EventHandler(this.cmbParameter_RequestData);
            this.cmbParameter.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbParameter_DropDown);
            this.cmbParameter.SelectedDataChanged += new System.EventHandler(this.cmbParameter_SelectedDataChanged);
            this.cmbParameter.RequestClear += new System.EventHandler(this.cmbParameter_RequestClear);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbNoOfLinesToCreate
            // 
            this.ntbNoOfLinesToCreate.AllowDecimal = false;
            this.ntbNoOfLinesToCreate.AllowNegative = false;
            this.ntbNoOfLinesToCreate.CultureInfo = null;
            this.ntbNoOfLinesToCreate.DecimalLetters = 0;
            resources.ApplyResources(this.ntbNoOfLinesToCreate, "ntbNoOfLinesToCreate");
            this.ntbNoOfLinesToCreate.ForeColor = System.Drawing.Color.Black;
            this.ntbNoOfLinesToCreate.HasMinValue = true;
            this.ntbNoOfLinesToCreate.MaxValue = 999D;
            this.ntbNoOfLinesToCreate.MinValue = 1D;
            this.ntbNoOfLinesToCreate.Name = "ntbNoOfLinesToCreate";
            this.ntbNoOfLinesToCreate.Value = 0D;
            // 
            // chkCreateMultipleLines
            // 
            resources.ApplyResources(this.chkCreateMultipleLines, "chkCreateMultipleLines");
            this.chkCreateMultipleLines.Name = "chkCreateMultipleLines";
            this.chkCreateMultipleLines.UseVisualStyleBackColor = true;
            this.chkCreateMultipleLines.CheckedChanged += new System.EventHandler(this.chkCreateMultipleLines_CheckedChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
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
            // 
            // NewButtonDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkCreateMultipleLines);
            this.Controls.Add(this.ntbNoOfLinesToCreate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbOperation);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label5);
            this.HasHelp = true;
            this.Name = "NewButtonDialog";
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbOperation, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.ntbNoOfLinesToCreate, 0);
            this.Controls.SetChildIndex(this.chkCreateMultipleLines, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.linkFields1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbCopyFrom, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblParameter2;
        private DualDataComboBox cmbParameter2;
        private System.Windows.Forms.Label lblParameter1;
        private DualDataComboBox cmbParameter;
        private DualDataComboBox cmbOperation;
        private System.Windows.Forms.Label label5;
        private LinkFields linkFields1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkCreateMultipleLines;
        private NumericTextBox ntbNoOfLinesToCreate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private DualDataComboBox cmbCopyFrom;
    }
}