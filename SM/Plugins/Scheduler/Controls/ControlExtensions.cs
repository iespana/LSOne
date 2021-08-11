using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public static class ControlExtensions
    {
        public static void AddToContextMenu(this ContextButtons contextButtons, ContextMenuStrip contextMenu, int priority, EventHandler editClicked, EventHandler addClicked, EventHandler removeClicked)
        {
            AddToolStripButton(contextMenu, Properties.Resources.Edit, ContextButtons.GetEditButtonImage(), contextButtons.EditButtonEnabled, editClicked, priority, null/*editButtonId*/);
            priority += 100;
            AddToolStripButton(contextMenu, Properties.Resources.Add, ContextButtons.GetAddButtonImage(), contextButtons.AddButtonEnabled, addClicked, priority, null/*addButtonId*/);
            priority += 100;
            AddToolStripButton(contextMenu, Properties.Resources.Remove, ContextButtons.GetRemoveButtonImage(), contextButtons.RemoveButtonEnabled, removeClicked, priority, null/*removeButtonId*/);
        }


        private static void AddToolStripButton(ContextMenuStrip menu, string text, Image image, bool enabled, EventHandler eventHandler, int priority, string tag)
        {
            var item = new ExtendedMenuItem(text, image, priority, eventHandler);
            item.Enabled = enabled;
            item.Tag = tag;
            menu.Items.Add(item);
        }

        public static void AddToContextMenu(this ContextButton button, ContextMenuStrip contextMenu, Image image, string text, int priority, EventHandler clicked)
        {
            AddToolStripButton(contextMenu, text, image, button.Enabled, clicked, priority, null);
        }
    }
}
