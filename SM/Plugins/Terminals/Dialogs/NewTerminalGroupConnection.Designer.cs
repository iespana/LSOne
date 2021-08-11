using LSOne.Controls;

namespace LSOne.ViewPlugins.Terminals.Dialogs
{
    partial class NewTerminalGroupConnection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTerminalGroupConnection));
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTerminalGroup = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbTerminal = new LSOne.Controls.DualDataComboBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtTerminalGroup
            // 
            resources.ApplyResources(this.txtTerminalGroup, "txtTerminalGroup");
            this.txtTerminalGroup.Name = "txtTerminalGroup";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOk);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbTerminal
            // 
            this.cmbTerminal.AddList = null;
            this.cmbTerminal.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTerminal, "cmbTerminal");
            this.cmbTerminal.MaxLength = 32767;
            this.cmbTerminal.Name = "cmbTerminal";
            this.cmbTerminal.NoChangeAllowed = false;
            this.cmbTerminal.OnlyDisplayID = false;
            this.cmbTerminal.RemoveList = null;
            this.cmbTerminal.RowHeight = ((short)(22));
            this.cmbTerminal.SecondaryData = null;
            this.cmbTerminal.SelectedData = null;
            this.cmbTerminal.SelectedDataID = null;
            this.cmbTerminal.SelectionList = null;
            this.cmbTerminal.SkipIDColumn = false;
            this.cmbTerminal.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbTerminal_DropDown);
            this.cmbTerminal.SelectedDataChanged += new System.EventHandler(this.cmbTerminal_SelectedDataChanged);
            // 
            // NewTerminalGroupConnection
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.txtTerminalGroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbTerminal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "NewTerminalGroupConnection";
            this.Controls.SetChildIndex(this.cmbTerminal, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtTerminalGroup, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DualDataComboBox cmbTerminal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTerminalGroup;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
    }
}