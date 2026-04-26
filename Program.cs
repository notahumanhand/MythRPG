using Microsoft.EntityFrameworkCore;
using MythRPG.Components;
using MythRPG.Core;
using MythRPG.Data;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddDbContextFactory<MythRPGContext>(options =>
{
//builder.Configuration.GetConnectionString("MythRPG")
options.UseSqlServer("Server=tcp:mythrpg-server.database.windows.net,1433;Initial Catalog=MythRPGDb;Persist Security Info=False;User ID=spleen;Password=GMilmc176;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount:2);
            sqlOptions.CommandTimeout(120);
        });
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTransient<ICharactersRepository, CharactersRepository>();
builder.Services.AddTransient<ITraitsRepository, TraitsRepository>();
builder.Services.AddTransient<ISpellsRepository, SpellsRepository>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<AppData>();

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
