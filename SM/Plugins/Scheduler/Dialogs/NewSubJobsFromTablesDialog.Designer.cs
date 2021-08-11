using LSOne.Controls;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class NewSubJobsFromTablesDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewSubJobsFromTablesDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.grpOptions = new LSOne.Controls.GroupPanel();
            this.lblActionTable = new System.Windows.Forms.Label();
            this.cmbActionTable = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.tbNamePrefix = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRepliactionMethod = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableDesignTree = new LSOne.ViewPlugins.Scheduler.Controls.TableDesignTree();
            this.panel2.SuspendLayout();
            this.grpOptions.SuspendLayout();
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
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            // 
            // grpOptions
            // 
            resources.ApplyResources(this.grpOptions, "grpOptions");
            this.grpOptions.Controls.Add(this.lblActionTable);
            this.grpOptions.Controls.Add(this.cmbActionTable);
            this.grpOptions.Controls.Add(this.tbNamePrefix);
            this.grpOptions.Controls.Add(this.label1);
            this.grpOptions.Controls.Add(this.cmbRepliactionMethod);
            this.grpOptions.Controls.Add(this.label2);
            this.grpOptions.Name = "grpOptions";
            // 
            // lblActionTable
            // 
            this.lblActionTable.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblActionTable, "lblActionTable");
            this.lblActionTable.Name = "lblActionTable";
            // 
            // cmbActionTable
            // 
            this.cmbActionTable.AddList = null;
            resources.ApplyResources(this.cmbActionTable, "cmbActionTable");
            this.cmbActionTable.MaxLength = 32767;
            this.cmbActionTable.Name = "cmbActionTable";
            this.cmbActionTable.RemoveList = null;
            this.cmbActionTable.RowHeight = ((short)(22));
            this.cmbActionTable.SecondaryData = null;
            this.cmbActionTable.SelectedData = null;
            this.cmbActionTable.SelectionList = null;
            this.cmbActionTable.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbActionTable_DropDown);
            this.cmbActionTable.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbActionTable_FormatData);
            this.cmbActionTable.SelectedDataChanged += new System.EventHandler(this.cmbActionTable_SelectedDataChanged);
            // 
            // tbNamePrefix
            // 
            resources.ApplyResources(this.tbNamePrefix, "tbNamePrefix");
            this.tbNamePrefix.Name = "tbNamePrefix";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbRepliactionMethod
            // 
            this.cmbRepliactionMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRepliactionMethod.FormattingEnabled = true;
            resources.ApplyResources(this.cmbRepliactionMethod, "cmbRepliactionMethod");
            this.cmbRepliactionMethod.Name = "cmbRepliactionMethod";
            this.cmbRepliactionMethod.SelectedIndexChanged += new System.EventHandler(this.cmbRepliactionMethod_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tableDesignTree
            // 
            resources.ApplyResources(this.tableDesignTree, "tableDesignTree");
            this.tableDesignTree.CheckBoxes = false;
            this.tableDesignTree.Name = "tableDesignTree";
            this.tableDesignTree.ItemCheckedChanged += new System.EventHandler<LSOne.ViewPlugins.Scheduler.Controls.ItemCheckChangedEventArgs>(this.tableDesignTree_ItemCheckedChanged);
            // 
            // NewSubJobsFromTablesDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.tableDesignTree);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.HasHelp = true;
            this.Name = "NewSubJobsFromTablesDialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewSubJobsFromTablesDialog_FormClosed);
            this.Shown += new System.EventHandler(this.NewSubJobsFromTablesDialog_Shown);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.tableDesignTree, 0);
            this.Controls.SetChildIndex(this.progressBar, 0);
            this.Controls.SetChildIndex(this.grpOptions, 0);
            this.panel2.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.TableDesignTree tableDesignTree;
        private System.Windows.Forms.ProgressBar progressBar;
        private GroupPanel grpOptions;
        private System.Windows.Forms.TextBox tbNamePrefix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbRepliactionMethod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblActionTable;
        private DropDownFormComboBox cmbActionTable;
    }
}