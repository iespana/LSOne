using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Scheduler.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    public partial class TableDesignPage : UserControl, IDetailView
    {
        public class InternalContext
        {
            public JscTableDesign TableDesign { get; set; }
        }

        private InternalContext internalContext;

        public TableDesignPage()
        {
            InitializeComponent();
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (!isRevert)
            {
                this.internalContext = (InternalContext)internalContext;
            }
            ObjectToFrom();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
        }


        private void ObjectToFrom()
        {
            Cursor.Current = Cursors.WaitCursor;

            tbTableName.Text = internalContext.TableDesign.TableName;
            chkEnabled.Checked = internalContext.TableDesign.Enabled;

            lvFields.ClearRows();
            foreach (var field in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetFieldDesignsOrderedBySequence(PluginEntry.DataModel, internalContext.TableDesign.ID))
            {
                Row row = new Row();
                SetRow(row, field);
                lvFields.AddRow(row);
            }

            lvFields.AutoSizeColumns();
            lvFields.Sort();
        }

        private void SetRow(Row row, JscFieldDesign field)
        {
            row.Clear();
            row.AddText(field.FieldName);
            row.AddText(field.DataType.ToString()); // NOTE: Do not localize
            int length = field.Length ?? -1;
            string lengthString = field.Length.HasValue ? field.Length.ToString() : string.Empty;
            row.AddCell(new NumericCell(lengthString, length));
            row.AddText(field.Enabled ? Properties.Resources.Yes : Properties.Resources.No);
            row.Tag = field;
        }

        public void ReadFieldDesign()
        {
            var locations = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocationsWhereConnectable(PluginEntry.DataModel);

            // First try where locations are using the current table's database
            var locationsWithCurrentDatabase =
                from
                    location in locations
                where
                    location.DatabaseDesign == internalContext.TableDesign.DatabaseDesign
                select
                    location;

            if (locationsWithCurrentDatabase.Any())
            {
                ReadFields( locationsWithCurrentDatabase);
            }
            else if (locations.Any())
            {
                ReadFields( locations);
            }
            else
            {
                string text = Properties.Resources.TableDesignCannotReadFieldsMsg;
                string caption = Properties.Resources.TableDesignReadDesignHeader;
                MessageBox.Show(this, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void ReadFields( IEnumerable<JscLocation> locations)
        {
            int count = locations.Count();
            Debug.Assert(count >= 1, "Do not call this function for empty locations enumerable");

            if (count == 1)
            {
                // We have a single location we can connect to, no need to prompt user
                ReadFieldFromLocation( locations.First());
            }
            else
            {
                // More than one location available, prompt user
                string header = Properties.Resources.TableDesignLocationHeaderMsg;

                using (SelectLocationsDialog dialog = new SelectLocationsDialog())
                {
                    dialog.MultiSelect = false;
                    if (dialog.ShowDialog(PluginEntry.Framework.MainWindow, header, locations) == DialogResult.OK)
                    {
                        ReadFieldFromLocation( dialog.SelectedLocations[0]);
                    }
                }
            }
        }

        private void ReadFieldFromLocation(JscLocation location)
        {
            using (DataDirectorDialog dialog = new DataDirectorDialog())
            {
                if (dialog.ReadFieldDesign(this, location, (Guid)internalContext.TableDesign.ID))
                {
                    ObjectToFrom();
                }
            }
        }
    }
}
