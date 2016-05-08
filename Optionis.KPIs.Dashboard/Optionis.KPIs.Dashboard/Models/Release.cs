using System;
using SQLite;

namespace Optionis.KPIs.Dashboard.Models
{
    public class Release
    {
        [PrimaryKey, AutoIncrement]
        public int Id{ get; set; }
     
        public DateTime ReleaseDate{ get; set; }
    }
}

