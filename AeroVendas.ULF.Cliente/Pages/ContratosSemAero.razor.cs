using AeroVendas.ULF.Cliente.HttpInterceptor;
using AeroVendas.ULF.Cliente.HttpRepository;
using Entities.Models;
using Shared.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace AeroVendas.ULF.Cliente.Pages
{
	public partial class ContratosSemAero : IDisposable
	{
		public List<ViewContratoSemAeroVendas> ViewContratoSemAeroVendas { get; set; } = new List<ViewContratoSemAeroVendas>();

		public List<string> CidadesList { get; set; } = new List<string>();

		public MetaData MetaData { get; set; } = new MetaData();

		private ViewAeroVendasParameters _viewContratoSemAeroVendasParameters = new ViewAeroVendasParameters();

		Dictionary<string, string> _cidadesParam = new Dictionary<string, string>();


        private string _selectNomeNormativo = "";
		[Inject]
		public IViewAeroVendasHttpRepository? ViewAeroVendasHttpRepo { get; set; }



        [Inject]
		public HttpInterceptorService? Interceptor { get; set; }

		protected async override Task OnInitializedAsync()
		{
			Interceptor.RegisterEvent();
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
            var pagingResponse = await ViewAeroVendasHttpRepo.GetCidades(_viewContratoSemAeroVendasParameters);

			CidadesList = pagingResponse.Items;
            MetaData = pagingResponse.MetaData;
        }
        private async Task GetContratosAeroVendas()
		{
			var pagingResponse = await ViewAeroVendasHttpRepo.GetContratosSemAeroVendas(_viewContratoSemAeroVendasParameters, _cidadesParam);

			ViewContratoSemAeroVendas = pagingResponse.Items;
			MetaData = pagingResponse.MetaData;
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

		private async Task SelectChanged(string searchTerm)
		{
			_viewContratoSemAeroVendasParameters.PageNumber = 1;
			_cidadesParam.Clear();
			_cidadesParam.Add("NomeNormativo",searchTerm);

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
