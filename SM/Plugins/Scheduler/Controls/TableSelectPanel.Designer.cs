using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class TableSelectPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableSelectPanel));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupPanel1 = new GroupPanel();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.databaseDesignTree = new DatabaseDesignTree();
            this.panel2.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnOK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnOK_KeyPress);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.tbFilter);
            this.groupPanel1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // tbFilter
            // 
            resources.ApplyResources(this.tbFilter, "tbFilter");
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.TextChanged += new System.EventHandler(this.tbFilter_TextChanged);
            this.tbFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbFilter_KeyPress);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // databaseDesignTree
            // 
            resources.ApplyResources(this.databaseDesignTree, "databaseDesignTree");
            this.databaseDesignTree.ExpandIfSingleRoot = true;
            this.databaseDesignTree.FilterIncludeDisabled = false;
            this.databaseDesignTree.FilterLevel = DatabaseDesignTree.FilterLevelType.DatabasesAndTables;
            this.databaseDesignTree.FilterOnlyLinked = false;
            this.databaseDesignTree.FilterTableName = null;
            this.databaseDesignTree.Name = "databaseDesignTree";
            this.databaseDesignTree.SelectNodeOnExpand = true;
            this.databaseDesignTree.SelectedItemChanged += new System.EventHandler<System.EventArgs>(this.databaseDesignTree_SelectedItemChanged);
            this.databaseDesignTree.ItemDoubleClicked += new System.EventHandler<System.EventArgs>(this.databaseDesignTree_ItemDoubleClicked);
            this.databaseDesignTree.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.databaseDesignTree_KeyPress);
            // 
            // TableSelectPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.databaseDesignTree);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.panel2);
            this.Name = "TableSelectPanel";
            this.Load += new System.EventHandler(this.TableSelectPanel_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TableSelectPanel_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TableSelectPanel_KeyPress);
            this.panel2.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DatabaseDesignTree databaseDesignTree;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.TextBox tbFilter;
        private System.Windows.Forms.Label label1;
    }
}
