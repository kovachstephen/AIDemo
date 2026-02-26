using System.Text.Json;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:8080");
var app = builder.Build();

app.Lifetime.ApplicationStopping.Register(() => {
    Console.WriteLine("Application is stopping, closing TCP connections...");
});

var jsonPath = Path.Combine(app.Environment.ContentRootPath, "data.json");
var jsonData = File.ReadAllText(jsonPath);

var cocktailPath = Path.Combine(app.Environment.ContentRootPath, "cocktails.json");
var cocktailData = File.ReadAllText(cocktailPath);

var brandPath = Path.Combine(app.Environment.ContentRootPath, "brands.json");
var brandData = File.ReadAllText(brandPath);

app.MapGet("/", () => {
    return Results.Text($@"<!DOCTYPE html>
<html> 
<head>
    <title>Whiskey & Cocktails</title>
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
    <style>
        .hero-section {{
            background: linear-gradient(rgba(0,0,0,0.6), rgba(0,0,0,0.6)), url('https://www.goodfreephotos.com/united-states/kentucky/other-kentucky/barrels-of-whiskey-in-the-cellar-at-buffalo-trace-distillery-kentucky.jpg');
            background-size: cover;
            background-position: center;
            min-height: 80vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }}
    </style>
</head>
<body>
    <div class=""hero-section text-center text-white"">
        <div class=""container"">
            <h1 class=""display-3 mb-4"">Welcome to Whiskey & Cocktails</h1>
            <p class=""lead mb-5"">Explore our collection of fine whiskeys and discover delicious cocktail recipes</p>
            <div class=""d-flex gap-3 justify-content-center"">
                <a href=""/whiskeys"" class=""btn btn-lg btn-outline-light"">Whiskey Collection</a>
                <a href=""/cocktails"" class=""btn btn-lg btn-primary"">View Cocktails</a>
            </div>
        </div>
    </div>
</body>
</html>", "text/html");
});

app.MapGet("/whiskeys", () => {
    var whiskeys = JsonSerializer.Deserialize<List<WhiskeyDto>>(jsonData)
        .Select(d => CreateWhiskey(d.Type, d.Brand, d.MashBill)).ToList();
    
    var spiritUrl = (string? name) => "/cocktails?spirit=" + (name?.ToLower() ?? "");
    var brandUrl = (string? name) => "/brand?b=" + (name?.Replace(" ", "-").Replace("'", "")?.ToLower() ?? "");
    var spiritLinks = string.Concat(whiskeys.Select(w => "<tr><td><a href=\"" + spiritUrl(w.Type) + "\" class=\"btn btn-link\">" + w.Type + "</a></td><td><a href=\"" + brandUrl(w.Brand) + "\" class=\"btn btn-link\">" + w.Brand + "</a></td><td>" + w.MashBill + "</td></tr>"));
    
    return Results.Text($@"<!DOCTYPE html>
<html> 
<head>
    <title>Whiskey Collection</title>
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
</head>
<body>
    <div class=""container mt-4"">
        <h1 class=""mb-4"">Whiskey Collection</h1>
        <div class=""mb-3"">
            <a href=""/"" class=""btn btn-outline-primary"">Back to Home</a>
            <a href=""/cocktails"" class=""btn btn-primary"">View Cocktails</a>
        </div>
        <input type=""text"" id=""search"" class=""form-control mb-3"" placeholder=""Search by type, brand, or mash bill..."" onkeyup=""filterTable()"">
        <table id=""whiskeyTable"" class=""table table-striped table-hover"">
            <thead class=""table-dark"">
                <tr><th>Type</th><th>Brand</th><th>Mash Bill</th></tr>
            </thead>
            <tbody>
                {spiritLinks}
            </tbody>
        </table>
    </div>
    <script>
        function filterTable() {{
            var input = document.getElementById('search');
            var filter = input.value.toLowerCase();
            var table = document.getElementById('whiskeyTable');
            var rows = table.getElementsByTagName('tr');
            for (var i = 1; i < rows.length; i++) {{
                var text = rows[i].textContent.toLowerCase();
                rows[i].style.display = text.includes(filter) ? '' : 'none';
            }}
        }}
    </script>
</body>
</html>", "text/html");
});

app.MapGet("/cocktails", (string? spirit) => {
    var cocktails = JsonSerializer.Deserialize<List<CocktailDto>>(cocktailData)
        .Select(d => CreateCocktail(d.Name, d.BaseSpirit, d.Image, d.Ingredients)).ToList();
    
    var filteredCocktails = string.IsNullOrEmpty(spirit) ? cocktails : 
        cocktails.Where(c => c.BaseSpirit?.ToLower() == spirit.ToLower()).ToList();
    
    var cocktailUrl = (string? name) => "/cocktail/" + (name?.ToLower().Replace(" ", "-") ?? "");
    var cocktailLinks = string.Concat(filteredCocktails.Select(c => "<tr><td><a href=\"" + cocktailUrl(c.Name) + "\" class=\"btn btn-link\">" + c.Name + "</a></td><td>" + c.BaseSpirit + "</td></tr>"));
    
    return Results.Text($@"<!DOCTYPE html>
<html> 
<head>
    <title>Cocktails</title>
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
</head>
<body>
    <div class=""container mt-4"">
        <h1 class=""mb-4"">Cocktails</h1>
        <div class=""mb-3"">
            <a href=""/"" class=""btn btn-outline-primary"">Back to Home</a>
            {(string.IsNullOrEmpty(spirit) ? "" : $"<a href=\"/cocktails\" class=\"btn btn-secondary\">Show all cocktails</a>")}
        </div>
        <input type=""text"" id=""search"" class=""form-control mb-3"" placeholder=""Search..."" onkeyup=""filterTable()"">
        <table id=""cocktailTable"" class=""table table-striped table-hover"">
            <thead class=""table-dark"">
                <tr><th>Name</th><th>Base Spirit</th></tr>
            </thead>
            <tbody>
                {cocktailLinks}
            </tbody>
        </table>
    </div>
    <script>
        function filterTable() {{
            var input = document.getElementById('search');
            var filter = input.value.toLowerCase();
            var table = document.getElementById('cocktailTable');
            var rows = table.getElementsByTagName('tr');
            for (var i = 1; i < rows.length; i++) {{
                var text = rows[i].textContent.toLowerCase();
                rows[i].style.display = text.includes(filter) ? '' : 'none';
            }}
        }}
    </script>
</body>
</html>", "text/html");
});

app.MapGet("/cocktail/{name}", (string name) => {
    var cocktails = JsonSerializer.Deserialize<List<CocktailDto>>(cocktailData)
        .Select(d => CreateCocktail(d.Name, d.BaseSpirit, d.Image, d.Ingredients)).ToList();
    var cocktail = cocktails.FirstOrDefault(c => c.Name?.ToLower().Replace(" ", "-") == name.ToLower());
    
    var recipe = cocktail?.Name switch
    {
        "Old Fashioned" => "Add sugar and bitters to a rocks glass. Muddle, add ice and bourbon. Stir, garnish with orange peel.",
        "Manhattan" => "Add rye, sweet vermouth, and bitters to a mixing glass with ice. Stir, strain into a coupe glass. Garnish with a cherry.",
        "Whiskey Sour" => "Add bourbon, lemon juice, simple syrup, and egg white to a shaker. Dry shake, add ice, shake again. Strain into a rocks glass with ice.",
        "Mule" => "Add whiskey to a copper mug filled with ice. Top with ginger beer, stir gently. Garnish with lime.",
        "Hot Toddy" => "Add bourbon, honey, and lemon juice to a heatproof glass. Top with hot water, stir well. Garnish with cinnamon stick.",
        "Rusty Nail" => "Add scotch and Drambuie to a rocks glass with ice. Stir well. Garnish with a lemon twist.",
        "Irish Coffee" => "Add Irish whiskey and brown sugar to a warm glass. Pour in hot coffee, stir. Float heavy cream on top.",
        "Jack and Coke" => "Add bourbon to a glass with ice. Top with cola, stir gently. Garnish with a lime wedge.",
        _ => "Mix ingredients according to taste preferences."
    };
    
    return Results.Text($@"<!DOCTYPE html>
<html> 
<head>
    <title>{cocktail?.Name ?? "Cocktail"} - Recipe</title>
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
</head>
<body>
    <div class=""container mt-4"">
        <h1 class=""mb-4"">{cocktail?.Name ?? "Cocktail"}</h1>
        <div class=""mb-3"">
            <a href=""/cocktails"" class=""btn btn-outline-primary"">Back to Cocktails</a>
        </div>
        {(cocktail != null ? $@"
        <div class=""row mb-4"">
            <div class=""col-md-6"">
                <img src=""{cocktail.Image}"" class=""img-fluid rounded"" alt=""{cocktail.Name}"">
            </div>
        </div>
        <h3>Ingredients</h3>
        <table class=""table table-striped mb-4"">
            <thead class=""table-dark"">
                <tr><th>Name</th><th>Volume</th><th>Preferred Brand</th></tr>
            </thead>
            <tbody>
                {cocktail.Ingredients.Select(i => $"<tr><td>{i.Name}</td><td>{i.Volume}</td><td>{i.PreferredBrand}</td></tr>").Aggregate("", (a, b) => a + b)}
            </tbody>
        </table>
        <h3>Recipe</h3>
        <div class=""card"">
            <div class=""card-body"">
                <p class=""lead"">{recipe}</p>
            </div>
        </div>" : "<p>Cocktail not found.</p>")}
    </div>
</body>
</html>", "text/html");
});

app.MapGet("/brand", (string? b) => {
    var brands = JsonSerializer.Deserialize<List<BrandDto>>(brandData).ToList();
    var brandKey = b?.Replace("-", " ").ToLower()?.Replace("'", "") ?? "";
    var brand = brands?.FirstOrDefault(x => x.Brand?.ToLower().Replace("'", "") == brandKey);
    
    return Results.Text($@"<!DOCTYPE html>
<html> 
<head>
    <title>{brand?.Brand ?? "Brand"} - Details</title>
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
</head>
<body>
    <div class=""container mt-4"">
        <h1 class=""mb-4"">{brand?.Brand ?? "Brand"}</h1>
        <div class=""mb-3"">
            <a href=""/"" class=""btn btn-outline-primary"">Back to Home</a>
            <a href=""/whiskeys"" class=""btn btn-secondary"">Whiskey Collection</a>
            <a href=""/cocktails"" class=""btn btn-primary"">View Cocktails</a>
        </div>
        {(brand != null ? $@"
        <div class=""row mb-4"">
            <div class=""col-md-6"">
                <img src=""{brand.Image}"" class=""img-fluid rounded"" alt=""{brand.Brand}"">
            </div>
        </div>
        <table class=""table table-striped mb-4"">
            <tbody>
                <tr><th>Distillery</th><td>{brand.Distillery}</td></tr>
                <tr><th>Location</th><td>{brand.Location}</td></tr>
                <tr><th>Founded</th><td>{brand.Founded}</td></tr>
                <tr><th>Description</th><td>{brand.Description}</td></tr>
            </tbody>
        </table>
        <h3>Location</h3>
        <iframe src=""{brand.MapUrl}"" width=""100%"" height=""400"" style=""border:0;"" allowfullscreen="""" loading=""lazy""></iframe>" : "<p>Brand not found.</p>")}
    </div>
</body>
</html>", "text/html");
});

ICocktail CreateCocktail(string? name, string? baseSpirit, string? image, List<Ingredient>? ingredients) => new Cocktail { Name = name, BaseSpirit = baseSpirit, Image = image, Ingredients = ingredients };

ILiquor CreateWhiskey(string? type, string? brand, string? mashBill) => type switch
{
    "Bourbon" => new Bourbon { Type = type, Brand = brand, MashBill = mashBill },
    "Rye" => new Rye { Type = type, Brand = brand, MashBill = mashBill },
    "Scotch" => new Scotch { Type = type, Brand = brand, MashBill = mashBill },
    "Irish Whiskey" => new IrishWhiskey { Type = type, Brand = brand, MashBill = mashBill },
    "Tennessee" => new Tennessee { Type = type, Brand = brand, MashBill = mashBill },
    _ => new Bourbon { Type = type, Brand = brand, MashBill = mashBill }
};

app.Run();

public interface ILiquor
{
    string? Type { get; set; }
    string? Brand { get; set; }
    string? MashBill { get; set; }
}

public class Bourbon : ILiquor
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }
    [JsonPropertyName("mashBill")]
    public string? MashBill { get; set; }
}

public class Rye : ILiquor
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }
    [JsonPropertyName("mashBill")]
    public string? MashBill { get; set; }
}

public class Scotch : ILiquor
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }
    [JsonPropertyName("mashBill")]
    public string? MashBill { get; set; }
}

public class IrishWhiskey : ILiquor
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }
    [JsonPropertyName("mashBill")]
    public string? MashBill { get; set; }
}

public class Tennessee : ILiquor
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }
    [JsonPropertyName("mashBill")]
    public string? MashBill { get; set; }
}

public class WhiskeyDto
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }
    [JsonPropertyName("mashBill")]
    public string? MashBill { get; set; }
}

public interface ICocktail
{
    string? Name { get; set; }
    string? BaseSpirit { get; set; }
    string? Image { get; set; }
    List<Ingredient>? Ingredients { get; set; }
}

public class Cocktail : ICocktail
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("baseSpirit")]
    public string? BaseSpirit { get; set; }
    [JsonPropertyName("image")]
    public string? Image { get; set; }
    [JsonPropertyName("ingredients")]
    public List<Ingredient>? Ingredients { get; set; }
}

public class Ingredient
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("volume")]
    public string? Volume { get; set; }
    [JsonPropertyName("preferredBrand")]
    public string? PreferredBrand { get; set; }
}

public class CocktailDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("baseSpirit")]
    public string? BaseSpirit { get; set; }
    [JsonPropertyName("image")]
    public string? Image { get; set; }
    [JsonPropertyName("ingredients")]
    public List<Ingredient>? Ingredients { get; set; }
}

public class BrandDto
{
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }
    [JsonPropertyName("distillery")]
    public string? Distillery { get; set; }
    [JsonPropertyName("location")]
    public string? Location { get; set; }
    [JsonPropertyName("founded")]
    public int Founded { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("image")]
    public string? Image { get; set; }
    [JsonPropertyName("mapUrl")]
    public string? MapUrl { get; set; }
}
