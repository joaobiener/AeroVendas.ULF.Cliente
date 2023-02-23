﻿using Microsoft.AspNetCore.Components;

namespace AeroVendas.ULF.Cliente.Components
{
    public partial class SortFiles
	{
		[Parameter]
		public EventCallback<string> OnSortChanged { get; set; }

		private async Task ApplySort(ChangeEventArgs eventArgs)
		{
			if (eventArgs.Value.ToString() == "-1")
				return;

			await OnSortChanged.InvokeAsync(eventArgs.Value.ToString());
		}
	}
}
