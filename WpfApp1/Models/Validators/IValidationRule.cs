namespace WpfApp1.Models.Validators
{
    public interface IValidationRule
    {
        ValidationResult Validate(object value);
    }
}