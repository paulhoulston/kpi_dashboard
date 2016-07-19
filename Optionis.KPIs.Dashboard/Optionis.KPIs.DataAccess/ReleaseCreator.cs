using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class ReleaseCreator : ReleseCreationService.ICreateReleases
    {
        public void Create(ReleseCreationService.ReleaseToCreate model, Action<int> onReleaseCreated)
        {
            var dbWrapper = new DbWrapper();
            int releaseId = dbWrapper.ExecuteScalar(SqlQueries.Queries[SqlQueries.Query.InsertRelease], new
            {
                application = model.Application,
                comments = model.Comments,
                created = model.Created,
                createdBy = model.CreatedBy,
                title = model.Title
            });

            new DeploymentCreator().CreateDeployment(new DeploymentCreationService.Deployment
            {
                DeploymentDate = model.DeploymentDate,
                ReleaseId = releaseId,
                Status = model.DeploymentStatus,
                Version = model.Version
            }, _ => onReleaseCreated(releaseId));
        }
    }
}