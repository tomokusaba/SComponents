using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.FluentUI.AspNetCore.Components;
using SComponents.Components;
using SmartComponents.Inference.OpenAI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSmartComponents()
    .WithInferenceBackend<OpenAIInferenceBackend>();
builder.Services.AddHttpClient();
builder.Services.AddFluentUIComponents();
//builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
//{
//    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"],


//});
var otel = builder.Services.AddOpenTelemetry();
otel.UseAzureMonitor();
otel.WithMetrics(metrics => metrics
.AddMeter("Microsoft.AspNetCore.Hosting")
.AddMeter("Microsoft.AspNetCore.Server.Kestrel"));

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
