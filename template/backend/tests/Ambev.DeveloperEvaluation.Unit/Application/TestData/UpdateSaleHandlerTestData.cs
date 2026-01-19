using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class UpdateSaleHandlerTestData
{
    private static readonly Faker<UpdateSaleItemCommand> itemFaker = new Faker<UpdateSaleItemCommand>()
        .RuleFor(i => i.Id, f => f.Random.Guid())
        .RuleFor(i => i.Product, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Number(1, 20))
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(10, 1000));

    private static readonly Faker<UpdateSaleCommand> saleFaker = new Faker<UpdateSaleCommand>()
        .RuleFor(s => s.Id, f => f.Random.Guid())
        .RuleFor(s => s.Customer, f => f.Company.CompanyName())
        .RuleFor(s => s.Branch, f => f.Address.City())
        .RuleFor(s => s.Items, f => itemFaker.Generate(f.Random.Number(1, 5)));

    public static UpdateSaleCommand GenerateValidCommand()
    {
        return saleFaker.Generate();
    }

    public static UpdateSaleCommand GenerateCommandWithQuantity(int quantity)
    {
        var command = saleFaker.Generate();
        command.Items = new List<UpdateSaleItemCommand>
        {
            new()
            {
                Product = "Test Product",
                Quantity = quantity,
                UnitPrice = 100m
            }
        };
        return command;
    }
}