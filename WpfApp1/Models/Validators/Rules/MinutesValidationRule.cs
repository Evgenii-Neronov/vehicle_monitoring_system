namespace WpfApp1.Models.Validators.Rules
{
    internal class MinutesValidationRule : ValidationRuleBase, IValidationRule
    {
        public ValidationResult Validate(object value)
        {
            value = TryCastStringToInt(value);

            if (value is int == false)
                return ValidationResult.Failure("Minutes must be a number");

            var minutes = (int)value;

            if (minutes < 0 || minutes > 59)
                return ValidationResult.Failure("Minutes must be a number between 0 and 59");

            return ValidationResult.Success;
        }
    }
}
