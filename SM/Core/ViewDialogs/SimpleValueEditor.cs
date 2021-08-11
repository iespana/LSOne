using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewCore.Dialogs
{
    public partial class SimpleValueEditor : DialogBase
    {
        public enum RulesEnum : int
        {
            None = 0,
            CanPressOkIfNotChanged = 0x1,
            CannotBeEmpty = 0x2
        };

        private string textValue;
        private double doubleValue;
        private bool   settingData;
        private RulesEnum rules;

        

        protected SimpleValueEditor()
            : base()
        {
            InitializeComponent();

            rules = RulesEnum.None;
            HasHelp = false;
        }

        public SimpleValueEditor(string caption, string textValue, int maxLength, RulesEnum rules)
            : this(caption,textValue,maxLength)
        {
            this.rules = rules;

            tbTextValue_TextChanged(this, EventArgs.Empty);
        }

        public SimpleValueEditor(string caption,string textValue,int maxLength)
            : this()
        {
            this.textValue = textValue;

            settingData = true;
            Header = caption;
            tbText.Text = textValue;
            tbText.MaxLength = maxLength;
            settingData = false;

            tbText.Visible = true;
            ntbText.Visible = false;
            
        }

        public SimpleValueEditor(string caption, double value, DecimalLimit limiter, double maxNumber, int maxLength)
            : this()
        {
            this.doubleValue = value;

            settingData = true;

            if (limiter.Max == 0)
            {
                ntbText.AllowDecimal = false;
                ntbText.MaxValue = maxNumber;
                ntbText.MaxLength = maxLength;
                ntbText.Value = value;
            }
            else
            {
                ntbText.AllowDecimal = true;
                ntbText.MaxValue = maxNumber;
                ntbText.MaxLength = maxLength;
                ntbText.SetValueWithLimit((decimal)value, limiter);
            }

            Header = caption;
            settingData = false;

            ntbText.Visible = true;
            tbText.Visible = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ntbText.Visible)
            {
                doubleValue = ntbText.Value;
            }
            else
            {
                textValue = tbText.Text;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        public string TextValue
        {
            get
            {
                return textValue;
            }
        }

        public double DoubleValue
        {
            get
            {
                return doubleValue;
            }
        }

        private void tbTextValue_TextChanged(object sender, EventArgs e)
        {
            if (!settingData)
            {
                bool enabled = false;

                if ((rules & RulesEnum.CanPressOkIfNotChanged) == RulesEnum.CanPressOkIfNotChanged)
                {
                    enabled = true;
                }
                else
                {
                    enabled = (tbText.Text != textValue);
                }

                if ((rules & RulesEnum.CannotBeEmpty) ==RulesEnum.CannotBeEmpty)
                {
                    if (tbText.Text.Trim() == "")
                    {
                        enabled = false;
                    }
                }

                btnOK.Enabled = enabled;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(222, 222, 222));

            e.Graphics.DrawLine(pen, 1, 1, panel2.Width, 1);

            pen.Dispose();
        }

        private void ntbText_TextChanged(object sender, EventArgs e)
        {
            if (!settingData)
            {
                btnOK.Enabled = (ntbText.Value != doubleValue);
            }
        }

       


    }
}