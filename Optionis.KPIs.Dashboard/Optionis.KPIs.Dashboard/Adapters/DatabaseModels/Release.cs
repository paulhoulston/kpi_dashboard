using System;
using SQLite;

namespace Optionis.KPIs.Dashboard.Adapters.DatabaseModels
{
    public class Release : IAmADatabaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id{ get; set; }
     
        public DateTime ReleaseDate{ get; set; }
    }
}

