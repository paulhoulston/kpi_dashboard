using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Modules.Routes;
using Optionis.KPIs.DataAccess;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class CreateIssue : NancyModule
    {
        static readonly IDictionary<IssueCreationService.ValidationError, string> _errorMessageLookup = new Dictionary<IssueCreationService.ValidationError, string>
        {
            { IssueCreationService.ValidationError.EmptyIssueId, "The Issue ID cannot be empty" },
            { IssueCreationService.ValidationError.EmptyTitle, "The title cannot be empty" },
            { IssueCreationService.ValidationError.InvalidIssueId, "The Issue ID cannot exceed 25 characters" },
            { IssueCreationService.ValidationError.InvalidLink, "The link cannot exceed 255 characters" },
            { IssueCreationService.ValidationError.InvalidTitle, "The title cannot exceed 255 characters" },
        };

        public CreateIssue()
        {
            Post[Routing.Issues.ROUTE] = _ =>
                Post[Routing.Deployments.ROUTE] = parameters =>
                {
                    var request = this.Bind<IssueCreationService.Issue>();
                    return PerformPost(request);
                };
        }

        Response PerformPost(IssueCreationService.Issue issue)
        {
            Response response = null;
            new IssueCreationService(
                validationError => response = new SerializedError(validationError, _errorMessageLookup[validationError], Response).ErrorResponse(),
                issueId => response = OnIssueCreated(issue.ReleaseId, issueId),
                new IssueCreator()
            ).Create(issue);

            return response;
        }

        Response OnIssueCreated(int releaseId, int issueId)
        {
            return Response.AsJson(new
            {
                links = new
                {
                    self = Routing.Issues.Get(releaseId, issueId)
                }
            }, HttpStatusCode.Created);
        }
    }
}