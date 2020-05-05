using Microsoft.AspNetCore.Components.Testing;
using NUnit.Framework;
using System.Collections.Generic;

namespace BlazingTheWeb.WebComponents.Tests
{
	public static class SurveyPromptTests
	{
		[Test]
		public static void Initialize()
		{
			var host = new TestHost();
			var component = host.AddComponent<SurveyPrompt>();
			var titleNode = component.Find("strong");
			Assert.That(titleNode.InnerText, Is.EqualTo(string.Empty));
		}

		[Test]
		public static void InitializeWithParameters()
		{
			var host = new TestHost();
			var component = host.AddComponent<SurveyPrompt>(
				new Dictionary<string, object> { { "Title", "My title" } } );

			//This shouldn't be set outside of the component
			//component.Instance.Title = "My title";

			var titleNode = component.Find("strong");
			Assert.That(titleNode.InnerText, Is.EqualTo("My title"));
		}
	}
}