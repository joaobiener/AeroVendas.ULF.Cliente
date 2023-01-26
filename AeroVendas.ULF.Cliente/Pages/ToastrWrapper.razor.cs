﻿using AeroVendas.ULF.Cliente.Toastr;
using AeroVendas.ULF.Cliente.Toastr.Enumerations;
using AeroVendas.ULF.Cliente.Toastr.Services;
using Microsoft.AspNetCore.Components;

namespace AeroVendas.ULF.Cliente.Pages
{
    public partial class ToastrWrapper
	{
		[Inject]
		public ToastrService? ToastrService { get; set; }

		private async Task ShowToastrInfo()
		{
			var message = "This is a message sent from the C# method.";

			var options = new ToastrOptions
			{
				CloseButton = true,
				HideDuration = 300,
				HideMethod = ToastrHideMethod.SlideUp,
				ShowMethod = ToastrShowMethod.SlideDown,
				Position = ToastrPosition.TopRight
			};
			await ToastrService.ShowInfoMessage(message, options);
		}
	}
}
