using Product.API.Entities;
using ILogger = Serilog.ILogger;

namespace Product.API.Persistence;

public static class ProductContextSeed
{
    public static async Task SeedProductAsync(ProductContext productContext, ILogger logger)
    {
        if (!productContext.Products.Any())
        {
            productContext.AddRange(GetCatalogProducts());
            await productContext.SaveChangesAsync();
            logger.Information($"Seeding {nameof(ProductContext)} database" );
        }
    }

    private static IEnumerable<CatalogProduct> GetCatalogProducts()
    {
        return new List<CatalogProduct>()
        {
            new()
            {
                No = "1",
                Name = "abc",
                Summary = "sdjfhs sdfgn",
                Description = "n sdf nsa sdfj dj ",
                Price = 1000
            },
            new ()
            {
                No = "2",
                Name = "abcdfh",
                Summary = "sdjfhs sdfgn d",
                Description = "n sdf nsa sdfj dj dge",
                Price = 2000
            }
        };
    }
}