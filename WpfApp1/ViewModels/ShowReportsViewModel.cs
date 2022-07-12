using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReportsLib;
using ReportsLib.Models.ReportServer;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    internal class ShowReportsViewModel : ViewModelBase
    {
        private IReportsDataAccess _reportApiService;

        private IReportInfoBuilder _reportInfoBuilder;

        public List<ReportModel> Reports { get; set; }

        public ReportInfo ReportInfo { get; set; }

        public string DisplayError { get; set; }

        public ShowReportsViewModel()
        {
            InitAsync();
        }

        private async Task InitAsync()
        {
            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    this._reportApiService = ReportsApiClientSingleton.Instance;
                    this.InitConnectionEvents(this._reportApiService);
                    this._reportInfoBuilder = new ReportInfoBuilder(this._reportApiService);
                    LoadReports();
                }
                catch (ServerException e)
                {
                    this.DisplayError = e.Message;
                    OnPropertyChanged(nameof(this.DisplayError));
                }
            });
        }

        public async Task LoadReports()
        {
            try
            {
                var getReportsResponse = await this._reportApiService.GetReportsAsync(new GetReportsRequest());
                this.Reports = getReportsResponse.Reports;
                OnPropertyChanged(nameof(this.Reports));
            }
            catch (ServerException e)
            {
                this.DisplayError = e.Message;
                OnPropertyChanged(nameof(this.DisplayError));
            }
        }

        private ReportModel _reportModel;

        public ReportModel ReportModel
        {
            get => _reportModel;
            set
            {
                this._reportModel = value;
                InitReportInfo(value);
            }
        }

        public async Task InitReportInfo(ReportModel reportModel)
        {
            this.ReportInfo = await _reportInfoBuilder.BuildAsync(reportModel);
            OnPropertyChanged(nameof(this.ReportInfo));
        }

        public async Task DeleteSelectedReport()
        {
            await Task.Factory.StartNew(async () =>
            {
                this.DisplayError = string.Empty;

                try
                {
                    await this._reportApiService.DeleteReportAsync(new DeleteReportRequest()
                    {
                        ReportId = this.ReportModel.ReportId
                    });

                    var getReportsResponse = await this._reportApiService.GetReportsAsync(new GetReportsRequest());
                    this.Reports = getReportsResponse.Reports;
                    this.ReportInfo = null;
                }
                catch (Exception e)
                {
                    this.DisplayError = $"Can't delete report: {e.Message}";
                }

                OnPropertyChanged(nameof(this.Reports));
                OnPropertyChanged(nameof(this.ReportInfo));
                OnPropertyChanged(nameof(this.DisplayError));
            });
        }
    }
}
