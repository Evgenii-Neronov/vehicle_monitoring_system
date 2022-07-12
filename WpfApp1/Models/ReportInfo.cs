using System;
using System.Collections.Generic;
using ReportsLib.Models;

namespace WpfApp1.Models
{
    public class ReportInfo
    {
        public Guid ReportId { get; set; }

        public string JobName { get; set; }

        public ReportType ReportType { get; set; }

        public List<MonitoringObject> MonitoringObjects { get; set; }

        public DateTime FirstReportDateTime { get; set; }

        public ReportPeriodicityDefine ReportPeriodicityDefine { get; set; }

        public List<BuildingParameter> ReportBuildingParameters { get; set; }
    }
}
