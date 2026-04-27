using Microsoft.EntityFrameworkCore;
using MythRPG.Components;
using MythRPG.Core;
using MythRPG.Data;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddDbContext<MythRPGContext>(options =>
{
options.UseSqlServer("Server=tcp:mythrpg-server.database.windows.net,1433;Initial Catalog=MythRPGDb;Persist Security Info=False;User ID=spleen;Password=GMilmc176;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount:2);
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

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddTransient<ICharactersRepository, CharactersRepository>();
builder.Services.AddTransient<ITraitsRepository, TraitsRepository>();
builder.Services.AddTransient<ISpellsRepository, SpellsRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    string adminUsername = "spleen";
    string adminPassword = "MMVkhld@87_";
    string fauxUsername = "faux";
    string fauxPassword = "MMVkdck@14&";

    var adminUser = await userManager.FindByNameAsync(adminUsername);
    var fauxUser = await userManager.FindByNameAsync(fauxUsername);

    if (adminUser == null)
    {
        adminUser = new User { UserName = adminUsername };

        var result = await userManager.CreateAsync(adminUser, adminPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        else
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
    if (fauxUser == null)
    {
        fauxUser = new User { UserName = fauxUsername };

        var result = await userManager.CreateAsync(fauxUser, fauxPassword);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}

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

app.MapPost("/login-post", async (HttpContext context, SignInManager<User> signInManager) =>
{
    var form = await context.Request.ReadFormAsync();

    var username = form["username"].FirstOrDefault();
    var password = form["password"].FirstOrDefault();

    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
        context.Response.Redirect("/login?error=true");
        return;
    }

    var userManager = signInManager.UserManager;
    var user = await userManager.FindByNameAsync(username);

    if (user == null)
    {
        context.Response.Redirect("/login?error=nouser");
        return;
    }

    var passwordValid = await userManager.CheckPasswordAsync(user, password);

    if (!passwordValid)
    {
        context.Response.Redirect("/login?error=badpassword");
        return;
    }

    var result = await signInManager.PasswordSignInAsync(username, password, false, false);

    if (result.Succeeded)
    {
        context.Response.Redirect("/");
    }
    else
    {
        context.Response.Redirect("/login?error=true");
    }
});
app.MapPost("/logout", async (HttpContext context, SignInManager<User> signInManager) =>
{
    await signInManager.SignOutAsync();
    context.Response.Redirect("/login");
});

app.Run();
