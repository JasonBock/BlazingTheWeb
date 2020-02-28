using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using BlazingTheWeb.Core;
using CollatzGrpc;
using Google.Protobuf;
using Grpc.Core;

namespace BlazingTheWeb.ServiceHost
{
	public sealed class CollatzService
		: Collatz.CollatzBase
	{
		private static (BigInteger value, int sequenceLength) FindLongestSequence(Range range)
		{
			(BigInteger value, int sequenceLength) result = (BigInteger.Zero, 0);

			for (var i = range.Start.Value; i < range.End.Value; i++)
			{
				var sequence = new CollatzSequence(i);

				if (sequence.Sequence.Length > result.sequenceLength)
				{
					result = (i, sequence.Sequence.Length);
				}
			}

			return result;
		}

		// https://softwareengineering.stackexchange.com/questions/187680/algorithm-for-dividing-a-range-into-ranges-and-then-finding-which-range-a-number
		private static List<Range> DivideRange(Range range, int numberOfRanges)
		{
			var highestLength = (range.End.Value - range.Start.Value + 1) / numberOfRanges;
			var bucketSizes = new int[numberOfRanges];
			Array.Fill(bucketSizes, highestLength);

			var surplus = (range.End.Value - range.Start.Value + 1) % numberOfRanges;
			var surplusIndex = 0;

			while (surplus > 0)
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

		public override Task<CollatzResponse> FindLongestSequence(CollatzRequest request, ServerCallContext context)
		{
			var tasks = new List<Task<(BigInteger value, int sequenceLength)>>();
			var range = request.Start..request.End;
			var ranges = DivideRange(range, Environment.ProcessorCount);

			for (var i = 0; i < Environment.ProcessorCount; i++)
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

			var response = new CollatzResponse()
			{
				Value = ByteString.CopyFrom(result.value.ToByteArray()),
				Length = result.sequenceLength
			};

			return Task.FromResult(response);
		}
	}
}
