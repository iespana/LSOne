using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class JobLogView : ViewBase
    {

        public JobLogView()
        {
            InitializeComponent();
            Attributes = 
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.JobLog;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }


        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.JobLog; 

            jobLogControl.Prepare( null);
            jobLogControl.Search();
        }
    }
}


        
    

