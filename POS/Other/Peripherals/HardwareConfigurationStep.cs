using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Peripherals.Dialogs;

namespace LSOne.Peripherals
{
    public class HardwareConfigurationStep
    {
        private string nextButtonCaption;

        public string NextButtonCaption
        {
            get { return string.IsNullOrEmpty(nextButtonCaption) ? "Next" : nextButtonCaption; }
            set { nextButtonCaption = value; }
        }
        
        public UserControl Step { get; set; }

        public bool BackAllowed { get; set; }

        public int StepIndex { get; set; }

        public List<HardwareConfigurationStep> ConfigurationSteps { get; set; }

        public HardwareConfigurationDialog Container { get; set; }
        public string Description { get; set; }


    }
}
