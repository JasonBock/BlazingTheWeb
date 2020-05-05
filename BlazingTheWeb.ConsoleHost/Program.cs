using BlazingTheWeb.Core;
using Spackle.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace BlazingTheWeb.ConsoleHost
{
	public static class Program
	{
		static void Main()
		{
			var range = 1_000_000..2_000_000;

			Program.Find(range);
			Program.FindParallel(range);
		}

		private static void Find(Range range)
		{
			var stopwatch = Stopwatch.StartNew();
			var (value, sequenceLength) = Program.FindLongestSequence(range);
			stopwatch.Stop();

			Console.Out.WriteLine(nameof(Find));
			Console.Out.WriteLine($"Longest sequence in range {range} is {sequenceLength} for {value}");
			Console.Out.WriteLine($"Elapsed time is {stopwatch.Elapsed}");
		}
		
		private static void FindParallel(Range range)
		{
			var stopwatch = Stopwatch.StartNew();
			var tasks = new List<Task<(BigInteger value, int sequenceLength)>>();
			var ranges = range.Partition(Environment.ProcessorCount);

			for(var i = 0; i < Environment.ProcessorCount; i++)
			{
				var r = ranges[i];
				tasks.Add(Task.Run(() => FindLongestSequence(r)));
			}

			Task.WaitAll(tasks.ToArray());
			(BigInteger value, int sequenceLength) result = (BigInteger.Zero, 0);

			foreach (var task in tasks)
			{
				var taskResult = task.Result;

				if (taskResult.sequenceLength > result.sequenceLength)
				{
					result = taskResult;
				}
			}

			stopwatch.Stop();

			Console.Out.WriteLine(nameof(FindParallel));
			Console.Out.WriteLine($"Longest sequence in range {range} is {result.sequenceLength} for {result.value}");
			Console.Out.WriteLine($"Elapsed time is {stopwatch.Elapsed}");
		}

		private static (BigInteger value, int sequenceLength) FindLongestSequence(Range range)
		{
			(BigInteger value, int sequenceLength) result = (BigInteger.Zero, 0);

			for (var i = range.Start.Value; i < range.End.Value; i++)
			{
				var sequence = new CollatzLength(i);

				if(sequence.Length > result.sequenceLength)
				{
					result = (i, sequence.Length);
				}
			}

			return result;
		}
	}
}