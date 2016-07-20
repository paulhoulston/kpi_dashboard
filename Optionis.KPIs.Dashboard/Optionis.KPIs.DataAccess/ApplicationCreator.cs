using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class ApplicationCreator : ApplicationCreationService.ICreateApplications
    {
        public void Create(ApplicationCreationService.Application application, Action<int> onApplicationCreated)
        {
            var applicationId = new DbWrapper().ExecuteScalar(SqlQueries.Queries[SqlQueries.Query.InsertApplication], new { applicationName = application.ApplicationName });
            onApplicationCreated(applicationId);
        }
    }
}