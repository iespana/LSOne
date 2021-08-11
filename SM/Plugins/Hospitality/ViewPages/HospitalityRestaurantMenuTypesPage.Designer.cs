using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityRestaurantMenuTypesPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityRestaurantMenuTypesPage));
            this.lvRestaurantMenuTypes = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsEditAddRemove = new ContextButtons();
            this.SuspendLayout();
            // 
            // lvRestaurantMenuTypes
            // 
            resources.ApplyResources(this.lvRestaurantMenuTypes, "lvRestaurantMenuTypes");
            this.lvRestaurantMenuTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvRestaurantMenuTypes.FullRowSelect = true;
            this.lvRestaurantMenuTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvRestaurantMenuTypes.HideSelection = false;
            this.lvRestaurantMenuTypes.LockDrawing = false;
            this.lvRestaurantMenuTypes.MultiSelect = false;
            this.lvRestaurantMenuTypes.Name = "lvRestaurantMenuTypes";
            this.lvRestaurantMenuTypes.SortColumn = -1;
            this.lvRestaurantMenuTypes.SortedBackwards = false;
            this.lvRestaurantMenuTypes.UseCompatibleStateImageBehavior = false;
            this.lvRestaurantMenuTypes.UseEveryOtherRowColoring = true;
            this.lvRestaurantMenuTypes.View = System.Windows.Forms.View.Details;
            this.lvRestaurantMenuTypes.SelectedIndexChanged += new System.EventHandler(this.lvRestaurantMenuTypes_SelectedIndexChanged);
            this.lvRestaurantMenuTypes.DoubleClick += new System.EventHandler(this.lvRestaurantMenuTypes_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // HospitalityRestaurantMenuTypesPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnsEditAddRemove);
            this.Controls.Add(this.lvRestaurantMenuTypes);
            this.DoubleBuffered = true;
            this.Name = "HospitalityRestaurantMenuTypesPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvRestaurantMenuTypes;
        private ContextButtons btnsEditAddRemove;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
