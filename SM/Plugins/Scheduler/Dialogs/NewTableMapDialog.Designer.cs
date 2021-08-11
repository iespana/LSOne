using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class NewTableMapDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTableMapDialog));
            this.splTrees = new System.Windows.Forms.SplitContainer();
            this.tvDesignFrom = new TableDesignTree();
            this.label1 = new System.Windows.Forms.Label();
            this.tvDesignTo = new TableDesignTree();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splTrees)).BeginInit();
            this.splTrees.Panel1.SuspendLayout();
            this.splTrees.Panel2.SuspendLayout();
            this.splTrees.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splTrees
            // 
            resources.ApplyResources(this.splTrees, "splTrees");
            this.splTrees.Name = "splTrees";
            // 
            // splTrees.Panel1
            // 
            this.splTrees.Panel1.Controls.Add(this.tvDesignFrom);
            this.splTrees.Panel1.Controls.Add(this.label1);
            // 
            // splTrees.Panel2
            // 
            this.splTrees.Panel2.Controls.Add(this.tvDesignTo);
            this.splTrees.Panel2.Controls.Add(this.label2);
            // 
            // tvDesignFrom
            // 
            this.tvDesignFrom.CheckBoxes = false;
            resources.ApplyResources(this.tvDesignFrom, "tvDesignFrom");
            this.tvDesignFrom.Name = "tvDesignFrom";
            this.tvDesignFrom.DatabaseDesignSelected += new System.EventHandler<DatabaseDesignEventArgs>(this.tvDesignFrom_DatabaseDesignSelected);
            this.tvDesignFrom.TableDesignSelected += new System.EventHandler<TableDesignEventArgs>(this.tvDesignFrom_TableDesignSelected);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tvDesignTo
            // 
            this.tvDesignTo.CheckBoxes = false;
            resources.ApplyResources(this.tvDesignTo, "tvDesignTo");
            this.tvDesignTo.Name = "tvDesignTo";
            this.tvDesignTo.DatabaseDesignSelected += new System.EventHandler<DatabaseDesignEventArgs>(this.tvDesignTo_DatabaseDesignSelected);
            this.tvDesignTo.TableDesignSelected += new System.EventHandler<TableDesignEventArgs>(this.tvDesignTo_TableDesignSelected);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // NewTableMapDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.ControlBox = true;
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splTrees);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.HasHelp = true;
            this.MaximizeBox = true;
            this.Name = "NewTableMapDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.NewTableMapDialog_Load);
            this.Shown += new System.EventHandler(this.NewTableMapDialog_Shown);
            this.Controls.SetChildIndex(this.splTrees, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.btnSearch, 0);
            this.splTrees.Panel1.ResumeLayout(false);
            this.splTrees.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splTrees)).EndInit();
            this.splTrees.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splTrees;
        private Controls.TableDesignTree tvDesignFrom;
        private Controls.TableDesignTree tvDesignTo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
    }
}