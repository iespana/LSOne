using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    partial class StoreTenderGeneralPage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTenderGeneralPage));
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbRoundingMethod = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkOpenDrawer = new System.Windows.Forms.CheckBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ntbMinChangeAmount = new LSOne.Controls.NumericTextBox();
            this.ntbRounding = new LSOne.Controls.NumericTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbPaymentUpperLimit = new LSOne.Controls.DualDataComboBox();
            this.cmbPaymentMethodChange = new LSOne.Controls.DualDataComboBox();
            this.chkBlindRecount = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.label14 = new System.Windows.Forms.Label();
            this.chkPaymentMethod = new System.Windows.Forms.CheckBox();
            this.ntbMaxRecount = new LSOne.Controls.NumericTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.ntbMaxTDDifference = new LSOne.Controls.NumericTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbActions = new LSOne.Controls.DataComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ntbMinAmountAllowed = new LSOne.Controls.NumericTextBox();
            this.ntbMaxAmountAllowed = new LSOne.Controls.NumericTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmbRoundingMethod
            // 
            this.cmbRoundingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoundingMethod.FormattingEnabled = true;
            this.cmbRoundingMethod.Items.AddRange(new object[] {
            resources.GetString("cmbRoundingMethod.Items"),
            resources.GetString("cmbRoundingMethod.Items1"),
            resources.GetString("cmbRoundingMethod.Items2")});
            resources.ApplyResources(this.cmbRoundingMethod, "cmbRoundingMethod");
            this.cmbRoundingMethod.Name = "cmbRoundingMethod";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // chkOpenDrawer
            // 
            resources.ApplyResources(this.chkOpenDrawer, "chkOpenDrawer");
            this.chkOpenDrawer.Name = "chkOpenDrawer";
            this.chkOpenDrawer.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Coins.png");
            // 
            // ntbMinChangeAmount
            // 
            this.ntbMinChangeAmount.AllowDecimal = false;
            this.ntbMinChangeAmount.AllowNegative = false;
            this.ntbMinChangeAmount.CultureInfo = null;
            this.ntbMinChangeAmount.DecimalLetters = 2;
            resources.ApplyResources(this.ntbMinChangeAmount, "ntbMinChangeAmount");
            this.ntbMinChangeAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbMinChangeAmount.HasMinValue = false;
            this.ntbMinChangeAmount.MaxValue = 0D;
            this.ntbMinChangeAmount.MinValue = 0D;
            this.ntbMinChangeAmount.Name = "ntbMinChangeAmount";
            this.ntbMinChangeAmount.Value = 0D;
            // 
            // ntbRounding
            // 
            this.ntbRounding.AllowDecimal = true;
            this.ntbRounding.AllowNegative = false;
            this.ntbRounding.CultureInfo = null;
            this.ntbRounding.DecimalLetters = 2;
            this.ntbRounding.ForeColor = System.Drawing.Color.Black;
            this.ntbRounding.HasMinValue = false;
            resources.ApplyResources(this.ntbRounding, "ntbRounding");
            this.ntbRounding.MaxValue = 1D;
            this.ntbRounding.MinValue = 0D;
            this.ntbRounding.Name = "ntbRounding";
            this.ntbRounding.Value = 0D;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.ntbMinAmountAllowed);
            this.groupBox1.Controls.Add(this.ntbMaxAmountAllowed);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cmbPaymentUpperLimit);
            this.groupBox1.Controls.Add(this.cmbPaymentMethodChange);
            this.groupBox1.Controls.Add(this.chkBlindRecount);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.linkFields1);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.chkPaymentMethod);
            this.groupBox1.Controls.Add(this.ntbMaxRecount);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.ntbMaxTDDifference);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.ntbRounding);
            this.groupBox1.Controls.Add(this.chkOpenDrawer);
            this.groupBox1.Controls.Add(this.cmbActions);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbRoundingMethod);
            this.groupBox1.Controls.Add(this.ntbMinChangeAmount);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmbPaymentUpperLimit
            // 
            this.cmbPaymentUpperLimit.AddList = null;
            this.cmbPaymentUpperLimit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPaymentUpperLimit, "cmbPaymentUpperLimit");
            this.cmbPaymentUpperLimit.MaxLength = 32767;
            this.cmbPaymentUpperLimit.Name = "cmbPaymentUpperLimit";
            this.cmbPaymentUpperLimit.NoChangeAllowed = false;
            this.cmbPaymentUpperLimit.OnlyDisplayID = false;
            this.cmbPaymentUpperLimit.RemoveList = null;
            this.cmbPaymentUpperLimit.RowHeight = ((short)(22));
            this.cmbPaymentUpperLimit.SecondaryData = null;
            this.cmbPaymentUpperLimit.SelectedData = null;
            this.cmbPaymentUpperLimit.SelectedDataID = null;
            this.cmbPaymentUpperLimit.SelectionList = null;
            this.cmbPaymentUpperLimit.SkipIDColumn = true;
            this.cmbPaymentUpperLimit.RequestData += new System.EventHandler(this.cmbPaymentmethodupperLimit_RequestData);
            this.cmbPaymentUpperLimit.RequestClear += new System.EventHandler(this.ClearHandler);
            // 
            // cmbPaymentMethodChange
            // 
            this.cmbPaymentMethodChange.AccessibleRole = System.Windows.Forms.AccessibleRole.Grip;
            this.cmbPaymentMethodChange.AddList = null;
            this.cmbPaymentMethodChange.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPaymentMethodChange, "cmbPaymentMethodChange");
            this.cmbPaymentMethodChange.MaxLength = 32767;
            this.cmbPaymentMethodChange.Name = "cmbPaymentMethodChange";
            this.cmbPaymentMethodChange.NoChangeAllowed = false;
            this.cmbPaymentMethodChange.OnlyDisplayID = false;
            this.cmbPaymentMethodChange.RemoveList = null;
            this.cmbPaymentMethodChange.RowHeight = ((short)(22));
            this.cmbPaymentMethodChange.SecondaryData = null;
            this.cmbPaymentMethodChange.SelectedData = null;
            this.cmbPaymentMethodChange.SelectedDataID = null;
            this.cmbPaymentMethodChange.SelectionList = null;
            this.cmbPaymentMethodChange.SkipIDColumn = true;
            this.cmbPaymentMethodChange.RequestData += new System.EventHandler(this.cmbPaymentMethodForChange_RequestData);
            this.cmbPaymentMethodChange.RequestClear += new System.EventHandler(this.ClearHandler);
            // 
            // chkBlindRecount
            // 
            resources.ApplyResources(this.chkBlindRecount, "chkBlindRecount");
            this.chkBlindRecount.Name = "chkBlindRecount";
            this.chkBlindRecount.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Mode = LSOne.Controls.LinkFields.ModeEnum.Tripple;
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // chkPaymentMethod
            // 
            resources.ApplyResources(this.chkPaymentMethod, "chkPaymentMethod");
            this.chkPaymentMethod.Name = "chkPaymentMethod";
            this.chkPaymentMethod.UseVisualStyleBackColor = true;
            this.chkPaymentMethod.CheckedChanged += new System.EventHandler(this.chkPaymentMethod_CheckedChanged);
            // 
            // ntbMaxRecount
            // 
            this.ntbMaxRecount.AllowDecimal = false;
            this.ntbMaxRecount.AllowNegative = false;
            this.ntbMaxRecount.CultureInfo = null;
            this.ntbMaxRecount.DecimalLetters = 2;
            this.ntbMaxRecount.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxRecount.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxRecount, "ntbMaxRecount");
            this.ntbMaxRecount.MaxValue = 999999999D;
            this.ntbMaxRecount.MinValue = 0D;
            this.ntbMaxRecount.Name = "ntbMaxRecount";
            this.ntbMaxRecount.Value = 0D;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // ntbMaxTDDifference
            // 
            this.ntbMaxTDDifference.AllowDecimal = true;
            this.ntbMaxTDDifference.AllowNegative = false;
            this.ntbMaxTDDifference.CultureInfo = null;
            this.ntbMaxTDDifference.DecimalLetters = 2;
            this.ntbMaxTDDifference.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxTDDifference.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxTDDifference, "ntbMaxTDDifference");
            this.ntbMaxTDDifference.MaxValue = 9999999999999D;
            this.ntbMaxTDDifference.MinValue = 0D;
            this.ntbMaxTDDifference.Name = "ntbMaxTDDifference";
            this.ntbMaxTDDifference.Value = 0D;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cmbActions
            // 
            this.cmbActions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbActions.FormattingEnabled = true;
            resources.ApplyResources(this.cmbActions, "cmbActions");
            this.cmbActions.Name = "cmbActions";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // ntbMinAmountAllowed
            // 
            this.ntbMinAmountAllowed.AllowDecimal = true;
            this.ntbMinAmountAllowed.AllowNegative = false;
            this.ntbMinAmountAllowed.CultureInfo = null;
            this.ntbMinAmountAllowed.DecimalLetters = 2;
            this.ntbMinAmountAllowed.ForeColor = System.Drawing.Color.Black;
            this.ntbMinAmountAllowed.HasMinValue = false;
            resources.ApplyResources(this.ntbMinAmountAllowed, "ntbMinAmountAllowed");
            this.ntbMinAmountAllowed.MaxValue = 9999999999999D;
            this.ntbMinAmountAllowed.MinValue = 0D;
            this.ntbMinAmountAllowed.Name = "ntbMinAmountAllowed";
            this.ntbMinAmountAllowed.Value = 0D;
            // 
            // ntbMaxAmountAllowed
            // 
            this.ntbMaxAmountAllowed.AllowDecimal = true;
            this.ntbMaxAmountAllowed.AllowNegative = false;
            this.ntbMaxAmountAllowed.CultureInfo = null;
            this.ntbMaxAmountAllowed.DecimalLetters = 2;
            this.ntbMaxAmountAllowed.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxAmountAllowed.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxAmountAllowed, "ntbMaxAmountAllowed");
            this.ntbMaxAmountAllowed.MaxValue = 9999999999999D;
            this.ntbMaxAmountAllowed.MinValue = 0D;
            this.ntbMaxAmountAllowed.Name = "ntbMaxAmountAllowed";
            this.ntbMaxAmountAllowed.Value = 0D;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // StoreTenderGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "StoreTenderGeneralPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

       
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbRoundingMethod;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkOpenDrawer;
        private NumericTextBox ntbRounding;
        private NumericTextBox ntbMinChangeAmount;
        private DataComboBox cmbActions;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label13;
        private NumericTextBox ntbMaxTDDifference;
        private System.Windows.Forms.Label label8;
        private NumericTextBox ntbMaxRecount;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkPaymentMethod;
        private LinkFields linkFields1;
        private System.Windows.Forms.CheckBox chkBlindRecount;
        private System.Windows.Forms.Label label15;
        private DualDataComboBox cmbPaymentMethodChange;
        private DualDataComboBox cmbPaymentUpperLimit;
        private System.Windows.Forms.Label label9;
        private NumericTextBox ntbMinAmountAllowed;
        private NumericTextBox ntbMaxAmountAllowed;
        private System.Windows.Forms.Label label10;
    }
}
