using AeroVendas.ULF.Cliente.HttpRepository;
using Entities.Models;
using Microsoft.AspNetCore.Components;

namespace AeroVendas.ULF.Cliente.Pages
{
    public partial class MensagemHTMLPreview
	{
		public MensagemHtml Mensagem { get; set; } = new MensagemHtml();

		[Inject]
		public IMensagemHtmlHttpRepository? MensagemRepo { get; set; }

		[Parameter]
		public Guid MensagemId { get; set; }

		protected async override Task OnInitializedAsync()
		{
			Mensagem = await MensagemRepo.GetMensagemHTMLById(MensagemId);
		}
	}
}
