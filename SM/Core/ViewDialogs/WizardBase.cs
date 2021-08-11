using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.BusinessObjects;
using System.ComponentModel;

namespace LSOne.ViewCore.Dialogs
{
    public partial class WizardBase : Form
    {
        private Stack<IWizardPage> backStack;
        private Stack<IWizardPage> forwardStack;
        private IWizardPage        currentPage;
        

        private IConnectionManager dataModel;

        private bool   hasFinish;
        private string finishSpecialText;

        public WizardBase()
        {
            finishSpecialText = "";

            InitializeComponent();
        }

		private void WizardBase_Load(object sender, EventArgs e)
		{
			btnHelp.Location = new Point(this.ClientRectangle.Width - btnHelp.Width, 0);
		}


		protected IWizardPage CurrentPage
        {
            get
            {
                return currentPage;
            }
        }

        [DefaultValue(false)]
        public bool CreateAnotherCheckboxVisible
        {
            get { return chkCreateAnother.Visible; }
            set { chkCreateAnother.Visible = value; }
        }

        public bool CreateAnother { get { return chkCreateAnother.Checked; } }

        public string FinishSpecialText
        {
            get
            {
                return finishSpecialText;
            }
            set
            {
                finishSpecialText = value;

                if (hasFinish)
                {
                    if (finishSpecialText.Length > 0)
                    {
                        btnForward.Text = finishSpecialText;
                    }
                    else
                    {
                        btnForward.Text = Properties.Resources.Finish;
                    }
                }
            }
        }

        public bool HasHelp
        {
            get
            {
                return btnHelp.Visible;
            }
            set
            {
                btnHelp.Visible = value;
            }
        }

        private bool HasFinish
        {
            get
            {
                return hasFinish;
            }
            set
            {
                hasFinish = value;

                if (hasFinish)
                {
                    btnForward.Text = finishSpecialText.Length > 0 ? finishSpecialText : Properties.Resources.Finish;
                }
                else
                {
                    btnForward.Text = Properties.Resources.Next;
                }
                
            }
        }

        public bool NextEnabled
        {
            get
            {
                return btnForward.Enabled;
            }
            set
            {
                btnForward.Enabled = value;
            }
        }

        public WizardBase(IConnectionManager connection)
        {
            backStack = new Stack<IWizardPage>();
            forwardStack = new Stack<IWizardPage>();
            currentPage = null;

            this.dataModel = connection;

            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(222,222,222));

            e.Graphics.DrawLine(pen, 1, 1, panel1.Width, 1);

            pen.Dispose();
        }

        public void Next()
        {
            bool canUseFromForwardStack = true;

            if (!currentPage.NextButtonClick(ref canUseFromForwardStack))
            {
                return;
            }

            backStack.Push(currentPage);

            Controls.Remove(currentPage.PanelControl);

            if (forwardStack.Count == 0)
            {
                currentPage = currentPage.RequestNextPage();
            }
            else
            {
                IWizardPage page = currentPage.RequestNextPage();

                if (canUseFromForwardStack && page.GetType() == forwardStack.Peek().GetType())
                {
                    currentPage = forwardStack.Pop();
                }
                else
                {
                    forwardStack.Clear();
                    currentPage = page;
                }
            }

            DisplayPage(currentPage);
        }

        public void Back()
        {
            forwardStack.Push(currentPage);

            Controls.Remove(currentPage.PanelControl);

            currentPage = backStack.Pop();

            DisplayPage(currentPage);
        }

        public void Finish()
        {
            bool cancelAction = false;

            List<IWizardPage> pages  = new List<IWizardPage>();
            Stack<IWizardPage> stack = new Stack<IWizardPage>();

            // First put the back stack on a new stack to reverse it
            foreach (IWizardPage page in backStack)
            {
                stack.Push(page);
            }

            foreach (IWizardPage page in stack)
            {
                pages.Add(page);
            }

            pages.Add(currentPage);

            OnFinish(pages,ref cancelAction);

            if(CreateAnother)
            {
                pages.ForEach(p => p.ResetControls());

                while(currentPage != pages[0])
                {
                    Back();
                }

                return;
            }

            if (!cancelAction)
            {
                DialogResult = DialogResult.OK;

                Close();
            }
        }

        protected virtual void OnFinish(List<IWizardPage> pages,ref bool cancelAction)
        {
            
        }

        public void AddPage(IWizardPage wizardPage)
        {
            if (currentPage != null)
            {
                backStack.Push(currentPage);
                Controls.Remove(currentPage.PanelControl);
            }

            wizardPage.PanelControl.Location = new Point(2, 2);

            currentPage = wizardPage;

            DisplayPage(currentPage);

            if (forwardStack.Count > 0)
            {
                forwardStack.Clear();
            }
        }

        public void SetupButtons(IWizardPage page)
        {
            btnForward.Visible = page.HasForward || page.HasFinish;
            HasFinish = page.HasFinish;

            btnBack.Visible = backStack.Count > 0;
            if (btnForward.Visible)
            {
                btnForward.Location = new Point(btnCancel.Location.X-81,13);
                
            }
            btnBack.Location = new Point((btnBack.Visible && !btnForward.Visible) ? btnCancel.Location.X - 81 : btnForward.Location.X-81, 13);
        }

        private void DisplayPage(IWizardPage page)
        {
            Controls.Add(page.PanelControl);
			page.PanelControl.Dock = DockStyle.Fill;
			page.PanelControl.BringToFront();
            btnHelp.BringToFront();

            FinishSpecialText = "";

            SetupButtons(page);
            
            btnForward.Enabled = false;

            page.Display();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            if (hasFinish)
            {
                Finish();
            }
            else
            {
                Next();
            }
        }

        public IConnectionManager Connection
        {
            get
            {
                return dataModel;
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            WizardBase_HelpRequested(this, null);
        }

        protected virtual IApplicationCallbacks OnGetFramework()
        {
            return null;
        }

        protected virtual HelpSettings GetOnlineHelpSettings()
        {
            return new HelpSettings(this.Name);
        }

        private void WizardBase_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            IApplicationCallbacks framework = OnGetFramework();

            if (framework != null)
            {
                framework.ShowHelp(System.Reflection.Assembly.GetAssembly(this.GetType()), GetOnlineHelpSettings());
            }
        }

    }
}