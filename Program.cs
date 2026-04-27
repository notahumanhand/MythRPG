using Microsoft.EntityFrameworkCore;
using MythRPG.Components;
using MythRPG.Core;
using MythRPG.Data;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Identity;
using HealthChecks.SqlServer;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddDbContext<MythRPGContext>((serviceProvider, options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("MythRPG") ?? throw new Exception("Connection string 'MythRPG' not found.");
    options.UseSqlServer(connectionString,
            sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 2);
                sqlOptions.CommandTimeout(120);
            });
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<MythRPGContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
});

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRazorPages();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("MythRPG"));

builder.Services.AddTransient<ICharactersRepository, CharactersRepository>();
builder.Services.AddTransient<ITraitsRepository, TraitsRepository>();
builder.Services.AddTransient<ISpellsRepository, SpellsRepository>();

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapRazorPages();

app.MapHealthChecks("/health");

app.MapPost("/login-post", async (HttpContext context, SignInManager<User> signInManager, ILogger<Program> logger) =>
{
    var form = await context.Request.ReadFormAsync();

    var username = form["username"].FirstOrDefault();
    var password = form["password"].FirstOrDefault();

    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
        logger.LogWarning("USER_ACTION: Failed login attempt (missing credentials)");
        context.Response.Redirect("/login?error=true");
        return;
    }

    var userManager = signInManager.UserManager;
    var user = await userManager.FindByNameAsync(username);

    if (user == null)
    {
        logger.LogWarning("USER_ACTION: Failed login attempt for username {Username} (user not found)", username);
        context.Response.Redirect("/login?error=nouser");
        return;
    }

    var passwordValid = await userManager.CheckPasswordAsync(user, password);

    if (!passwordValid)
    {
        logger.LogWarning("USER_ACTION: Failed login attempt for username {Username} (bad password)", username);
        context.Response.Redirect("/login?error=badpassword");
        return;
    }

    var result = await signInManager.PasswordSignInAsync(username, password, false, false);

    if (result.Succeeded)
    {
        logger.LogInformation("USER_ACTION: User {Username} logged in", username);
        context.Response.Redirect("/");
    }
    else
    {
        logger.LogWarning("USER_ACTION: Failed login attempt for username {Username} (something went wrong)", username);
        context.Response.Redirect("/login?error=true");
    }
});
app.MapPost("/logout", async (HttpContext context, SignInManager<User> signInManager, ILogger<Program> logger) =>
{
    var username = context.User.Identity?.Name ?? "UNKNOWN";

    logger.LogInformation("USER_ACTION: User {Username} logged out", username);

    await signInManager.SignOutAsync();
    context.Response.Redirect("/login");
});

app.Run();
