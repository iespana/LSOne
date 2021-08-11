using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalitySetupReservationsPage : UserControl, ITabView
    {
        private HospitalitySetup setup;
        private TimeSpanConverter timeSpanConverter;

        public HospitalitySetupReservationsPage()
        {
            InitializeComponent();

            timeSpanConverter = new TimeSpanConverter();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalitySetupReservationsPage();
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            if (dtpPeriod1TimeFrom.Value.Hour != setup.Period1TimeFrom.Hours 
                || dtpPeriod1TimeFrom.Value.Minute != setup.Period1TimeFrom.Minutes 
                || dtpPeriod1TimeFrom.Value.Second != setup.Period1TimeFrom.Seconds) 
                return true;

            if (dtpPeriod1TimeTo.Value.Hour != setup.Period1TimeTo.Hours
                || dtpPeriod1TimeTo.Value.Minute != setup.Period1TimeTo.Minutes
                || dtpPeriod1TimeTo.Value.Second != setup.Period1TimeTo.Seconds)
                return true;

            if (dtpPeriod2TimeFrom.Value.Hour != setup.Period2TimeFrom.Hours
                || dtpPeriod2TimeFrom.Value.Minute != setup.Period2TimeFrom.Minutes
                || dtpPeriod2TimeFrom.Value.Second != setup.Period2TimeFrom.Seconds)
                return true;

            if (dtpPeriod2TimeTo.Value.Hour != setup.Period2TimeTo.Hours
                || dtpPeriod2TimeTo.Value.Minute != setup.Period2TimeTo.Minutes
                || dtpPeriod2TimeTo.Value.Second != setup.Period2TimeTo.Seconds)
                return true;

            if (dtpPeriod3TimeFrom.Value.Hour != setup.Period3TimeFrom.Hours
                || dtpPeriod3TimeFrom.Value.Minute != setup.Period3TimeFrom.Minutes
                || dtpPeriod3TimeFrom.Value.Second != setup.Period3TimeFrom.Seconds)
                return true;

            if (dtpPeriod3TimeTo.Value.Hour != setup.Period3TimeTo.Hours
                || dtpPeriod3TimeTo.Value.Minute != setup.Period3TimeTo.Minutes
                || dtpPeriod3TimeTo.Value.Second != setup.Period3TimeTo.Seconds)
                return true;

            if (dtpPeriod4TimeFrom.Value.Hour != setup.Period4TimeFrom.Hours
                || dtpPeriod4TimeFrom.Value.Minute != setup.Period4TimeFrom.Minutes
                || dtpPeriod4TimeFrom.Value.Second != setup.Period4TimeFrom.Seconds)
                return true;

            if (dtpPeriod4TimeTo.Value.Hour != setup.Period4TimeTo.Hours
                || dtpPeriod4TimeTo.Value.Minute != setup.Period4TimeTo.Minutes
                || dtpPeriod4TimeTo.Value.Second != setup.Period4TimeTo.Seconds)
                return true;

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            throw new NotImplementedException();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            setup = (HospitalitySetup)internalContext;

            dtpPeriod1TimeFrom.Value = new DateTime(2010, 1, 1, setup.Period1TimeFrom.Hours, setup.Period1TimeFrom.Minutes, setup.Period1TimeFrom.Seconds);
            dtpPeriod1TimeTo.Value = new DateTime(2010, 1, 1, setup.Period1TimeTo.Hours, setup.Period1TimeTo.Minutes, setup.Period1TimeTo.Seconds);
            dtpPeriod2TimeFrom.Value = new DateTime(2010, 1, 1, setup.Period2TimeFrom.Hours, setup.Period2TimeFrom.Minutes, setup.Period2TimeFrom.Seconds);
            dtpPeriod2TimeTo.Value = new DateTime(2010, 1, 1, setup.Period2TimeTo.Hours, setup.Period2TimeTo.Minutes, setup.Period2TimeTo.Seconds);
            dtpPeriod3TimeFrom.Value = new DateTime(2010, 1, 1, setup.Period3TimeFrom.Hours, setup.Period3TimeFrom.Minutes, setup.Period3TimeFrom.Seconds);
            dtpPeriod3TimeTo.Value = new DateTime(2010, 1, 1, setup.Period3TimeTo.Hours, setup.Period3TimeTo.Minutes, setup.Period3TimeTo.Seconds);
            dtpPeriod4TimeFrom.Value = new DateTime(2010, 1, 1, setup.Period4TimeFrom.Hours, setup.Period4TimeFrom.Minutes, setup.Period4TimeFrom.Seconds);
            dtpPeriod4TimeTo.Value = new DateTime(2010, 1, 1, setup.Period4TimeTo.Hours, setup.Period4TimeTo.Minutes, setup.Period4TimeTo.Seconds);

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            throw new NotImplementedException();
        }

        public bool SaveData()
        {
            setup.Period1TimeFrom = new TimeSpan(dtpPeriod1TimeFrom.Value.Hour, dtpPeriod1TimeFrom.Value.Minute, dtpPeriod1TimeFrom.Value.Second);
            setup.Period1TimeTo = new TimeSpan(dtpPeriod1TimeTo.Value.Hour, dtpPeriod1TimeTo.Value.Minute, dtpPeriod1TimeTo.Value.Second);
            setup.Period2TimeFrom = new TimeSpan(dtpPeriod2TimeFrom.Value.Hour, dtpPeriod2TimeFrom.Value.Minute, dtpPeriod2TimeFrom.Value.Second);
            setup.Period2TimeTo = new TimeSpan(dtpPeriod2TimeTo.Value.Hour, dtpPeriod2TimeTo.Value.Minute, dtpPeriod2TimeTo.Value.Second);
            setup.Period3TimeFrom = new TimeSpan(dtpPeriod3TimeFrom.Value.Hour, dtpPeriod3TimeFrom.Value.Minute, dtpPeriod3TimeFrom.Value.Second);
            setup.Period3TimeTo = new TimeSpan(dtpPeriod3TimeTo.Value.Hour, dtpPeriod3TimeTo.Value.Minute, dtpPeriod3TimeTo.Value.Second);
            setup.Period4TimeFrom = new TimeSpan(dtpPeriod4TimeFrom.Value.Hour, dtpPeriod4TimeFrom.Value.Minute, dtpPeriod4TimeFrom.Value.Second);
            setup.Period4TimeTo = new TimeSpan(dtpPeriod4TimeTo.Value.Hour, dtpPeriod4TimeTo.Value.Minute, dtpPeriod4TimeTo.Value.Second);

            return true;
        }

        public void OnClose()
        {
            
        }

        public void SaveUserInterface()
        {
        }

        #endregion
    }
}
