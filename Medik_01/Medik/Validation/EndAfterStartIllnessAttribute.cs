using System.ComponentModel.DataAnnotations;

namespace Medik.Validation
{
    public class EndAfterStartIllnessAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var model = (Models.MedDocumentation)validationContext.ObjectInstance;

            if (model.EndIllness.HasValue && model.EndIllness < model.StartIllness)
            {
                return new ValidationResult("End date must be after start date", new[] {nameof(model.EndIllness) });
            }

            return ValidationResult.Success;
        }
    }
}
