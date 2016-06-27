using Nancy;
using Optionis.KPIs.Common;
using Nancy.ModelBinding;
using Optionis.KPIs.Dashboard.Application;
using System.Collections.Generic;
using Optionis.KPIs.DataAccess;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class CreateUser : NancyModule
    {
        readonly static IDictionary<UserCreationService.ValidationError, string> _errorMessageLookup = new Dictionary<UserCreationService.ValidationError, string> {
            { UserCreationService.ValidationError.UserIsNull, "The user is null" },
            { UserCreationService.ValidationError.UserNameNotSet, "The user name must not be empty" },
            { UserCreationService.ValidationError.UserNameTooLong, "The user name cannot exceed 50 characters" }
        };

        class UserToCreate
        {
            public string UserName { get; set; }
        }

        public CreateUser ()
        {
            Post [Routing.Users.ROUTE] = _ => {
                var request = this.Bind<UserToCreate>();
                return PerformPost(request);
            };
        }
            
        Response PerformPost (UserToCreate user)
        {
            Response response = null;
            new UserCreationService (
                new UserCreator(),
                validationError => response = OnValidationError (validationError),
                userId => response = OnUserCreated(userId)
            ).Create (new UserCreationService.User {
                UserName = user.UserName
            });

            return response;
        }

        Response OnValidationError (UserCreationService.ValidationError validationError)
        {
            return Response.AsJson (new {
                Error = new {
                    Code = validationError.ToString (),
                    Message = _errorMessageLookup [validationError]
                }
            }, HttpStatusCode.BadRequest);
        }

        Response OnUserCreated (int userId)
        {
            return Response.AsJson (new {
                links = new {
                    self = Routing.Users.Get(userId)
                }
            }, HttpStatusCode.Created);
        }

    }
}

