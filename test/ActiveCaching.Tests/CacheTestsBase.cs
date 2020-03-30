using System.Threading;
using Xunit;

namespace ActiveCaching.Tests
{
	public abstract class CacheTestsBase
	{
		protected ICache Cache;

		[Fact]
		public virtual void Can_add_and_skip()
		{
			Cache.Add("key", "value");
			Cache.Add("key", "value2");
			Assert.Equal("value", Cache.Get<string>("key")); // Cache did not skip value when adding
		}

		[Fact]
		public virtual void Can_cache_with_absolute_expiry()
		{
			Cache.Set("key", "value", 1.Seconds().FromNow());
			Thread.Sleep(1.Seconds());
			Assert.Null(Cache.Get<string>("key")); // Cache didn't expire in time
		}

		[Fact]
		public virtual void Can_cache_with_sliding_expiry()
		{
			Cache.Set("key", "value", 1.Seconds());
			Thread.Sleep(1.Seconds());
			Assert.Null(Cache.Get<string>("key")); // Cache didn't expire in time
		}

		[Fact]
		public virtual void Can_set_and_override()
		{
			Cache.Set("key", "value");
			Cache.Set("key", "value2");
			Assert.Equal("value2", Cache.Get<string>("key")); // Cache did not override value when setting
		}
	}
}