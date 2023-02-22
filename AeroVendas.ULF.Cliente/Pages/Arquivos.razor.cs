using AeroVendas.ULF.Cliente.HttpInterceptor;
using AeroVendas.ULF.Cliente.HttpRepository;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Blazored.Toast.Services;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;

namespace AeroVendas.ULF.Cliente.Pages
{
    public partial class Arquivos : IDisposable
	{

		private Arquivo _arquivo = new Arquivo();
		private string arquivoDtFile;
		private EditContext? _editContext;

		public bool Aguardando = false;
		public List<Arquivo> ArquivoList { get; set; } = new List<Arquivo>();
		public MetaData MetaData { get; set; } = new MetaData();

		private ViewAeroVendasParameters _mensagemParameters = new ViewAeroVendasParameters();


		[Inject]
		public IJSRuntime? JSRuntime { get; set; }

		private IJSObjectReference? _jsModule;
		[Inject]
		public IToastService? ToastService { get; set; }
		[Inject]
		public IArquivoHttpRepository ArquivoRepo { get; set; }

		[Inject]
		public HttpInterceptorService Interceptor { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                "/scripts/jsAero.js");

            }
        }



		private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
		{

			StateHasChanged();

		}

		protected async override Task OnInitializedAsync()
		{
			_editContext = new EditContext(_arquivo);
			_editContext.OnFieldChanged += HandleFieldChanged;
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
				if ((arquivo.Tipo==null) || (arquivo.Tipo.Length < 20)){ 
					string path = await ArquivoRepo.DownloadFilePath(arquivo.Id);
					arquivo.Tipo = path;
				}

			}
            Aguardando = false;
		}

		private async Task AssignImageUrl(string arquivoSrc)
		{
			_mensagemParameters.PageNumber = 1;
			arquivoDtFile = arquivoSrc;
			await GetArquivo();
		
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
			ToastService.ShowSuccess($"Imagem foi apagada com sucesso.");
			if (_mensagemParameters.PageNumber > 1 && ArquivoList.Count == 1)
				_mensagemParameters.PageNumber--;

			await GetArquivo();
		}


		private async Task CopyClipbard(Guid id) { 
				await _jsModule.InvokeVoidAsync("CopyClipbardSrcImage", id);
				ToastService.ShowSuccess($"Link da imagem compiado com sucesso.");
		}
		public void Dispose() => Interceptor.DisposeEvent();
	}
}
