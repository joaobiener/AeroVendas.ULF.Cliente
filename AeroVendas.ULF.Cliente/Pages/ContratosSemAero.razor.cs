﻿using AeroVendas.ULF.Cliente.HttpInterceptor;
using AeroVendas.ULF.Cliente.HttpRepository;
using Entities.Models;
using Shared.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Blazored.Toast.Services;

namespace AeroVendas.ULF.Cliente.Pages
{
	public partial class ContratosSemAero : IDisposable
	{
		public bool Aguardando = false;
		public List<ViewContratoSemAeroVendas> ListContratoSemAeroVendas { get; set; } = new List<ViewContratoSemAeroVendas>();

		public List<string> CidadesList { get; set; } = new List<string>();

		public MetaData MetaData { get; set; } = new MetaData();

		private ViewAeroVendasParameters _viewContratoSemAeroVendasParameters = new ViewAeroVendasParameters();

		Dictionary<string, string> _cidadesParam = new Dictionary<string, string>();


		[Inject]
		public IViewAeroVendasHttpRepository? ViewAeroVendasHttpRepo { get; set; }

	
		[Inject]
		public HttpInterceptorService? Interceptor { get; set; }


		protected async override Task OnInitializedAsync()
		{
			Interceptor.RegisterEvent();
			Interceptor.RegisterBeforeSendEvent();
			await GetCidades();
			await GetContratosAeroVendas();

		}

		private async Task SelectedPage(int page)
		{

			_viewContratoSemAeroVendasParameters.PageNumber = page;
			await GetContratosAeroVendas();
		}

		//Busca de todos os normtativos
        private async Task GetCidades()
        {
			//Não irei fazer chamada na API pela demora. Irei inicializar a lista de Cidades 
			CidadesList.Add("");
			CidadesList.Add("ITABORAÍ");
            CidadesList.Add("MARICÁ");
            CidadesList.Add("NITERÓI");
            CidadesList.Add("RIO BONITO");
            CidadesList.Add("SÃO GONÇALO");
            CidadesList.Add("SILVA JARDIM");
            CidadesList.Add("TANGUÁ");
            CidadesList.Add("OUTROS");
            //         var pagingResponse = await ViewAeroVendasHttpRepo.GetCidades(_viewContratoSemAeroVendasParameters);

            //CidadesList = pagingResponse.Items;
            //         MetaData = pagingResponse.MetaData;
        }
        private async Task GetContratosAeroVendas()
		{
			Aguardando = true;
			ListContratoSemAeroVendas.Clear();
			var pagingResponse = await ViewAeroVendasHttpRepo.GetContratosSemAeroVendas(_viewContratoSemAeroVendasParameters, _cidadesParam);

            ListContratoSemAeroVendas = pagingResponse.Items;
			MetaData = pagingResponse.MetaData;
			Aguardando = false;
		}

		private async Task SetPageSize(int pageSize)
		{
			_viewContratoSemAeroVendasParameters.PageSize = pageSize;
			_viewContratoSemAeroVendasParameters.PageNumber = 1;

			await GetContratosAeroVendas();
		}

		private async Task SearchChanged(string searchTerm)
		{
			_viewContratoSemAeroVendasParameters.PageNumber = 1;
			_viewContratoSemAeroVendasParameters.SearchTerm = searchTerm;

			await GetContratosAeroVendas();
		}

		private async Task SelectCidadeChanged(string searchTerm)
		{
			
			_viewContratoSemAeroVendasParameters.PageNumber = 1;
			_cidadesParam.Clear();
			_cidadesParam.Add("Cidade", searchTerm);

			await GetContratosAeroVendas();
		}
		private async Task SortChanged(string orderBy)
		{
			_viewContratoSemAeroVendasParameters.OrderBy = orderBy;

			await GetContratosAeroVendas();
		}
        [Inject]
        public IJSRuntime IJSRuntime { get; set; }
        public async Task Print()
        {

            await IJSRuntime.InvokeVoidAsync("Print", "Inprime Relatório");
        }

		public void Dispose() => Interceptor.DisposeEvent();
	}
}
