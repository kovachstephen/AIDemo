using MyWebApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IWhiskeyDataSource, JsonDataSource>();
builder.Services.AddSingleton<ICocktailDataSource, JsonDataSource>();
builder.Services.AddSingleton<IBrandDataSource, JsonDataSource>();

builder.Services.AddSingleton<IWhiskeyService, DataService>();
builder.Services.AddSingleton<ICocktailService, DataService>();
builder.Services.AddSingleton<IBrandService, DataService>();

builder.Services.AddSingleton<IRecipeService, RecipeService>();

builder.WebHost.UseUrls("http://localhost:8080");

var app = builder.Build();

app.MapRazorPages();

app.Lifetime.ApplicationStopping.Register(() => {
    Console.WriteLine("Application is stopping, closing TCP connections...");
});

app.Run();
