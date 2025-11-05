using FluentValidation;
using DemandManagement.Application.Requests;
using DemandManagement.Domain.Constants;

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
                    .MinimumLength(ValidationConstants.Demand.TitleMinLength)
                    .WithMessage($"El título debe tener al menos {ValidationConstants.Demand.TitleMinLength} caracteres")
                    .MaximumLength(ValidationConstants.Demand.TitleMaxLength)
                    .WithMessage($"El título no puede exceder {ValidationConstants.Demand.TitleMaxLength} caracteres");
            });

        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.Demand.DescriptionMaxLength)
            .WithMessage($"La descripción no puede exceder {ValidationConstants.Demand.DescriptionMaxLength} caracteres")
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