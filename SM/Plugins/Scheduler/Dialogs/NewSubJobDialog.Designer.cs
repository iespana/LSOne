namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class NewSubJobDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewSubJobDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbRepliactionMethod = new System.Windows.Forms.ComboBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbCopyFrom = new System.Windows.Forms.ComboBox();
            this.lblActionTable = new System.Windows.Forms.Label();
            this.cmbActionTable = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbRepliactionMethod
            // 
            this.cmbRepliactionMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRepliactionMethod.FormattingEnabled = true;
            resources.ApplyResources(this.cmbRepliactionMethod, "cmbRepliactionMethod");
            this.cmbRepliactionMethod.Name = "cmbRepliactionMethod";
            this.cmbRepliactionMethod.SelectedIndexChanged += new System.EventHandler(this.cmbRepliactionMethod_SelectedIndexChanged);
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmbCopyFrom
            // 
            this.cmbCopyFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCopyFrom.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCopyFrom, "cmbCopyFrom");
            this.cmbCopyFrom.Name = "cmbCopyFrom";
            // 
            // lblActionTable
            // 
            this.lblActionTable.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblActionTable, "lblActionTable");
            this.lblActionTable.Name = "lblActionTable";
            // 
            // cmbActionTable
            // 
            this.cmbActionTable.AddList = null;
            resources.ApplyResources(this.cmbActionTable, "cmbActionTable");
            this.cmbActionTable.MaxLength = 32767;
            this.cmbActionTable.Name = "cmbActionTable";
            this.cmbActionTable.RemoveList = null;
            this.cmbActionTable.RowHeight = ((short)(22));
            this.cmbActionTable.SecondaryData = null;
            this.cmbActionTable.SelectedData = null;
            this.cmbActionTable.SelectionList = null;
            this.cmbActionTable.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbActionTable_DropDown);
            this.cmbActionTable.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbActionTable_FormatData);
            this.cmbActionTable.SelectedDataChanged += new System.EventHandler(this.cmbActionTable_SelectedDataChanged);
            // 
            // NewSubJobDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblActionTable);
            this.Controls.Add(this.cmbActionTable);
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbRepliactionMethod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.HasHelp = true;
            this.Name = "NewSubJobDialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewSubJobDialog_FormClosed);
            this.Shown += new System.EventHandler(this.NewSubJobDialog_Shown);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbRepliactionMethod, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.cmbCopyFrom, 0);
            this.Controls.SetChildIndex(this.cmbActionTable, 0);
            this.Controls.SetChildIndex(this.lblActionTable, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbRepliactionMethod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbCopyFrom;
        private System.Windows.Forms.Label lblActionTable;
        private LSOne.Controls.DropDownFormComboBox cmbActionTable;
    }
}