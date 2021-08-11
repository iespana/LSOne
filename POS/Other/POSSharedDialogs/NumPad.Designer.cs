using System.Windows.Forms;

namespace LSOne.Controls.Dialogs
{
    partial class NumPad
    {

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.touchNumPad = new LSOne.Controls.TouchNumPad();
            this.enteredValue = new LSOne.Controls.ShadeTextBoxTouch();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // touchNumPad
            // 
            this.touchNumPad.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.touchNumPad.BackColor = System.Drawing.Color.Transparent;
            this.touchNumPad.Location = new System.Drawing.Point(0, 56);
            this.touchNumPad.Margin = new System.Windows.Forms.Padding(0);
            this.touchNumPad.Name = "touchNumPad";
            this.touchNumPad.Size = new System.Drawing.Size(294, 314);
            this.touchNumPad.TabIndex = 36;
            this.touchNumPad.TabStop = false;
            this.touchNumPad.EnterPressed += new System.EventHandler(this.touchNumPad_EnterPressed);
            this.touchNumPad.ClearPressed += new System.EventHandler(this.touchNumPad_ClearPressed);
            this.touchNumPad.BackspacePressed += new System.EventHandler(this.OnTouchNumPad_BackspacePressed);
            this.touchNumPad.TouchKeyPressed += new LSOne.Controls.TouchKeyEventHandler(this.OnTouchNumPad_TouchKeyPressed);
            // 
            // enteredValue
            // 
            this.enteredValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enteredValue.BackColor = System.Drawing.Color.White;
            this.enteredValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.enteredValue.GhostText = "Barcode";
            this.enteredValue.Location = new System.Drawing.Point(0, 0);
            this.enteredValue.Margin = new System.Windows.Forms.Padding(0);
            this.enteredValue.Name = "enteredValue";
            this.enteredValue.ReadOnly = false;
            this.enteredValue.ShowFocusedBorder = false;
            this.enteredValue.Size = new System.Drawing.Size(294, 50);
            this.enteredValue.TabIndex = 35;
            this.enteredValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.enteredValue.Enter += new System.EventHandler(this.ntbInput_Enter);
            this.enteredValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEnteredValue_KeyDown);
            this.enteredValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnEnteredValue_KeyPress);
            this.enteredValue.Leave += new System.EventHandler(this.ntbInput_Leave);
            // 
            // NumPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.touchNumPad);
            this.Controls.Add(this.enteredValue);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "NumPad";
            this.Size = new System.Drawing.Size(294, 369);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components;
        private LSOne.Controls.ShadeTextBoxTouch enteredValue;
        private TouchNumPad touchNumPad;
    }
}


