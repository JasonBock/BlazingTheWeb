using Microsoft.Extensions.DependencyInjection;

namespace BlazingTheWeb.WebComponents.Extensions
{
	public static class IServiceCollectionExtensions 
	{
		public static void AddWebComponents(this IServiceCollection @this) =>
			@this.AddTransient<SequenceViewModel>();
	}
}
