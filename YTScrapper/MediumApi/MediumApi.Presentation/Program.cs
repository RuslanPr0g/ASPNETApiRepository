using MediumApi.Application.Contract;
using MediumApi.Application.Service;
using MediumApi.Domain.Global;
using MediumApi.Infrastructure.WebsiteGetter;
using MediumApi.Infrastructure.WebsiteRepository;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMediumPostService, MediumPostService>();
builder.Services.AddScoped<IMediumWebsiteCaller, MediumWebsiteCaller>();
builder.Services.AddScoped<IMediumWebsiteRepository, MediumWebsiteRepository>();

builder.Services.AddHttpClient(MediumConstants.Name, client =>
{
    client.BaseAddress = new Uri("https://api.rss2json.com/v1/api.json");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "MediumApplication");
}).AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder.WaitAndRetryAsync(
            3, retryNumber => TimeSpan.FromMilliseconds(600)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
