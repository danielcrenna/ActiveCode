// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json;
using ActiveCaching.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ActiveCaching
{
	public static class Add
	{
		public static IServiceCollection AddInProcessCache(this IServiceCollection services,
			Action<CacheOptions> configureAction = null)
		{
			services.AddOptions();

			if (configureAction != null)
				services.Configure(configureAction);

			services.TryAdd(ServiceDescriptor.Singleton<IMemoryCache, MemoryCache>());
			services.TryAdd(ServiceDescriptor.Singleton<ICache, InProcessCache>());

			return services;
		}

		public static IServiceCollection AddDistributedCache(this IServiceCollection services,
			Action<CacheOptions> configureAction = null)
		{
			services.AddOptions();

			if (configureAction != null)
				services.Configure(configureAction);

			services.TryAdd(ServiceDescriptor.Singleton<IDistributedCache, MemoryDistributedCache>());
			services.TryAdd(ServiceDescriptor.Singleton<ICache, DistributedCache>());

			return services;
		}

		public static IServiceCollection AddHttpCaching(this IServiceCollection services)
		{
			services.TryAddSingleton<IHttpCache, InProcessHttpCache>();
			services.TryAddSingleton<IETagGenerator, WeakETagGenerator>();
			services.AddScoped(r => new HttpCacheFilterAttribute(r.GetRequiredService<IETagGenerator>(),
				r.GetRequiredService<IHttpCache>(), r.GetRequiredService<JsonSerializerOptions>()));
			return services;
		}
	}
}