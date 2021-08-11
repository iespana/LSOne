namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    partial class ColorPalettePreviewDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPalettePreviewDialog));
            this.touchButton1 = new LSOne.Controls.TouchButton();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.touchButton2 = new LSOne.Controls.TouchButton();
            this.touchButton3 = new LSOne.Controls.TouchButton();
            this.touchButton4 = new LSOne.Controls.TouchButton();
            this.touchButton5 = new LSOne.Controls.TouchButton();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // touchButton1
            // 
            this.touchButton1.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.touchButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.touchButton1.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            resources.ApplyResources(this.touchButton1, "touchButton1");
            this.touchButton1.ForeColor = System.Drawing.Color.White;
            this.touchButton1.Name = "touchButton1";
            this.touchButton1.UseVisualStyleBackColor = false;
            // 
            // pnlBottom
            // 
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Name = "pnlBottom";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // touchButton2
            // 
            this.touchButton2.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.touchButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.touchButton2.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            resources.ApplyResources(this.touchButton2, "touchButton2");
            this.touchButton2.ForeColor = System.Drawing.Color.White;
            this.touchButton2.Name = "touchButton2";
            this.touchButton2.UseVisualStyleBackColor = false;
            // 
            // touchButton3
            // 
            this.touchButton3.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.touchButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.touchButton3.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Action;
            resources.ApplyResources(this.touchButton3, "touchButton3");
            this.touchButton3.ForeColor = System.Drawing.Color.White;
            this.touchButton3.Name = "touchButton3";
            this.touchButton3.UseVisualStyleBackColor = false;
            // 
            // touchButton4
            // 
            this.touchButton4.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.touchButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.touchButton4.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            resources.ApplyResources(this.touchButton4, "touchButton4");
            this.touchButton4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.touchButton4.Name = "touchButton4";
            this.touchButton4.UseVisualStyleBackColor = false;
            // 
            // touchButton5
            // 
            this.touchButton5.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.touchButton5.BackColor = System.Drawing.Color.White;
            this.touchButton5.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.None;
            resources.ApplyResources(this.touchButton5, "touchButton5");
            this.touchButton5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.touchButton5.Name = "touchButton5";
            this.touchButton5.UseVisualStyleBackColor = false;
            // 
            // ColorPalettePreviewDialog
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.touchButton5);
            this.Controls.Add(this.touchButton4);
            this.Controls.Add(this.touchButton3);
            this.Controls.Add(this.touchButton2);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.touchButton1);
            this.DoubleBuffered = true;
            this.HasHelp = true;
            this.Name = "ColorPalettePreviewDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Controls.SetChildIndex(this.touchButton1, 0);
            this.Controls.SetChildIndex(this.pnlBottom, 0);
            this.Controls.SetChildIndex(this.touchButton2, 0);
            this.Controls.SetChildIndex(this.touchButton3, 0);
            this.Controls.SetChildIndex(this.touchButton4, 0);
            this.Controls.SetChildIndex(this.touchButton5, 0);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchButton touchButton1;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnOK;
        private Controls.TouchButton touchButton2;
        private Controls.TouchButton touchButton3;
        private Controls.TouchButton touchButton4;
        private Controls.TouchButton touchButton5;
    }
}