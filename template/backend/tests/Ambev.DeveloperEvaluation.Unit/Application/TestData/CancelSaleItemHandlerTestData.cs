namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class CancelSaleItemHandlerTestData
{
    public static (Guid SaleId, Guid ItemId) GenerateValidIds()
    {
        return (Guid.NewGuid(), Guid.NewGuid());
    }
}