using AeroVendas.ULF.Cliente;
using AeroVendas.ULF.Cliente.HttpInterceptor;
using AeroVendas.ULF.Cliente.HttpRepository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Entities.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Authorization;
using AeroVendas.ULF.Cliente.AuthProviders;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));


builder.Services.AddHttpClient("AeroVendasPI", (sp, cl) =>
{
	var apiConfiguration = sp.GetRequiredService<IOptions<ApiConfiguration>>();
	cl.BaseAddress = new Uri(apiConfiguration.Value.BaseAddress); //Development
	cl.EnableIntercept(sp);
});

builder.Services.AddScoped(
	sp => sp.GetService<IHttpClientFactory>().CreateClient("AeroVendasPI"));

builder.Services.AddHttpClientInterceptor();

builder.Services.AddScoped<IViewAeroVendasHttpRepository, ViewAeroVendasHttpRepository>();

builder.Services.AddScoped<HttpInterceptorService>();

builder.Services.Configure<ApiConfiguration>
		(builder.Configuration.GetSection("ApiConfiguration"));

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<RefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IViewAeroVendasHttpRepository, ViewAeroVendasHttpRepository>();
await builder.Build().RunAsync();