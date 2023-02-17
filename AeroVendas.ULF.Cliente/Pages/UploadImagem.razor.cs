using Blazored.Toast.Services;
using AeroVendas.ULF.Cliente.HttpInterceptor;
using AeroVendas.ULF.Cliente.HttpRepository;
using Entities.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AeroVendas.ULF.Cliente.Pages
{
    public partial class UploadImagem : IDisposable
	{
		private Arquivo _arquivo = new Arquivo();
		private string arquivoDtFile;
		private EditContext? _editContext;
		private bool formInvalid = false;
        [Inject]
        public IArquivoHttpRepository? ArquivoRepo { get; set; }
        [Inject]
		public HttpInterceptorService? Interceptor { get; set; }

		[Inject]
		public IToastService? ToastService { get; set; }

        protected override void OnInitialized()
        {
            _editContext = new EditContext(_arquivo);
            _editContext.OnFieldChanged += HandleFieldChanged;
            Interceptor.RegisterEvent();
        }

        private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
        {
            formInvalid = !_editContext.Validate();
            StateHasChanged();
            
        }

        private void AssignImageUrl(string arquivoSrc)
            => arquivoDtFile = arquivoSrc;
        //     private void AssignImage(FileUploadModel fileUpload)
        //=> _fileUpload = fileUpload;

        public void Dispose()
		{
			Interceptor.DisposeEvent();
	
		}

	}
}
