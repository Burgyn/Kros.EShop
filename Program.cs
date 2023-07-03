using Kros.EShop;
using Kros.Framework.Modules;
using Kros.Framework.UiSettings;
using Kros.Framework.UniCatalog;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddUiSettings();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddUniCatalog(c => c.AllowUnknownCatalogs = false)
    .WithCatalogs(c =>
    {
        c.AddCatalog("Countries");
        c.AddCatalog("Towns");
        c.AddCatalog("Banks");
        c.AddCatalog("Currencies");
        c.AddCatalog("ProductBrands");
        c.AddCatalog("ProductCategories");
    });

var appBuilder = builder.CreateAppBuilder(c =>
{
    c.AddAuditExtension();
    c.AddPriceRecalculationExtension();
    c.WithSqlServer<EShopDbContext>(useStoreInitializer: true);
});
appBuilder.AddModule<Address>();
appBuilder.AddModule<BankAccount>();
appBuilder.AddModule<Product, ProductDto, Product, Product, Guid>();
    //.WithApiBuilder<ReadOnlyApiBuilder<Product, ProductDto, Product, Product, Guid>>();

appBuilder.AddModule(new OrderModule());
appBuilder.AddModule<Basket>()
    .WithRepository<BasketRepository>();

appBuilder.AddModule<Invoice>()
    .WithApiBuilder<ReadOnlyApiBuilder<Invoice>>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseUiSettings();
app.UseUniCatalog();
app.UseModules();

app.Run();
