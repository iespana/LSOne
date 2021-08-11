using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewPlugins.Scheduler.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class DatabaseStringControl : SimpleDatabaseStringControl
    {
        private DatabaseStringDialog databaseStringDialog = new DatabaseStringDialog();

        private ButtonTextBox.ButtonItemButton buttonItemTest;
        private ButtonTextBox.ButtonItemButton buttonItemEdit;

        public DatabaseStringControl()
        {
            InitializeComponent();

            databaseStringDialog.TestClicked += new EventHandler<DatabaseStringEventArgs>(databaseStringDialog_TestClicked);

            // Add buttons to the button text box
            buttonItemTest = new ButtonTextBox.ButtonItemButton();
            buttonItemTest.Image = Properties.Resources.DataGreenCheck;
            buttonItemTest.Text = null;
            buttonItemTest.Click += new EventHandler(buttonItemTest_Click);

            buttonItemEdit = new ButtonTextBox.ButtonItemButton();
            buttonItemEdit.Image = ContextButtons.GetEditButtonImage();
            buttonItemEdit.Text = null;
            buttonItemEdit.Click += new EventHandler(buttonItemEdit_Click);

            buttonTextBox.Buttons.Add(buttonItemTest, buttonItemEdit);
        }




        public List<DatabaseDriverType> DatabaseDriverTypes
        {
            get { return databaseStringDialog.DatabaseDriverTypes; }
            set { databaseStringDialog.DatabaseDriverTypes = value; }
        }

        public bool EditButtonVisible
        {
            get { return buttonItemEdit.Visible; }
            set { buttonItemEdit.Visible = value; }
        }

        public bool TestButtonVisible
        {
            get { return buttonItemTest.Visible; }
            set { buttonItemTest.Visible = value; }
        }

        public event EventHandler<DatabaseStringEventArgs> TestClicked;



        protected override void UpdateActions()
        {
            base.UpdateActions();
            buttonItemTest.Enabled = buttonTextBox.TextLength > 0;
        }


        private void buttonItemTest_Click(object sender, EventArgs e)
        {
            OnTestClicked(this.DatabaseString);
        }

        private void OnTestClicked(DatabaseString databaseString)
        {
            if (TestClicked != null)
            {
                TestClicked(this, new DatabaseStringEventArgs { DatabaseString = databaseString });
            }
        }

        private void buttonItemEdit_Click(object sender, EventArgs e)
        {
            databaseStringDialog.IsDDString = IsDDString;
            if (buttonTextBox.TextLength > 0)
            {
                databaseStringDialog.DatabaseString = databaseString;
            }
            else
            {
                databaseStringDialog.DatabaseString = null;
            }
            if (databaseStringDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.databaseString = databaseStringDialog.DatabaseString;
                SetTextBoxText();
                OnDatabaseStringChanged();
            }
        }

        private void databaseStringDialog_TestClicked(object sender, DatabaseStringEventArgs e)
        {
            OnTestClicked(e.DatabaseString);
        }

    }
}
