namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class ReasonCodeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReasonCodeDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkCreateAnother = new LSOne.Controls.DoubleBufferedCheckbox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblAction = new System.Windows.Forms.Label();
            this.dtBegin = new System.Windows.Forms.DateTimePicker();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.lblBegin = new System.Windows.Forms.Label();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblPos = new System.Windows.Forms.Label();
            this.chkPos = new LSOne.Controls.DoubleBufferedCheckbox();
            this.cmbAction = new System.Windows.Forms.ComboBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.chkCreateAnother);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // chkCreateAnother
            // 
            resources.ApplyResources(this.chkCreateAnother, "chkCreateAnother");
            this.chkCreateAnother.Name = "chkCreateAnother";
            this.chkCreateAnother.UseVisualStyleBackColor = true;
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
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.Control_DataChanged);
            // 
            // lblAction
            // 
            this.lblAction.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblAction, "lblAction");
            this.lblAction.Name = "lblAction";
            // 
            // dtBegin
            // 
            this.dtBegin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            resources.ApplyResources(this.dtBegin, "dtBegin");
            this.dtBegin.Name = "dtBegin";
            this.dtBegin.ValueChanged += new System.EventHandler(this.Control_DataChanged);
            // 
            // dtEnd
            // 
            this.dtEnd.Checked = false;
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            resources.ApplyResources(this.dtEnd, "dtEnd");
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.ShowCheckBox = true;
            this.dtEnd.ValueChanged += new System.EventHandler(this.Control_DataChanged);
            // 
            // lblBegin
            // 
            this.lblBegin.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblBegin, "lblBegin");
            this.lblBegin.Name = "lblBegin";
            // 
            // lblEnd
            // 
            this.lblEnd.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblEnd, "lblEnd");
            this.lblEnd.Name = "lblEnd";
            // 
            // lblPos
            // 
            this.lblPos.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPos, "lblPos");
            this.lblPos.Name = "lblPos";
            // 
            // chkPos
            // 
            resources.ApplyResources(this.chkPos, "chkPos");
            this.chkPos.Checked = true;
            this.chkPos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPos.Name = "chkPos";
            this.chkPos.UseVisualStyleBackColor = true;
            this.chkPos.CheckedChanged += new System.EventHandler(this.Control_DataChanged);
            // 
            // cmbAction
            // 
            this.cmbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAction.FormattingEnabled = true;
            resources.ApplyResources(this.cmbAction, "cmbAction");
            this.cmbAction.Name = "cmbAction";
            this.cmbAction.SelectedIndexChanged += new System.EventHandler(this.Control_DataChanged);
            // 
            // ReasonCodeDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbAction);
            this.Controls.Add(this.chkPos);
            this.Controls.Add(this.lblPos);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.lblBegin);
            this.Controls.Add(this.dtEnd);
            this.Controls.Add(this.dtBegin);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "ReasonCodeDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblAction, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.lblDescription, 0);
            this.Controls.SetChildIndex(this.dtBegin, 0);
            this.Controls.SetChildIndex(this.dtEnd, 0);
            this.Controls.SetChildIndex(this.lblBegin, 0);
            this.Controls.SetChildIndex(this.lblEnd, 0);
            this.Controls.SetChildIndex(this.lblPos, 0);
            this.Controls.SetChildIndex(this.chkPos, 0);
            this.Controls.SetChildIndex(this.cmbAction, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.DoubleBufferedCheckbox chkCreateAnother;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.DateTimePicker dtBegin;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.Label lblBegin;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label lblPos;
        private Controls.DoubleBufferedCheckbox chkPos;
        private System.Windows.Forms.ComboBox cmbAction;
    }
}