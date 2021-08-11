namespace LSOne.Controls.Dialogs
{
    partial class DatePickerTouch
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
            this.embeddedCheckBox = new LSOne.Controls.TouchCheckBox();
            this.SuspendLayout();
            // 
            // embeddedCheckBox
            // 
            this.embeddedCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.embeddedCheckBox.Location = new System.Drawing.Point(10, 13);
            this.embeddedCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.embeddedCheckBox.Name = "embeddedCheckBox";
            this.embeddedCheckBox.Size = new System.Drawing.Size(24, 24);
            this.embeddedCheckBox.TabIndex = 0;
            this.embeddedCheckBox.Text = null;
            this.embeddedCheckBox.CheckedChanged += new System.EventHandler(this.embeddedCheckBox_CheckedChanged);
            // 
            // DatePickerTouch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.embeddedCheckBox);
            this.Name = "DatePickerTouch";
            this.Size = new System.Drawing.Size(300, 50);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TouchCheckBox embeddedCheckBox;
    }
}
