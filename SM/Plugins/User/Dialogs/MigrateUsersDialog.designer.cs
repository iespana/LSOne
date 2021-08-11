using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.User.Dialogs
{
    partial class MigrateUsersDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MigrateUsersDialog));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblLoginName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnMigrate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupPanel1 = new GroupPanel();
            this.listView1 = new ListView();
            this.User = new Column();
            this.Grop = new Column();
            this.POSusers = new Column();
            this.groupPanel2 = new GroupPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbNonManagers = new DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbManagers = new DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listView2 = new ListView();
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblLoginName
            // 
            this.lblLoginName.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblLoginName, "lblLoginName");
            this.lblLoginName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.errorProvider1.SetIconAlignment(this.lblLoginName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblLoginName.IconAlignment"))));
            this.lblLoginName.Name = "lblLoginName";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.errorProvider1.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.label1.Name = "label1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnMigrate);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnMigrate
            // 
            resources.ApplyResources(this.btnMigrate, "btnMigrate");
            this.btnMigrate.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnMigrate.Name = "btnMigrate";
            this.btnMigrate.UseVisualStyleBackColor = true;
            this.btnMigrate.Click += new System.EventHandler(this.btnMigrate_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.listView1);
            this.groupPanel1.Controls.Add(this.lblLoginName);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // listView1
            // 
            this.listView1.BuddyControl = null;
            this.listView1.Columns.Add(this.User);
            this.listView1.Columns.Add(this.Grop);
            this.listView1.ContentBackColor = System.Drawing.Color.White;
            this.listView1.DefaultRowHeight = ((short)(22));
            this.listView1.DimSelectionWhenDisabled = true;
            this.listView1.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.listView1.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.HeaderHeight = ((short)(25));
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.Name = "listView1";
            this.listView1.OddRowColor = System.Drawing.Color.White;
            this.listView1.RowLineColor = System.Drawing.Color.LightGray;
            this.listView1.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.listView1.CellAction += new CellActionDelegate(this.listView1_CellAction);
            this.listView1.CellDropDown += new CellDropDownDelegate(this.listView1_CellDropDown);
            // 
            // User
            // 
            this.User.AutoSize = true;
            this.User.Clickable = false;
            this.User.DefaultStyle = null;
            resources.ApplyResources(this.User, "User");
            this.User.MaximumWidth = ((short)(0));
            this.User.MinimumWidth = ((short)(10));
            this.User.Sizable = false;
            this.User.Tag = null;
            this.User.Width = ((short)(234));
            // 
            // Grop
            // 
            this.Grop.AutoSize = true;
            this.Grop.Clickable = false;
            this.Grop.DefaultStyle = null;
            resources.ApplyResources(this.Grop, "Grop");
            this.Grop.MaximumWidth = ((short)(0));
            this.Grop.MinimumWidth = ((short)(10));
            this.Grop.Sizable = false;
            this.Grop.Tag = null;
            this.Grop.Width = ((short)(234));
            // 
            // POSusers
            // 
            this.POSusers.AutoSize = true;
            this.POSusers.Clickable = false;
            this.POSusers.DefaultStyle = null;
            resources.ApplyResources(this.POSusers, "POSusers");
            this.POSusers.MaximumWidth = ((short)(0));
            this.POSusers.MinimumWidth = ((short)(50));
            this.POSusers.RelativeSize = 50;
            this.POSusers.Sizable = false;
            this.POSusers.Tag = null;
            this.POSusers.Width = ((short)(468));
            // 
            // groupPanel2
            // 
            this.groupPanel2.Controls.Add(this.groupBox1);
            this.groupPanel2.Controls.Add(this.listView2);
            this.groupPanel2.Controls.Add(this.label1);
            resources.ApplyResources(this.groupPanel2, "groupPanel2");
            this.groupPanel2.Name = "groupPanel2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbNonManagers);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbManagers);
            this.groupBox1.Controls.Add(this.label2);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmbNonManagers
            // 
            this.cmbNonManagers.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbNonManagers, "cmbNonManagers");
            this.cmbNonManagers.MaxLength = 32767;
            this.cmbNonManagers.Name = "cmbNonManagers";
            this.cmbNonManagers.RowHeight = ((short)(22));
            this.cmbNonManagers.SelectedData = null;
            this.cmbNonManagers.SkipIDColumn = true;
            this.cmbNonManagers.RequestData += new System.EventHandler(this.cmbNonManagers_RequestData);
            this.cmbNonManagers.SelectedDataChanged += new System.EventHandler(this.cmbNonManagers_SelectedDataChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbManagers
            // 
            this.cmbManagers.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbManagers, "cmbManagers");
            this.cmbManagers.MaxLength = 32767;
            this.cmbManagers.Name = "cmbManagers";
            this.cmbManagers.RowHeight = ((short)(22));
            this.cmbManagers.SelectedData = null;
            this.cmbManagers.SkipIDColumn = true;
            this.cmbManagers.RequestData += new System.EventHandler(this.cmbManagers_RequestData);
            this.cmbManagers.SelectedDataChanged += new System.EventHandler(this.cmbManagers_SelectedDataChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // listView2
            // 
            this.listView2.BuddyControl = null;
            this.listView2.Columns.Add(this.POSusers);
            this.listView2.ContentBackColor = System.Drawing.Color.White;
            this.listView2.DefaultRowHeight = ((short)(22));
            this.listView2.DimSelectionWhenDisabled = true;
            this.listView2.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.listView2.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView2.HeaderHeight = ((short)(25));
            resources.ApplyResources(this.listView2, "listView2");
            this.listView2.Name = "listView2";
            this.listView2.OddRowColor = System.Drawing.Color.White;
            this.listView2.RowLineColor = System.Drawing.Color.LightGray;
            this.listView2.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.listView2.CellAction += new CellActionDelegate(this.listView2_CellAction);
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // MigrateUsersDialog
            // 
            this.AcceptButton = this.btnMigrate;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "MigrateUsersDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.groupPanel1, 0);
            this.Controls.SetChildIndex(this.groupPanel2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private GroupPanel groupPanel1;
        private GroupPanel groupPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private ListView listView2;
        private Column POSusers;
        private System.Windows.Forms.Label label1;
        private ListView listView1;
        private System.Windows.Forms.Label lblLoginName;
        private DualDataComboBox cmbNonManagers;
        private System.Windows.Forms.Label label3;
        private DualDataComboBox cmbManagers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMigrate;
        private Column User;
        private Column Grop;
        private System.Windows.Forms.ErrorProvider errorProvider2;
    }
}