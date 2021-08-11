using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis.POSProcesses.WinFormsTouch;
using LSRetailPosis.Settings;
using LSRetailPosis.POSProcesses;
using LSRetailPosis;
using LSRetail.Utilities.DataTypes;

namespace Dialog.WinFormsTouch
{
    public enum InputType
    {
        Normal = 0,
        Email = 1
    }
    /// <summary>
    /// Summary description for frmInput.
    /// </summary>
    public class frmInput : ModalTouchBase
    {
        

        private InputType currentInputType;
        private string inputText;
        private string promptText;
        private LSRetailPosis.UIControls.Keyboard keyboard1;
        private SimpleButton btnCancel;
        private SimpleButton btnOk;
        private PanelControl panelBase;
        private Label lblPrompt;
        private int maxLength;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public InputType CurrentInputType
        {
            get { return currentInputType; }
            set { currentInputType = value; }
        }

        public string InputText
        {
            get { return inputText; }
            set { keyboard1.EnteredText = value; }
        }

        public string PromptText
        {
            set { promptText = value; }
            get { return promptText; }
        }

        public int MaxLength
        {
            set { 
                  maxLength = value;
                  if (maxLength > 0) { keyboard1.MaxLength = maxLength; }
                }
            get { return maxLength; }
        }

        public frmInput()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            currentInputType = InputType.Normal;
            keyboard1.EnterButtonPressed += new LSRetailPosis.UIControls.Keyboard.enterbuttonDelegate(keyboard1_EnterButtonPressed);
            this.Width = (ApplicationSettings.Settings.MainFormInfo.MainWindowWidth * 8) / 10;
            this.Height = ApplicationSettings.Settings.MainFormInfo.MainWindowHeight / 2;
            
            int centre = this.Width / 2;
            int y = btnOk.Location.Y;
            int x = centre - btnOk.Width - btnOk.Width;
            btnOk.Location = new System.Drawing.Point(x, y);
            x = centre + btnOk.Width;
            btnCancel.Location = new System.Drawing.Point(x, y);
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for frmInput are reserved at 1240 - 1259
            // In use now are ID's 1240 - 1241
            //
            btnOk.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1240); //OK
            btnCancel.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1241); //Cancel
            
            POSFormsManager.SetAppearanceOfForm(this);
        }

        public frmInput(int maxLength)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            currentInputType = InputType.Normal;
            keyboard1.EnterButtonPressed += new LSRetailPosis.UIControls.Keyboard.enterbuttonDelegate(keyboard1_EnterButtonPressed);
            this.Width = (ApplicationSettings.Settings.MainFormInfo.MainWindowWidth * 8) / 10;
            this.Height = ApplicationSettings.Settings.MainFormInfo.MainWindowHeight / 2;
            int centre = this.Width / 2;
            int y = btnOk.Location.Y;
            int x = centre - btnOk.Width - btnOk.Width;
            btnOk.Location = new System.Drawing.Point(x, y);
            x = centre + btnOk.Width;
            btnCancel.Location = new System.Drawing.Point(x, y);
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for frmInput are reserved at 1240 - 1259
            // In use now are ID's 1240 - 1241
            //
            btnOk.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1240); //OK
            btnCancel.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1241); //Cancel

            keyboard1.MaxLength = maxLength;

            POSFormsManager.SetAppearanceOfForm(this);
        }

        private void keyboard1_EnterButtonPressed()
        {
            btnOk_Click(this, new EventArgs());
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
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.panelBase = new DevExpress.XtraEditors.PanelControl();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.keyboard1 = new LSRetailPosis.UIControls.Keyboard();
            ((System.ComponentModel.ISupportInitialize)(this.panelBase)).BeginInit();
            this.panelBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AllowFocus = false;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(510, 324);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 52);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Tag = "BtnNormal";
            this.btnCancel.Text = "&Cancel";
            // 
            // btnOk
            // 
            this.btnOk.AllowFocus = false;
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Appearance.Options.UseTextOptions = true;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(290, 324);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(105, 52);
            this.btnOk.TabIndex = 1;
            this.btnOk.Tag = "BtnNormal";
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panelBase
            // 
            this.panelBase.Controls.Add(this.lblPrompt);
            this.panelBase.Controls.Add(this.btnCancel);
            this.panelBase.Controls.Add(this.btnOk);
            this.panelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBase.Location = new System.Drawing.Point(0, 0);
            this.panelBase.Name = "panelBase";
            this.panelBase.Size = new System.Drawing.Size(903, 383);
            this.panelBase.TabIndex = 1;
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblPrompt.Location = new System.Drawing.Point(5, 13);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(0, 23);
            this.lblPrompt.TabIndex = 2;
            this.lblPrompt.Tag = "H2";
            // 
            // keyboard1
            // 
            this.keyboard1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.keyboard1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.keyboard1.BuddyControl = null;
            this.keyboard1.DelayedVisible = true;
            this.keyboard1.EnteredText = "";
            this.keyboard1.KeyboardEntry = LSRetailPosis.UIControls.Keyboard.KeyboardEntryTypes.Normal;
            this.keyboard1.Location = new System.Drawing.Point(1, 53);
            this.keyboard1.MaxLength = 100;
            this.keyboard1.Name = "keyboard1";
            this.keyboard1.PasswordChar = "*";
            this.keyboard1.Size = new System.Drawing.Size(897, 263);
            this.keyboard1.TabIndex = 0;
            this.keyboard1.ObtainCultureName += new LSRetailPosis.UIControls.CultureNameHandler(this.keyboard1_ObtainCultureName);
            // 
            // frmInput
            // 
            this.ClientSize = new System.Drawing.Size(903, 383);
            this.Controls.Add(this.keyboard1);
            this.Controls.Add(this.panelBase);
            this.DoubleBuffered = true;
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmInput";
            this.Text = "frmInput";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_FormClosing);
            this.Load += new System.EventHandler(this.frmInput_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelBase)).EndInit();
            this.panelBase.ResumeLayout(false);
            this.panelBase.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if ((currentInputType == InputType.Email) && (!keyboard1.EnteredText.IsEmail()))
            {
                this.DialogResult = DialogResult.None;
                frmMessage dialog = new frmMessage(ApplicationLocalizer.Language.Translate(1242), MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                POSFormsManager.ShowPOSForm(dialog);
                dialog.Dispose();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                inputText = keyboard1.EnteredText;
                Close();
            }
        }

        private void frmInput_Load(object sender, EventArgs e)
        {
            lblPrompt.Text = promptText;
            keyboard1.Focus();
        }

        private void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            keyboard1.ResetDefaultKeyboard();
        }

        private void keyboard1_ObtainCultureName(object sender, UIControls.EventArguments.CultureEventArguments args)
        {
            args.CultureName = DLLEntry.Settings.CultureName;
        }
    }
}

