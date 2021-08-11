namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class LocationTreeControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationTreeControl));
            this.tvLocations = new System.Windows.Forms.TreeView();
            this.ilLocations = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // tvLocations
            // 
            resources.ApplyResources(this.tvLocations, "tvLocations");
            this.tvLocations.ImageList = this.ilLocations;
            this.tvLocations.Name = "tvLocations";
            this.tvLocations.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvLocations_BeforeExpand);
            this.tvLocations.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvLocations_AfterSelect);
            this.tvLocations.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvLocations_NodeMouseDoubleClick);
            // 
            // ilLocations
            // 
            this.ilLocations.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.ilLocations, "ilLocations");
            this.ilLocations.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // LocationTreeControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvLocations);
            this.Name = "LocationTreeControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvLocations;
        private System.Windows.Forms.ImageList ilLocations;
    }
}
