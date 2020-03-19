// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace ActiveResolver
{
    public static class InstanceIsUnique
    {
        public static Func<T> PerProcess<T>(Func<T> f)
        {
            var cache = new ConcurrentDictionary<Type, T>();

            return () => cache.GetOrAdd(typeof(T), v => f());
        }

        public static Func<T> PerThread<T>(Func<T> f)
        {
            var cache = new ThreadLocal<T>(f);

            return () => cache.Value;
        }

        public static Func<T> PerHttpRequest<T>(IServiceProvider host, Func<T> f)
        {
            return () =>
            {
                var accessor = host.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
                if (accessor?.HttpContext == null)
                    return f();

                var cache = accessor.HttpContext.Items;
                var cacheKey = f.ToString();
                if (cache.TryGetValue(cacheKey, out var item))
                    return (T) item;

                item = f(); // need it
                cache.Add(cacheKey, item);
                if (item is IDisposable disposable)
                    accessor.HttpContext.Response.RegisterForDispose(disposable);

                return (T) item;
            };
        }
    }
}