﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;
using BlazingTheWeb.WebComponents;

namespace BlazingTheWeb.WebAssemblyHost
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			await builder.Build().RunAsync();
		}
	}
}
