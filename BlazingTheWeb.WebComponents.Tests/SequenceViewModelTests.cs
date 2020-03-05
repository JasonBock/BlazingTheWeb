using Microsoft.AspNetCore.Components;
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
			Assert.That(viewModel.ChartReference, Is.EqualTo(default(ElementReference)), nameof(viewModel.Changed));
			Assert.That(viewModel.CurrentSequence, Is.EqualTo(string.Empty), nameof(viewModel.CurrentSequence));
			Assert.That(viewModel.Labels, Is.EqualTo(Array.Empty<string>()), nameof(viewModel.Labels));
			Assert.That(viewModel.Sequence, Is.EqualTo(Array.Empty<int>()), nameof(viewModel.Sequence));
			Assert.That(viewModel.Value, Is.Null, nameof(viewModel.Value));
		}

		[Test]
		public static void CreateWithNullRuntime() =>
			Assert.That(() => new SequenceViewModel(null), Throws.TypeOf<ArgumentNullException>());

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

			await viewModel.CreateSequenceAsync();
			Assert.That(viewModel.CurrentSequence, Is.EqualTo("5, 8, 4, 2, 1"), nameof(viewModel.CurrentSequence));
			Assert.That(viewModel.Sequence, Is.EquivalentTo(new int[] { 5, 8, 4, 2, 1 }), nameof(viewModel.Sequence));
			Assert.That(viewModel.Labels, Is.EquivalentTo(new[] { "1", "2", "3", "4", "5" }), nameof(viewModel.Labels));
			Assert.That(wasChangedRaised, Is.True, nameof(wasChangedRaised));

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

			await viewModel.CreateSequenceAsync();
			Assert.That(viewModel.CurrentSequence, Is.EqualTo("The value, -5, is incorrect."), nameof(viewModel.CurrentSequence));
			Assert.That(viewModel.Sequence, Is.EqualTo(Array.Empty<int>()), nameof(viewModel.Sequence));
			Assert.That(viewModel.Labels, Is.EqualTo(Array.Empty<string>()), nameof(viewModel.Labels));
			Assert.That(wasChangedRaised, Is.True, nameof(wasChangedRaised));

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

			await viewModel.CreateSequenceAsync();
			Assert.That(viewModel.CurrentSequence, Is.EqualTo("quux is not a valid integer."), nameof(viewModel.CurrentSequence));
			Assert.That(viewModel.Sequence, Is.EqualTo(Array.Empty<int>()), nameof(viewModel.Sequence));
			Assert.That(viewModel.Labels, Is.EqualTo(Array.Empty<string>()), nameof(viewModel.Labels));
			Assert.That(wasChangedRaised, Is.True, nameof(wasChangedRaised));

			runtime.Verify();
		}
	}
}