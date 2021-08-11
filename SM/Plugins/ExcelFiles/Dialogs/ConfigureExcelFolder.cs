using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.Utilities.GUI;
using LSOne.Utilities.IO;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.ExcelFiles.Dialogs
{
    public partial class ConfigureExcelFolder : DialogBase
    {
        private RecordIdentifier templateID = "";

        public ConfigureExcelFolder()
        {
            InitializeComponent();
            var workingfolder = PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, PluginEntry.ExcelFolderLocationSettingID,
               SettingType.Generic, null);
            if (workingfolder != null && !string.IsNullOrEmpty(workingfolder.Value))
            {
                tbLocation.Text = workingfolder.Value;
                btnOK.Enabled = Directory.Exists(tbLocation.Text);

                if(!btnOK.Enabled)
                {
                    pnlError.PanelStyle = new BaseStyle()
                    {
                        ForeColor = ColorPalette.Black,
                        BackColor = ColorPalette.RedLight,
                        BackColor2 = ColorPalette.RedDark,
                        GradientMode = GradientModeEnum.ForwardDiagonal
                    };
                    pnlError.Visible = true;
                    
                }
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting(true,  tbLocation.Text,"", SettingType.Generic);
            PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, PluginEntry.ExcelFolderLocationSettingID,
                SettingsLevel.User, setting);

            if (!Directory.Exists(Path.Combine(tbLocation.Text, "Excel Import")))
            {
                Directory.CreateDirectory(Path.Combine(tbLocation.Text, "Excel Import"));
            }
            if (!Directory.Exists(Path.Combine(tbLocation.Text, "Default templates")))
            {
                Directory.CreateDirectory(Path.Combine(tbLocation.Text, "Default templates"));
                string templatePath = Path.Combine(Application.StartupPath, "Templates");

                if (File.Exists(Path.Combine(templatePath, "Import Template.xlsx")))
                {
                    File.Copy(Path.Combine(templatePath, "Import Template.xlsx"), Path.Combine(tbLocation.Text, "Default templates", "Import Template.xlsx"));

                }
                if (File.Exists(Path.Combine(templatePath, "Basic Import Template.xlsx")))
                {
                    File.Copy(Path.Combine(templatePath, "Basic Import Template.xlsx"), Path.Combine(tbLocation.Text, "Default templates", "Basic Import Template.xlsx"));

                }
                if (File.Exists(Path.Combine(templatePath, "Import Basic Example.xlsx")))
                {
                    File.Copy(Path.Combine(templatePath, "Import Basic Example.xlsx"), Path.Combine(tbLocation.Text, "Default templates", "Import Basic Example.xlsx"));

                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        public RecordIdentifier TemplateID
        {
            get { return templateID; }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog { ShowNewFolderButton = false, SelectedPath = tbLocation.Text };

            if (DialogResult.OK == fbd.ShowDialog())
            {
                pnlError.Visible = false;
                tbLocation.Text = fbd.SelectedPath;
                btnOK.Enabled = Directory.Exists(tbLocation.Text);
            }

           
        }
    }
}
