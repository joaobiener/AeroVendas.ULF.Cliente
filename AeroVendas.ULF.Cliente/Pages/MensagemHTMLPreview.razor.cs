using AeroVendas.ULF.Cliente.HttpRepository;
using Entities.Models;
using Ganss.Xss;
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

		public struct MarkupStringSanitized
		{
			public MarkupStringSanitized(string value)
			{
				Value = Sanitize(value);
			}

			public string Value { get; }

			public static explicit operator MarkupStringSanitized(string value) => new MarkupStringSanitized(value);

			public static explicit operator MarkupString(MarkupStringSanitized value) => new MarkupString(value.Value);

			public override string ToString() => Value ?? string.Empty;

			private static string Sanitize(string value)
			{
				var sanitizer = new HtmlSanitizer();
				return sanitizer.Sanitize(value);
			}
		}
	}
}
