using AeroVendas.ULF.Cliente.Features;
using Entities.Models;
using Shared.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using System.Text.Json;
using AeroVendas.ULF.Cliente.HttpRepository;
using Shared.DataTransferObjects;
using Entities.Configuration;
using Microsoft.Extensions.Options;
using AngleSharp.Io;
using AeroVendas.ULF.Cliente.Pages;

namespace AeroVendas.ULF.Cliente.HttpRepository
{
    public class ArquivoHttpRepository : IArquivoHttpRepository
	{
		private readonly HttpClient _client;
		private readonly NavigationManager _navManager;
		private readonly ApiConfiguration _apiConfiguration;
		private readonly JsonSerializerOptions _options =
			new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

		public ArquivoHttpRepository(HttpClient client, NavigationManager navManager,
			IOptions<ApiConfiguration> configuration)
		{
			_client = client;
			_navManager = navManager;
			_apiConfiguration = configuration.Value;

		}


		public async Task<Arquivo> UploadImagem(MultipartFormDataContent content)
		{
			var postResult = await _client.PostAsync("Upload", content);
			var postContent = await postResult.Content.ReadAsStringAsync();



			Arquivo arquivo = JsonSerializer.Deserialize<Arquivo>(postContent, _options);
				//MetaData = JsonSerializer.Deserialize<MetaData>(
				//	response.Headers.GetValues("X-Pagination").First(), _options)
		
			
			return arquivo;
		}

		/*
		 Busca Todas Imagens da Base de Dados
		 */
		public async Task<PagingResponse<Arquivo>> GetImagensHTML(ViewAeroVendasParameters parameters)
		{
			var queryStringParam = new Dictionary<string, string>
			{
				["pageNumber"] = parameters.PageNumber.ToString(),
				["pageSize"] = parameters.PageSize.ToString(),
				["searchTerm"] = parameters.SearchTerm == null ? string.Empty : parameters.SearchTerm,
				["orderBy"] = parameters.OrderBy == null ? "" : parameters.OrderBy
			};


			var response =
				await _client.GetAsync(QueryHelpers.AddQueryString("Upload", queryStringParam));

			var content = await response.Content.ReadAsStringAsync();

			var pagingResponse = new PagingResponse<Arquivo>
			{
				Items = JsonSerializer.Deserialize<List<Arquivo>>(content, _options),
				MetaData = JsonSerializer.Deserialize<MetaData>(
					response.Headers.GetValues("X-Pagination").First(), _options)
			};

			return pagingResponse;
		}


		public async Task DeleteArquivo(Guid id)
			=> await _client.DeleteAsync(Path.Combine("Upload", id.ToString()));

		public async Task<string> DownloadFilePath(Guid id)
		{
			var path = await _client.GetStringAsync(Path.Combine("Upload", "DownloadFile", id.ToString()));

			return path;

		}

		public async Task<Arquivo> GetArquivoById(Guid id)
		{
			var arquivo = await _client.GetFromJsonAsync<Arquivo>($"Upload/{id}");

			return arquivo;

		}
	}
}
