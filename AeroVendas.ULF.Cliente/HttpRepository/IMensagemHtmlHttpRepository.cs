using AeroVendas.ULF.Cliente.Features;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;


namespace AeroVendas.ULF.Cliente.HttpRepository
{
    public interface IMensagemHtmlHttpRepository
	{



		Task<PagingResponse<MensagemHtml>> GetMensagensHTML(ViewAeroVendasParameters parameters);
		Task<MensagemHtml> GetMensagemHTMLById(Guid id);
		Task CreateMensagem(MensagemHtmlForCreationDto mensagem);
		Task<string> UploadMensagensImage(MultipartFormDataContent content);
		Task UpdateMensagem(MensagemHtml mensagem);
		Task DeleteMensagem(Guid id);

	}
}
