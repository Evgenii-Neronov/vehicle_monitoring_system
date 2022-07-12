using System.Collections.Generic;

namespace WpfApp1.Models.Validators
{
    internal interface IValidator
    {
        ValidationResult Validate(string propertyName, object propertyValue);

        void AddValidationRule(string propertyName, IValidationRule rule);

        Dictionary<string, string> GetValidationErrors();
    }
}
