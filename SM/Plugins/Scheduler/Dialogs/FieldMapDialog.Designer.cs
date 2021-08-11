namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class FieldMapDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FieldMapDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbFromField = new System.Windows.Forms.ComboBox();
            this.tbConversionValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbConversionMethod = new System.Windows.Forms.ComboBox();
            this.cmbToField = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
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
            // 
            // cmbFromField
            // 
            this.cmbFromField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromField.FormattingEnabled = true;
            resources.ApplyResources(this.cmbFromField, "cmbFromField");
            this.cmbFromField.Name = "cmbFromField";
            this.cmbFromField.SelectedIndexChanged += new System.EventHandler(this.cmbFromField_SelectedIndexChanged);
            // 
            // tbConversionValue
            // 
            resources.ApplyResources(this.tbConversionValue, "tbConversionValue");
            this.tbConversionValue.Name = "tbConversionValue";
            this.tbConversionValue.TextChanged += new System.EventHandler(this.tbConversionValue_TextChanged);
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
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbConversionMethod
            // 
            this.cmbConversionMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConversionMethod.FormattingEnabled = true;
            resources.ApplyResources(this.cmbConversionMethod, "cmbConversionMethod");
            this.cmbConversionMethod.Name = "cmbConversionMethod";
            this.cmbConversionMethod.SelectedIndexChanged += new System.EventHandler(this.cmbConversionMethod_SelectedIndexChanged);
            // 
            // cmbToField
            // 
            this.cmbToField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToField.FormattingEnabled = true;
            resources.ApplyResources(this.cmbToField, "cmbToField");
            this.cmbToField.Name = "cmbToField";
            this.cmbToField.SelectedIndexChanged += new System.EventHandler(this.cmbToField_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // FieldMapDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbFromField);
            this.Controls.Add(this.tbConversionValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbConversionMethod);
            this.Controls.Add(this.cmbToField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "FieldMapDialog";
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbFromField;
        private System.Windows.Forms.TextBox tbConversionValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbConversionMethod;
        private System.Windows.Forms.ComboBox cmbToField;
        private System.Windows.Forms.Label label2;
    }
}