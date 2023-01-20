using Entities.Models;
using Microsoft.AspNetCore.Components;


namespace AeroVendas.ULF.Cliente.Components
{

    public partial class AeroVendasTable
	{
		[Parameter]
		public List<ViewContratoSemAeroVendas> viewAeroVendas { get; set; }

		[Parameter]
		public bool Aguardando { get; set; }

	}
}
