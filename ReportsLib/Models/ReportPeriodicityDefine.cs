namespace ReportsLib.Models
{

    public class ReportPeriodicityDefine
    {
        public int Id { get; }
        public string Description { get; }

        public ReportPeriodicityDefine(int id, string description)
        {
            this.Id = id;
            this.Description = description;
        }
    }
}