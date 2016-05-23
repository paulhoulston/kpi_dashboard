using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.Application;
using System;
using SQLite;

namespace Optionis.KPIs.Adapters
{
    class ReleaseCreator : ReleseCreationService.ICreateReleases
    {
        public void Create (ReleseCreationService.ReleaseToCreate model, Action<int> onReleaseCreated)
        {
            int releaseId;
            using (var connection = new SqliteWrapper ().Connection ()) {
                releaseId = InsertRelease (model, connection);
                InsertDeployment (model, releaseId, connection);
                model.Issues.ForEachNullSafe (issue => InsertIssue (releaseId, connection, issue));
            }   
            onReleaseCreated (releaseId);
        }

        static int InsertRelease (ReleseCreationService.ReleaseToCreate model, SQLiteConnection connection)
        {
            var release = new Release {
                Application = model.Application,
                Comments = model.Comments,
                Created = model.Created,
                CreatedBy = model.CreatedBy,
                Title = model.Title
            };
            connection.Insert (release);
            return release.Id;
        }

        static void InsertDeployment (ReleseCreationService.ReleaseToCreate model, int releaseId, SQLiteConnection connection)
        {
            connection.Insert (new Deployment {
                DeploymentDate = model.DeploymentDate,
                ReleaseId = releaseId,
                Version = model.Version
            });
        }

        static void InsertIssue (int releaseId, SQLiteConnection connection, ReleseCreationService.Issue issue)
        {
            connection.Insert (new Issue {
                IssueId = issue.Id,
                Link = issue.Link,
                Title = issue.Title,
                ReleaseId = releaseId
            });
        }
    }
}