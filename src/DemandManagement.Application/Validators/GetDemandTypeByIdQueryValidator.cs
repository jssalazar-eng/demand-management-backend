using FluentValidation;
using DemandManagement.Application.Requests;

namespace DemandManagement.Application.Validators;

public sealed class GetDemandTypeByIdQueryValidator : AbstractValidator<GetDemandTypeByIdQuery>
{
    public GetDemandTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El ID del tipo de demanda es obligatorio");
    }
}