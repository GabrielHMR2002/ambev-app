using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class CreateSaleHandlerTestData
{
    private static readonly Faker<CreateSaleItemCommand> itemFaker = new Faker<CreateSaleItemCommand>()
        .RuleFor(i => i.Product, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Number(1, 20))
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(10, 1000));

    private static readonly Faker<CreateSaleCommand> saleFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10).ToUpper())
        .RuleFor(s => s.SaleDate, f => f.Date.Recent(30))
        .RuleFor(s => s.Customer, f => f.Company.CompanyName())
        .RuleFor(s => s.Branch, f => f.Address.City())
        .RuleFor(s => s.Items, f => itemFaker.Generate(f.Random.Number(1, 5)));

    public static CreateSaleCommand GenerateValidCommand()
    {
        return saleFaker.Generate();
    }

    public static CreateSaleCommand GenerateCommandWithItemQuantity(int quantity)
    {
        var command = saleFaker.Generate();
        command.Items = new List<CreateSaleItemCommand>
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

    public static CreateSaleCommand GenerateCommandWithMultipleItems(int itemCount)
    {
        var command = saleFaker.Generate();
        command.Items = itemFaker.Generate(itemCount);
        return command;
    }

    public static CreateSaleCommand GenerateInvalidCommand()
    {
        return new CreateSaleCommand
        {
            SaleNumber = "",
            Customer = "",
            Branch = "",
            Items = new List<CreateSaleItemCommand>()
        };
    }

    public static CreateSaleItemCommand GenerateItemWithQuantity(int quantity, decimal unitPrice = 100m)
    {
        return new CreateSaleItemCommand
        {
            Product = "Test Product",
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
}