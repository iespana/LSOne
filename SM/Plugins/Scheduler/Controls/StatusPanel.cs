using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public class StatusPanel : GroupPanel
    {
        private Label labelStatus;
        private SpinningProgress spinningProgress;
        private LinkLabel linkLabel;
        private bool autoLocate;

        private Control lastParent;

        public StatusPanel()
        {
            labelStatus = new Label();
            spinningProgress = new SpinningProgress();
            linkLabel = new LinkLabel();

            this.SuspendLayout();

            labelStatus.AutoSize = true;
            labelStatus.BackColor = System.Drawing.Color.Transparent;
            labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            labelStatus.ForeColor = System.Drawing.SystemColors.GrayText;
            labelStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelStatus.Location = new System.Drawing.Point(48, 32);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new System.Drawing.Size(90, 25);
            labelStatus.TabIndex = 1;

            spinningProgress.BackColor = Color.Transparent;
            spinningProgress.CausesValidation = false;
            spinningProgress.Location = new System.Drawing.Point(this.Margin.Left, 32);
            spinningProgress.Name = "spinningProgress";
            spinningProgress.Size = new System.Drawing.Size(32, 32);
            spinningProgress.TabIndex = 2;
            spinningProgress.TabStop = false;
            spinningProgress.Visible = false;

            linkLabel.AutoSize = true;
            linkLabel.BackColor = Color.Transparent;
            linkLabel.Location = new System.Drawing.Point(48, 48);
            linkLabel.Name = "linkLabel";
            linkLabel.Size = new System.Drawing.Size(100, 23);
            linkLabel.TabIndex = 0;
            linkLabel.TabStop = true;
            linkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel_LinkClicked);
            linkLabel.Visible = false;

            Controls.Add(this.labelStatus);
            Controls.Add(this.spinningProgress);
            Controls.Add(this.linkLabel);
            this.Size = new System.Drawing.Size(100, 100);
            this.ResumeLayout(false);
            this.PerformLayout();
        }


        [Category("Appearance")]
        public string StatusText
        {
            get { return labelStatus.Text; }
            set
            {
                labelStatus.Text = value;
                RecalculateXLayout();
            }
        }

        [DefaultValue(false)]
        [Category("Appearance")]
        public bool SpinningProgressVisible
        {
            get { return spinningProgress.Visible; }
            set
            {
                spinningProgress.Visible = value;
                RecalculateXLayout();
            }
        }


        [Category("Appearance")]
        public string ActionText
        {
            get
            {
                return linkLabel.Text;
            }
            set
            {
                linkLabel.Text = value;
                RecalculateXLayout();
            }
        }

        [DefaultValue(false)]
        [Category("Appearance")]
        public bool ActionVisible
        {
            get
            {
                return linkLabel.Visible;
            }
            set
            {
                linkLabel.Visible = value;
                RecalculateYLayout();
            }
        }


        [DefaultValue(true)]
        [Category("Behaviour")]
        public bool ActionEnabled
        {
            get
            {
                return linkLabel.Enabled;
            }
            set
            {
                linkLabel.Enabled = value;
            }
        }


        [DefaultValue(false)]
        [Category("Layout")]
        public bool AutoLocate
        {
            get { return autoLocate; }
            set
            {
                autoLocate = value;
                RecalculateLayout();
            }
        }


        [DefaultValue(typeof(Font), "Microsoft Sans Serif, 15.75pt")]
        public Font StatusFont
        {
            get { return labelStatus.Font; }
            set { labelStatus.Font = value; }
        }


        public event EventHandler ActionClicked;



        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (ActionClicked != null && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ActionClicked(this, EventArgs.Empty);
            }
        }



        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (lastParent != null)
            {
                lastParent.Resize -= new EventHandler(Parent_Resize);
            }

            if (Parent != null)
            {
                Parent.Resize += new EventHandler(Parent_Resize);
            }

            lastParent = Parent;
            RecalculateLayout();
        }


        private void Parent_Resize(object sender, EventArgs e)
        {
            if (AutoLocate)
            {
                RecalculateLayout();
            }
        }


        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                RecalculateLayout();
            }
        }

        protected override void OnMarginChanged(EventArgs e)
        {
            base.OnMarginChanged(e);
            RecalculateLayout();
        }

        protected override void OnAutoSizeChanged(EventArgs e)
        {
            base.OnAutoSizeChanged(e);
            if (Visible)
            {
                RecalculateLayout();
            }
        }


        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            if (!AutoSize)
            {
                RecalculateLayout();
            }
        }


        private void RecalculateLayout()
        {
            if (Visible && Parent != null)
            {
                this.Parent.SuspendLayout();

                DoRecalculateYLayout();
                DoRecalculateXLayout();

                this.Parent.ResumeLayout();
                this.Parent.PerformLayout();
            }
        }

        private void RecalculateXLayout()
        {
            if (Visible && Parent != null)
            {
                this.SuspendLayout();
                DoRecalculateXLayout();
                this.ResumeLayout();
                this.PerformLayout();
            }
        }


        private void RecalculateYLayout()
        {
            if (Visible && Parent != null)
            {
                this.SuspendLayout();
                DoRecalculateYLayout();
                this.ResumeLayout();
                this.PerformLayout();
            }
        }

        private void DoRecalculateYLayout()
        {
            if (AutoSize)
            {
                DoRecalculateYLayoutWithAutoSize();
            }
            else
            {
                DoRecalculateYLayoutNoAutoSize();
            }
        }

        private void DoRecalculateYLayoutWithAutoSize()
        {
            var padding = this.Margin;

            var spinningHeight = 0;
            if (spinningProgress.Visible)
            {
                spinningHeight = spinningProgress.Height;
            }

            int linkHeight = 0;
            if (linkLabel.Visible)
            {
                linkHeight = linkLabel.Height + (labelStatus.Height + 2) * 3 / 2;
            }

            int height =
                padding.Top +
                Math.Max(spinningHeight, labelStatus.Height) +
                linkHeight +
                padding.Bottom;

            this.Height = height;

            int areaHeight = this.ClientRectangle.Height - padding.Top - padding.Bottom;
            labelStatus.Top = (areaHeight - labelStatus.Height) / 2 + padding.Top;
            spinningProgress.Top = (areaHeight - spinningProgress.Height) / 2 + padding.Top;
            linkLabel.Top = labelStatus.Top + (labelStatus.Height + 2) * 3 / 2;

            if (AutoLocate)
            {
                DoRecalulateYPosition();
            }
        }


        private void DoRecalculateYLayoutNoAutoSize()
        {
            var spinningTop = (this.ClientRectangle.Height - spinningProgress.Height) / 2;
            var labelTop = (this.ClientRectangle.Height - labelStatus.Height) / 2;
            int linkLabelTop;
            int adjustment;
            if (spinningTop > labelTop)
            {
                linkLabelTop = spinningTop + spinningProgress.Height + 2;
                adjustment = (spinningProgress.Height + 2) / 2;
            }
            else
            {
                linkLabelTop = labelTop + labelStatus.Height + 2;
                adjustment = (labelStatus.Height + 2) / 2;
            }

            if (!linkLabel.Visible)
            {
                adjustment = 0;
            }

            spinningProgress.Top = spinningTop - adjustment;
            labelStatus.Top = labelTop - adjustment;
            linkLabel.Top = linkLabelTop;
        }


        private void DoRecalculateXLayout()
        {
            if (AutoSize)
            {
                DoRecalculateXLayoutWithAutoSize();
            }
            else
            {
                DoRecalculateXLayoutNoAutoSize();
            }
        }


        private void DoRecalculateXLayoutWithAutoSize()
        {
            var padding = this.Margin;

            spinningProgress.Left = padding.Left;

            int spinningWidth;
            if (spinningProgress.Visible)
            {
                spinningWidth = spinningProgress.Left + spinningProgress.Width + 4;
            }
            else
            {
                spinningWidth = padding.Left;
            }

            // Just a local block
            {
                int width = spinningWidth + padding.Right;
                if (labelStatus.Visible)
                {
                    width += labelStatus.Width;
                }

                this.Width = width;
            }

            labelStatus.Left = spinningWidth;
            linkLabel.Left = (this.ClientRectangle.Width - linkLabel.Width) / 2;

            if (AutoLocate)
            {
                DoRecalulateXPosition();
            }
        }


        private void DoRecalculateXLayoutNoAutoSize()
        {
            int spinningWidth = 0;
            if (spinningProgress.Visible)
            {
                spinningWidth = spinningProgress.Width + 4;
            }
            int areaWidth = spinningWidth;
            if (labelStatus.Visible)
            {
                areaWidth += labelStatus.Width;
            }

            spinningProgress.Left = (this.ClientRectangle.Width - areaWidth) / 2;
            labelStatus.Left = spinningProgress.Left + spinningWidth;
            linkLabel.Left = (this.ClientRectangle.Width - linkLabel.Width) / 2;
        }


        private void DoRecalulateXPosition()
        {
            this.Left = (this.Parent.ClientRectangle.Width - this.Width) / 2;
        }


        private void DoRecalulateYPosition()
        {
            this.Top = (this.Parent.ClientRectangle.Height - this.Height) / 2;
        }


    }
}
