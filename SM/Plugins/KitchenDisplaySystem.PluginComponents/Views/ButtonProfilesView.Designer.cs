using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    partial class ButtonProfilesView
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonProfilesView));
            this.lvKitchenDisplayMenuHeaders = new LSOne.Controls.ExtendedListView();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsEditAddRemovePosMenu = new LSOne.Controls.ContextButtons();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.btnMoveDown = new LSOne.Controls.ContextButton();
            this.btnMoveUp = new LSOne.Controls.ContextButton();
            this.btnsEditAddRemovePosMenuLine = new LSOne.Controls.ContextButtons();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lvKitchenDisplayMenuLines = new LSOne.Controls.ExtendedListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.groupPanel1);
            this.pnlBottom.Controls.Add(this.lvKitchenDisplayMenuHeaders);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemovePosMenu);
            // 
            // lvKitchenDisplayMenuHeaders
            // 
            resources.ApplyResources(this.lvKitchenDisplayMenuHeaders, "lvKitchenDisplayMenuHeaders");
            this.lvKitchenDisplayMenuHeaders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader13});
            this.lvKitchenDisplayMenuHeaders.FullRowSelect = true;
            this.lvKitchenDisplayMenuHeaders.HideSelection = false;
            this.lvKitchenDisplayMenuHeaders.LockDrawing = false;
            this.lvKitchenDisplayMenuHeaders.Name = "lvKitchenDisplayMenuHeaders";
            this.lvKitchenDisplayMenuHeaders.SortColumn = -1;
            this.lvKitchenDisplayMenuHeaders.SortedBackwards = false;
            this.lvKitchenDisplayMenuHeaders.UseCompatibleStateImageBehavior = false;
            this.lvKitchenDisplayMenuHeaders.UseEveryOtherRowColoring = true;
            this.lvKitchenDisplayMenuHeaders.View = System.Windows.Forms.View.Details;
            this.lvKitchenDisplayMenuHeaders.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvKitchenDisplayMenuHeaders_ColumnClick);
            this.lvKitchenDisplayMenuHeaders.SelectedIndexChanged += new System.EventHandler(this.lvKitchenDisplayMenuHeaders_SelectedIndexChanged);
            this.lvKitchenDisplayMenuHeaders.DoubleClick += new System.EventHandler(this.lvKitchenDisplayMenuHeaders_DoubleClick);
            // 
            // columnHeader8
            // 
            resources.ApplyResources(this.columnHeader8, "columnHeader8");
            // 
            // columnHeader13
            // 
            resources.ApplyResources(this.columnHeader13, "columnHeader13");
            // 
            // btnsEditAddRemovePosMenu
            // 
            this.btnsEditAddRemovePosMenu.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemovePosMenu, "btnsEditAddRemovePosMenu");
            this.btnsEditAddRemovePosMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemovePosMenu.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemovePosMenu.EditButtonEnabled = false;
            this.btnsEditAddRemovePosMenu.Name = "btnsEditAddRemovePosMenu";
            this.btnsEditAddRemovePosMenu.RemoveButtonEnabled = false;
            this.btnsEditAddRemovePosMenu.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenu_EditButtonClicked);
            this.btnsEditAddRemovePosMenu.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenu_AddButtonClicked);
            this.btnsEditAddRemovePosMenu.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenu_RemoveButtonClicked);
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.btnMoveDown);
            this.groupPanel1.Controls.Add(this.btnMoveUp);
            this.groupPanel1.Controls.Add(this.btnsEditAddRemovePosMenuLine);
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.lvKitchenDisplayMenuLines);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
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
            // btnsEditAddRemovePosMenuLine
            // 
            this.btnsEditAddRemovePosMenuLine.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemovePosMenuLine, "btnsEditAddRemovePosMenuLine");
            this.btnsEditAddRemovePosMenuLine.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemovePosMenuLine.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemovePosMenuLine.EditButtonEnabled = true;
            this.btnsEditAddRemovePosMenuLine.Name = "btnsEditAddRemovePosMenuLine";
            this.btnsEditAddRemovePosMenuLine.RemoveButtonEnabled = true;
            this.btnsEditAddRemovePosMenuLine.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenuLine_EditButtonClicked);
            this.btnsEditAddRemovePosMenuLine.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenuLine_AddButtonClicked);
            this.btnsEditAddRemovePosMenuLine.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenuLine_RemoveButtonClicked);
            // 
            // lblGroupHeader
            // 
            resources.ApplyResources(this.lblGroupHeader, "lblGroupHeader");
            this.lblGroupHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupHeader.Name = "lblGroupHeader";
            // 
            // lvKitchenDisplayMenuLines
            // 
            resources.ApplyResources(this.lvKitchenDisplayMenuLines, "lvKitchenDisplayMenuLines");
            this.lvKitchenDisplayMenuLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvKitchenDisplayMenuLines.FullRowSelect = true;
            this.lvKitchenDisplayMenuLines.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvKitchenDisplayMenuLines.HideSelection = false;
            this.lvKitchenDisplayMenuLines.LockDrawing = false;
            this.lvKitchenDisplayMenuLines.Name = "lvKitchenDisplayMenuLines";
            this.lvKitchenDisplayMenuLines.SortColumn = -1;
            this.lvKitchenDisplayMenuLines.SortedBackwards = false;
            this.lvKitchenDisplayMenuLines.UseCompatibleStateImageBehavior = false;
            this.lvKitchenDisplayMenuLines.UseEveryOtherRowColoring = false;
            this.lvKitchenDisplayMenuLines.View = System.Windows.Forms.View.Details;
            this.lvKitchenDisplayMenuLines.SelectedIndexChanged += new System.EventHandler(this.lvKitchenDisplayMenuLines_SelectedIndexChanged);
            this.lvKitchenDisplayMenuLines.DoubleClick += new System.EventHandler(this.lvKitchenDisplayMenuLines_DoubleClick);
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // ButtonProfilesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ButtonProfilesView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvKitchenDisplayMenuHeaders;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private ContextButtons btnsEditAddRemovePosMenu;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblGroupHeader;
        private ExtendedListView lvKitchenDisplayMenuLines;
        private System.Windows.Forms.Label lblNoSelection;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private ContextButtons btnsEditAddRemovePosMenuLine;
        private ContextButton btnMoveDown;
        private ContextButton btnMoveUp;
    }
}