using System;
using System.Windows.Forms;
using LSOne.DataLayer.DDBusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class TableDesignSelectorDialog : Form
    {

        public TableDesignSelectorDialog()
        {
            InitializeComponent();
            UpdateActions();
        }


        private void TableDesignSelectorDialog_Shown(object sender, EventArgs e)
        {
            databaseTableDesignSelector1.LoadData();
        }

        private void TableDesignSelectorDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void databaseTableDesignSelector1_TableSelectionChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void UpdateActions()
        {
            btnOK.Enabled = databaseTableDesignSelector1.SelectedTableDesign != null;
        }


        public JscTableDesign SelectedTable
        {
            get
            {
                return databaseTableDesignSelector1.SelectedTableDesign;
            }
        }


    }
}
