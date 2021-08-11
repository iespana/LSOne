using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSRetail.StoreController.Controls;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.SharedCore;
using LSRetail.StoreController.SharedCore.Enums;
using LSRetail.StoreController.Controls.SharedControls.Interfaces;

namespace LSRetail.StoreController.ExcelImporter.SheetPages
{
    internal partial class HelloWorldViewPage : UserControl, ITabView
    {
        public HelloWorldViewPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, LSRetail.StoreController.Controls.TabControl.Tab tab)
        {
            return new SheetPages.HelloWorldViewPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }


        public void GetAuditDescriptors(List<LSRetail.StoreController.SharedCore.AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {

        }

        public void OnClose()
        {
            // Use this to clean up for example to Avoid Microsoft memory leak bug in ListViews
            // myListView.SmallImageList = null;
        }

        #endregion
    }
}
