using System;
using SQLite;

namespace Optionis.KPIs.Dashboard.ReadCache.DatabaseModels
{
    public class Release : IAmADatabaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id{ get; set; }
     
        public DateTime Created{ get; set; }
        public int CreatedBy{ get; set; }
        public string Title{ get; set; }
        public string Application{ get; set; }
        public string Comments{ get; set; }
    }

    public class Deployment : IAmADatabaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id{ get; set; }
        public int ReleaseId{get;set;}
        public DateTime DeploymentDate { get; set; }
        public string Version{ get; set; }
    }

    public class Issue : IAmADatabaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id{ get; set; }
        public int ReleaseId{get;set;}
        public string IssueId{get;set;}
        public string Title{get;set;}
        public string Link{get;set;}
    }

    public class User : IAmADatabaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id{ get; set; }
        public string UserName{get;set;}
        public DateTime Created{get;set;}
    }
}

