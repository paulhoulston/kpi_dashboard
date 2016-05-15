using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Core;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.Application;
using System;

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
            using (var connection = new SqliteWrapper ().Connection ()) {
                var releaseId = connection.Insert (new Release {
                    Application = model.Application,
                    Comments = model.Comments,
                    Created = model.Created,
                    CreatedBy = model.CreatedBy,
                    Title = model.Title
                });
                connection.Insert (new Deployment {
                    DeploymentDate = model.DeploymentDate,
                    ReleaseId = releaseId,
                    Version = model.Version
                });
                if (model.Issues != null) {
                    foreach (var issue in model.Issues)
                        connection.Insert (new Issue {
                            IssueId = issue.Id,
                            Link = issue.Link,
                            Title = issue.Title,
                            ReleaseId = releaseId
                        });
                }
            }
        }
    }
}