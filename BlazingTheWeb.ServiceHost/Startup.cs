using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazingTheWeb.ServiceHost
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddGrpc();

			services.AddCors(o => o.AddPolicy("AllowAll", builder =>
			{
				builder.AllowAnyOrigin()
						 .AllowAnyMethod()
						 .AllowAnyHeader()
						 .WithExposedHeaders("Grpc-Status", "Grpc-Message");
			}));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseGrpcWeb();
			app.UseCors();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGrpcService<CollatzService>().EnableGrpcWeb().RequireCors("AllowAll");

				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync(
						"Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
				});
			});
		}
	}
}