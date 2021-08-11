using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.DialogPanels
{
    public partial class TouristInformationPanel : DialogPageBase
    {
        private List<DataEntity> regions;
        private Task getRegionsTask;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        
        public TouristInformationPanel(IConnectionManager entry)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            getRegionsTask = Task.Factory.StartNew(getRegions);
            actTourist.DataModel = entry;
        }

        private void getRegions()
        {
            var regions = new ConcurrentDictionary<string, DataEntity>();
            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                try
                {
                    var info = new RegionInfo(cultureInfo.LCID);
                    regions.TryAdd(info.ThreeLetterISORegionName, new DataEntity(info.ThreeLetterISORegionName, info.DisplayName));
                }
                catch (Exception)
                {
                }
            }
            var ordered = regions.Values.AsParallel().OrderBy(r => r.Text);
            this.regions = ordered.ToList();
        }
        
        public void SetData(Tourist tourist)
        {
            tbName.Text = tourist.Name;
            cmbNationality.Text = tourist.Nationality;
            tbEmail.Text = tourist.Email;
            actTourist.SetData(tourist.Address);
        }

        public override void GetData(Tourist tourist)
        {
            tourist.Name = tbName.Text;
            tourist.Nationality = cmbNationality.Text;
            tourist.Email = tbEmail.Text;
            tourist.Address = actTourist.AddressRecord;
        }

        public override bool ValidateData()
        {
            return true;
        }

        private void cmbNationality_RequestData(object sender, EventArgs e)
        {
            cmbNationality.SetData(regions, null);
        }
    }
}
