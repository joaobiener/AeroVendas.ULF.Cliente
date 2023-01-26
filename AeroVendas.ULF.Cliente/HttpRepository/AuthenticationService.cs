using Shared.DataTransferObjects;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using Blazored.LocalStorage;
using AeroVendas.ULF.Cliente.AuthProviders;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace AeroVendas.ULF.Cliente.HttpRepository
{
    public class AuthenticationService : IAuthenticationService
	{
		private readonly HttpClient _client;
		private readonly JsonSerializerOptions _options =
			new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		private readonly AuthenticationStateProvider _authStateProvider;

		private readonly ILocalStorageService _localStorage;

		public AuthenticationService(HttpClient client,
			AuthenticationStateProvider authStateProvider,
			ILocalStorageService localStorage)
		{
			_client = client;
			_authStateProvider = authStateProvider;
			_localStorage = localStorage;
		}

		public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication)
		{
			var response = await _client.PostAsJsonAsync("account/loginLDAP",
				userForAuthentication);
			var content = await response.Content.ReadAsStringAsync();

			var result = JsonSerializer.Deserialize<AuthResponseDto>(content, _options);

			if (!response.IsSuccessStatusCode)
				return result;

			await _localStorage.SetItemAsync("authToken", result.Token);
			await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

			((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(
				result.Token);

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
				"bearer", result.Token);

			return new AuthResponseDto { IsAuthSuccessful = true };
		}

		public async Task Logout()
		{
			await _localStorage.RemoveItemAsync("authToken");
			await _localStorage.RemoveItemAsync("refreshToken");

			((AuthStateProvider)_authStateProvider).NotifyUserLogout();

			_client.DefaultRequestHeaders.Authorization = null;
		}


		public async Task<ResponseDto> RegisterUserOld(UserForRegistrationDto userForRegistrationDto)
		{
			var response = await _client.PostAsJsonAsync("authentication",
				userForRegistrationDto);

			if (!response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();

				var result = JsonSerializer.Deserialize<IActionResult>(content, _options);

				return new ResponseDto { IsSuccessfulRegistration = true };
			}

			return new ResponseDto { IsSuccessfulRegistration = true };
		}

		public async Task<ResponseDto> RegisterUser(UserForRegistrationDto userForRegistrationDto)
		{
			var response = await _client.PostAsJsonAsync("account/register",
				userForRegistrationDto);

			if (!response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();

				var result = JsonSerializer.Deserialize<ResponseDto>(content, _options);

				return result;
			}

			return new ResponseDto { IsSuccessfulRegistration = true };
		}


		public async Task<string> RefreshToken()
		{
			var token = await _localStorage.GetItemAsync<string>("authToken");
			var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

			var response = await _client.PostAsJsonAsync("token/refresh",
				new TokenDto(token, refreshToken));

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<AuthResponseDto>(content, _options);

			await _localStorage.SetItemAsync("authToken", result.Token);
			await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
				("bearer", result.Token);

			return result.Token;
		}
	}
}
