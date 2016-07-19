using System;

namespace Optionis.KPIs.Dashboard.Application.Validators
{
    public class ValidateLengthLessThan<T, TU> : IValidateObjects<T, TU>
    {
        readonly int _maxStringLength;
        readonly Func<TU> _getValidationError;
        readonly Func<T, string> _getProperty;

        public ValidateLengthLessThan(int maxStringLength, Func<TU> getValidationError, Func<T, string> getProperty)
        {
            _maxStringLength = maxStringLength;
            _getValidationError = getValidationError;
            _getProperty = getProperty;
        }

        public TU ValidationError { get { return _getValidationError(); } }

        public bool IsValid(T obj)
        {
            var property = _getProperty(obj);
            return string.IsNullOrEmpty(property) || property.Length <= _maxStringLength;
        }
    }
}
