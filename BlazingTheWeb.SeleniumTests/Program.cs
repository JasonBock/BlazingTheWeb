using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

using var driver = new ChromeDriver();
driver.Navigate().GoToUrl("http://localhost:50455/sequence/55");

var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
var firstResult = wait.Until(driver => driver.FindElement(By.Id("currentSequence")) is { });

var resultElement = driver.FindElement(By.Id("currentSequence"));
Console.Out.WriteLine(resultElement.Text);