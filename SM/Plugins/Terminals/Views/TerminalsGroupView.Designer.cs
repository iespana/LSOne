using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Terminals.Views
{
    partial class TerminalsGroupView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalsGroupView));
            this.lvGroups = new LSOne.Controls.ListView();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.lvDetailedGroups = new LSOne.Controls.ListView();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.btnGroupContextButtons = new LSOne.Controls.ContextButtons();
            this.btnTerminalGroupContextButtons = new LSOne.Controls.ContextButtons();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvGroups);
            this.pnlBottom.Controls.Add(this.btnGroupContextButtons);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            // 
            // lvGroups
            // 
            resources.ApplyResources(this.lvGroups, "lvGroups");
            this.lvGroups.BuddyControl = null;
            this.lvGroups.Columns.Add(this.column4);
            this.lvGroups.ContentBackColor = System.Drawing.Color.White;
            this.lvGroups.DefaultRowHeight = ((short)(22));
            this.lvGroups.DimSelectionWhenDisabled = true;
            this.lvGroups.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvGroups.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvGroups.HeaderHeight = ((short)(25));
            this.lvGroups.Name = "lvGroups";
            this.lvGroups.OddRowColor = System.Drawing.Color.White;
            this.lvGroups.RowLineColor = System.Drawing.Color.LightGray;
            this.lvGroups.SecondarySortColumn = ((short)(-1));
            this.lvGroups.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvGroups.SortSetting = "-1:1";
            this.lvGroups.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvGroups_HeaderClicked);
            this.lvGroups.SelectionChanged += new System.EventHandler(this.lvGroups_SelectionChanged);
            this.lvGroups.DoubleClick += new System.EventHandler(this.lvGroups_DoubleClick);
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(150));
            this.column4.RelativeSize = 100;
            this.column4.Tag = null;
            this.column4.Width = ((short)(400));
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(70));
            this.column1.Tag = null;
            this.column1.Width = ((short)(70));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(200));
            this.column2.Tag = null;
            this.column2.Width = ((short)(250));
            // 
            // lvDetailedGroups
            // 
            resources.ApplyResources(this.lvDetailedGroups, "lvDetailedGroups");
            this.lvDetailedGroups.BuddyControl = null;
            this.lvDetailedGroups.Columns.Add(this.column1);
            this.lvDetailedGroups.Columns.Add(this.column2);
            this.lvDetailedGroups.Columns.Add(this.column3);
            this.lvDetailedGroups.ContentBackColor = System.Drawing.Color.White;
            this.lvDetailedGroups.DefaultRowHeight = ((short)(22));
            this.lvDetailedGroups.DimSelectionWhenDisabled = true;
            this.lvDetailedGroups.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvDetailedGroups.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDetailedGroups.HeaderHeight = ((short)(25));
            this.lvDetailedGroups.Name = "lvDetailedGroups";
            this.lvDetailedGroups.OddRowColor = System.Drawing.Color.White;
            this.lvDetailedGroups.RowLineColor = System.Drawing.Color.LightGray;
            this.lvDetailedGroups.SecondarySortColumn = ((short)(-1));
            this.lvDetailedGroups.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvDetailedGroups.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvDetailedGroups.SortSetting = "-1:1";
            this.lvDetailedGroups.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvDetailedGroups_HeaderClicked);
            this.lvDetailedGroups.SelectionChanged += new System.EventHandler(this.lvDetailedGroups_SelectionChanged);
            // 
            // column3
            // 
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(200));
            this.column3.Tag = null;
            this.column3.Width = ((short)(200));
            // 
            // btnGroupContextButtons
            // 
            this.btnGroupContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnGroupContextButtons, "btnGroupContextButtons");
            this.btnGroupContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnGroupContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnGroupContextButtons.EditButtonEnabled = false;
            this.btnGroupContextButtons.Name = "btnGroupContextButtons";
            this.btnGroupContextButtons.RemoveButtonEnabled = false;
            this.btnGroupContextButtons.EditButtonClicked += new System.EventHandler(this.btnGroupContextButtons_EditButtonClicked);
            this.btnGroupContextButtons.AddButtonClicked += new System.EventHandler(this.btnGroupContextButtons_AddButtonClicked);
            this.btnGroupContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnGroupContextButtons_RemoveButtonClicked);
            // 
            // btnTerminalGroupContextButtons
            // 
            this.btnTerminalGroupContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnTerminalGroupContextButtons, "btnTerminalGroupContextButtons");
            this.btnTerminalGroupContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnTerminalGroupContextButtons.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.btnTerminalGroupContextButtons.EditButtonEnabled = false;
            this.btnTerminalGroupContextButtons.Name = "btnTerminalGroupContextButtons";
            this.btnTerminalGroupContextButtons.RemoveButtonEnabled = false;
            this.btnTerminalGroupContextButtons.AddButtonClicked += new System.EventHandler(this.btnTerminalGroupContextButtons_AddButtonClicked);
            this.btnTerminalGroupContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnTerminalGroupContextButtons_RemoveButtonClicked);
            this.btnTerminalGroupContextButtons.Load += new System.EventHandler(this.btnTerminalGroupContextButtons_Load);
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.btnTerminalGroupContextButtons);
            this.groupPanel1.Controls.Add(this.lvDetailedGroups);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lblGroupHeader
            // 
            this.lblGroupHeader.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblGroupHeader, "lblGroupHeader");
            this.lblGroupHeader.Name = "lblGroupHeader";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // TerminalsGroupView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TerminalsGroupView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListView lvGroups;
        private Column column1;
        private Column column2;
        private ListView lvDetailedGroups;
        private Column column4;
        private ContextButtons btnGroupContextButtons;
        private ContextButtons btnTerminalGroupContextButtons;
        private Column column3;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblGroupHeader;
        private System.Windows.Forms.Label lblNoSelection;
    }
}
