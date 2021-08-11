namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class DatabaseDesignTree
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseDesignTree));
            this.tvMain = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ilMain = new System.Windows.Forms.ImageList(this.components);
            this.lblNoDesignsFound = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tvMain
            // 
            this.tvMain.ContextMenuStrip = this.contextMenuStrip;
            resources.ApplyResources(this.tvMain, "tvMain");
            this.tvMain.ImageList = this.ilMain;
            this.tvMain.Name = "tvMain";
            this.tvMain.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvMain_BeforeExpand);
            this.tvMain.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvMain_AfterExpand);
            this.tvMain.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvMain_BeforeSelect);
            this.tvMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvMain_AfterSelect);
            this.tvMain.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvMain_NodeMouseDoubleClick);
            this.tvMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvMain_KeyPress);
            this.tvMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvMain_MouseDown);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // ilMain
            // 
            this.ilMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilMain.ImageStream")));
            this.ilMain.TransparentColor = System.Drawing.Color.Transparent;
            this.ilMain.Images.SetKeyName(0, "DatabaseDesign");
            this.ilMain.Images.SetKeyName(1, "TableDesign");
            this.ilMain.Images.SetKeyName(2, "LinkedTable");
            this.ilMain.Images.SetKeyName(3, "DataDisabled");
            this.ilMain.Images.SetKeyName(4, "TableDesignDisabled");
            // 
            // lblNoDesignsFound
            // 
            resources.ApplyResources(this.lblNoDesignsFound, "lblNoDesignsFound");
            this.lblNoDesignsFound.BackColor = System.Drawing.Color.Transparent;
            this.lblNoDesignsFound.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoDesignsFound.Name = "lblNoDesignsFound";
            // 
            // DatabaseDesignTree
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvMain);
            this.Controls.Add(this.lblNoDesignsFound);
            this.Name = "DatabaseDesignTree";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvMain;
        private System.Windows.Forms.ImageList ilMain;
        private System.Windows.Forms.Label lblNoDesignsFound;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    }
}
