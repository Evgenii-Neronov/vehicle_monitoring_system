using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models.Validators.Rules
{
    internal class ShouldNotBeNullRule : IValidationRule
    {
        private readonly string _fieldName;
        public ShouldNotBeNullRule(string fieldName)
        {
            this._fieldName = fieldName;
        }

        public ValidationResult Validate(object value)
        {
            if (value is null)
                return ValidationResult.Failure($"{this._fieldName} must not be empty");

            return ValidationResult.Success;
        }
    }
}
