using Microsoft.AspNetCore.Mvc;
using ReportsLib;
using ReportsLib.Models;
using ReportsLib.Models.ReportServer;

namespace HttpReportServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IReportsDataAccess _reportService;

        public ReportController(ILogger<ReportController> logger, IReportsDataAccess reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }
        
        [Route("GetAll")]
        [HttpGet]
        public Task<GetReportsResponse> GetReports()
        {
            return this._reportService.GetReportsAsync(new GetReportsRequest());
        }
        
        [HttpPost]
        [Route("Create")]
        public Task<CreateReportResponse> CreateReport([FromBody]CreateReportRequest request)
        {
            return this._reportService.CreateReportAsync(request);
        }


        [HttpPost]
        [Route("Delete")]
        public Task<DeleteReportResponse> DeleteReport([FromBody]DeleteReportRequest request)
        {
            return this._reportService.DeleteReportAsync(request);
        }
        
        [HttpGet]
        [Route("GetReportTypes")]
        public Task<List<ReportType>> GetReportTypes()
        {
            return this._reportService.GetReportTypesAsync();
        }
        
        [HttpGet]
        [Route("GetMonitoringObjects")]
        public Task<List<MonitoringObject>> GetMonitoringObjects()
        {
            return this._reportService.GetMonitoringObjectsAsync();
        }

        [HttpGet]
        [Route("GetBuildingParametersDefine")]
        public Task<BuildingParametersDefine> GetBuildingParametersDefine(int reportTypeCode)
        {
            return this._reportService.GetBuildingParametersDefineAsync(reportTypeCode);
        }

        [HttpGet]
        [Route("GetReportPeriodicityDefines")]
        public Task<List<ReportPeriodicityDefine>> GetReportPeriodicityDefines()
        {
            return this._reportService.GetReportPeriodicityDefinesAsync();
        }
    }
}