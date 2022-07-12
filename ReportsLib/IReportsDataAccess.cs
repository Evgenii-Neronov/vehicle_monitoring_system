using ReportsLib.Models;
using ReportsLib.Models.ReportServer;

namespace ReportsLib
{
    public interface IReportsDataAccess
    {
        public Task<List<ReportType>> GetReportTypesAsync();
        public Task<List<MonitoringObject>> GetMonitoringObjectsAsync();
        public Task<List<ReportPeriodicityDefine>> GetReportPeriodicityDefinesAsync();
        public Task<BuildingParametersDefine> GetBuildingParametersDefineAsync(int reportTypeCode);
        public Task<CreateReportResponse> CreateReportAsync(CreateReportRequest request);
        public Task<DeleteReportResponse> DeleteReportAsync(DeleteReportRequest request);
        public Task<GetReportsResponse> GetReportsAsync(GetReportsRequest request);
    }
}