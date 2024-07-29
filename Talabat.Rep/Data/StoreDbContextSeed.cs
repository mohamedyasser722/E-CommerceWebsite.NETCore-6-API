using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Repository.Data;

public static class StoreDbContextSeed
{
    public static async Task SeedAsync(StoreDbContext storeDbContext)
    {
        string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "SeedData");

        //Console.WriteLine($"Base Path: {basePath}"); // Add this line for debugging
        if (!storeDbContext.ProductBrands.Any())
        {
            var brandsData = await File.ReadAllTextAsync(Path.Combine(basePath, "brands.json"));
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            if (brands?.Count() > 0)
                await storeDbContext.ProductBrands.AddRangeAsync(brands);
        }

        if (!storeDbContext.ProductCategories.Any())
        {
            var categoriesData = await File.ReadAllTextAsync(Path.Combine(basePath, "categories.json"));
            var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);
            if (categories?.Count() > 0)
                await storeDbContext.ProductCategories.AddRangeAsync(categories);
        }

        if (!storeDbContext.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync(Path.Combine(basePath, "products.json"));
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products?.Count > 0)
                await storeDbContext.Products.AddRangeAsync(products);
        }
       
        if (!storeDbContext.DeliveryMethods.Any())
        {
            var DeliveryMethodsData = await File.ReadAllTextAsync(Path.Combine(basePath, "delivery.json"));
            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
            if (deliveryMethods?.Count > 0)
                await storeDbContext.DeliveryMethods.AddRangeAsync(deliveryMethods);
        }


        await storeDbContext.SaveChangesAsync();
    }
}
