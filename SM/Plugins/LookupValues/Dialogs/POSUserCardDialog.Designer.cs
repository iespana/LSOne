﻿using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    partial class POSUserCardDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POSUserCardDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbCardNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPOSUsers = new LSOne.Controls.DualDataComboBox();
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
            // tbCardNumber
            // 
            resources.ApplyResources(this.tbCardNumber, "tbCardNumber");
            this.tbCardNumber.Name = "tbCardNumber";
            this.tbCardNumber.TextChanged += new System.EventHandler(this.CheckEnabled);
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbPOSUsers
            // 
            this.cmbPOSUsers.AddList = null;
            this.cmbPOSUsers.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPOSUsers, "cmbPOSUsers");
            this.cmbPOSUsers.MaxLength = 32767;
            this.cmbPOSUsers.Name = "cmbPOSUsers";
            this.cmbPOSUsers.NoChangeAllowed = false;
            this.cmbPOSUsers.OnlyDisplayID = false;
            this.cmbPOSUsers.RemoveList = null;
            this.cmbPOSUsers.RowHeight = ((short)(22));
            this.cmbPOSUsers.SecondaryData = null;
            this.cmbPOSUsers.SelectedData = null;
            this.cmbPOSUsers.SelectedDataID = null;
            this.cmbPOSUsers.SelectionList = null;
            this.cmbPOSUsers.SkipIDColumn = false;
            this.cmbPOSUsers.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbPOSUsers_DropDown);
            this.cmbPOSUsers.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // POSUserCardDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbPOSUsers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbCardNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "POSUserCardDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbCardNumber, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbPOSUsers, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbCardNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbPOSUsers;
    }
}