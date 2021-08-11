using LSOne.Controls;

namespace LSOne.ViewPlugins.Administration.Dialogs
{
    partial class EditNumberSequenceDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditNumberSequenceDialog));
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbID = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.ntbHighest = new LSOne.Controls.NumericTextBox();
			this.tbFormat = new System.Windows.Forms.TextBox();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.label5 = new System.Windows.Forms.Label();
			this.chkEmbedStoreID = new System.Windows.Forms.CheckBox();
			this.ntbNextValue = new LSOne.Controls.NumericTextBox();
			this.lblCurrent = new System.Windows.Forms.Label();
			this.btnEnableNextValue = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.chkEmbedTerminalID = new System.Windows.Forms.CheckBox();
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
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
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
			this.tbDescription.TextChanged += new System.EventHandler(this.CheckChanged);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// tbID
			// 
			this.tbID.AcceptsTab = true;
			resources.ApplyResources(this.tbID, "tbID");
			this.tbID.Name = "tbID";
			this.tbID.ReadOnly = true;
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// ntbHighest
			// 
			this.ntbHighest.AllowDecimal = false;
			this.ntbHighest.AllowNegative = false;
			this.ntbHighest.CultureInfo = null;
			this.ntbHighest.DecimalLetters = 2;
			this.ntbHighest.ForeColor = System.Drawing.Color.Black;
			this.ntbHighest.HasMinValue = false;
			resources.ApplyResources(this.ntbHighest, "ntbHighest");
			this.ntbHighest.MaxValue = 4000000000D;
			this.ntbHighest.MinValue = 0D;
			this.ntbHighest.Name = "ntbHighest";
			this.ntbHighest.Value = 0D;
			this.ntbHighest.TextChanged += new System.EventHandler(this.CheckChanged);
			// 
			// tbFormat
			// 
			resources.ApplyResources(this.tbFormat, "tbFormat");
			this.tbFormat.Name = "tbFormat";
			this.tbFormat.TextChanged += new System.EventHandler(this.CheckChanged);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// chkEmbedStoreID
			// 
			resources.ApplyResources(this.chkEmbedStoreID, "chkEmbedStoreID");
			this.chkEmbedStoreID.Checked = true;
			this.chkEmbedStoreID.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEmbedStoreID.Name = "chkEmbedStoreID";
			this.chkEmbedStoreID.UseVisualStyleBackColor = true;
			this.chkEmbedStoreID.CheckedChanged += new System.EventHandler(this.CheckChanged);
			// 
			// ntbNextValue
			// 
			this.ntbNextValue.AllowDecimal = false;
			this.ntbNextValue.AllowNegative = false;
			this.ntbNextValue.CultureInfo = null;
			this.ntbNextValue.DecimalLetters = 2;
			resources.ApplyResources(this.ntbNextValue, "ntbNextValue");
			this.ntbNextValue.ForeColor = System.Drawing.Color.Black;
			this.ntbNextValue.HasMinValue = false;
			this.ntbNextValue.MaxValue = 4000000000D;
			this.ntbNextValue.MinValue = 0D;
			this.ntbNextValue.Name = "ntbNextValue";
			this.ntbNextValue.Value = 0D;
			this.ntbNextValue.TextChanged += new System.EventHandler(this.CheckChanged);
			// 
			// lblCurrent
			// 
			resources.ApplyResources(this.lblCurrent, "lblCurrent");
			this.lblCurrent.Name = "lblCurrent";
			// 
			// btnEnableNextValue
			// 
			resources.ApplyResources(this.btnEnableNextValue, "btnEnableNextValue");
			this.btnEnableNextValue.Name = "btnEnableNextValue";
			this.btnEnableNextValue.UseVisualStyleBackColor = true;
			this.btnEnableNextValue.Click += new System.EventHandler(this.btnEnableNextValue_Click);
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// chkEmbedTerminalID
			// 
			resources.ApplyResources(this.chkEmbedTerminalID, "chkEmbedTerminalID");
			this.chkEmbedTerminalID.Checked = true;
			this.chkEmbedTerminalID.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEmbedTerminalID.Name = "chkEmbedTerminalID";
			this.chkEmbedTerminalID.UseVisualStyleBackColor = true;
			this.chkEmbedTerminalID.CheckedChanged += new System.EventHandler(this.CheckChanged);
			// 
			// EditNumberSequenceDialog
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.CancelButton = this.btnCancel;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.label7);
			this.Controls.Add(this.chkEmbedTerminalID);
			this.Controls.Add(this.btnEnableNextValue);
			this.Controls.Add(this.ntbNextValue);
			this.Controls.Add(this.lblCurrent);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.chkEmbedStoreID);
			this.Controls.Add(this.tbFormat);
			this.Controls.Add(this.ntbHighest);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbDescription);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbID);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.panel2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HasHelp = true;
			this.Name = "EditNumberSequenceDialog";
			this.Controls.SetChildIndex(this.panel2, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.tbID, 0);
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.tbDescription, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.label4, 0);
			this.Controls.SetChildIndex(this.ntbHighest, 0);
			this.Controls.SetChildIndex(this.tbFormat, 0);
			this.Controls.SetChildIndex(this.chkEmbedStoreID, 0);
			this.Controls.SetChildIndex(this.label5, 0);
			this.Controls.SetChildIndex(this.lblCurrent, 0);
			this.Controls.SetChildIndex(this.ntbNextValue, 0);
			this.Controls.SetChildIndex(this.btnEnableNextValue, 0);
			this.Controls.SetChildIndex(this.chkEmbedTerminalID, 0);
			this.Controls.SetChildIndex(this.label7, 0);
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
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private NumericTextBox ntbHighest;
        private System.Windows.Forms.TextBox tbFormat;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkEmbedStoreID;
        private System.Windows.Forms.Button btnEnableNextValue;
        private NumericTextBox ntbNextValue;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkEmbedTerminalID;
    }
}