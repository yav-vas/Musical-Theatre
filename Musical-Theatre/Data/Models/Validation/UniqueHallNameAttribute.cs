using Musical_Theatre.Constants;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Models;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace Musical_Theatre.Data.Models.Validation
{
    public class UniqueHallNameAttribute : ValidationAttribute
    {
        public UniqueHallNameAttribute(string nameOfId)
            => NameOfId = nameOfId;

        public string NameOfId { get; set; }

        public string GetErrorMessage(string name) => $"Hall with name {name} already exists in the database.";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var name = value.ToString();
            var _context = (Musical_TheatreContext)validationContext.GetService(typeof(Musical_TheatreContext));
            var property = validationContext.ObjectType.GetProperty(NameOfId);

            if (property != null)
            {
                var id = (int)property.GetValue(validationContext.ObjectInstance);

                try
                {
                    var match = _context.Halls.FirstOrDefault(h => h.Name == name && h.Id != id);

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
