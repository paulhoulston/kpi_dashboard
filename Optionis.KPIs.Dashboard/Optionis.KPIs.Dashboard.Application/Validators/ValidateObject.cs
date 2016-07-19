using System;
using System.Collections.Generic;

namespace Optionis.KPIs.Dashboard.Application.Validators
{
    public interface IValidateObjects<T, TU>
    {
        TU ValidationError { get; }
        bool IsValid(T obj);
    }

    public class ValidateObject<T, TU>
    {
        readonly IDictionary<TU, Func<T, bool>> _validators = new Dictionary<TU, Func<T, bool>>();
        readonly Action<TU> _onInvalid;
        readonly Action<T> _onValid;

        public ValidateObject (Action<TU> onInvalid, Action<T> onValid, params IValidateObjects<T, TU>[] validators)
        {
            _onInvalid = onInvalid;
            _onValid = onValid;
            foreach (var validator in validators) {
                _validators.Add (validator.ValidationError, validator.IsValid);
            }
        }

        public void IsValid(T obj)
        {
            foreach (var validationMethod in _validators) {
                if (!validationMethod.Value(obj)) {
                    _onInvalid (validationMethod.Key);
                    return;
                }
            }
            _onValid (obj);
        }
    }

}

