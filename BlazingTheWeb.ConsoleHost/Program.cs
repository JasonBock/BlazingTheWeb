using BlazingTheWeb.Core;
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

			Find(range);
			FindParallel(range);
		}

		private static void Find(Range range)
		{
			var stopwatch = Stopwatch.StartNew();
			var result = Program.FindLongestSequence(range);
			stopwatch.Stop();

			Console.Out.WriteLine(nameof(Find));
			Console.Out.WriteLine($"Longest sequence in range {range} is {result.sequenceLength} for {result.value}");
			Console.Out.WriteLine($"Elapsed time is {stopwatch.Elapsed}");
		}

		// https://softwareengineering.stackexchange.com/questions/187680/algorithm-for-dividing-a-range-into-ranges-and-then-finding-which-range-a-number
		private static List<Range> DivideRange(Range range, int numberOfRanges)
		{
			var highestLength = (range.End.Value - range.Start.Value + 1) / numberOfRanges;
			var bucketSizes = new int[numberOfRanges];
			Array.Fill(bucketSizes, highestLength);

			var surplus = (range.End.Value - range.Start.Value + 1) % numberOfRanges;
			var surplusIndex = 0;

			while(surplus > 0)
			{
				bucketSizes[surplusIndex]++;
				surplusIndex = (surplusIndex++) % numberOfRanges;
				surplus--;
			}

			var ranges = new List<Range>();

			var k = range.Start.Value;

			for (var i = 0; i < numberOfRanges; i++)
			{
				ranges.Add(new Range(k, k + bucketSizes[i] - 1));
				k += bucketSizes[i];
			}

			return ranges;
		}

		private static void FindParallel(Range range)
		{
			var stopwatch = Stopwatch.StartNew();
			var tasks = new List<Task<(BigInteger value, int sequenceLength)>>();
			var ranges = DivideRange(range, Environment.ProcessorCount);

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
				var sequence = new CollatzSequence(i);

				if(sequence.Sequence.Length > result.sequenceLength)
				{
					result = (i, sequence.Sequence.Length);
				}
			}

			return result;
		}
	}
}
