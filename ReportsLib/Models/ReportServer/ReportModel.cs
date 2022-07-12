namespace ReportsLib.Models.ReportServer
{
    public class ReportModel
    {
        public Guid ReportId { get; set; }

        public string JobName { get; set; }

        public int ReportTypeCode { get; set; }

        public List<int> MonitoringObjects { get; set; }

        public DateTime FirstReportDateTime { get; set; }

        public int ReportPeriodicityId { get; set; }

        public List<int> ReportBuildingParametersIds { get; set; }
        
        public override string ToString()
        {
            return this.JobName;
        }
    }
}
