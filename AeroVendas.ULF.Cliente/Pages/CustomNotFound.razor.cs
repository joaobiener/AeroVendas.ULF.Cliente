using Microsoft.AspNetCore.Components;

namespace Normativo.Cliente.Pages
{
    public partial class CustomNotFound
	{
		[Inject]
		public NavigationManager navigationManager { get; set; }

		public void NavigateToHome()
		{
			navigationManager.NavigateTo("index");
		}
	}
}
