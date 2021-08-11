namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class LinkedFilterDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LinkedFilterDialog));
            this.cmbFromField = new System.Windows.Forms.ComboBox();
            this.lbFrom = new System.Windows.Forms.Label();
            this.lbTo = new System.Windows.Forms.Label();
            this.cmbToField = new System.Windows.Forms.ComboBox();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbLinkType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbFromTable = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbToTable = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbFromField
            // 
            this.cmbFromField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromField.FormattingEnabled = true;
            resources.ApplyResources(this.cmbFromField, "cmbFromField");
            this.cmbFromField.Name = "cmbFromField";
            this.cmbFromField.SelectedIndexChanged += new System.EventHandler(this.cmbFromField_SelectedIndexChanged);
            // 
            // lbFrom
            // 
            this.lbFrom.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbFrom, "lbFrom");
            this.lbFrom.Name = "lbFrom";
            // 
            // lbTo
            // 
            this.lbTo.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbTo, "lbTo");
            this.lbTo.Name = "lbTo";
            // 
            // cmbToField
            // 
            this.cmbToField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToField.FormattingEnabled = true;
            resources.ApplyResources(this.cmbToField, "cmbToField");
            this.cmbToField.Name = "cmbToField";
            this.cmbToField.SelectedIndexChanged += new System.EventHandler(this.cmbToField_SelectedIndexChanged);
            // 
            // tbFilter
            // 
            resources.ApplyResources(this.tbFilter, "tbFilter");
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.TextChanged += new System.EventHandler(this.tbFilter_TextChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbLinkType
            // 
            this.cmbLinkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLinkType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLinkType, "cmbLinkType");
            this.cmbLinkType.Name = "cmbLinkType";
            this.cmbLinkType.SelectedIndexChanged += new System.EventHandler(this.cmbLinkType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
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
            // tbFromTable
            // 
            resources.ApplyResources(this.tbFromTable, "tbFromTable");
            this.tbFromTable.Name = "tbFromTable";
            this.tbFromTable.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbToTable
            // 
            resources.ApplyResources(this.tbToTable, "tbToTable");
            this.tbToTable.Name = "tbToTable";
            this.tbToTable.ReadOnly = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbFromTable);
            this.groupBox1.Controls.Add(this.lbFrom);
            this.groupBox1.Controls.Add(this.cmbFromField);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbToTable);
            this.groupBox2.Controls.Add(this.lbTo);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cmbToField);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // LinkedFilterDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tbFilter);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbLinkType);
            this.Controls.Add(this.label2);
            this.HasHelp = true;
            this.Name = "LinkedFilterDialog";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbLinkType, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.tbFilter, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbFromField;
        private System.Windows.Forms.Label lbFrom;
        private System.Windows.Forms.Label lbTo;
        private System.Windows.Forms.ComboBox cmbToField;
        private System.Windows.Forms.TextBox tbFilter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbLinkType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbFromTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbToTable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}