using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    partial class EditPosMenuHeaderDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditPosMenuHeaderDialog));
            this.pnlBottom = new DialogBottomPanel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label13 = new System.Windows.Forms.Label();
            this.groupPanel3 = new GroupPanel();
            this.chkUseNavOperation = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbAppliesTo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupPanel1 = new GroupPanel();
            this.ntbRows = new NumericTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ntbColumns = new NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupPanel2 = new GroupPanel();
            this.cmbShape = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.cmbGradientMode = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cwBackColor2 = new ColorWell();
            this.label8 = new System.Windows.Forms.Label();
            this.ntbFontCharset = new NumericTextBox();
            this.cwForeColor = new ColorWell();
            this.cwBackColor = new ColorWell();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.ntbFontSize = new NumericTextBox();
            this.chkFontItalic = new System.Windows.Forms.CheckBox();
            this.chkFontBold = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnEditFont = new ContextButton();
            this.tbFontName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel3.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Name = "pnlBottom";
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
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label13.Name = "label13";
            // 
            // groupPanel3
            // 
            this.groupPanel3.Controls.Add(this.chkUseNavOperation);
            this.groupPanel3.Controls.Add(this.label5);
            this.groupPanel3.Controls.Add(this.cmbAppliesTo);
            this.groupPanel3.Controls.Add(this.label4);
            this.groupPanel3.Controls.Add(this.tbDescription);
            this.groupPanel3.Controls.Add(this.label3);
            resources.ApplyResources(this.groupPanel3, "groupPanel3");
            this.groupPanel3.Name = "groupPanel3";
            // 
            // chkUseNavOperation
            // 
            resources.ApplyResources(this.chkUseNavOperation, "chkUseNavOperation");
            this.chkUseNavOperation.BackColor = System.Drawing.Color.Transparent;
            this.chkUseNavOperation.Name = "chkUseNavOperation";
            this.chkUseNavOperation.UseVisualStyleBackColor = false;
            this.chkUseNavOperation.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbAppliesTo
            // 
            this.cmbAppliesTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAppliesTo.FormattingEnabled = true;
            this.cmbAppliesTo.Items.AddRange(new object[] {
            resources.GetString("cmbAppliesTo.Items"),
            resources.GetString("cmbAppliesTo.Items1"),
            resources.GetString("cmbAppliesTo.Items2"),
            resources.GetString("cmbAppliesTo.Items3")});
            resources.ApplyResources(this.cmbAppliesTo, "cmbAppliesTo");
            this.cmbAppliesTo.Name = "cmbAppliesTo";
            this.cmbAppliesTo.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
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
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label1.Name = "label1";
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.ntbRows);
            this.groupPanel1.Controls.Add(this.label7);
            this.groupPanel1.Controls.Add(this.ntbColumns);
            this.groupPanel1.Controls.Add(this.label6);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // ntbRows
            // 
            this.ntbRows.AllowDecimal = false;
            this.ntbRows.AllowNegative = false;
            this.ntbRows.CultureInfo = null;
            this.ntbRows.DecimalLetters = 2;
            this.ntbRows.HasMinValue = false;
            resources.ApplyResources(this.ntbRows, "ntbRows");
            this.ntbRows.MaxValue = 0D;
            this.ntbRows.MinValue = 0D;
            this.ntbRows.Name = "ntbRows";
            this.ntbRows.Value = 0D;
            this.ntbRows.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // ntbColumns
            // 
            this.ntbColumns.AllowDecimal = false;
            this.ntbColumns.AllowNegative = false;
            this.ntbColumns.CultureInfo = null;
            this.ntbColumns.DecimalLetters = 2;
            this.ntbColumns.HasMinValue = false;
            resources.ApplyResources(this.ntbColumns, "ntbColumns");
            this.ntbColumns.MaxValue = 0D;
            this.ntbColumns.MinValue = 0D;
            this.ntbColumns.Name = "ntbColumns";
            this.ntbColumns.Value = 0D;
            this.ntbColumns.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label2.Name = "label2";
            // 
            // groupPanel2
            // 
            this.groupPanel2.Controls.Add(this.cmbShape);
            this.groupPanel2.Controls.Add(this.label18);
            this.groupPanel2.Controls.Add(this.cmbGradientMode);
            this.groupPanel2.Controls.Add(this.label17);
            this.groupPanel2.Controls.Add(this.cwBackColor2);
            this.groupPanel2.Controls.Add(this.label8);
            this.groupPanel2.Controls.Add(this.ntbFontCharset);
            this.groupPanel2.Controls.Add(this.cwForeColor);
            this.groupPanel2.Controls.Add(this.cwBackColor);
            this.groupPanel2.Controls.Add(this.label16);
            this.groupPanel2.Controls.Add(this.label15);
            this.groupPanel2.Controls.Add(this.label14);
            this.groupPanel2.Controls.Add(this.ntbFontSize);
            this.groupPanel2.Controls.Add(this.chkFontItalic);
            this.groupPanel2.Controls.Add(this.chkFontBold);
            this.groupPanel2.Controls.Add(this.label12);
            this.groupPanel2.Controls.Add(this.label11);
            this.groupPanel2.Controls.Add(this.label10);
            this.groupPanel2.Controls.Add(this.btnEditFont);
            this.groupPanel2.Controls.Add(this.tbFontName);
            this.groupPanel2.Controls.Add(this.label9);
            resources.ApplyResources(this.groupPanel2, "groupPanel2");
            this.groupPanel2.Name = "groupPanel2";
            // 
            // cmbShape
            // 
            this.cmbShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShape.FormattingEnabled = true;
            resources.ApplyResources(this.cmbShape, "cmbShape");
            this.cmbShape.Name = "cmbShape";
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // cmbGradientMode
            // 
            this.cmbGradientMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGradientMode.FormattingEnabled = true;
            resources.ApplyResources(this.cmbGradientMode, "cmbGradientMode");
            this.cmbGradientMode.Name = "cmbGradientMode";
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // cwBackColor2
            // 
            resources.ApplyResources(this.cwBackColor2, "cwBackColor2");
            this.cwBackColor2.Name = "cwBackColor2";
            this.cwBackColor2.SelectedColor = System.Drawing.Color.White;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // ntbFontCharset
            // 
            this.ntbFontCharset.AllowDecimal = false;
            this.ntbFontCharset.AllowNegative = false;
            this.ntbFontCharset.CultureInfo = null;
            this.ntbFontCharset.DecimalLetters = 2;
            this.ntbFontCharset.HasMinValue = false;
            resources.ApplyResources(this.ntbFontCharset, "ntbFontCharset");
            this.ntbFontCharset.MaxValue = 0D;
            this.ntbFontCharset.MinValue = 0D;
            this.ntbFontCharset.Name = "ntbFontCharset";
            this.ntbFontCharset.ReadOnly = true;
            this.ntbFontCharset.Value = 0D;
            this.ntbFontCharset.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cwForeColor
            // 
            resources.ApplyResources(this.cwForeColor, "cwForeColor");
            this.cwForeColor.Name = "cwForeColor";
            this.cwForeColor.SelectedColor = System.Drawing.Color.White;
            this.cwForeColor.SelectedColorChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cwBackColor
            // 
            resources.ApplyResources(this.cwBackColor, "cwBackColor");
            this.cwBackColor.Name = "cwBackColor";
            this.cwBackColor.SelectedColor = System.Drawing.Color.White;
            this.cwBackColor.SelectedColorChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.ForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // ntbFontSize
            // 
            this.ntbFontSize.AllowDecimal = false;
            this.ntbFontSize.AllowNegative = false;
            this.ntbFontSize.CultureInfo = null;
            this.ntbFontSize.DecimalLetters = 2;
            this.ntbFontSize.HasMinValue = false;
            resources.ApplyResources(this.ntbFontSize, "ntbFontSize");
            this.ntbFontSize.MaxValue = 0D;
            this.ntbFontSize.MinValue = 0D;
            this.ntbFontSize.Name = "ntbFontSize";
            this.ntbFontSize.ReadOnly = true;
            this.ntbFontSize.Value = 0D;
            this.ntbFontSize.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // chkFontItalic
            // 
            resources.ApplyResources(this.chkFontItalic, "chkFontItalic");
            this.chkFontItalic.BackColor = System.Drawing.Color.Transparent;
            this.chkFontItalic.Name = "chkFontItalic";
            this.chkFontItalic.UseVisualStyleBackColor = false;
            this.chkFontItalic.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // chkFontBold
            // 
            resources.ApplyResources(this.chkFontBold, "chkFontBold");
            this.chkFontBold.BackColor = System.Drawing.Color.Transparent;
            this.chkFontBold.Name = "chkFontBold";
            this.chkFontBold.UseVisualStyleBackColor = false;
            this.chkFontBold.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.ForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.ForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.ForeColor = System.Drawing.SystemColors.GrayText;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // btnEditFont
            // 
            this.btnEditFont.BackColor = System.Drawing.Color.Transparent;
            this.btnEditFont.Context = ButtonType.Edit;
            resources.ApplyResources(this.btnEditFont, "btnEditFont");
            this.btnEditFont.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnEditFont.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnEditFont.Name = "btnEditFont";
            this.btnEditFont.Click += new System.EventHandler(this.btnEditFont_Click);
            // 
            // tbFontName
            // 
            resources.ApplyResources(this.tbFontName, "tbFontName");
            this.tbFontName.Name = "tbFontName";
            this.tbFontName.ReadOnly = true;
            this.tbFontName.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // EditPosMenuHeaderDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.pnlBottom);
            this.HasHelp = true;
            this.Name = "EditPosMenuHeaderDialog";
            this.Controls.SetChildIndex(this.pnlBottom, 0);
            this.Controls.SetChildIndex(this.groupPanel3, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.groupPanel1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.groupPanel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DialogBottomPanel pnlBottom;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label13;
        private GroupPanel groupPanel3;
        private System.Windows.Forms.Label label1;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label label2;
        private GroupPanel groupPanel2;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbAppliesTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkUseNavOperation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private NumericTextBox ntbRows;
        private System.Windows.Forms.Label label7;
        private NumericTextBox ntbColumns;
        private System.Windows.Forms.FontDialog fontDialog1;
        private ContextButton btnEditFont;
        private System.Windows.Forms.TextBox tbFontName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private ColorWell cwForeColor;
        private ColorWell cwBackColor;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private NumericTextBox ntbFontSize;
        private System.Windows.Forms.CheckBox chkFontItalic;
        private System.Windows.Forms.CheckBox chkFontBold;
        private System.Windows.Forms.Label label12;
        private NumericTextBox ntbFontCharset;
        private ColorWell cwBackColor2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbGradientMode;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cmbShape;
        private System.Windows.Forms.Label label18;
    }
}