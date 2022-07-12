using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WpfApp1.Models;
using WpfApp1.ViewModels;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CreateReportViewModel _createReportViewModel;
        private ShowReportsViewModel _showReportsViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnCreateReportClick(object sender, RoutedEventArgs e)
        {
            if (_createReportViewModel == null)
            {
                _createReportViewModel = new CreateReportViewModel();
                _createReportViewModel.OnConnectionStatusChanged += (x) =>
                {
                    this.SetConnectionStatus(x);
                };

                _createReportViewModel.InitAsync();
            }

            DataContext = _createReportViewModel;
        }

        private void OnShowReportsClick(object sender, RoutedEventArgs e)
        {
            if (_showReportsViewModel == null)
            {
                _showReportsViewModel = new ShowReportsViewModel();
                _showReportsViewModel.OnConnectionStatusChanged += (x) =>
                {
                    this.SetConnectionStatus(x);
                };

                _createReportViewModel.InitAsync();
            }

            DataContext = _showReportsViewModel;
        }

        private void SetConnectionStatus(ConnectingStatus status)
        {
            this.Dispatcher.Invoke(() => {
                switch (status)
                {
                    case ConnectingStatus.Start:
                        this.connectingStatusTxt.Foreground = Brushes.Black;
                        this.connectingStatusTxt.Text = "Connecting...";
                        break;
                    case ConnectingStatus.Complete:
                        this.connectingStatusTxt.Foreground = Brushes.Green;
                        this.connectingStatusTxt.Text = "Server connection is Ok";
                        break;
                    case ConnectingStatus.Error:
                        this.connectingStatusTxt.Foreground = Brushes.Red;
                        this.connectingStatusTxt.Text = "Server connection error";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(status), status, null);
                }
            });
        }
    }
}
