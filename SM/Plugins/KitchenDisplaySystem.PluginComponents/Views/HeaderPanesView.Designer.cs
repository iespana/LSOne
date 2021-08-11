using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    partial class HeaderPanesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeaderPanesView));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbLine = new System.Windows.Forms.ComboBox();
            this.btnAddLine = new System.Windows.Forms.Button();
            this.btnDeleteLine = new System.Windows.Forms.Button();
            this.lblLine = new System.Windows.Forms.Label();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lvHeaderPaneColumns = new LSOne.Controls.ListView();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.column9 = new LSOne.Controls.Columns.Column();
            this.btnMoveDown = new LSOne.Controls.ContextButton();
            this.btnMoveUp = new LSOne.Controls.ContextButton();
            this.btnsContextButtonsColumns = new LSOne.Controls.ContextButtons();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.lvHeaderPanes = new LSOne.Controls.ListView();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvHeaderPanes);
            this.pnlBottom.Controls.Add(this.groupBox1);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEditHeaderPane_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAddHeaderPane_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemoveHeaderPane_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.cmbLine);
            this.groupBox1.Controls.Add(this.btnAddLine);
            this.groupBox1.Controls.Add(this.btnDeleteLine);
            this.groupBox1.Controls.Add(this.lblLine);
            this.groupBox1.Controls.Add(this.groupPanel1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmbLine
            // 
            this.cmbLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLine.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLine, "cmbLine");
            this.cmbLine.Name = "cmbLine";
            this.cmbLine.SelectedIndexChanged += new System.EventHandler(this.cmbLine_SelectedIndexChanged);
            // 
            // btnAddLine
            // 
            resources.ApplyResources(this.btnAddLine, "btnAddLine");
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.UseVisualStyleBackColor = true;
            this.btnAddLine.Click += new System.EventHandler(this.btnAddLine_Click);
            // 
            // btnDeleteLine
            // 
            resources.ApplyResources(this.btnDeleteLine, "btnDeleteLine");
            this.btnDeleteLine.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDeleteLine.Name = "btnDeleteLine";
            this.btnDeleteLine.UseVisualStyleBackColor = true;
            this.btnDeleteLine.Click += new System.EventHandler(this.btnDeleteLine_Click);
            // 
            // lblLine
            // 
            this.lblLine.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblLine, "lblLine");
            this.lblLine.Name = "lblLine";
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.lvHeaderPaneColumns);
            this.groupPanel1.Controls.Add(this.btnMoveDown);
            this.groupPanel1.Controls.Add(this.btnMoveUp);
            this.groupPanel1.Controls.Add(this.btnsContextButtonsColumns);
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lvHeaderPaneColumns
            // 
            resources.ApplyResources(this.lvHeaderPaneColumns, "lvHeaderPaneColumns");
            this.lvHeaderPaneColumns.BorderColor = System.Drawing.Color.DarkGray;
            this.lvHeaderPaneColumns.BuddyControl = null;
            this.lvHeaderPaneColumns.Columns.Add(this.column5);
            this.lvHeaderPaneColumns.Columns.Add(this.column6);
            this.lvHeaderPaneColumns.Columns.Add(this.column7);
            this.lvHeaderPaneColumns.Columns.Add(this.column8);
            this.lvHeaderPaneColumns.Columns.Add(this.column9);
            this.lvHeaderPaneColumns.ContentBackColor = System.Drawing.Color.White;
            this.lvHeaderPaneColumns.DefaultRowHeight = ((short)(22));
            this.lvHeaderPaneColumns.DimSelectionWhenDisabled = true;
            this.lvHeaderPaneColumns.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvHeaderPaneColumns.HeaderBackColor = System.Drawing.Color.White;
            this.lvHeaderPaneColumns.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvHeaderPaneColumns.HeaderHeight = ((short)(25));
            this.lvHeaderPaneColumns.Name = "lvHeaderPaneColumns";
            this.lvHeaderPaneColumns.OddRowColor = System.Drawing.Color.White;
            this.lvHeaderPaneColumns.RowLineColor = System.Drawing.Color.LightGray;
            this.lvHeaderPaneColumns.SecondarySortColumn = ((short)(-1));
            this.lvHeaderPaneColumns.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvHeaderPaneColumns.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvHeaderPaneColumns.SortSetting = "0:1";
            this.lvHeaderPaneColumns.HideVerticalScrollbarWhenDisabled = true;
            this.lvHeaderPaneColumns.VerticalScrollbarValue = 0;
            this.lvHeaderPaneColumns.VerticalScrollbarYOffset = 0;
            this.lvHeaderPaneColumns.SelectionChanged += new System.EventHandler(this.lvHeaderPaneColumns_SelectedIndexChanged);
            this.lvHeaderPaneColumns.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvHeaderPaneColumn_DoubleClick);
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.SecondarySortColumn = ((short)(-1));
            this.column7.Tag = null;
            this.column7.Width = ((short)(50));
            // 
            // column8
            // 
            this.column8.AutoSize = true;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.SecondarySortColumn = ((short)(-1));
            this.column8.Tag = null;
            this.column8.Width = ((short)(50));
            // 
            // column9
            // 
            this.column9.AutoSize = true;
            this.column9.DefaultStyle = null;
            resources.ApplyResources(this.column9, "column9");
            this.column9.MaximumWidth = ((short)(0));
            this.column9.MinimumWidth = ((short)(10));
            this.column9.SecondarySortColumn = ((short)(-1));
            this.column9.Tag = null;
            this.column9.Width = ((short)(50));
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
            // btnsContextButtonsColumns
            // 
            this.btnsContextButtonsColumns.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtonsColumns, "btnsContextButtonsColumns");
            this.btnsContextButtonsColumns.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtonsColumns.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtonsColumns.EditButtonEnabled = true;
            this.btnsContextButtonsColumns.Name = "btnsContextButtonsColumns";
            this.btnsContextButtonsColumns.RemoveButtonEnabled = true;
            this.btnsContextButtonsColumns.EditButtonClicked += new System.EventHandler(this.btnEditHeaderColumn_Click);
            this.btnsContextButtonsColumns.AddButtonClicked += new System.EventHandler(this.btnAddHeaderColumn_Click);
            this.btnsContextButtonsColumns.RemoveButtonClicked += new System.EventHandler(this.btnRemoveHeaderColumn_Click);
            // 
            // lblGroupHeader
            // 
            resources.ApplyResources(this.lblGroupHeader, "lblGroupHeader");
            this.lblGroupHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupHeader.Name = "lblGroupHeader";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // lvHeaderPanes
            // 
            resources.ApplyResources(this.lvHeaderPanes, "lvHeaderPanes");
            this.lvHeaderPanes.BorderColor = System.Drawing.Color.DarkGray;
            this.lvHeaderPanes.BuddyControl = null;
            this.lvHeaderPanes.Columns.Add(this.column3);
            this.lvHeaderPanes.Columns.Add(this.column4);
            this.lvHeaderPanes.ContentBackColor = System.Drawing.Color.White;
            this.lvHeaderPanes.DefaultRowHeight = ((short)(22));
            this.lvHeaderPanes.DimSelectionWhenDisabled = true;
            this.lvHeaderPanes.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvHeaderPanes.HeaderBackColor = System.Drawing.Color.White;
            this.lvHeaderPanes.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvHeaderPanes.HeaderHeight = ((short)(25));
            this.lvHeaderPanes.Name = "lvHeaderPanes";
            this.lvHeaderPanes.OddRowColor = System.Drawing.Color.White;
            this.lvHeaderPanes.RowLineColor = System.Drawing.Color.LightGray;
            this.lvHeaderPanes.SecondarySortColumn = ((short)(-1));
            this.lvHeaderPanes.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvHeaderPanes.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvHeaderPanes.SortSetting = "0:1";
            this.lvHeaderPanes.VerticalScrollbar = false;
            this.lvHeaderPanes.VerticalScrollbarValue = 0;
            this.lvHeaderPanes.VerticalScrollbarYOffset = 0;
            this.lvHeaderPanes.SelectionChanged += new System.EventHandler(this.lvHeaderPanes_SelectedIndexChanged);
            this.lvHeaderPanes.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvHeaderPane_DoubleClick);
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
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.InternalSort = true;
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // HeaderPanesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HeaderPanesView";
            this.pnlBottom.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.GroupBox groupBox1;
        private GroupPanel groupPanel1;
        private ContextButtons btnsContextButtonsColumns;
        private System.Windows.Forms.Label lblGroupHeader;
        private System.Windows.Forms.Label lblNoSelection;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.Button btnDeleteLine;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.ComboBox cmbLine;
        private ContextButton btnMoveDown;
        private ContextButton btnMoveUp;
        private ListView lvHeaderPanes;
        private ListView lvHeaderPaneColumns;
        private LSOne.Controls.Columns.Column column3;
        private LSOne.Controls.Columns.Column column4;
        private LSOne.Controls.Columns.Column column5;
        private LSOne.Controls.Columns.Column column6;
        private LSOne.Controls.Columns.Column column7;
        private LSOne.Controls.Columns.Column column8;
        private LSOne.Controls.Columns.Column column9;
    }
}
