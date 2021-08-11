using LSOne.Controls;

namespace LSOne.ViewPlugins.BarCodes.Dialogs
{
    partial class BarCodeMaskSegmentDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BarCodeMaskSegmentDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbSegmentNum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.tbCharacter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ntbLength = new LSOne.Controls.NumericTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ntbDecimals = new LSOne.Controls.NumericTextBox();
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
            // tbSegmentNum
            // 
            resources.ApplyResources(this.tbSegmentNum, "tbSegmentNum");
            this.tbSegmentNum.Name = "tbSegmentNum";
            this.tbSegmentNum.ReadOnly = true;
            this.tbSegmentNum.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1"),
            resources.GetString("cmbType.Items2"),
            resources.GetString("cmbType.Items3"),
            resources.GetString("cmbType.Items4"),
            resources.GetString("cmbType.Items5"),
            resources.GetString("cmbType.Items6"),
            resources.GetString("cmbType.Items7"),
            resources.GetString("cmbType.Items8"),
            resources.GetString("cmbType.Items9"),
            resources.GetString("cmbType.Items10"),
            resources.GetString("cmbType.Items11"),
            resources.GetString("cmbType.Items12"),
            resources.GetString("cmbType.Items13"),
            resources.GetString("cmbType.Items14"),
            resources.GetString("cmbType.Items15"),
            resources.GetString("cmbType.Items16")});
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // tbCharacter
            // 
            resources.ApplyResources(this.tbCharacter, "tbCharacter");
            this.tbCharacter.Name = "tbCharacter";
            this.tbCharacter.ReadOnly = true;
            this.tbCharacter.TabStop = false;
            this.tbCharacter.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbLength
            // 
            this.ntbLength.AllowDecimal = false;
            this.ntbLength.AllowNegative = false;
            this.ntbLength.CultureInfo = null;
            this.ntbLength.DecimalLetters = 2;
            this.ntbLength.HasMinValue = false;
            resources.ApplyResources(this.ntbLength, "ntbLength");
            this.ntbLength.MaxValue = 22D;
            this.ntbLength.MinValue = 0D;
            this.ntbLength.Name = "ntbLength";
            this.ntbLength.Value = 0D;
            this.ntbLength.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // ntbDecimals
            // 
            this.ntbDecimals.AllowDecimal = false;
            this.ntbDecimals.AllowNegative = false;
            this.ntbDecimals.CultureInfo = null;
            this.ntbDecimals.DecimalLetters = 2;
            this.ntbDecimals.HasMinValue = false;
            resources.ApplyResources(this.ntbDecimals, "ntbDecimals");
            this.ntbDecimals.MaxValue = 100D;
            this.ntbDecimals.MinValue = 0D;
            this.ntbDecimals.Name = "ntbDecimals";
            this.ntbDecimals.Value = 0D;
            this.ntbDecimals.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // BarCodeMaskSegmentDialog
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ntbDecimals);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ntbLength);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbCharacter);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbSegmentNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "BarCodeMaskSegmentDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.tbSegmentNum, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbType, 0);
            this.Controls.SetChildIndex(this.tbCharacter, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.ntbLength, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.ntbDecimals, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbSegmentNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label4;
        private NumericTextBox ntbLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCharacter;
        private System.Windows.Forms.Label label5;
        private NumericTextBox ntbDecimals;
    }
}