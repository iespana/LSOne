using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    partial class CurrencyDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurrencyDialog));
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ntbRoundoffSales = new LSOne.Controls.NumericTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ntbRoundoffPurch = new LSOne.Controls.NumericTextBox();
            this.ntbRoundoffAmt = new LSOne.Controls.NumericTextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbRoundoffTypePurch = new System.Windows.Forms.ComboBox();
            this.cmbRoundoffTypeAmt = new System.Windows.Forms.ComboBox();
            this.cmbRoundoffTypeSales = new System.Windows.Forms.ComboBox();
            this.lblPreview = new System.Windows.Forms.Label();
            this.tbPrefix = new System.Windows.Forms.TextBox();
            this.tbSuffix = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblPreviewResult = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbID
            // 
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            this.tbID.TextChanged += new System.EventHandler(this.tbID_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ntbRoundoffSales
            // 
            this.ntbRoundoffSales.AllowDecimal = true;
            this.ntbRoundoffSales.AllowNegative = false;
            this.ntbRoundoffSales.CultureInfo = null;
            this.ntbRoundoffSales.DecimalLetters = 2;
            this.ntbRoundoffSales.HasMinValue = false;
            resources.ApplyResources(this.ntbRoundoffSales, "ntbRoundoffSales");
            this.ntbRoundoffSales.MaxValue = 10000000D;
            this.ntbRoundoffSales.MinValue = 0D;
            this.ntbRoundoffSales.Name = "ntbRoundoffSales";
            this.ntbRoundoffSales.Value = 0.01D;
            this.ntbRoundoffSales.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // ntbRoundoffPurch
            // 
            this.ntbRoundoffPurch.AllowDecimal = true;
            this.ntbRoundoffPurch.AllowNegative = false;
            this.ntbRoundoffPurch.CultureInfo = null;
            this.ntbRoundoffPurch.DecimalLetters = 2;
            this.ntbRoundoffPurch.HasMinValue = false;
            resources.ApplyResources(this.ntbRoundoffPurch, "ntbRoundoffPurch");
            this.ntbRoundoffPurch.MaxValue = 10000000D;
            this.ntbRoundoffPurch.MinValue = 0D;
            this.ntbRoundoffPurch.Name = "ntbRoundoffPurch";
            this.ntbRoundoffPurch.Value = 0.01D;
            this.ntbRoundoffPurch.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ntbRoundoffAmt
            // 
            this.ntbRoundoffAmt.AllowDecimal = true;
            this.ntbRoundoffAmt.AllowNegative = false;
            this.ntbRoundoffAmt.CultureInfo = null;
            this.ntbRoundoffAmt.DecimalLetters = 2;
            this.ntbRoundoffAmt.HasMinValue = false;
            resources.ApplyResources(this.ntbRoundoffAmt, "ntbRoundoffAmt");
            this.ntbRoundoffAmt.MaxValue = 10000000D;
            this.ntbRoundoffAmt.MinValue = 0D;
            this.ntbRoundoffAmt.Name = "ntbRoundoffAmt";
            this.ntbRoundoffAmt.Value = 0.01D;
            this.ntbRoundoffAmt.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbRoundoffTypePurch);
            this.groupBox1.Controls.Add(this.cmbRoundoffTypeAmt);
            this.groupBox1.Controls.Add(this.cmbRoundoffTypeSales);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ntbRoundoffSales);
            this.groupBox1.Controls.Add(this.ntbRoundoffPurch);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.ntbRoundoffAmt);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmbRoundoffTypePurch
            // 
            this.cmbRoundoffTypePurch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoundoffTypePurch.FormattingEnabled = true;
            this.cmbRoundoffTypePurch.Items.AddRange(new object[] {
            resources.GetString("cmbRoundoffTypePurch.Items"),
            resources.GetString("cmbRoundoffTypePurch.Items1"),
            resources.GetString("cmbRoundoffTypePurch.Items2")});
            resources.ApplyResources(this.cmbRoundoffTypePurch, "cmbRoundoffTypePurch");
            this.cmbRoundoffTypePurch.Name = "cmbRoundoffTypePurch";
            this.cmbRoundoffTypePurch.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbRoundoffTypeAmt
            // 
            this.cmbRoundoffTypeAmt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoundoffTypeAmt.FormattingEnabled = true;
            this.cmbRoundoffTypeAmt.Items.AddRange(new object[] {
            resources.GetString("cmbRoundoffTypeAmt.Items"),
            resources.GetString("cmbRoundoffTypeAmt.Items1"),
            resources.GetString("cmbRoundoffTypeAmt.Items2")});
            resources.ApplyResources(this.cmbRoundoffTypeAmt, "cmbRoundoffTypeAmt");
            this.cmbRoundoffTypeAmt.Name = "cmbRoundoffTypeAmt";
            this.cmbRoundoffTypeAmt.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbRoundoffTypeSales
            // 
            this.cmbRoundoffTypeSales.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoundoffTypeSales.FormattingEnabled = true;
            this.cmbRoundoffTypeSales.Items.AddRange(new object[] {
            resources.GetString("cmbRoundoffTypeSales.Items"),
            resources.GetString("cmbRoundoffTypeSales.Items1"),
            resources.GetString("cmbRoundoffTypeSales.Items2")});
            resources.ApplyResources(this.cmbRoundoffTypeSales, "cmbRoundoffTypeSales");
            this.cmbRoundoffTypeSales.Name = "cmbRoundoffTypeSales";
            this.cmbRoundoffTypeSales.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblPreview
            // 
            this.lblPreview.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPreview, "lblPreview");
            this.lblPreview.Name = "lblPreview";
            // 
            // tbPrefix
            // 
            resources.ApplyResources(this.tbPrefix, "tbPrefix");
            this.tbPrefix.Name = "tbPrefix";
            this.tbPrefix.TextChanged += new System.EventHandler(this.tbPrefix_TextChanged);
            // 
            // tbSuffix
            // 
            resources.ApplyResources(this.tbSuffix, "tbSuffix");
            this.tbSuffix.Name = "tbSuffix";
            this.tbSuffix.TextChanged += new System.EventHandler(this.tbSuffix_TextChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // lblPreviewResult
            // 
            this.lblPreviewResult.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPreviewResult, "lblPreviewResult");
            this.lblPreviewResult.Name = "lblPreviewResult";
            // 
            // CurrencyDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblPreviewResult);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbSuffix);
            this.Controls.Add(this.tbPrefix);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "CurrencyDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.tbID, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label18, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.lblPreview, 0);
            this.Controls.SetChildIndex(this.tbPrefix, 0);
            this.Controls.SetChildIndex(this.tbSuffix, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.lblPreviewResult, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private NumericTextBox ntbRoundoffSales;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private NumericTextBox ntbRoundoffAmt;
        private System.Windows.Forms.Label label14;
        private NumericTextBox ntbRoundoffPurch;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbRoundoffTypeSales;
        private System.Windows.Forms.ComboBox cmbRoundoffTypePurch;
        private System.Windows.Forms.ComboBox cmbRoundoffTypeAmt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSuffix;
        private System.Windows.Forms.TextBox tbPrefix;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblPreviewResult;
    }
}