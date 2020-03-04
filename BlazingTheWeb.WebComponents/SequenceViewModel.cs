using BlazingTheWeb.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace BlazingTheWeb.WebComponents
{
	public sealed class SequenceViewModel
		: IDisposable
	{
		private readonly IJSRuntime runtime;
		private DotNetObjectReference<SequenceViewModel> reference;

		public SequenceViewModel(IJSRuntime runtime) =>
			this.runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));

		public async Task CreateSequenceAsync()
		{
			if (BigInteger.TryParse(this.Value, out var value))
			{
				try
				{
					var sequence = new CollatzSequence(value);
					this.CurrentSequence = string.Join(", ", sequence.Sequence);
					this.Sequence = sequence.Sequence.Select(_ => (object)(int)_).ToList();
					this.Labels = Enumerable.Range(1, sequence.Sequence.Length).Select(_ => _.ToString()).ToArray();
					this.Changed?.Invoke(this, EventArgs.Empty);
					await this.runtime.InvokeAsync<object>(
						Constants.ChartsMethod, this.ChartReference,
						sequence.Sequence.Select(_ => (int)_).ToArray(), this.Labels);
				}
				catch (ArgumentException)
				{
					this.CurrentSequence = $"The value, {value}, is incorrect.";
					this.Sequence = default;
					this.Labels = Array.Empty<string>();
					await this.runtime.InvokeAsync<object>(
						Constants.ChartsMethod, this.ChartReference, 
						Array.Empty<int>(), Array.Empty<string>());
					this.Changed?.Invoke(this, EventArgs.Empty);
				}
			}
			else
			{
				this.CurrentSequence = $"{this.Value} is not a valid integer.";
				this.Sequence = default;
				this.Labels = Array.Empty<string>();
				await this.runtime.InvokeAsync<object>(
					Constants.ChartsMethod, this.ChartReference, 
					Array.Empty<int>(), Array.Empty<string>());
				this.Changed?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);

			if (this.reference is { })
			{
				this.reference.Dispose();
			}
		}

		public ElementReference ChartReference { get; set; }
		public string CurrentSequence { get; private set; }
		public string[] Labels { get; private set; }
		public List<object> Sequence { get; private set; }
		public string Value { get; set; }

		public event EventHandler Changed;
	}
}