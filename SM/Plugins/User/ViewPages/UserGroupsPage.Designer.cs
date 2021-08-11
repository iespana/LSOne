using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.User.ViewPages
{
    partial class UserGroupsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserGroupsPage));
            this.lvUserGroups = new ListView();
            this.column1 = new Column();
            this.SuspendLayout();
            // 
            // lvUserGroups
            // 
            resources.ApplyResources(this.lvUserGroups, "lvUserGroups");
            this.lvUserGroups.BuddyControl = null;
            this.lvUserGroups.Columns.Add(this.column1);
            this.lvUserGroups.ContentBackColor = System.Drawing.Color.White;
            this.lvUserGroups.DefaultRowHeight = ((short)(22));
            this.lvUserGroups.DimSelectionWhenDisabled = true;
            this.lvUserGroups.EvenRowColor = System.Drawing.Color.White;
            this.lvUserGroups.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvUserGroups.HeaderHeight = ((short)(1));
            this.lvUserGroups.Name = "lvUserGroups";
            this.lvUserGroups.OddRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvUserGroups.RowLineColor = System.Drawing.Color.Transparent;
            this.lvUserGroups.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.lvUserGroups.CellAction += new CellActionDelegate(this.lvUserGroups_CellAction);
            // 
            // column1
            // 
            this.column1.AutoSize = false;
            this.column1.Clickable = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.RelativeSize = 100;
            this.column1.Sizable = true;
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // UserGroupsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvUserGroups);
            this.Name = "UserGroupsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ListView lvUserGroups;
        private Column column1;
    }
}
