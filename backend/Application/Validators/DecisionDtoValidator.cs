using FluentValidation;
using Application.DTOs; 

namespace Application.Validators
{
    public class DecisionDtoValidator : AbstractValidator<DecisionDto>
    {
        public DecisionDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID del paso debe ser mayor a 0.");

            RuleFor(x => x.Status)
                .InclusiveBetween(1, 4).WithMessage("El estado debe estar entre 1 y 4.");

            RuleFor(x => x.User)
                .GreaterThan(0).WithMessage("El ID del usuario debe ser mayor a 0.");

            RuleFor(x => x.Observation)
                .MaximumLength(500).WithMessage("La observacion no puede superar los 500 caracteres.");
        }
    }
}
