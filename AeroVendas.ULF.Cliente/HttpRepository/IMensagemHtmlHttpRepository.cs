using AeroVendas.ULF.Cliente.Features;
using Entities.Models;
using Shared.RequestFeatures;


namespace AeroVendas.ULF.Cliente.HttpRepository
{
    public interface IMensagemHtmlHttpRepository
	{



		Task<PagingResponse<MensagemHtml>> GetMensagensHTML(ViewAeroVendasParameters parameters);
		Task<MensagemHtml> GetMensagemHTMLById(Guid id);
		Task CreateMensagem(MensagemHtml mensagem);

		//Task<string> UploadProductImage(MultipartFormDataContent content);
		Task UpdateMensagem(MensagemHtml mensagem);
		Task DeleteMensagem(Guid id);

	}
}
