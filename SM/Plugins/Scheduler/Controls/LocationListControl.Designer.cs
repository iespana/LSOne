using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class LocationListControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationListControl));
            this.ilLocations = new System.Windows.Forms.ImageList(this.components);
            this.lvLocations = new LSOne.Controls.ListView();
            this.colGroups = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // ilLocations
            // 
            this.ilLocations.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.ilLocations, "ilLocations");
            this.ilLocations.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lvLocations
            // 
            resources.ApplyResources(this.lvLocations, "lvLocations");
            this.lvLocations.BuddyControl = null;
            this.lvLocations.Columns.Add(this.colGroups);
            this.lvLocations.ContentBackColor = System.Drawing.Color.White;
            this.lvLocations.DefaultRowHeight = ((short)(22));
            this.lvLocations.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvLocations.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLocations.HeaderHeight = ((short)(25));
            this.lvLocations.Name = "lvLocations";
            this.lvLocations.OddRowColor = System.Drawing.Color.White;
            this.lvLocations.RowLineColor = System.Drawing.Color.LightGray;
            this.lvLocations.SecondarySortColumn = ((short)(-1));
            this.lvLocations.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvLocations.SortSetting = "0:1";
            this.lvLocations.SelectionChanged += new System.EventHandler(this.lvLocations_SelectionChanged);
            this.lvLocations.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvLocations_RowDoubleClick);
            // 
            // colGroups
            // 
            this.colGroups.AutoSize = true;
            this.colGroups.Clickable = false;
            this.colGroups.DefaultStyle = null;
            resources.ApplyResources(this.colGroups, "colGroups");
            this.colGroups.MaximumWidth = ((short)(0));
            this.colGroups.MinimumWidth = ((short)(10));
            this.colGroups.Sizable = false;
            this.colGroups.Tag = null;
            this.colGroups.Width = ((short)(50));
            // 
            // LocationListControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvLocations);
            this.Name = "LocationListControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList ilLocations;
        private ListView lvLocations;
        private LSOne.Controls.Columns.Column colGroups;
    }
}
