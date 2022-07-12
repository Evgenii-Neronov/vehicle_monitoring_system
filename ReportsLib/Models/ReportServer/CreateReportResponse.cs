namespace ReportsLib.Models.ReportServer
{
    public class CreateReportResponse : ResponseBase
    {
        public static CreateReportResponse Ok => new() { Success = true };
        public static CreateReportResponse Failure(string error) => new() { Success = false, Error = error };
    }
}