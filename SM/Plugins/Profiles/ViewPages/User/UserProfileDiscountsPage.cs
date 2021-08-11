using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.GenericConnector.Enums;

namespace LSOne.ViewPlugins.Profiles.ViewPages.User
{
    public partial class UserProfileDiscountsPage : UserControl, ITabView
    {
        private UserProfile userProfile;

        public UserProfileDiscountsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new UserProfileDiscountsPage();
        }

        public bool DataIsModified()
        {
            if (ntbMaxLineDiscAmount.Value != (double)userProfile.MaxLineDiscountAmount) return true;
            if (ntbMaxLineDiscPercentage.Value != (double)userProfile.MaxLineDiscountPercentage) return true;
            if (ntbMaxTotalDiscAmount.Value != (double)userProfile.MaxTotalDiscountAmount) return true;
            if (ntbMaxTotalDiscPercentage.Value != (double)userProfile.MaxTotalDiscountPercentage) return true;
            if (ntbMaxLineReturnAmount.Value != (double)userProfile.MaxLineReturnAmount) return true;
            if (ntbMaxTotalReturnAmount.Value != (double)userProfile.MaxTotalReturnAmount) return true;
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("UserProfile", userProfile.ID, Properties.Resources.UserProfile, true));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            var pricesLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            var percentageLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

            if (!isRevert)
            {
                userProfile = (UserProfile)internalContext;
            }

            ntbMaxLineDiscAmount.SetValueWithLimit(userProfile.MaxLineDiscountAmount, pricesLimiter);
            ntbMaxLineDiscPercentage.SetValueWithLimit(userProfile.MaxLineDiscountPercentage, percentageLimiter);
            ntbMaxTotalDiscAmount.SetValueWithLimit(userProfile.MaxTotalDiscountAmount, pricesLimiter);
            ntbMaxTotalDiscPercentage.SetValueWithLimit(userProfile.MaxTotalDiscountPercentage, percentageLimiter);
            ntbMaxLineReturnAmount.SetValueWithLimit(userProfile.MaxLineReturnAmount, pricesLimiter);
            ntbMaxTotalReturnAmount.SetValueWithLimit(userProfile.MaxTotalReturnAmount, pricesLimiter);
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            userProfile.MaxLineDiscountAmount = (decimal)ntbMaxLineDiscAmount.Value;
            userProfile.MaxLineDiscountPercentage = (decimal)ntbMaxLineDiscPercentage.Value;
            userProfile.MaxTotalDiscountAmount = (decimal)ntbMaxTotalDiscAmount.Value;
            userProfile.MaxTotalDiscountPercentage = (decimal)ntbMaxTotalDiscPercentage.Value;
            userProfile.MaxLineReturnAmount = (decimal)ntbMaxLineReturnAmount.Value;
            userProfile.MaxTotalReturnAmount = (decimal)ntbMaxTotalReturnAmount.Value;
            return true;
        }

        public void SaveUserInterface()
        {
            
        }
    }
}
