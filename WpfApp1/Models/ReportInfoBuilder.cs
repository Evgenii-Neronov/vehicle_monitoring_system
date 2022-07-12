using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportsLib;
using ReportsLib.Models;
using ReportsLib.Models.ReportServer;

namespace WpfApp1.Models
{
    public class ReportInfoBuilder : IReportInfoBuilder
    {
        private readonly IReportsDataAccess _reportsDataAccess;

        public ReportInfoBuilder(IReportsDataAccess reportsDataAccess)
        {
            _reportsDataAccess = reportsDataAccess;
        }

        public async Task<ReportInfo> BuildAsync(ReportModel reportModel)
        {
            var reportInfo = new ReportInfo();

            reportInfo.JobName = reportModel.JobName;
            reportInfo.ReportId = reportInfo.ReportId;
            reportInfo.FirstReportDateTime = reportModel.FirstReportDateTime;

            // monitoring objects
            reportInfo.MonitoringObjects = new List<MonitoringObject>();

            var monitoringObjectsList = await _reportsDataAccess.GetMonitoringObjectsAsync();
            var monitoringObjects = monitoringObjectsList.ToDictionary(x => x.Code);
            foreach (var objectCodeId in reportModel.MonitoringObjects)
            {
                reportInfo.MonitoringObjects.Add(monitoringObjects.ContainsKey(objectCodeId)
                    ? monitoringObjects[objectCodeId]
                    // if monitoring object doesn't exists no more:
                    : new MonitoringObject(objectCodeId, objectCodeId.ToString()));
            }

            // report type
            var reportTypesList = await this._reportsDataAccess.GetReportTypesAsync();
            var reportTypes = reportTypesList.ToDictionary(x => x.Code);
            reportInfo.ReportType = reportTypes.ContainsKey(reportModel.ReportTypeCode)
                ? reportTypes[reportModel.ReportTypeCode]
                // same case as above and below
                : new ReportType(reportModel.ReportTypeCode, reportModel.ReportTypeCode.ToString());

            // reportPeriodicity
            var reportPeriodicityDefinesList = await _reportsDataAccess.GetReportPeriodicityDefinesAsync();
            var periodicity = reportPeriodicityDefinesList.ToDictionary(x => x.Id);
            reportInfo.ReportPeriodicityDefine = periodicity.ContainsKey(reportModel.ReportPeriodicityId)
                ? periodicity[reportModel.ReportPeriodicityId]
                : new ReportPeriodicityDefine(reportModel.ReportPeriodicityId, reportModel.ReportPeriodicityId.ToString());


            // building parameters
            reportInfo.ReportBuildingParameters = new List<BuildingParameter>();
            var buildingParametersDefine = await _reportsDataAccess.GetBuildingParametersDefineAsync(reportInfo.ReportType.Code);
            var buildingParameters = buildingParametersDefine.BuildingParameters.ToDictionary(x => x.Id);
            foreach (var parameterCodeId in reportModel.ReportBuildingParametersIds)
            {
                reportInfo.ReportBuildingParameters.Add(buildingParameters.ContainsKey(parameterCodeId)
                    ? buildingParameters[parameterCodeId]
                    : new BuildingParameter(parameterCodeId, parameterCodeId.ToString()));
            }

            return reportInfo;
        }
    }
}