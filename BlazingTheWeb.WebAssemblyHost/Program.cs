using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;
using BlazingTheWeb.WebComponents;
using BlazingTheWeb.WebComponents.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingTheWeb.WebAssemblyHost
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");
			builder.Services.AddWebComponents();

			await builder.Build().RunAsync();
		}
	}
}
