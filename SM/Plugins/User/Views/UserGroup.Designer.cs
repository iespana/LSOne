using LSOne.Controls;

namespace LSOne.ViewPlugins.User.Views
{
    partial class GroupPermissions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupPermissions));
            this.lblGroup = new System.Windows.Forms.Label();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.lvPermissions = new LSOne.Controls.ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblPermissions = new System.Windows.Forms.Label();
            this.lnkNew = new System.Windows.Forms.LinkLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lnkEdit = new System.Windows.Forms.LinkLabel();
            this.lnkDelete = new System.Windows.Forms.LinkLabel();
            this.lvUsers = new LSOne.Controls.ExtendedListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.btnGrant = new System.Windows.Forms.Button();
            this.btnDeny = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnEditUser = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnEditUser);
            this.pnlBottom.Controls.Add(this.button2);
            this.pnlBottom.Controls.Add(this.button1);
            this.pnlBottom.Controls.Add(this.textBox1);
            this.pnlBottom.Controls.Add(this.btnDeny);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.btnGrant);
            this.pnlBottom.Controls.Add(this.lvUsers);
            this.pnlBottom.Controls.Add(this.flowLayoutPanel1);
            this.pnlBottom.Controls.Add(this.lvPermissions);
            this.pnlBottom.Controls.Add(this.cmbGroup);
            this.pnlBottom.Controls.Add(this.lblPermissions);
            this.pnlBottom.Controls.Add(this.lblGroup);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.SizeChanged += new System.EventHandler(this.pnlBottom_SizeChanged);
            // 
            // lblGroup
            // 
            this.lblGroup.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblGroup, "lblGroup");
            this.lblGroup.Name = "lblGroup";
            // 
            // cmbGroup
            // 
            this.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroup.FormattingEnabled = true;
            resources.ApplyResources(this.cmbGroup, "cmbGroup");
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.SelectedIndexChanged += new System.EventHandler(this.cmbGroup_SelectedIndexChanged);
            // 
            // lvPermissions
            // 
            resources.ApplyResources(this.lvPermissions, "lvPermissions");
            this.lvPermissions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvPermissions.FullRowSelect = true;
            this.lvPermissions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvPermissions.HideSelection = false;
            this.lvPermissions.LockDrawing = false;
            this.lvPermissions.Name = "lvPermissions";
            this.lvPermissions.SortColumn = -1;
            this.lvPermissions.SortedBackwards = false;
            this.lvPermissions.UseCompatibleStateImageBehavior = false;
            this.lvPermissions.UseEveryOtherRowColoring = true;
            this.lvPermissions.View = System.Windows.Forms.View.Details;
            this.lvPermissions.SelectedIndexChanged += new System.EventHandler(this.lvPermissions_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // lblPermissions
            // 
            this.lblPermissions.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPermissions, "lblPermissions");
            this.lblPermissions.Name = "lblPermissions";
            // 
            // lnkNew
            // 
            resources.ApplyResources(this.lnkNew, "lnkNew");
            this.lnkNew.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkNew.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
            this.lnkNew.Name = "lnkNew";
            this.lnkNew.TabStop = true;
            this.lnkNew.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNew_LinkClicked);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.lnkNew);
            this.flowLayoutPanel1.Controls.Add(this.lnkEdit);
            this.flowLayoutPanel1.Controls.Add(this.lnkDelete);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // lnkEdit
            // 
            resources.ApplyResources(this.lnkEdit, "lnkEdit");
            this.lnkEdit.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkEdit.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
            this.lnkEdit.Name = "lnkEdit";
            this.lnkEdit.TabStop = true;
            this.lnkEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEdit_LinkClicked);
            // 
            // lnkDelete
            // 
            resources.ApplyResources(this.lnkDelete, "lnkDelete");
            this.lnkDelete.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkDelete.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
            this.lnkDelete.Name = "lnkDelete";
            this.lnkDelete.TabStop = true;
            this.lnkDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDelete_LinkClicked);
            // 
            // lvUsers
            // 
            resources.ApplyResources(this.lvUsers, "lvUsers");
            this.lvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lvUsers.FullRowSelect = true;
            this.lvUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvUsers.LockDrawing = false;
            this.lvUsers.MultiSelect = false;
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.SortColumn = -1;
            this.lvUsers.SortedBackwards = false;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.UseEveryOtherRowColoring = true;
            this.lvUsers.View = System.Windows.Forms.View.Details;
            this.lvUsers.SelectedIndexChanged += new System.EventHandler(this.lvUsers_SelectedIndexChanged);
            this.lvUsers.DoubleClick += new System.EventHandler(this.lvUsers_DoubleClick);
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // btnGrant
            // 
            resources.ApplyResources(this.btnGrant, "btnGrant");
            this.btnGrant.Name = "btnGrant";
            this.btnGrant.UseVisualStyleBackColor = true;
            this.btnGrant.Click += new System.EventHandler(this.btnGrant_Click);
            // 
            // btnDeny
            // 
            resources.ApplyResources(this.btnDeny, "btnDeny");
            this.btnDeny.Name = "btnDeny";
            this.btnDeny.UseVisualStyleBackColor = true;
            this.btnDeny.Click += new System.EventHandler(this.btnDeny_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // btnEditUser
            // 
            resources.ApplyResources(this.btnEditUser, "btnEditUser");
            this.btnEditUser.Name = "btnEditUser";
            this.btnEditUser.UseVisualStyleBackColor = true;
            this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
            // 
            // GroupPermissions
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 34;
            this.Name = "GroupPermissions";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.Label lblPermissions;
        private ExtendedListView lvPermissions;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.LinkLabel lnkNew;
        private System.Windows.Forms.LinkLabel lnkEdit;
        private System.Windows.Forms.LinkLabel lnkDelete;
        private System.Windows.Forms.Label label1;
        private ExtendedListView lvUsers;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnDeny;
        private System.Windows.Forms.Button btnGrant;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnEditUser;
    }
}