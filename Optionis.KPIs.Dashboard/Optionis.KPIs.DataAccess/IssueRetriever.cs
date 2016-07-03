using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class IssueRetriever : GetIssueService.IGetIssues
    {
        public void Get(int issueId, Action onIssueNotFound, Action<GetIssueService.Issue> onIssueFound)
        {
            var issue = new DbWrapper().GetSingle<GetIssueService.Issue>(SqlQueries.Queries[SqlQueries.Query.GetIssueById], new { issueId });
            if (issue == null)
                onIssueNotFound();
            else
                onIssueFound(issue);
        }
    }
}