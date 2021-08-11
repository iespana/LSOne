using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class AggregateGroupRelationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AggregateGroupRelationDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbGroup = new LSOne.Controls.DualDataComboBox();
            this.btnAddGroup = new LSOne.Controls.ContextButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
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
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbGroup
            // 
            this.cmbGroup.AddList = null;
            this.cmbGroup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbGroup, "cmbGroup");
            this.cmbGroup.MaxLength = 32767;
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.NoChangeAllowed = false;
            this.cmbGroup.OnlyDisplayID = false;
            this.cmbGroup.RemoveList = null;
            this.cmbGroup.RowHeight = ((short)(22));
            this.cmbGroup.SecondaryData = null;
            this.cmbGroup.SelectedData = null;
            this.cmbGroup.SelectedDataID = null;
            this.cmbGroup.SelectionList = null;
            this.cmbGroup.SkipIDColumn = true;
            this.cmbGroup.RequestData += new System.EventHandler(this.cmbRelation_RequestData);
            this.cmbGroup.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnAddGroup.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddGroup, "btnAddGroup");
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // AggregateGroupRelationDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnAddGroup);
            this.Controls.Add(this.cmbGroup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "AggregateGroupRelationDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbGroup, 0);
            this.Controls.SetChildIndex(this.btnAddGroup, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbGroup;
        private ContextButton btnAddGroup;
    }
}