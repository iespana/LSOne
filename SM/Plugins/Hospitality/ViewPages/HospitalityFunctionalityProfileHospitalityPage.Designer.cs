namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityFunctionalityProfileHospitalityPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityFunctionalityProfileHospitalityPage));
            this.chkIsHospitalityProfile = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkSkipTableView = new System.Windows.Forms.CheckBox();
            this.chkItemChangesAfterSplit = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkIsHospitalityProfile
            // 
            resources.ApplyResources(this.chkIsHospitalityProfile, "chkIsHospitalityProfile");
            this.chkIsHospitalityProfile.Name = "chkIsHospitalityProfile";
            this.chkIsHospitalityProfile.UseVisualStyleBackColor = true;
            this.chkIsHospitalityProfile.Click += new System.EventHandler(this.chkIsHospitalityProfile_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkSkipTableView
            // 
            resources.ApplyResources(this.chkSkipTableView, "chkSkipTableView");
            this.chkSkipTableView.Name = "chkSkipTableView";
            this.chkSkipTableView.UseVisualStyleBackColor = true;
            // 
            // chkItemChangesAfterSplit
            // 
            resources.ApplyResources(this.chkItemChangesAfterSplit, "chkItemChangesAfterSplit");
            this.chkItemChangesAfterSplit.Name = "chkItemChangesAfterSplit";
            this.chkItemChangesAfterSplit.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // HospitalityFunctionalityProfileHospitalityPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkItemChangesAfterSplit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkSkipTableView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkIsHospitalityProfile);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "HospitalityFunctionalityProfileHospitalityPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIsHospitalityProfile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkSkipTableView;
        private System.Windows.Forms.CheckBox chkItemChangesAfterSplit;
        private System.Windows.Forms.Label label3;

    }
}
