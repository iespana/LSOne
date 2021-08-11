using System.Drawing;
using System.Windows.Forms;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls
{
    public partial class KitchenDisplayPanePreview : UserControl
    {
        public KitchenDisplayPanePreview()
        {
            InitializeComponent();
        }

        public string Description
        {
            get 
            { 
                return lblDescription.Text; 
            }
            set
            { 
                lblDescription.Text = value; 
            }
        }

        public int X
        {
            get
            {
                return Location.X;
            }
            set
            {
                Location = new Point(value, Location.Y);
            }
        }

        public int Y
        {
            get
            {
                return Location.Y;
            }
            set
            {
                Location = new Point(Location.X, value);
            }
        }

        public void MoveX(int x)
        {
            Location = new Point(Location.X + x, Location.Y);
        }

        public void MoveY(int y)
        {
            Location = new Point(Location.X, Location.Y + y);
        }
    }
}