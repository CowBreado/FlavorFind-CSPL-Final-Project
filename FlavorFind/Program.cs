using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentication/Login"; // Your login page
        options.LogoutPath = "/Authentication/Logout";
        options.AccessDeniedPath = "/Authentication/AccessDenied"; // Optional
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Cookie duration
        options.SlidingExpiration = true; // Renew cookie on activity
    });


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
   options.IdleTimeout = TimeSpan.FromMinutes(30);
   options.Cookie.HttpOnly = true;
   options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Or your custom error page
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// app.UseSession(); // If using session, place before UseAuthentication & UseAuthorization

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();  // Enable authorization middleware

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Welcome}/{id?}");

app.Run();
