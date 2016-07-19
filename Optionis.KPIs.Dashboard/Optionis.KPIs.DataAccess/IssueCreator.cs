using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class IssueCreator : IssueCreationService.ICreateIssues
    {
        public void Create(IssueCreationService.Issue issue, Action<int> onIssueCreated)
        {
            var issueId = new DbWrapper().ExecuteScalar(SqlQueries.Queries[SqlQueries.Query.InsertIssue], new
            {
                releaseId = issue.ReleaseId,
                issueId = issue.IssueId,
                link = issue.Link,
                title = issue.Title
            });
            onIssueCreated(issueId);
        }
    }
}