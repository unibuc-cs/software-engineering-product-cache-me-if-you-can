using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Developer_Toolbox.Models.CustomValidations
{
    public class MinimumCountAttribute : ValidationAttribute
    {
        private readonly int _minCount;

        // Constructor pentru a seta numărul minim de elemente
        public MinimumCountAttribute(int minCount)
        {
            _minCount = minCount;
        }

        // Metoda care validează valoarea
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Verifică dacă valoarea este o colecție
            if (value is ICollection collection && collection.Count < _minCount)
            {
                // Returnează eroarea dacă numărul de elemente este mai mic decât minimul
                return new ValidationResult(ErrorMessage ?? $"The collection must contain at least {_minCount} item(s).");
            }

            // Dacă este valid, returnează succes
            return ValidationResult.Success;
        }
    }
}
