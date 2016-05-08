using SQLite;

namespace Optionis.KPIs.Dashboard.ReadCache.DatabaseModels
{
	public interface IAmADatabaseModel
	{
        [PrimaryKey, AutoIncrement]
        int Id{ get; set; }
	}
}

