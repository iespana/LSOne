using LSOne.Controls;

namespace LSOne.ViewPlugins.Administration.Dialogs
{
    partial class InsertDefaultDataDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsertDefaultDataDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lvScripts = new LSOne.Controls.ListView();
            this.clmCheck = new LSOne.Controls.Columns.Column();
            this.clmScript = new LSOne.Controls.Columns.Column();
            this.clmSystem = new LSOne.Controls.Columns.Column();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.lblProgress);
            this.panel2.Controls.Add(this.progressBar);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // lblProgress
            // 
            resources.ApplyResources(this.lblProgress, "lblProgress");
            this.lblProgress.Name = "lblProgress";
            // 
            // progressBar
            // 
            this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(189)))), ((int)(((byte)(139)))));
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
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
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lvScripts
            // 
            resources.ApplyResources(this.lvScripts, "lvScripts");
            this.lvScripts.BuddyControl = null;
            this.lvScripts.Columns.Add(this.clmCheck);
            this.lvScripts.Columns.Add(this.clmScript);
            this.lvScripts.Columns.Add(this.clmSystem);
            this.lvScripts.ContentBackColor = System.Drawing.Color.White;
            this.lvScripts.DefaultRowHeight = ((short)(22));
            this.lvScripts.DimSelectionWhenDisabled = true;
            this.lvScripts.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvScripts.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvScripts.HeaderHeight = ((short)(25));
            this.lvScripts.Name = "lvScripts";
            this.lvScripts.OddRowColor = System.Drawing.Color.White;
            this.lvScripts.RowLineColor = System.Drawing.Color.LightGray;
            this.lvScripts.SecondarySortColumn = ((short)(-1));
            this.lvScripts.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvScripts.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvScripts.SortSetting = "0:1";
            this.lvScripts.CellAction += new LSOne.Controls.CellActionDelegate(this.lvScripts_CellAction);
            // 
            // clmCheck
            // 
            this.clmCheck.AutoSize = true;
            this.clmCheck.Clickable = false;
            this.clmCheck.DefaultStyle = null;
            resources.ApplyResources(this.clmCheck, "clmCheck");
            this.clmCheck.MaximumWidth = ((short)(0));
            this.clmCheck.MinimumWidth = ((short)(10));
            this.clmCheck.SecondarySortColumn = ((short)(-1));
            this.clmCheck.Tag = null;
            this.clmCheck.Width = ((short)(50));
            // 
            // clmScript
            // 
            this.clmScript.AutoSize = true;
            this.clmScript.DefaultStyle = null;
            resources.ApplyResources(this.clmScript, "clmScript");
            this.clmScript.InternalSort = true;
            this.clmScript.MaximumWidth = ((short)(0));
            this.clmScript.MinimumWidth = ((short)(10));
            this.clmScript.SecondarySortColumn = ((short)(-1));
            this.clmScript.Tag = null;
            this.clmScript.Width = ((short)(300));
            // 
            // clmSystem
            // 
            this.clmSystem.AutoSize = true;
            this.clmSystem.Clickable = false;
            this.clmSystem.DefaultStyle = null;
            resources.ApplyResources(this.clmSystem, "clmSystem");
            this.clmSystem.MaximumWidth = ((short)(0));
            this.clmSystem.MinimumWidth = ((short)(10));
            this.clmSystem.SecondarySortColumn = ((short)(-1));
            this.clmSystem.Tag = null;
            this.clmSystem.Width = ((short)(100));
            // 
            // InsertDefaultDataDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lvScripts);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "InsertDefaultDataDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lvScripts, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private ListView lvScripts;
        private Controls.Columns.Column clmCheck;
        private Controls.Columns.Column clmScript;
        private Controls.Columns.Column clmSystem;
    }
}