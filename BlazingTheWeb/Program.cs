using Microsoft.AspNetCore.Blazor.Hosting;

namespace BlazingTheWeb
{
	public static class Program
	{
		public static void Main() => 
			Program.CreateHostBuilder().Build().Run();

		public static IWebAssemblyHostBuilder CreateHostBuilder() =>
			 BlazorWebAssemblyHost.CreateDefaultBuilder()
				  .UseBlazorStartup<Startup>();
	}
}
