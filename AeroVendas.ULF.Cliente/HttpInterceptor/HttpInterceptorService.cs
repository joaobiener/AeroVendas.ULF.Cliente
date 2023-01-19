using Microsoft.AspNetCore.Components;
using System.Net;
using Toolbelt.Blazor;

namespace AeroVendas.ULF.Cliente.HttpInterceptor
{
    public class HttpInterceptorService
	{
		private readonly HttpClientInterceptor _interceptor;
		private readonly NavigationManager _navManager;

		public HttpInterceptorService(HttpClientInterceptor interceptor, NavigationManager navManager)
		{
			_interceptor = interceptor;
			_navManager = navManager;
		}

		public void RegisterEvent() => _interceptor.AfterSend += HandleResponse;
		public void DisposeEvent() => _interceptor.AfterSend -= HandleResponse;

		private void HandleResponse(object sender, HttpClientInterceptorEventArgs e)
		{
			if(e.Response is null)
			{
				_navManager.NavigateTo("error");
				throw new HttpResponseException("Servidor não disponível.");
			}

			var message = "";

			if(!e.Response.IsSuccessStatusCode)
			{
				switch (e.Response.StatusCode)
				{
					case HttpStatusCode.NotFound:
						_navManager.NavigateTo("404");
						message = "Recurso não encontrado.";
						break;
					case HttpStatusCode.Unauthorized:
						_navManager.NavigateTo("unauthorized");
						message = "Acesso não autorizado";
						break;
					default:
						_navManager.NavigateTo("error");
						message = "Algo deu errado. Entre em contato com o administrador.";
						break;
				}

				throw new HttpResponseException(message);
			}
		}
	}
}
