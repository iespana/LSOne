using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.TaskBarItems;
using LSOne.Controls.TillLayoutDesigner;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.TouchButtons.Views
{
    public partial class TouchButtonView : ViewBase
    {
        private RecordIdentifier layoutID;
        private TouchLayout layout;
        private ISession sessionHandler;

        private TillLayoutDesigner tillLayoutDesigner;

        public TouchButtonView(RecordIdentifier layoutID)
            : this()
        {
            this.layoutID = layoutID;
        }

        public TouchButtonView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.Help;

            imgClose.Image = CloseImage;
            imgDelete.Image = DeleteImage;
            imgRevert.Image = RevertImage;
            imgSave.Image = SaveImage;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout);

            if (ReadOnly)
            {
                lnkSave.Visible = false;
                imgSave.Visible = false;
                lnkDelete.Visible = false;
                imgDelete.Visible = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.ShowProgress(LoadLayout);

        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("TouchButtonLayout",layoutID, Properties.Resources.TouchButtonLayout, true));
        }

        

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.TouchButtonLayout;
            }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.TouchButtonLayout + ": " + tbID.Text + " - " + tbDescription.Text;
            }
        }

  

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return layoutID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            layout = Providers.TouchLayoutData.Get(PluginEntry.DataModel,layoutID);

            imgEdit.Image =  ImageUtils.ResizeImage(ContextButtons.GetEditButtonImage(), 16);

            tbID.Text = (string)layout.ID;
            tbDescription.Text = layout.Text;
            
            if (isRevert)
            {
                this.ShowProgress();

                if (tillLayoutDesigner != null)
                {
                    tillLayoutDesigner.HideDesigner();

                    
                    Controls.Remove(tillLayoutDesigner); 
                    this.pnlHost.Controls.Remove(tillLayoutDesigner);
                    LoadLayout(this, EventArgs.Empty);
                }
            }

            //HeaderIcon = Properties.Resources.Profiles16;
            HeaderText = Description;
        }

        protected override bool DataIsModified()
        {
            if (tbDescription.Text != layout.Text) return true;

            if (tillLayoutDesigner.IsModified)
            {
                return true;
            }
          
            return false;
        }

        protected override bool SaveData()
        {
            layout.Text = tbDescription.Text;

            if (tillLayoutDesigner.IsModified)
            {
                tillLayoutDesigner.SaveTillDesign((string)layoutID);
            }
            
            Providers.TouchLayoutData.SaveHeader(PluginEntry.DataModel, layout);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "TouchButtonLayout", layout.ID, null);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "TouchButtonLayout":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == layoutID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    else if (changeHint == DataEntityChangeType.Edit && changeIdentifier == layoutID)
                    {
                        RefreshLayout();
                    }
                    break;
            }
        }
 

        private void pnlHost_SizeChanged(object sender, EventArgs e)
        {
            if (this.tillLayoutDesigner != null)
            {
                this.tillLayoutDesigner.Size = pnlHost.Size;
            }
        }
        
        private void LoadLayout(object sender, EventArgs e)
        {
            if(sessionHandler == null)
            {
                sessionHandler = new SessionHandler();
            }

            this.tillLayoutDesigner = new TillLayoutDesigner();
            this.tillLayoutDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tillLayoutDesigner.Name = "tillLayoutDesigner";
            this.tillLayoutDesigner.Size = pnlHost.Size;
            this.tillLayoutDesigner.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.tillLayoutDesigner.TabIndex = 3;

            // Not ready yet - to be continued

            Currency companyCurrency = Providers.CurrencyData.GetCompanyCurrency(PluginEntry.DataModel) ?? new Currency();
            Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);

            Settings settings = new Settings();
            settings.Store = new Store();
            settings.Store.Currency = (string)companyCurrency.ID;
            settings.Store.ID = parameters.LocalStore;
            tillLayoutDesigner.Start(PluginEntry.DataModel, sessionHandler, settings, layoutID, true, new POSDialogHandler(), companyCurrency.ID, PluginEntry.Framework.MainWindow);

            this.tillLayoutDesigner.Size = pnlHost.Size;

            this.HideProgress();
            
            this.pnlHost.Controls.Add(this.tillLayoutDesigner);            
        }

        private void RefreshLayout()
        {
            this.ShowProgress();

            tillLayoutDesigner.RefreshButtonGrids(PluginEntry.DataModel, layoutID);

            this.HideProgress();
        }

        
        private void lnkRevert_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Revert();
        }

        private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ManualSave();
        }

        private void lnkClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tillLayoutDesigner.HideDesigner();
            ManualClose();
        }

        private void lnkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ManualDelete();
        }

        private void lnkChangeDesign_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tillLayoutDesigner.Design();
        }

        public override void OnHide()
        {
            tillLayoutDesigner.HideDesigner();
        }

        protected override void OnDelete()
        {
            base.OnDelete();

            PluginOperations.DeleteTouchButtonLayout(layoutID);
        }

        private void lnkButtonGridMenus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PluginOperations.EditTouchLayoutButtonGrids(layout.ID);
        }        
    }
    internal class Settings : ISettings
    {
        private HardwareProfile hardwareProfile;
        private Store store;

        public void ResetPOSUser()
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, KeyboardMapping> KeyboardMapping
        {
            get { throw new NotImplementedException(); }
        }

        public IPOSApp POSApp
        {
            get { return new POSApp(); }
        }

        public DiscountCalculation DiscountCalculation
        {
            get { throw new NotImplementedException(); }
        }

        public bool TrainingMode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public RecordIdentifier StorePriceGroup
        {
            get { throw new NotImplementedException(); }
        }

        public bool TaxIncludedInPrice
        {
            get { throw new NotImplementedException(); }
        }

        
        public Store Store
        {
            get 
            { 
                if(store == null)
                {
                    store = new Store();
                }
                return store;
            }
            set
            {
                store = value;
            }
        }

        public Terminal Terminal
        {
            get { throw new NotImplementedException(); }
        }
        
        public HardwareProfile HardwareProfile
        {
            get 
            {
                if (hardwareProfile == null)
                {
                    hardwareProfile = new HardwareProfile();
                }
                return hardwareProfile;
            }
            set
            {
                hardwareProfile = value;
            }
        }

        public FunctionalityProfile FunctionalityProfile
        {
            get { throw new NotImplementedException(); }
        }

        public VisualProfile VisualProfile
        {
            get { return new VisualProfile(); }
        }

        public UserProfile UserProfile
        {
            get { return new UserProfile(); }
        }

        public SiteServiceProfile SiteServiceProfile
        {
            get { throw new NotImplementedException(); }
        }

        public SiteServiceProfile HospitalitySiteServiceProfile
        {
            get { throw new NotImplementedException(); }
        }

        public Parameters Parameters
        {
            get { throw new NotImplementedException(); }
        }

        public POSUser POSUser { get; set; }

        public CompanyInfo CompanyInfo
        {
            get { return new CompanyInfo(); }
        }

        public System.Globalization.CultureInfo CultureInfo
        {
            get { throw new NotImplementedException(); }
        }

        public MainFormInfo MainFormInfo
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime BusinessDay
        {
            get { throw new NotImplementedException();}
        }

        public DateTime BusinessSystemDay
        {
            get { throw new NotImplementedException(); }
        }

        public void ApplyCultureSettings()
        {
        }

        public bool LoadProfiles(IConnectionManager entry, RecordIdentifier userID)
        {
            return true;
        }

        public List<string> GetProfileLoadErrors(IConnectionManager entry)
        {
            return new List<string>();
        }

        public bool LoadVisualProfile(IConnectionManager entry, out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        public void ResetToPreviousState()
        {
            throw new NotImplementedException();
        }

        public void AddEmployeeToRecentList(Employee employee)
        {
            throw new NotImplementedException();
        }

        public List<Employee> GetRecentEmployees()
        {
            throw new NotImplementedException();
        }

        public bool ForceHospitalityExit
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool FinalizeSplitBill
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public RecordIdentifier SplitBillID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public KitchenServiceProfile KitchenServiceProfile
        {
            get { throw new NotImplementedException(); }
        }


        public ILicenseValidator License
        {
            get { throw new NotImplementedException(); }
        }


        public string CultureName
        {
            get { return "en-us"; }
        }


        public bool HardwareProfileNeedsSetup
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public bool RestartDevices
        {
            get { return false; }
            set { }
        }

        public bool IsBasic { get; set; }
        public bool SuppressUI { get; set; }
        public Image DefaultItemImage { get; }

        public bool NeedsEFTSetup
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }
    }

    internal class POSApp : IPOSApp
    {
        public bool RunOperation(POSOperations operationID, object extraInfo)
        {
            throw new NotImplementedException();
        }

        public bool RunOperation(POSOperations operationID, object extraInfo, ref IPosTransaction posTransaction)
        {
            throw new NotImplementedException();
        }

        public bool RunOperation(POSOperations operationID, object extraInfo, OperationInfo operationInfo, ref IPosTransaction posTransaction)
        {
            throw new NotImplementedException();
        }

        public void BusinessDateSet()
        {
            throw new NotImplementedException();
        }

        public void LogOffForce()
        {
            throw new NotImplementedException();
        }

        public void RunOpenMenu(RecordIdentifier menuID, ButtonGridsEnum buttonGrid)
        {
            throw new NotImplementedException();
        }

        public void LoadTouchLayout(RecordIdentifier layoutID)
        {
            throw new NotImplementedException();
        }

        public void ResetTouchLayout()
        {
            throw new NotImplementedException();
        }

        public void InitializeEngine(DBConnection dbConn)
        {
            throw new NotImplementedException();
        }

        public RunOpenMenuHandler RunOpenMenuHandler { get; set; }

        public POSLoadDesignHandler POSLoadDesignHandler { get; set; }

        public IWin32Window POSMainWindow { get; set; }
        #pragma warning disable 0067 //Event is never used warning
        public event POSSetFocusRequestEventHandler SetFocusRequest;
        public event POSStatusEnabledEventHandler POSStatusEnabled;
        public event POSStatusDisabledEventHandler POSStatusDisabled;
        #pragma warning restore 0067
    }
}
