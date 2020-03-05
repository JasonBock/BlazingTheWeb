using BlazingTheWeb.Core.Extensions;
using NUnit.Framework;

namespace BlazingTheWeb.Core.Tests.Extensions
{
	public static class RangeExtensionsTests
	{
		[Test]
		public static void Partition()
		{
			var range = 50..100;
			var ranges = range.Partition(4);

			Assert.Multiple(() =>
			{
				Assert.That(ranges.Length, Is.EqualTo(4));
				Assert.That(ranges[0], Is.EqualTo(50..64));
				Assert.That(ranges[1], Is.EqualTo(65..76));
				Assert.That(ranges[2], Is.EqualTo(77..88));
				Assert.That(ranges[3], Is.EqualTo(89..100));
			});
		}
	}
}