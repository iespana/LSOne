using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class DatabaseStringDialog : DialogBase
    {
        public DatabaseStringDialog()
        {
            InitializeComponent();
        }

        public List<DatabaseDriverType> DatabaseDriverTypes
        {
            get { return databaseStringFieldsControl.DatabaseDriverTypes; }
            set { databaseStringFieldsControl.DatabaseDriverTypes = value; }
        }

        public bool IsDDString
        {
            get { return databaseStringFieldsControl.IsDDString; }
            set { databaseStringFieldsControl.IsDDString = value; }
        }

        public DatabaseString DatabaseString
        {
            get { return databaseStringFieldsControl.DatabaseString; }
            set { databaseStringFieldsControl.DatabaseString = value; }
        }

        public event EventHandler<DatabaseStringEventArgs> TestClicked;

        private void databaseStringFieldsControl_DatabaseStringChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = databaseStringFieldsControl.DatabaseString != null;
        }

        private void databaseStringFieldsControl_TestClicked(object sender, DatabaseStringEventArgs e)
        {
            if (TestClicked != null)
            {
                TestClicked(this, e);
            }
        }
    }
}
