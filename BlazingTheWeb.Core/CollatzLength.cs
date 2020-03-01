using System;
using System.Numerics;

namespace BlazingTheWeb.Core
{
   public sealed class CollatzLength
	{
		public CollatzLength(BigInteger start)
		{
			if (start <= BigInteger.One)
			{
				throw new ArgumentException("Must provide a starting value greater than one.", nameof(start));
			}

			this.Start = start;
			this.Length = 1;

			while (start > 1)
			{
				start = start % 2 == 0 ?
					start / 2 : ((3 * start) + 1) / 2;
				this.Length++;
			}
		}

		public int Length { get; }
		public BigInteger Start { get; }
   }
}