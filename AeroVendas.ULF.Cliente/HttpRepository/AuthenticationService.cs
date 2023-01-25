using Shared.DataTransferObjects;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;


namespace AeroVendas.ULF.Cliente.HttpRepository
{
    public class AuthenticationService : IAuthenticationService
	{
		private readonly HttpClient _client;
		private readonly JsonSerializerOptions _options =
			new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		private readonly AuthenticationStateProvider _authStateProvider;

		public AuthenticationService(HttpClient client,
			AuthenticationStateProvider authStateProvider)
		{
			_client = client;
			_authStateProvider = authStateProvider;
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
	}
}
