
namespace LSOne.Controls.Dialogs.SelectionDialog
{
    /// <summary>
    /// Interface for setting data in the list view of <see cref="SelectionDialog"/>
    /// </summary>
    public interface ISelectionListView
    {
        /// <summary>
        /// Setup the columns of the listview
        /// </summary>
        /// <param name="listView">ListView on which to set the columns</param>
        void SetupListViewHeader(ListView listView);

        /// <summary>
        /// Load the data into the listview
        /// </summary>
        /// <param name="listView">ListView in which to load data</param>
        /// <param name="filter">Optional filter to apply to the data</param>
        void LoadData(ListView listView, string filter = "");
    }
}
