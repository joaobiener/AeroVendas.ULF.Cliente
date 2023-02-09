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
    public class MensagemHtmlHttpRepository : IMensagemHtmlHttpRepository
	{
		private readonly HttpClient _client;
		private readonly NavigationManager _navManager;
		private readonly JsonSerializerOptions _options =
			new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

		public MensagemHtmlHttpRepository(HttpClient client, NavigationManager navManager,IConfiguration configuration)
		{
			_client = client;
			_navManager = navManager;

		}

		public async Task<PagingResponse<MensagemHtml>> GetMensagensHTML(ViewAeroVendasParameters parameters)
		{
			var queryStringParam = new Dictionary<string, string>
			{
				["pageNumber"] = parameters.PageNumber.ToString(),
				["pageSize"] = parameters.PageSize.ToString(),
				["searchTerm"] = parameters.SearchTerm == null ? string.Empty : parameters.SearchTerm,
				["orderBy"] = parameters.OrderBy == null ? "" : parameters.OrderBy
			};


			var response =
				await _client.GetAsync(QueryHelpers.AddQueryString("MensagemHtml", queryStringParam));

			var content = await response.Content.ReadAsStringAsync();

			var pagingResponse = new PagingResponse<MensagemHtml>
			{
				Items = JsonSerializer.Deserialize<List<MensagemHtml>>(content, _options),
				MetaData = JsonSerializer.Deserialize<MetaData>(
					response.Headers.GetValues("X-Pagination").First(), _options)
			};

			return pagingResponse;
		}
		public Task CreateMensagem(MensagemHtml mensagem)
		{
			throw new NotImplementedException();
		}

		public Task DeleteMensagem(Guid id)
		{
			throw new NotImplementedException();
		}



		public Task UpdateMensagem(MensagemHtml mensagem)
		{
			throw new NotImplementedException();
		}

        public Task<MensagemHtml> GetMensagemHTMLById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
