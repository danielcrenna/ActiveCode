// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using ActiveCaching.Configuration;
using ActiveCaching.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ActiveCaching
{
	public abstract class InProcessCacheManager : ICacheManager
	{
		private readonly MemoryCacheOptions _memoryCacheOptions;

		protected readonly IMemoryCache Cache;
		protected readonly IOptions<CacheOptions> CacheOptions;

		protected InProcessCacheManager(IOptions<CacheOptions> cacheOptions, Func<DateTimeOffset> timestamps)
		{
			_memoryCacheOptions = new MemoryCacheOptions
			{
				CompactionPercentage = 0.05,
				ExpirationScanFrequency = TimeSpan.FromMinutes(1.0),
				SizeLimit = cacheOptions.Value.MaxSizeBytes,
				Clock = new DelegatedSystemClock(timestamps)
			};
			Cache = new MemoryCache(_memoryCacheOptions);
			CacheOptions = cacheOptions;
		}

		#region ICacheManager

		public int KeyCount
		{
			get
			{
				if (!(Cache is MemoryCache memory))
					return 0;
				var getCount = typeof(MemoryCache).GetProperty(nameof(MemoryCache.Count),
					BindingFlags.Instance | BindingFlags.Public);
				return (int) (getCount?.GetValue(memory) ?? 0);
			}
		}

		public long SizeBytes
		{
			get
			{
				if (!(Cache is MemoryCache memory))
					return 0L;
				var getSize = typeof(MemoryCache).GetProperty("Size", BindingFlags.Instance | BindingFlags.NonPublic);
				return (long) (getSize?.GetValue(memory) ?? 0L);
			}
			set => SizeLimitBytes = value;
		}

		public long? SizeLimitBytes
		{
			get => _memoryCacheOptions.SizeLimit;
			set => _memoryCacheOptions.SizeLimit = value;
		}

		public void Clear()
		{
			if (Cache is MemoryCache memory)
				memory.Compact(1);
		}

		#endregion
	}
}