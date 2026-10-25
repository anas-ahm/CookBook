using CookBookLibrary.Repository;
using CookBookLibrary.Service;
using CookBookLibrary.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Connecting to service from class library to be able to use Methods
builder.Services.AddScoped<ICrud<FoodItem>, FoodItemRepo>();
builder.Services.AddScoped<ICrud<Recipe>, RecipeRepo>();
builder.Services.AddScoped<IRecipeIngredient, RecipeRepo>();
builder.Services.AddScoped<DBWorker>();
builder.Services.AddScoped<CookBookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();