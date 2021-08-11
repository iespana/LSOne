using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class HospitalityTypeRoutingPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityTypeRoutingPage));
            this.btnsHospitalityTypeConnection = new LSOne.Controls.ContextButtons();
            this.chkAllHospitalityTypes = new System.Windows.Forms.CheckBox();
            this.lvHospitalityTypeConnections = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // btnsHospitalityTypeConnection
            // 
            this.btnsHospitalityTypeConnection.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsHospitalityTypeConnection, "btnsHospitalityTypeConnection");
            this.btnsHospitalityTypeConnection.BackColor = System.Drawing.Color.Transparent;
            this.btnsHospitalityTypeConnection.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsHospitalityTypeConnection.EditButtonEnabled = false;
            this.btnsHospitalityTypeConnection.Name = "btnsHospitalityTypeConnection";
            this.btnsHospitalityTypeConnection.RemoveButtonEnabled = true;
            this.btnsHospitalityTypeConnection.EditButtonClicked += new System.EventHandler(this.btnsHospitalityTypeConnection_EditButtonClicked);
            this.btnsHospitalityTypeConnection.AddButtonClicked += new System.EventHandler(this.btnsHospitalityTypeConnection_AddButtonClicked);
            this.btnsHospitalityTypeConnection.RemoveButtonClicked += new System.EventHandler(this.btnsHospitalityTypeConnection_RemoveButtonClicked);
            // 
            // chkAllHospitalityTypes
            // 
            resources.ApplyResources(this.chkAllHospitalityTypes, "chkAllHospitalityTypes");
            this.chkAllHospitalityTypes.Name = "chkAllHospitalityTypes";
            this.chkAllHospitalityTypes.UseVisualStyleBackColor = true;
            // 
            // lvHospitalityTypeConnections
            // 
            resources.ApplyResources(this.lvHospitalityTypeConnections, "lvHospitalityTypeConnections");
            this.lvHospitalityTypeConnections.BorderColor = System.Drawing.Color.DarkGray;
            this.lvHospitalityTypeConnections.BuddyControl = null;
            this.lvHospitalityTypeConnections.Columns.Add(this.column1);
            this.lvHospitalityTypeConnections.Columns.Add(this.column2);
            this.lvHospitalityTypeConnections.ContentBackColor = System.Drawing.Color.White;
            this.lvHospitalityTypeConnections.DefaultRowHeight = ((short)(18));
            this.lvHospitalityTypeConnections.DimSelectionWhenDisabled = true;
            this.lvHospitalityTypeConnections.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvHospitalityTypeConnections.HeaderBackColor = System.Drawing.Color.White;
            this.lvHospitalityTypeConnections.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvHospitalityTypeConnections.HeaderHeight = ((short)(25));
            this.lvHospitalityTypeConnections.HorizontalScrollbar = true;
            this.lvHospitalityTypeConnections.Name = "lvHospitalityTypeConnections";
            this.lvHospitalityTypeConnections.OddRowColor = System.Drawing.Color.White;
            this.lvHospitalityTypeConnections.RowLineColor = System.Drawing.Color.LightGray;
            this.lvHospitalityTypeConnections.SecondarySortColumn = ((short)(-1));
            this.lvHospitalityTypeConnections.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvHospitalityTypeConnections.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvHospitalityTypeConnections.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvHospitalityTypeConnections.SortSetting = "0:1";
            this.lvHospitalityTypeConnections.VerticalScrollbarValue = 0;
            this.lvHospitalityTypeConnections.VerticalScrollbarYOffset = 0;
            this.lvHospitalityTypeConnections.SelectionChanged += new System.EventHandler(this.lvHospitalityTypeConnections_SelectionChanged);
            this.lvHospitalityTypeConnections.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvHospitalityTypeConnections_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.InternalSort = true;
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // HospitalityTypeRoutingPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvHospitalityTypeConnections);
            this.Controls.Add(this.chkAllHospitalityTypes);
            this.Controls.Add(this.btnsHospitalityTypeConnection);
            this.DoubleBuffered = true;
            this.Name = "HospitalityTypeRoutingPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ContextButtons btnsHospitalityTypeConnection;
        private System.Windows.Forms.CheckBox chkAllHospitalityTypes;
        private ListView lvHospitalityTypeConnections;
        private LSOne.Controls.Columns.Column column1;
        private LSOne.Controls.Columns.Column column2;
    }
}
