using AeroVendas.ULF.Cliente.HttpRepository;
using Shared.DataTransferObjects;
using Microsoft.AspNetCore.Components;

namespace AeroVendas.ULF.Cliente.Pages
{
    public partial class Login
	{
		private UserForAuthenticationDto _userForAuthentication = new UserForAuthenticationDto();

		[Inject]
		public IAuthenticationService? AuthenticationService { get; set; }

		[Inject]
		public NavigationManager? NavigationManager { get; set; }

		public bool ShowAuthError { get; set; }
		public string? Error { get; set; }

		public async Task ExecuteLogin()
		{
			ShowAuthError = false;

			var result = await AuthenticationService.Login(_userForAuthentication);
			if (!result.IsAuthSuccessful)
			{
				Error = result.ErrorMessage;
				ShowAuthError = true;
			}
			else
			{
				NavigationManager.NavigateTo("/");
			}
		}
	}
}
