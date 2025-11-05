using FluentValidation;
using DemandManagement.Application.Requests;
using DemandManagement.Domain.Constants;

namespace DemandManagement.Application.Validators;

public sealed class UpdateDemandCommandValidator : AbstractValidator<UpdateDemandCommand>
{
    public UpdateDemandCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("El ID es obligatorio");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("El título es obligatorio")
            .MinimumLength(ValidationConstants.Demand.TitleMinLength)
            .WithMessage($"El título debe tener al menos {ValidationConstants.Demand.TitleMinLength} caracteres")
            .MaximumLength(ValidationConstants.Demand.TitleMaxLength)
            .WithMessage($"El título no puede exceder {ValidationConstants.Demand.TitleMaxLength} caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.Demand.DescriptionMaxLength)
            .WithMessage($"La descripción no puede exceder {ValidationConstants.Demand.DescriptionMaxLength} caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Prioridad inválida");

        RuleFor(x => x.DemandTypeId)
            .NotEmpty()
            .WithMessage("El tipo de demanda es obligatorio");

        // ✅ Validación opcional para AssignedToId
        RuleFor(x => x.AssignedToId)
            .NotEqual(Guid.Empty)
            .WithMessage("El ID del usuario asignado no puede ser vacío")
            .When(x => x.AssignedToId.HasValue);
    }
}