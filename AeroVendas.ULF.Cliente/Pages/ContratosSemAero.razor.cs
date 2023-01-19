using Normativo.Cliente.HttpInterceptor;
using Normativo.Cliente.HttpRepository;
using Entities.Models;
using Shared.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace Normativo.Cliente.Pages
{
	public partial class ReportNormativos : IDisposable
	{
		public List<ViewLogsNormativo> ViewLogNormativoList { get; set; } = new List<ViewLogsNormativo>();

		public List<string> ViewNomeNormativoList { get; set; } = new List<string>();

		public MetaData MetaData { get; set; } = new MetaData();

		private ViewLogNormativoParameters _viewLogNormativoParameters = new ViewLogNormativoParameters();

		Dictionary<string, string> _nomeNormativoParam = new Dictionary<string, string>();


        private string _selectNomeNormativo = "";
		[Inject]
		public IViewLogNormativoHttpRepository? ViewLogsNormativoRepo { get; set; }



        [Inject]
		public HttpInterceptorService? Interceptor { get; set; }

		protected async override Task OnInitializedAsync()
		{
			Interceptor.RegisterEvent();
			await GetNomeNormativos();
			await GetLogsNormativo();

		}

		private async Task SelectedPage(int page)
		{
			
			_viewLogNormativoParameters.PageNumber = page;
			await GetLogsNormativo();
		}

		//Busca de todos os normtativos
        private async Task GetNomeNormativos()
        {
            var pagingResponse = await ViewLogsNormativoRepo.GetNomeNormativo(_viewLogNormativoParameters);

			ViewNomeNormativoList = pagingResponse.Items;
            MetaData = pagingResponse.MetaData;
        }
        private async Task GetLogsNormativo()
		{
			var pagingResponse = await ViewLogsNormativoRepo.GetLogsNormativo(_viewLogNormativoParameters, _nomeNormativoParam);

			ViewLogNormativoList = pagingResponse.Items;
			MetaData = pagingResponse.MetaData;
		}

		private async Task SetPageSize(int pageSize)
		{
			_viewLogNormativoParameters.PageSize = pageSize;
			_viewLogNormativoParameters.PageNumber = 1;

			await GetLogsNormativo();
		}

		private async Task SearchChanged(string searchTerm)
		{
			_viewLogNormativoParameters.PageNumber = 1;
			_viewLogNormativoParameters.SearchTerm = searchTerm;

			await GetLogsNormativo();
		}

		private async Task SelectChanged(string searchTerm)
		{
			_viewLogNormativoParameters.PageNumber = 1;
			_nomeNormativoParam.Clear();
			_nomeNormativoParam.Add("NomeNormativo",searchTerm);

			await GetLogsNormativo();
		}
		private async Task SortChanged(string orderBy)
		{
			_viewLogNormativoParameters.OrderBy = orderBy;

			await GetLogsNormativo();
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
