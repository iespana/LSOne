using LSOne.Controls;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    partial class DatabaseDesignView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseDesignView));
            this.splMain = new System.Windows.Forms.SplitContainer();
            this.databaseDesignTree = new LSOne.ViewPlugins.Scheduler.Controls.DatabaseDesignTree();
            this.grpTreeFilter = new LSOne.Controls.GroupPanel();
            this.chkIncludeDisabled = new System.Windows.Forms.CheckBox();
            this.chkFilterOnlyLinked = new System.Windows.Forms.CheckBox();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnsAddRemove = new LSOne.Controls.ContextButtons();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).BeginInit();
            this.splMain.Panel1.SuspendLayout();
            this.splMain.SuspendLayout();
            this.grpTreeFilter.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.splMain);
            // 
            // splMain
            // 
            resources.ApplyResources(this.splMain, "splMain");
            this.splMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splMain.Name = "splMain";
            // 
            // splMain.Panel1
            // 
            this.splMain.Panel1.Controls.Add(this.databaseDesignTree);
            this.splMain.Panel1.Controls.Add(this.grpTreeFilter);
            this.splMain.Panel1.Controls.Add(this.pnlActions);
            // 
            // databaseDesignTree
            // 
            resources.ApplyResources(this.databaseDesignTree, "databaseDesignTree");
            this.databaseDesignTree.ExpandIfSingleRoot = false;
            this.databaseDesignTree.FilterIncludeDisabled = false;
            this.databaseDesignTree.FilterOnlyLinked = false;
            this.databaseDesignTree.FilterTableName = null;
            this.databaseDesignTree.Name = "databaseDesignTree";
            this.databaseDesignTree.SelectNodeOnExpand = false;
            this.databaseDesignTree.SelectedItemChanging += new System.EventHandler<System.ComponentModel.CancelEventArgs>(this.databaseDesignTree_SelectedItemChanging);
            this.databaseDesignTree.SelectedItemChanged += new System.EventHandler<System.EventArgs>(this.databaseDesignTree_SelectedItemChanged);
            // 
            // grpTreeFilter
            // 
            this.grpTreeFilter.Controls.Add(this.chkIncludeDisabled);
            this.grpTreeFilter.Controls.Add(this.chkFilterOnlyLinked);
            resources.ApplyResources(this.grpTreeFilter, "grpTreeFilter");
            this.grpTreeFilter.Name = "grpTreeFilter";
            // 
            // chkIncludeDisabled
            // 
            resources.ApplyResources(this.chkIncludeDisabled, "chkIncludeDisabled");
            this.chkIncludeDisabled.BackColor = System.Drawing.Color.Transparent;
            this.chkIncludeDisabled.Name = "chkIncludeDisabled";
            this.chkIncludeDisabled.UseVisualStyleBackColor = false;
            this.chkIncludeDisabled.CheckedChanged += new System.EventHandler(this.chkIncludeDisabled_CheckedChanged);
            // 
            // chkFilterOnlyLinked
            // 
            resources.ApplyResources(this.chkFilterOnlyLinked, "chkFilterOnlyLinked");
            this.chkFilterOnlyLinked.BackColor = System.Drawing.Color.Transparent;
            this.chkFilterOnlyLinked.Name = "chkFilterOnlyLinked";
            this.chkFilterOnlyLinked.UseVisualStyleBackColor = false;
            this.chkFilterOnlyLinked.CheckedChanged += new System.EventHandler(this.chkFilterOnlyLinked_CheckedChanged);
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnsAddRemove);
            resources.ApplyResources(this.pnlActions, "pnlActions");
            this.pnlActions.Name = "pnlActions";
            // 
            // btnsAddRemove
            // 
            this.btnsAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsAddRemove, "btnsAddRemove");
            this.btnsAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsAddRemove.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.btnsAddRemove.EditButtonEnabled = false;
            this.btnsAddRemove.Name = "btnsAddRemove";
            this.btnsAddRemove.RemoveButtonEnabled = false;
            this.btnsAddRemove.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // DatabaseDesignView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "DatabaseDesignView";
            this.pnlBottom.ResumeLayout(false);
            this.splMain.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).EndInit();
            this.splMain.ResumeLayout(false);
            this.grpTreeFilter.ResumeLayout(false);
            this.grpTreeFilter.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splMain;
        private Controls.DatabaseDesignTree databaseDesignTree;
        private System.Windows.Forms.Panel pnlActions;
        private GroupPanel grpTreeFilter;
        private System.Windows.Forms.CheckBox chkFilterOnlyLinked;
        private System.Windows.Forms.CheckBox chkIncludeDisabled;
        private ContextButtons btnsAddRemove;
    }
}
