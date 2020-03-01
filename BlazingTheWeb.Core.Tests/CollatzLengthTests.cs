using NUnit.Framework;
using System;
using System.Numerics;

namespace BlazingTheWeb.Core.Tests
{
	public static class CollatzLengthTests
	{
		[Test]
		public static void GenerateLength()
		{
			var start = new BigInteger(5);
			var sequence = new CollatzLength(start);

			Assert.That(sequence.Start, Is.EqualTo(start), nameof(start));
			Assert.That(sequence.Length, Is.EqualTo(5), nameof(sequence.Length));
		}

		[Test]
		public static void GenerateLengthWithIncorrectStartValue() =>
			Assert.That(() => new CollatzSequence(BigInteger.One), Throws.TypeOf<ArgumentException>());
	}
}