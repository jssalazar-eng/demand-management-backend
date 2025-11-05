using FluentValidation;
using DemandManagement.Application.Requests;
using DemandManagement.Domain.Constants;

namespace DemandManagement.Application.Validators;

public sealed class GetFilteredDemandsQueryValidator : AbstractValidator<GetFilteredDemandsQuery>
{
    public GetFilteredDemandsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("El número de página debe ser mayor a 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("El tamaño de página debe ser mayor a 0")
            .LessThanOrEqualTo(ValidationConstants.Pagination.MaxPageSize)
            .WithMessage($"El tamaño de página no puede exceder {ValidationConstants.Pagination.MaxPageSize}");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(ValidationConstants.Search.SearchTermMaxLength)
            .WithMessage($"El término de búsqueda no puede exceder {ValidationConstants.Search.SearchTermMaxLength} caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Prioridad inválida")
            .When(x => x.Priority.HasValue);
    }
}