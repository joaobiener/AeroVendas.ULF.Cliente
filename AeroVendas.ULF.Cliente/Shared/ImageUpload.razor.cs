using AeroVendas.ULF.Cliente.HttpRepository;
using Entities.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Security.AccessControl;

namespace AeroVendas.ULF.Cliente.Shared
{
    public partial class ImageUpload
	{
		private string _fileUploadMessage = "No file chosen";
		private string _content;
		[Parameter]
		public Arquivo? arquivo { get; set; }

		[Parameter]
		public EventCallback<string> OnChange { get; set; }

		[Inject]
		public IArquivoHttpRepository? ArquivoRepo { get; set; }

		private async Task HandleSelected(InputFileChangeEventArgs e)
		{
			var imageFile = e.File;
			_fileUploadMessage = string.Empty;

			if (imageFile == null)
				return;

			_fileUploadMessage += $"{imageFile.Name}";

			var resizedFile = await imageFile.RequestImageFileAsync("image/png", 300, 500);

			using (var ms = resizedFile.OpenReadStream(resizedFile.Size))
			{
				var content = new MultipartFormDataContent();
				content.Headers.ContentDisposition = 
					new ContentDispositionHeaderValue("form-data");
				content.Add(new StreamContent(ms, Convert.ToInt32(resizedFile.Size)),
					"image", imageFile.Name);

				arquivo = await ArquivoRepo.UploadImagem(content);

				 _content = Convert.ToBase64String(arquivo.DataFiles);
			
				await OnChange.InvokeAsync(_content);
			}
		}
	}
}
