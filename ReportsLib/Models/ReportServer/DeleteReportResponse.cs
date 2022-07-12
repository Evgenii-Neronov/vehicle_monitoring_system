namespace ReportsLib.Models.ReportServer
{
    public class DeleteReportResponse : ResponseBase
    {
        public static DeleteReportResponse Ok => new() { Success = true };
        public static DeleteReportResponse Failure(string error) => new() { Success = false, Error = error };
    }
}
