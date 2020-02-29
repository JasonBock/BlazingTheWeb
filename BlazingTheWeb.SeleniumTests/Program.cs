using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace BlazingTheWeb.SeleniumTests
{
	public static class Program
	{
		static void Main()
		{
			using var driver = new ChromeDriver();
			driver.Navigate().GoToUrl("https://localhost:44383/sequence/55");

			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
			var firstResult = wait.Until(driver => driver.FindElement(By.Id("currentSequence")) is { });

			var resultElement = driver.FindElement(By.Id("currentSequence"));
			Console.Out.WriteLine(resultElement.Text);
		}
	}
}
