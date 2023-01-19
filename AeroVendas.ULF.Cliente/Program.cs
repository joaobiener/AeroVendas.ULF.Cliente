using AeroVendas.ULF.Cliente;
using AeroVendas.ULF.Cliente;
using AeroVendas.ULF.Cliente.HttpInterceptor;
using AeroVendas.ULF.Cliente.HttpRepository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Entities.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Authorization;
using Normativo.Cliente.AuthProviders;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));


builder.Services.AddHttpClient("LogsNormativoAPI", (sp, cl) =>
{
	var apiConfiguration = sp.GetRequiredService<IOptions<ApiConfiguration>>();
	cl.BaseAddress = new Uri(apiConfiguration.Value.BaseAddress); //Development
	cl.EnableIntercept(sp);
});

builder.Services.AddScoped(
	sp => sp.GetService<IHttpClientFactory>().CreateClient("LogsNormativoAPI"));

builder.Services.AddHttpClientInterceptor();

builder.Services.AddScoped<IViewLogNormativoHttpRepository, ViewLogNormativoHttpRepository>();

builder.Services.AddScoped<HttpInterceptorService>();

builder.Services.Configure<ApiConfiguration>
		(builder.Configuration.GetSection("ApiConfiguration"));


builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, TestAuthStateProvider>();
await builder.Build().RunAsync();