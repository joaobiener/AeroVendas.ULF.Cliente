using AeroVendas.ULF.Cliente.Features;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;


namespace AeroVendas.ULF.Cliente.HttpRepository
{
    public interface IArquivoHttpRepository
	{



		Task<PagingResponse<Arquivo>> GetImagensHTML(ViewAeroVendasParameters parameters);

		Task<Arquivo> UploadImagem(MultipartFormDataContent content);
		Task DeleteArquivo(Guid id);

		Task<string> DownloadFilePath(Guid id);

		Task<Arquivo> GetArquivoById(Guid id);
	}
}
