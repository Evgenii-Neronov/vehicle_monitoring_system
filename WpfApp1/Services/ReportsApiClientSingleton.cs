using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ReportsLib;
using ReportsLib.Models;
using ReportsLib.Models.ReportServer;

namespace WpfApp1.Services
{
    internal class ReportsApiClientSingleton : ApiClientBase, IReportsDataAccess
    {
        private ReportsApiClientSingleton()
        {
        }

        private static ReportsApiClientSingleton _selft;

        private static object _lockObject = new();

        public static ReportsApiClientSingleton Instance {
            get
            {
                lock (_lockObject)
                {
                    _selft ??= new ReportsApiClientSingleton();
                }
                return _selft;
            }
        }

        public Task<List<ReportType>> GetReportTypesAsync()
        {
            return this.GetRequestAsync<List<ReportType>>("/Report/GetReportTypes");
        }

        public Task<List<MonitoringObject>> GetMonitoringObjectsAsync()
        {
            return this.GetRequestAsync<List<MonitoringObject>>("/Report/GetMonitoringObjects");
        }

        public Task<List<ReportPeriodicityDefine>> GetReportPeriodicityDefinesAsync()
        {
            return this.GetRequestAsync<List<ReportPeriodicityDefine>>("/Report/GetReportPeriodicityDefines");
        }

        public Task<BuildingParametersDefine> GetBuildingParametersDefineAsync(int reportTypeCode)
        {
            return this.GetRequestAsync<BuildingParametersDefine>($"/Report/GetBuildingParametersDefine?reportTypeCode={reportTypeCode}");
        }

        public Task<CreateReportResponse> CreateReportAsync(CreateReportRequest request)
        {
            return this.PostRequestAsync<CreateReportRequest, CreateReportResponse>("/Report/Create", request);
        }

        public Task<DeleteReportResponse> DeleteReportAsync(DeleteReportRequest request)
        {
            return this.PostRequestAsync<DeleteReportRequest, DeleteReportResponse>("/Report/Delete", request);
        }

        public Task<GetReportsResponse> GetReportsAsync(GetReportsRequest request)
        {
            return this.GetRequestAsync<GetReportsResponse>("/Report/GetAll");
        }
    }
}