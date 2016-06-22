using System;
using System.Text.RegularExpressions;

namespace Optionis.KPIs.Dashboard.Application.Validators
{
    public interface IHaveAVersionNumber
    {
        string Version{ get; set; }
    }

    public class ValidateVersionNumber<T, TU> : IValidateObjects<T, TU> where T : IHaveAVersionNumber
    {
        readonly Func<TU> _getValidationError;

        public ValidateVersionNumber (Func<TU> getValidationError)
        {
            _getValidationError = getValidationError;
            
        }

        public TU ValidationError { get { return _getValidationError(); } }

        const string VERSION_REGEX = @"^\d+[.]\d+[.]\d+[.](\d+|\*)$";

        public bool IsValid(T validationObj)
        {
            return 
                !string.IsNullOrEmpty(validationObj.Version) &&
                new Regex (VERSION_REGEX).IsMatch (validationObj.Version);
        }
    }
}

