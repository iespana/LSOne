using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class LocationSelectPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationSelectPanel));
            this.groupPanel1 = new GroupPanel();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.locationListControl = new LocationListControl();
            this.locationTreeControl = new LocationTreeControl();
            this.groupPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.tbSearch);
            this.groupPanel1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // tbSearch
            // 
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel2, "panel2");
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
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // locationListControl
            // 
            this.locationListControl.ContextMenuStrip = null;
            resources.ApplyResources(this.locationListControl, "locationListControl");
            this.locationListControl.MultiSelect = false;
            this.locationListControl.Name = "locationListControl";
            this.locationListControl.LocationsSelected += new System.EventHandler<System.EventArgs>(this.locationListControl_LocationsSelected);
            this.locationListControl.DoubleClick += new System.EventHandler(this.locationListControl_DoubleClick);
            // 
            // locationTreeControl
            // 
            resources.ApplyResources(this.locationTreeControl, "locationTreeControl");
            this.locationTreeControl.Name = "locationTreeControl";
            this.locationTreeControl.SelectedLocation = null;
            this.locationTreeControl.SelectedLocationId = null;
            this.locationTreeControl.LocationSelected += new System.EventHandler<LocationSelectedEventArgs>(this.locationTreeControl_LocationSelected);
            this.locationTreeControl.LocationDoubleClicked += new System.EventHandler<LocationSelectedEventArgs>(this.locationTreeControl_LocationDoubleClicked);
            // 
            // LocationSelectPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.locationListControl);
            this.Controls.Add(this.locationTreeControl);
            this.Controls.Add(this.groupPanel1);
            this.Name = "LocationSelectPanel";
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LocationTreeControl locationTreeControl;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Label label1;
        private LocationListControl locationListControl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;


    }
}
