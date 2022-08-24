using System.ComponentModel.DataAnnotations;

namespace GymApp14V1.Core.Filters
{
    public class DateLessThanNow : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            var message = "Selected date and time must be greated than now";

            if (value is DateTime dateTimeNow)
            {
                if (dateTimeNow > DateTime.Now)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(message);
                }
            }

            return new ValidationResult("Invalid date format");
        }
    }
}
