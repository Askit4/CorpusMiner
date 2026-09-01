using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Azure.Communication.Email;
using CorpusMiner.Web.Components;
using CorpusMiner.Web.Components.Account;
using CorpusMiner.Web.Data;
using CorpusMiner.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddLocalization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

var acsConnectionString = builder.Configuration["Acs:ConnectionString"];
if (!string.IsNullOrWhiteSpace(acsConnectionString))
{
    builder.Services.AddSingleton(new EmailClient(acsConnectionString));
    builder.Services.AddSingleton<AzureEmailSender>();
    builder.Services.AddSingleton<IEmailSender<ApplicationUser>>(sp => sp.GetRequiredService<AzureEmailSender>());
    builder.Services.AddSingleton<INotificationEmailSender>(sp => sp.GetRequiredService<AzureEmailSender>());
}
else
{
    builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
    builder.Services.AddSingleton<INotificationEmailSender, NoOpNotificationEmailSender>();
}

builder.Services.AddScoped<MentionNotifier>();

var app = builder.Build();

var supportedCultures = new[] { "es", "en" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

app.MapGet("/culture/set", (string culture, string redirectUri, HttpContext httpContext) =>
{
    httpContext.Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true });
    return Results.LocalRedirect(redirectUri);
});

await SeedIdentityAsync(app.Services, app.Configuration, app.Logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();

// Crea los roles Admin/Contributor/Read y preregistra la cuenta admin inicial (ver Seed:AdminEmail).
// El password nunca se conoce en texto plano: el admin usa "Olvide mi password" para establecerlo.
static async Task SeedIdentityAsync(IServiceProvider services, IConfiguration configuration, ILogger logger)
{
    await using var scope = services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (dbContext.Database.IsRelational())
    {
        await dbContext.Database.MigrateAsync();
    }
    else
    {
        // Proveedores no relacionales (ej. InMemory en pruebas) no soportan migraciones.
        await dbContext.Database.EnsureCreatedAsync();
    }

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    foreach (var roleName in Roles.All)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminEmail = configuration["Seed:AdminEmail"];
    if (string.IsNullOrWhiteSpace(adminEmail))
    {
        return;
    }

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser is null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            IsApproved = true,
            ApprovedAtUtc = DateTimeOffset.UtcNow,
            ApprovedByUserId = "seed",
        };

        var randomPassword = Guid.NewGuid().ToString("N") + "Aa1!";
        var createResult = await userManager.CreateAsync(adminUser, randomPassword);
        if (!createResult.Succeeded)
        {
            logger.LogError("No se pudo preregistrar el admin {AdminEmail}: {Errors}", adminEmail,
                string.Join(", ", createResult.Errors.Select(e => e.Description)));
            return;
        }

        logger.LogInformation("Admin preregistrado: {AdminEmail}. Debe usar 'Olvide mi password' para establecer su acceso.", adminEmail);
    }

    if (!await userManager.IsInRoleAsync(adminUser, Roles.Admin))
    {
        await userManager.AddToRoleAsync(adminUser, Roles.Admin);
    }
}

public partial class Program { }
