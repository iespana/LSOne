using LSOne.Controls;

namespace LSOne.ViewPlugins.CustomerContacts.ViewPages
{
    partial class CustomerContactsPage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerContactsPage));
            this.lvContacts = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnsEditAddRemove = new ContextButtons();
            this.SuspendLayout();
            // 
            // lvContacts
            // 
            resources.ApplyResources(this.lvContacts, "lvContacts");
            this.lvContacts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvContacts.FullRowSelect = true;
            this.lvContacts.HideSelection = false;
            this.lvContacts.LockDrawing = false;
            this.lvContacts.Name = "lvContacts";
            this.lvContacts.SmallImageList = this.imageList1;
            this.lvContacts.SortColumn = -1;
            this.lvContacts.SortedBackwards = false;
            this.lvContacts.UseCompatibleStateImageBehavior = false;
            this.lvContacts.UseEveryOtherRowColoring = true;
            this.lvContacts.View = System.Windows.Forms.View.Details;
            this.lvContacts.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvContacts_ColumnClick);
            this.lvContacts.SelectedIndexChanged += new System.EventHandler(this.lvContacts_SelectedIndexChanged);
            this.lvContacts.DoubleClick += new System.EventHandler(this.lvContacts_DoubleClick);
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
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.imageList1, "imageList1");
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
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
            // CustomerContactsPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnsEditAddRemove);
            this.Controls.Add(this.lvContacts);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "CustomerContactsPage";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvContacts;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private ContextButtons btnsEditAddRemove;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ImageList imageList1;



    }
}
