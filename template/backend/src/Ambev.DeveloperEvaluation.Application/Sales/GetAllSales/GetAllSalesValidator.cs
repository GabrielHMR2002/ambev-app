using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
/// <summary>
/// Validator for getting all sales
/// </summary>
public class GetAllSalesValidator : AbstractValidator<GetAllSalesCommand>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public GetAllSalesValidator()
    {
    }
}