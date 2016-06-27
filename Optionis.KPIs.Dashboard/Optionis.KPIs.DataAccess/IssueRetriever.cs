using System;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class IssueRetriever : GetIssueService.IGetIssues
    {
        const string SQL = @"SELECT Id, ReleaseId, IssueId, Link, Title FROM Issues WHERE Id = @issueId";

        public void Get(int issueId, Action onIssueNotFound, Action<GetIssueService.Issue> onIssueFound)
        {
            var issue = new DbWrapper().GetSingle<GetIssueService.Issue>(SQL, new { issueId });
            if (issue == null)
                onIssueNotFound();
            else
                onIssueFound(issue);
        }
    }
}