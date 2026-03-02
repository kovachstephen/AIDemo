using MyWebApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IDataSource, JsonDataSource>();
builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddSingleton<IRecipeService, RecipeService>();
builder.WebHost.UseUrls("http://localhost:8080");

var app = builder.Build();

app.MapRazorPages();

app.Lifetime.ApplicationStopping.Register(() => {
    Console.WriteLine("Application is stopping, closing TCP connections...");
});

app.Run();
