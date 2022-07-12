namespace WpfApp1.Models.Validators.Rules
{
    internal class HoursValidationRule : ValidationRuleBase, IValidationRule
    {
        public ValidationResult Validate(object value)
        {
            value = TryCastStringToInt(value);

            if (value is int == false)
                return ValidationResult.Failure("Hours must be a number");

            var hours = (int)value;

            if (hours < 0 || hours > 23)
                return ValidationResult.Failure("Hours must be a number between 0 and 23");

            return ValidationResult.Success;
        }
    }
}