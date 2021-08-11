using LSOne.Controls;

namespace LSOne.ViewPlugins.User.ViewPages
{
    partial class UserPermissionPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserPermissionPage));
            this.btnInherit = new System.Windows.Forms.Button();
            this.btnDeny = new System.Windows.Forms.Button();
            this.btnGrant = new System.Windows.Forms.Button();
            this.lvPermissions = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInherit
            // 
            resources.ApplyResources(this.btnInherit, "btnInherit");
            this.btnInherit.Name = "btnInherit";
            this.btnInherit.UseVisualStyleBackColor = true;
            this.btnInherit.Click += new System.EventHandler(this.btnInherit_Click);
            // 
            // btnDeny
            // 
            resources.ApplyResources(this.btnDeny, "btnDeny");
            this.btnDeny.Name = "btnDeny";
            this.btnDeny.UseVisualStyleBackColor = true;
            this.btnDeny.Click += new System.EventHandler(this.btnDeny_Click);
            // 
            // btnGrant
            // 
            resources.ApplyResources(this.btnGrant, "btnGrant");
            this.btnGrant.Name = "btnGrant";
            this.btnGrant.UseVisualStyleBackColor = true;
            this.btnGrant.Click += new System.EventHandler(this.btnGrant_Click);
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
            this.lvPermissions.SizeChanged += new System.EventHandler(this.lvPermissions_SizeChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // UserPermissionPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnInherit);
            this.Controls.Add(this.btnDeny);
            this.Controls.Add(this.btnGrant);
            this.Controls.Add(this.lvPermissions);
            this.Name = "UserPermissionPage";
            this.SizeChanged += new System.EventHandler(this.UserPermissionPage_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInherit;
        private System.Windows.Forms.Button btnDeny;
        private System.Windows.Forms.Button btnGrant;
        private ExtendedListView lvPermissions;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
