//using DataSelector = LSRetail.DD.Common.Data.DataSelector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class DatabaseTableDesignSelector : UserControl
    {
        private bool isTableDesignRefreshed;
    
        public DatabaseTableDesignSelector()
        {
            InitializeComponent();

        }


        public void LoadData()
        {
          
            // Populate database box
            cmbDatabaseDesigns.Items.Clear();
            cmbDatabaseDesigns.Items.AddRange(GetDatabaseDesigns().ToArray());
            cmbTableDesigns.Items.Clear();
            isTableDesignRefreshed = false;
        }


        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    gbMain.Text = value;
                }
            }
        }


        [Browsable(false)]
        public Guid SelectedDatabaseDesignId
        {
            get
            {
                Guid result = Guid.Empty;

                var databaseDesign = SelectedDatabaseDesign;
                if (databaseDesign != null)
                    result = (Guid) databaseDesign.ID;

                return result;
            }
            set
            {
                for (int i = 0; i < cmbDatabaseDesigns.Items.Count; i++)
                {
                    DataSelector selector = (DataSelector)cmbDatabaseDesigns.Items[i];
                    if (((JscDatabaseDesign)selector.Object).ID == value)
                    {
                        cmbDatabaseDesigns.SelectedIndex = i;
                        return;
                    }
                }
            }
        }


        [Browsable(false)]
        public JscDatabaseDesign SelectedDatabaseDesign
        {
            get
            {
                JscDatabaseDesign result = null;

                DataSelector selector = cmbDatabaseDesigns.SelectedItem as DataSelector;
                if (selector != null)
                {
                    result = (JscDatabaseDesign)selector.Object;
                }

                return result;
            }
            set
            {
                if (value != null)
                {
                    SelectedDatabaseDesignId = (Guid) value.ID;
                }
                else
                {
                    cmbDatabaseDesigns.SelectedItem = null;
                }
            }
        }

        [Browsable(false)]
        public Guid SelectedTableDesignId
        {
            get
            {
                Guid result = Guid.Empty;

                var tableDesign = SelectedTableDesign;
                if (tableDesign != null)
                {
                    result = (Guid)tableDesign.ID;
                }

                return result;
            }
            set
            {
                AssertTableDesigns();
                for (int i = 0; i < cmbTableDesigns.Items.Count; i++)
                {
                    DataSelector selector = (DataSelector)cmbTableDesigns.Items[i];
                    if (((JscTableDesign)selector.Object).ID == value)
                    {
                        cmbTableDesigns.SelectedIndex = i;
                        return;
                    }
                }
            }
        }



        [Browsable(false)]
        public JscTableDesign SelectedTableDesign
        {
            get
            {
                JscTableDesign result = null;

                DataSelector selector = cmbTableDesigns.SelectedItem as DataSelector;
                if (selector != null)
                {
                    result = (JscTableDesign)selector.Object;
                }

                return result;
            }
            set
            {
                if (value != null)
                {
                    SelectedTableDesignId = (Guid) value.ID;
                }
                else
                {
                    cmbTableDesigns.SelectedItem = null;
                }
            }
        }


        public event EventHandler<EventArgs> TableSelectionChanged;


        private List<DataSelector> GetDatabaseDesigns()
        {
            List<DataSelector> databaseDesigns = new List<DataSelector>();
            foreach (var databaseDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetDatabaseDesigns(PluginEntry.DataModel, false))
            {
                databaseDesigns.Add(new DataSelector { Text = databaseDesign.Description, Object = databaseDesign });
            }

            return databaseDesigns;
        }

        private void cmbDatabaseDesigns_SelectedValueChanged(object sender, EventArgs e)
        {
            isTableDesignRefreshed = false;
            cmbTableDesigns.Items.Clear();
        }

        private void cmbTableDesigns_DropDown(object sender, EventArgs e)
        {
            AssertTableDesigns();
        }

        private void cmbTableDesigns_Enter(object sender, EventArgs e)
        {
            AssertTableDesigns();
        }

        private void AssertTableDesigns()
        {
            if (!isTableDesignRefreshed)
            {
                cmbTableDesigns.Items.AddRange(GetTableDesigns(SelectedDatabaseDesignId).ToArray());
                isTableDesignRefreshed = true;
            }
        }

        private List<DataSelector> GetTableDesigns(Guid databaseDesignId)
        {
            List<DataSelector> tableDesigns = new List<DataSelector>();
            foreach (var tableDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetTableDesigns(PluginEntry.DataModel, databaseDesignId, false))
            {
                tableDesigns.Add(new DataSelector { Text = tableDesign.TableName, Object = tableDesign });
            }

            return tableDesigns;
        }

        private void cmbTableDesigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnTableSelectionChanged();
        }

        private void OnTableSelectionChanged()
        {
            if (TableSelectionChanged != null)
            {
                TableSelectionChanged(this, EventArgs.Empty);
            }
        }


    }


    
}
