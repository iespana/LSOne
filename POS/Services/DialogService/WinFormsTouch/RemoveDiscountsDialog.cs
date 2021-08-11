using LSOne.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class RemoveDiscountsDialog : TouchBaseForm
    {

        public RemoveDiscountsDialog(string maxDiscountedPurchases, string currentDiscountedPurchases, string currencySymbol)
        {
            InitializeComponent();

            lblMessage.Text =
                String.Format(
                    "The current customer has gone over the maximum discounted purchases limit. With this sale he will reach {0} {2} but his limit is {1} {2}. Do you want to remove all the customer discounts or manually edit the sale ?", currentDiscountedPurchases, maxDiscountedPurchases, currencySymbol);
        }

        public static DialogResult Show(string maxDiscountedPurchases, string currentDiscountedPurchases, string currencySymbol)
        {
            using (var dlgMessage = new RemoveDiscountsDialog(maxDiscountedPurchases, currentDiscountedPurchases, currencySymbol))
            {
                return dlgMessage.ShowDialog();
            }
        }
    }
}