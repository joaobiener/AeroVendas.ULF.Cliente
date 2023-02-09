using AeroVendas.ULF.Cliente.Shared;
using Entities.Models;
using Microsoft.AspNetCore.Components;

namespace AeroVendas.ULF.Cliente.Components
{
    public partial class MensagensHtmlTable
	{
		[Parameter]
		public List<MensagemHtml>? mensagensHTML { get; set; }

		[Parameter]
		public EventCallback<Guid> OnDelete { get; set; }

		[Parameter]
		public bool Aguardando { get; set; }

		private Confirmation? _confirmation;
		private Guid _mensagemIdToDelete;

		private void CallConfirmationModal(Guid id)
		{
			_mensagemIdToDelete = id;
			_confirmation.Show();
		}

		private async Task DeleteMensagem()
		{
			_confirmation.Hide();
			await OnDelete.InvokeAsync(_mensagemIdToDelete);
		}
	}
}
