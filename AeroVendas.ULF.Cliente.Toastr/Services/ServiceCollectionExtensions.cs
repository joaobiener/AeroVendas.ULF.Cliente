using Microsoft.Extensions.DependencyInjection;

namespace AeroVendas.ULF.Cliente.Toastr.Services
{
    public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddBlazorToastr(this IServiceCollection services)
			=> services.AddScoped<ToastrService>();
	}
}
