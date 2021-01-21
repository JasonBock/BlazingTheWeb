using BlazingTheWeb.WebAssembly;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

CreateWebHostBuilder(args).Build().Run();

static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
	WebHost.CreateDefaultBuilder(args)
		.UseStartup<Startup>();