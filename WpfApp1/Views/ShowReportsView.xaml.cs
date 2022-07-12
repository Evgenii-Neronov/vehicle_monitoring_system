using System.Windows;
using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for ShowReportsView.xaml
    /// </summary>
    public partial class ShowReportsView : UserControl
    {
        private readonly ShowReportsViewModel _showReportsViewModel;

        public ShowReportsView()
        {
            InitializeComponent();

            this._showReportsViewModel = new ShowReportsViewModel();
            this.DataContext = this._showReportsViewModel;
            
        }

        private void OnDeleteReportClicked(object sender, RoutedEventArgs e)
        {
            this._showReportsViewModel.DeleteSelectedReport();
        }
    }
}
