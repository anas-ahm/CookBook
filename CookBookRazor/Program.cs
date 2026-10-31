using CookBookLibrary.Repository;
using CookBookLibrary.Service;
using CookBookLibrary.Model;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/admin", "Admin");
});

// Adding authentication with cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/auth/Login"; //hvor du sendes hen hvis du prøver at tilgå en side der kræver login
    options.AccessDeniedPath = "/Index";  //hvor du sendes hen, hvis du er logget ind men prøver at tilgå en side du ikke er autoriseret til 
    options.Cookie.Name = "session-cookie"; // Set the name of the cookie
    options.Cookie.HttpOnly = true; // Prevent client side scripts from accessing the cookie
    options.Cookie.SameSite = SameSiteMode.Lax; // Restricts the cookie to same-site requests
    options.ExpireTimeSpan = TimeSpan.FromHours(8); // Set the expiration time for the cookie
    options.SlidingExpiration = true; // Increases the expiration time if the user is still active
});

// Adding autherization for sites
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AdminGuest", policy => policy.RequireRole("Admin", "Guest"));
});

// Connecting to service from class library to be able to use Methods
builder.Services.AddScoped<ICrud<FoodItem>, FoodItemRepo>();
builder.Services.AddScoped<ICrud<Recipe>, RecipeRepo>();
builder.Services.AddScoped<ICrud<Admin>, AdminRepo>();  
builder.Services.AddScoped<IAdmin, AdminRepo>();
builder.Services.AddScoped<IAdminAuth, AdminAuthService>();
builder.Services.AddScoped<IRecipeIngredient, RecipeRepo>();
builder.Services.AddScoped<DBWorker>();
builder.Services.AddScoped<FoodItemService>();
builder.Services.AddScoped<RecipeService>();

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

app.UseAuthentication(); // For authenticating login
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();