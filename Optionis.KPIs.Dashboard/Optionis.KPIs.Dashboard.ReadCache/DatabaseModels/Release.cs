using System;
using SQLite;

namespace Optionis.KPIs.Dashboard.ReadCache.DatabaseModels
{
    public class Release : IAmADatabaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id{ get; set; }
     
        public DateTime ReleaseDate{ get; set; }
    }
}

