﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NUnit.Framework;
using Rocks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingTheWeb.WebComponents.Tests
{
	public static class SequenceViewModelTests
	{
		[Test]
		public static void Create()
		{
			var viewModel = new SequenceViewModel(Rock.Make<IJSRuntime>());
			Assert.That(viewModel.Accuracy, Is.EqualTo(default(double)));
			Assert.That(viewModel.BingMainUrl, Is.Null);
			Assert.That(viewModel.BingLargeMapUrl, Is.Null);
			Assert.That(viewModel.BingDirectionsUrl, Is.Null);
			Assert.That(viewModel.ChartReference, Is.EqualTo(default(ElementReference)));
			Assert.That(viewModel.CurrentSequence, Is.Null);
			Assert.That(viewModel.Labels, Is.Null);
			Assert.That(viewModel.Latitude, Is.EqualTo(default(double)));
			Assert.That(viewModel.Longitude, Is.EqualTo(default(double)));
			Assert.That(viewModel.Sequence, Is.Null);
			Assert.That(viewModel.Value, Is.Null);
		}

		[Test]
		public static void CreateWithNullRuntime() =>
			Assert.That(() => new SequenceViewModel(null), Throws.TypeOf<ArgumentNullException>());

		[Test]
		public static async Task GetGeolocationAsync()
		{
			var runtime = Rock.Create<IJSRuntime>();
			runtime.Handle(_ => _.InvokeAsync<object>(Constants.GeolocationMethod, Arg.IsAny<object[]>()))
				.Returns(new ValueTask<object>());

			var viewModel = new SequenceViewModel(runtime.Make());

			await viewModel.GetGeolocationAsync();

			runtime.Verify();
		}

		[Test]
		public static void Change()
		{
			var wasChangedRaised = false;
			var viewModel = new SequenceViewModel(Rock.Make<IJSRuntime>());
			viewModel.Changed += (s, e) => wasChangedRaised = true;
			viewModel.Change(1, 2, 3);

			Assert.That(viewModel.BingDirectionsUrl, Is.Not.Null);
			Assert.That(viewModel.BingLargeMapUrl, Is.Not.Null);
			Assert.That(viewModel.BingMainUrl, Is.Not.Null);
			Assert.That(wasChangedRaised, Is.True);
		}

		[Test]
		public static async Task CreateSequence()
		{
			var runtime = Rock.Create<IJSRuntime>();
			runtime.Handle(_ => _.InvokeAsync<object>(Constants.ChartsMethod, Arg.IsAny<object[]>()))
				.Returns(new ValueTask<object>());

			var wasChangedRaised = false;

			var viewModel = new SequenceViewModel(runtime.Make())
			{
				Value = "5"
			};
			viewModel.Changed += (s, e) => wasChangedRaised = true;

			await viewModel.CreateSequence();
			Assert.That(viewModel.CurrentSequence, Is.EqualTo("5, 8, 4, 2, 1"));
			Assert.That(viewModel.Sequence, Is.EquivalentTo(new object[] { 5, 8, 4, 2, 1 }));
			Assert.That(viewModel.Labels, Is.EquivalentTo(new[] { "1", "2", "3", "4", "5" }));
			Assert.That(wasChangedRaised, Is.True);

			runtime.Verify();
		}

		[Test]
		public static async Task CreateSequenceWhenValueIsIncorrect()
		{
			var runtime = Rock.Create<IJSRuntime>();
			runtime.Handle(_ => _.InvokeAsync<object>(Constants.ChartsMethod, Arg.IsAny<object[]>()))
				.Returns(new ValueTask<object>());

			var wasChangedRaised = false;

			var viewModel = new SequenceViewModel(runtime.Make())
			{
				Value = "-5"
			};
			viewModel.Changed += (s, e) => wasChangedRaised = true;

			await viewModel.CreateSequence();
			Assert.That(viewModel.CurrentSequence, Is.EqualTo("The value, -5, is incorrect."));
			Assert.That(viewModel.Sequence, Is.EqualTo(default(List<object>)));
			Assert.That(viewModel.Labels, Is.EqualTo(Array.Empty<string>()));
			Assert.That(wasChangedRaised, Is.True);

			runtime.Verify();
		}

		[Test]
		public static async Task CreateSequenceWhenValueIsNotABigInteger()
		{
			var runtime = Rock.Create<IJSRuntime>();
			runtime.Handle(_ => _.InvokeAsync<object>(Constants.ChartsMethod, Arg.IsAny<object[]>()))
				.Returns(new ValueTask<object>());

			var wasChangedRaised = false;

			var viewModel = new SequenceViewModel(runtime.Make())
			{
				Value = "quux"
			};
			viewModel.Changed += (s, e) => wasChangedRaised = true;

			await viewModel.CreateSequence();
			Assert.That(viewModel.CurrentSequence, Is.EqualTo("quux is not a valid integer."));
			Assert.That(viewModel.Sequence, Is.EqualTo(default(List<object>)));
			Assert.That(viewModel.Labels, Is.EqualTo(Array.Empty<string>()));
			Assert.That(wasChangedRaised, Is.True);

			runtime.Verify();
		}
	}
}