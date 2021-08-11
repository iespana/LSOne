using System;
using System.ComponentModel;
using DevExpress.XtraEditors;
using LSOne.POS.Processes.WinControlsKeyboard;

namespace LSOne.Services.WinFormsKeyboard
{
    /// <summary>
    /// Summary description for frmKeyboardBase.
    /// </summary>
    public class frmKeyboardBase : DevExpress.XtraEditors.XtraForm
    {
        public StyleController styleController;
        private PanelControl basePanel;
        protected KeyboardButtonControl keyboardButtonControl;
        private IContainer components;

        //public KeyboardButtonControl.KeyboardButtonControl KeyboardButtonControl
        //{
        //    get { return keyboardButtonControl; };
        //}

        public frmKeyboardBase()
        {
            //
            // Required for Windows Form Designer support
            //
            try
            {
                InitializeComponent();
            }
            catch (Exception) { }

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.basePanel = new DevExpress.XtraEditors.PanelControl();
            this.keyboardButtonControl = new KeyboardButtonControl();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePanel)).BeginInit();
            this.basePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // basePanel
            // 
            this.basePanel.Controls.Add(this.keyboardButtonControl);
            this.basePanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.basePanel.Location = new System.Drawing.Point(463, 0);
            this.basePanel.Name = "basePanel";
            this.basePanel.Size = new System.Drawing.Size(198, 557);
            this.basePanel.TabIndex = 0;
            this.basePanel.Text = "panelControl1";
            // 
            // keyboardButtonControl
            // 
            this.keyboardButtonControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keyboardButtonControl.Location = new System.Drawing.Point(2, 2);
            this.keyboardButtonControl.Name = "keyboardButtonControl";
            this.keyboardButtonControl.Size = new System.Drawing.Size(194, 553);
            this.keyboardButtonControl.TabIndex = 10;
            this.keyboardButtonControl.TabStop = false;
            // 
            // frmKeyboardBase
            // 
            this.ClientSize = new System.Drawing.Size(661, 557);
            this.Controls.Add(this.basePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmKeyboardBase";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmKeyboardBase";
            this.Activated += new System.EventHandler(this.frmKeyboardBase_Activated);
            this.Load += new System.EventHandler(this.frmKeyboardBase_Load);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePanel)).EndInit();
            this.basePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void frmKeyboardBase_Activated(object sender, EventArgs e)
        {
            
        }

        private void frmKeyboardBase_Load(object sender, EventArgs e)
        {
            try
            {
                this.Height = DLLEntry.Settings.MainFormInfo.MainWindowHeight;
                this.Top = this.Owner.Top;
                this.Left = this.Owner.Left + this.Owner.Width - this.Width;
            }
            catch (Exception)
            {

            }

        }
    }
}

