using System;

namespace Optionis.KPIs.Dashboard.Application
{
    public class GetIssueService
    {
        readonly Action _onIssueNotFound;
        readonly Action<Issue> _onIssueFound;
        readonly IGetIssues _repository;

        public interface IGetIssues
        {
            void Get(int issueId, Action onIssueNotFound, Action<Issue> onIssueFound);
        }

        public class Issue
        {
            public int Id { get; set; }
            public int ReleaseId { get; set; }
            public string IssueId{ get; set; }
            public string Title{ get; set; }
            public string Link{ get; set; }
        }

        public GetIssueService (IGetIssues repository, Action onIssueNotFound, Action<Issue> onIssueFound)
        {
            _repository = repository;
            _onIssueNotFound = onIssueNotFound;
            _onIssueFound = onIssueFound;
        }

        public void Get (int issueId)
        {
            _repository.Get (issueId, _onIssueNotFound, _onIssueFound);
        }
    }
}

