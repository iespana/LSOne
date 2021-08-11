using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Utilities.ColorPalette;
using LSOne.Controls.SupportClasses;

namespace LSOne.Controls.Dialogs
{
    /// <summary>
    /// A touch friendly date picker control. Allows the user to select a date by using the <see cref="DatePickerDialog"/>
    /// </summary>
    public partial class DatePickerTouch : UserControl
    {
        private DateTime value;
        private DateTime maxDate;
        private DateTime minDate;        
        private bool showEmbeddedCheckBox;        
        private const int dateStringIndent = 10;        
        

        public DatePickerTouch()
        {
            InitializeComponent();
            DoubleBuffered = true;
            value = DateTime.Now;
            maxDate = DateTime.MaxValue;
            minDate = DateTime.MinValue;
            Size = new Size(300, 50);
            Font = new Font("Segoe UI", 12);

            showEmbeddedCheckBox = false;
            Checked = true;                                   
            embeddedCheckBox.Checked = true;
            embeddedCheckBox.Enabled = false;
            embeddedCheckBox.Visible = false;
            embeddedCheckBox.Location = new Point(dateStringIndent, Height / 2 - embeddedCheckBox.Size.Height / 2);            
        }

        /// <summary>
        /// Occurs when the date value is changed by the user
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Occurs when <see cref="ShowEmbeddedCheckBox"/> is set to "true" and the state of the embedded checkbox changes
        /// </summary>
        public event EventHandler CheckedChanged;

        /// <summary>
        /// Gets or sets the date value assigned to the control.
        /// </summary>
        [Category("Behaviour")]          
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime Value
        {
            // The DesignerSerializationVisibility attribute is set to Hidden so that the winForms designer doesn't assign a date value
            // to this property when you add it to a form. Otherwise the control will always have the date that it was added to the form as it's default
            // starting date instead of getting the current date at runtime.

            get => value;
            set
            {
                if(value > maxDate || value < minDate)
                {
                    return;
                }

                DateTime prevValue = this.value;
                this.value = value;

                if(prevValue != value)
                {
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the maximum date and time that can be selected in the control.
        /// </summary>
        [Category("Behaviour")]
        public DateTime MaxDate
        {
            get => maxDate;
            set
            {
                maxDate = value;
                ApplyMinMaxDate();
            }
        }

        /// <summary>
        /// Gets or sets the minimum date and time that can be selected in the control.
        /// </summary>
        [Category("Behaviour")]
        public DateTime MinDate
        {
            get => minDate;
            set
            {
                minDate = value;
                ApplyMinMaxDate();
            }
        }

        /// <summary>
        /// Indicates whether the Value property has been set
        /// </summary>
        [Category("Behaviour")]
        [DefaultValue(true)]
        public bool Checked 
        {
            get => embeddedCheckBox.Checked;
            set
            {
                embeddedCheckBox.Checked = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether to display and enable the embedded checkbox. The embedded checkbox controls the <see cref="Checked"/> property
        /// </summary>
        [Category("Behaviour")]
        [DefaultValue(false)]
        public bool ShowEmbeddedCheckBox 
        {
            get => showEmbeddedCheckBox;
            set
            {
                showEmbeddedCheckBox = value;

                embeddedCheckBox.Enabled = embeddedCheckBox.Visible = showEmbeddedCheckBox;

                Invalidate();
            }
        }        

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;                        
            
            // Draw the text
            SizeF stringSize = e.Graphics.MeasureString(value.ToShortDateString(), Font);


            Brush textBrush = new SolidBrush(Enabled && (!showEmbeddedCheckBox || Checked) ? ColorPalette.POSTextColor : ColorPalette.POSDisabledTextBox);
            e.Graphics.DrawString(value.ToShortDateString(), Font, textBrush, dateStringIndent + (showEmbeddedCheckBox ? embeddedCheckBox.Right : 0), Height / 2 - stringSize.Height / 2);
            textBrush.Dispose();

            // Draw the border
            Pen borderPen = new Pen(Focused ? ColorPalette.POSFocusedBorderColor : ColorPalette.POSControlBorderColor);
            Rectangle borderRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            e.Graphics.DrawRectangle(borderPen, borderRect);
            borderPen.Dispose();

            // Draw the calendar image
            Brush calendarBackgroundBrush = new SolidBrush(Enabled && (!showEmbeddedCheckBox || Checked) ? TouchButtonTypeHelper.GetBackColor(TouchButtonType.OK) : ColorPalette.POSDisabledTextBox);
            RectangleF calendarImageBackgroundRect = new Rectangle(Width - 50, 0, 50, 50);
            e.Graphics.FillRectangle(calendarBackgroundBrush, calendarImageBackgroundRect);
            calendarBackgroundBrush.Dispose();

            RectangleF calendarImageRect = new RectangleF(Width - 48, Height / 2 - 24, 48, 48);
            e.Graphics.DrawImage(Properties.Resources.Calendar_48px, calendarImageRect);
        }

        protected override void OnClick(EventArgs e)
        {
            if(showEmbeddedCheckBox && !Checked)
            {
                return;
            }

            base.OnClick(e);

            using (DatePickerDialog datePickerDialog = new DatePickerDialog(value))
            {
                datePickerDialog.MaximumDate = maxDate;
                datePickerDialog.MinimumDate = minDate;
                datePickerDialog.SelectedDate = value;

                if(datePickerDialog.ShowDialog() == DialogResult.OK)
                {
                    Value = datePickerDialog.SelectedDate;
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            Invalidate();
        }

        private void ApplyMinMaxDate()
        {
            bool invalidate = false;

            if(value > maxDate)
            {
                value = maxDate;
                invalidate = true;
            }
            else if (value < minDate)
            {
                value = minDate;
                invalidate = true;
            }

            if(invalidate)
            {
                Invalidate();
            }
        }

        private void embeddedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(!ShowEmbeddedCheckBox)
            {
                return;
            }

            Checked = embeddedCheckBox.Checked;

            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
