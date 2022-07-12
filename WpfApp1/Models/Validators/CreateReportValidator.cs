using System.Collections.Generic;

namespace WpfApp1.Models.Validators
{
    internal class CreateReportValidator : IValidator
    {
        private readonly Dictionary<string, string> _validationErrors;

        private readonly Dictionary<string, IValidationRule> _validationRules;

        public CreateReportValidator()
        {
            _validationErrors = new Dictionary<string, string>();

            _validationRules = new Dictionary<string, IValidationRule>();
        }

        public void AddValidationRule(string propertyName, IValidationRule rule)
        {
            _validationRules[propertyName] = rule;
        }

        public Dictionary<string, string> GetValidationErrors()
        {
            return this._validationErrors;
        }

        public ValidationResult Validate(string propertyName, object propertyValue)
        {
            if (_validationRules.ContainsKey(propertyName))
            {
                var validationResult = _validationRules[propertyName].Validate(propertyValue);

                if (validationResult.IsSuccess == false)
                    _validationErrors[propertyName] = validationResult.Error;
                else
                    _validationErrors.Remove(propertyName);

                return validationResult;
            }

            return ValidationResult.Success;
        }
    }
}
