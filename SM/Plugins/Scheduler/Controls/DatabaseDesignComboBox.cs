//using DataSelector = LSRetail.DD.Common.Data.DataSelector;
using System;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public class DatabaseDesignComboBox : ComboBox
    {
        private bool dataLoaded;


        public Guid? DatabaseDesignId
        {
            get
            {
                return GetDatabaseDesignId();
            }

            set
            {
                Guid effectiveValue;
                if (value != null)
                {
                    effectiveValue = value.Value;
                }
                else
                {
                    effectiveValue = Guid.Empty;
                }

                if (object.ReferenceEquals(effectiveValue, GetDatabaseDesignId()))
                    return;

                bool found = false;

                if (base.Items.Count >= 0)
                {
                    // Try and find the design in the existing list
                    foreach (DataSelector dataSelector in base.Items)
                    {
                        if (dataSelector.GuidId == effectiveValue)
                        {
                            base.SelectedItem = dataSelector;
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    // add the design explicitly to the list
                    int index = base.Items.Add(CreateDataSelector(DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetDatabaseDesign(PluginEntry.DataModel,effectiveValue)));
                    base.SelectedIndex = index;
                }
            }

        }

        public void InvalidateData()
        {
            dataLoaded = false;
        }

        private Guid? GetDatabaseDesignId()
        {

            if (base.SelectedItem != null)
            {
                var dataSelector = (DataSelector)base.SelectedItem;
                if (dataSelector.GuidId != null && dataSelector.GuidId != Guid.Empty)
                {
                    return (Guid)dataSelector.GuidId;
                }
            }

            return null;
        }

        private DataSelector CreateDataSelector(JscDatabaseDesign value)
        {
            if (value != null)
            {
                return new DataSelector { GuidId = (Guid)value.ID, Text = value.Description, Object = value };
            }
            else
            {
                return new DataSelector { GuidId = Guid.Empty, Text = Properties.Resources.DatabaseDesignSelecteNone, Object = null };
            }
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);

            NeedData();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            NeedData();
        }

        private void NeedData()
        {
            if (!dataLoaded)
            {
                var databaseDesignId = GetDatabaseDesignId();
                LoadData();
                dataLoaded = true;
                DatabaseDesignId = databaseDesignId;
            }
        }

        private void LoadData()
        {
            base.Items.Clear();

            base.Items.Add(CreateDataSelector( null));

            foreach (var databaseDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetDatabaseDesigns(PluginEntry.DataModel, false))
            {
                var selector = CreateDataSelector(databaseDesign);
                base.Items.Add(selector);
            }

        }

    }
}
