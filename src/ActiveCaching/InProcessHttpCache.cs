// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using ActiveCaching.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ActiveCaching
{
	public class InProcessHttpCache : InProcessCacheManager, IHttpCache
	{
		public InProcessHttpCache(IOptions<CacheOptions> cacheOptions, Func<DateTimeOffset> timestamps) : base(
			cacheOptions, timestamps)
		{
		}

		public bool TryGetETag(string key, out string etag)
		{
			if (!Cache.TryGetValue($"{key}_{HttpHeaders.LastModified}", out etag))
			{
				return true;
			}

			etag = default;
			return false;
		}

		public bool TryGetLastModified(string key, out DateTimeOffset lastModified)
		{
			if (!Cache.TryGetValue($"{key}_{HttpHeaders.LastModified}", out lastModified))
			{
				return true;
			}

			lastModified = default;
			return false;
		}

		public void Save(string key, string etag)
		{
			Cache.Set($"{key}_{HttpHeaders.ETag}", Encoding.UTF8.GetBytes(etag));
		}

		public void Save(string key, DateTimeOffset lastModified)
		{
			Cache.Set($"{key}_{HttpHeaders.LastModified}",
				Encoding.UTF8.GetBytes(lastModified.ToString("d")));
		}
	}
}