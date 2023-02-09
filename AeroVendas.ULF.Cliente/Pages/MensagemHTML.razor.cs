using AeroVendas.ULF.Cliente.HttpInterceptor;
using AeroVendas.ULF.Cliente.HttpRepository;
using Entities.Models;
using Shared.RequestFeatures;
using Microsoft.AspNetCore.Components;

namespace AeroVendas.ULF.Cliente.Pages
{
    public partial class MensagemHTML : IDisposable
	{
		public List<MensagemHtml> MensagemList { get; set; } = new List<MensagemHtml>();
		public MetaData MetaData { get; set; } = new MetaData();

		private ViewAeroVendasParameters _mensagemParameters = new ViewAeroVendasParameters();

		[Inject]
		public IMensagemHtmlHttpRepository MensagemHTMLRepo { get; set; }

		[Inject]
		public HttpInterceptorService Interceptor { get; set; }

		protected async override Task OnInitializedAsync()
		{
			Interceptor.RegisterEvent();
			Interceptor.RegisterBeforeSendEvent();
			await GetMensagensHTML();
		}

		private async Task SelectedPage(int page)
		{
			_mensagemParameters.PageNumber = page;
			await GetMensagensHTML();
		}

		private async Task GetMensagensHTML()
		{
			var pagingResponse = await MensagemHTMLRepo.GetMensagensHTML(_mensagemParameters);

			MensagemList = pagingResponse.Items;
			MetaData = pagingResponse.MetaData;
		}

		private async Task SetPageSize(int pageSize)
		{
			_mensagemParameters.PageSize = pageSize;
			_mensagemParameters.PageNumber = 1;

			await GetMensagensHTML();
		}

		private async Task SearchChanged(string searchTerm)
		{
			_mensagemParameters.PageNumber = 1;
			_mensagemParameters.SearchTerm = searchTerm;

			await GetMensagensHTML();
		}

		private async Task SortChanged(string orderBy)
		{
			_mensagemParameters.OrderBy = orderBy;

			await GetMensagensHTML();
		}

		private async Task DeleteMensagem(Guid id)
		{
			await MensagemHTMLRepo.GetMensagemHTMLById(id);

			if (_mensagemParameters.PageNumber > 1 && MensagemList.Count == 1)
				_mensagemParameters.PageNumber--;

			await GetMensagensHTML();
		}

		public void Dispose() => Interceptor.DisposeEvent();
	}
}
