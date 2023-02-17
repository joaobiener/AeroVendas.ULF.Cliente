﻿using AeroVendas.ULF.Cliente.HttpInterceptor;
using AeroVendas.ULF.Cliente.HttpRepository;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using Microsoft.AspNetCore.Components;

namespace AeroVendas.ULF.Cliente.Pages
{
    public partial class Arquivos : IDisposable
	{
		public bool Aguardando = false;
		public List<Arquivo> ArquivoList { get; set; } = new List<Arquivo>();
		public MetaData MetaData { get; set; } = new MetaData();

		private ViewAeroVendasParameters _mensagemParameters = new ViewAeroVendasParameters();

		[Inject]
		public IArquivoHttpRepository ArquivoRepo { get; set; }

		[Inject]
		public HttpInterceptorService Interceptor { get; set; }

		protected async override Task OnInitializedAsync()
		{
			Interceptor.RegisterEvent();
			Interceptor.RegisterBeforeSendEvent();
			await GetArquivo();
		}

		private async Task SelectedPage(int page)
		{
			_mensagemParameters.PageNumber = page;
			await GetArquivo();
		}

		private async Task GetArquivo()
		{
			Aguardando = true;
			var pagingResponse = await ArquivoRepo.GetImagensHTML(_mensagemParameters);

			ArquivoList = pagingResponse.Items;
			MetaData = pagingResponse.MetaData;

            foreach (Arquivo arquivo in ArquivoList)
            {
                arquivo.Tipo = string.Concat("data:image / gif; base64,", Convert.ToBase64String(arquivo.DataFiles));
            }
            Aguardando = false;
		}

		private async Task SetPageSize(int pageSize)
		{
			_mensagemParameters.PageSize = pageSize;
			_mensagemParameters.PageNumber = 1;

			await GetArquivo();
		}

		private async Task SearchChanged(string searchTerm)
		{
			_mensagemParameters.PageNumber = 1;
			_mensagemParameters.SearchTerm = searchTerm;

			await GetArquivo();
		}

		private async Task SortChanged(string orderBy)
		{
			_mensagemParameters.OrderBy = orderBy;

			await GetArquivo();
		}

		private async Task DeleteArquivo(Guid id)
		{
			await ArquivoRepo.DeleteArquivo(id);

			if (_mensagemParameters.PageNumber > 1 && ArquivoList.Count == 1)
				_mensagemParameters.PageNumber--;

			await GetArquivo();
		}
		
		public void Dispose() => Interceptor.DisposeEvent();
	}
}
