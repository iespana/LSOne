using LSOne.DataLayer.BusinessObjects;
using Microsoft.Reporting.WinForms;

namespace LSOne.ViewPlugins.ReportViewer
{
    class ReportViewerLocalizer : IReportViewerMessages
    {
        public string BackButtonToolTip
        {
            get { return Properties.Resources.BackToParentReport; } // Back to parent report
        }

        public string BackMenuItemText
        {
            get { return null; }
        }

        public string ChangeCredentialsText
        {
            get { return null; }
        }

        public string CurrentPageTextBoxToolTip
        {
            get { return Properties.Resources.CurrentPage; } // Current page
        }

        public string DocumentMapButtonToolTip
        {
            get { return null; }
        }

        public string DocumentMapMenuItemText
        {
            get { return Properties.Resources.DocumentMap; } // Document map
        }

        public string ExportButtonToolTip
        {
            get { return Properties.Resources.Export; } // Export
        }

        public string ExportMenuItemText
        {
            get { return Properties.Resources.Export; } // Export
        }

        public string FalseValueText
        {
            get { return null; }
        }

        public string FindButtonText
        {
            get { return Properties.Resources.Find; } // Find
        }

        public string FindButtonToolTip
        {
            get { return Properties.Resources.Find; } // Find
        }

        public string FindNextButtonText
        {
            get { return Properties.Resources.Next; } // Next
        }

        public string FindNextButtonToolTip
        {
            get { return Properties.Resources.FindNext; } // Find next
        }

        public string FirstPageButtonToolTip
        {
            get { return Properties.Resources.FirstPage; } // First page
        }

        public string LastPageButtonToolTip
        {
            get { return Properties.Resources.LastPage; } // Last page
        }

        public string NextPageButtonToolTip
        {
            get { return Properties.Resources.NextPage; } // Next page
        }

        public string NoMoreMatches
        {
            get { return Properties.Resources.NoMoreMatchesFound; } // No more matches found.
        }

        public string NullCheckBoxText
        {
            get { return null; }
        }

        public string NullCheckBoxToolTip
        {
            get { return null; }
        }

        public string NullValueText
        {
            get { return null; }
        }

        public string PageOf
        {
            get { return Properties.Resources.Of; } // of (as in page 1 of 2)
        }

        public string PageSetupButtonToolTip
        {
            get { return Properties.Resources.PageSetup; } // Page setup
        }

        public string PageSetupMenuItemText
        {
            get { return Properties.Resources.PageSetup; } // Page setup
        }

        public string ParameterAreaButtonToolTip
        {
            get { return null; }
        }

        public string PasswordPrompt
        {
            get { return null; }
        }

        public string PreviousPageButtonToolTip
        {
            get { return Properties.Resources.PreviousPage; } // Previous page
        }

        public string PrintButtonToolTip
        {
            get { return Properties.Resources.Print; } // Print
        }

        public string PrintLayoutButtonToolTip
        {
            get { return Properties.Resources.PrintLayout; } // Print layout
        }

        public string PrintLayoutMenuItemText
        {
            get { return Properties.Resources.PrintLayout; } // Print layout
        }

        public string PrintMenuItemText
        {
            get { return Properties.Resources.Print; } // Print
        }

        public string ProgressText
        {
            get { return Properties.Resources.Loading; } // Loading...
        }

        public string RefreshButtonToolTip
        {
            get { return Properties.Resources.Refresh; } // Refresh
        }

        public string RefreshMenuItemText
        {
            get { return Properties.Resources.Refresh; } // Refresh
        }

        public string SearchTextBoxToolTip
        {
            get { return Properties.Resources.FindTextInReport; } // Find text in repoprt
        }

        public string SelectAValue
        {
            get { return null; }
        }

        public string SelectAll
        {
            get { return null; }
        }

        public string StopButtonToolTip
        {
            get { return Properties.Resources.StopRendering; } // Stop rendering
        }

        public string StopMenuItemText
        {
            get { return Properties.Resources.Stop; } // Stop
        }

        public string TextNotFound
        {
            get { return null; }
        }

        public string TotalPagesToolTip
        {
            get { return Properties.Resources.TotalPages; } // Total pages
        }

        public string TrueValueText
        {
            get { return null; }
        }

        public string UserNamePrompt
        {
            get { return null; }
        }

        public string ViewReportButtonText
        {
            get { return null; }
        }

        public string ViewReportButtonToolTip
        {
            get { return null; }
        }

        public string ZoomControlToolTip
        {
            get { return Properties.Resources.Zoom; } // Zoom
        }

        public string ZoomMenuItemText
        {
            get { return Properties.Resources.Zoom; } // Zoom
        }

        public string ZoomToPageWidth
        {
            get { return Properties.Resources.PageWidth; } // Page width  (as in zoom to page width)
        }

        public string ZoomToWholePage
        {
            get { return Properties.Resources.WholePage; } // Whole page
        }
    }
}
