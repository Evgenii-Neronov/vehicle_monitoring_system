namespace WpfApp1.Models.Validators.Rules
{
    internal class JobNameValidationRule : IValidationRule
    {
        public ValidationResult Validate(object value)
        {
            if (value is string == false)
                return ValidationResult.Failure("Job name is not set");

            var jobName = (string)value;

            if (string.IsNullOrEmpty(jobName))
                return ValidationResult.Failure("Job must not be empty");

            return ValidationResult.Success;
        }
    }
}
