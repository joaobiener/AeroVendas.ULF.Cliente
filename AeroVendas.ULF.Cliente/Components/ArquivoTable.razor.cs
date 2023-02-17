using AeroVendas.ULF.Cliente.Pages;
using AeroVendas.ULF.Cliente.Shared;
using Blazored.Toast.Services;
using Entities.Models;
using Microsoft.AspNetCore.Components;
using Shared.DataTransferObjects;

namespace AeroVendas.ULF.Cliente.Components
{
    public partial class ArquivoTable
	{
		[Parameter]
		public List<Arquivo>? arquivos{ get; set; }

		[Parameter]
		public EventCallback<Guid> OnDelete { get; set; }

		[Parameter]
		public bool Aguardando { get; set; }

		private Confirmation? _confirmation;
		private Guid _arquivoIdToDelete;


		private void CallConfirmationModal(Guid id)
		{
			_arquivoIdToDelete = id;
			_confirmation.Show();
		}

		private async Task DeleteArquivo()
		{
			_confirmation.Hide();
			await OnDelete.InvokeAsync(_arquivoIdToDelete);
		}
	}
}
