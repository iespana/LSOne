using System.Drawing;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;

namespace LSOne.Controls.Dialogs
{
    public static class TouchScrollButtonsListViewHelper
    {
        public static void SetNavigationButtons(this TouchScrollButtonPanel panel, bool appendToPanel = false)
        {
            if (!appendToPanel)
            {
                panel.Clear();
            }                       

            panel.AddButton("", NavigationBtnEnum.PageUp, Conversion.ToStr((int)NavigationBtnEnum.PageUp), image: Properties.Resources.double_arrow_up_16, imageAlignment: ImageAlignment.Center);
            panel.AddButton("", NavigationBtnEnum.Up, Conversion.ToStr((int)NavigationBtnEnum.Up), image: Properties.Resources.single_arrow_up_16, imageAlignment: ImageAlignment.Center);
            panel.AddButton("", NavigationBtnEnum.Down, Conversion.ToStr((int)NavigationBtnEnum.Down), image: Properties.Resources.single_arrow_down_16, imageAlignment: ImageAlignment.Center);
            panel.AddButton("", NavigationBtnEnum.PageDown, Conversion.ToStr((int)NavigationBtnEnum.PageDown), image: Properties.Resources.double_arrow_down_16, imageAlignment: ImageAlignment.Center);            
        }

        public static void SetAllFunctionButtons(this TouchScrollButtonPanel panel, bool appendToPanel = true)
        {
            SetFunctionButtons(panel, true, true, true, true, appendToPanel);
        }

        public static void SetFunctionButtons(this TouchScrollButtonPanel panel, bool clear, bool search, bool select, bool cancel, bool appendToPanel = true)
        {
            if (!appendToPanel)
            {
                panel.Clear();
            }

            if (clear)
            {
                panel.AddButton(Properties.Resources.Clear, NavigationBtnEnum.Clear, Conversion.ToStr((int)NavigationBtnEnum.Clear), dock: DockEnum.DockEnd);
            }

            if (search)
            {
                panel.AddButton(Properties.Resources.Search, NavigationBtnEnum.Search, Conversion.ToStr((int)NavigationBtnEnum.Search), TouchButtonType.Action, dock: DockEnum.DockEnd);
            }

            if (select)
            {
                panel.AddButton(Properties.Resources.Select, NavigationBtnEnum.Select, Conversion.ToStr((int)NavigationBtnEnum.Select), TouchButtonType.OK, DockEnum.DockEnd);
            }                        

            if (cancel)
            {
                panel.AddButton(Properties.Resources.CancelSearch, NavigationBtnEnum.Cancel, Conversion.ToStr((int)NavigationBtnEnum.Cancel), TouchButtonType.Cancel, DockEnum.DockEnd);
            }
        }
    }
}
