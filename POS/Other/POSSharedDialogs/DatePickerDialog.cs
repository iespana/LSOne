using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.Controls.Dialogs
{
    public partial class DatePickerDialog : TouchBaseForm
    {
        private bool inputRequired;
        private Point okButtonOriginalLocation;
        private Point cancelButtonOriginalLocation;

        /// <summary>
        /// Gets the date selected on the calendar
        /// </summary>
        public DateTime SelectedDate
        {
            get => calendarControl1.SelectedDate;
            set => calendarControl1.SelectedDate = value;
        }
        
        /// <summary>
        /// Gets or sets the maximum date that can be selected
        /// </summary>
        public DateTime MaximumDate
        {
            get => calendarControl1.MaxDate;
            set
            {
                calendarControl1.UseMaxDate = true;
                calendarControl1.MaxDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum date that can be selected
        /// </summary>
        public DateTime MinimumDate
        {
            get => calendarControl1.MinDate;
            set
            {
                calendarControl1.UseMinDate = true;
                calendarControl1.MinDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the text that is shown at the top of the dialog
        /// </summary>
        public string Caption
        {
            get => touchDialogBanner1.BannerText;
            set => touchDialogBanner1.BannerText = value;                        
        }        

        /// <summary>
        /// Gets or sets whether the user must select a date or if the user can press cancel
        /// </summary>
        public bool InputRequired
        {
            get
            {
                return inputRequired;
            }
            set
            {
                bool valueChanged = value != inputRequired;                                                
                inputRequired = value;

                if(valueChanged)
                {
                    ArrangeButtons();
                }
            }

        }

        /// <summary>
        /// Shows the dialog with the current date selected
        /// </summary>
        public DatePickerDialog()
        {            
            InitializeComponent();

            okButtonOriginalLocation = btnOK.Location;
            cancelButtonOriginalLocation = btnCancel.Location;

            inputRequired = false;
        }

        /// <summary>
        /// Shows the dialog with the given date selected
        /// </summary>
        /// <param name="selectedDate"></param>
        public DatePickerDialog(DateTime selectedDate)
            : this()
        {
            calendarControl1.SelectedDate = selectedDate;
        }

        private void ArrangeButtons()
        {
            btnCancel.Enabled = btnCancel.Visible = !inputRequired;
            btnOK.Location = inputRequired ? cancelButtonOriginalLocation : okButtonOriginalLocation;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
