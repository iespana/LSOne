using LSOne.Controls;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class ExternalPlugin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExternalPlugin));
            this.tbPluginName = new System.Windows.Forms.TextBox();
            this.lblPluginName = new System.Windows.Forms.Label();
            this.lvParameters = new LSOne.Controls.ListView();
            this.colName = new LSOne.Controls.Columns.Column();
            this.colValue = new LSOne.Controls.Columns.Column();
            this.btnMoveDown = new LSOne.Controls.ContextButton();
            this.btnMoveUp = new LSOne.Controls.ContextButton();
            this.contextButtons = new LSOne.Controls.ContextButtons();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.pnlParameters = new System.Windows.Forms.Panel();
            this.pnlSetupControl = new System.Windows.Forms.Panel();
            this.pnlParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPluginName
            // 
            resources.ApplyResources(this.tbPluginName, "tbPluginName");
            this.tbPluginName.Name = "tbPluginName";
            // 
            // lblPluginName
            // 
            resources.ApplyResources(this.lblPluginName, "lblPluginName");
            this.lblPluginName.Name = "lblPluginName";
            // 
            // lvParameters
            // 
            resources.ApplyResources(this.lvParameters, "lvParameters");
            this.lvParameters.BuddyControl = null;
            this.lvParameters.Columns.Add(this.colName);
            this.lvParameters.Columns.Add(this.colValue);
            this.lvParameters.ContentBackColor = System.Drawing.Color.White;
            this.lvParameters.DefaultRowHeight = ((short)(22));
            this.lvParameters.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvParameters.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvParameters.HeaderHeight = ((short)(25));
            this.lvParameters.Name = "lvParameters";
            this.lvParameters.OddRowColor = System.Drawing.Color.White;
            this.lvParameters.RowLineColor = System.Drawing.Color.LightGray;
            this.lvParameters.SecondarySortColumn = ((short)(-1));
            this.lvParameters.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvParameters.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvParameters.SortSetting = "0:1";
            this.lvParameters.SelectionChanged += new System.EventHandler(this.lvJobSubJobs_SelectionChanged);
            this.lvParameters.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvJobSubJobs_RowDoubleClick);
            // 
            // colName
            // 
            this.colName.AutoSize = true;
            this.colName.Clickable = false;
            this.colName.DefaultStyle = null;
            resources.ApplyResources(this.colName, "colName");
            this.colName.MaximumWidth = ((short)(0));
            this.colName.MinimumWidth = ((short)(10));
            this.colName.SecondarySortColumn = ((short)(-1));
            this.colName.Sizable = false;
            this.colName.Tag = null;
            this.colName.Width = ((short)(50));
            // 
            // colValue
            // 
            this.colValue.AutoSize = true;
            this.colValue.DefaultStyle = null;
            resources.ApplyResources(this.colValue, "colValue");
            this.colValue.MaximumWidth = ((short)(0));
            this.colValue.MinimumWidth = ((short)(10));
            this.colValue.SecondarySortColumn = ((short)(-1));
            this.colValue.Sizable = false;
            this.colValue.Tag = null;
            this.colValue.Width = ((short)(50));
            // 
            // btnMoveDown
            // 
            resources.ApplyResources(this.btnMoveDown, "btnMoveDown");
            this.btnMoveDown.BackColor = System.Drawing.Color.Transparent;
            this.btnMoveDown.Context = LSOne.Controls.ButtonType.MoveDown;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            resources.ApplyResources(this.btnMoveUp, "btnMoveUp");
            this.btnMoveUp.BackColor = System.Drawing.Color.Transparent;
            this.btnMoveUp.Context = LSOne.Controls.ButtonType.MoveUp;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // contextButtons
            // 
            this.contextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.contextButtons, "contextButtons");
            this.contextButtons.BackColor = System.Drawing.Color.Transparent;
            this.contextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.contextButtons.EditButtonEnabled = true;
            this.contextButtons.Name = "contextButtons";
            this.contextButtons.RemoveButtonEnabled = true;
            this.contextButtons.EditButtonClicked += new System.EventHandler(this.contextButtons_EditButtonClicked);
            this.contextButtons.AddButtonClicked += new System.EventHandler(this.contextButtons_AddButtonClicked);
            this.contextButtons.RemoveButtonClicked += new System.EventHandler(this.contextButtons_RemoveButtonClicked);
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // pnlParameters
            // 
            resources.ApplyResources(this.pnlParameters, "pnlParameters");
            this.pnlParameters.Controls.Add(this.lvParameters);
            this.pnlParameters.Controls.Add(this.btnMoveUp);
            this.pnlParameters.Controls.Add(this.btnMoveDown);
            this.pnlParameters.Controls.Add(this.contextButtons);
            this.pnlParameters.Name = "pnlParameters";
            // 
            // pnlSetupControl
            // 
            resources.ApplyResources(this.pnlSetupControl, "pnlSetupControl");
            this.pnlSetupControl.Name = "pnlSetupControl";
            // 
            // ExternalPlugin
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlParameters);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblPluginName);
            this.Controls.Add(this.tbPluginName);
            this.Controls.Add(this.pnlSetupControl);
            this.Name = "ExternalPlugin";
            this.pnlParameters.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ContextButtons contextButtons;
        private ContextButton btnMoveDown;
        private ContextButton btnMoveUp;
        private System.Windows.Forms.TextBox tbPluginName;
        private System.Windows.Forms.Label lblPluginName;
        private ListView lvParameters;
        private LSOne.Controls.Columns.Column colName;
        private LSOne.Controls.Columns.Column colValue;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Panel pnlParameters;
        private System.Windows.Forms.Panel pnlSetupControl;
    }
}
