namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class TableDesignTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableDesignTree));
            this.tvDesigns = new System.Windows.Forms.TreeView();
            this.ilDesigns = new System.Windows.Forms.ImageList(this.components);
            this.lblNoDesignsFound = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tvDesigns
            // 
            resources.ApplyResources(this.tvDesigns, "tvDesigns");
            this.tvDesigns.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tvDesigns.HideSelection = false;
            this.tvDesigns.ImageList = this.ilDesigns;
            this.tvDesigns.Name = "tvDesigns";
            this.tvDesigns.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvDesigns_BeforeCheck);
            this.tvDesigns.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvDesigns_AfterCheck);
            this.tvDesigns.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvDesigns_BeforeExpand);
            this.tvDesigns.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tvDesigns_DrawNode);
            this.tvDesigns.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDesigns_AfterSelect);
            // 
            // ilDesigns
            // 
            this.ilDesigns.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilDesigns.ImageStream")));
            this.ilDesigns.TransparentColor = System.Drawing.Color.Transparent;
            this.ilDesigns.Images.SetKeyName(0, "Data Green.bmp");
            this.ilDesigns.Images.SetKeyName(1, "Data Green Table.bmp");
            // 
            // lblNoDesignsFound
            // 
            resources.ApplyResources(this.lblNoDesignsFound, "lblNoDesignsFound");
            this.lblNoDesignsFound.BackColor = System.Drawing.Color.Transparent;
            this.lblNoDesignsFound.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoDesignsFound.Name = "lblNoDesignsFound";
            // 
            // TableDesignTree
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvDesigns);
            this.Controls.Add(this.lblNoDesignsFound);
            this.Name = "TableDesignTree";
            this.Load += new System.EventHandler(this.TableDesignTree_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvDesigns;
        private System.Windows.Forms.ImageList ilDesigns;
        private System.Windows.Forms.Label lblNoDesignsFound;
    }
}
