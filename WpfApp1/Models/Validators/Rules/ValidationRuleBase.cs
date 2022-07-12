using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models.Validators.Rules
{
    abstract class ValidationRuleBase
    {
        protected static object TryCastStringToInt(object value)
        {
            if (value is string)
            {
                if (int.TryParse((string)value, out var number))
                {
                    value = number;
                }
            }

            return value;
        }
    }
}
