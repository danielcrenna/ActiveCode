using System;
using Microsoft.Extensions.Internal;

namespace ActiveCaching.Tests
{
	public class DistributedCacheTests : CacheTestsBase
	{
		public DistributedCacheTests()
		{
			Cache = new DistributedCache(
				new JsonCacheSerializer(),
				new JsonCacheSerializer(),
				new SystemClock(),
				() => DateTimeOffset.Now);
		}
	}
}