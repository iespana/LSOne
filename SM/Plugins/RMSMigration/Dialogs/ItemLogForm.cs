using LSOne.Controls.Rows;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.RMSMigration.Model.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.RMSMigration.Dialogs
{
    public partial class ItemLogForm : Form
    {
        private List<ImportLogItem> ImportLogs { get; set; }
        public ItemLogForm(List<ImportLogItem> importLogs) : base()
        {
            InitializeComponent();
            ImportLogs = importLogs;
            dataGridViewLog.DataSource = ImportLogs;
        }
    }
}
