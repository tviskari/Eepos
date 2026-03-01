using Asumistuki.Contracts;
using Asumistuki.Models;
using Asumistuki.Services;
using Eepos.Kunnat;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IKuntaryhmaService, KuntaryhmaService>();
builder.Services.AddTransient<IAsumismenotLaskenta, AsumismenotLaskenta>();
builder.Services.AddTransient<IPerusomavastuuLaskenta, PerusomavastuuLaskenta>();
builder.Services.AddTransient<IOmaisuustuloLaskenta, OmaisuustuloLaskenta>();
builder.Services.AddTransient<IAsumistukiLaskuri, AsumistukiLaskuri>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/asumistuki/laske", (RuokakuntaInput input, IAsumistukiLaskuri laskuri) =>
{
    var tulos = laskuri.Laske(input);
    return Results.Ok(tulos);
})
.WithName("LaskeAsumistuki");

app.Run();
