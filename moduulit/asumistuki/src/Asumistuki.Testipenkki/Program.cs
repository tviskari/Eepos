using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Asumistuki.Contracts;
using Asumistuki.Services;
using Asumistuki.Testipenkki.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddLocalization();

builder.Services.AddScoped<IAsumismenotLaskenta, AsumismenotLaskenta>();
builder.Services.AddScoped<IPerusomavastuuLaskenta, PerusomavastuuLaskenta>();
builder.Services.AddScoped<IOmaisuustuloLaskenta, OmaisuustuloLaskenta>();
builder.Services.AddScoped<IKuntaryhmaService, KuntaryhmaService>();
builder.Services.AddScoped<IAsumistukiLaskuri, AsumistukiLaskuri>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

var supportedCultures = new[] { new CultureInfo("fi"), new CultureInfo("sv") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("fi"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
