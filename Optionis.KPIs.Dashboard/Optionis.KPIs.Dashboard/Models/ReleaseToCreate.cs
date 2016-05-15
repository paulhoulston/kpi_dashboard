namespace Optionis.KPIs.Dashboard
{
    public class ReleaseToCreate
    {
        public string Title{ get; set; }
        public int CreatedBy{ get; set; }
        public string Comments{ get; set; }
        public string Application{ get; set; }
        public string Version{ get; set; }
        public Issue[] Issues{get;set;}
        public string ChangeSets{ get; set; }
    }
}

