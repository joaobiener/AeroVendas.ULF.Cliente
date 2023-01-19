using AeroVendas.ULF.Cliente.Features;
using Entities.Models;
using Shared.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using System.Text.Json;
using AeroVendas.ULF.Cliente.HttpRepository;

namespace AeroVendas.ULF.Cliente.HttpRepository
{
    public class ViewAeroVendasHttpRepository : IViewAeroVendasHttpRepository
	{
		private readonly HttpClient _client;
		private readonly NavigationManager _navManager;
		private readonly JsonSerializerOptions _options =
			new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

		public ViewAeroVendasHttpRepository(HttpClient client, NavigationManager navManager,IConfiguration configuration)
		{
			_client = client;
			_navManager = navManager;

		}

        public async Task<PagingResponse<ViewContratoSemAeroVendas>> GetContratosSemAeroVendas(ViewAeroVendasParameters viewAeroVendasParameters,Dictionary<string,string> cidades)
        {
			var queryStringParam = new Dictionary<string, string>
			{
				["pageNumber"] = viewAeroVendasParameters.PageNumber.ToString(),
				["pageSize"] = viewAeroVendasParameters.PageSize.ToString(),
				["searchTerm"] = viewAeroVendasParameters.SearchTerm == null ? string.Empty : viewAeroVendasParameters.SearchTerm,
				["orderBy"] = viewAeroVendasParameters.OrderBy == null ? "" : viewAeroVendasParameters.OrderBy
			};

			
			var queryStringParamMerged = queryStringParam
							 .Concat(cidades)
							 .GroupBy(i => i.Key)
                 .ToDictionary(
					 group => group.Key, 
                     group => group.First().Value);



			var response =
				await _client.GetAsync(QueryHelpers.AddQueryString("ViewContratoSemAero/GetByRequest", queryStringParamMerged));

			var content = await response.Content.ReadAsStringAsync();

			var pagingResponse = new PagingResponse<ViewContratoSemAeroVendas>
			{
				Items = JsonSerializer.Deserialize<List<ViewContratoSemAeroVendas>>(content, _options),
				MetaData = JsonSerializer.Deserialize<MetaData>(
					response.Headers.GetValues("X-Pagination").First(), _options)
			};

			return pagingResponse;
		}

        public async Task<PagingResponse<string>> GetCidades(ViewAeroVendasParameters viewAeroVendasParameters)
		{
			var queryStringParam = new Dictionary<string, string>
			{
				["pageNumber"] = viewAeroVendasParameters.PageNumber.ToString(),
				["pageSize"] = viewAeroVendasParameters.PageSize.ToString(),
				["searchTerm"] = viewAeroVendasParameters.SearchTerm == null ? string.Empty : viewAeroVendasParameters.SearchTerm,
				["orderBy"] = viewAeroVendasParameters.OrderBy == null ? "" : viewAeroVendasParameters.OrderBy
			};

			var response =
                await _client.GetAsync(QueryHelpers.AddQueryString("ViewContratoSemAero/GetCidadeSemAero", queryStringParam));

            var content = await response.Content.ReadAsStringAsync();

            var pagingResponse = new PagingResponse<string>
            {
                Items = JsonSerializer.Deserialize<List<string>>(content, _options),
                MetaData = JsonSerializer.Deserialize<MetaData>(
                    response.Headers.GetValues("X-Pagination").First(), _options)
            };

            return pagingResponse;
        }
    }
}
