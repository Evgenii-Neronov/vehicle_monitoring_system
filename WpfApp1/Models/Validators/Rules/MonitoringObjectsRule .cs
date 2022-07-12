using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportsLib.Models;

namespace WpfApp1.Models.Validators.Rules
{
    public class BuildingParametersRule : IValidationRule
    {
        public bool IsAllowMultiply { get; set; }

        public ValidationResult Validate(object value)
        {
            var buildingParameters = value as List<BuildingParameter>;

            if(buildingParameters == null || !buildingParameters.Any())
                return ValidationResult.Failure("You should set building parameters");

            if (IsAllowMultiply == false && buildingParameters.Count > 1)
                return ValidationResult.Failure("Building parameters must have one option");

            return ValidationResult.Success;
        }
    }
}
