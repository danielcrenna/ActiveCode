// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ActiveCaching.Configuration;
using ActiveCaching.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ActiveCaching
{
	public class InProcessCache : InProcessCacheManager, ICache
	{
		public InProcessCache(IOptions<CacheOptions> cacheOptions, Func<DateTimeOffset> timestamps) : base(
			cacheOptions, timestamps)
		{
		}

		public void Remove(string key)
		{
			Cache.Remove(key);
		}

		private static bool Try(Action closure)
		{
			try
			{
				closure.Invoke();
				return true;
			}
			catch
			{
				return false;
			}
		}

		private bool RemoveByKeyThen(string key, Func<bool> operation)
		{
			try
			{
				Cache.Remove(key);
				return operation();
			}
			catch
			{
				return false;
			}
		}

		private bool EnsureKeyExistsThen(string key, Func<bool> operation)
		{
			try
			{
				return Cache.Get(key) != null && operation();
			}
			catch
			{
				return false;
			}
		}

		private static MemoryCacheEntryOptions CreateEntry(DateTimeOffset? absoluteExpiration = null,
			TimeSpan? slidingExpiration = null, long? size = null, ICacheDependency dependency = null)
		{
			var policy = new MemoryCacheEntryOptions
			{
				Priority = CacheItemPriority.Normal,
				AbsoluteExpiration = absoluteExpiration,
				SlidingExpiration = slidingExpiration,
				Size = size
			};

			policy.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration {EvictionCallback = OnEviction});

			if (dependency != null)
				policy.AddExpirationToken(dependency.GetChangeToken());

			return policy;
		}

		private static void OnEviction(object key, object value, EvictionReason reason, object state)
		{
			switch (reason)
			{
				case EvictionReason.Capacity:
				{
					// ... collect stats on memory pressure for health checks ...

					break;
				}
				case EvictionReason.None:
				case EvictionReason.Removed:
				case EvictionReason.Replaced:
				case EvictionReason.Expired:
				case EvictionReason.TokenExpired:
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(reason), reason, null);
			}
		}

		#region Set

		public bool Set(string key, object value)
		{
			return Try(() => Cache.Set(key, value, CreateEntry()));
		}

		public bool Set(string key, object value, DateTimeOffset absoluteExpiration)
		{
			return Try(() => Cache.Set(key, value, CreateEntry(absoluteExpiration)));
		}

		public bool Set(string key, object value, TimeSpan slidingExpiration)
		{
			return Try(() => Cache.Set(key, value, CreateEntry(slidingExpiration: slidingExpiration)));
		}

		public bool Set(string key, object value, ICacheDependency dependency)
		{
			return Try(() => Cache.Set(key, value, CreateEntry(dependency: dependency)));
		}

		public bool Set(string key, object value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			return Try(() => Cache.Set(key, value, CreateEntry(absoluteExpiration, dependency: dependency)));
		}

		public bool Set(string key, object value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			return Try(() =>
				Cache.Set(key, value, CreateEntry(slidingExpiration: slidingExpiration, dependency: dependency)));
		}

		public bool Set<T>(string key, T value)
		{
			return Try(() => Cache.Set(key, value, CreateEntry()));
		}

		public bool Set<T>(string key, T value, DateTimeOffset absoluteExpiration)
		{
			return Try(() => Cache.Set(key, value, CreateEntry(absoluteExpiration)));
		}

		public bool Set<T>(string key, T value, TimeSpan slidingExpiration)
		{
			return Try(() => Cache.Set(key, value, CreateEntry(slidingExpiration: slidingExpiration)));
		}

		public bool Set<T>(string key, T value, ICacheDependency dependency)
		{
			return Try(() => Cache.Set(key, value, CreateEntry(dependency: dependency)));
		}

		public bool Set<T>(string key, T value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			return Try(() => Cache.Set(key, value, CreateEntry(absoluteExpiration, dependency: dependency)));
		}

		public bool Set<T>(string key, T value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			return Try(() =>
				Cache.Set(key, value, CreateEntry(slidingExpiration: slidingExpiration, dependency: dependency)));
		}

		#endregion

		#region Add

		public bool Add(string key, object value)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry());
			return true;
		}

		public bool Add(string key, object value, DateTimeOffset absoluteExpiration)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry(absoluteExpiration));
			return true;
		}

		public bool Add(string key, object value, TimeSpan slidingExpiration)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry(slidingExpiration: slidingExpiration));
			return true;
		}

		public bool Add(string key, object value, ICacheDependency dependency)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry(dependency: dependency));
			return true;
		}

		public bool Add(string key, object value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry(absoluteExpiration, dependency: dependency));
			return true;
		}

		public bool Add(string key, object value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry(slidingExpiration: slidingExpiration));
			return true;
		}

		public bool Add<T>(string key, T value)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry());
			return true;
		}

		public bool Add<T>(string key, T value, DateTimeOffset absoluteExpiration)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry(absoluteExpiration));
			return true;
		}

		public bool Add<T>(string key, T value, TimeSpan slidingExpiration)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry(slidingExpiration: slidingExpiration));
			return true;
		}

		public bool Add<T>(string key, T value, ICacheDependency dependency)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry(dependency: dependency));
			return true;
		}

		public bool Add<T>(string key, T value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry(absoluteExpiration, dependency: dependency));
			return true;
		}

		public bool Add<T>(string key, T value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			if (Cache.Get(key) != null)
				return false;
			Cache.Set(key, value, CreateEntry(slidingExpiration: slidingExpiration));
			return true;
		}

		#endregion

		#region Replace

		public bool Replace(string key, object value)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value)));
		}

		public bool Replace(string key, object value, DateTimeOffset absoluteExpiration)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, absoluteExpiration)));
		}

		public bool Replace(string key, object value, TimeSpan slidingExpiration)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, slidingExpiration)));
		}

		public bool Replace(string key, object value, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, dependency)));
		}

		public bool Replace(string key, object value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key,
				() => RemoveByKeyThen(key, () => Add(key, value, absoluteExpiration, dependency)));
		}

		public bool Replace(string key, object value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key,
				() => RemoveByKeyThen(key, () => Add(key, value, slidingExpiration, dependency)));
		}

		public bool Replace<T>(string key, T value)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value)));
		}

		public bool Replace<T>(string key, T value, DateTimeOffset absoluteExpiration)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, absoluteExpiration)));
		}

		public bool Replace<T>(string key, T value, TimeSpan slidingExpiration)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, slidingExpiration)));
		}

		public bool Replace<T>(string key, T value, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, dependency)));
		}

		public bool Replace<T>(string key, T value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key,
				() => RemoveByKeyThen(key, () => Add(key, value, absoluteExpiration, dependency)));
		}

		public bool Replace<T>(string key, T value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key,
				() => RemoveByKeyThen(key, () => Add(key, value, slidingExpiration, dependency)));
		}

		#endregion

		#region Get

		public object Get(string key, TimeSpan? timeout = null)
		{
			return GetOrAdd(key, null, timeout);
		}

		public object GetOrAdd(string key, Func<object> add = null, TimeSpan? timeout = null)
		{
			var item = Cache.Get(key);
			if (item != null)
				return item;

			if (add == null)
				return null;
			
			var itemToAdd = add();
			if (itemToAdd != null)
				this.Add(key, itemToAdd);
			return itemToAdd;
		}

		public object GetOrAdd(string key, object add = null, TimeSpan? timeout = null)
		{
			return GetOrAdd(key, () => add, timeout);
		}

		public T Get<T>(string key, TimeSpan? timeout = null)
		{
			return GetOrAdd<T>(key, null, timeout);
		}

		public T GetOrAdd<T>(string key, Func<T> add = null, TimeSpan? timeout = null)
		{
			var item = Cache.Get(key) is T typed ? typed : default;
			if (item != null)
				return item;

			if (add == null)
				return default;

			var itemToAdd = add();
			if (itemToAdd != null)
				Add(key, itemToAdd);

			return itemToAdd;
		}

		public T GetOrAdd<T>(string key, T add = default, TimeSpan? timeout = null)
		{
			return GetOrAdd(key, () => add, timeout);
		}

		#endregion
	}
}