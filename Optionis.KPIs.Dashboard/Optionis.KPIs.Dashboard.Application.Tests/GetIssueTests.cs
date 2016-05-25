using System;
using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_get_an_issue
    {
        public class WHEN_the_issue_does_not_exist : GetIssueService.IGetIssues
        {
            public void Get (int issueId, Action onIssueNotFound, Action<GetIssueService.Issue> onIssueFound)
            {
                onIssueNotFound ();
            }

            [Test]
            public void THEN_the_issue_is_not_returned()
            {
                var isNotFound = false;
                GetIssueService.Issue issue = null;
                new GetIssueService (this, () => isNotFound = true, _ => issue = _).Get (0);
                Assert.IsTrue (isNotFound);
                Assert.IsNull (issue);
            }
        }

        public class WHEN_the_release_exists : GetIssueService.IGetIssues
        {
            public void Get (int releaseId, Action onIssueNotFound, Action<GetIssueService.Issue> onIssueFound)
            {
                onIssueFound (new GetIssueService.Issue());
            }

            [Test]
            public void THEN_the_release_is_returned()
            {
                var isNotFound = false;
                GetIssueService.Issue release = null;
                new GetIssueService (this, () => isNotFound = true, _ => release = _).Get(0);
                Assert.IsFalse (isNotFound);
                Assert.IsNotNull(release);
            }
        }
    }
}

