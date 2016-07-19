using System;
using System.Collections.Generic;

namespace Optionis.KPIs.Dashboard.Application
{
    public class ApplicationListingService
    {
        readonly Action<IEnumerable<Application>> _onApplicationsRetrieved;
        readonly IListApplications _repository;

        public interface IListApplications
        {
            IEnumerable<Application> List();
        }

        public class Application
        {
            public string Name { get; set; }
        }

        public ApplicationListingService(Action<IEnumerable<Application>> onApplicationsRetrieved, IListApplications repository)
        {
            _onApplicationsRetrieved = onApplicationsRetrieved;
            _repository = repository;
        }

        public void List()
        {
            _onApplicationsRetrieved(_repository.List());
        }
    }
}