using Musical_Theatre.Constants;
using Musical_Theatre.Data.Context;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace Musical_Theatre.Data.Models.Validation
{
    public class UniquePerformanceNameAttribute : ValidationAttribute
    {
        public UniquePerformanceNameAttribute(string nameOfId)
            => NameOfId = nameOfId;

        public string NameOfId { get; set; }

        public string GetErrorMessage(string name) => $"Performance with name {name} already exists in the database.";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Empty name is not allowed!");
            }

            var name = value.ToString();
            var _context = (Musical_TheatreContext)validationContext.GetService(typeof(Musical_TheatreContext));
            var property = validationContext.ObjectType.GetProperty(NameOfId);

            if (property != null)
            {
                var id = (int)property.GetValue(validationContext.ObjectInstance);

                try
                {
                    var match = _context.Performances.FirstOrDefault(p => p.Name == name && p.Id != id);

                    if (match != default)
                    {
                        return new ValidationResult(GetErrorMessage(name));
                    }
                }
                catch (MySqlException ex)
                {
                    return new ValidationResult(ErrorMessages.AccsessingError);
                }
                catch (Exception exception)
                {
                    return new ValidationResult(ErrorMessages.UnknownError);
                }
            }

            return ValidationResult.Success;
        }

    }
}