using FluentValidation;
using DemandManagement.Application.Requests;

namespace DemandManagement.Application.Validators;

public sealed class CreateDemandCommandValidator : AbstractValidator<CreateDemandCommand>
{
    public CreateDemandCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio")
            .DependentRules(() =>
            {
                RuleFor(x => x.Title)
                    .MinimumLength(3).WithMessage("El título debe tener al menos 3 caracteres")
                    .MaximumLength(200).WithMessage("El título no puede exceder 200 caracteres");
            });

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("La descripción no puede exceder 2000 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Prioridad inválida");

        RuleFor(x => x.DemandTypeId)
            .NotEmpty().WithMessage("El tipo de demanda es obligatorio");

        RuleFor(x => x.StatusId)
            .NotEmpty().WithMessage("El estado es obligatorio");

        RuleFor(x => x.RequestingUserId)
            .NotEmpty().WithMessage("El usuario solicitante es obligatorio");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTimeOffset.UtcNow).WithMessage("La fecha de vencimiento debe ser futura")
            .When(x => x.DueDate.HasValue);
    }
}