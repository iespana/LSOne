using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.POS.Processes.Common;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class DimensionService : IDimensionService
    {
        public DimensionService()
        {

        }

        #region IDimension Members

       
        #endregion

        public IErrorLog ErrorLog
        {
            set 
            {

            }
        }

        public virtual bool SetDimensionOnItem(IConnectionManager entry, ISaleLineItem saleLineItem)
        {
            List<RetailItemDimension> dimensions = Providers.RetailItemDimensionData.GetListForRetailItem(entry, saleLineItem.MasterID);            

            using (DimensionDialog dlg = new DimensionDialog(entry, saleLineItem))
            {
                dlg.ShowDialog();
            }

            return false;
        }

        /// <summary>
        /// Shows the DimensionDialog which shows all available dimensions and attribute for the given master item.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="saleLineItem">The master item to show dimensions and attributes for</param>
        /// <returns>The ID of the variant item for the attribute combination that the user selected</returns>
        public virtual RecordIdentifier ShowDimensionDialog(IConnectionManager entry, ISaleLineItem saleLineItem)
        {
            using (DimensionDialog dlg = new DimensionDialog(entry, saleLineItem))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.SelectedItemID;
                }

                return RecordIdentifier.Empty;
            }
        }

        /// <summary>
        /// Shows the DimensionDialog which shows all available dimensions and attribute for the given master item.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="masterItemID">ID of the master item to show dimensions and attributes for</param>
        /// <param name="masterItemName">Name/description of the master item to show dimensions and attributes for</param>
        /// <returns>The ID of the variant item for the attribute combination that the user selected</returns>
        public virtual RecordIdentifier ShowDimensionDialog(IConnectionManager entry, RecordIdentifier masterItemID, string masterItemName)
        {
            using (DimensionDialog dlg = new DimensionDialog(entry, masterItemID, masterItemName))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.SelectedItemID;
                }

                return RecordIdentifier.Empty;
            }
        }

        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }
    }
}
