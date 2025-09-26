using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Validations
{
    public class TodayDateAttribute : ValidationAttribute
    {
        public TodayDateAttribute()
        {
            ErrorMessage = "La fecha y hora deben corresponder al día de hoy.";
        }

        public override bool IsValid(object? value)
        {
            if (value is not DateTime fecha)
                return false;

            var hoy = DateTime.Today;
            return fecha.Date == hoy; 
        }
    }
}
