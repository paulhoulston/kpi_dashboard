using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Core;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.Application;
using System;
using SQLite;

namespace Optionis.KPIs.Adapters
{
    class ReleasesRepository : ReleasesLister.IStoreReleases, ReleseCreationService.ICreateReleases
    {
        public IEnumerable<Release> GetAll ()
        {
            using (var cnn = new SqliteWrapper ().Connection ()) {
                return cnn.Query<Release> ("SELECT Id FROM Release");
            }
        }

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
            return connection.Insert (new Release {
                Application = model.Application,
                Comments = model.Comments,
                Created = model.Created,
                CreatedBy = model.CreatedBy,
                Title = model.Title
            });
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