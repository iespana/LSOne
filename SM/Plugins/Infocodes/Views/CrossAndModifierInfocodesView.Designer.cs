using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.Views
{
    partial class CrossAndModifierInfocodesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrossAndModifierInfocodesView));
            this.lvGroups = new LSOne.Controls.ExtendedListView();
            this.colInfocode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colUsageCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colExplHeaderText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTriggering = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOKPressedAction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMinSelection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMaxSelection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLinkedInfocode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvGroupSubcodes = new LSOne.Controls.ExtendedListView();
            this.colTriggerCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colListType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDescr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInfocodePrompt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.btnsContextButtonsItems = new LSOne.Controls.ContextButtons();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbInfocodeGroupType = new System.Windows.Forms.ComboBox();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.btnViewTargetInfocode = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.cmbInfocodeGroupType);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            this.pnlBottom.Controls.Add(this.lvGroups);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // lvGroups
            // 
            resources.ApplyResources(this.lvGroups, "lvGroups");
            this.lvGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colInfocode,
            this.colUsageCategory,
            this.colDescription,
            this.colExplHeaderText,
            this.colTriggering,
            this.colOKPressedAction,
            this.colMinSelection,
            this.colMaxSelection,
            this.colLinkedInfocode});
            this.lvGroups.FullRowSelect = true;
            this.lvGroups.HideSelection = false;
            this.lvGroups.LockDrawing = false;
            this.lvGroups.MultiSelect = false;
            this.lvGroups.Name = "lvGroups";
            this.lvGroups.SortColumn = 0;
            this.lvGroups.SortedBackwards = false;
            this.lvGroups.UseCompatibleStateImageBehavior = false;
            this.lvGroups.UseEveryOtherRowColoring = true;
            this.lvGroups.View = System.Windows.Forms.View.Details;
            this.lvGroups.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGroups_ColumnClick);
            this.lvGroups.SelectedIndexChanged += new System.EventHandler(this.lvGroups_SelectedIndexChanged);
            this.lvGroups.DoubleClick += new System.EventHandler(this.lvGroups_DoubleClick);
            // 
            // colInfocode
            // 
            resources.ApplyResources(this.colInfocode, "colInfocode");
            // 
            // colUsageCategory
            // 
            resources.ApplyResources(this.colUsageCategory, "colUsageCategory");
            // 
            // colDescription
            // 
            resources.ApplyResources(this.colDescription, "colDescription");
            // 
            // colExplHeaderText
            // 
            resources.ApplyResources(this.colExplHeaderText, "colExplHeaderText");
            // 
            // colTriggering
            // 
            resources.ApplyResources(this.colTriggering, "colTriggering");
            // 
            // colOKPressedAction
            // 
            resources.ApplyResources(this.colOKPressedAction, "colOKPressedAction");
            // 
            // colMinSelection
            // 
            resources.ApplyResources(this.colMinSelection, "colMinSelection");
            // 
            // colMaxSelection
            // 
            resources.ApplyResources(this.colMaxSelection, "colMaxSelection");
            // 
            // colLinkedInfocode
            // 
            resources.ApplyResources(this.colLinkedInfocode, "colLinkedInfocode");
            // 
            // lvGroupSubcodes
            // 
            resources.ApplyResources(this.lvGroupSubcodes, "lvGroupSubcodes");
            this.lvGroupSubcodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTriggerCode,
            this.colListType,
            this.colDescr,
            this.colInfocodePrompt});
            this.lvGroupSubcodes.FullRowSelect = true;
            this.lvGroupSubcodes.HideSelection = false;
            this.lvGroupSubcodes.LockDrawing = false;
            this.lvGroupSubcodes.MultiSelect = false;
            this.lvGroupSubcodes.Name = "lvGroupSubcodes";
            this.lvGroupSubcodes.SortColumn = 2;
            this.lvGroupSubcodes.SortedBackwards = false;
            this.lvGroupSubcodes.UseCompatibleStateImageBehavior = false;
            this.lvGroupSubcodes.UseEveryOtherRowColoring = true;
            this.lvGroupSubcodes.View = System.Windows.Forms.View.Details;
            this.lvGroupSubcodes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGroupSubcodes_ColumnClick);
            this.lvGroupSubcodes.SelectedIndexChanged += new System.EventHandler(this.lvGroupSubcodes_SelectedIndexChanged);
            this.lvGroupSubcodes.Click += new System.EventHandler(this.lvGroupSubcodes_Click);
            // 
            // colTriggerCode
            // 
            resources.ApplyResources(this.colTriggerCode, "colTriggerCode");
            // 
            // colListType
            // 
            resources.ApplyResources(this.colListType, "colListType");
            // 
            // colDescr
            // 
            resources.ApplyResources(this.colDescr, "colDescr");
            // 
            // colInfocodePrompt
            // 
            resources.ApplyResources(this.colInfocodePrompt, "colInfocodePrompt");
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.btnViewTargetInfocode);
            this.groupPanel1.Controls.Add(this.btnsContextButtonsItems);
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.lvGroupSubcodes);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // btnsContextButtonsItems
            // 
            this.btnsContextButtonsItems.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtonsItems, "btnsContextButtonsItems");
            this.btnsContextButtonsItems.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtonsItems.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.btnsContextButtonsItems.EditButtonEnabled = false;
            this.btnsContextButtonsItems.Name = "btnsContextButtonsItems";
            this.btnsContextButtonsItems.RemoveButtonEnabled = false;
            this.btnsContextButtonsItems.AddButtonClicked += new System.EventHandler(this.btnAddItem_Click);
            this.btnsContextButtonsItems.RemoveButtonClicked += new System.EventHandler(this.btnRemoveItem_Click);
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
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbInfocodeGroupType
            // 
            this.cmbInfocodeGroupType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInfocodeGroupType.FormattingEnabled = true;
            this.cmbInfocodeGroupType.Items.AddRange(new object[] {
            resources.GetString("cmbInfocodeGroupType.Items"),
            resources.GetString("cmbInfocodeGroupType.Items1"),
            resources.GetString("cmbInfocodeGroupType.Items2")});
            resources.ApplyResources(this.cmbInfocodeGroupType, "cmbInfocodeGroupType");
            this.cmbInfocodeGroupType.Name = "cmbInfocodeGroupType";
            this.cmbInfocodeGroupType.SelectedIndexChanged += new System.EventHandler(this.cmbInfocodeGroupType_SelectedIndexChanged);
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
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnViewTargetInfocode
            // 
            resources.ApplyResources(this.btnViewTargetInfocode, "btnViewTargetInfocode");
            this.btnViewTargetInfocode.Name = "btnViewTargetInfocode";
            this.btnViewTargetInfocode.UseVisualStyleBackColor = true;
            this.btnViewTargetInfocode.Click += new System.EventHandler(this.btnViewTargetInfocode_Click);
            // 
            // CrossAndModifierInfocodesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 40;
            this.Name = "CrossAndModifierInfocodesView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvGroups;
        private System.Windows.Forms.ColumnHeader colInfocode;
        private System.Windows.Forms.ColumnHeader colDescription;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblGroupHeader;
        private ExtendedListView lvGroupSubcodes;
        private System.Windows.Forms.ColumnHeader colDescr;
        private System.Windows.Forms.ColumnHeader colInfocodePrompt;
        private System.Windows.Forms.ColumnHeader colTriggerCode;
        private System.Windows.Forms.Label lblNoSelection;
        private System.Windows.Forms.ColumnHeader colExplHeaderText;
        private System.Windows.Forms.ColumnHeader colTriggering;
        private System.Windows.Forms.ColumnHeader colOKPressedAction;
        private System.Windows.Forms.ColumnHeader colMinSelection;
        private System.Windows.Forms.ColumnHeader colMaxSelection;
        private System.Windows.Forms.ColumnHeader colLinkedInfocode;
        private System.Windows.Forms.ColumnHeader colListType;
        private System.Windows.Forms.ColumnHeader colUsageCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbInfocodeGroupType;
        private ContextButtons btnsContextButtons;
        private ContextButtons btnsContextButtonsItems;
        private System.Windows.Forms.Button btnViewTargetInfocode;
    }
}
