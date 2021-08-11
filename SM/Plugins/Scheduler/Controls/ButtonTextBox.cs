using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public class ButtonTextBox : Control
    {
        private const int DefaultWidth = 100;
        private const int DefaultHeight = 20;

        private TextBox textBox;
        private ButtonItemCollection buttonItems;

        private bool internalSettingBackColor;
        private Color setBackColor;
        private bool internalSettingText;

        public ButtonTextBox()
        {
            textBox = new TextBox();
            //
            // textBox
            //
            textBox.Location = new Point(2, 3);
            textBox.BorderStyle = BorderStyle.None;
            textBox.Size = new Size(DefaultSize.Width - 6, DefaultHeight - 7);
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            textBox.TextChanged += textBox_TextChanged;
            textBox.Validating += textBox_Validating;
            textBox.Validated += textBox_Validated;
            textBox.KeyDown += textBox_KeyDown;

            Controls.Add(textBox);
            Size = DefaultSize;
            BackColor = ColorPalette.White;

            buttonItems = new ButtonItemCollection(this);
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && string.IsNullOrWhiteSpace(textBox.Text) == false)
                Clipboard.SetText(textBox.Text);
        }

        private void textBox_Validated(object sender, EventArgs e)
        {
            OnValidated(e);
        }

        private void textBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OnValidating(e);
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (internalSettingText) 
                return;

            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            internalSettingText = true;
            textBox.Text = Text;
            internalSettingText = false;
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(DefaultWidth, DefaultHeight);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rect = new Rectangle(this.DisplayRectangle.X, this.DisplayRectangle.Y, this.DisplayRectangle.Width + 1, this.DisplayRectangle.Height + 1);

                ButtonState state = ButtonState.Normal;
                if (!Enabled)
                {
                    state = ButtonState.Inactive;
                }
                else if (Focused)
                {
                    state = ButtonState.Pushed;
                }
                ControlPaint.DrawBorder3D(e.Graphics, rect);
                ControlPaint.DrawButton(e.Graphics, rect, state);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            if (!internalSettingBackColor)
            {
                setBackColor = BackColor;
            }
            textBox.BackColor = BackColor;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            internalSettingBackColor = true;
            if (Enabled)
            {
                BackColor = setBackColor;
            }
            else
            {
                BackColor = SystemColors.Control;
            }
            internalSettingBackColor = false;
        }

        private void ClearButtons()
        {
            int index = 0;
            while (index < Controls.Count)
            {
                var button = Controls[index] as Button;
                if (button != null)
                {
                    Controls.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }

        public override string Text
        {
            get
            {
                return textBox.Text;
            }
            set
            {
                textBox.Text = value;
            }
        }

        public void Clear()
        {
            textBox.Clear();
        }

        public int TextLength
        {
            get { return textBox.TextLength; }
        }

        public new event EventHandler TextChanged;


        public ButtonItemCollection Buttons
        {
            get { return buttonItems; }
        }

        public abstract class ButtonItem
        {
            public abstract Control Control { get; }
            public abstract int Width { get; set; }
            public abstract bool Visible { get; set; }
            public abstract bool Enabled { get; set; }
            
            internal ButtonItemCollection Owner { get; set; }
        }

        public class ButtonItemButton : ButtonItem
        {
            public ButtonItemButton()
            {
                Button = new Button();
                Button.Size = new Size(DefaultHeight - 2, DefaultHeight - 2);
                Button.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
                Button.FlatStyle = FlatStyle.Flat;
                Button.FlatAppearance.BorderSize = 0;
                Button.Click += Button_Click;
            }

            private void Button_Click(object sender, EventArgs e)
            {
                if (Click != null)
                {
                    Click(this, e);
                }
            }

            public override Control Control
            {
                get { return Button; }
            }

            public Button Button { get; private set; }

            public override int Width
            {
                get { return Button.Width; }
                set { Button.Width = value; }
            }

            public string Text
            {
                get { return Button.Text; }
                set { Button.Text = value; }
            }

            public Image Image
            {
                get { return Button.Image; }
                set { Button.Image = value; }
            }

            public override bool Visible
            {
                get { return Button.Visible; }
                set
                {
                    Button.Visible = value;
                    if (Owner != null)
                    {
                        Owner.RelocateControls();
                    }
                }
            }

            public override bool Enabled
            {
                get { return Button.Enabled; }
                set { Button.Enabled = value; }
            }

            public event EventHandler Click;
        }

        public class ButtonItemCheckBox : ButtonItem
        {
            public ButtonItemCheckBox()
            {
                CheckBox = new CheckBox();
                CheckBox.Size = new Size(DefaultHeight - 2, DefaultHeight - 2);
                CheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
                CheckBox.FlatStyle = FlatStyle.Flat;
                CheckBox.FlatAppearance.BorderSize = 0;
                CheckBox.FlatAppearance.CheckedBackColor = Color.FromArgb(0xB9, 0xD1, 0xEA);
                CheckBox.Appearance = Appearance.Button;
                CheckBox.CheckedChanged += CheckBox_CheckedChanged;
            }

            private void CheckBox_CheckedChanged(object sender, EventArgs e)
            {
                if (CheckedChanged != null)
                {
                    CheckedChanged(this, e);
                }
            }

            public override Control Control
            {
                get { return CheckBox; }
            }

            public CheckBox CheckBox { get; private set; }

            public override int Width
            {
                get { return CheckBox.Width; }
                set { CheckBox.Width = value; }
            }

            public string Text
            {
                get { return CheckBox.Text; }
                set { CheckBox.Text = value; }
            }

            public Image Image
            {
                get { return CheckBox.Image; }
                set { CheckBox.Image = value; }
            }

            public bool Checked
            {
                get { return CheckBox.Checked; }
                set { CheckBox.Checked = value; }
            }

            public override bool Visible
            {
                get { return CheckBox.Visible; }
                set
                {
                    CheckBox.Visible = value;
                    if (Owner != null)
                    {
                        Owner.RelocateControls();
                    }
                }
            }

            public override bool Enabled
            {
                get { return CheckBox.Enabled; }
                set { CheckBox.Enabled = value; }
            }

            public event EventHandler CheckedChanged;

        }

        public class ButtonItemCollection : ICollection<ButtonItem>
        {
            private ButtonTextBox owner;
            private List<ButtonItem> list = new List<ButtonItem>();

            public ButtonItemCollection(ButtonTextBox owner)
            {
                this.owner = owner;
            }

            public void Add(ButtonItem buttonItem)
            {
                list.Add(buttonItem);
                owner.Controls.Add(buttonItem.Control);
                buttonItem.Owner = this;
                RelocateControls();
            }

            public void Add(params ButtonItem[] buttonItems)
            {
                foreach (var item in buttonItems)
                {
                    list.Add(item);
                    owner.Controls.Add(item.Control);
                    item.Owner = this;
                }
                RelocateControls();
            }

            public bool Remove(ButtonItem buttonItem)
            {
                var result = list.Remove(buttonItem);
                if (result)
                {
                    owner.Controls.Remove(buttonItem.Control);
                    buttonItem.Owner = null;
                    RelocateControls();
                }
                return result;
            }

            internal void RelocateControls()
            {
                int x = owner.Width - 1;

                foreach (var textBoxButton in list)
                {
                    if (!textBoxButton.Visible) 
                        continue;

                    x -= textBoxButton.Control.Width;
                    textBoxButton.Control.Location = new Point(x, 1);
                }

                owner.textBox.Width = x - owner.textBox.Left - 1;
            }

            public void Clear()
            {
                list.Clear();
                owner.ClearButtons();
            }

            public bool Contains(ButtonItem item)
            {
                return list.Contains(item);
            }

            public void CopyTo(ButtonItem[] array, int arrayIndex)
            {
                list.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return list.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public IEnumerator<ButtonItem> GetEnumerator()
            {
                return list.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return ((System.Collections.IEnumerable)list).GetEnumerator();
            }
        }
    }
}
