using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Runtime.InteropServices;

namespace BlazingTheWeb.WebComponents
{
	public sealed class Platform
		: ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "div");
			builder.AddContent(1, this.Value);
			builder.CloseElement();
		}

		private string Value =>
			RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY")) ?
			"WebAssembly" : "Server";
	}
}