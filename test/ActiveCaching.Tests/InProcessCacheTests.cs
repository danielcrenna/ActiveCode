using System;
using ActiveCaching.Configuration;

namespace ActiveCaching.Tests
{
	public class InProcessCacheTests : CacheTestsBase
	{
		public InProcessCacheTests()
		{
			Cache = new InProcessCache(
				Microsoft.Extensions.Options.Options.Create(
					new CacheOptions()), () => DateTimeOffset.Now);
		}
	}
}
