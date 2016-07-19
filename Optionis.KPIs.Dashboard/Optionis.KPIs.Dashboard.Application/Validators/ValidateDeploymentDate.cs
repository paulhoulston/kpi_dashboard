using System;
using Optionis.KPIs.Dashboard.Application.Validators;

namespace Optionis.KPIs.Dashboard.Application
{
    public interface IHaveADeploymentDate
    {
        DateTime DeploymentDate { get;set;}
    }

    public class ValidateDeploymentDate<T, TU> : IValidateObjects<T, TU> where T : IHaveADeploymentDate
    {
        readonly Func<TU> _getValidationError;

        public ValidateDeploymentDate (Func<TU> getValidationError)
        {
            _getValidationError = getValidationError;

        }

        public TU ValidationError { get { return _getValidationError(); } }

        public bool IsValid (T validationObj)
        {
            return validationObj.DeploymentDate > DateTime.Today.AddDays(-30);
        }
    }
}

