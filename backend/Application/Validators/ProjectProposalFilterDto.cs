using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class ProjectProposalFilterDtoValidator : AbstractValidator<ProjectProposalFilterDto>
    {
        public ProjectProposalFilterDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("El titulo no puede tener mas de 200 caracteres.")
                .When(x => x.Title != null);

            RuleFor(x => x.Status)
                .InclusiveBetween(1, 4).WithMessage("El estado debe estar entre 1 y 4.")
                .When(x => x.Status.HasValue);

            RuleFor(x => x.Applicant)
                .GreaterThan(0).WithMessage("El ID del creador debe ser mayor a 0.")
                .When(x => x.Applicant.HasValue);

            RuleFor(x => x.ApprovalUser)
                .GreaterThan(0).WithMessage("El ID del aprobador debe ser mayor a 0.")
                .When(x => x.ApprovalUser.HasValue);
        }
    }
}
