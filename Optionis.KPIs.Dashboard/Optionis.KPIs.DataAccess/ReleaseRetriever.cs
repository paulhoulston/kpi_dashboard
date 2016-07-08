using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class ReleaseRetriever : GetReleaseService.IGetReleases
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["DeploymentsDb"].ConnectionString;

        class MultilpleResult
        {
            public ReleaseModel Release { get; set; }
            public IEnumerable<DeploymentModel> Deployments { get; set; }
            public IEnumerable<IssueModel> Issues { get; set; }
        }

        class ReleaseModel
        {
            public int Id { get; set; }
            public string Application { get; set; }
            public string Comments { get; set; }
            public DateTime Created { get; set; }
            public string CreatedBy { get; set; }
            public string Title { get; set; }
        }

        class DeploymentModel
        {
            public int DeploymentId { get; set; }
        }

        class IssueModel
        {
            public int IssueId { get; set; }
        }

        public void Get(int releaseId, Action onReleaseNotFound, Action<GetReleaseService.Release> onReleaseFound)
        {
            var release = QueryDatabase(releaseId);
            if (release != null && release.Release != null)
                onReleaseFound(Convert(release));
            else
                onReleaseNotFound();
        }

        static GetReleaseService.Release Convert(MultilpleResult data)
        {
            return new GetReleaseService.Release
            {
                Id = data.Release.Id,
                Application = data.Release.Application,
                Comments = data.Release.Comments,
                Created = data.Release.Created,
                CreatedBy = data.Release.CreatedBy,
                Title = data.Release.Title,
                DeploymentIds = data.Deployments.NullSafe().Select(d => d.DeploymentId),
                IssueIds = data.Issues.NullSafe().Select(i => i.IssueId)
            };
        }

        MultilpleResult QueryDatabase(int releaseId)
        {
            var results = new MultilpleResult();
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var multi = sqlConnection.QueryMultiple(SqlQueries.Queries[SqlQueries.Query.GetReleaseById], new { releaseId }))
                {
                    results.Release = multi.Read<ReleaseModel>().SingleOrDefault();
                    results.Deployments = multi.Read<DeploymentModel>().ToArray();
                    results.Issues = multi.Read<IssueModel>().ToArray();
                }
                sqlConnection.Close();
            }
            return results;
        }
    }
}
