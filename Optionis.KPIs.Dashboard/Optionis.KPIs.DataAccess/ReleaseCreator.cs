using System;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class ReleaseCreator : ReleseCreationService.ICreateReleases
    {
        public const string INSERT_RELEASE_SQL = @"INSERT INTO Releases(Application, Comments, Created, CreatedBy, Title) VALUES ('@application', '@comments', @created, '@createdBy', '@title')";
        public const string INSERT_DEPLOYMENT_SQL = @"INSERT INTO Deployments(ReleaseId, DeploymentDate, DeploymentStatus, Version) VALUES (@releaseId, @deploymentDate, '@deploymentStatus', '@version')";
        public const string INSERT_ISSUE_SQL = @"INSERT INTO Issues(ReleaseId, IssueId, Link, Title) VALUES (@releaseId, '@issueId', '@link', '@title')";
        public void Create(ReleseCreationService.ReleaseToCreate model, Action<int> onReleaseCreated)
        {
            var dbWrapper = new DbWrapper();
            int releaseId = dbWrapper.ExecuteScalar(INSERT_RELEASE_SQL, new
            {
                application = model.Application,
                comments = model.Comments,
                created = model.Created,
                createdBy = model.CreatedBy,
                title = model.Title
            });
            dbWrapper.ExecuteScalar(INSERT_DEPLOYMENT_SQL, new
            {
                releaseId,
                deploymentDate = model.DeploymentDate,
                deploymentStatus = model.DeploymentStatus,
                version = model.Version
            });

            model.Issues.ForEachNullSafe(issue =>
           dbWrapper.ExecuteScalar(INSERT_ISSUE_SQL, new
           {
               releaseId,
               issueId = issue.Id,
               link = issue.Link,
               title = issue.Title
           }));

            onReleaseCreated(releaseId);
        }
    }
}