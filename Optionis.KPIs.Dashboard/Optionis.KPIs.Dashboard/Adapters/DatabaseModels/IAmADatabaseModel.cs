using SQLite;

namespace Optionis.KPIs.Dashboard.Adapters.DatabaseModels
{
	public interface IAmADatabaseModel
	{
        [PrimaryKey, AutoIncrement]
        int Id{ get; set; }
	}
}

