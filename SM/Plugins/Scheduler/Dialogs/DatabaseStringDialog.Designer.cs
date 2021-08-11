using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class DatabaseStringDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseStringDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.databaseStringFieldsControl = new DatabaseStringFieldsControl();
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
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
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
            // databaseStringFieldsControl
            // 
            resources.ApplyResources(this.databaseStringFieldsControl, "databaseStringFieldsControl");
            this.databaseStringFieldsControl.BackColor = System.Drawing.Color.Transparent;
            this.databaseStringFieldsControl.DatabaseDriverTypes = null;
            this.databaseStringFieldsControl.DatabaseString = null;
            this.databaseStringFieldsControl.IsDDString = false;
            this.databaseStringFieldsControl.Name = "databaseStringFieldsControl";
            this.databaseStringFieldsControl.DatabaseStringChanged += new System.EventHandler(this.databaseStringFieldsControl_DatabaseStringChanged);
            this.databaseStringFieldsControl.TestClicked += new System.EventHandler<DatabaseStringEventArgs>(this.databaseStringFieldsControl_TestClicked);
            // 
            // DatabaseStringDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.databaseStringFieldsControl);
            this.HasHelp = true;
            this.Name = "DatabaseStringDialog";
            this.Controls.SetChildIndex(this.databaseStringFieldsControl, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DatabaseStringFieldsControl databaseStringFieldsControl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}