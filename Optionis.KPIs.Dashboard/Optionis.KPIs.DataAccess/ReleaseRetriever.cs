using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapper;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class ReleaseRetriever : GetReleaseService.IGetReleases
    {
        static readonly string _sql;
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["DeploymentsDb"].ConnectionString;

        static ReleaseRetriever()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Optionis.KPIs.DataAccess.Queries.GetReleaseById.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                _sql = reader.ReadToEnd();
            }
        }

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
            var release = QueryDatabase(_sql, new { releaseId });
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

        MultilpleResult QueryDatabase(string sql, object sqlParams = null)
        {
            var results = new MultilpleResult();
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var multi = sqlConnection.QueryMultiple(sql, sqlParams))
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
