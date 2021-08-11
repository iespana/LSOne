using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    partial class CompanyInfoLogoPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompanyInfoLogoPage));
            this.contextButton2 = new LSOne.Controls.ContextButton();
            this.contextButton1 = new LSOne.Controls.ContextButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextButton2
            // 
            resources.ApplyResources(this.contextButton2, "contextButton2");
            this.contextButton2.Context = LSOne.Controls.ButtonType.Remove;
            this.contextButton2.Name = "contextButton2";
            this.contextButton2.Click += new System.EventHandler(this.contextButton2_Click);
            // 
            // contextButton1
            // 
            resources.ApplyResources(this.contextButton1, "contextButton1");
            this.contextButton1.Context = LSOne.Controls.ButtonType.Edit;
            this.contextButton1.Name = "contextButton1";
            this.contextButton1.Click += new System.EventHandler(this.contextButton1_Click);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // CompanyInfoLogoPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.contextButton2);
            this.Controls.Add(this.contextButton1);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "CompanyInfoLogoPage";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButton contextButton2;
        private ContextButton contextButton1;
        private System.Windows.Forms.PictureBox pictureBox1;




    }
}
