namespace WpfApp1.Models.Validators
{
    public class ValidationResult
    {
        public bool IsSuccess { get; set; }

        public string Error { get; set; }

        public ValidationResult()
        {
            IsSuccess = true;
        }

        public ValidationResult(string error)
        {
            IsSuccess = false;
            this.Error = error;
        }

        public static ValidationResult Success => new ValidationResult();

        public static ValidationResult Failure(string error) => new ValidationResult(error);
    }
}
