using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
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
    public partial class DatabaseDesignPage : UserControl, IDetailView
    {
        public class InternalContext
        {
            public JscDatabaseDesign DatabaseDesign { get; set; }
        }

        public LSOne.Controls.ListView LVLocations
        {
            get { return lvLocations; }
        }
        public JscDatabaseDesign DatabaseDesign
        {
            get { return databaseDesign; }
            set { databaseDesign = value; }
        }

        private JscDatabaseDesign databaseDesign;

        public DatabaseDesignPage()
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
                databaseDesign = ((InternalContext) internalContext).DatabaseDesign;
            }
            ObjectToFrom(databaseDesign);
        }

        private void ObjectToFrom(JscDatabaseDesign obj)
        {
            tbDescription.Text = obj.Description;
            tbCodePage.Text = obj.CodePage.HasValue ? obj.CodePage.Value.ToString() : string.Empty;
            chkEnabled.Checked = obj.Enabled;

            LoadLocations();
        }

        private bool ObjectFromForm(JscDatabaseDesign obj)
        {
            obj.Description = tbDescription.Text;

            int? codePage;
            if (!GetCodePageValue(out codePage, true))
                return false;

            obj.CodePage = codePage;
            obj.Enabled = chkEnabled.Checked;

            return true;
        }


        public bool DataIsModified()
        {
            bool result = true;

            JscDatabaseDesign tempDesign = new JscDatabaseDesign();
            if (ObjectFromForm(tempDesign))
            {
                result = !AreEqual(databaseDesign, tempDesign);
            }

            return result;
        }

        private bool AreEqual(JscDatabaseDesign a, JscDatabaseDesign b)
        {
            return
                a.Description == b.Description &&
                a.CodePage == b.CodePage &&
                a.Enabled == b.Enabled;
        }

        public bool SaveData()
        {
            Validate();
            if (!ObjectFromForm(databaseDesign))
                return false;

            DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(PluginEntry.DataModel, databaseDesign);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "DatabaseDesign",
                databaseDesign.ID, null);

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            // TODO: implement
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier,
            object param)
        {
            switch (objectName)
            {
                case "DatabaseDesign":
                    LoadData(true, 0, null);
                    break;
            }
        }


        void IDetailView.Show()
        {
            this.Show();
        }

        void IDetailView.Hide()
        {
            this.Hide();
        }


        private void LoadLocations()
        {
            Cursor.Current = Cursors.WaitCursor;
            lvLocations.ClearRows();
            foreach (
                var location in
                    DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>()
                        .GetLocationsUsingDatabase(PluginEntry.DataModel, databaseDesign.ID))
            {

                var row = new Row();
                row.AddText(location.Text);
                row.AddText(location.ExCode);
                row.Tag = location;
                lvLocations.AddRow(row);
            }

            contextButtonsMembers.RemoveButtonEnabled = lvLocations.Rows.Count > 0;
            lvLocations.AutoSizeColumns();
        }

        private bool CheckCodePageValue()
        {
            int? dummy;
            return GetCodePageValue(out dummy, false);
        }

        private bool GetCodePageValue(out int? codePage, bool focusOnError)
        {
            bool isError = false;
            codePage = null;

            if (tbCodePage.TextLength > 0)
            {
                int value;
                if (int.TryParse(tbCodePage.Text, out value))
                {
                    isError = value < 0;
                    if (!isError)
                        codePage = value;
                }
                else
                {
                    isError = true;
                }
            }

            if (isError)
            {
                errorProvider.SetError(tbCodePage, Properties.Resources.DatabaseDesignCodePageMsg);
                if (focusOnError)
                {
                    tbCodePage.Focus();
                }
            }
            else
            {
                errorProvider.SetError(tbCodePage, string.Empty);
            }

            return !isError;
        }

        private void tbCodePage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < '0' || e.KeyChar > '9')
                e.Handled = true;
        }

        private void tbCodePage_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !CheckCodePageValue();

        }

        private void contextButtonsMembers_RemoveButtonClicked(object sender, System.EventArgs e)
        {
            string caption = string.Empty;
            string message = string.Empty;
            if (lvLocations.Selection.Count > 1)
            {
                message = "Do you want to stop the seleted locations using this Database Design?";
                caption = "Remove Locations";
            }
            else
            {
                message = "Do you want to stop the seleted location using this Database Design?";
                caption = "Remove Location";
            }

            if (
                MessageBox.Show(message,
                    caption, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                for (int i = 0; i < lvLocations.Selection.Count; i++)
                {
                    var tag = lvLocations.Rows[lvLocations.Selection.GetRowIndex(i)].Tag;
                    if (tag is JscLocation)
                    {
                        JscLocation location = tag as JscLocation;
                        location.DatabaseDesign = Guid.Empty;
                        DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                            .Save(PluginEntry.DataModel, location);

                    }
                }
                LoadLocations();

            }
        }

        private void contextButtonsMembers_AddButtonClicked(object sender, System.EventArgs e)
        {
            List<JscLocation> locations =
                DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                    .GetLocations(PluginEntry.DataModel, true)
                    .ToList();
            List<RecordIdentifier> currentLocations = new List<RecordIdentifier>();

            List<JscLocation> locationsToExclude = new List<JscLocation>();
            foreach (var row in lvLocations.Rows)
            {
                JscLocation temp = row.Tag as JscLocation;
                if (temp != null)
                {
                    currentLocations.Add(temp.ID);
                }
            }


            foreach (var jscLocation in locations)
            {
                if (currentLocations.Any(x => x == jscLocation.ID))
                {
                    locationsToExclude.Add(jscLocation);
                }
            }
            foreach (var jscLocation in locationsToExclude)
            {
                locations.Remove(jscLocation);

            }

            LocationSelectorDialog dialog = new LocationSelectorDialog(locations);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.LocationItem.DatabaseDesign != databaseDesign.ID)
                {
                    dialog.LocationItem.DatabaseDesign = databaseDesign.ID;


                    DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                        .Save(PluginEntry.DataModel, dialog.LocationItem);
                    LoadLocations();
                }
            }
        }



    }
}
