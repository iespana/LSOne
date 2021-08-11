namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class RemoveActionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoveActionDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCounterCaption = new System.Windows.Forms.Label();
            this.tbCounterReference = new System.Windows.Forms.TextBox();
            this.rbThisCounter = new System.Windows.Forms.RadioButton();
            this.rbOlderCounters = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
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
            // lblCounterCaption
            // 
            resources.ApplyResources(this.lblCounterCaption, "lblCounterCaption");
            this.lblCounterCaption.Name = "lblCounterCaption";
            // 
            // tbCounterReference
            // 
            resources.ApplyResources(this.tbCounterReference, "tbCounterReference");
            this.tbCounterReference.Name = "tbCounterReference";
            // 
            // rbThisCounter
            // 
            resources.ApplyResources(this.rbThisCounter, "rbThisCounter");
            this.rbThisCounter.Name = "rbThisCounter";
            this.rbThisCounter.TabStop = true;
            this.rbThisCounter.UseVisualStyleBackColor = true;
            this.rbThisCounter.CheckedChanged += new System.EventHandler(this.rbThisCounter_CheckedChanged);
            // 
            // rbOlderCounters
            // 
            resources.ApplyResources(this.rbOlderCounters, "rbOlderCounters");
            this.rbOlderCounters.Name = "rbOlderCounters";
            this.rbOlderCounters.TabStop = true;
            this.rbOlderCounters.UseVisualStyleBackColor = true;
            this.rbOlderCounters.CheckedChanged += new System.EventHandler(this.rbThisCounter_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // RemoveActionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbOlderCounters);
            this.Controls.Add(this.rbThisCounter);
            this.Controls.Add(this.tbCounterReference);
            this.Controls.Add(this.lblCounterCaption);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "RemoveActionDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblCounterCaption, 0);
            this.Controls.SetChildIndex(this.tbCounterReference, 0);
            this.Controls.SetChildIndex(this.rbThisCounter, 0);
            this.Controls.SetChildIndex(this.rbOlderCounters, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCounterCaption;
        private System.Windows.Forms.TextBox tbCounterReference;
        private System.Windows.Forms.RadioButton rbThisCounter;
        private System.Windows.Forms.RadioButton rbOlderCounters;
        private System.Windows.Forms.Label label1;
    }
}