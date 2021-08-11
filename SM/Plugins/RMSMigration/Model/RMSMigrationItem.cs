using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LSOne.ViewPlugins.RMSMigration.Model.Enums;
using LSOne.ViewPlugins.RMSMigration.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LSOne.ViewPlugins.RMSMigration.Model.Import;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSMigrationItem
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _ShouldImport = true;
        public bool ShouldImport
        {
            get
            {
                return _ShouldImport;
            }
            set
            {
                _ShouldImport = value;
                if (!_ShouldImport)
                {
                    ProgressStatus = ProgressStatus.Ignored;
                }
                else
                {
                    ProgressStatus = ProgressStatus.Ready;
                }
                PropertyChanged(this, new PropertyChangedEventArgs("ShouldImport"));
            }
        }
        public RMSMigrationItemType ItemType { get; set; }
        public string ItemName
        {
            get
            {
                return ItemType.GetAttributeOfType<DisplayAttribute>().Name;
            }
        }
        public ProgressStatus ProgressStatus { get; set; }

        public string ProgressStatusDisplay
        {
            get
            {
                return ProgressStatus.GetAttributeOfType<DisplayAttribute>().Name;
            }
        }

        public List<ImportLogItem> LogItems { get; set; }
        public string ErrorMessage
        {
            get
            {
                if (LogItems == null || LogItems.Count == 0)
                {
                    return string.Empty;
                }
                StringBuilder sb = new StringBuilder();
                LogItems.ForEach(l => sb.AppendLine(l.ErrorMessage));
                return sb.ToString();
            }
        }
    }
}
