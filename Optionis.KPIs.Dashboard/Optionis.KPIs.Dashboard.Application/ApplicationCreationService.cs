using System;
using System.ComponentModel;
using Optionis.KPIs.Dashboard.Application.Validators;

namespace Optionis.KPIs.Dashboard.Application
{
    public class ApplicationCreationService
    {
        readonly Action<ValidationError> _onApplicationNotCreated;
        readonly Action<int> _onApplicationCreated;
        readonly ICreateApplications _repository;
        readonly ValidateObject<Application, ValidationError> _validators;

        public interface ICreateApplications
        {
            void Create(Application application, Action<int> onApplicationCreated);
        }

        public ApplicationCreationService (ICreateApplications repository, Action<ValidationError> onApplicationNotCreated, Action<int> onApplicationCreated)
        {
            _repository = repository;
            _onApplicationCreated = onApplicationCreated;
            _onApplicationNotCreated = onApplicationNotCreated;
            _validators =
                new ValidateObject<Application, ValidationError>(
                    _onApplicationNotCreated,
                    DoCreateApplication,
                    new ValidateApplicationIsNotNull(),
                    new ValidateApplicationNameIsNotNull(),
                    new ValidateLengthLessThan<Application, ValidationError>(255, () => ValidationError.ApplicationNameTooLong, application => application.ApplicationName));
        }

        [DefaultValue(None)]
        public enum ValidationError
        {
            None = 0,
            ApplicationIsNull = 1,
            ApplicationNameNotSet = 2,
            ApplicationNameTooLong = 3
        }

        public class Application
        {
            public string ApplicationName { get; set; }
        }

        public void Create(Application application)
        {
            _validators.IsValid (application);
        }

        void DoCreateApplication(Application application)
        {
            _repository.Create (application, _onApplicationCreated);
        }

        class ValidateApplicationIsNotNull : IValidateObjects<Application, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.ApplicationIsNull; } }

            public bool IsValid(Application application)
            {
                return application != null;
            }
        }

        class ValidateApplicationNameIsNotNull : IValidateObjects<Application, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.ApplicationNameNotSet; } }

            public bool IsValid(Application application)
            {
                return !string.IsNullOrEmpty(application.ApplicationName);
            }
        }
    }
}