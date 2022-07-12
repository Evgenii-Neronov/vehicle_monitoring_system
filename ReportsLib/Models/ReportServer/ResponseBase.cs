namespace ReportsLib.Models.ReportServer
{
    public abstract class ResponseBase
    {
        public bool Success { get; set; }

        public string Error { get; set; }
    }
}
