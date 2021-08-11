using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class TableDesignSelectorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableDesignSelectorDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.databaseTableDesignSelector1 = new DatabaseTableDesignSelector();
            this.panel2.SuspendLayout();
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
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // databaseTableDesignSelector1
            // 
            this.databaseTableDesignSelector1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.databaseTableDesignSelector1, "databaseTableDesignSelector1");
            this.databaseTableDesignSelector1.Name = "databaseTableDesignSelector1";
            this.databaseTableDesignSelector1.SelectedDatabaseDesign = null;
            this.databaseTableDesignSelector1.SelectedDatabaseDesignId = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.databaseTableDesignSelector1.SelectedTableDesign = null;
            this.databaseTableDesignSelector1.SelectedTableDesignId = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.databaseTableDesignSelector1.TableSelectionChanged += new System.EventHandler<System.EventArgs>(this.databaseTableDesignSelector1_TableSelectionChanged);
            // 
            // TableDesignSelectorDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.databaseTableDesignSelector1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TableDesignSelectorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TableDesignSelectorDialog_FormClosed);
            this.Shown += new System.EventHandler(this.TableDesignSelectorDialog_Shown);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.DatabaseTableDesignSelector databaseTableDesignSelector1;
    }
}