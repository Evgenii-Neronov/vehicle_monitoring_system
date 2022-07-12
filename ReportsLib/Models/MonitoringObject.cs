namespace ReportsLib.Models
{
    public class MonitoringObject
    {
        public int Code { get; }

        public string Name { get; }

        public MonitoringObject(int code, string name)
        {
            this.Code = code;
            this.Name = name;
        }

    }
}
