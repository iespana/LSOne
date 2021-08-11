namespace LSRetail.StoreController.Infocodes.Views
{
    partial class ModifierGroupInfocodesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModifierGroupInfocodesView));
            this.lvGroups = new LSRetail.SharedControls.ExtendedListView();
            this.colInfocode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colExplHeaderText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTriggering = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOKPressedAction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMinSelection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMaxSelection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLinkedInfocode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvItems = new LSRetail.SharedControls.ExtendedListView();
            this.colSubcode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTriggerCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDescr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInfocodePrompt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnEditItem = new System.Windows.Forms.Button();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.groupPanel1 = new LSRetail.SharedControls.GroupPanel();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.groupPanel1);
            this.pnlBottom.Controls.Add(this.btnRemove);
            this.pnlBottom.Controls.Add(this.btnEdit);
            this.pnlBottom.Controls.Add(this.lvGroups);
            this.pnlBottom.Controls.Add(this.btnAdd);
            this.pnlBottom.Size = new System.Drawing.Size(731, 490);
            // 
            // lvGroups
            // 
            this.lvGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colInfocode,
            this.colDescription,
            this.colExplHeaderText,
            this.colTriggering,
            this.colOKPressedAction,
            this.colMinSelection,
            this.colMaxSelection,
            this.colLinkedInfocode});
            this.lvGroups.FullRowSelect = true;
            this.lvGroups.HideSelection = false;
            this.lvGroups.Location = new System.Drawing.Point(20, 3);
            this.lvGroups.LockDrawing = false;
            this.lvGroups.MultiSelect = false;
            this.lvGroups.Name = "lvGroups";
            this.lvGroups.Size = new System.Drawing.Size(505, 222);
            this.lvGroups.SortColumn = -1;
            this.lvGroups.SortedBackwards = false;
            this.lvGroups.TabIndex = 3;
            this.lvGroups.UseCompatibleStateImageBehavior = false;
            this.lvGroups.UseEveryOtherRowColoring = true;
            this.lvGroups.View = System.Windows.Forms.View.Details;
            this.lvGroups.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGroups_ColumnClick);
            this.lvGroups.SelectedIndexChanged += new System.EventHandler(this.lvGroups_SelectedIndexChanged);
            this.lvGroups.VisibleChanged += new System.EventHandler(this.lvGroups_VisibleChanged);
            this.lvGroups.DoubleClick += new System.EventHandler(this.lvGroups_DoubleClick);
            // 
            // colInfocode
            // 
            this.colInfocode.Text = "Infocode";
            this.colInfocode.Width = 150;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 180;
            // 
            // colExplHeaderText
            // 
            this.colExplHeaderText.Text = "Explanatory Header Text";
            // 
            // colTriggering
            // 
            this.colTriggering.Text = "Triggering";
            // 
            // colOKPressedAction
            // 
            this.colOKPressedAction.Text = "OK Pressed Action";
            // 
            // colMinSelection
            // 
            this.colMinSelection.Text = "Min. Selection";
            // 
            // colMaxSelection
            // 
            this.colMaxSelection.Text = "Max. Selection";
            // 
            // colLinkedInfocode
            // 
            this.colLinkedInfocode.Text = "Linked Infocode";
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Enabled = false;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEdit.Location = new System.Drawing.Point(441, 231);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(24, 24);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Enabled = false;
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRemove.Location = new System.Drawing.Point(501, 231);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(24, 24);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdd.Location = new System.Drawing.Point(471, 231);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(24, 24);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvItems
            // 
            this.lvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSubcode,
            this.colTriggerCode,
            this.colDescr,
            this.colInfocodePrompt});
            this.lvItems.FullRowSelect = true;
            this.lvItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvItems.HideSelection = false;
            this.lvItems.Location = new System.Drawing.Point(7, 27);
            this.lvItems.LockDrawing = false;
            this.lvItems.MultiSelect = false;
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(491, 161);
            this.lvItems.SortColumn = -1;
            this.lvItems.SortedBackwards = false;
            this.lvItems.TabIndex = 7;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.UseEveryOtherRowColoring = true;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.Visible = false;
            this.lvItems.VisibleChanged += new System.EventHandler(this.lvItems_VisibleChanged);
            this.lvItems.DoubleClick += new System.EventHandler(this.lvItems_DoubleClick);
            // 
            // colSubcode
            // 
            this.colSubcode.Text = "Subcode";
            this.colSubcode.Width = 90;
            // 
            // colTriggerCode
            // 
            this.colTriggerCode.Text = "Infocode";
            this.colTriggerCode.Width = 160;
            // 
            // colDescr
            // 
            this.colDescr.Text = "Description";
            this.colDescr.Width = 90;
            // 
            // colInfocodePrompt
            // 
            this.colInfocodePrompt.Text = "Infocode Prompt";
            this.colInfocodePrompt.Width = 90;
            // 
            // btnEditItem
            // 
            this.btnEditItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditItem.Enabled = false;
            this.btnEditItem.Image = ((System.Drawing.Image)(resources.GetObject("btnEditItem.Image")));
            this.btnEditItem.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEditItem.Location = new System.Drawing.Point(410, 194);
            this.btnEditItem.Name = "btnEditItem";
            this.btnEditItem.Size = new System.Drawing.Size(24, 24);
            this.btnEditItem.TabIndex = 6;
            this.btnEditItem.UseVisualStyleBackColor = true;
            this.btnEditItem.Visible = false;
            this.btnEditItem.Click += new System.EventHandler(this.btnEditItem_Click);
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveItem.Enabled = false;
            this.btnRemoveItem.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveItem.Image")));
            this.btnRemoveItem.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRemoveItem.Location = new System.Drawing.Point(470, 194);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(24, 24);
            this.btnRemoveItem.TabIndex = 8;
            this.btnRemoveItem.UseVisualStyleBackColor = true;
            this.btnRemoveItem.Visible = false;
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddItem.Image = ((System.Drawing.Image)(resources.GetObject("btnAddItem.Image")));
            this.btnAddItem.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddItem.Location = new System.Drawing.Point(440, 194);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(24, 24);
            this.btnAddItem.TabIndex = 7;
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Visible = false;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.lvItems);
            this.groupPanel1.Controls.Add(this.btnEditItem);
            this.groupPanel1.Controls.Add(this.btnRemoveItem);
            this.groupPanel1.Controls.Add(this.btnAddItem);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Location = new System.Drawing.Point(20, 261);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(505, 225);
            this.groupPanel1.TabIndex = 9;
            // 
            // lblGroupHeader
            // 
            this.lblGroupHeader.AutoSize = true;
            this.lblGroupHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblGroupHeader.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblGroupHeader.Location = new System.Drawing.Point(11, 7);
            this.lblGroupHeader.Name = "lblGroupHeader";
            this.lblGroupHeader.Size = new System.Drawing.Size(63, 13);
            this.lblGroupHeader.TabIndex = 9;
            this.lblGroupHeader.Text = "Subcodes";
            this.lblGroupHeader.Visible = false;
            // 
            // lblNoSelection
            // 
            this.lblNoSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblNoSelection.Location = new System.Drawing.Point(7, 7);
            this.lblNoSelection.Name = "lblNoSelection";
            this.lblNoSelection.Size = new System.Drawing.Size(491, 211);
            this.lblNoSelection.TabIndex = 10;
            this.lblNoSelection.Text = "No selection";
            this.lblNoSelection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ModifierGroupInfocodesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ModifierGroupInfocodesView";
            this.Size = new System.Drawing.Size(734, 518);
            this.pnlBottom.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private LSRetail.SharedControls.ExtendedListView lvGroups;
        private System.Windows.Forms.ColumnHeader colInfocode;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private LSRetail.SharedControls.GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblGroupHeader;
        private LSRetail.SharedControls.ExtendedListView lvItems;
        private System.Windows.Forms.ColumnHeader colSubcode;
        private System.Windows.Forms.ColumnHeader colDescr;
        private System.Windows.Forms.ColumnHeader colInfocodePrompt;
        private System.Windows.Forms.ColumnHeader colTriggerCode;
        private System.Windows.Forms.Button btnEditItem;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Label lblNoSelection;
        private System.Windows.Forms.ColumnHeader colExplHeaderText;
        private System.Windows.Forms.ColumnHeader colTriggering;
        private System.Windows.Forms.ColumnHeader colOKPressedAction;
        private System.Windows.Forms.ColumnHeader colMinSelection;
        private System.Windows.Forms.ColumnHeader colMaxSelection;
        private System.Windows.Forms.ColumnHeader colLinkedInfocode;
    }
}
