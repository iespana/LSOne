namespace LSOne.ViewPlugins.Replenishment.ViewPages
{
    partial class InventoryTemplateStockCountingPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryTemplateStockCountingPage));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.btnEditArea = new LSOne.Controls.ContextButton();
            this.txtArea = new System.Windows.Forms.TextBox();
            this.lblArea = new System.Windows.Forms.Label();
            this.btnCopyFrom = new System.Windows.Forms.Button();
            this.lblAreaLines = new System.Windows.Forms.Label();
            this.lvAreas = new LSOne.Controls.ListView();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.contextButtons = new LSOne.Controls.ContextButtons();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.Controls.Add(this.btnEditArea);
            this.groupBox.Controls.Add(this.txtArea);
            this.groupBox.Controls.Add(this.lblArea);
            this.groupBox.Controls.Add(this.btnCopyFrom);
            this.groupBox.Controls.Add(this.lblAreaLines);
            this.groupBox.Controls.Add(this.lvAreas);
            this.groupBox.Controls.Add(this.contextButtons);
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // btnEditArea
            // 
            this.btnEditArea.BackColor = System.Drawing.Color.Transparent;
            this.btnEditArea.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditArea, "btnEditArea");
            this.btnEditArea.Name = "btnEditArea";
            this.btnEditArea.Click += new System.EventHandler(this.btnEditArea_Click);
            // 
            // txtArea
            // 
            resources.ApplyResources(this.txtArea, "txtArea");
            this.txtArea.Name = "txtArea";
            // 
            // lblArea
            // 
            this.lblArea.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblArea, "lblArea");
            this.lblArea.Name = "lblArea";
            // 
            // btnCopyFrom
            // 
            resources.ApplyResources(this.btnCopyFrom, "btnCopyFrom");
            this.btnCopyFrom.Name = "btnCopyFrom";
            this.btnCopyFrom.UseVisualStyleBackColor = true;
            this.btnCopyFrom.Click += new System.EventHandler(this.btnCopyFrom_Click);
            // 
            // lblAreaLines
            // 
            this.lblAreaLines.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblAreaLines, "lblAreaLines");
            this.lblAreaLines.Name = "lblAreaLines";
            // 
            // lvAreas
            // 
            resources.ApplyResources(this.lvAreas, "lvAreas");
            this.lvAreas.BorderColor = System.Drawing.Color.DarkGray;
            this.lvAreas.BuddyControl = null;
            this.lvAreas.Columns.Add(this.clmDescription);
            this.lvAreas.ContentBackColor = System.Drawing.Color.White;
            this.lvAreas.DefaultRowHeight = ((short)(18));
            this.lvAreas.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvAreas.HeaderBackColor = System.Drawing.Color.White;
            this.lvAreas.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lvAreas.HeaderHeight = ((short)(25));
            this.lvAreas.Name = "lvAreas";
            this.lvAreas.OddRowColor = System.Drawing.Color.White;
            this.lvAreas.RowLineColor = System.Drawing.Color.LightGray;
            this.lvAreas.SecondarySortColumn = ((short)(-1));
            this.lvAreas.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvAreas.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvAreas.SortSetting = "0:1";
            this.lvAreas.VerticalScrollbarValue = 0;
            this.lvAreas.VerticalScrollbarYOffset = 0;
            this.lvAreas.SelectionChanged += new System.EventHandler(this.lvAreas_SelectionChanged);
            this.lvAreas.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvAreas_RowDoubleClick);
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.Clickable = false;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.InternalSort = true;
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.RelativeSize = 100;
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(100));
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
            // InventoryTemplateStockCountingPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox);
            this.DoubleBuffered = true;
            this.Name = "InventoryTemplateStockCountingPage";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private Controls.ContextButtons contextButtons;
        private System.Windows.Forms.Button btnCopyFrom;
        private System.Windows.Forms.Label lblAreaLines;
        private Controls.ListView lvAreas;
        private Controls.Columns.Column clmDescription;
        private System.Windows.Forms.TextBox txtArea;
        private System.Windows.Forms.Label lblArea;
        private Controls.ContextButton btnEditArea;
    }
}
