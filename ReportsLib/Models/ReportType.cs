namespace ReportsLib.Models
{
    public class ReportType
    {
        public int Code { get; }
        public string Description { get; }

        public ReportType( int code, string description)
        {
            this.Code = code;
            this.Description = description;
        }
    }
}