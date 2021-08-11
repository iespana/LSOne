namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    partial class StationPrintingDebug
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StationPrintingDebug));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.xLvlError = new System.Windows.Forms.CheckBox();
            this.xLvlDetail = new System.Windows.Forms.CheckBox();
            this.xLvlMain = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lstDebug = new System.Windows.Forms.ListView();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.xLvlError);
            this.panel2.Controls.Add(this.xLvlDetail);
            this.panel2.Controls.Add(this.xLvlMain);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // xLvlError
            // 
            resources.ApplyResources(this.xLvlError, "xLvlError");
            this.xLvlError.Checked = true;
            this.xLvlError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xLvlError.Name = "xLvlError";
            this.xLvlError.UseVisualStyleBackColor = true;
            this.xLvlError.CheckedChanged += new System.EventHandler(this.xLvl_CheckedChanged);
            // 
            // xLvlDetail
            // 
            resources.ApplyResources(this.xLvlDetail, "xLvlDetail");
            this.xLvlDetail.Name = "xLvlDetail";
            this.xLvlDetail.UseVisualStyleBackColor = true;
            this.xLvlDetail.CheckedChanged += new System.EventHandler(this.xLvl_CheckedChanged);
            // 
            // xLvlMain
            // 
            resources.ApplyResources(this.xLvlMain, "xLvlMain");
            this.xLvlMain.Checked = true;
            this.xLvlMain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xLvlMain.Name = "xLvlMain";
            this.xLvlMain.UseVisualStyleBackColor = true;
            this.xLvlMain.CheckedChanged += new System.EventHandler(this.xLvl_CheckedChanged);
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
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            // 
            // lstDebug
            // 
            this.lstDebug.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            resources.ApplyResources(this.lstDebug, "lstDebug");
            this.lstDebug.Name = "lstDebug";
            this.lstDebug.UseCompatibleStateImageBehavior = false;
            this.lstDebug.View = System.Windows.Forms.View.Details;
            this.lstDebug.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstDebug_KeyDown);
            // 
            // StationPrintingDebug
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lstDebug);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.HasHelp = true;
            this.Name = "StationPrintingDebug";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StationPrintingDebug_FormClosing);
            this.Resize += new System.EventHandler(this.StationPrintingDebug_Resize);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lstDebug, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox xLvlError;
        private System.Windows.Forms.CheckBox xLvlDetail;
        private System.Windows.Forms.CheckBox xLvlMain;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ListView lstDebug;
    }
}