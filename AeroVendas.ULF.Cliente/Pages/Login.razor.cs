using AeroVendas.ULF.Cliente.HttpRepository;
using Shared.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using AeroVendas.ULF.Cliente.Toastr.Enumerations;
using AeroVendas.ULF.Cliente.Toastr.Services;
using AeroVendas.ULF.Cliente.Toastr;
using Microsoft.JSInterop;

namespace AeroVendas.ULF.Cliente.Pages
{
    public partial class Login
	{
		[Inject]
		public IJSRuntime? JSRuntime { get; set; }

		private IJSObjectReference? _jsModule;

		private UserForAuthenticationDto _userForAuthentication = new UserForAuthenticationDto();
		private bool loading;
		[Inject]
		public IAuthenticationService? AuthenticationService { get; set; }

		[Inject]
		public NavigationManager? NavigationManager { get; set; }

		public bool ShowAuthError { get; set; }
		public string? Error { get; set; }
		[Inject]
		public ToastrService? ToastrService { get; set; }

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
				"/scripts/jsExamples.js");

				await FocusAndStyleElement();
			}
		}

		private async Task FocusAndStyleElement() =>
			await _jsModule.InvokeVoidAsync("focusInputComponent", "username");

		private async Task ShowToastrInfo(string message)
		{
		

			var options = new ToastrOptions
			{
				
				CloseButton = true,
				HideDuration = 300,
				HideMethod = ToastrHideMethod.SlideUp,
				ShowMethod = ToastrShowMethod.SlideDown,
				Position = ToastrPosition.TopCenter
			};
			await ToastrService.ShowInfoMessage(message, options);
		}
		public async Task ExecuteLogin()
		{
			ShowAuthError = false;
			loading = true;
			var result = await AuthenticationService.Login(_userForAuthentication);
			loading = false;
			if (!result.IsAuthSuccessful)
			{
				
				Error = result.ErrorMessage;
				Task task = ShowToastrInfo(Error);
				//ShowAuthError = true;
			}
			else
			{
				NavigationManager.NavigateTo("/");
			}
		}
	}
}
