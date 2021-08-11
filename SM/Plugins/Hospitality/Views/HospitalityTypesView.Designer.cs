using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    partial class HospitalityTypesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityTypesView));
            this.lvHospitalityTypes = new LSOne.Controls.ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            this.pnlBottom.Controls.Add(this.lvHospitalityTypes);
            // 
            // lvHospitalityTypes
            // 
            resources.ApplyResources(this.lvHospitalityTypes, "lvHospitalityTypes");
            this.lvHospitalityTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader4});
            this.lvHospitalityTypes.FullRowSelect = true;
            this.lvHospitalityTypes.HideSelection = false;
            this.lvHospitalityTypes.LockDrawing = false;
            this.lvHospitalityTypes.MultiSelect = false;
            this.lvHospitalityTypes.Name = "lvHospitalityTypes";
            this.lvHospitalityTypes.SortColumn = -1;
            this.lvHospitalityTypes.SortedBackwards = false;
            this.lvHospitalityTypes.UseCompatibleStateImageBehavior = false;
            this.lvHospitalityTypes.UseEveryOtherRowColoring = true;
            this.lvHospitalityTypes.View = System.Windows.Forms.View.Details;
            this.lvHospitalityTypes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvHospitalityTypes_ColumnClick);
            this.lvHospitalityTypes.SelectedIndexChanged += new System.EventHandler(this.lvHospitalityTypes_SelectedIndexChanged);
            this.lvHospitalityTypes.DoubleClick += new System.EventHandler(this.lvHospitalityTypes_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // HospitalityTypesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HospitalityTypesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvHospitalityTypes;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private ContextButtons btnsEditAddRemove;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}
