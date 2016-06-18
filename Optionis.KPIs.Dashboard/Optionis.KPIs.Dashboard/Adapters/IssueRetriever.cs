using System;
using System.Linq;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;

namespace Optionis.KPIs.Dashboard.Adapters
{
    public class IssueRetriever : GetIssueService.IGetIssues
    {
        public void Get (int issueId, Action onIssueNotFound, Action<GetIssueService.Issue> onIssueFound)
        {
            using (var db = new SqliteWrapper ().Connection ()) {
                var issue = db.Table<Issue> ().Where (r => r.Id == issueId).SingleOrDefault ();

                if (issue == null) {
                    onIssueNotFound ();
                    return;
                }

                onIssueFound (new GetIssueService.Issue {
                    Id = issue.Id,
                    IssueId = issue.IssueId,
                    Link = issue.Link,
                    Title = issue.Title
                });
            }
        }
    }
}
