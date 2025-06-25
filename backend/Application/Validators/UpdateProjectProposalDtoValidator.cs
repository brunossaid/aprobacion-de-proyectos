using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class UpdateProjectProposalDtoValidator : AbstractValidator<UpdateProjectProposalDto>
    {
        public UpdateProjectProposalDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("El titulo no puede tener mas de 200 caracteres.")
                .When(x => x.Title != null);

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("La descripcion no puede tener más de 1000 caracteres.")
                .When(x => x.Description != null);

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("La duracion estimada debe ser mayor a 0.")
                .When(x => x.Duration.HasValue);
        }
    }
}
