using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ReportsLib.Models;
using WpfApp1.ViewModels;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for CreateReportView.xaml
    /// </summary>
    public partial class CreateReportView : UserControl
    {
        private readonly CreateReportViewModel _createReportViewModel;
        public CreateReportView()
        {
            InitializeComponent();
            this._createReportViewModel = new CreateReportViewModel();
            this.DataContext = this._createReportViewModel;
        }

        private void OnCreateReportClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this._createReportViewModel.ValidateAll())
            {
                this._createReportViewModel.CreateReport();

                if (string.IsNullOrWhiteSpace(this._createReportViewModel.ServerError))
                {
                    MessageBox.Show("Report created successfully!");
                }
            }
        }

        private void OnSelectionChangedBuildingParameters(object sender, SelectionChangedEventArgs e)
        {
            _createReportViewModel.SelectedBuildingParameters = new List<BuildingParameter>();

            this._createReportViewModel.ClearErrorInfoBlock();

            if (sender is ListBox listBox)
            {
                
                foreach (var item in listBox.SelectedItems)
                {
                    if(item is BuildingParameter parameter)
                        _createReportViewModel.SelectedBuildingParameters.Add(parameter);
                }
            }
            else
            {
                throw new ArgumentException("Failed receive selected items for building parameters");
            }
        }

        private void OnMonitoringObjectsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _createReportViewModel.SelectedMonitoringObjects = new List<MonitoringObject>();

            this._createReportViewModel.ClearErrorInfoBlock();

            if (sender is ListBox listBox)
            {

                foreach (var item in listBox.SelectedItems)
                {
                    if (item is MonitoringObject parameter)
                        _createReportViewModel.SelectedMonitoringObjects.Add(parameter);
                }
            }
            else
            {
                throw new ArgumentException("Failed receive selected items for building parameters");
            }
        }
    }
}