using BlazingTheWeb.Core;
using CollatzGrpc;
using Google.Protobuf;
using Grpc.Core;
using Spackle.Extensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

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
				var sequence = new CollatzLength(i);

				if (sequence.Length > result.sequenceLength)
				{
					result = (i, sequence.Length);
				}
			}

			return result;
		}

		public override Task<CollatzResponse> FindLongestSequence(CollatzRequest request, ServerCallContext context)
		{
			var tasks = new List<Task<(BigInteger value, int sequenceLength)>>();
			var range = request.Start..request.End;
			var ranges = range.Partition(Environment.ProcessorCount);

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