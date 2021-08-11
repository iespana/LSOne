using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.Dialogs
{
    partial class SelectPriceGroupDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectPriceGroupDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbPriceGroup = new LSOne.Controls.DualDataComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddPriceGroup = new LSOne.Controls.ContextButton();
            this.txtStore = new System.Windows.Forms.TextBox();
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbPriceGroup
            // 
            this.cmbPriceGroup.AddList = null;
            this.cmbPriceGroup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPriceGroup, "cmbPriceGroup");
            this.cmbPriceGroup.MaxLength = 32767;
            this.cmbPriceGroup.Name = "cmbPriceGroup";
            this.cmbPriceGroup.NoChangeAllowed = false;
            this.cmbPriceGroup.OnlyDisplayID = false;
            this.cmbPriceGroup.RemoveList = null;
            this.cmbPriceGroup.RowHeight = ((short)(22));
            this.cmbPriceGroup.SecondaryData = null;
            this.cmbPriceGroup.SelectedData = null;
            this.cmbPriceGroup.SelectedDataID = null;
            this.cmbPriceGroup.SelectionList = null;
            this.cmbPriceGroup.SkipIDColumn = true;
            this.cmbPriceGroup.RequestData += new System.EventHandler(this.cmbPriceGroup_RequestData);
            this.cmbPriceGroup.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbPriceGroup.RequestClear += new System.EventHandler(this.RequestClear);
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnAddPriceGroup
            // 
            this.btnAddPriceGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnAddPriceGroup.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddPriceGroup, "btnAddPriceGroup");
            this.btnAddPriceGroup.Name = "btnAddPriceGroup";
            this.btnAddPriceGroup.Click += new System.EventHandler(this.btnAddPriceGroup_Click);
            // 
            // txtStore
            // 
            resources.ApplyResources(this.txtStore, "txtStore");
            this.txtStore.Name = "txtStore";
            this.txtStore.ReadOnly = true;
            // 
            // SelectPriceGroupDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.txtStore);
            this.Controls.Add(this.btnAddPriceGroup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cmbPriceGroup);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "SelectPriceGroupDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbPriceGroup, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnAddPriceGroup, 0);
            this.Controls.SetChildIndex(this.txtStore, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label12;
        private DualDataComboBox cmbPriceGroup;
        private System.Windows.Forms.Label label1;
        private ContextButton btnAddPriceGroup;
        private System.Windows.Forms.TextBox txtStore;
    }
}