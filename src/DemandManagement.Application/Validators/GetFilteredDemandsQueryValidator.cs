using FluentValidation;
using DemandManagement.Application.Requests;

namespace DemandManagement.Application.Validators;

public sealed class GetFilteredDemandsQueryValidator : AbstractValidator<GetFilteredDemandsQuery>
{
    public GetFilteredDemandsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("El número de página debe ser mayor a 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("El tamaño de página debe ser mayor a 0")
            .LessThanOrEqualTo(100).WithMessage("El tamaño de página no puede exceder 100");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200).WithMessage("El término de búsqueda no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Prioridad inválida")
            .When(x => x.Priority.HasValue);
    }
}