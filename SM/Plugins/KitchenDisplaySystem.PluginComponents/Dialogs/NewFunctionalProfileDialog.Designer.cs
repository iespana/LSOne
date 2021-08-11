using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class NewFunctionalProfileDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewFunctionalProfileDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbKitchenProfileName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAdd = new LSOne.Controls.ContextButton();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbButtons = new LSOne.Controls.DualDataComboBox();
            this.panel2.SuspendLayout();
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
            // tbKitchenProfileName
            // 
            resources.ApplyResources(this.tbKitchenProfileName, "tbKitchenProfileName");
            this.tbKitchenProfileName.Name = "tbKitchenProfileName";
            this.tbKitchenProfileName.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnAdd
            // 
            this.btnAdd.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cmbButtons
            // 
            this.cmbButtons.AddList = null;
            this.cmbButtons.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbButtons, "cmbButtons");
            this.cmbButtons.MaxLength = 32767;
            this.cmbButtons.Name = "cmbButtons";
            this.cmbButtons.OnlyDisplayID = false;
            this.cmbButtons.RemoveList = null;
            this.cmbButtons.RowHeight = ((short)(22));
            this.cmbButtons.SecondaryData = null;
            this.cmbButtons.SelectedData = null;
            this.cmbButtons.SelectedDataID = null;
            this.cmbButtons.SelectionList = null;
            this.cmbButtons.SkipIDColumn = true;
            this.cmbButtons.RequestData += new System.EventHandler(this.cmbButtons_RequestData);
            this.cmbButtons.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // NewFunctionalProfileDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbButtons);
            this.Controls.Add(this.tbKitchenProfileName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "NewFunctionalProfileDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbKitchenProfileName, 0);
            this.Controls.SetChildIndex(this.cmbButtons, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbKitchenProfileName;
        private System.Windows.Forms.Label label3;
        private ContextButton btnAdd;
        private System.Windows.Forms.Label label9;
        private DualDataComboBox cmbButtons;
    }
}