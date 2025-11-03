using FluentValidation;
using DemandManagement.Application.Requests;

namespace DemandManagement.Application.Validators;

public sealed class UpdateDemandCommandValidator : AbstractValidator<UpdateDemandCommand>
{
    public UpdateDemandCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El ID es obligatorio");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio")
            .MaximumLength(200).WithMessage("El título no puede exceder 200 caracteres")
            .MinimumLength(3).WithMessage("El título debe tener al menos 3 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("La descripción no puede exceder 2000 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Prioridad inválida");

        RuleFor(x => x.DemandTypeId)
            .NotEmpty().WithMessage("El tipo de demanda es obligatorio");
    }
}