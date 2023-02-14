using AeroVendas.ULF.Cliente.Toastr.Services;
using AeroVendas.ULF.Cliente.HttpInterceptor;
using AeroVendas.ULF.Cliente.HttpRepository;
using Entities.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace AeroVendas.ULF.Cliente.Pages
{
    public partial class UpdateMensagemHTML : IDisposable
	{
		private MensagemHtml _mensagem = new MensagemHtml();
		private EditContext? _editContext;
		private bool formInvalid = false;

		bool disable = false;
		[Parameter]
		public Guid MensagemHtmlId { get; set; }

		private Dictionary<string, object> editorConf = new Dictionary<string, object>{
			{"toolbar", "save | insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image"
			},
			{"plugins",  "['advlist autolink lists link image charmap print preview anchor,save,autosave, " +
			"'searchreplace visualblocks code fullscreen'," +
			"'insertdatetime media table paste code help wordcount']"
			},
			{"content_style",  "body { font-family:Helvetica,Arial,sans-serif; font-size:14px" },
			{"height" , "1000px" }
            //{"skin", "oxide-dark" }
            ////{"content_css", "dark" }

            };
		[Inject]
		private AuthenticationStateProvider _authStateProvider { get; set; }

		[Inject]
		public IMensagemHtmlHttpRepository? MensagemHtmlRepo { get; set; }
		[Inject]
		public NavigationManager? NavigationManager { get; set; }
		[Inject]
		public HttpInterceptorService? Interceptor { get; set; }

		[Inject]
		public IToastService? ToastService { get; set; }

		[Parameter]
		public Guid Id { get; set; }

		private string GetFirstDetalhes(string identName)
		{
			
			char[] chavetas = { '[', ']', '"' };
			string identSemChaves = identName.Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty);


			string[] arrIdent = identSemChaves.Split(",");


			return arrIdent[0];
		}
		protected async override Task OnInitializedAsync()
		{
			_mensagem = await MensagemHtmlRepo.GetMensagemHTMLById(Id);
			
			_editContext = new EditContext(_mensagem);
			
			_editContext.OnFieldChanged += HandleFieldChanged;
			Interceptor.RegisterEvent();
			var authState = await _authStateProvider.GetAuthenticationStateAsync();
			var user = authState.User;
			_mensagem.ModificadoPor  = GetFirstDetalhes(user.Identity.Name);


		}

		private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
		{
			formInvalid = !_editContext.Validate();
			StateHasChanged();
		}

		private async Task Update()
		{

			await MensagemHtmlRepo.UpdateMensagem(_mensagem);
			
			ToastService.ShowSuccess($"Mensagem \"{_mensagem.Titulo}\" atualizada com sucesso.");

			NavigationManager.NavigateTo("/MensagemHTML");
		}


		public void Dispose()
		{
			Interceptor.DisposeEvent();
			_editContext.OnFieldChanged -= HandleFieldChanged;
		}
	}
}
