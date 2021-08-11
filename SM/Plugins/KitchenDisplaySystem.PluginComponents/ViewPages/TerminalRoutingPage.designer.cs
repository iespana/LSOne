using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class TerminalRoutingPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalRoutingPage));
            this.btnsTerminalConnection = new LSOne.Controls.ContextButtons();
            this.chkAllTerminals = new System.Windows.Forms.CheckBox();
            this.lvTerminalConnections = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // btnsTerminalConnection
            // 
            this.btnsTerminalConnection.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsTerminalConnection, "btnsTerminalConnection");
            this.btnsTerminalConnection.BackColor = System.Drawing.Color.Transparent;
            this.btnsTerminalConnection.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsTerminalConnection.EditButtonEnabled = false;
            this.btnsTerminalConnection.Name = "btnsTerminalConnection";
            this.btnsTerminalConnection.RemoveButtonEnabled = true;
            this.btnsTerminalConnection.EditButtonClicked += new System.EventHandler(this.btnsTerminalConnection_EditButtonClicked);
            this.btnsTerminalConnection.AddButtonClicked += new System.EventHandler(this.btnsTerminalConnection_AddButtonClicked);
            this.btnsTerminalConnection.RemoveButtonClicked += new System.EventHandler(this.btnsTerminalConnection_RemoveButtonClicked);
            // 
            // chkAllTerminals
            // 
            resources.ApplyResources(this.chkAllTerminals, "chkAllTerminals");
            this.chkAllTerminals.Name = "chkAllTerminals";
            this.chkAllTerminals.UseVisualStyleBackColor = true;
            // 
            // lvTerminalConnections
            // 
            resources.ApplyResources(this.lvTerminalConnections, "lvTerminalConnections");
            this.lvTerminalConnections.BorderColor = System.Drawing.Color.DarkGray;
            this.lvTerminalConnections.BuddyControl = null;
            this.lvTerminalConnections.Columns.Add(this.column1);
            this.lvTerminalConnections.Columns.Add(this.column2);
            this.lvTerminalConnections.Columns.Add(this.column3);
            this.lvTerminalConnections.ContentBackColor = System.Drawing.Color.White;
            this.lvTerminalConnections.DefaultRowHeight = ((short)(18));
            this.lvTerminalConnections.DimSelectionWhenDisabled = true;
            this.lvTerminalConnections.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvTerminalConnections.HeaderBackColor = System.Drawing.Color.White;
            this.lvTerminalConnections.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvTerminalConnections.HeaderHeight = ((short)(25));
            this.lvTerminalConnections.HorizontalScrollbar = true;
            this.lvTerminalConnections.Name = "lvTerminalConnections";
            this.lvTerminalConnections.OddRowColor = System.Drawing.Color.White;
            this.lvTerminalConnections.RowLineColor = System.Drawing.Color.LightGray;
            this.lvTerminalConnections.SecondarySortColumn = ((short)(-1));
            this.lvTerminalConnections.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvTerminalConnections.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvTerminalConnections.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvTerminalConnections.SortSetting = "0:1";
            this.lvTerminalConnections.VerticalScrollbarValue = 0;
            this.lvTerminalConnections.VerticalScrollbarYOffset = 0;
            this.lvTerminalConnections.SelectionChanged += new System.EventHandler(this.lvTerminalConnections_SelectionChanged);
            this.lvTerminalConnections.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvTerminalConnections_RowDoubleClick);
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
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // TerminalRoutingPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvTerminalConnections);
            this.Controls.Add(this.chkAllTerminals);
            this.Controls.Add(this.btnsTerminalConnection);
            this.DoubleBuffered = true;
            this.Name = "TerminalRoutingPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ContextButtons btnsTerminalConnection;
        private System.Windows.Forms.CheckBox chkAllTerminals;
        private ListView lvTerminalConnections;
        private LSOne.Controls.Columns.Column column1;
        private LSOne.Controls.Columns.Column column2;
        private LSOne.Controls.Columns.Column column3;
    }
}
