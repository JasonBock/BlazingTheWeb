﻿using NUnit.Framework;
using System;
using System.Numerics;

namespace BlazingTheWeb.Core.Tests
{
	public static class CollatzSequenceTests
	{
		[Test]
		public static void GenerateSequence()
		{
			var start = new BigInteger(5);
			var sequence = new CollatzSequence(start);

			Assert.Multiple(() =>
			{
				Assert.That(sequence.Start, Is.EqualTo(start), nameof(start));
				Assert.That(sequence.Sequence.Length, Is.EqualTo(5), nameof(sequence.Sequence.Length));
				Assert.That(sequence.Sequence[0], Is.EqualTo(start), "0");
				Assert.That(sequence.Sequence[1], Is.EqualTo(new BigInteger(8)), "1");
				Assert.That(sequence.Sequence[2], Is.EqualTo(new BigInteger(4)), "2");
				Assert.That(sequence.Sequence[3], Is.EqualTo(new BigInteger(2)), "3");
				Assert.That(sequence.Sequence[4], Is.EqualTo(new BigInteger(1)), "4");
			});
		}

		[Test]
		public static void GenerateSequenceWithIncorrectStartValue() =>
			Assert.That(() => new CollatzSequence(BigInteger.One), Throws.TypeOf<ArgumentException>());
	}
}