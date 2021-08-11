using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    partial class PosLookupDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PosLookupDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDynamicMenuID = new DualDataComboBox();
            this.cmbDynamicMenu2ID = new DualDataComboBox();
            this.cmbGrid1MenuID = new DualDataComboBox();
            this.cmbGrid2MenuID = new DualDataComboBox();
            this.btnAddDynamicMenu = new ContextButton();
            this.btnAddDynamicMenu2 = new ContextButton();
            this.btnAddGrid1Menu = new ContextButton();
            this.btnAddGrid2Menu = new ContextButton();
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
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
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbDynamicMenuID
            // 
            resources.ApplyResources(this.cmbDynamicMenuID, "cmbDynamicMenuID");
            this.cmbDynamicMenuID.MaxLength = 32767;
            this.cmbDynamicMenuID.Name = "cmbDynamicMenuID";
            this.cmbDynamicMenuID.SelectedData = null;
            this.cmbDynamicMenuID.SkipIDColumn = true;
            this.cmbDynamicMenuID.RequestData += new System.EventHandler(this.cmbDynamicMenuID_RequestData);
            this.cmbDynamicMenuID.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbDynamicMenu2ID
            // 
            resources.ApplyResources(this.cmbDynamicMenu2ID, "cmbDynamicMenu2ID");
            this.cmbDynamicMenu2ID.MaxLength = 32767;
            this.cmbDynamicMenu2ID.Name = "cmbDynamicMenu2ID";
            this.cmbDynamicMenu2ID.SelectedData = null;
            this.cmbDynamicMenu2ID.SkipIDColumn = false;
            this.cmbDynamicMenu2ID.RequestData += new System.EventHandler(this.cmbDynamicMenu2ID_RequestData);
            this.cmbDynamicMenu2ID.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbGrid1MenuID
            // 
            resources.ApplyResources(this.cmbGrid1MenuID, "cmbGrid1MenuID");
            this.cmbGrid1MenuID.MaxLength = 32767;
            this.cmbGrid1MenuID.Name = "cmbGrid1MenuID";
            this.cmbGrid1MenuID.SelectedData = null;
            this.cmbGrid1MenuID.SkipIDColumn = true;
            this.cmbGrid1MenuID.RequestData += new System.EventHandler(this.cmbGrid1MenuID_RequestData);
            this.cmbGrid1MenuID.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbGrid2MenuID
            // 
            resources.ApplyResources(this.cmbGrid2MenuID, "cmbGrid2MenuID");
            this.cmbGrid2MenuID.MaxLength = 32767;
            this.cmbGrid2MenuID.Name = "cmbGrid2MenuID";
            this.cmbGrid2MenuID.SelectedData = null;
            this.cmbGrid2MenuID.SkipIDColumn = true;
            this.cmbGrid2MenuID.RequestData += new System.EventHandler(this.cmbGrid2MenuID_RequestData);
            this.cmbGrid2MenuID.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // btnAddDynamicMenu
            // 
            this.btnAddDynamicMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnAddDynamicMenu.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddDynamicMenu, "btnAddDynamicMenu");
            this.btnAddDynamicMenu.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnAddDynamicMenu.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnAddDynamicMenu.Name = "btnAddDynamicMenu";
            this.btnAddDynamicMenu.Click += new System.EventHandler(this.addMenuHandler);
            // 
            // btnAddDynamicMenu2
            // 
            this.btnAddDynamicMenu2.BackColor = System.Drawing.Color.Transparent;
            this.btnAddDynamicMenu2.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddDynamicMenu2, "btnAddDynamicMenu2");
            this.btnAddDynamicMenu2.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnAddDynamicMenu2.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnAddDynamicMenu2.Name = "btnAddDynamicMenu2";
            this.btnAddDynamicMenu2.Click += new System.EventHandler(this.addMenuHandler);
            // 
            // btnAddGrid1Menu
            // 
            this.btnAddGrid1Menu.BackColor = System.Drawing.Color.Transparent;
            this.btnAddGrid1Menu.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddGrid1Menu, "btnAddGrid1Menu");
            this.btnAddGrid1Menu.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnAddGrid1Menu.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnAddGrid1Menu.Name = "btnAddGrid1Menu";
            this.btnAddGrid1Menu.Click += new System.EventHandler(this.addMenuHandler);
            // 
            // btnAddGrid2Menu
            // 
            this.btnAddGrid2Menu.BackColor = System.Drawing.Color.Transparent;
            this.btnAddGrid2Menu.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddGrid2Menu, "btnAddGrid2Menu");
            this.btnAddGrid2Menu.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnAddGrid2Menu.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnAddGrid2Menu.Name = "btnAddGrid2Menu";
            this.btnAddGrid2Menu.Click += new System.EventHandler(this.addMenuHandler);
            // 
            // PosLookupDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnAddGrid2Menu);
            this.Controls.Add(this.btnAddGrid1Menu);
            this.Controls.Add(this.btnAddDynamicMenu2);
            this.Controls.Add(this.btnAddDynamicMenu);
            this.Controls.Add(this.cmbGrid2MenuID);
            this.Controls.Add(this.cmbGrid1MenuID);
            this.Controls.Add(this.cmbDynamicMenu2ID);
            this.Controls.Add(this.cmbDynamicMenuID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "PosLookupDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cmbDynamicMenuID, 0);
            this.Controls.SetChildIndex(this.cmbDynamicMenu2ID, 0);
            this.Controls.SetChildIndex(this.cmbGrid1MenuID, 0);
            this.Controls.SetChildIndex(this.cmbGrid2MenuID, 0);
            this.Controls.SetChildIndex(this.btnAddDynamicMenu, 0);
            this.Controls.SetChildIndex(this.btnAddDynamicMenu2, 0);
            this.Controls.SetChildIndex(this.btnAddGrid1Menu, 0);
            this.Controls.SetChildIndex(this.btnAddGrid2Menu, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbGrid2MenuID;
        private DualDataComboBox cmbGrid1MenuID;
        private DualDataComboBox cmbDynamicMenu2ID;
        private DualDataComboBox cmbDynamicMenuID;
        private ContextButton btnAddGrid2Menu;
        private ContextButton btnAddGrid1Menu;
        private ContextButton btnAddDynamicMenu2;
        private ContextButton btnAddDynamicMenu;
    }
}