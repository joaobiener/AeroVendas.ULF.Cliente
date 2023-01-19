using Entities.Models;
using Microsoft.AspNetCore.Components;

namespace AeroVendas.ULF.Cliente.Components;

public partial class Cidades
{

    [Parameter]
    public List<string> CidadesList { get; set; }

	[Parameter]
	public EventCallback<string> OnSelectChanged { get; set; }

	async Task OnSelectAsync(ChangeEventArgs e)
	{
		if (e.Value.ToString() == "-1")
			return;

		await OnSelectChanged.InvokeAsync(e.Value.ToString());
	}


}
