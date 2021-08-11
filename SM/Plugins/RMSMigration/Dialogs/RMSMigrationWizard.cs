using System;
using System.Collections.Generic;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewPlugins.RMSMigration.Dialogs.WizardPages;
using LSOne.ViewPlugins.RMSMigation.Dialogs.WizardPages;
using LSOne.DataLayer.DataProviders;
using LSOne.ViewPlugins.RMSMigration.Helper;
using System.Windows.Forms;
using LSOne.ViewPlugins.RMSMigration.Properties;
using System.IO;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.ViewPlugins.RMSMigration.Dialogs
{
    public partial class RMSMigrationWizard : WizardBase
    {
        public RMSMigrationWizard(IConnectionManager connection)
            : base(connection)
        {
            InitializeComponent();

            PluginEntry.DataModel.DisableReplicationActionCreation = true;
            DataIntegrityTestPanel dataIntegrityTestPanel = new DataIntegrityTestPanel(this);
            AddPage(dataIntegrityTestPanel);
        }

        private void Page_RequestFinish(object sender, EventArgs e)
        {
            PageRequestedFinish = true;
            Finish();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override HelpSettings GetOnlineHelpSettings()
        {
            return new HelpSettings("StoreSetupChecklistForRMSUsers");
        }

        private bool PageRequestedFinish = false;

        protected override void OnFinish(List<IWizardPage> pages, ref bool cancelAction)
        {
            PageRequestedFinish = true;

            PluginEntry.DataModel.DisableReplicationActionCreation = false;

            if (pages.Count < 5 || !(pages[4] is ImportPanel))
            {
                return;
            }

            ImportPanel importPanel = pages[4] as ImportPanel;
            bool hasLogItems = importPanel.MigrationItems.Any(el => el.LogItems != null && el.LogItems.Count > 0);

            if (hasLogItems)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowNewFolderButton = true;
                fbd.Description = Resources.SaveLogToFileHeader;
                DialogResult dr = fbd.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    if (!Directory.Exists(fbd.SelectedPath))
                    {
                        Directory.CreateDirectory(fbd.SelectedPath);
                    }
                    importPanel.MigrationItems.ForEach(mi =>
                    {
                        if (mi.LogItems != null && mi.LogItems.Count > 0)
                        {
                            string path = Path.Combine(fbd.SelectedPath, string.Format("{0}_{1}.txt", mi.ItemName, DateTime.Now.ToString("yyyy-dd-M-HH-mm")));
                            File.Create(path).Dispose();
                            using (TextWriter tw = new StreamWriter(path))
                            {
                                tw.WriteLine(mi.ErrorMessage);
                                tw.Close();
                            }
                        }
                    });
                }
            }
        }

        private void RMSMigrationWizard_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!PageRequestedFinish)
            {
                Finish();
            }
        }
    }
}
