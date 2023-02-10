﻿using Blazored.Toast.Services;
using AeroVendas.ULF.Cliente.HttpRepository;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
using Toolbelt.Blazor;

namespace AeroVendas.ULF.Cliente.HttpInterceptor
{
	public class HttpInterceptorService
	{
		private readonly HttpClientInterceptor _interceptor;
		private readonly NavigationManager _navManager;
		private readonly IToastService _toastService;
		private readonly RefreshTokenService _refreshTokenService;

		public HttpInterceptorService(HttpClientInterceptor interceptor,
			NavigationManager navManager, IToastService toastService,
			RefreshTokenService refreshTokenService)
		{
			_interceptor = interceptor;
			_navManager = navManager;
			_toastService = toastService;
			_refreshTokenService = refreshTokenService;
		}

		public void RegisterEvent() => _interceptor.AfterSend += HandleResponse;
		public void RegisterBeforeSendEvent() =>
			_interceptor.BeforeSendAsync += InterceptBeforeSendAsync;

		public void DisposeEvent()
		{
			_interceptor.AfterSend -= HandleResponse;
			_interceptor.BeforeSendAsync -= InterceptBeforeSendAsync;
		}

		private async Task InterceptBeforeSendAsync(object sender,
			HttpClientInterceptorEventArgs e)
		{
			var absolutePath = e.Request.RequestUri.AbsolutePath;

			if (!absolutePath.Contains("token") && !absolutePath.Contains("account"))
			{
				var token = "";

                try { 
					token = await _refreshTokenService.TryRefreshToken();
                }
				catch (Exception ex)  
				{
                    
                    _navManager.NavigateTo("/logout");

                }

                if (!string.IsNullOrEmpty(token))
				{
					e.Request.Headers.Authorization =
						new AuthenticationHeaderValue("bearer", token);
				}
			}
		}

		private void HandleResponse(object? sender, HttpClientInterceptorEventArgs e)
		{
			if (e.Response is null)
			{
				_navManager.NavigateTo("/error");
				throw new HttpResponseException("Server not available.");
			}

			var message = "";

			if (!e.Response.IsSuccessStatusCode)
			{
				switch (e.Response.StatusCode)
				{
					case HttpStatusCode.NotFound:
						_navManager.NavigateTo("/404");
						message = "Resource not found.";
						break;
					case HttpStatusCode.BadRequest:
						message = "Invalid request. Please try again.";
						_toastService.ShowError(message);
						break;
					case HttpStatusCode.Unauthorized:
						_navManager.NavigateTo("/unauthorized");
						message = "Unauthorized access";
						break;
					default:
						_navManager.NavigateTo("/error");
						message = "Something went wrong. Please contact the administrator.";
						break;
				}

				throw new HttpResponseException(message);
			}
		}
	}
}
