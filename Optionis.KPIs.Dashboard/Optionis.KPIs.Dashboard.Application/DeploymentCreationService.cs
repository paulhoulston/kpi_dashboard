using System;
using Optionis.KPIs.Dashboard.Application.Validators;
using System.ComponentModel;

namespace Optionis.KPIs.Dashboard.Application
{
    public class DeploymentCreationService
    {
        readonly Action<int> _onDeploymentCreated;
        readonly ICreateDeployments _repository;
        readonly Action<ValidationError> _onValidationError;
        readonly ValidateObject<Deployment, ValidationError> _validators;

        public class Deployment : IHaveAVersionNumber, IHaveADeploymentDate
        {
            public int ReleaseId { get; set; }
            public string Version { get; set; }
            public DateTime DeploymentDate{ get; set; }
            public DeploymentStatus Status{ get; set; }
            public string Comments { get; set; }
        }

        [DefaultValue(None)]
        public enum ValidationError
        {
            None = 0,
            InvalidVersionNumber = 1,
            InvalidDeploymentDate = 2,
            InvalidComments = 3
        }

        public interface ICreateDeployments
        {
            void CreateDeployment(Deployment deployment, Action<int> onDeploymentCreated);
        }

        public DeploymentCreationService(Action<ValidationError> onValidationError, Action<int> onDeploymentCreated, ICreateDeployments repository)
        {
            _onValidationError = onValidationError;
            _repository = repository;
            _onDeploymentCreated = onDeploymentCreated;

            _validators =
                new ValidateObject<Deployment, ValidationError>(
                    _onValidationError,
                    Create,
                    new ValidateVersionNumber<Deployment, ValidationError>(() => ValidationError.InvalidVersionNumber),
                    new ValidateDeploymentDate<Deployment, ValidationError>(() => ValidationError.InvalidDeploymentDate),
                    new ValidateLengthLessThan<Deployment, ValidationError>(1000, () => ValidationError.InvalidComments, deployment => deployment.Comments));
        }

        public void CreateDeployment(Deployment deployment)
        {
            _validators.IsValid(deployment);
        }

        void Create(Deployment deployment)
        {
            _repository.CreateDeployment (deployment, _onDeploymentCreated);
        }
    }
}

