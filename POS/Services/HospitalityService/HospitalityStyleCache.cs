using System.Collections.Generic;
using System.Drawing;
using HospitalityInterface.Enums;
using LSRetail.StoreController.BusinessObjects.TouchButtons;
using LSRetail.StoreController.DataProviders.TouchButtons;
using LSRetail.Utilities.Enums;

namespace Hospitality
{
    public class HospitalityStyleCache
    {
        private static volatile HospitalityStyleCache instance;
        private static readonly object SyncLock = new object();

        private readonly Dictionary<DiningTableStatus, PosStyle> styles;

        public static HospitalityStyleCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncLock)
                    {
                        if (instance == null)
                        {
                            instance = new HospitalityStyleCache();
                        }
                    }
                }
                return instance;
            }
        }

        private HospitalityStyleCache()
        {
            styles = new Dictionary<DiningTableStatus, PosStyle>();
        }

        public PosStyle GetTableStyle(DiningTableStatus status)
        {
            lock (SyncLock)
            {
                if (!styles.ContainsKey(status))
                    SetStatusDefault(status);

                System.Diagnostics.Debug.Assert(styles.ContainsKey(status));
                if (!styles.ContainsKey(status))
                    return new PosStyle();

                return styles[status];
            }
        }

        #region Historical defaults
        private void SetStatusDefault(DiningTableStatus status)
        {
            var style = DataProviderFactory.Instance.GetProvider<IPosStyleData, PosStyle>().GetByName(DLLEntry.DataModel, "SystemHospitalityTable" + status);
            if (style != null)
            {
                styles[status] = style;
                return;
            }

            // No style found, create a style with the same default info as before
            style = new PosStyle();
            switch (status)
            {
                case DiningTableStatus.Available:            // Green
                    style.BackColor = Color.FromArgb(238, 251, 234).ToArgb();
                    style.BackColor2 = Color.FromArgb(154, 226, 145).ToArgb();
                    break;
                case DiningTableStatus.GuestsSeated:         // Yellow
                    style.BackColor = Color.FromArgb(248, 247, 215).ToArgb();
                    style.BackColor2 = Color.FromArgb(224, 227, 146).ToArgb();
                    break;
                case DiningTableStatus.OrderNotPrinted:      // Yellow
                    style.BackColor = Color.FromArgb(248, 247, 215).ToArgb();
                    style.BackColor2 = Color.FromArgb(224, 227, 146).ToArgb();
                    break;
                case DiningTableStatus.OrderPartiallyPrinted:// Yellow
                    style.BackColor = Color.FromArgb(248, 247, 215).ToArgb();
                    style.BackColor2 = Color.FromArgb(224, 227, 146).ToArgb();
                    break;
                case DiningTableStatus.OrderPrinted:         // Yellow
                    style.BackColor = Color.FromArgb(248, 247, 215).ToArgb();
                    style.BackColor2 = Color.FromArgb(224, 227, 146).ToArgb();
                    break;
                case DiningTableStatus.OrderStarted:         // Yellow
                    style.BackColor = Color.FromArgb(248, 247, 215).ToArgb();
                    style.BackColor2 = Color.FromArgb(224, 227, 146).ToArgb();
                    break;
                case DiningTableStatus.OrderFinished:        // Red
                    style.BackColor = Color.FromArgb(253, 239, 239).ToArgb();
                    style.BackColor2 = Color.FromArgb(233, 132, 129).ToArgb();
                    break;
                case DiningTableStatus.OrderConfirmed:       // Red
                    style.BackColor = Color.FromArgb(253, 239, 239).ToArgb();
                    style.BackColor2 = Color.FromArgb(233, 132, 129).ToArgb();
                    break;
                case DiningTableStatus.Locked:               // Blue
                    style.BackColor = Color.FromArgb(241,247,253).ToArgb();
                    style.BackColor2 = Color.FromArgb(145, 182, 227).ToArgb();
                    break;
                case DiningTableStatus.Unavailable:          // Grey
                    style.BackColor = Color.FromArgb(242, 242, 242).ToArgb();
                    style.BackColor2 = Color.FromArgb(207, 207, 207).ToArgb();                              
                    break;
                case DiningTableStatus.AlertNotServed:       // Yellow
                    style.BackColor = Color.FromArgb(248, 247, 215).ToArgb();
                    style.BackColor2 = Color.FromArgb(224, 227, 146).ToArgb();
                    break;
                default: // Grey
                    style.BackColor = Color.FromArgb(242, 242, 242).ToArgb();
                    style.BackColor2 = Color.FromArgb(207, 207, 207).ToArgb(); 
                    break;
            }

            style.Shape = ShapeEnum.RoundRectangle;
            style.GradientMode = GradientModeEnum.ForwardDiagonal;
            style.FontName = "Tahoma";
            style.FontSize = 14;

            styles[status] = style;
        }
        #endregion
    }
}
