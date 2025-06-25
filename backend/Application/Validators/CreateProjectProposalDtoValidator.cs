using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class CreateProjectProposalDtoValidator : AbstractValidator<CreateProjectProposalDto>
    {
        public CreateProjectProposalDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El titulo es obligatorio.")
                .MaximumLength(200).WithMessage("El titulo no puede tener mas de 200 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripcion es obligatoria.")
                .MaximumLength(1000).WithMessage("La descripcion no puede tener mas de 1000 caracteres.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("El monto estimado debe ser mayor a 0.");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("La duracion estimada debe ser mayor a 0.");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("El area debe ser un valor valido.");

            RuleFor(x => x.User)
                .GreaterThan(0).WithMessage("El ID del creador debe ser un valor valido.");

            RuleFor(x => x.Type)
                .GreaterThan(0).WithMessage("El tipo de proyecto debe ser un valor valido.");
        }
    }
}
