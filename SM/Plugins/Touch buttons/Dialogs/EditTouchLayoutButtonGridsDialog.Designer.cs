using LSOne.Controls;

namespace LSOne.ViewPlugins.TouchButtons.Dialogs
{
    partial class EditTouchLayoutButtonGridsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditTouchLayoutButtonGridsDialog));
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddButtonGrid5Menu = new ContextButton();
            this.btnAddButtonGrid4Menu = new ContextButton();
            this.btnAddButtonGrid3Menu = new ContextButton();
            this.btnAddButtonGrid2Menu = new ContextButton();
            this.btnAddButtonGrid1Menu = new ContextButton();
            this.cmbButtonGrid5Menu = new DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbButtonGrid4Menu = new DualDataComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbButtonGrid3Menu = new DualDataComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbButtonGrid2Menu = new DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbButtonGrid1Menu = new DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAddButtonGrid5Menu);
            this.groupBox1.Controls.Add(this.btnAddButtonGrid4Menu);
            this.groupBox1.Controls.Add(this.btnAddButtonGrid3Menu);
            this.groupBox1.Controls.Add(this.btnAddButtonGrid2Menu);
            this.groupBox1.Controls.Add(this.btnAddButtonGrid1Menu);
            this.groupBox1.Controls.Add(this.cmbButtonGrid5Menu);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbButtonGrid4Menu);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbButtonGrid3Menu);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbButtonGrid2Menu);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbButtonGrid1Menu);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnAddButtonGrid5Menu
            // 
            this.btnAddButtonGrid5Menu.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddButtonGrid5Menu, "btnAddButtonGrid5Menu");
            this.btnAddButtonGrid5Menu.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnAddButtonGrid5Menu.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnAddButtonGrid5Menu.Name = "btnAddButtonGrid5Menu";
            this.btnAddButtonGrid5Menu.Click += new System.EventHandler(this.btnAddButtonGridMenu_Click);
            // 
            // btnAddButtonGrid4Menu
            // 
            this.btnAddButtonGrid4Menu.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddButtonGrid4Menu, "btnAddButtonGrid4Menu");
            this.btnAddButtonGrid4Menu.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnAddButtonGrid4Menu.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnAddButtonGrid4Menu.Name = "btnAddButtonGrid4Menu";
            this.btnAddButtonGrid4Menu.Click += new System.EventHandler(this.btnAddButtonGridMenu_Click);
            // 
            // btnAddButtonGrid3Menu
            // 
            this.btnAddButtonGrid3Menu.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddButtonGrid3Menu, "btnAddButtonGrid3Menu");
            this.btnAddButtonGrid3Menu.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnAddButtonGrid3Menu.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnAddButtonGrid3Menu.Name = "btnAddButtonGrid3Menu";
            this.btnAddButtonGrid3Menu.Click += new System.EventHandler(this.btnAddButtonGridMenu_Click);
            // 
            // btnAddButtonGrid2Menu
            // 
            this.btnAddButtonGrid2Menu.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddButtonGrid2Menu, "btnAddButtonGrid2Menu");
            this.btnAddButtonGrid2Menu.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnAddButtonGrid2Menu.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnAddButtonGrid2Menu.Name = "btnAddButtonGrid2Menu";
            this.btnAddButtonGrid2Menu.Click += new System.EventHandler(this.btnAddButtonGridMenu_Click);
            // 
            // btnAddButtonGrid1Menu
            // 
            this.btnAddButtonGrid1Menu.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddButtonGrid1Menu, "btnAddButtonGrid1Menu");
            this.btnAddButtonGrid1Menu.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnAddButtonGrid1Menu.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnAddButtonGrid1Menu.Name = "btnAddButtonGrid1Menu";
            this.btnAddButtonGrid1Menu.Click += new System.EventHandler(this.btnAddButtonGridMenu_Click);
            // 
            // cmbButtonGrid5Menu
            // 
            resources.ApplyResources(this.cmbButtonGrid5Menu, "cmbButtonGrid5Menu");
            this.cmbButtonGrid5Menu.MaxLength = 32767;
            this.cmbButtonGrid5Menu.Name = "cmbButtonGrid5Menu";
            this.cmbButtonGrid5Menu.SelectedData = null;
            this.cmbButtonGrid5Menu.SkipIDColumn = true;
            this.cmbButtonGrid5Menu.RequestData += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            this.cmbButtonGrid5Menu.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbButtonGrid5Menu.RequestClear += new System.EventHandler(this.cmbButtonGridMenu_RequestClear);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbButtonGrid4Menu
            // 
            resources.ApplyResources(this.cmbButtonGrid4Menu, "cmbButtonGrid4Menu");
            this.cmbButtonGrid4Menu.MaxLength = 32767;
            this.cmbButtonGrid4Menu.Name = "cmbButtonGrid4Menu";
            this.cmbButtonGrid4Menu.SelectedData = null;
            this.cmbButtonGrid4Menu.SkipIDColumn = true;
            this.cmbButtonGrid4Menu.RequestData += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            this.cmbButtonGrid4Menu.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbButtonGrid4Menu.RequestClear += new System.EventHandler(this.cmbButtonGridMenu_RequestClear);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmbButtonGrid3Menu
            // 
            resources.ApplyResources(this.cmbButtonGrid3Menu, "cmbButtonGrid3Menu");
            this.cmbButtonGrid3Menu.MaxLength = 32767;
            this.cmbButtonGrid3Menu.Name = "cmbButtonGrid3Menu";
            this.cmbButtonGrid3Menu.SelectedData = null;
            this.cmbButtonGrid3Menu.SkipIDColumn = true;
            this.cmbButtonGrid3Menu.RequestData += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            this.cmbButtonGrid3Menu.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbButtonGrid3Menu.RequestClear += new System.EventHandler(this.cmbButtonGridMenu_RequestClear);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbButtonGrid2Menu
            // 
            resources.ApplyResources(this.cmbButtonGrid2Menu, "cmbButtonGrid2Menu");
            this.cmbButtonGrid2Menu.MaxLength = 32767;
            this.cmbButtonGrid2Menu.Name = "cmbButtonGrid2Menu";
            this.cmbButtonGrid2Menu.SelectedData = null;
            this.cmbButtonGrid2Menu.SkipIDColumn = true;
            this.cmbButtonGrid2Menu.RequestData += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            this.cmbButtonGrid2Menu.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbButtonGrid2Menu.RequestClear += new System.EventHandler(this.cmbButtonGridMenu_RequestClear);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbButtonGrid1Menu
            // 
            resources.ApplyResources(this.cmbButtonGrid1Menu, "cmbButtonGrid1Menu");
            this.cmbButtonGrid1Menu.MaxLength = 32767;
            this.cmbButtonGrid1Menu.Name = "cmbButtonGrid1Menu";
            this.cmbButtonGrid1Menu.SelectedData = null;
            this.cmbButtonGrid1Menu.SkipIDColumn = true;
            this.cmbButtonGrid1Menu.RequestData += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            this.cmbButtonGrid1Menu.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbButtonGrid1Menu.RequestClear += new System.EventHandler(this.cmbButtonGridMenu_RequestClear);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbID
            // 
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            this.tbID.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // EditTouchLayoutButtonGridsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "EditTouchLayoutButtonGridsDialog";
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.tbID, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DualDataComboBox cmbButtonGrid5Menu;
        private System.Windows.Forms.Label label7;
        private DualDataComboBox cmbButtonGrid4Menu;
        private System.Windows.Forms.Label label6;
        private DualDataComboBox cmbButtonGrid3Menu;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbButtonGrid2Menu;
        private System.Windows.Forms.Label label2;
        private DualDataComboBox cmbButtonGrid1Menu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label4;
        private ContextButton btnAddButtonGrid5Menu;
        private ContextButton btnAddButtonGrid4Menu;
        private ContextButton btnAddButtonGrid3Menu;
        private ContextButton btnAddButtonGrid2Menu;
        private ContextButton btnAddButtonGrid1Menu;
    }
}