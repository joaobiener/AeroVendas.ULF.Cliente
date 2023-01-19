using AeroVendas.ULF.Cliente.Features;
using Entities.Models;
using Shared.RequestFeatures;


namespace AeroVendas.ULF.Cliente.HttpRepository
{
    public interface IViewAeroVendasHttpRepository
	{
		Task<PagingResponse<ViewContratoSemAeroVendas>> GetContratosSemAeroVendas(ViewAeroVendasParameters viewAeroVendasParameters,Dictionary<string,string> cidades);
        Task<PagingResponse<string>> GetCidades(ViewAeroVendasParameters viewAeroVendasParameters);

	}
}
