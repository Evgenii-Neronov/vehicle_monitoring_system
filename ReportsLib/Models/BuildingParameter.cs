namespace ReportsLib.Models
{
    public class BuildingParametersDefine
    {
        public bool IsAllowMultiply { get; set; }
        public List<BuildingParameter> BuildingParameters { get; set; }
    }

    public class BuildingParameter
    {
        public int Id { get; }
        public string Description { get; }

        public BuildingParameter(int id, string description)
        {
            this.Id = id;
            this.Description = description;
        }
    }
}
